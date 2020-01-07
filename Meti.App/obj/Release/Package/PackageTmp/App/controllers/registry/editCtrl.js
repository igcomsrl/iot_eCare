app.controller('registry-editCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        'registryService',
        '$uibModalInstance',
        '$q',
        'registryId',
        'toastr',
        'utilsService',
        '$rootScope',
        'processInstanceService',
        function ($scope, $log, responseDispatcherService, registryService, $uibModalInstance, $q, registryId, toastr, utilsService, $rootScope, processInstanceService) {
            //#region Model
            $scope.title = '';
            $scope.registryModel = null;
            $scope.registryTypeList = [];
            $scope.sexList = [];
            $scope.bloodGroupList = [];
            $scope.isCtrlReady = false;
            $scope.isSaveBusy = false;
            $scope.isEdit = false;
            $scope.isMedico = false;
            $scope.processInstanceModelList = null;
            $scope.showProcessInstance = false;

            //#endregion
            $scope.$watch('registryModel.registryType', function (newvalue) {
                if (!$scope.registryModel || !$scope.registryModel.registryType)
                    return;

                //Medico
                if ($scope.registryModel.registryType.id === 2) {
                    $scope.isMedico = true;
                } else
                    $scope.isMedico = false;
            });
            $scope.$watch('registryModel.municipalityPlaceAddress', function (newvalue) {
                if (!$scope.registryModel || !$scope.registryModel.municipalityPlaceAddress)
                    return;
                
               //POpola le coordinate lat e long
                var request = {
                    street: $scope.registryModel.municipalityPlaceAddress,
                    city: $scope.registryModel.municipalityPlace,
                    format:'json'
                }

                var promise = registryService.getGeocoding(request);

                promise.then(function (response) {
                    var data = response.data[0];
                    if (!data) return;
                    var latitude = data.lat;
                    var longitude = data.lon;
                    $scope.registryModel.latitude = latitude;
                    $scope.registryModel.longitude = longitude;
                    console.log(response)
                }, function(error) {
                    $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));                    
                });

                //https://nominatim.openstreetmap.org/search?street=via%20lenzi%2015&format=geocodejson&city=messina
            });

            //#region Behaviors


            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            var clear = function () {
                $scope.registryModel = null;
                $scope.isCtrlReady = false;
                $scope.isSaveBusy = false;
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                registryService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        $scope.registryTypeList = Enumerable.from(data.registryTypeList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        $scope.sexList = Enumerable.from(data.sexList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();
                        $scope.bloodGroupList = Enumerable.from(data.bloodGroupList).select(function (x) { return utilsService.createItemDtoModel(x) }).toArray();

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

                var promise = registryService.get(request);

                promise.then(function (dto) {
                    $scope.registryModel = registryService.registryEditModel(dto.data);

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

            var getProcessInstance = function (registryId) {
                var deferred = $q.defer();

                var request = { "registryId": registryId }

                var promise = processInstanceService.getByRegistry(request);

                promise.then(function (dto) {
                    $scope.processInstanceModelList = [];

                    angular.forEach(dto.data, function(model) {
                        var processInstanceModel = processInstanceService.processInstanceUpdateModel(model);
                        $scope.processInstanceModelList.push(processInstanceModel);
                    });

                    $scope.showProcessInstance = true;
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
                if (!$scope.registryEditForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                $scope.isSaveBusy = true;

                var request = {
                    'id': $scope.registryModel.id,
                    'firstname': $scope.registryModel.firstname,
                    'surname': $scope.registryModel.surname,
                    'sex': $scope.registryModel.sex ? $scope.registryModel.sex.id: '',
                    'registryType': $scope.registryModel.registryType ? $scope.registryModel.registryType.id: '',
                    'mobilePhone': $scope.registryModel.mobilePhone,
                    'phone': $scope.registryModel.phone,
                    'email': $scope.registryModel.email,
                    'birthDate': $scope.registryModel.birthDate ? moment($scope.registryModel.birthDate, 'DD-MM-YYYYTHH:mm:ss').format() : '',    
                    'birthPlace': $scope.registryModel.birthPlace,
                    'municipalityPlace': $scope.registryModel.municipalityPlace,
                    'municipalityPlaceAddress': $scope.registryModel.municipalityPlaceAddress,
                    'municipalityPlaceCap': $scope.registryModel.municipalityPlaceCap,
                    'domicilePlace': $scope.registryModel.domicilePlace,
                    'domicilePlaceAddress': $scope.registryModel.domicilePlaceAddress,
                    'domicilePlaceCap': $scope.registryModel.domicilePlaceCap,
                    'regionalMedicalCode': $scope.registryModel.regionalMedicalCode,
                    'latitude': $scope.registryModel.latitude,
                    'longitude': $scope.registryModel.longitude,
                    'allergy': $scope.registryModel.allergy,
                    'intollerance': $scope.registryModel.intollerance,
                    'bloodGroup': $scope.registryModel.bloodGroup ? $scope.registryModel.bloodGroup.id: '',
                    'diagnosis': $scope.registryModel.diagnosis,
                    'previousIllnesses': $scope.registryModel.previousIllnesses,
                    'nextMedicalHistory': $scope.registryModel.nextMedicalHistory,
                    'remoteAnamnesis': $scope.registryModel.remoteAnamnesis,
                    'diet': $scope.registryModel.diet,
                    'pathologiesInProgress': $scope.registryModel.pathologiesInProgress,
                    'note': $scope.registryModel.note,
                    'height': $scope.registryModel.height,
                    'weight': $scope.registryModel.weight
                }

                var promise = null;

                if ($scope.isEdit)
                    promise = registryService.update(request);
                else
                    promise = registryService.create(request);

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
                essentialDataPromises.push(fetchEssentialData());

                if (registryId) {
                    essentialDataPromises.push(get(registryId));
                    essentialDataPromises.push(getProcessInstance(registryId));
                    $scope.isEdit = true;
                    $scope.title = 'Modifica Anagrafica';
                } else {
                    $scope.registryModel = registryService.registryEditModel({});
                    $scope.title = 'Crea Anagrafica';
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