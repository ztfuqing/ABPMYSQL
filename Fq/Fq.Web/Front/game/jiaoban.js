(function () {
    appModule.controller('game.views.jiaoban', [
        '$scope', "abp.services.app.transition",
        function ($scope, transitionService) {

            var vm = this;
            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            vm.item = {};
            vm.loading = false;


            vm.getjiaoban = function () {
                vm.loading = true;
                transitionService.getCurrentJiaoBanInfo().success(function (result) {
                    vm.item = result;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.jiaoban = function () {
                transitionService.jiaoBan(vm.item).success(function () {
                    location.href = "/";
                });
            };

            vm.getjiaoban();
        }
    ]);
})();