(function () {
    appModule.controller('game.views.liushui', [
        '$rootScope', 'appSession', 'abp.services.app.transition', 'abp.services.app.tradingRecord',
        function ($scope, appSession, transitionService, tradingRecordService) {

            var vm = this;
            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.items = [];
            vm.transitionId = null;

            function getTransition() {
                vm.loading = true;
                transitionService.getCurrentTransition().success(function (result) {
                    vm.transitionId = result.id;
                    getList();
                });
            }

            function getList() {
                vm.loading = true;
                tradingRecordService.getCurrentTransitionTradingRecords({ transitionId: vm.transitionId }).success(function (result) {
                    vm.items = result;
                }).finally(function () {
                    vm.loading = false;
                });;
            }

            getTransition();
        }
    ]);
})();