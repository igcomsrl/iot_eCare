app.factory('alarmService',
    ['serverRouteMap',
        'httpService',
        'alarmMetricService',
        'utilsService',
        function (serverRouteMap, httpService, alarmMetricService, utilsService) {
            var factory = {};

            factory.alarmDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'parameter': dto.parameter,
                    'emails': dto.emails,
                    'smsNumbers': dto.smsNumbers,
                    'contactOperator': dto.contactOperator || false,
                    'isEnabled': dto.isEnabled || true,
                    'alarmMetrics': Enumerable.from(dto.alarmMetrics).select(function (y) { return alarmMetricService.alarmMetricEditModel(y) }).toArray(),
                    'alarmColor': dto.alarmColor ? utilsService.createItemDtoModel(dto.alarmColor) : '',
                    'helpMessage': dto.helpMessage
                };

                return model;
            }

            factory.alarmEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'parameter': dto.parameter,
                    'emails': dto.emails,
                    'smsNumbers': dto.smsNumbers,
                    'contactOperator': dto.contactOperator || false,
                    'isEnabled': dto.isEnabled || true,
                    'helpMessage': dto.helpMessage,
                    'alarmMetrics': Enumerable.from(dto.alarmMetrics).select(function (y) { return alarmMetricService.alarmMetricEditModel(y) }).toArray(),
                    'alarmColor': dto.alarmColor ? utilsService.createItemDtoModel(dto.alarmColor) : ''
                };

                return model;
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.alarm.create, 'POST', request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.alarm.fetchEssentialData, "GET", request);
            }

            factory.alarmFiredGreenCount = 0;
            factory.alarmFiredYellowCount = 0;
            factory.alarmFiredRedCount = 0;

            return factory;
        }]);