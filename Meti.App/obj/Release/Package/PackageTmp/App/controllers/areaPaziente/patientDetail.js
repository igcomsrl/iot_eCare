app.controller("patientDetailCtrl",
    ["$scope",
        "accountService",
        "toastr",
        "responseDispatcherService",
        'processInstanceService',
        "geolocationService",
        function ($scope, accountService, toastr, responseDispatcherService, processInstanceService, geolocationService) {

            //#region Model
            $scope.isBusy= false;
            $scope.processInstanceList = [];
            $scope.processInstanceModel = null;
            $scope.getPatientDashboardUrl = geolocationService.getPatientDashboardUrl;
            //#endregion

            //#region Behaviors
            
            //Funzione che esegue il reset della password
            var getProcessInstance = function () {
                
                var email = accountService.email;
                
                //Attivo il busy indicator
                $scope.isBusy = true;

                //Eseguo la richiesta di login
                var promise = processInstanceService.getByRegistryEmail({ 'email': email });

                //Valuto la risposta
                promise.then(function (response) {

                    angular.forEach(response.data, function (data) {
                        var model = processInstanceService.processInstanceUpdateModel(data);
                        $scope.processInstanceList.push(model);
                    })
                    $scope.processInstanceModel = $scope.processInstanceList[0];

                }, function (error) {
                    //Valuto un eventuale messaggio di errore di login.
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                }).finally(function () {
                    $scope.isBusy = false;
                });
            }

            //#endregion

            var constructor = function () {
                getProcessInstance();
            }

            constructor();

            $scope.$on("$destroy", function () {
            });
        }])