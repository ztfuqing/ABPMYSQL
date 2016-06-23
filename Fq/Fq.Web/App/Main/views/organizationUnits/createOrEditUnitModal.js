(function () {
    appModule.controller('common.views.organizationUnits.createOrEditUnitModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.organizationUnit', 'organizationUnit',
        function ($scope, $uibModalInstance, organizationUnitService, organizationUnit) {
            var vm = this;

            vm.organizationUnit = organizationUnit;

            vm.saving = false;

            vm.save = function () {
                if (vm.organizationUnit.id) {
                    organizationUnitService
                        .updateOrganizationUnit(vm.organizationUnit)
                        .success(function(result) {
                            abp.notify.info("保存成功！");
                            $uibModalInstance.close(result);
                        });
                } else {
                    organizationUnitService
                        .createOrganizationUnit(vm.organizationUnit)
                        .success(function(result) {
                            abp.notify.info("保存成功");
                            $uibModalInstance.close(result);
                        });
                }
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();