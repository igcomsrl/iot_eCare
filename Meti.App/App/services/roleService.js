app.factory('roleService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        'utilsService',
        function (utilsService, serverRouteMap, httpService, utilsService) {
            var factory = {};

            factory.roleIndexModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'description': dto.description              
                }

                return model;
            }

            factory.roleDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'description': dto.description,
                    'users': dto.usersId,
                    'claims': dto.claimsId      
                };

                return model;
            }

            factory.roleEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'description': dto.description,
                    'users': dto.usersId,
                    'claims': dto.claimsId          
                };

                return model;
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.role.create, 'POST', request);
            }

            factory.update = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.role.update, 'POST', request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.role.deleteItem, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.role.fetch, "GET", request);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.role.get, "GET", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.role.fetchEssentialData, "GET", request);
            }

            return factory;
        }]);