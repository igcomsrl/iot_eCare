app.factory('userService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        function (utilsService, serverRouteMap, httpService) {
            var factory = {};

            factory.userIndexModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'firstname': dto.firstname,
                    'surname': dto.surname,
                    'username': dto.username,
                    'email': dto.email,
                    'accessFailedCount': dto.accessFailedCount,
                    'fileName': dto.fileName,
                    'imgFilePath': dto.imgFilePath                    
                }

                return model;
            }

            factory.userDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'firstname': dto.firstname,
                    'surname': dto.surname,
                    'username': dto.userName,
                    'email': dto.email,
                    'accessFailedCount': dto.accessFailedCount,
                    'fileName': dto.fileName,
                    'imgFilePath': dto.imgFilePath   
                };

                return model;
            }

            factory.userEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'firstname': dto.firstname,
                    'surname': dto.surname,
                    'username': dto.username,
                    'email': dto.email,
                    'accessFailedCount': dto.accessFailedCount || 0,
                    'fileName': dto.fileName,
                    'imgFilePath': dto.imgFilePath   
                };

                return model;
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.user.create, 'POST', request);
            }

            factory.update = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.user.update, 'POST', request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.user.deleteItem, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.user.fetch, "GET", request);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.user.get, "GET", request);
            }

            factory.resetPassword = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.user.resetPassword, "POST", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.user.fetchEssentialData, "GET", request);
            }

            return factory;
        }]);