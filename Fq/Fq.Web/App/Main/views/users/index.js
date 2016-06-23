(function () {

    appModule.controller('common.views.users.index', [
        '$scope', '$uibModal', '$stateParams', 'uiGridConstants', 'abp.services.app.user',
        function ($scope, $uibModal, $stateParams, uiGridConstants, userService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.filterText = $stateParams.filterText || '';
            vm.currentUserId = abp.session.userId;

            vm.permissions = {
                create: abp.auth.hasPermission('Admin.Users.Create'),
                edit: abp.auth.hasPermission('Admin.Users.Edit'),
                changePermissions: abp.auth.hasPermission('Admin.Users.ChangePermissions'),
                'delete': abp.auth.hasPermission('Admin.Users.Delete')
            };

            var requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: "操作",
                        enableSorting: false,
                        width: 120,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown" uib-dropdown="">' +
                            '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i>操作<span class="caret"></span></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editUser(row.entity)">编辑</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.changePermissions" ng-click="grid.appScope.editPermissions(row.entity)">权限</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.delete" ng-click="grid.appScope.deleteUser(row.entity)">删除</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: "用户名",
                        field: 'userName',
                        minWidth: 100
                    },
                    {
                        name: "部门",
                        field: 'organization.displayName',
                        minWidth: 120
                    },
                    {
                        name: "姓名",
                        field: 'surname',
                        minWidth: 120
                    },
                    {
                        name: "手机号码",
                        field: 'mobile',
                        minWidth: 120
                    },
                    {
                        name: "邮箱",
                        field: 'emailAddress',
                        minWidth: 200
                    },
                    {
                        name: "激活",
                        field: 'isActive',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span ng-show="row.entity.isActive" class="label label-success">是</span>' +
                            '  <span ng-show="!row.entity.isActive" class="label label-default">否</span>' +
                            '</div>',
                        minWidth: 80
                    },
                    {
                        name: "最后登录时间",
                        field: 'lastLoginTime',
                        cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    },
                    {
                        name: "创建时间",
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            requestParams.sorting = null;
                        } else {
                            requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getUsers();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        requestParams.skipCount = (pageNumber - 1) * pageSize;
                        requestParams.maxResultCount = pageSize;

                        vm.getUsers();
                    });
                },
                data: []
            };

            if (!vm.permissions.edit &&
                !vm.permissions.changePermissions &&
                !vm.permissions.impersonation &&
                !vm.permissions.delete) {
                vm.userGridOptions.columnDefs.shift();
            }

            vm.getUsers = function () {
                vm.loading = true;
                userService.getUsers({
                    skipCount: requestParams.skipCount,
                    maxResultCount: requestParams.maxResultCount,
                    sorting: requestParams.sorting,
                    filter: vm.filterText
                }).success(function (result) {
                    vm.userGridOptions.totalItems = result.totalCount;
                    vm.userGridOptions.data = result.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.editUser = function (user) {
                openCreateOrEditUserModal(user.id);
            };

            vm.createUser = function () {
                openCreateOrEditUserModal(null);
            };

            vm.editPermissions = function (user) {
                $uibModal.open({
                    templateUrl: '~/App/common/views/users/permissionsModal.cshtml',
                    controller: 'common.views.users.permissionsModal as vm',
                    backdrop: 'static',
                    resolve: {
                        user: function () {
                            return user;
                        }
                    }
                });
            };

            vm.deleteUser = function (user) {
                if (user.userName == app.consts.userManagement.defaultAdminUserName) {
                    abp.message.warn("默认系统管理员不允许删除!");
                    return;
                }

                abp.message.confirm(
                    "确认要删除该用户?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            userService.deleteUser({
                                id: user.id
                            }).success(function () {
                                vm.getUsers();
                                abp.notify.success("删除成功!");
                            });
                        }
                    }
                );
            };

            function openCreateOrEditUserModal(userId) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/main/views/users/createOrEditModal.cshtml',
                    controller: 'common.views.users.createOrEditModal as vm',
                    backdrop: 'static',
                    resolve: {
                        userId: function () {
                            return userId;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getUsers();
                });
            }

            vm.getUsers();
        }]);
})();