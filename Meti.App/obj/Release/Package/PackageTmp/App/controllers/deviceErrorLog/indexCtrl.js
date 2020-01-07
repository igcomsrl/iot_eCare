app.controller('deviceErrorLog-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'deviceErrorLogService',
        '$q',
        '$uibModal',
        'NgTableParams',
        'ngTableService',
        'utilsService',
        '$rootScope',
        function ($scope, $log, toastr, responseDispatcherService, deviceErrorLogService, $q, $uibModal, NgTableParams, ngTableService, utilsService, $rootScope) {
            //#region Model
            $scope.model = {
                deviceErrorLogList: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            //#region Behaviors
            

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                deviceErrorLogService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.deviceErrorLogList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = deviceErrorLogService.deviceErrorLogIndexModel(dto);
                            $scope.model.deviceErrorLogList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.deviceErrorLogList, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }
            
            //#endregion

            $scope.deviceErrorLogListNgTable = new NgTableParams({
                page: 1,
                count: 100,
                sorting: { "InsertDate": "desc" }
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