app.controller('healthRisk-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'healthRiskService',
        '$uibModalInstance',
        '$q',
        'healthRiskModel',
        'toastr',
        'utilsService',
        '$rootScope',
        '$uibModal',
        function ($scope, $log, responseDispatcherService, healthRiskService, $uibModalInstance, $q, healthRiskModel, toastr, utilsService, $rootScope, $uibModal) {
            //#region Model
            $scope.title = '';
            $scope.healthRiskModel = null;
            $scope.healthRiskLevelList = [];
            $scope.healthRiskTypeList = [];
            //$scope.deviceList = [];
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;

            //#endregion

            //#region Behaviors

            $scope.openStartDatePicker = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.openStartDate = true;
            };

            $scope.openEndDatePicker = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.openEndDate= true;
            };

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            var clear = function () {
                $scope.healthRiskModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                healthRiskService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.healthRiskTypeList = Enumerable.from(data.healthRiskTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        $scope.healthRiskLevelList = Enumerable.from(data.healthRiskLevelList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            $scope.save = function () {
                //Valido il form
                if (!$scope.healthRiskEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $uibModalInstance.close({ "model": $scope.healthRiskModel, "isEdit": $scope.isEdit });

                closeModal();
            }

            var closeModal = function () {
                $uibModalInstance.close();
                clear();
            }
            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = false;
                var essentialDataPromises = [];

                if (healthRiskModel) {
                    $scope.healthRiskModel = angular.copy(healthRiskModel);
                    //essentialDataPromises.push(get(healthRiskId));
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Comorbidità';
                } else {
                    $scope.healthRiskModel = healthRiskService.healthRiskEditModel({});
                    $scope.title = 'Crea Comorbidità';
                }

                essentialDataPromises.push(fetchEssentialData());

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            //$scope.$on("$destroy", function () {
            //    clear();
            //});
        }]);