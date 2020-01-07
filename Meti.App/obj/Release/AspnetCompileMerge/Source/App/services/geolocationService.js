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
                    'lat': dto.patient.latitude,
                    'lng': dto.patient.longitude
                };

                model.comboText = model.patient.firstname + ' ' + model.patient.surname;
                model.latlng = [parseFloat(model.lat), parseFloat(model.lng)];

                return model;
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
                    'lat': dto.patient.latitude,
                    'lng': dto.patient.longitude
                };

                model.comboText = model.patient.firstname + ' ' + model.patient.surname;
                model.latlng = [parseFloat(model.lat), parseFloat(model.lng)];

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
                //return $sce.trustAsResourceUrl('http://212.189.207.224:3000/d/XFWJlkuiz/test-sempre-vicini?orgId=1&var-processInstanceId='+processInstanceId+'&refresh=5s&kiosk=tv');
                return $sce.trustAsResourceUrl(grafanaUrl);
            }

            return factory;
        }]);