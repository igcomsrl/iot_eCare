app.factory('healthRiskService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        'utilsService',
        function (utilsService, serverRouteMap, httpService, utilsService) {
            var factory = {};

            factory.healthRiskIndexModel = function(dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'type': dto.type ? utilsService.createItemDtoModel(dto.type) : null,
                    'level': dto.level ? utilsService.createItemDtoModel(dto.level) : null,
                    'rating': dto.rating,
                    'startDate': dto.startDate ? new Date(dto.startDate) : null,
                    'endDate': dto.endDate ? new Date(dto.endDate) : null,
                    //'isLast': dto.isLast,
                    'registry': dto.registry ? dto.registry.id : null
                };

                return model;
            }

            factory.healthRiskDetailModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'type': dto.type ? utilsService.createItemDtoModel(dto.type) : null,
                    'level': dto.level ? utilsService.createItemDtoModel(dto.level) : null,
                    'rating': dto.rating,
                    'startDate': dto.startDate,
                    'endDate': dto.endDate,   
                    //'isLast': dto.isLast,
                    'registry': dto.registry ? dto.registry.id : null
                };

                return model;
            }

            factory.healthRiskEditModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'id': dto.id,
                    'type': dto.type ? utilsService.createItemDtoModel(dto.type) : null,
                    'level': dto.level ? utilsService.createItemDtoModel(dto.level) : null,
                    'rating': dto.rating,
                    //'startDate': dto.startDate ? new Date(dto.startDate) : null,
                    //'endDate': dto.endDate ? new Date(dto.endDate) : null,                    
                    'startDate': dto.startDate,
                    'endDate': dto.endDate,   
                    'isLast': dto.isLast,
                    'registry': dto.registry ? dto.registry.id: null                    
                };

                return model;
            }

            //Esegue la media aritmetica tra i livelli. (Campo level)
            factory.calculateSeverityIndex = function (models) {
                if(!models || models.count <1)
                    return 0;

                //Filtro la sorgente dati
                models = filterHealthRisksByDistinctAndDateStartLast(models);
                if (!models)
                    return 0;

                //Estraggo il numero di elementi da elaborare
                var count = models.length;

                //Definisco il contatore che conterrà la somma dei livelli presenti nella sorgente dati
                var levelSummed = 0;

                //Eseguo il conteggio dei livelli
                angular.forEach(models, function (item) {
                    levelSummed = levelSummed + item.level.id;
                })

                //Calcolo il valore medio
                var medium = levelSummed / count;

                //Considero solo gli ultimi due decimali
                if (medium)
                    medium = medium.toFixed(2);

                if (isNaN(medium))
                    medium = 0;

                //Ritorno il risultato
                return medium;
            }

            //Conta quanti sono gli oggetti con un valore (campo level) maggiore o uguale a 3
            factory.calculateComorbidityIndex = function (models) {
                if (!models || models.count < 1)
                    return 0;

                // //Filtro la sorgente dati
                //models = filterHealthRisksByDistinctAndDateStartLast(models);
                //if (!models)
                //    return 0;

                //DIchiaro il numero modelli che andrò a conteggiare
                var count = 0;

                //Conto i modelli che hanno un valore maggiore o uguale a 3
                angular.forEach(models, function (item) {
                    if (item.level.id >= 3)
                        count = count + 1;
                })

                //Ritorno i risultati
                return count;
            }

            //Esegue una distinct ed considera solo le ultime date di inizio
            var filterHealthRisksByDistinctAndDateStartLast = function (models) {
                if (!models || models.count < 1)
                    return null;
         
                //Distinct
                //var modelsDistincted = Enumerable.from(models).orderByDescending("$.startDate").distinct(function (x) { return x.type.id; }).select(function (a) { return a; }).toArray();
                var modelsDistincted = Enumerable.from(models).where(function (x) { return x.type.id != 14; }).select(function (a) { return a; }).toArray();

                return modelsDistincted;
            }

            factory.healthRisks = [];

            factory.create = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.create, 'POST', request);
            }

            factory.fetch = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.fetch, "GET", request);
            }

            factory.delete = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.deleteItem, "POST", request);
            }

            factory.fetchEssentialData = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.fetchEssentialData, "GET", request);
            }

            factory.fetchByProcessInstanceIds = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.healthRisk.fetchByProcessInstanceIds, "POST", request);
            }

            factory.registryId;

            return factory;
        }]);