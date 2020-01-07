app.factory('alarmFiredService',
    ['serverRouteMap',
        'httpService',
        'utilsService',
        'processInstanceService',
        'parameterService',
        'alarmService',
        function (serverRouteMap, httpService, utilsService, processInstanceService, parameterService, alarmService) {
            var factory = {};

            factory.poolingTimer;
            
            factory.alarmFiredDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'alarmColor': dto.alarmColor ? utilsService.createItemDtoModel(dto.alarmColor): null,
                    'result': dto.result,
                    'parameter': dto.parameter ? parameterService.parameterCreateModel(dto.parameter) : null,
                    'alarm': dto.alarm ? alarmService.alarmEditModel(dto.alarm) : null,
                    'processInstance': dto.processInstance ? processInstanceService.processInstanceUpdateModel(dto.processInstance): null,
                    'isActive': dto.isActive,
                    'resolvingNotes': dto.resolvingNotes,
                    'insertDate': dto.insertDate ? moment(new Date(dto.insertDate)).format('DD/MM/YY hh:mm A') : '',
                    'updateDate': dto.updateDate ? moment(new Date(dto.updateDate)).format('DD/MM/YY hh:mm A') : '',
                    'lat': dto.latitude,
                    'lng': dto.longitude,
                    'isFallNoResponse': dto.fallNoResponse,
                    'isFall': dto.fall
                };

                model.latlng = [parseFloat(model.lat), parseFloat(model.lng)];

                return model;
            }

            factory.alarmFiredSwiftModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'alarmColor': dto.alarmColor ? utilsService.createItemDtoModel(dto.alarmColor) : null,
                    'result': dto.result,
                    //'parameter': dto.parameter ? parameterService.parameterCreateModel(dto.parameter) : null,
                    //'alarm': dto.alarm ? alarmService.alarmEditModel(dto.alarm) : null,
                    //'processInstance': dto.processInstance ? processInstanceService.processInstanceUpdateModel(dto.processInstance) : null,
                    'parameterName': dto.parameterName,
                    'patientName': dto.patientName,
                    'isActive': dto.isActive,
                    'resolvingNotes': dto.resolvingNotes,
                    'insertDate': dto.insertDate ? moment(new Date(dto.insertDate)).format('DD/MM/YY hh:mm A') : '',
                    'updateDate': dto.updateDate ? moment(new Date(dto.updateDate)).format('DD/MM/YY hh:mm A') : '',
                    'lat': dto.latitude,
                    'lng': dto.longitude,
                    'isFallNoResponse': dto.fallNoResponse,
                    'isFall': dto.fall
                };

                model.latlng = [parseFloat(model.lat), parseFloat(model.lng)];

                return model;
            }

            factory.fetch = function (request, ignoreLoadingBar) {
                return httpService.httpRequest(serverRouteMap.routeMap.alarmFired.fetch, "GET", request, null, null, null, ignoreLoadingBar);
            }

            factory.get = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.alarmFired.get, "GET", request);
            }

            factory.turnOff = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.alarmFired.turnOff, "POST", request);
            }

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.alarmFired.create, "POST", request);
            }
            
            return factory;
        }]);