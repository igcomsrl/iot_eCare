app.factory('processService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        'utilsService',
        'registryService',
        'parameterService',
        function (utilsService, serverRouteMap, httpService, utilsService, registryService, parameterService) {
            var factory = {};

            factory.processIndexModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'insertDate': dto.insertDate ? new Date(dto.insertDate) : '',   
                    'isEnabled': dto.isEnabled || true
                }

                return model;
            }

            factory.processUpdateModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'parameters': dto.parameters ? Enumerable.from(dto.parameters).select(function (x) { return parameterService.parameterCreateModel(x) }).toArray() : [],
                    'processMacros': dto.processMacros || [],
                    'isEnabled': dto.isEnabled || true,
                    'processType': dto.processType
            };

                return model;
            }

            factory.processCreateModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'parameters': dto.parameters ? Enumerable.from(dto.parameters).select(function (x) { return parameterService.parameterCreateModel(x) }).toArray() : [],
                    'processMacros': dto.processMacros || [],
                    'isEnabled': dto.isEnabled || true,
                    'processType': dto.processType
                };

                return model;
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.process.create, 'POST', request);
            }

            factory.update = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.process.update, 'POST', request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.process.deleteItem, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.process.fetch, "GET", request);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.process.get, "GET", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.process.fetchEssentialData, "GET", request);
            }

            return factory;
        }]);