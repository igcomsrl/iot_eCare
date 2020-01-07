app.controller('processMacro-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'processMacroService',
        '$uibModalInstance',
        '$q',
        'processMacroModel',
        'toastr',
        'utilsService',
        '$rootScope',
        '$uibModal',
        'deviceService',
        function ($scope, $log, responseDispatcherService, processMacroService, $uibModalInstance, $q, processMacroModel, toastr, utilsService, $rootScope, $uibModal, deviceService) {
            //#region Model
            $scope.title = '';
            $scope.processMacroModel = null;
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;

            //#endregion

            //#region Behaviors

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            var clear = function () {
                $scope.processMacroModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }
            
            $scope.save = function () {
                //Valido il form
                if (!$scope.processMacroEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                
                $uibModalInstance.close({ "model": $scope.processMacroModel, "isEdit": $scope.isEdit });
                closeModal();
            }

            var closeModal = function() {
                $uibModalInstance.close();
                clear();
            }
            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = false;
                var essentialDataPromises = [];
                

                if (processMacroModel) {
                    $scope.processMacroModel = angular.copy(processMacroModel);
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Macro';
                } else {
                    $scope.processMacroModel = processMacroService.processMacroEditModel({});
                    $scope.title = 'Crea Macro';
                }

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            //$scope.$on("$destroy", function () {
            //    clear();
            //});
        }]);