app.factory('parameterService',
    ['utilsService',
    'serverRouteMap',
    'httpService',
        'deviceService',
        'alarmService',
        function (utilsService, serverRouteMap, httpService, deviceService, alarmService) {
            var factory = {};

            factory.parameterIndexModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'process': dto.process,
                    'device': dto.device ? deviceService.deviceEditModel(dto.device) : '',
                    'frequency': dto.frequency,
                    'frequencyType': dto.frequencyType ?utilsService.createItemDtoModel(dto.frequancyType): '',
                    'alarms': Enumerable.from(dto.alarms).select(function (y) { return alarmService.alarmDetailModel(y) }).toArray(),
                    'isEnabled': dto.isEnabled,
                    'positionMisure': dto.positionMisure
                }

                return model;
            }

            factory.parameterCreateModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'process': dto.process,
                    'device': dto.device ? deviceService.deviceEditModel(dto.device): '',
                    'frequency': dto.frequency,
                    'frequencyType': dto.frequencyType,
                    'alarms': Enumerable.from(dto.alarms).select(function (y) { return alarmService.alarmDetailModel(y) }).toArray(),
                    'isEnabled': dto.isEnabled || true,
                    'positionMisure': dto.positionMisure
                };

                return model;
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.parameter.create, 'POST', request);
            }

            factory.update = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.parameter.update, 'POST', request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.parameter.deleteItem, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.parameter.fetch, "GET", request);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.parameter.get, "GET", request);
            }

            factory.getByDevice = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.parameter.getByDevice, "GET", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.parameter.fetchEssentialData, "GET", request);
            }

            return factory;
        }]);