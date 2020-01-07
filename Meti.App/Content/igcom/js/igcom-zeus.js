var zeusApp = angular.module("igcom-zeus", []);

zeusApp.config(["$httpProvider",function ($httpProvider) {
    $httpProvider.interceptors.push('zeusAuthInterceptorService');
}]);

zeusApp.factory("zeusAuthService",
    ["$log", "$window", "$q", "$http", "$rootScope",
        function ($log, $window, $q, $http, $rootScope) {
            var clientId = "";
            var tokenEndPoint = "";
            var USER_INFO_LOCAL_STORAGE_KEY = "AuthUserInfo";

            var createUserInfoModel = function (expires, issued, accessToken, clientId, expiresIn, refreshToken, tokenType, username, sign) {
                return {
                    expires: expires,
                    issued: issued,
                    accessToken: accessToken,
                    clientId: clientId,
                    expiresIn: expiresIn,
                    refreshToken: refreshToken,
                    tokenType: tokenType,
                    username: username,
                    sign: sign
                }
            }

            var loginStatus = {
                success: 0,
                credentialsInvalid: 1,
                userIsDisable: 2,
                passwordExpired: 3,
                userIsBanned: 4,
                loginAttemptsExceeded: 5,
                usernameInvalid: 6,
                passwordInvalid: 7,
                firstAccessDetected: 8
            }

            var changePasswordStatus = {
                success: 0,
                fail: 1,
                oldPasswordInvalid: 2,
                userNotFound: 3,
                userBanned: 4,
                userDisabled: 5
            }

            var deleteAuthUserInfo = function () {
                $window.localStorage.removeItem(USER_INFO_LOCAL_STORAGE_KEY);
            }

            var setClientId = function (value) {
                clientId = value;
            }

            var getClientId = function () {
                return clientId;
            }

            var setUserInfo = function (val) {
                $window.localStorage[USER_INFO_LOCAL_STORAGE_KEY] = angular.toJson(val);
            }
            var getUserInfo = function () {
                return angular.fromJson($window.localStorage[USER_INFO_LOCAL_STORAGE_KEY]);
            }

            var getUsername = function () {
                var user = angular.fromJson($window.localStorage[USER_INFO_LOCAL_STORAGE_KEY]);
                return user ? user.username : null;
            }

            var init = function (clientIdParam, tokenEndPointParam, zeusLabelParams) {
                clientId = clientIdParam;
                tokenEndPoint = tokenEndPointParam;
                loginTrack();
                claimsTrack();
                if (zeusLabelParams)
                    zeusLabels = zeusLabelParams;
            }

            var zeusLabels = {
                "error": "Errore",
                "warning": "Attenzione",
                "credentialsInvalid": "Verifica che i dati di autenticazione siano corretti.",
                "firstAccessDetected": "In seguito al primo accesso è necessario cambiare la password temporanea con una definitiva.",
                "loginAttemptsExceeded": "Troppi accessi errati. Contattare l'assistenza per eseguire lo sblocco dell'account.",
                "passwordExpired": "Password scaduta, si prega di impostarla nuovamente.",
                "passwordInvalid": "Password non riconosciuta.",
                "userIsBanned": "Utente bannato dal sistema. Contattare l'assistenza per eseguire lo sblocco dell'account.",
                "userIsDisable": "Utente disabilitato dal sistema. Contattare l'assistenza per eseguire lo sblocco dell'account.",
                "usernameInvalid": "Username non riconosciuto.",
                "insufficientPermission": "Non possiedi i permessi necessari per poter effettuare questa operazione."
            }

            //Set delle label localizzate
            var setZeusLabels = function (labels) {
                zeusLabels = labels;
            }

            //In base al messaggio di errore del server, ritorna un risultato
            var generateStatusErrorMessage = function (errorStatus, toastr, showLogs) {
                var labelErrorTitle = zeusLabels.error;
                var labelErrorDescription = "";

                switch (errorStatus) {
                    case this.loginStatus.credentialsInvalid:
                        labelErrorDescription = zeusLabels.credentialsInvalid;
                        if (toastr)
                            toastr.error(labelErrorDescription, labelErrorTitle);
                        break;
                    case this.loginStatus.firstAccessDetected:
                        labelErrorTitle = zeusLabels.warning;
                        labelErrorDescription = zeusLabels.firstAccessDetected;
                        $rootScope.$broadcast("zeus-firstAccessDetected");
                        if (toastr)
                            toastr.warning(labelErrorDescription, labelErrorTitle);
                        break;
                    case this.loginStatus.loginAttemptsExceeded:
                        labelErrorDescription = zeusLabels.loginAttemptsExceeded;
                        if (toastr)
                            toastr.error(labelErrorDescription, labelErrorTitle);
                        break;

                    case this.loginStatus.passwordExpired:
                        labelErrorDescription = zeusLabels.passwordExpired;
                        if (toastr)
                            toastr.error(labelErrorDescription, labelErrorTitle);
                        break;

                    case this.loginStatus.passwordInvalid:
                        labelErrorDescription = zeusLabels.passwordInvalid;
                        if (toastr)
                            toastr.error(labelErrorDescription, labelErrorTitle);
                        break;

                    case this.loginStatus.userIsBanned:
                        labelErrorDescription = zeusLabels.userIsBanned;
                        if (toastr)
                            toastr.error(labelErrorDescription, labelErrorTitle);
                        break;

                    case this.loginStatus.userIsDisable:
                        labelErrorDescription = zeusLabels.userIsDisable;
                        if (toastr)
                            toastr.error(labelErrorDescription, labelErrorTitle);
                        break;

                    case this.loginStatus.usernameInvalid:
                        labelErrorDescription = zeusLabels.usernameInvalid;
                        if (toastr)
                            toastr.error(labelErrorDescription, labelErrorTitle);
                        break;
                }

                //if (toastr)
                //    toastr.error(labelErrorDescription, labelErrorTitle);

                if (showLogs || !toastr)
                    $log.error(labelErrorTitle + " " + labelErrorDescription);
            }

            //Funzione che esegue il login utente
            var login = function (username, password, tokenEndPointParam) {
                //Compongo la richiesta
                var requestUri = "grant_type=password&username=" + username + "&password=" + password + "&client_id=" + getClientId();

                //Definisco l'header della richiesta
                var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };

                var request = {
                    method: "POST",
                    url: tokenEndPointParam || tokenEndPoint,
                    headers: headers,
                    data: requestUri
                };

                var deferred = $q.defer();

                //Eseguo la richiesta
                var promise = $http(request);

                //Valuto la risposta
                promise.then(function (response) {
                    var response = response.data;
                    var userInfoModel = createUserInfoModel(response[".expires"], response[".issued"], response.access_token, response["as:client_id"], response.token_type,
                        response.refresh_token, response.token_type, response.userName, response.sign);
                    setUserInfo(userInfoModel);
                    deferred.resolve(userInfoModel);
                    $rootScope.$broadcast("zeus-login-success", response);
                    $log.info('Zeus - Login eseguito - Response: ', response);
                }, function(error) {
                    deferred.reject(error);
                    $rootScope.$broadcast("zeus-login-error", error);
                    $log.error('Zeus - Login fallito - Errori: ', error);
                });

                return deferred.promise;
            }

            //Recupera l'utente tramite username
            var getUser = function (username, url) {
                if (!url) throw new Error();

                //Compongo la richiesta
                var request = {
                    method: "GET",
                    url: url,
                    params: { "username": username }
                };

                var deferred = $q.defer();

                //Eseguo la richiesta
                var promise = $http(request);

                //Valuto la risposta
                promise.success(function (response) {
                    deferred.resolve(response);
                    $log.info('Zeus - Get user eseguito - Response: ', response);
                }).error(function (error) {
                    deferred.reject(error);
                    $log.info('Zeus - Get user fallito - Response: ', error);
                });

                return deferred.promise;
            }

            //Esegue il cambio di password
            var changePassword = function (url, username, oldPassword, newPassword) {
                if (!url) throw new Error();

                var deferred = $q.defer();

                //Compongo la richiesta
                var request = {
                    method: "POST",
                    url: url,
                    data: {
                        "Username": username,
                        "OldPassword": oldPassword,
                        "NewPassword": newPassword
                    }
                };

                //Eseguo la richiesta
                var promise = $http(request);

                //Valuto la risposta
                promise.success(function (response) {
                    deferred.resolve(response);
                    $log.info('Zeus - changePassword eseguito - Response: ', response);
                }).error(function (error) {
                    deferred.reject(error);
                    $log.info('Zeus - changePassword fallito - Response: ', error);
                });

                return deferred.promise;
            }

            //Esegue il reset della password
            var resetPassword = function (username, url) {
                if (!url) throw new Error();

                //Compongo la richiesta
                var request = {
                    method: "POST",
                    url: url,
                    data: { "username": username }
                };

                var deferred = $q.defer();

                //Eseguo la richiesta
                var promise = $http(request);

                //Valuto la risposta
                promise.then(function (response) {
                    deferred.resolve(response);
                    $log.info('Zeus - ResetPassword eseguito - Response: ', response);
                }, function (error) {
                    deferred.reject(error);
                    $log.info('Zeus - ResetPassword fallito - Response: ', error);                    
                    
                });

                return deferred.promise;
            }

            //Esegue il refresh del token
            var refreshToken = function (tokenEndPointParam) {
                var refresh_token = getUserInfo().refreshToken;

                //Compongo la richiesta
                var requestUri = "grant_type=refresh_token&refresh_token=" + refresh_token + "&client_id=" + getClientId();

                //Definisco l'header della richiesta
                var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };

                var request = {
                    method: "POST",
                    url: tokenEndPointParam || tokenEndPoint,
                    headers: headers,
                    data: requestUri
                };

                var deferred = $q.defer();

                //Eseguo la richiesta
                var promise = $http(request);

                //Valuto la risposta
                promise.success(function (response) {
                    var userInfoModel = createUserInfoModel(response[".expires"], response[".issued"], response.access_token, response["as:client_id"], response.token_type,
                        response.refresh_token, response.token_type, response.userName);
                    setUserInfo(userInfoModel);
                    deferred.resolve(userInfoModel);
                    $log.info('Zeus - Login eseguito - Response: ', response);
                    $rootScope.$broadcast("zeus-refreshtoken-success", response);
                }).error(function (error) {
                    deferred.reject(error);
                    $log.error('Zeus - Login fallito - Errori: ', error);
                    $rootScope.$broadcast("zeus-refreshtoken-error", error);
                });

                return deferred.promise;
            }

            //Esegue il logout
            var logout = function () {
                var deferred = $q.defer();
                deleteAuthUserInfo();
                deferred.resolve();
                $rootScope.$broadcast("zeus-logout");
                return deferred.promise;
            }

            var registration = function (url, request) {
                //request.callback = "JSON_CALLBACK"; //callback=JSON_CALLBACK

                //var config = {
                //    params: request
                //};

                //var deferred = $q.defer();

                ////Eseguo la richiesta
                ////var promise = $http(requestModel);
                //var promise = $http.jsonp(url, config);

                //var requestModel = {
                //    method: "POST",
                //    url: url,
                //    data: request
                //};

                var deferred = $q.defer();

                //Eseguo la richiesta
                //var promise = $http(requestModel);

                ////Valuto la risposta
                //promise.success(function (response) {
                //    var userInfoModel = createUserInfoModel("", "", response.access_token, response.client_id,
                //        response.refresh_token, response.token_type, response.username);
                //    setUserInfo(userInfoModel);
                //    deferred.resolve(userInfoModel);
                //    $log.info('Zeus - Login eseguito - Errori: ', error);
                //}).error(function (error) {
                //    deferred.reject(error);
                //    $log.error('Zeus - Login fallito - Errori: ', error);
                //});

                return deferred.promise;
            }

            var claimsTrack = function () {
                $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
                    //Esempio: requireClaim = {"claimEntity": "Clinic", "claimNames"[]}
                    var requireClaim = null;

                    if (toState.data !== undefined && toState.data !== null)
                        requireClaim = toState.data.requireClaim;

                    //Verifico che l'utente sia autorizzato per accedere ai contenuti del controller
                    if (requireClaim && $rootScope.isAuth) {
                        //SE NON POSSIEDE I CLAIMS
                        //if(!accountService.hasClaim(requireClaim.claimEntity, requireClaim.claimNames)) {
                        if (!hasClaim(requireClaim)) {
                            event.preventDefault();
                            $rootScope.$broadcast("zeus-claims-missing", { errorMessage: zeusLabels.insufficientPermission });
                            if ($rootScope.claims && $rootScope.claims.length === 0) {
                                $rootScope.$broadcast("zeus-logout");
                            }
                        }
                    }
                });
            }

            var loginTrack = function () {
                $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
                    var requireLogin = false;

                    if (toState.data !== undefined && toState.data !== null)
                        requireLogin = toState.data.requireLogin;

                    //Verifico che l'utente sia autorizzato per accedere ai contenuti del controller
                    if (requireLogin && requireLogin === true) {
                        if ($rootScope === false || $rootScope.isAuth === "false" || $rootScope.isAuth === false || $rootScope.isAuth === undefined) {
                            event.preventDefault();
                            $rootScope.$broadcast("zeus-login-required");
                        }
                    }
                });
            }

            var hasClaim = function (claimsParam) {
                //Valido gli argomenti
                if (!$rootScope.claims) return false;
                if (!claimsParam) return false;

                //Imposto il risultato di ritorno
                var result = false;

                //Verifica che i claims di una entity siano posseduti
                function checkClaim(claimType, claimValues) {
                    var result = false;
                    angular.forEach(claimValues, function (claimName) {
                        angular.forEach($rootScope.claims, function (claimProfile) {
                            if (claimProfile.claimType === claimType && claimProfile.claimValue === claimName)
                                result = true;
                        });
                    });
                    return result;
                }

                angular.forEach(claimsParam, function (claimParam) {
                    //result = checkClaim(claimParam.claimEntity, claimParam.claimNames);
                    var claimType = claimParam.claimType || claimParam.type || claimParam.claimEntity || "";
                    var claimValues = claimParam.claimValues || claimParam.values || claimParam.claimNames;

                    result = checkClaim(claimType, claimValues);
                    if (!result)
                        return;
                });

                //Ritorno il risultato
                return result;
            }

            var claimModel = function (dto) {
                if (!dto) return null;

                return {
                    claimType: dto.m_type || dto.ValueType || dto.type,
                    claimValue: dto.m_value || dto.Value || dto.value
                }
            }

            return {
                logout: logout,
                login: login,
                registration: registration,
                getClientId: getClientId,
                setClientId: setClientId,
                getUserInfo: getUserInfo,
                setUserInfo: setUserInfo,
                USER_INFO_LOCAL_STORAGE_KEY: USER_INFO_LOCAL_STORAGE_KEY,
                refreshToken: refreshToken,
                init: init,
                deleteAuthUserInfo: deleteAuthUserInfo,
                loginStatus: loginStatus,
                changePasswordStatus: changePasswordStatus,
                getUser: getUser,
                generateStatusErrorMessage: generateStatusErrorMessage,
                zeusLabels: zeusLabels,
                setZeusLabels: setZeusLabels,
                resetPassword: resetPassword,
                changePassword: changePassword,
                claimsTrack: claimsTrack,
                hasClaim: hasClaim,
                claimModel: claimModel,
                getUsername: getUsername,
                loginTrack: loginTrack
            }
        }]);

zeusApp.factory('zeusAuthInterceptorService',
    ['$q',
        '$injector',
        '$window',
        "$log",
        "$rootScope",
        function ($q, $injector, $window, $log, $rootScope) {
            var isRefreshingToken = false;
            var request = function (config) {
                config.headers = config.headers || {};
                var zeusAuthService = $injector.get('zeusAuthService');
                var authData = zeusAuthService.getUserInfo();
                if (authData) {
                    config.headers.Authorization = 'Bearer ' + authData.accessToken;
                }

                return config;
            }

            var responseError = function (rejection) {
                if (rejection.status === 401) {
                    $log.info("Zeus Unauthorized: " + angular.toJson(rejection));
                    $rootScope.$broadcast("zeus-unauthorized", rejection);

                    var zeusAuthService = $injector.get('zeusAuthService');
                    var authData = zeusAuthService.getUserInfo();

                    if (authData && !isRefreshingToken) {
                        isRefreshingToken = true;
                        zeusAuthService.refreshToken().then(function (success) {
                            return $q.reject(rejection);
                        }, function (error) {
                            zeusAuthService.logout();
                            return $q.reject(rejection);
                        }).finally(function () {
                            isRefreshingToken = false;
                        });
                    }
                    else if (!isRefreshingToken) {
                        $log.info("Sessione di lavoro scaduta, riesegui il login.");
                        zeusAuthService.logout();
                    }
                }
                return $q.reject(rejection);
            }

            return {
                request: request,
                responseError: responseError
            };
        }]);