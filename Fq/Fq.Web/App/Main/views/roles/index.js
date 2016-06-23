(function () {
    appModule.controller('common.views.roles.index', [
        '$rootScope', '$scope', '$uibModal', '$templateCache', 'abp.services.app.role', 'uiGridConstants',
        function ($rootScope, $scope, $uibModal, $templateCache, roleService, uiGridConstants) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            $rootScope.settings.layout.showLoginInfo = true;
            vm.loading = false;

            vm.permissions = {
                create: abp.auth.hasPermission('Admin.Roles.Create'),
                edit: abp.auth.hasPermission('Admin.Roles.Edit'),
                'delete': abp.auth.hasPermission('Admin.Roles.Delete')
            };

            vm.roleGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                appScopeProvider: vm,
                columnDefs: [
                    {
                        name: "操作",
                        enableSorting: false,
                        width: 120,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown" uib-dropdown="">' +
                            '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> 操作<span class="caret"></span></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editRole(row.entity)">编辑</a></li>' +
                            '      <li><a ng-if="!row.entity.isStatic && grid.appScope.permissions.delete" ng-click="grid.appScope.deleteRole(row.entity)">删除</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: "名称",
                        field: 'displayName',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  {{COL_FIELD CUSTOM_FILTERS}} &nbsp;' +
                            '  <span ng-show="row.entity.isStatic" class="label label-info" uib-popover="系统角色不能删除" popover-placement="bottom" popover-trigger="mouseenter click">系统</span>&nbsp;' +
                            '  <span ng-show="row.entity.isDefault" class="label label-default" uib-popover="新用户默认拥有此角色" popover-placement="bottom" popover-trigger="mouseenter click">默认</span>' +
                            '</div>'
                    },
                    {
                        name: "创建时间",
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\''
                    }
                ],
                data: []
            };

            if (!vm.permissions.edit && !vm.permissions.delete) {
                vm.roleGridOptions.columnDefs.shift();
            }

            vm.getRoles = function () {
                vm.loading = true;
                roleService.getRoles({}).success(function (data) {
                    vm.roleGridOptions.data = data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.editRole = function (role) {
                openCreateOrEditRoleModal(role.id);
            };

            vm.deleteRole = function (role) {
                abp.message.confirm(
                    '确定要删除该角色吗？',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            roleService.deleteRole({
                                id: role.id
                            }).success(function () {
                                vm.getRoles();
                                abp.notify.success("删除成功");
                            });
                        }
                    }
                );
            };

            vm.createRole = function () {
                openCreateOrEditRoleModal(null);
            };

            function openCreateOrEditRoleModal(roleId) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/app/main/views/roles/createOrEditModal.cshtml',
                    controller: 'common.views.roles.createOrEditModal as vm',
                    backdrop: 'static',
                    resolve: {
                        roleId: function () {
                            return roleId;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getRoles();
                });
            }

            vm.getRoles();
        }
    ]);
})();