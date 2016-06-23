(function () {
    appModule.controller('common.views.layout.header', [
         '$scope', 'appSession',
        function ($scope, appSession) {
            var vm = this;

            $scope.$on('$includeContentLoaded', function () {
                //Layout.initHeader();
            });

            vm.getShownUserName = function () {
                return appSession.user.surname;
            };
        }
    ]);
})();