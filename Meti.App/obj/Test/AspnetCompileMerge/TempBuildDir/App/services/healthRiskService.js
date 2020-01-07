//app.factory('healthRiskService',
//    ['utilsService',
//        'serverRouteMap',
//        'httpService',
//        'utilsService',
//        function (utilsService, serverRouteMap, httpService, utilsService) {
//            var factory = {};

//            factory.healthRiskIndexModel = function(dto) {
//                if (!dto) return {};

//                var model = {
//                    'id': dto.id,
//                    'type': dto.type ? utilsService.createItemDtoModel(dto.type) : null,
//                    'level': dto.level ? utilsService.createItemDtoModel(dto.level) : null,
//                    'rating': dto.rating,
//                    'startDate': dto.startDate ? new Date(dto.startDate) : null,
//                    'endDate': dto.endDate ? new Date(dto.endDate) : null,
//                    'isLast': dto.isLast,
//                    'phone': dto.phone,
//                    'registry': dto.registry ? dto.registry.id : null
//                };

//                return model;
//            }

//            factory.healthRiskDetailModel = function (dto) {
//                if (!dto) return {};

//                var model = {
//                    'id': dto.id,
//                    'type': dto.type ? utilsService.createItemDtoModel(dto.type) : null,
//                    'level': dto.level ? utilsService.createItemDtoModel(dto.level) : null,
//                    'rating': dto.rating,
//                    'startDate': dto.startDate ? new Date(dto.startDate) : null,
//                    'endDate': dto.endDate ? new Date(dto.endDate) : null,
//                    'isLast': dto.isLast,
//                    'phone': dto.phone,
//                    'registry': dto.registry ? dto.registry.id : null
//                };

//                return model;
//            }

//            factory.healthRiskEditModel = function (dto) {
//                if (!dto) return {};

//                var model = {
//                    'id': dto.id,
//                    'type': dto.type ? utilsService.createItemDtoModel(dto.type) : null,
//                    'level': dto.level ? utilsService.createItemDtoModel(dto.level) : null,
//                    'rating': dto.rating,
//                    'startDate': dto.startDate ? new Date(dto.startDate) : null,
//                    'endDate': dto.endDate ? new Date(dto.endDate) : null,                    
//                    'isLast': dto.isLast,
//                    'phone': dto.phone,
//                    'registry': dto.registry ? dto.registry.id: null                    
//                };

//                return model;
//            }

//            factory.create = function (request) {
//                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.create, 'POST', request);
//            }

//            factory.fetch = function (request) {
//                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.fetch, "GET", request);
//            }
            
//            factory.fetchEssentialData = function (request) {
//                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.fetchEssentialData, "GET", request);
//            }
//            return factory;
//        }]);