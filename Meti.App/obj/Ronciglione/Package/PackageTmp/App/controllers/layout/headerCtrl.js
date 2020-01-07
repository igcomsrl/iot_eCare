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

            var currFFZoom = 1;
            var currIEZoom = 100;

            $('#plusBtn').on('click', function () {
                //if (navigator.userAgent.indexOf("Mozilla") > -1) {
                //    var step = 0.02;
                //    currFFZoom += step;
                //    $('body').css('MozTransform', 'scale(' + currFFZoom + ')');
                //} else {
                    var step = 2;
                    currIEZoom += step;
                    $('body').css('zoom', ' ' + currIEZoom + '%');
                //}
            });

            $('#minusBtn').on('click', function () {
                //if ($.browser.mozilla) {
                //    var step = 0.02;
                //    currFFZoom -= step;
                //    $('body').css('MozTransform', 'scale(' + currFFZoom + ')');

                //} else {
                    var step = 2;
                    currIEZoom -= step;
                    $('body').css('zoom', ' ' + currIEZoom + '%');
                //}
            });
        }]);