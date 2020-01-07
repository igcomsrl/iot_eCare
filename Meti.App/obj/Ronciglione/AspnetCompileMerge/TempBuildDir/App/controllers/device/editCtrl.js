app.controller('device-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'deviceService',
        '$uibModalInstance',
        '$q',
        'deviceId',
        'toastr',
        'utilsService',
        '$rootScope',
        function ($scope, $log, responseDispatcherService, deviceService, $uibModalInstance, $q, deviceId, toastr, utilsService, $rootScope) {
            //#region Model
            $scope.title = '';
            $scope.deviceModel = null;
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;

            //#endregion

            //#region Behaviors

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            var clear = function () {
                $scope.deviceModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            //var fetchEssentialData = function () {
            //    var deferred = $q.defer();

            //    var request = {};

            //    deviceService.fetchEssentialData(request)
            //        .then(function (response) {
            //            var data = response ? response.data : {};

            //            $scope.deviceTypeList = Enumerable.from(data.deviceTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
            //            $scope.sexList = Enumerable.from(data.sexList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

            //            deferred.resolve(response);
            //        }, function (error) {
            //            $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
            //            deferred.reject(error);
            //        }).finally(function () {
            //            $scope.isCtrlReady = true;
            //        });

            //    return deferred.promise;
            //}

            var get = function (id) {
                var deferred = $q.defer();

                var request = { "id": id }

                var promise = deviceService.get(request);

                promise.then(function (dto) {
                    $scope.deviceModel = deviceService.deviceEditModel(dto.data);

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
                if (!$scope.deviceEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $scope.isSaveBusy = true;

                var request = {
                    'id': $scope.deviceModel.id,
                    'macaddress': $scope.deviceModel.macaddress,
                    'name': $scope.deviceModel.name,
                    'isEnabled': $scope.deviceModel.isEnabled
                }

                var promise = null;

                if ($scope.isEdit)
                    promise = deviceService.update(request);
                else
                    promise = deviceService.create(request);

                promise.then(function (response) {
                    toastr.success($rootScope.validationMessages.saveSuccess);
                    closeModal(); 
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data, toastr);
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
                //essentialDataPromises.push(fetchEssentialData());

                if (deviceId) {
                    essentialDataPromises.push(get(deviceId));
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Dispositivo';
                } else {
                    $scope.deviceModel = deviceService.deviceEditModel({});
                    $scope.title = 'Crea Dispositivo';
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