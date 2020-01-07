app.controller('headerCtrl',
    ['$scope',
        'zeusAuthService',
        function ($scope, zeusAuthService) {
            //$scope.model =
            //    {
            //        username: 'User'
            //    }

            $scope.logout = function () {
                zeusAuthService.logout();
            }

            //$rootScope.$on('update:username', function (param, data) {
            //    $scope.model.username = data;
            //    console.log("stampo user")
            //    console.log($scope.model.username )
            //});
        }]);