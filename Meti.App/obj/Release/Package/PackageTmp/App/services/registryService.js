app.factory('registryService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        'utilsService',
        function (utilsService, serverRouteMap, httpService, utilsService) {
            var factory = {};

            factory.registryIndexModel = function(dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'firstname': dto.firstname,
                    'surname': dto.surname,
                    'sexType': dto.sex ? utilsService.createItemDtoModel(dto.sex) : null,
                    'registryType': dto.registryType ? utilsService.createItemDtoModel(dto.registryType) : null,
                    'mobilePhone': dto.mobilePhone,
                    'phone': dto.phone,
                    'email': dto.email,
                    'birthDate': dto.birthDate ? moment(new Date(dto.birthDate)).format('DD/MM/YY') : '',
                    'birthPlace': dto.birthPlace,
                    'regionalMedicalCode': dto.regionalMedicalCode,
                    'contacts': dto.mobilePhone,
                    'insertDate': dto.insertDate ? moment(new Date(dto.insertDate)).format('DD/MM/YY hh:mm') : '',
                    'latitude': dto.latitude,
                    'longitude': dto.longitude
                }

                if (model.phone)
                    model.contacts = model.contacts + ' / ' + model.phone;

                return model;
            }

            factory.registryDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'firstname': dto.firstname,
                    'surname': dto.surname,
                    'sex': dto.sex ? utilsService.createItemDtoModel(dto.sex) : null,
                    'registryType': dto.registryType ? utilsService.createItemDtoModel(dto.registryType) : null,
                    'mobilePhone': dto.mobilePhone,
                    'phone': dto.phone,
                    'email': dto.email,
                    'birthDate': dto.birthDate ? new Date(dto.birthDate) : '',
                    'birthPlace': dto.birthPlace,
                    'municipalityPlace': dto.municipalityPlace,
                    'municipalityPlaceAddress': dto.municipalityPlaceAddress,
                    'municipalityPlaceCap': dto.municipalityPlaceCap,
                    'domicilePlace': dto.domicilePlace,
                    'domicilePlaceAddress': dto.domicilePlaceAddress,
                    'domicilePlaceCap': dto.domicilePlaceCap,
                    'regionalMedicalCode': dto.regionalMedicalCode,
                    'latitude': dto.latitude,
                    'longitude': dto.longitude,
                    'allergy': dto.allergy,
                    'intollerance': dto.intollerance,
                    'bloodGroup': dto.bloodGroup ? utilsService.createItemDtoModel(dto.bloodGroup): '',
                    'diagnosis': dto.diagnosis,
                    'previousIllnesses': dto.previousIllnesses,
                    'nextMedicalHistory': dto.nextMedicalHistory,
                    'remoteAnamnesis': dto.remoteAnamnesis,
                    'diet': dto.diet,
                    'pathologiesInProgress': dto.pathologiesInProgress,
                    'note': dto.note,
                    'previousIllnesses': dto.previousIllnesses,
                    'weight': dto.weight,
                    'height': dto.height
                };

                model.comboMunicipality = model.municipalityPlace + ' ' + model.municipalityPlaceAddress + ' ' + model.municipalityPlaceCap || ''
                model.comboText = model.firstname + ' ' + model.surname;   
                model.comboTextAddress = model.firstname + ' ' + model.surname + ' - ' + model.comboMunicipality;
                model.latlng = [parseFloat(model.latitude), parseFloat(model.longitude)];

                return model;
            }

            factory.registryEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'firstname': dto.firstname,
                    'surname': dto.surname,
                    'sex': dto.sex ? utilsService.createItemDtoModel(dto.sex) : null,
                    'registryType': dto.registryType ? utilsService.createItemDtoModel(dto.registryType) : null,
                    'mobilePhone': dto.mobilePhone,
                    'phone': dto.phone,
                    'email': dto.email,
                    'birthDate': dto.birthDate ? moment(dto.birthDate).format('DD/MM/YYYY'): '',
                    'birthPlace': dto.birthPlace,
                    'municipalityPlace': dto.municipalityPlace,
                    'municipalityPlaceAddress': dto.municipalityPlaceAddress,
                    'municipalityPlaceCap': dto.municipalityPlaceCap,
                    'domicilePlace': dto.domicilePlace,
                    'domicilePlaceAddress': dto.domicilePlaceAddress,
                    'domicilePlaceCap': dto.domicilePlaceCap,
                    'regionalMedicalCode': dto.regionalMedicalCode,
                    'latitude': dto.latitude,
                    'longitude': dto.longitude,
                    'allergy': dto.allergy,
                    'intollerance': dto.intollerance,
                    'bloodGroup': dto.bloodGroup,
                    'diagnosis': dto.diagnosis,
                    'previousIllnesses': dto.previousIllnesses,
                    'nextMedicalHistory': dto.nextMedicalHistory,
                    'remoteAnamnesis': dto.remoteAnamnesis,
                    'diet': dto.diet,
                    'pathologiesInProgress': dto.pathologiesInProgress,
                    'note': dto.note,
                    'previousIllnesses': dto.previousIllnesses,
                    'weight': dto.weight,
                    'height': dto.height
                };

                model.comboMunicipality = model.municipalityPlace + ', ' + model.municipalityPlaceAddress + ' ' + model.municipalityPlaceCap || ''
                model.comboText = model.firstname + ' ' + model.surname;
                model.comboTextAddress = model.firstname + ' ' + model.surname + ' - ' + model.comboMunicipality;
                model.latlng = [parseFloat(model.latitude), parseFloat(model.longitude)];

                return model;
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.registry.create, 'POST', request);
            }

            factory.update = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.registry.update, 'POST', request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.registry.deleteItem, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.registry.fetch, "GET", request);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.registry.get, "GET", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.registry.fetchEssentialData, "GET", request);
            }

            factory.getGeocoding = function(request) {
                return httpService.httpRequest(serverRouteMap.routeMap.registry.getGeocoding, "GET", request);
            }
            return factory;
        }]);