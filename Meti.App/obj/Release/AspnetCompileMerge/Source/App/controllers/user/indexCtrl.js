app.controller('user-indexCtrl',
    ['$scope',
        '$log',
        'toastr',
        "responseDispatcherService",
        'userService',
        '$q',
        '$uibModal',
        'NgTableParams',
        'ngTableService',
        'utilsService',
        '$rootScope',
        function ($scope, $log, toastr, responseDispatcherService, userService, $q, $uibModal, NgTableParams, ngTableService, utilsService, $rootScope) {
            //#region Model
            $scope.model = {
                firstname: null,
                surname: null,
                userName: null,
                email: null,
                userList: [],
                isCtrlReady: false,
                isFetchBusy: false
            };
            //#endregion

            $scope.$watchGroup(['model.firstname', 'model.surname', 'model.email'],
                function (newValues, oldValues, scope) {
                    $scope.model.userList = [];
                });

            //#region Behaviors

            $scope.clearFilter = function () {
                $scope.model.firstname = null;
                $scope.model.surname = null;
                $scope.model.email = null;
                $scope.model.isFetchBusy = false;
                $scope.model.userList = [];
            }

            $scope.fetch = function (startRowIndex, maxRowIndex, orderByProperty, orderByType) {
                $scope.model.isFetchBusy = true;

                var deferred = $q.defer();

                var request =
                {
                    'firstname': $scope.model.firstname,
                    'surname': $scope.model.surname,
                    'email': $scope.model.email,
                    'username': $scope.model.userName,
                    "startRowIndex": startRowIndex,
                    "maxRowIndex": maxRowIndex,
                    "orderByProperty": orderByProperty || "",
                    "orderByType": orderByType || ""
                };

                userService.fetch(request)
                    .then(function (response) {
                        var data = response.data;

                        $scope.model.userList = [];
                        angular.forEach(data.data, function (dto) {
                            var model = userService.userIndexModel(dto);
                            $scope.model.userList.push(model);
                        });

                        deferred.resolve({ "models": $scope.model.userList, "count": data.count });
                    }, function (error) {
                        //Loggo gli errori
                        $log.warn("Errore in fetch: " + JSON.stringify(error));
                    }).finally(function () {
                        $scope.model.isFetchBusy = false;
                    });

                return deferred.promise;
            }

            $scope.openEdit = function (item) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'App/templates/user/edit.html',
                    controller: 'user-editCtrl',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        "userId": function () {
                            return item ? item.id : null;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    $scope.userListNgTable.reload();
                });
            }

            $scope.delete = function (item) {
                
                //Definisco la richiesta
                var request = {'id': item.id}

                //Eseguo la richiesta al server
                var promise = userService.delete(request);

                //Valuto la risposta
                promise.then(function (response) {
                    //var index = $scope.model.userList.indexOf(item);
                    //$scope.model.userList.splice(index, 1);
                    $scope.userListNgTable.reload();
                    toastr.success($rootScope.validationMessages.deleteSuccess);
                }, function (error) {                   
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Cancellazione entity fallita in user-indexCtrl" + JSON.stringify(error));
                }).finally(function () {
                });
            }

            $scope.resetPassword = function (item) {

                //Definisco la richiesta
                var request = { 'username': item.username }

                //Eseguo la richiesta al server
                var promise = userService.resetPassword(request);

                //Valuto la risposta
                promise.then(function (response) {
                    //var index = $scope.model.userList.indexOf(item);
                    //$scope.model.userList.splice(index, 1);
                    //$scope.userListNgTable.reload();
                    toastr.success("La nuova password è stata inviata all'indirizzo: " + item.email);
                }, function (error) {
                    responseDispatcherService.printErrorMessages(error.data.data, toastr);
                    $log.error("Cancellazione entity fallita in user-indexCtrl" + JSON.stringify(error));
                }).finally(function () {
                });
            }

            var fetchEssentialData = function () {
                var deferred = $q.defer();

                var request = {};

                userService.fetchEssentialData(request)
                    .then(function (response) {
                        var data = response ? response.data : {};

                        deferred.resolve(response);
                    }, function (error) {
                        $log.error("Errore in fetchEssentialData: " + JSON.stringify(error));
                        deferred.reject(error);
                    }).finally(function () {
                        $scope.isCtrlReady = true;
                    });

                return deferred.promise;
            }
            //#endregion

            $scope.userListNgTable = new NgTableParams({
                page: 1,
                sorting: { "InsertDate": "asc" }
            },
                {
                    getData: function (params) {
                        var paginationIndexs = ngTableService.buildPaginatorIndexs(params);
                        return $scope.fetch(paginationIndexs.startRowIndex, paginationIndexs.maxRowIndex, paginationIndexs.orderByProperty, paginationIndexs.orderByType)
                            .then(function (data) {
                                params.total(data.count);
                                return data.models;
                            });
                    }
                });

            var constructor = function () {
                $scope.model.isCtrlReady = false;

                var essentialDataPromises = [];
                //essentialDataPromises.push(fetchEssentialData());

                $q.all(essentialDataPromises).then(function () {
                    $scope.model.isCtrlReady = true;
                    //$scope.fetch();
                });
            };

            constructor();
        }]);