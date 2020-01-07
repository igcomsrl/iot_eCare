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
                    'processId': dto.processId,
                    'insertDate': dto.insertDate ? moment(dto.insertDate).format('DD/MM/YY hh:mm A') : ''
                };

                return model;
            }

            return factory;
        }]);