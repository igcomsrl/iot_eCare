//app.directive("csDateToIso", function () {

//    var linkFunction = function (scope, element, attrs, ngModelCtrl) {

//        ngModelCtrl.$parsers.push(function (datepickerValue) {
//            return moment(datepickerValue).format("YYYY-MM-DD");
//        });
//    };

//    return {
//        restrict: "A",
//        require: "ngModel",
//        link: linkFunction
//    };
//});