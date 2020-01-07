var igcomFramework = angular.module("igcom-framework", []);

//#region Controller
igcomFramework.controller('paginatorController',
    ['$scope',
    function ($scope) {
        //Flag che determinà la visibilità del paginatore
        $scope.isVisible = false;
        //Dimensione della paginazione
        $scope.pageSizeList = [10, 20, 30, 40, 50, 60, 70, 80, 100, 1000];
        //Testo che riassume sinteticamente lo stato di paginazione
        $scope.paginationSummary = null;
        //Tiene il conteggio di quanti elementi sono presenti nel paginatore
        $scope.countAll = 0;
        //Numero di elementi da visualizzare per ogni pagina
        $scope.pageSize = 0;
        //Pagina corrente
        $scope.actualPage = 1;

        //#region Behaviors
        //Scorro all'ultima pagina.
        $scope.setLastPage = function () {
            //Se l'ultima riga della pagina corrisponde già all'ultimo elemento disponibile, esco.
            if (!$scope.canSetLastPage()) return;

            //Incremento i pivots di paginazione
            $scope.startRowIndex = $scope.countAll - $scope.pageSize;
            $scope.maxRowIndex = $scope.pageSize;

            //Invoco il caricamento degli oggetti paginati paginate
            $scope.fetchItemsCallback();

            $scope.countItemsCallback().then(function (response) {
                $scope.countAll = response.itemsLength;
                $scope.paginationSummary = $scope.buildSummary();
            });

            //Setto la pagina attuale
            $scope.actualPage = Math.ceil($scope.countAll / $scope.pageSize);
        };

        $scope.canSetLastPage = function () {
            //Può impostarla se esistono elementi e il numero di elementi per pagina moltiplicato
            //per la pagina attuale è minore del numero totale di elementi
            return $scope.countAll > 0 && ($scope.pageSize * $scope.actualPage) < $scope.countAll;
        };

        //Scorro alla prima pagina.
        $scope.setFirstPage = function () {
            //Se la prima riga della pagina corrisponde al primo elemento disponibile, esco.
            if (!$scope.canSetFirstPage()) return;

            //Incremento i pivots di paginazione
            $scope.startRowIndex = 0;
            $scope.maxRowIndex = pageSize;

            //Invoco il caricamento degli elementi paginati
            $scope.fetchItemsCallback();

            $scope.countItemsCallback().then(function (response) {
                $scope.countAll = response.itemsLength;
                $scope.paginationSummary = $scope.buildSummary();
            });

            //Setto la pagina attuale
            $scope.actualPage = 1;
        };

        $scope.canSetFirstPage = function () {
            //Può impostarla se esistono elementi e se la pagina attuale non è già la prima
            return $scope.countAll > 0 || $scope.actualPage !== 1;
        };

        //Scorro alla pagina successiva.
        $scope.setNextPage = function () {
            //Se l'ultima riga della pagina corrisponde già all'ultimo elemento disponibile, esco.
            if (!$scope.canSetNextPage()) return;

            //Incremento i pivots di paginazione
            $scope.startRowIndex = $scope.startRowIndex + $scope.pageSize;
            $scope.maxRowIndex = $scope.pageSize;

            //Invoco il caricamento degli elementi paginati
            $scope.fetchItemsCallback();

            $scope.countItemsCallback().then(function (response) {
                $scope.countAll = response.itemsLength;
                $scope.paginationSummary = $scope.buildSummary();
            });

            //Setto la pagina attuale
            $scope.actualPage = $scope.actualPage + 1;
        };

        $scope.canSetNextPage = function () {
            return $scope.countAll > 0 && ($scope.pageSize * $scope.actualPage) < $scope.countAll;
        };

        $scope.setPreviousPage = function () {
            //Se la prima riga della pagina corrisponde al primo elemento disponibile, esco.
            if (!$scope.canSetPreviousPage()) return;

            //Decremento i pivots di paginazione
            if ($scope.startRowIndex - $scope.pageSize < 0)
                $scope.startRowIndex = 0;
            else
                $scope.startRowIndex = $scope.startRowIndex - $scope.pageSize;
            $scope.maxRowIndex = $scope.pageSize;

            //Invoco il caricamento degli oggetti paginati
            fetchItemsCallback();

            $scope.countItemsCallback().then(function (response) {
                $scope.countAll = response.itemsLength;
                $scope.paginationSummary = $scope.buildSummary();
            });

            //Setto la pagina attuale
            $scope.actualPage = $scope.actualPage - 1;
        };

        $scope.canSetPreviousPage = function () {
            return $scope.countAll > 0 && $scope.actualPage !== 1;
        };

        $scope.setPaginatorSize = function (size) {
            $scope.pageSize = size;
            $scope.resetPaginatorIndexs();
            $scope.fetchItemsCallback();
        };

        //Logica di composizione del sommario di paginazione
        $scope.buildSummary = function () {
            var summary = "";
            if ($scope.startRowIndex + $scope.pageSize > $scope.countAll)
                summary = "Elementi: " + ($scope.startRowIndex + 1) + "-" + ($scope.countAll) + " di " + $scope.countAll;
            else
                summary = "Elementi: " + ($scope.startRowIndex + 1) + "-" + ($scope.startRowIndex + $scope.pageSize) + " di " + $scope.countAll;
            return summary;
        };

        $scope.actualPageSummary = function () {
            return 'Pagina: ' + $scope.actualPage + ' di ' + (Math.ceil($scope.countAll / $scope.pageSize));
        }

        $scope.refresh = function () {
            $scope.fetchItemsCallback();
            $scope.countItemsCallback().then(function (response) {
                $scope.countAll = response.itemsLength;
                $scope.paginationSummary = $scope.buildSummary();
            });
        };

        //Reset degli indici di paginazione
        $scope.resetPaginatorIndexs = function () {
            $scope.startRowIndex = 0;
            $scope.maxRowIndex = $scope.pageSize;
            $scope.actualPage = 1;
            $scope.countItemsCallback().then(function (response) {
                $scope.countAll = response.itemsLength;
                $scope.paginationSummary = $scope.buildSummary();
            });
        };
        //#endregion
    }]);
//#endregion

//#region Services
igcomFramework.factory("idleWatchDogService",
    ["$interval", "$document", "$rootScope",
    function IdleWatch($interval, $document, $rootScope) {
        var interval = null;
        var idleTime = 0;
        var watchDogTimer = 180000;
        var isEnabled = false;

        var isIdle = function () {
            return (idleTime > 0);
        };

        function startInterval() {
            if (!isEnabled) return;

            interval = $interval(function () {
                idleTime += watchDogTimer;
                $rootScope.$broadcast("igcom-framework-on-idle");
                $interval.cancel(interval);
                isEnabled = false;
            }, watchDogTimer);
        }

        var enableIdleWatchDog = function () {
            isEnabled = true;
            startInterval();
        }

        var disableIdleWatchDog = function () {
            isEnabled = false;
            $interval.cancel(interval);
        }

        function timerLogic() {
            idleTime = 0;
            $interval.cancel(interval);
            startInterval();
        }

        angular.element($document).bind('mousemove', function (e) {
            timerLogic();
        });

        angular.element($document).bind('keypress', function (e) {
            timerLogic();
        });

        return {
            enableIdleWatchDog: enableIdleWatchDog,
            disableIdleWatchDog: disableIdleWatchDog,
            isIdle: isIdle,
            watchDogTimer: watchDogTimer
        }
    }
    ]);

igcomFramework.factory('codiceFiscaleService', function () {
    var factory = {};

    factory.pad = function (number, width, pad) {
        // Default pad to 0 if none provided
        pad = pad || '0';
        // Convert number to a string
        number = number + '';
        return number.length >= width ? number : new Array(width - number.length + 1).join(pad) + number;
    };

    //options = ({ first: 'Sophia', last: 'Loren', gender: 'Female', birthday: new Date(1934, 8, 20), city: 'h501' });
    factory.generateCodiceFiscale = function (options) {
        options = options || {};
        var gender = !!options.gender ? options.gender : this.gender(),
            first = !!options.first ? options.first : this.first({ gender: gender, nationality: 'it' }),
            last = !!options.last ? options.last : this.last({ nationality: 'it' }),
            birthday = !!options.birthday ? options.birthday : this.birthday(),
            city = !!options.city ? options.city : this.pickone(['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'L', 'M', 'Z']) + factory.pad(this.natural({ max: 999 }), 3),
            cf = [],
            name_generator = function (name, isLast) {
                var temp,
                    return_value = [];

                if (name.length < 3) {
                    return_value = name.split("").concat("XXX".split("")).splice(0, 3);
                }
                else {
                    temp = name.toUpperCase().split('').map(function (c) {
                        return ("BCDFGHJKLMNPRSTVWZ".indexOf(c) !== -1) ? c : undefined;
                    }).join('');
                    if (temp.length > 3) {
                        if (isLast) {
                            temp = temp.substr(0, 3);
                        } else {
                            temp = temp[0] + temp.substr(2, 2);
                        }
                    }
                    if (temp.length < 3) {
                        return_value = temp;
                        temp = name.toUpperCase().split('').map(function (c) {
                            return ("AEIOU".indexOf(c) !== -1) ? c : undefined;
                        }).join('').substr(0, 3 - return_value.length);
                    }
                    return_value = return_value + temp;
                }

                return return_value;
            },
            date_generator = function (birthday, gender, that) {
                var lettermonths = ['A', 'B', 'C', 'D', 'E', 'H', 'L', 'M', 'P', 'R', 'S', 'T'];

                return birthday.getFullYear().toString().substr(2) +
                        lettermonths[birthday.getMonth()] +
                        factory.pad(birthday.getDate() + ((gender === 2) ? 40 : 0), 2);
                //that.pad(birthday.getDate() + ((gender.toLowerCase() === "female") ? 40 : 0), 2);
            },
            checkdigit_generator = function (cf) {
                var range1 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    range2 = "ABCDEFGpaHIJABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    evens = "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    odds = "BAKPLCQDREVOSFTGUHMINJWZYX",
                    digit = 0;


                for (var i = 0; i < 15; i++) {
                    if (i % 2 !== 0) {
                        digit += evens.indexOf(range2[range1.indexOf(cf[i])]);
                    }
                    else {
                        digit += odds.indexOf(range2[range1.indexOf(cf[i])]);
                    }
                }
                return evens[digit % 26];
            };

        cf = cf.concat(name_generator(last, true), name_generator(first), date_generator(birthday, gender, this), city.toUpperCase().split("")).join("");
        cf += checkdigit_generator(cf.toUpperCase(), this);

        return cf.toUpperCase();
    };

    return factory;
});

igcomFramework.factory('paginatorService', function () {
    var maxRowIndexDefault = 50;

    var paginatorModel = function (maxRowIndexParam, maxSizeParam) {
        return {
            currentPage: 1,
            totalItems: 0,
            maxRowIndex: maxRowIndexParam || maxRowIndexDefault,
            startRowIndex: 0,
            maxSize: maxSizeParam || 5,
            summary: '',
            isVisible: false
        }
    }

    var setMaxRowIndex = function (max) {
        maxRowIndexDefault = max;
    }

    var show = function (paginator) {
        paginator.isVisible = true;
    }

    var hide = function (paginator) {
        paginator.isVisible = false;
    }

    var updateIndexs = function (totalItems, currentPage, startRowIndex, maxRowIndex) {
        //Ricavo il punto di indice da cui paginare
        startRowIndex = Math.ceil((maxRowIndex * currentPage) - maxRowIndex);
        return startRowIndex;
    }

    var buildSummary = function (startRowIndex, maxRowIndex, totalItems) {
        var summary = "";
        if (startRowIndex + maxRowIndex > totalItems)
            summary = "Elementi: " + (startRowIndex + 1) + "-" + (totalItems) + " di " + totalItems;
        else
            summary = "Elementi: " + (startRowIndex + 1) + "-" + (startRowIndex + totalItems) + " di " + totalItems;
        return summary;
    }

    return {
        paginatorModel: paginatorModel,
        buildSummary: buildSummary,
        updateIndexs: updateIndexs,
        show: show,
        hide: hide,
        maxRowIndexDefault: maxRowIndexDefault,
        setMaxRowIndex: setMaxRowIndex
    }
});

igcomFramework.factory('responseDispatcherService', ["$log", function ($log) {
    var isValidationResult = function (vResult) {
        try {
            var result = false;
            if (vResult instanceof Array)
                result = vResult[0].ErrorMessage != null || vResult[0].errorMessage != null;
            else
                result = vResult.ErrorMessage != null || vResult.errorMessage != null;
            return result;
        } catch (ex) {
            return false;
        }
    }

    var isOperationResult = function (vResult) {
        try {
            return vResult.ValidationResults != null || vResult.ReturnedValue != null || vResult.validationResults != null || vResult.returnedValue != null;
        } catch (ex) {
            return false;
        }
    }

    var isString = function (vResult) {
        try {
            return angular.isString(vResult);
        } catch (ex) {
            return false;
        }
    }
    var printvalidationresult = function (results) {
        var message = "";
        angular.forEach(results, function (value, index) {
            var errorMessage = value.errorMessage || value.ErrorMessage;
            if (index === 0) message = message + errorMessage;
            else message = message + "<br/>" + value;
        });
        return message;
    };
    var printErrorMessages = function (vResults, toastr) {
        if (!toastr) throw Error("Toastr is null!");

        angular.forEach(vResults, function (value) {
            if (isValidationResult(value))

                toastr.error(printvalidationresult(value));
            else if (isOperationResult(value))
                toastr.error(printvalidationresult(value.ValidationResults));
            else if (isString(value))
                toastr.error(value);
            //  $log.info("printErrorMessages: " + value.ErrorMessage);
        });
    }

    return {
        isValidationResult: isValidationResult,
        isOperationResult: isOperationResult,
        isString: isString,
        printErrorMessages: printErrorMessages
    }
}]);

igcomFramework.factory('mockService',
    ["$log",
     "$q",
    function ($log, $q) {
        //Tempo di attesa tra una richiesta e una risposta
        var delay = 1000;

        // Setta un delay custom per le richieste del mock
        function setDelay(milliseconds) {
            delay = milliseconds;
        }

        //Simula una richiesta server. Ritorna un output dopo aver aspettato il tempo di 'delay'
        function mockServerRequest(requestMethod, requestArgs, responseModel) {
            //Serializzo gli argomenti per i servizi remoti
            var data = JSON.stringify(requestArgs);
            $log.info('[MOCK] Requesting server method "' + requestMethod + '" with ' +
                'parameters "' + data + '"...');

            var deferred = $q.defer();

            //Use setTimeout to simulate server loading
            //wait delay, then resolve promise
            //uso un timeout per simulare il caricamento del server.
            setTimeout(function () {
                //Log promise resolution
                $log.info('[MOCK] Resolving server method "' + requestMethod + '"...');

                //Resolve promise with model
                deferred.resolve(responseModel);
                $log.info('[MOCK] Server method "' + requestMethod + '" resolved.');
            }, delay);

            //Return promise
            return deferred.promise;
        }

        //Returns module schema
        return {
            setDelay: setDelay,
            mockServerRequest: mockServerRequest
        };
    }]);

igcomFramework.factory('httpService',
    ['$http',
     '$log',
     '$q',
    function ($http, $log, $q) {
        var httpRequest = function (url, method, data, headers, transformRequest, responseType, ignoreLoadingBar) {
            
            //Compose request
            var request = {
                method: method,
                url: url,
                headers: headers,
                //ignoreLoadingBar: ignoreLoadingBar || false
            };

            if (transformRequest)
                request.transformRequest = transformRequest;

            if (method === 'POST' || method === 'PUT')
                request.data = data;
            else
                request.params = data;

            if (responseType)
                request.responseType = responseType;

            var deferred = $q.defer();

            $http(request)
                .then(function (responseData) {
                    $log.debug('Http request success: ' + responseData);
                    deferred.resolve(responseData);
                }, function (responseData, status) {
                    $log.error('Http request error (' + status + '): ' + responseData);
                    deferred.reject({ "data": responseData, "status": status });
                });
                //.error(function (responseData, status) {
                //    $log.error('Http request error (' + status + '): ' + responseData);
                //    deferred.reject({ "data": responseData, "status": status });
                //})
                //.success(function (responseData) {
                //    $log.debug('Http request success: ' + responseData);
                //    deferred.resolve(responseData);
                //});

            return deferred.promise;
        }

        var httpJsonpRequest = function (url, query) {
            var deferred = $q.defer();
            $http.jsonp(url, { params: query })
                .then(function (response) {
                    $log.debug('Http request success: ' + response);
                    deferred.resolve(response);;
                }, function (response) {
                    $log.error('Http request error (' + status + '): ' + response);
                    //Resolve promise with model
                    deferred.reject(response);
                });
                //.success(function (response) {
                //    $log.debug('Http request success: ' + response);
                //    deferred.resolve(response);
                //}).error(function (response) {
                //    $log.error('Http request error (' + status + '): ' + response);
                //    //Resolve promise with model
                //    deferred.reject(response);
                //});

            //Return promise
            return deferred.promise;
        }

        return {
            httpRequest: httpRequest,
            httpJsonpRequest: httpJsonpRequest
        };
    }]);

igcomFramework.factory('dateService',
    [
    function () {
        //Aggiungo N giorni ad una data
        var addDays = function (oldDate, days) {
            if (!(oldDate instanceof Date)) throw Error("OldDate must be a Date javascript type");
            if (!days) days = 0;
            var newDate = new Date(oldDate);
            newDate.setDate(oldDate.getDate() + days);
            return newDate;
        }

        //Rimuovo N giorni ad una data
        var removeDays = function (oldDate, days) {
            if (!(oldDate instanceof Date)) throw Error("OldDate must be a Date javascript type");
            if (!days) days = 0;
            var newDate = new Date(oldDate);
            newDate.setDate(oldDate.getDate() - days);
            return newDate;
        }

        //Esegue il cast di una data in forma testuale. 19/07/2014
        var toString = function (dt) {
            var datetime = dt instanceof Date ? dt : toDateTime(dt);
            if (!datetime) return '';
            var d = datetime.getDate();
            var m = datetime.getMonth() + 1;
            var y = datetime.getFullYear();
            return '' + (d <= 9 ? '0' + d : d) + '-' + (m <= 9 ? '0' + m : m) + '-' + y;
        };

        //Esegue il cast di una data in forma testuale. 19/07/2014 00:01
        var toStringWithHour = function (dt) {
            var datetime = dt instanceof Date ? dt : toDateTime(dt);
            if (!datetime) return '';
            var d = datetime.getDate();
            var m = datetime.getMonth() + 1;
            var y = datetime.getFullYear();
            var h = datetime.getHours();
            var mt = datetime.getMinutes();
            return '' + (d <= 9 ? '0' + d : d) + '-' + (m <= 9 ? '0' + m : m) + '-' + y + '  ' + h + ':' + mt;
        };

        //Esegue il casta da un Datetime c# e una Date javascript
        var toDateTimeFromMvc = function (dateMvc) {
            try {
                return new Date(parseInt(dateMvc.substr(6)));
            } catch (ex) {
                return '';
            }
        };

        //Conta il numero di settimane presenti in un mese
        var weekCount = function (year, monthNumber, dayStart) {
            //monthNumber è compreso tra 1 e 12
            //Calcolo il primo del mese
            var firstOfMonth = new Date(year, monthNumber, dayStart || 1);
            //Calcolo l'ultimo giorno del mese
            var lastOfMonth = new Date(year, monthNumber, 0);
            //var used = firstOfMonth.getDay() + lastOfMonth.getDate();
            var used = lastOfMonth.getDate() + firstOfMonth.getDate();
            return Math.ceil(used / 7);
        }

        var weekCountInRange = function (monthFirstDate, actualMonthNumber, yearNumber) {
            //Se il parametro non è riconosciuto come Data, forzo il cast.
            if (!angular.isDate(monthFirstDate))
                monthFirstDate = new Date(monthFirstDate);
            //Recupero il primo mese in forma numerica
            var monthFirst = getMonth(monthFirstDate);
            //Recupero l'ultimo mese
            var monthLast = monthFirst + actualMonthNumber;
            //Recupero l'anno
            var year = yearNumber || new Date().getFullYear();
            //Recupero il giorno del primo mese
            var dayStart = monthFirstDate.getDate();
            //Inizializzo il conteggio delle settimane
            var weekNumberCount = 0;

            //Eseguo il conteggio del primo mese considerando il giorno di partenza
            weekNumberCount = weekNumberCount + weekCount(year, monthFirst, dayStart);
            monthFirst++;
            //Scorro i conteggi riguardanti i restanti mesi
            for (var i = monthFirst; i < monthLast; i++) {
                weekNumberCount = weekNumberCount + weekCount(year, i);
            }
            return weekNumberCount;
        }

        var getMonth = function (date) {
            //Se il parametro non è riconosciuto come Data, forzo il cast.
            if (!angular.isDate(date))
                date = new Date(date);
            //Ritorno il mese attuale(numerico)
            return date.getMonth();
        }

        //Recupero il mese richiesto considerando un pivot di riferimento (quanti mesi andare avanti)
        var getMonthWithPivot = function (date, pivot) {
            //Se il parametro non è riconosciuto come Data, forzo il cast.
            if (!angular.isDate(date))
                date = new Date(date);

            //Valuto il pivot
            if (!pivot) pivot = 1;

            //Recupero il mese attuale
            var actualMonth = date.getMonth();

            //Recupero il mese successivo
            return new Date(date.getFullYear(), actualMonth + pivot);
        }

        //Calcola quanti giorni intervallano due date
        var daysBetween = function (firstDate, secondDate) {
            var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
            firstDate = new Date(firstDate);
            secondDate = new Date(secondDate);
            return Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
        }

        return {
            toString: toString,
            toStringWithHour: toStringWithHour,
            toDateTimeFromMvc: toDateTimeFromMvc,
            addDays: addDays,
            removeDays: removeDays,
            weekCount: weekCount,
            weekCountInRange: weekCountInRange,
            getMonth: getMonth,
            daysBetween: daysBetween,
            getMonthWithPivot: getMonthWithPivot
        };
    }]);
//#endregion

//#region Directives
igcomFramework.directive('fileUpload',
    [function () {
        return {
            scope: true,        //create a new scope
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var files = event.target.files;
                    //iterate files since 'multiple' may be specified on the element
                    for (var i = 0; i < files.length; i++) {
                        //emit event upward
                        scope.$emit("fileSelected", { file: files[i] });
                    }
                });
            }
        };
    }]);

igcomFramework.directive('ngFocus', function ($timeout) {
    return {
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngFocus, function (val) {
                if (angular.isDefined(val) && val) {
                    $timeout(function () { element[0].focus(); });
                }
            }, true);

            element.bind('blur', function () {
                if (angular.isDefined(attrs.ngFocusLost)) {
                    scope.$apply(attrs.ngFocusLost);

                }
            });
        }
    };
});
igcomFramework.directive('igNumbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (inputValue) {
                // this next if is necessary for when using ng-required on your input.
                // In such cases, when a letter is typed first, this parser will be called
                // again, and the 2nd time, the value will be undefined
                if (inputValue == undefined) return ''
                var transformedInput = inputValue.replace(/[^0-9]/g, '');
                if (transformedInput != inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }

                return transformedInput;
            });
        }
    };
});
igcomFramework.directive("passwordVerify", function () {
    return {
        require: "ngModel",
        scope: {
            passwordVerify: '='
        },
        link: function (scope, element, attrs, ctrl) {
            scope.$watch(function () {
                var combined;

                if (scope.passwordVerify || ctrl.$viewValue) {
                    combined = scope.passwordVerify + '_' + ctrl.$viewValue;
                }
                return combined;
            }, function (value) {
                if (value) {
                    ctrl.$parsers.unshift(function (viewValue) {
                        var origin = scope.passwordVerify;
                        if (origin !== viewValue) {
                            ctrl.$setValidity("passwordVerify", false);
                            return undefined;
                        } else {
                            ctrl.$setValidity("passwordVerify", true);
                            return viewValue;
                        }
                    });
                }
            });
        }
    };
});
igcomFramework.directive('igUppercase', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {
                if (inputValue == undefined) inputValue = '';
                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                }
                return capitalized;
            }
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]);  // capitalize initial value
        }
    };
});
igcomFramework.provider('igOrderByService', [function() {
    //#region Provider Infrastructure
    var iconAscCssClass = 'fa fa-sort-asc';
    var iconDescCssClass = 'fa fa-sort-desc';
    var pointerCssClass = 'pointer';

    this.setEnvironment = function (ascCss, descCss, pointerCss) {
        iconAscCssClass = ascCss;
        iconDescCssClass = descCss;
        pointerCssClass = pointerCss;
    };

    this.$get = [function () {
                     return new serviceBase();
                 }];
    //#endregion

    var serviceBase = function () {
        this.orderType = { "asc": "asc", "desc": "desc" };
        this.iconAscCssClass = iconAscCssClass;
        this.iconDescCssClass = iconDescCssClass;
        this.pointerCssClass = pointerCssClass;
        this.orderByModel = function() {
            return {
                "property": "",
                "propertyType": "",
                "type": ""
            }
        }
    }
}]);
igcomFramework.directive('igOrderBy', [
  '$rootScope', '$window', '$timeout', 'igOrderByService', '$compile', function ($rootScope, $window, $timeout, igOrderByService, $compile) {
      return {
          restrict: 'AEC',
          scope: {
              property: "=",
              propertyType: "=",
              type: "=",
              fetch: "&"
          },
          link: function (scope, elem, attrs) {
              var orderType = igOrderByService.orderType;

              elem.css({ cursor: igOrderByService.pointerCssClass });
              
              //Elimina tutte le icone dal rigo 
              function setOrderIcons() {
                  //Recupero tutti gli elementi fratelli rispetto all'elemento corrente
                  var elements = elem.parent().children();
                  if (!elements) return;

                  //Dichiaro l'array che conterrà soltanto gli elementi fratelli che contengono la direttiva ig-order-by
                  var elementsFiltered = [];
                  for (var i = 0; i <= elements.length; i++) {
                      //Recupero l'elemento corrente
                      var element = elem.parent().children()[i];

                      if(!element) continue;
                      //Verifico l'esistenza del tag ig-order-by
                      var result = element.hasAttribute("ig-order-by");

                      //Verifico la condizione di successo
                      if (result)
                          elementsFiltered.push(element);
                  }

                  var icons = angular.element(elementsFiltered).find("i");
                  angular.forEach(icons, function (value) {
                      value.remove();
                  });
              }
              
              elem.bind('click', function () {

                  try {
                      setOrderIcons();

                      var type = attrs.type;

                      switch (type) {
                      case orderType.asc:
                          var iconHtmlElement = "<i class=\"" + igOrderByService.iconDescCssClass + "\"></i>";
                          elem.append(iconHtmlElement);
                          break;
                      case orderType.desc:
                          iconHtmlElement = "<i class=\"" + igOrderByService.iconAscCssClass + "\"></i>";
                          elem.append(iconHtmlElement);
                          break;
                      }


                      scope.$parent.model.orderBy.property = scope.property;
                      scope.$parent.model.orderBy.propertyType = scope.propertyType;
                      scope.$parent.model.orderBy.type = attrs.type;

                      switch (scope.$parent.model.orderBy.type) {
                      case orderType.asc:
                          scope.$parent.model.orderBy.type = orderType.desc;
                          attrs.type = orderType.desc;
                          attrs.$set('type', attrs.type);
                          $compile(elem)(scope);
                          break;
                      case orderType.desc:
                          scope.$parent.model.orderBy.type = orderType.asc;
                          attrs.type = orderType.asc;
                          attrs.$set('type', attrs.type);
                          $compile(elem)(scope);
                          break;
                      }

                      scope.fetch();
                  } catch (ex) {

                  }

              });

              scope.$on('$destroy', function () {
              });
          }
      };
  }
]);
igcomFramework.directive('mbInfiniteScroll', [
  '$rootScope', '$window', '$timeout', function ($rootScope, $window, $timeout) {
      return {
          //scope: {
          // variableWatches: "=mbInfiniteScrollVariablesToWatch",
          // resetPivot: "=mbInfiniteScrollResetPivot"
          //    elements: "=mbInfiniteScrollElements",
          //    fetchMethod: "=mbInfiniteScrollFetchMethod"
          //},
          link: function (scope, elem, attrs) {
              var checkWhenEnabled;
              var handler;
              var scrollDistance;
              var scrollEnabled;
              var variableWatchesHolder;
              //Variabili di cui monitorare i cambiamenti
              var variableWatches = attrs.mbInfiniteScrollVariablesToWatch;// ? scope.$eval(attrs.mbInfiniteScrollVariablesToWatch) : null;
              ////Indice di paginazione da resettare
              var resetPivot = attrs.mbInfiniteScrollResetPivot; //? JSON.parse(attrs.mbInfiniteScrollResetPivot) : null;
              ////Elementi visualizzati in griglia
              var elements = attrs.mbInfiniteScrollElements;// ? JSON.parse(attrs.mbInfiniteScrollElements) : null;
              //console.log(scope.resetPivot);
              //Recupero la finestra del documento html
              $window = angular.element($window);
              //Imposto la distanza di scroll a 0 di default
              scrollDistance = 0;

              //Se è richiesto un reset dei valori al modificarsi dei campi di ricerca
              if (resetPivot != null && elements != null) {
                  if (variableWatches != null) {
                      //variableWatchesHolder = scope.$watchCollection(variableWatches, function () {
                      //    elements = [];
                      //});
                      //Per ogni variabile passata, imposto un watch per monitorarne il cambiamento
                      //angular.forEach(variableWatches, function (valueToWatch) {
                      //    scope.$watch(valueToWatch, function (value) {
                      //        elements = [];
                      //    });
                      //});
                      //scope.$watch(variableWatches, function (value) {
                      //    elements = [];
                      //   // alert("watched!");
                      //}, true);
                  }
              }

              //Se è richiesta una distanza di scroll custom, la imposto
              if (attrs.mbInfiniteScrollDistance != null) {
                  scope.$watch(attrs.mbInfiniteScrollDistance, function (value) {
                      return scrollDistance = parseInt(value, 10);
                  });
              }
              scrollEnabled = true;
              checkWhenEnabled = false;

              //Se è richiesto che lo scroll venga disabilitato in fase di caricamento dei dati, lo imposto.
              if (attrs.mbInfiniteScrollDisabled != null) {
                  scope.$watch(attrs.mbInfiniteScrollDisabled, function (value) {
                      scrollEnabled = !value;
                      if (scrollEnabled && checkWhenEnabled) {
                          checkWhenEnabled = false;
                          return handler();
                      }
                  });
              }

              //Algoritmo per lo scrolling
              handler = function () {
                  var elementBottom, remaining, shouldScroll, windowBottom;
                  windowBottom = $window.height() + $window.scrollTop();
                  elementBottom = elem.offset().top + elem.height();
                  remaining = elementBottom - windowBottom;
                  shouldScroll = remaining <= $window.height() * scrollDistance;
                  if (shouldScroll && scrollEnabled) {
                      if ($rootScope.$$phase) {
                          return scope.$eval(attrs.mbInfiniteScroll);
                      } else {
                          return scope.$apply(attrs.mbInfiniteScroll);
                      }
                  } else if (shouldScroll) {
                      return checkWhenEnabled = true;
                  }
              };

              //Aggancio allo scroll l'algoritmo
              $window.on('scroll', handler);

              scope.$on('$destroy', function () {
                  //variableWatchesHolder();
                  return $window.off('scroll', handler);
              });

              return $timeout((function () {
                  if (attrs.mbInfiniteScrollImmediateCheck) {
                      if (scope.$eval(attrs.mbInfiniteScrollImmediateCheck)) {
                          return handler();
                      }
                  } else {
                      return handler();
                  }
              }), 0);
          }
      };
  }]);

//#endregion