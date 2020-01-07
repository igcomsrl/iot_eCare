app.controller('device-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'deviceService',
        '$q',
        '$uibModal',
        'NgTableParams',
        'ngTableService',
        'utilsService',
        '$rootScope',
        function ($scope, $log, toastr, responseDispatcherService, deviceService, $q, $uibModal, NgTableParams, ngTableService, utilsService, $rootScope) {
            //#region Model
            $scope.model = {
                macaddress: null,
                name: null,
                isEnabled: null,
                deviceList: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            $scope.$watchGroup(['model.macaddress', 'model.name', 'model.isEnabled'],
                function (newValues, oldValues, scope) {
                    $scope.model.deviceList = [];
                });

            //#region Behaviors

            $scope.clearFilter = function () {
                $scope.model.macaddress = null;
                $scope.model.name = null;
                $scope.model.isFetchBusy = false;
                $scope.model.deviceList = [];
            }

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'macaddress': $scope.model.macaddress,
                    'name': $scope.model.name,
                    'isEnabled': '',
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                deviceService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.deviceList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = deviceService.deviceIndexModel(dto);
                            $scope.model.deviceList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.deviceList, "count": data.count });
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
                    templateUrl: 'App/templates/device/edit.html',
                    controller: 'device-editCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "deviceId": function () {
                            return item ? item.id : null;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    $scope.deviceListNgTable.reload();
                });
            }

            $scope.delete = function (item) {
                
                //Definisco la richiesta
                var request = {'id': item.id}

                //Eseguo la richiesta al server
                var promise = deviceService.delete(request);

                //Valuto la risposta
                promise.then(function (response) {
                    //var index = $scope.model.deviceList.indexOf(item);
                    //$scope.model.deviceList.splice(index, 1);
                    $scope.deviceListNgTable.reload();
                    toastr.success($rootScope.validationMessages.deleteSuccess);
                }, function (error) {                   
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Cancellazione entity fallita in device-indexCtrl" + JSON.stringify(error));
                }).finally(function () {
                });
            }

            //var fetchEssentialData = function () {
            //    var deferred = $q.defer();

            //    var request = {};

            //    deviceService.fetchEssentialData(request)
            //        .then(function (response) {
            //            var data = response ? response.data : {};

            //            //$scope.model.deviceTypeList = Enumerable.from(data.deviceTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
            //            //$scope.model.sexList = Enumerable.from(data.sexList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

            //            deferred.resolve(response);
            //        }, function (error) {
            //            $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
            //            deferred.reject(error);
            //        }).finally(function () {
            //            $scope.isCtrlReady = true;
            //        });

            //    return deferred.promise;
            //}
            //#endregion

            $scope.deviceListNgTable = new NgTableParams({
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
               // essentialDataPromises.push(fetchEssentialData());

                $q.all(essentialDataPromises).then(function () {
                    $scope.model.isCtrlReady = true;
                    //$scope.fetch();
                });
            };

            constructor();
        }]);