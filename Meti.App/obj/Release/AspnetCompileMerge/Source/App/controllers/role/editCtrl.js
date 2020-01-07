app.controller('role-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'roleService',
        '$uibModalInstance',
        '$q',
        'roleId',
        'toastr',
        'utilsService',
        '$rootScope',
        'processInstanceService',
        function ($scope, $log, responseDispatcherService, roleService, $uibModalInstance, $q, roleId, toastr, utilsService, $rootScope, processInstanceService) {
            //#region Model
            $scope.title = '';
            $scope.roleModel = null;
            $scope.claimsKendoDataSource = [];//new kendo.data.DataSource({ data: [] }),
                $scope.usersKendoDataSource = [];//new kendo.data.DataSource({ data: [] }),
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;

            //#endregion

            //#region Behaviors


            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            var clear = function () {
                $scope.roleModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                roleService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.claimsKendoDataSource = Enumerable.from(data.claims).select(function (x) { return x }).toArray();
                        $scope.usersKendoDataSource = Enumerable.from(data.users).select(function (x) { return x }).toArray();

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

                var promise = roleService.get(request);

                promise.then(function (dto) {
                    $scope.roleModel = roleService.roleEditModel(dto.data);

                    deferred.resolve(dto);
                }, function (error) {
                    //Visualizzo gli errori
                    responseDispatcherService.printErrorMessages(error.data, toastr);
                    //Loggo gli errori
                    $log.error("Errore in editCtrl: " + JSON.stringify(error));
                    deferred.reject(error);
                });

                return deferred.promise;
            }

           
            $scope.save = function () {
                //Valido il form
                if (!$scope.roleEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $scope.isSaveBusy = true;

                var usersId = Enumerable.from($scope.roleModel.users).select(function (x) { return x.id }).toArray();
                var claimsId = Enumerable.from($scope.roleModel.claims).select(function (x) { return x.id }).toArray();

                var request = {
                    'id': $scope.roleModel.id,
                    'name': $scope.roleModel.name,
                    'description': $scope.roleModel.description,
                    'claimsId': claimsId,
                    'usersId': usersId               
                }

                var promise = null;

                if ($scope.isEdit)
                    promise = roleService.update(request);
                else
                    promise = roleService.create(request);

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

               

                $q.all(essentialDataPromises).then(function () {
                    if (roleId) {
                        essentialDataPromises.push(get(roleId));
                        $scope.isEdit = true;
                        $scope.title = 'Modifica Ruolo';
                    } else {
                        $scope.roleModel = roleService.roleEditModel({});
                        $scope.title = 'Crea Ruolo';
                    }
                    $scope.isCtrlReady = true;
                });
            }

            constructor();

            $scope.$on("$destroy", function () {
                clear();
            });
        }]);