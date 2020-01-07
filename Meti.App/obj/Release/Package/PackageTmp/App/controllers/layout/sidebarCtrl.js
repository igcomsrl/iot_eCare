app.controller('sidebarCtrl',
    ['$scope',
    '$log',
    'alarmFiredService',
    '$rootScope',
        '$interval',
    'accountService',
        function ($scope, $log, alarmFiredService, $rootScope, $interval, accountService) {
            $scope.hasManageConfigurazione = accountService.hasManageConfigurazione();
            $scope.hasManageMonitoraggio = accountService.hasManageMonitoraggio();
            $scope.hasManageAreaPaziente = accountService.hasManageAreaPaziente();

            $scope.audioAlert = null;
            $scope.interval = null;

            $scope.playAudioAlert = function () {
                if (!$scope.audioAlert || $scope.audioAlert.paused)
                {
                    $scope.audioAlert = new Audio($rootScope.alarmFiredSoundServerPath);
                    $scope.audioAlert.loop = true;
                    $scope.audioAlert.play();
                }   
            };

            $scope.stopAudioAlert = function () { 
                if ($scope.audioAlert) {
                    $scope.audioAlert.pause();
                    $scope.audioAlert.currentTime = 0;
                }
            };

            $rootScope.$on("zeus-logout", function (data) {
                $scope.stopAudioAlert();
                $interval.cancel($scope.interval);
            });

            var fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                
                var request =
                {
                    'isActive': true,
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                alarmFiredService.fetch(request, true)
                    .then(function (response) {
                        var data = response.data;

                        var alarmFiredList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = alarmFiredService.alarmFiredDetailModel(dto);
                            alarmFiredList.push(model);
                        });

                        var alarmFiredGreenList = Enumerable.from(alarmFiredList).where(function (x) { return x.alarmColor.id == 1 && x.isActive == true }).toArray();
                        var alarmFiredYellowList = Enumerable.from(alarmFiredList).where(function (x) { return x.alarmColor.id == 2 && x.isActive == true }).toArray();
                        var alarmFiredRedList = Enumerable.from(alarmFiredList).where(function (x) { return x.alarmColor.id == 3 && x.isActive == true}).toArray();

                        alarmFiredService.alarmFiredGreenCount = alarmFiredGreenList.length;
                        alarmFiredService.alarmFiredYellowCount = alarmFiredYellowList.length;
                        alarmFiredService.alarmFiredRedCount = alarmFiredRedList.length;

                        $scope.alarmFiredGreenCount = alarmFiredService.alarmFiredGreenCount;
                        $scope.alarmFiredYellowCount = alarmFiredService.alarmFiredYellowCount;
                        $scope.alarmFiredRedCount = alarmFiredService.alarmFiredRedCount;

                        if ($scope.alarmFiredRedCount > 0 || $scope.alarmFiredYellowCount > 0 || $scope.alarmFiredGreenCount > 0) {
                            if ($rootScope.enableAlarmFiredSound) {
                                $scope.playAudioAlert();
                            }
                        }
                        else {
                            $scope.stopAudioAlert();
                        }

                    }, function (error) {
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                    });
                
            }

            var constructor = function () {
                fetch();
                if ($scope.hasManageMonitoraggio)
                    $scope.interval = $interval(fetch, $rootScope.alarmFiredPooling);
            };

            constructor();
        }]);
