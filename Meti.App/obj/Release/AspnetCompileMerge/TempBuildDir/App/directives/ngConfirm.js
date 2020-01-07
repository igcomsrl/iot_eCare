
app.directive('ngConfirmClick', ['$uibModal',
    function ($uibModal) {
        var modalInstanceCtrl = ["$scope", "$uibModalInstance", function ($scope, $uibModalInstance) {
            $scope.ok = function () {
                $uibModalInstance.close();
            };

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }];

        return {
            restrict: 'A',
            scope: {
                ngConfirmClick: "&",
                item: "="
            },
            link: function (scope, element, attrs) {
                element.bind('click', function () {
                    var message = attrs.ngConfirmMessage || "Sei sicuro?";
                    var title = attrs.ngConfirmTitle || "Popup";
                    var smallTitle = attrs.ngConfirmSmallTitle || "";
                    var btnOkTitle = attrs.ngConfirmBtnOkTitle || "Ok";
                    var btnCancelTitle = attrs.ngConfirmBtnCancelTitle || "Cancel";

                    //Modal Header
                    var modalHtml = '<div class="modal-header" style="padding-bottom: 0px;"> <h3 class="modal-title">' + title + '</h3> ' +
                        '<small>' + smallTitle + '</small>';
                    //Modal body
                    modalHtml += '<br/></div><div class="modal-body modal-scrollbody" style="height: 0; min-height:100px;">' + message + '</div>';

                    //Modal footer
                    modalHtml += '<br/><div class="modal-footer"><button type="button" class="btn btn-danger" ng-click="ok()">' + btnOkTitle + '</button><button type="button" class="btn btn-default" ng-click="cancel()">' + btnCancelTitle + '</button></div>';

                    var modalInstance = $uibModal.open({
                        template: modalHtml,
                        controller: modalInstanceCtrl
                    });

                    modalInstance.result.then(function () {
                        scope.ngConfirmClick();
                    }, function () {
                    });
                    //*/
                });
            }
        }
    }]);