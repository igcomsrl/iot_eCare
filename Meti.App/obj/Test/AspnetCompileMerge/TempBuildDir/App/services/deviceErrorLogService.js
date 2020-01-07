app.factory('deviceErrorLogService',
    ['serverRouteMap',
        'httpService',
        function (serverRouteMap, httpService) {
            var factory = {};

            factory.log = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.deviceErrorLog.log, "POST", request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.deviceErrorLog.fetch, "GET", request);
            }

            factory.deviceErrorLogIndexModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'error': dto.error,
                    'deviceId': dto.deviceId,
                    'processInstanceId': dto.processInstanceId,
                    'insertDate': dto.insertDate
                };

                return model;
            }

            return factory;
        }]);