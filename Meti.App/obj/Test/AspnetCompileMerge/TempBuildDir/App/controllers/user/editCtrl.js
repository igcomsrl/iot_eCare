app.controller('user-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'userService',
        '$uibModalInstance',
        '$q',
        'userId',
        'toastr',
        'utilsService',
        '$rootScope',
        'processInstanceService',
        function ($scope, $log, responseDispatcherService, userService, $uibModalInstance, $q, userId, toastr, utilsService, $rootScope, processInstanceService) {
            //#region Model
            $scope.title = '';
            $scope.userModel = null;
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;

            //#endregion

            //#region Behaviors


            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            var clear = function () {
                $scope.userModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                userService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        //$scope.userTypeList = Enumerable.from(data.userTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        //$scope.sexList = Enumerable.from(data.sexList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        //$scope.bloodGroupList = Enumerable.from(data.bloodGroupList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }

            var get = function (id) {
                var deferred = $q.defer();

                var request = { "id": id }

                var promise = userService.get(request);

                promise.then(function (dto) {
                    $scope.userModel = userService.userEditModel(dto.data);

                    deferred.resolve(dto);
                }, function (error) {
                    //Visualizzo gli errori
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    //Loggo gli errori
                    $log.error("Errore in editCtrl: " + JSON.stringify(error));
                    deferred.reject(error);
                });

                return deferred.promise;
            }

           
            $scope.save = function () {
                //Valido il form
                if (!$scope.userEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $scope.isSaveBusy = true;

                var request = {
                    'id': $scope.userModel.id,
                    'firstname': $scope.userModel.firstname,
                    'surname': $scope.userModel.surname,
                    'username': $scope.userModel.username,
                    'email': $scope.userModel.email,
                    'accessFailedCount': $scope.userModel.accessFailedCount,
                    'imgFilePath': $scope.userModel.imgFilePath                 
                }

                var promise = null;

                if ($scope.isEdit)
                    promise = userService.update(request);
                else
                    promise = userService.create(request);

                promise.then(function (response) {
                    toastr.success($rootScope.validationMessages.saveSuccess);
                    closeModal(); 
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Errore in richiesta-createCtrl.create: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.isSaveBusy = false;
                });
            }

            var closeModal = function() {
                $uibModalInstance.close();
                clear();
            }
            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = false;
                var essentialDataPromises = [];
                essentialDataPromises.push(fetchEssentialData());

                if (userId) {
                    essentialDataPromises.push(get(userId));
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Utente';
                } else {
                    $scope.userModel = userService.userEditModel({});
                    $scope.title = 'Crea Utente';
                }

                $q.all(essentialDataPromises).then(function () {
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            $scope.$on("$destroy", function () {
                clear();
            });
        }]);