/* Used by user and role permission settings. 
 */
appModule.directive('dateTimePicker', [
      function () {
          return {
              restrict: "A",
              require: "ngModel",
              link: function (scope, element, attrs, ngModelCtrl) {
                  var parent = $(element).parent();
                  var dtp = parent.datetimepicker({
                      format:'YYYY-MM-DD HH:mm',
                      locale: 'zh-cn',
                      showTodayButton: true
                  });
                  dtp.on("dp.change", function (e) {
                      ngModelCtrl.$setViewValue(moment(e.date).format("YYYY-MM-DD HH:mm"));
                      scope.$apply();
                  });
              }
          };
      }]);