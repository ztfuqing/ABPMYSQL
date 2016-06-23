(function () {
    appModule.controller('common.views.users.createOrEditModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'userId',
        function ($scope, $uibModalInstance, userService, userId) {
            var vm = this;

            vm.saving = false;
            vm.user = null;
            vm.roles = [];
            vm.organizations = [];
            vm.setRandomPassword = (userId == null);
            vm.sendActivationEmail = (userId == null);
            vm.canChangeUserName = true;

            vm.save = function () {
                var assignedRoleNames = _.map(
                    _.where(vm.roles, { isAssigned: true }), //Filter assigned roles
                    function (role) {
                        return role.roleName; //Get names
                    });

                if (vm.setRandomPassword) {
                    vm.user.password = null;
                }

                vm.saving = true;
                userService.createOrUpdateUser({
                    user: vm.user,
                    assignedRoleNames: assignedRoleNames,
                    sendActivationEmail: vm.sendActivationEmail
                }).success(function () {
                    abp.notify.info("保存成功");
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.getAssignedRoleCount = function () {
                return _.where(vm.roles, { isAssigned: true }).length;
            };

            vm.getOrganizationValue = function (item) {
                return parseInt(item.value);
            };

            function init() {
                userService.getUserForEdit({
                    id: userId
                }).success(function (result) {
                    vm.user = result.user;
                    vm.user.passwordRepeat = vm.user.password;
                    vm.roles = result.roles;
                    vm.organizations = result.organizations;
                    vm.canChangeUserName = vm.user.userName != app.consts.userManagement.defaultAdminUserName;
                });
            }

            init();
        }
    ]);
})();