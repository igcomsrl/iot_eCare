app.factory('processInstanceService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        'registryService',
        'processService',
        function (utilsService, serverRouteMap, httpService, registryService, processService) {
            var factory = {};

            factory.processInstanceIndexModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'doctors': dto.doctors ? Enumerable.from(dto.doctors).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'patient': dto.patient ? registryService.registryEditModel(dto.patient) : '',
                    'referencePersons': dto.referencePersons ? Enumerable.from(dto.referencePersons).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    //'doctorFirstname': dto.doctorFirstname,
                    //'doctorSurname': dto.doctorSurname,
                    //'patientFirstname': dto.patientFirstname,
                    //'patientSurname': dto.patientSurname,
                    'processName': dto.processName,
                    'insertDate': dto.insertDate ? new Date(dto.insertDate) : '',   
                    'isEnabled': dto.isEnabled || true,
                    'processInstanceState': dto.processInstanceState ? utilsService.createItemDtoModel(dto.processInstanceState) : null,
                }

                if (model.doctors && model.doctors.length > 0)
                {
                    var doctorComboText = '';
                    angular.forEach(model.doctors, function (value) {

                        doctorComboText = doctorComboText + value.firstname + ' ' + value.surname + ' ';
                        
                    });
                    model.doctorsComboText = doctorComboText;
                }

                if (model.referencePersons && model.referencePersons.length > 0) {
                    var referencePersonsComboText = '';
                    angular.forEach(model.referencePersons, function (value) {
                        referencePersonsComboText = referencePersonsComboText + value.firstname + ' ' + value.surname + ' ';
                    });
                    model.referencePersonsComboText = referencePersonsComboText;
                }
                
                return model;
            }

            factory.processInstanceUpdateModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'process': dto.process ? processService.processUpdateModel(dto.process) : '',
                    'isEnabled': dto.isEnabled || true,
                    'doctors': dto.doctors ? Enumerable.from(dto.doctors).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'patient': dto.patient ? registryService.registryEditModel(dto.patient) : '',
                    'referencePersons': dto.referencePersons ? Enumerable.from(dto.referencePersons).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'processInstanceState': dto.processInstanceState ? utilsService.createItemDtoModel(dto.processInstanceState) : null,
                    'reOpenDate': dto.reOpenDate ? new Date(dto.reOpenDate): null,
                    'reOpenFrom': dto.reOpenFrom,
                    'closeDate': dto.closeDate ? new Date(dto.closeDate): null,
                    'closeFrom': dto.closeFrom
                };

                return model;
            }

            factory.processInstanceCreateModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'process': dto.process ? processService.processUpdateModel(dto.process) : '',
                    'isEnabled': dto.isEnabled || true,
                    //'doctor': dto.doctor ? registryService.registryEditModel(dto.doctor) : '',
                    //'patient': dto.registry ? registryService.registryEditModel(dto.patient) : ''
                    'doctors': dto.doctors ? Enumerable.from(dto.doctors).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'patient': dto.patient ? registryService.registryEditModel(dto.patient) : '',
                    'referencePersons': dto.referencePersons ? Enumerable.from(dto.referencePersons).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'processInstanceState': dto.processInstanceState ? utilsService.createItemDtoModel(dto.processInstanceState) : null,
                    'reOpenDate': dto.reOpenDate ? new Date(dto.reOpenDate) : null,
                    'reOpenFrom': dto.reOpenFrom,
                    'closeDate': dto.closeDate ? new Date(dto.closeDate) : null,
                    'closeFrom': dto.closeFrom
                };

                return model;
            }
            

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.create, 'POST', request);
            }

            factory.update = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.update, 'POST', request);
            }

            factory.open = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.open, 'POST', request);
            }

            factory.close = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.close, 'POST', request);
            }

            factory.updatePositionLast = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.updatePositionLast, 'POST', request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.deleteItem, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.fetch, "GET", request);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.get, "GET", request);
            }

            factory.getByRegistry = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.getByRegistry, "GET", request);
            }

            factory.getByRegistryEmail = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.getByRegistryEmail, "GET", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.fetchEssentialData, "GET", request);
            }

            return factory;
        }]);