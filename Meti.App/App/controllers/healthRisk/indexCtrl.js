app.controller('healthRisk-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'healthRiskService',
        '$q',
        '$uibModal',
        'utilsService',
        '$filter',
        '$uibModal',
        '$rootScope',
        function ($scope, $log, toastr, responseDispatcherService, healthRiskService, $q, $uibModal, utilsService, $filter, $uibModal, $rootScope) {
            //#region Model
            $scope.model = {
                registryId: 0,
                comorbidityIndex: 0,
                severityIndex:0,
                healthRisks: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            //#region Behaviors

            $rootScope.$on("registryModelUpdated", function (data) {
                if (data) {
                    $scope.model.registryId = healthRiskService.registryId;
                    $scope.fetch();
                }
            })

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'registryId': $scope.model.registryId,
                    'firstname': $scope.model.firstname,
                    'surname': $scope.model.surname,
                    'healthRiskLevel': $scope.model.healthRiskLevel,
                    'healthRiskType': $scope.model.healthRiskType,
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                healthRiskService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.healthRisks = [];
                        healthRiskService.healthRisks = [];
                        angular.forEach(data.data, function (dto) {
                            var model = healthRiskService.healthRiskIndexModel(dto);
                            $scope.model.healthRisks.push(model);
                            healthRiskService.healthRisks.push(model);
                        });

                        $scope.model.healthRisks = $filter('orderBy')($scope.model.healthRisks, '-startDate');

                        calculateIndexs($scope.model.healthRisks);
                        deferred.resolve({ "models": $scope.model.healthRisks, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }

            $scope.deleteHealthRisk = function (item) {

                //Definisco la richiesta
                var request = {'id': item.id}

                //Eseguo la richiesta al server
                var promise = healthRiskService.delete(request);

                //Valuto la risposta
                promise.then(function (response) {
                    var index = $scope.model.healthRisks.indexOf(item);
                    $scope.model.healthRisks.splice(index, 1);
                   // $scope.healthRisksNgTable.reload();
                    calculateIndexs($scope.model.healthRisks)
                    toastr.success($rootScope.validationMessages.deleteSuccess);
                }, function (error) {                   
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Cancellazione entity fallita in healthRisk-indexCtrl" + JSON.stringify(error));
                }).finally(function () {
                });
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                healthRiskService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.model.healthRiskTypeList = Enumerable.from(data.healthRiskTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        $scope.model.healthRiskLevelList = Enumerable.from(data.healthRiskLevelList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            $scope.openHealthRiskEdit = function (item, isEdit, index) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/healthRisk/edit.html',
                    controller: 'healthRisk-editCtrl',
                    backdrop: 'static',
                    size: 'md',
                    resolve: {
                        "healthRiskModel": function () {
                            return item;
                        },
                        "isEdit": function () {
                            return isEdit;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    var model = result.model;
                    var isEdit = result.isEdit;

                    if (isEdit) {
                        $scope.removeHealthRisk(index);
                    }

                    $scope.model.healthRisks.push(model);

                    $scope.model.healthRisks = $filter('orderBy')($scope.model.healthRisks, '-startDate');
                    healthRiskService.healthRisks = $scope.model.healthRisks;

                    calculateIndexs($scope.model.healthRisks)
                   // $scope.healthRisksNgTable.reload();
                });
            }

            var calculateIndexs = function (models) {
                $scope.model.severityIndex = healthRiskService.calculateSeverityIndex(models);
                $scope.model.comorbidityIndex = healthRiskService.calculateComorbidityIndex(models);
            }

            $scope.removeHealthRisk = function (index) {
                //var index = $scope.model.healthRisks.indexOf(healthRisk);
                if (index > -1)
                    $scope.model.healthRisks.splice(index, 1);
            }
            //#endregion

            //$scope.healthRisksNgTable = new NgTableParams({
            //    page: 1,
            //    sorting: { "dateStart": "desc" }
            //},
            //    {
            //        getData: function (params) {
            //            var paginationIndexs = ngTableService.buildPaginatorIndexs(params);
            //            return $scope.fetch(paginationIndexs.startRowIndex, paginationIndexs.maxRowIndex, paginationIndexs.orderByProperty, paginationIndexs.orderByType)
            //                .then(function (data) {
            //                    params.total(data.count);
            //                    return data.models;
            //                });
            //        }
            //    });

            var constructor = function () {
                $scope.model.isCtrlReady = false;
                healthRiskService.registryId = '';

                var essentialDataPromises = [];
                essentialDataPromises.push(fetchEssentialData());
                //essentialDataPromises.push($scope.fetch());

                $q.all(essentialDataPromises).then(function () {
                    $scope.model.isCtrlReady = true;
                    healthRiskService.healthRisks = $scope.model.healthRisks;
                    //$scope.fetch();
                });
            };

            constructor();
        }]);