app.controller('alarmFired-indexCtrl',
    ['$scope',
        '$log',
        'alarmFiredService',
        '$q',
        '$rootScope',
        '$interval',
        '$uibModal',
        'NgTableParams',
        'ngTableService',
        function ($scope, $log, alarmFiredService, $q, $rootScope, $interval, $uibModal, NgTableParams, ngTableService) {
            //#region Model
            $scope.model = {
                alarmFiredGreenList: [],
                alarmFiredYellowList: [],
                alarmFiredRedList: [],
                alarmFiredList: [],
                alarmFiredDisabledList: [],
                isCtrlReady: false,
                isFetchBusy: false,
                timer: false,
                lastUpdatePooling: new Date()
            };
            //#endregion

            //#region Behaviors

            $scope.clearFilter = function () {
                $scope.model.isFetchBusy = false;
                $scope.model.alarmFiredList = [];
            }

            $scope.toDetail = function (alertFiredModel, onlyRead) {

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/alarmFired/detail.html',
                    controller: 'alarmFiredDetailCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "alarmFiredId": function () {
                            return alertFiredModel.id;
                        },
                        "onlyRead": function () {
                            return onlyRead;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    $scope.fetch();
                });
            }


            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'isActive': true,
                    "startRowIndex": "",
                    "maxRowIndex": "",
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                alarmFiredService.fetch(request, true)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.alarmFiredList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = alarmFiredService.alarmFiredSwiftModel(dto);
                            $scope.model.alarmFiredList.push(model);
                        });

                        $scope.model.alarmFiredGreenList = Enumerable.from($scope.model.alarmFiredList).where(function (x) { return x.alarmColor.id == 1 && x.isActive == true }).toArray();
                        $scope.model.alarmFiredYellowList = Enumerable.from($scope.model.alarmFiredList).where(function (x) { return x.alarmColor.id == 2 && x.isActive == true }).toArray();
                        $scope.model.alarmFiredRedList = Enumerable.from($scope.model.alarmFiredList).where(function (x) { return x.alarmColor.id == 3 && x.isActive == true }).toArray();

                        alarmFiredService.alarmFiredGreenCount = $scope.model.alarmFiredGreenList.length;
                        alarmFiredService.alarmFiredYellowCount = $scope.model.alarmFiredYellowList.length;
                        alarmFiredService.alarmFiredRedCount = $scope.model.alarmFiredRedList.length;

                        $scope.model.lastUpdatePooling = new Date();

                        $rootScope.$broadcast("sidebar-fetchAlarmFired");

                        deferred.resolve({ "models": $scope.model.alarmFiredList, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }

            $scope.fetchDisabled = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'isActive': false,
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                alarmFiredService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.alarmFiredDisabledList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = alarmFiredService.alarmFiredSwiftModel(dto);
                            $scope.model.alarmFiredDisabledList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.alarmFiredDisabledList, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }

            $scope.alarmFiredDisabledListNgTable = new NgTableParams({
                page: 1,
                count: 25,
                sorting: { "UpdateDate": "desc" }
            },
                {
                    getData: function (params) {
                        var paginationIndexs = ngTableService.buildPaginatorIndexs(params);
                        return $scope.fetchDisabled(paginationIndexs.startRowIndex, paginationIndexs.maxRowIndex, paginationIndexs.orderByProperty, paginationIndexs.orderByType)
                            .then(function (data) {
                                params.total(data.count);
                                return data.models;
                            });
                    }
                });

            var constructor = function () {
                $scope.model.isCtrlReady = false;

                var essentialDataPromises = [];
                essentialDataPromises.push($scope.fetch());

                $q.all(essentialDataPromises).then(function () {
                    $scope.model.isCtrlReady = true;
                    if (!alarmFiredService.poolingTimer)
                    {
                        alarmFiredService.poolingTimer = $interval($scope.fetch, $rootScope.alarmFiredPooling);                        
                    }
                        
                });
            };

            constructor();
        }]);