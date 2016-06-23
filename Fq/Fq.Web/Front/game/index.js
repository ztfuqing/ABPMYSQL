(function () {
    appModule.controller('game.views.index', [
        '$rootScope', 'appSession', 'abp.services.app.transition', 'abp.services.app.tradingRecord',
        function ($scope, appSession,transitionService, tradingRecordService) {

            console.log(tradingRecordService);
            var vm = this;
            vm.saving = false;
            vm.item = {};
            vm.transitionId = null;

            function getTransition()
            {
                transitionService.getCurrentTransition().success(function (result) {
                    vm.transitionId = result.id;
                    getNewRecord();
                });
            }

            function getNewRecord() {
                tradingRecordService.getNewTradingRecord({ id: vm.transitionId }).success(function (result) {
                    vm.item = result;
                });
            }

            vm.save = function () {
                vm.saving = true;
                tradingRecordService.createTradingRecord(vm.item).success(function () {
                    abp.notify.info("办理成功");
                    getNewRecord();
                }).finally(function () { 
                    vm.saving = false;
                });
            }

            getTransition();
        }
    ]);
})();