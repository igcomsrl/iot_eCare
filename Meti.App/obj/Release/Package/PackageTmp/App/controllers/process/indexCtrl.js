app.controller('process-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'processService',
        '$q',
        'NgTableParams',
        'ngTableService',
        'utilsService',
        '$rootScope',
        '$state',
        'registryService',
        function ($scope, $log, toastr, responseDispatcherService, processService, $q, NgTableParams, ngTableService, utilsService, $rootScope, $state, registryService) {
            //#region Model
            $scope.model = {
                name: null,
                //doctor: null,
                //patient: null,
                //doctorList: [],
                //patientList: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            //$scope.$watchGroup(['model.doctor', 'model.patient'],
            //    function (newValues, oldValues, scope) {
            //        $scope.model.processList = [];
            //    });

            //#region Behaviors

            $scope.clearFilter = function () {
                $scope.model.name = null;
                //$scope.model.doctor = null;
                //$scope.model.patient = null;
                $scope.model.isFetchBusy = false;
                $scope.model.processList = [];
            }

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'name': $scope.model.name,
                    'processType': 1,//template
                    //'doctorId': $scope.model.doctor ? $scope.model.doctor.id : '',
                    //'patientId': $scope.model.patient ? $scope.model.patient.id : '',
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                processService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.processList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = processService.processIndexModel(dto);
                            $scope.model.processList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.processList, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }

            $scope.navigateUpdate = function (item) {
                if (!item) {
                    $log.error("Item is null!");
                    return;
                }

                $state.go('shell.process-update', {'id': item.id});
            }

            $scope.delete = function (item) {
                if (!item) {
                    $log.error("Item is null!");
                    return;
                }

                //Definisco la richiesta
                var request = { 'id': item.id }

                //Eseguo la richiesta al server
                var promise = processService.delete(request);

                //Valuto la risposta
                promise.then(function (response) {
                    //var index = $scope.model.processList.indexOf(item);
                    //$scope.model.processList.splice(index, 1);
                    $scope.processListNgTable.reload();
                    toastr.success($rootScope.validationMessages.deleteSuccess);
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Cancellazione entity fallita in process-indexCtrl" + JSON.stringify(error));
                }).finally(function () {
                });
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                processService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        //$scope.model.doctorList = Enumerable.from(data.doctorList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();
                        //$scope.model.patientList = Enumerable.from(data.patientList).select(function (x) { return registryService.registryDetailModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                    });

                return deferred.promise;
            }
            //#endregion

            $scope.processListNgTable = new NgTableParams({
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