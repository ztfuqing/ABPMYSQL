(function () {
    appModule.controller('common.views.layout.pagehead', [
        '$scope',
        function ($scope) {
            $scope.$on('$includeContentLoaded', function () {
               // Layout.initSidebar(); // init sidebar
            });
        }
    ]);
})();