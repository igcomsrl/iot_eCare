app.controller('role-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'roleService',
        '$q',
        '$uibModal',
        'NgTableParams',
        'ngTableService',
        'utilsService',
        '$rootScope',
        function ($scope, $log, toastr, responseDispatcherService, roleService, $q, $uibModal, NgTableParams, ngTableService, utilsService, $rootScope) {
            //#region Model
            $scope.model = {
                name: null,
                description: null,
                roleList: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            $scope.$watchGroup(['model.name', 'model.descriptions'],
                function (newValues, oldValues, scope) {
                    $scope.model.roleList = [];
                });

            //#region Behaviors

            $scope.clearFilter = function () {
                $scope.model.name = null;
                $scope.model.description = null;
                $scope.model.isFetchBusy = false;
                $scope.model.roleList = [];
            }

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'name': $scope.model.name,
                    'description': $scope.model.description,
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                roleService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.roleList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = roleService.roleIndexModel(dto);
                            $scope.model.roleList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.roleList, "count": data.count });
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
                    templateUrl: 'App/templates/role/edit.html',
                    controller: 'role-editCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "roleId": function () {
                            return item ? item.id : null;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    $scope.roleListNgTable.reload();
                });
            }

            $scope.delete = function (item) {
                
                //Definisco la richiesta
                var request = {'id': item.id}

                //Eseguo la richiesta al server
                var promise = roleService.delete(request);

                //Valuto la risposta
                promise.then(function (response) {
                    //var index = $scope.model.roleList.indexOf(item);
                    //$scope.model.roleList.splice(index, 1);
                    $scope.roleListNgTable.reload();
                    toastr.success($rootScope.validationMessages.deleteSuccess);
                }, function (error) {                   
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Cancellazione entity fallita in role-indexCtrl" + JSON.stringify(error));
                }).finally(function () {
                });
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                roleService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

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

            $scope.roleListNgTable = new NgTableParams({
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
                //essentialDataPromises.push(fetchEssentialData());

                $q.all(essentialDataPromises).then(function () {
                    $scope.model.isCtrlReady = true;
                    //$scope.fetch();
                });
            };

            constructor();
        }]);