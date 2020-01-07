var app = angular.module('app',
    [
        'igcom-framework',
        'igcom-zeus',
        'ui.router',
        'ui.bootstrap',
        'ngTable',
        'kendo.directives',
        'ngAutocomplete',
        'toastr',
        'angular-loading-bar',
        'ngAnimate',
        'ngFileUpload'
    ]);

app.run(['$state', '$rootScope', 'metiClientId', 'configurationService', 'zeusAuthService', 'accountService', 'serverRouteMap', 'toastr',
    function ($state, $rootScope, metiClientId, configurationService, zeusAuthService, accountService, serverRouteMap, toastr) {
    
        //$state.go('shell.registry-index');
        configurationService.configureEssentials();

        zeusAuthService.init(metiClientId, serverRouteMap.routeMap.zeus.login);
       
        //Configurazione applicativa di base
        configurationService.initConfiguration().then(function (success) {
            //Abilito i listeners di zeus
            configurationService.enableZeusWatchs();

            //Se l'utente è autenticato va alla home, altrimenti alla login
            accountService.getProfile().then(function (response) {
                console.log(response)
                var claims = response.data ? response.data.claims : response.claims;
                var imgProfilePath = response.data ? response.data.imgProfilePath : response.imgProfilePath;
                var username = response.data ? response.data.username : response.username;
                var email = response.data ? response.data.email : response.email;

                //Inizializzo il profilo utente
                accountService.initProfile(claims, imgProfilePath, true, username, email);
                $rootScope.$broadcast('update:username', username);
                // controllo se ha i claim per gestire i moduli o per backoffice
                $state.go("shell.home");
            }, function (error) {                
                $state.go("login");
            });
            
        }, function (error) {
            //toastr.error("Errore durante il download della configurazione. Contattare l'assistenza tecnica.");
            zeusAuthService.logout();            
            $state.go("login");
        }).finally(function () {
            $rootScope.isAppReady = true;
        });
    }]);

//Configurazione endpoint dell'applicativo igRsa
app.value('apiEndPoint',
    $("body").attr("data-apiEndPoint")
);
app.value('zeusEndpoint',
    $("body").attr("data-zeusEndpoint")
);
//Configurazione clientId igRsa
app.value("metiClientId", "metiApp");

app.config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        var root = "App/templates/";

        $stateProvider
            .state('shell', {
                abstract: true,
                templateUrl: root + "layout/shell.html",
                data: {
                    requireLogin: true,
                    //requireClaim: [{ "claimEntity": "", "claimNames": ["IsCentraleOperativa"] },
                    //    { "claimEntity": "", "claimNames": ["IsConfiguratore"] },
                    //    { "claimEntity": "", "claimNames": ["IsMedico"] }]
                    ////", ", 
                }
            })
            .state('login', {
                url: '/login',
                templateUrl: root + "layout/login.html",
                controller: 'loginCtrl'
            })
            .state('shell.home', {
                url: '/home',
                views: {
                    'content': {
                        templateUrl: root + 'home.html',
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true
                }
            })
            .state('shell.registry-index', {
                url: '/registry-index',
                views: {
                    'content': {
                        controller: "registry-indexCtrl",
                        templateUrl: root + "registry/index.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsMedico", "IsConfiguratore"] }]
                }
            })
            .state('shell.user-index', {
                url: '/user-index',
                views: {
                    'content': {
                        controller: "user-indexCtrl",
                        templateUrl: root + "user/index.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore"] }]
                }
            })
            .state('shell.role-index', {
                url: '/role-index',
                views: {
                    'content': {
                        controller: "role-indexCtrl",
                        templateUrl: root + "role/index.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore"] }]
                }
            })
            .state('shell.device-index', {
                url: '/device-index',
                views: {
                    'content': {
                        controller: "device-indexCtrl",
                        templateUrl: root + "device/index.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore"] }]
                }
            })
            .state('shell.deviceErrorLog-index', {
                url: '/deviceErrorLog-index',
                views: {
                    'content': {
                        controller: "deviceErrorLog-indexCtrl",
                        templateUrl: root + "deviceErrorLog/index.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore"] }]
                }
            })
            .state('shell.process-index', {
                url: '/process-index',
                views: {
                    'content': {
                        controller: "process-indexCtrl",
                        templateUrl: root + "process/index.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore", "IsMedico"] }]
                }
            })
            .state('shell.process-update', {
                url: '/process-update/:id',
                views: {
                    'content': {
                        controller: "process-updateCtrl",
                        templateUrl: root + "process/update.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore", "IsMedico"] }]
                }
            })
            .state('shell.process-create', {
                url: '/process-create',
                views: {
                    'content': {
                        controller: "process-createCtrl",
                        templateUrl: root + "process/create.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore", "IsMedico"] }]
                }
            })
            .state('shell.processInstance-create', {
                url: '/processInstance-create',
                views: {
                    'content': {
                        controller: "processInstance-createCtrl",
                        templateUrl: root + "processInstance/create.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsMedico", "IsCentraleOperativa"] }]
                }
            })
            .state('shell.processInstance-index', {
                url: '/processInstance-index',
                views: {
                    'content': {
                        controller: "processInstance-indexCtrl",
                        templateUrl: root + "processInstance/index.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsCentraleOperativa", "IsMedico"] }]
                }
            })
            .state('shell.processInstance-update', {
                url: '/processInstance-update/:id',
                views: {
                    'content': {
                        controller: "processInstance-updateCtrl",
                        templateUrl: root + "processInstance/update.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsCentraleOperativa", "IsMedico"] }]
                }
            })
            .state('shell.faqServiceSmartMe', {
                url: '/faqServiceSmartMe',
                views: {
                    'content': {
                        controller: "faqServiceSmartMeCtrl",
                        templateUrl: root + "processInstance/faqServiceSmartMe.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    //requireClaim: [{ "claimEntity": "IndicatorCategory", "claimNames": ["CanRead", "CanCreate"] },
                    //               { "claimEntity": "", "claimNames": ["CanManageBackOffice"] }]
                }
            })
            .state('shell.geolocation', {
                url: '/geolocation',
                views: {
                    'content': {
                        templateUrl: root + "geolocation/map.html",
                        controller: "mapCtrl",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsCentraleOperativa", "IsMedico"] }]
                }
            })
                    
            .state('shell.patientDashboard', {
                url: '/patientDashboard',
                views: {
                    'content': {
                        templateUrl: root + "geolocation/patientDashboard.html",
                        controller: "patientDetailCtrl",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsCentraleOperativa", "IsMedico"] }]
                }
            })
            .state('shell.bugiardino', {
                url: '/bugiardino',
                views: {
                    'content': {
                        templateUrl: root + "processInstance/bugiardino.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsCentraleOperativa", "IsMedico"] }]
                }
            })
            .state('shell.alarmsFired', {
                url: '/alarmsFired',
                views: {
                    'content': {
                        templateUrl: root + "alarmFired/index.html",
                        controller: 'alarmFired-indexCtrl',
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsCentraleOperativa", "IsMedico"] }]
                }
            })
            .state('shell.nodered', {
                url: '/nodered',
                views: {
                    'content': {
                        templateUrl: root + "layout/nodered.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsConfiguratore"] }]
                }
            })
            .state('shell.artificialIntelligenzeNodered', {
                url: '/artificialIntelligenzeNodered',
                views: {
                    'content': {
                        templateUrl: root + "layout/artificialIntelligenceNodered.html",
                    }, 'header': {}, 'sidebar': {}, 'footer': {}
                },
                data: {
                    requireLogin: true,
                    requireClaim: [{ "claimEntity": "", "claimNames": ["IsMedico"] }]
                }
            })
        
            ;

        $urlRouterProvider.otherwise(function ($injector) {
        });
    }]);

//Fix: https://github.com/chieffancypants/angular-loading-bar/pull/50
app.factory('handlerInterceptorFactory', [function () {
    return {
        'response': function (response) {
            return null; // <-- this is not returning the config object
            return response || $q.when(response);
        }
    }
}]);