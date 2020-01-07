app.factory('processMacroService',
    ['serverRouteMap',
    'httpService',
        function (serverRouteMap, httpService) {
            var factory = {};
            
            factory.processMacroDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'value': dto.value,
                };

                return model;
            }

            factory.processMacroEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'name': dto.name,
                    'value': dto.value,
                };

                return model;
            }
            
            return factory;
        }]);