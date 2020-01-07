app.controller('faqServiceSmartMeCtrl',
    ['$scope',
        '$log',
        "responseDispatcherService",
        '$q',
        'toastr',
        '$rootScope',
        'parameterService',
        'deviceErrorLogService',
        'alarmFiredService',
        function ($scope, $log, responseDispatcherService, $q, toastr, $rootScope, parameterService, deviceErrorLogService, alarmFiredService) {
            //#region Model
            $scope.uuidDeviceRequest = angular.toJson({ 'uuid': '34-81-F4-31-DA-4A'});
            $scope.uuidDeviceResponse = null;
            $scope.logRequest = angular.toJson({ 'Error': 'Test log', 'DeviceId':'', 'ProcessInstanceId': '' });
            $scope.logResponse = null;
            $scope.alarmFiredRequest = angular.toJson({
                //"temperature": 36.7,
                //"weight": 71,
                "result": "Temperatura: 36.7, Peso 71",
                "AlarmColor": 1,
                "ParameterId": "3e9c66cd-2e90-4f98-afed-aa0900b2dc20",
                "ProcessInstanceId": "f22c7811-e55c-4412-9d2e-aa0700c555ce",
                "alarmId": "4fc6d63d-01c3-4298-9083-aa0900b2dc26"
            });
            $scope.alarmFiredResponse = null;
            $scope.isCtrlReady = false;
            $scope.isUuidDeviceBusy = false;
            $scope.postLogBusy= false;
            //#endregion

            //#region Behaviors
            var clear = function () {
                $scope.uuidDeviceRequest = null;
                $scope.uuidDeviceResponse = null;
            }

            $scope.getParameterByDevice = function () {

                $scope.isUuidDeviceBusy = true;

                var request = angular.fromJson($scope.uuidDeviceRequest);

                var promise = parameterService.getByDevice(request);

                promise.then(function (response) {
                    var data = response.data;
                    $scope.uuidDeviceResponse = angular.toJson(data);
                    toastr.success("Richiesta Eseguita");
                }, function (error) {
                    var data = error.data.data;
                    var vResults = data.validationResults;
                    toastr.error("Richiesta Fallita");
                    $log.error("Errore in processInstance-createCtrl.create: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.isUuidDeviceBusy = false;
                });
            }

            $scope.postLog = function () {

                $scope.postLogBusy = true;

                var request = angular.fromJson($scope.logRequest);

                var promise = deviceErrorLogService.log(request);

                promise.then(function (response) {
                    var data = response.data;
                    $scope.logResponse = angular.toJson(data);
                    toastr.success("Richiesta Eseguita");
                }, function (error) {
                    var data = error.data.data;
                    var vResults = data.validationResults;
                    toastr.error("Richiesta Fallita");
                    $log.error("Errore in processInstance-createCtrl.create: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.postLogBusy = false;
                });
            }

            $scope.createAlarmFired = function () {

                $scope.createAlarmFiredBusy = true;

                var request = angular.fromJson($scope.alarmFiredRequest);

                var promise = alarmFiredService.create(request);

                promise.then(function (response) {
                    var data = response.data;
                    $scope.alarmFiredResponse = angular.toJson(data);
                    toastr.success("Richiesta Eseguita");
                }, function (error) {
                    var data = error.data.data;
                    var vResults = data.validationResults;
                    toastr.error("Richiesta Fallita");
                    $log.error("Errore in processInstance-createCtrl.create: " + angular.fromJson(error));
                }).finally(function () {
                    $scope.createAlarmFiredBusy = false;
                });
            }
            //#endregion

            var constructor = function () {
                $scope.isCtrlReady = true;                
            }

            constructor();

            $scope.$on("$destroy", function () {
                clear();
            });
        }]);