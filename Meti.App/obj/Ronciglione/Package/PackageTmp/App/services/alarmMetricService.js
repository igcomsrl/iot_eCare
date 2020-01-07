app.factory('alarmMetricService',
    ['serverRouteMap',
        'httpService',
        'deviceService',
        function (serverRouteMap, httpService, deviceService) {
            var factory = {};
            
            factory.alarmMetricDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'metric': dto.metric,
                    'thresholdMin': dto.thresholdMin ? parseFloat(dto.thresholdMin): null,
                    'thresholdMax': dto.thresholdMax ? parseFloat(dto.thresholdMax) : null,
                    'device': dto.device ? deviceService.deviceEditModel(dto.device): null,
                    'alarm': dto.alarm             
                };

                return model;
            }

            factory.alarmMetricEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'metric': dto.metric,
                    'thresholdMin': dto.thresholdMin,
                    'thresholdMax': dto.thresholdMax,
                    'device': dto.device ? deviceService.deviceEditModel(dto.device) : null,
                    'alarm': dto.alarm 
                };

                return model;
            }
            
            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.alarmMetric.fetchEssentialData, "GET", request);
            }

            return factory;
        }]);