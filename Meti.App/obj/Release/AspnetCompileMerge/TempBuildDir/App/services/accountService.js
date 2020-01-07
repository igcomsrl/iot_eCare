app.factory("accountService",
    ["httpService", "serverRouteMap", "$q", "$rootScope", "zeusAuthService",
        function (httpService, coreRouteMap, $q, $rootScope, zeusAuthService) {
            var factory = {};

            factory.username = "";
            factory.email= "";
            factory.picturePath = "";

            factory.getSign = function () {
                return factory.sign;
            }

            factory.getProfile = function (request) {
                var authData = zeusAuthService.getUserInfo();
                if (!authData)
                    return $q.reject();
                else
                    return httpService.httpRequest(coreRouteMap.routeMap.account.getProfile, "GET", request);
            }

            //Verifico se il profilo utente possiede il claim richiesto
            factory.hasClaim = function (claimsParam) {
                //Valido gli argomenti
                if (!$rootScope.claims) return false;
                if (!claimsParam) return false;

                //Imposto il risultato di ritorno
                var result = false;

                //Verifica che i claims di una entity siano posseduti
                function checkClaim(claimEntity, claimNames) {
                    var result = false;
                    angular.forEach(claimNames, function (claimName) {
                        angular.forEach($rootScope.claims, function (claimProfile) {
                            if (claimProfile.EntityName === claimEntity && claimProfile.Name === claimName)
                                result = true;
                        });
                    });
                    return result;
                }

                angular.forEach(claimsParam, function (claimParam) {
                    result = checkClaim(claimParam.claimEntity, claimParam.claimNames);
                    if (!result)
                        return;
                });

                //Ritorno il risultato
                return result;
            }

            factory.hasManageConfigurazione = function () {
                return zeusAuthService.hasClaim([{ "claimType": "", "claimValues": ["ManageConfigurazione"] }]);
            }

            factory.hasManageMonitoraggio = function () {
                return zeusAuthService.hasClaim([{ "claimType": "", "claimValues": ["ManageMonitoraggio"] }]);
            }

            factory.hasManageAreaPaziente = function () {
                return zeusAuthService.hasClaim([{ "claimType": "", "claimValues": ["ManageAreaPaziente"] }]);
            }

            factory.initProfile = function (claims, profilePath, isAuth, name, email) {
                var userInfo = zeusAuthService.getUserInfo();

                $rootScope.isAuth = isAuth || false;
                $rootScope.claims = claims ? Enumerable.from(claims).select(function (x) { return zeusAuthService.claimModel(x) }).toArray() : [];
                factory.picturePath = profilePath || "";
                factory.username = name || "";
                $rootScope.username = factory.username;
                factory.email = email || "";
                $rootScope.$broadcast('userProfile:updated');
            }

            return factory;
        }]);