app.controller("loginCtrl",
    ["$scope",
        "accountService",
        "zeusAuthService",
        "toastr",
        "$state",
        "$rootScope",
        "$log",
        "serverRouteMap",
        "responseDispatcherService",
        "configurationService",
        function ($scope, accountService, zeusAuthService, toastr, $state, $rootScope, $log, serverRouteMap, responseDispatcherService, configurationService) {

            //#region Model
            $scope.model = {
                username: null,
                password: null,
                oldPassword: null,
                forgotPasswordFormVisible: false,
                changePasswordFormVisible: false,
                loginFormVisible: true,
                changeNewPassword: null,
                changeConfirmPassword: null,
                isBusy: false,
                isLoginReady: false,
            }
            //#endregion

            //#region Watchs
            var appReadyWatch;
            function initWatchs() {
                appReadyWatch = $scope.$watch(function () {
                    return $rootScope.isAppReady;
                }, function () {
                    $scope.model.isLoginReady = $rootScope.isAppReady;
                }, true);
            }

            //#endregion

            //#region Behaviors

            $rootScope.$on("zeus-firstAccessDetected", function (data) {
                $scope.showChangePasswordForm();
            });

            $scope.showChangePasswordForm = function () {
                $scope.model.changePasswordFormVisible = true;
                $scope.model.forgotPasswordFormVisible = false;
                $scope.model.loginFormVisible = false;
            }

            $scope.showForgotPasswordForm = function () {
                $scope.model.changePasswordFormVisible = false;
                $scope.model.forgotPasswordFormVisible = true;
                $scope.model.loginFormVisible = false;
            }

            $scope.showloginForm = function () {
                $scope.model.changePasswordFormVisible = false;
                $scope.model.forgotPasswordFormVisible = false;
                $scope.model.loginFormVisible = true;
            }

            //Funzione che esegue il login
            $scope.login = function () {

                if (!$scope.loginForm.$valid) {
                    toastr.error($rootScope.validationMessages.formErrors);
                    return;
                }

                //Attivo il busy indicator
                $scope.model.isBusy = true;

                $scope.model.oldPassword = angular.copy($scope.model.password);

                //Eseguo la richiesta di login
                var promise = zeusAuthService.login($scope.model.username, $scope.model.password, serverRouteMap.routeMap.zeus.login);

                //Valuto la risposta
                promise.then(function (response) {

                    configurationService.enableZeusWatchs();

                    //Richiedo il profilo utente al server
                    var profileRequest = accountService.getProfile();

                    //Valuto la risposta
                    profileRequest.then(function (response) {
                        $scope.model.isBusy = true;
                        //Eseguo il bindings dei claims rilevati
                        accountService.initProfile(response.data.claims, response.data.imgProfilePath, true, $scope.model.username, response.data.email);
                        $state.go("shell.home");

                        $rootScope.$broadcast('update:username', $scope.model.username);
                        // $(".backstretch").hide();

                    }, function (error) {
                        toastr.error(error.data);
                        //Loggo eventuali errori
                        $log.error(error.data);
                    }).finally(function () {
                        $scope.model.isBusy = false;
                    });

                }, function (error) {
                    $scope.model.isBusy = false;
                    if (!error) return;

                    //Valuto un eventuale messaggio di errore di login.
                    var status = JSON.parse(error.data.error_description).loginStatus;
                    toastr.error(status)
                    zeusAuthService.generateStatusErrorMessage(status, toastr, false);
                });
            }

            //Funzione che esegue il reset della password
            $scope.resetPassword = function () {

                if (!$scope.forgotPasswordForm.$valid) {
                    toastr.error($rootScope.localization.common.validazioneFallita);
                    return;
                }

                //Attivo il busy indicator
                $scope.model.isBusy = true;

                //Eseguo la richiesta di login
                var promise = zeusAuthService.resetPassword($scope.model.username, serverRouteMap.routeMap.zeus.resetPassword);

                //Valuto la risposta
                promise.then(function (response) {
                    toastr.success("Operazione riuscita. La nuova password è stata inviata nella casella di posta dell'account: " + $scope.model.username);
                    $scope.showloginForm();
                    $scope.model.password = "";
                }, function (error) {
                    //Valuto un eventuale messaggio di errore di login.
                    responseDispatcherService.printErrorMessages(error.data, toastr);
                }).finally(function () {
                    $scope.model.isBusy = false;
                });
            }

            ////Funzione che esegue il cambio della password da temporanea a definitiva
            //$scope.changePassword = function () {

            //    if (!$scope.changePasswordForm.$valid) {
            //        toastr.error($rootScope.localization.common.validazioneFallita);
            //        return;
            //    }

            //    //Attivo il busy indicator
            //    $scope.model.isBusy = true;

            //    //Eseguo la richiesta di login
            //    var promise = zeusAuthService.changePassword(coreRouteMap.routeMap.zeus.changePassword, $scope.model.username, $scope.model.oldPassword, $scope.model.changeNewPassword);

            //    //Valuto la risposta
            //    promise.then(function (response) {
            //        toastr.success($rootScope.localization.common.changePasswordSuccess);
            //        $scope.showloginForm();
            //        $scope.model.password = null;
            //        $scope.model.changeNewPassword = null;
            //        $scope.model.changeConfirmPassword = null;
            //    }, function (error) {
            //        responseDispatcherService.printErrorMessages(error, toastr);
            //    }).finally(function () {
            //        $scope.model.isBusy = false;
            //    });
            //}
            //#endregion

            var constructor = function () {
                $(".backstretch").show();
                initWatchs();
                $scope.model.sedeDefault = $rootScope.sedeDefault;
            }

            constructor();

            $scope.$on("$destroy", function () {
                if (angular.isFunction(appReadyWatch))
                    appReadyWatch();
                //if (angular.isFunction(sedeDefaultWatch))
                //    sedeDefaultWatch();
            });
        }])