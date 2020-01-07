//app.controller('healthRisk-indexCtrl',
//    ['$scope',
//        '$log',
//        'toastr',
//        "responseDispatcherService",
//        'healthRiskService',
//        '$q',
//        '$uibModal',
//        'NgTableParams',
//        'ngTableService',
//        'utilsService',
//        '$rootScope',
//        function ($scope, $log, toastr, responseDispatcherService, healthRiskService, $q, $uibModal, NgTableParams, ngTableService, utilsService, $rootScope) {
//            //#region Model
//            $scope.model = {
//                type: null,
//                level: null,
//                typeList: [],
//                levelList: [],
//                healthRiskType: null,
//                healthRiskList: [],
//                isCtrlReady: false,
//                isFetchBusy: false
//            };
//            //#endregion

//            //$scope.$watchGroup(['model.firstname', 'model.surname', 'model.sex', 'model.healthRiskType'],
//            //    function (newValues, oldValues, scope) {
//            //        $scope.model.healthRiskList = [];
//            //    });

//            //#region Behaviors

//            //$scope.clearFilter = function () {
//            //    $scope.model.firstname = null;
//            //    $scope.model.surname = null;
//            //    $scope.model.sex = null;
//            //    $scope.model.healthRiskType = null;
//            //    $scope.model.isFetchBusy = false;
//            //    $scope.model.healthRiskList = [];
//            //}

//            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
//                $scope.model.isFetchBusy = true;

//                var deferred = $q.defer();

//                var request =
//                {
//                    'registryId': $scope.model.firstname,
//                    'surname': $scope.model.surname,
//                    'sex': $scope.model.sex,
//                    'healthRiskType': $scope.model.healthRiskType,
//                    "startRowIndex": startRowIndex,
//                    "maxRowIndex": maxRowIndex,
//                    "orderByProperty": orderByProperty || "",
//                    "orderByType": orderByType || ""
//                };

//                healthRiskService.fetch(request)
//                    .then(function (response) {
//                        var data = response.data;

//                        $scope.model.healthRiskList = [];
//                        angular.forEach(data.data, function (dto) {
//                            var model = healthRiskService.healthRiskIndexModel(dto);
//                            $scope.model.healthRiskList.push(model);
//                        });

//                        deferred.resolve({ "models": $scope.model.healthRiskList, "count": data.count });
//                    }, function (error) {
//                        //Loggo gli errori
//                        $log.warn("Errore in fetch: " + JSON.stringify(error));
//                    }).finally(function () {
//                        $scope.model.isFetchBusy = false;
//                    });

//                return deferred.promise;
//            }

//            $scope.openEdit = function (item) {
//                var modalInstance = $uibModal.open({
//                    animation: true,
//                    templateUrl: 'App/templates/healthRisk/edit.html',
//                    controller: 'healthRisk-editCtrl',
//                    backdrop: 'static',
//                    size: 'lg',
//                    resolve: {
//                        "healthRiskId": function () {
//                            return item ? item.id : null;
//                        }
//                    }
//                });

//                modalInstance.result.then(function (result) {
//                    $scope.healthRiskListNgTable.reload();
//                });
//            }

//            $scope.delete = function (item) {
                
//                //Definisco la richiesta
//                var request = {'id': item.id}

//                //Eseguo la richiesta al server
//                var promise = healthRiskService.delete(request);

//                //Valuto la risposta
//                promise.then(function (response) {
//                    //var index = $scope.model.healthRiskList.indexOf(item);
//                    //$scope.model.healthRiskList.splice(index, 1);
//                    $scope.healthRiskListNgTable.reload();
//                    toastr.success($rootScope.validationMessages.deleteSuccess);
//                }, function (error) {                   
//                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
//                    $log.error("Cancellazione entity fallita in healthRisk-indexCtrl" + JSON.stringify(error));
//                }).finally(function () {
//                });
//            }

//            var fetchEssentialData = function () {
//                var deferred = $q.defer();

//                var request = {};

//                healthRiskService.fetchEssentialData(request)
//                    .then(function (response) {
//                        var data = response ? response.data : {};

//                        $scope.model.healthRiskTypeList = Enumerable.from(data.healthRiskTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
//                        $scope.model.sexList = Enumerable.from(data.sexList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

//                        deferred.resolve(response);
//                    }, function (error) {
//                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
//                        deferred.reject(error);
//                    }).finally(function () {
//                        $scope.isCtrlReady = true;
//                    });

//                return deferred.promise;
//            }
//            //#endregion

//            $scope.healthRiskListNgTable = new NgTableParams({
//                page: 1,
//                sorting: { "InsertDate": "asc" }
//            },
//                {
//                    getData: function (params) {
//                        var paginationIndexs = ngTableService.buildPaginatorIndexs(params);
//                        return $scope.fetch(paginationIndexs.startRowIndex, paginationIndexs.maxRowIndex, paginationIndexs.orderByProperty, paginationIndexs.orderByType)
//                            .then(function (data) {
//                                params.total(data.count);
//                                return data.models;
//                            });
//                    }
//                });

//            var constructor = function () {
//                $scope.model.isCtrlReady = false;

//                var essentialDataPromises = [];
//                essentialDataPromises.push(fetchEssentialData());

//                $q.all(essentialDataPromises).then(function () {
//                    $scope.model.isCtrlReady = true;
//                    //$scope.fetch();
//                });
//            };

//            constructor();
//        }]);