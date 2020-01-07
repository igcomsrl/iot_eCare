app.controller('registry-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'registryService',
        '$q',
        '$uibModal',
        'NgTableParams',
        'ngTableService',
        'utilsService',
        '$rootScope',
        function ($scope, $log, toastr, responseDispatcherService, registryService, $q, $uibModal, NgTableParams, ngTableService, utilsService, $rootScope) {
            //#region Model
            $scope.model = {
                firstname: null,
                surname: null,
                sex: null,
                sexList: [],
                registryType: null,
                registryList: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            $scope.$watchGroup(['model.firstname', 'model.surname', 'model.sex', 'model.registryType'],
                function (newValues, oldValues, scope) {
                    $scope.model.registryList = [];
                });

            //#region Behaviors

            $scope.clearFilter = function () {
                $scope.model.firstname = null;
                $scope.model.surname = null;
                $scope.model.sex = null;
                $scope.model.registryType = null;
                $scope.model.isFetchBusy = false;
                $scope.model.registryList = [];
            }

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'firstname': $scope.model.firstname,
                    'surname': $scope.model.surname,
                    'sex': $scope.model.sex,
                    'registryType': $scope.model.registryType,
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                registryService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.registryList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = registryService.registryIndexModel(dto);
                            $scope.model.registryList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.registryList, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }

            $scope.openEdit = function (item) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/registry/edit.html',
                    controller: 'registry-editCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "registryId": function () {
                            return item ? item.id : null;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    $scope.registryListNgTable.reload();
                });
            }

            $scope.delete = function (item) {
                
                //Definisco la richiesta
                var request = {'id': item.id}

                //Eseguo la richiesta al server
                var promise = registryService.delete(request);

                //Valuto la risposta
                promise.then(function (response) {
                    //var index = $scope.model.registryList.indexOf(item);
                    //$scope.model.registryList.splice(index, 1);
                    $scope.registryListNgTable.reload();
                    toastr.success($rootScope.validationMessages.deleteSuccess);
                }, function (error) {                   
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Cancellazione entity fallita in registry-indexCtrl" + JSON.stringify(error));
                }).finally(function () {
                });
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                registryService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.model.registryTypeList = Enumerable.from(data.registryTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        $scope.model.sexList = Enumerable.from(data.sexList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }
            //#endregion

            $scope.registryListNgTable = new NgTableParams({
                page: 1,
                sorting: { "InsertDate": "asc" }
            },
                {
                    getData: function (params) {
                        var paginationIndexs = ngTableService.buildPaginatorIndexs(params);
                        return $scope.fetch(paginationIndexs.startRowIndex, paginationIndexs.maxRowIndex, paginationIndexs.orderByProperty, paginationIndexs.orderByType)
                            .then(function (data) {
                                params.total(data.count);
                                return data.models;
                            });
                    }
                });

            var constructor = function () {
                $scope.model.isCtrlReady = false;

                var essentialDataPromises = [];
                essentialDataPromises.push(fetchEssentialData());

                $q.all(essentialDataPromises).then(function () {
                    $scope.model.isCtrlReady = true;
                    //$scope.fetch();
                });
            };

            constructor();
        }]);