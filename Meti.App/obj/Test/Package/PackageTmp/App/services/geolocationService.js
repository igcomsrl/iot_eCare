app.factory('geolocationService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        'registryService',
        'processService',
        '$sce',
        function (utilsService, serverRouteMap, httpService, registryService, processService, $sce) {
            var factory = {};
            
            factory.geoLocationModel = function (dto) {
                if (!dto) return {};

                var model = {
                    'doctors': dto.doctors ? Enumerable.from(dto.doctors).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'referencePersons': dto.referencePersons ? Enumerable.from(dto.referencePersons).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'processInstanceId': dto.processInstanceId,
                    'process': dto.process ? processService.processUpdateModel(dto.process) : '',
                    'isEnabled': dto.isEnabled || true,
                    'patient': dto.patient ? registryService.registryEditModel(dto.patient) : '',
                    'lat': dto.patient.latitude ? dto.patient.latitude.replace(",", "."): '',
                    'lng': dto.patient.longitude ? dto.patient.longitude.replace(",", ".") : '',
                    'latLast': dto.patient.latitudeLast ? dto.patient.latitudeLast.replace(",", ".") : '',
                    'lngLast': dto.patient.longitudeLast ? dto.patient.longitudeLast.replace(",", ".") : '',
                    'latlngLast': ''
                };

                model.comboText = model.patient.firstname + ' ' + model.patient.surname;
                model.latlng = [parseFloat(model.lat), parseFloat(model.lng)];
                if (model.latLast && model.lngLast)
                    model.latlngLast = [parseFloat(model.latLast), parseFloat(model.lngLast)];

                return model;
            }

            //Verifica se il modello contiene almeno una coordinata valida tra indirizzo di residenza oppure aggiornamenti remoti
            factory.hasValidCoordinateInMap = function (model) {
                if (!model)
                    return false;

                if (model.lat && model.lng)
                    return true;
                else if (model.latLast && lngLast)
                    return true;
                else
                    return false;
            }

            factory.geoLocationSwiftModel = function (dto) {
                if (!dto) return {};

                var model = {
                    //'doctors': dto.doctors ? Enumerable.from(dto.doctors).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    //'referencePersons': dto.referencePersons ? Enumerable.from(dto.referencePersons).where(function (x) { return registryService.registryEditModel(x) }).toArray() : '',
                    'processInstanceId': dto.processInstanceId,
                    //'process': dto.process ? processService.processUpdateModel(dto.process) : '',
                    'isEnabled': dto.isEnabled || true,
                    'patient': dto.patient ? registryService.registryEditModel(dto.patient) : '',
                    'lat': dto.patient.latitude ? dto.patient.latitude.replace(",", ".") : '',
                    'lng': dto.patient.longitude ? dto.patient.longitude.replace(",", ".") : '',
                    'latLast': dto.patient.latitudeLast ? dto.patient.latitudeLast.replace(",", ".") : '',
                    'lngLast': dto.patient.longitudeLast ? dto.patient.longitudeLast.replace(",", ".") : '',
                    'latlngLast': ''
                };

                model.comboText = model.patient.firstname + ' ' + model.patient.surname;
                model.latlng = [parseFloat(model.lat), parseFloat(model.lng)];
                if (model.latLast && model.lngLast)
                    model.latlngLast = [parseFloat(model.latLast), parseFloat(model.lngLast)];

                return model;
            }

            factory.fetchProcessInstanceGeo = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.fetchProcessInstanceGeo, "GET", request);
            }

            factory.getProcessInstanceGeo = function (request) {
                return httpService.httpRequest(serverRouteMap.routeMap.processInstance.getProcessInstanceGeo, "GET", request);
            }
            
            factory.getPatientDashboardUrl = function (processInstanceId) {
                var grafanaUrl = $("body").attr("data-grafanaConsoleUrl");
                var grafanaConsoleRefreshTime = $("body").attr("data-grafanaConsoleRefreshTime");
                grafanaUrl = grafanaUrl + "?orgId=1";
                grafanaUrl = grafanaUrl + "&var-processInstanceId=" + processInstanceId;
                grafanaUrl = grafanaUrl + "&refresh="+grafanaConsoleRefreshTime+"s&kiosk";                
                //grafanaUrl = grafanaUrl + "&refresh=5s&kiosk";                
                //return $sce.trustAsResourceUrl('http://212.189.207.224:3000/d/XFWJlkuiz/test-sempre-vicini?orgId=1&var-processInstanceId='+processInstanceId+'&refresh=5s&kiosk=tv');
                return $sce.trustAsResourceUrl(grafanaUrl);
            }

            return factory;
        }]);