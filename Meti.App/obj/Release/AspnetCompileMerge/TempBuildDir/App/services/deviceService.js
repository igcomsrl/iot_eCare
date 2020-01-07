app.factory('deviceService',
    ['utilsService',
    'serverRouteMap',
        'httpService',
        function (utilsService, serverRouteMap, httpService) {
            var factory = {};

            factory.deviceIndexModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'macaddress': dto.macaddress,
                    'name': dto.name,
                    'isEnabled': dto.isEnabled == 'undefined' || true,
                    'processInstanceName': dto.processInstanceName,
                    'patientName': dto.patientName
                }

                model.comboText = model.name + ' ' + model.macaddress;
                if (model.processInstance)
                    model.processInstanceComboText = model.processInstance.process.name + ' ' + model.processInstance.patient.firstname + ' ' + model.processInstance.patient.surname ;

                return model;
            }

            factory.deviceEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'macaddress': dto.macaddress,
                    'name': dto.name,
                    'isEnabled': dto.isEnabled == 'undefined' || true
                }

                model.comboText = model.name + ' ' + model.macaddress;

                return model;
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.device.create, 'POST', request);
            }

            factory.update = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.device.update, 'POST', request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.device.deleteItem, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.device.fetch, "GET", request);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.device.get, "GET", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.device.fetchEssentialData, "GET", request);
            }

            return factory;
        }]);