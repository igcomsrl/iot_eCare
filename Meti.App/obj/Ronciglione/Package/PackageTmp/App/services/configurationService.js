app.factory("configurationService",
    ["httpService", "$q", "$rootScope", "$state", "zeusAuthService", "toastr", "serverRouteMap", "$sce",
        function (httpService, $q, $rootScope, $state, zeusAuthService, toastr, serverRouteMap, $sce) {
            var factory = {};
            

            factory.configureEssentials = function () {
                $rootScope.isDebug = $("body").attr("data-isDebug") === "true";
                $rootScope.noderedUrl = $sce.trustAsResourceUrl($("body").attr("data-noderedConsoleUrl"));
                //console.log("nodered console url")
                //console.log($rootScope.noderedUrl)
                //console.log($("body").attr("data-noderedConsoleUrl"));
                $rootScope.isAppReady = false;
                
                $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                });
                $rootScope.validationMessages =
                    {
                        'required': "Campo obbligatorio.",
                        'fieldsStarRequired': "I campi marcati con * sono obbligatori.",
                        'maxLength': "Numero massimo di caratteri: ",
                        'formErrors': 'Validazione fallita. Verifica che i campi segnati in rosso siano validi.',
                        'emailFormatInvalid': 'Formato email non valido.',
                        'saveSuccess': 'Salvataggio eseguito con successo.',
                        'deleteSuccess': 'Eliminazione avvenuta con successo.',
                        'commonError': 'Errore, operazione fallita.'
                    }

                $rootScope.siteRootTilde = $("body").attr("data-siteRootTilde");
                $rootScope.alarmFiredPooling = $("body").attr("data-alarmFiredPooling");
                $rootScope.alarmFiredSoundServerPath = $("body").attr("data-alarmFiredSoundServerPath");
                $rootScope.enableAlarmFiredSound = $("body").attr("data-enableAlarmFiredSound");
            }
            

            factory.enableZeusWatchs = function () {
                $rootScope.$on("zeus-login-success", function (event, loginInfo) {
                    $rootScope.isAuth = true;
                    //Idle.watch();
                });
                $rootScope.$on("zeus-logout", function (data) {
                    $state.go("login");
                    //Idle.unwatch();
                    $rootScope.isAuth = false;
                });
                $rootScope.$on("zeus-refreshtoken-success", function (data) {
                });
                $rootScope.$on("zeus-claims-missing", function (data, args) {

                    if ($state.current.url === "" || $state.current.url === "^") {
                        $state.go("login");
                    }
                    toastr.info(args.errorMessage);
                });
                $rootScope.refreshTokenClick = zeusAuthService.refreshToken;
            }

            factory.initConfiguration = function (request) {
                var deferred = $q.defer();

                $rootScope.configuration = null;
                httpService.httpRequest(serverRouteMap.routeMap.configuration.get, "GET", request)
                    .then(function (success) {
                        $rootScope.configuration = success;
                        $rootScope.siteRootTilde = $("body").attr("data-siteRootTilde");
                        $rootScope.$broadcast('configuration:updated');
                        deferred.resolve();
                    }, function (error) {
                        deferred.reject();
                        //Eseguire un log
                    });

                return deferred.promise;
            }


            return factory;
        }]);