(function () {
    appModule.controller('game.views.inventory', [
        '$scope', '$uibModal', '$stateParams', 'uiGridConstants', "abp.services.app.inventory",
        function ($scope, $uibModal, $stateParams, uiGridConstants, inventoryService) {

            var vm = this;
            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            var requestParams = {
                userName: '',
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };


            vm.gridOptions = {
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
                        name: "时间",
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                        minWidth: 150
                    },
                    {
                        name: "用户",
                        field: 'userName',
                        minWidth: 150
                    },
                    {
                        name: "类型",
                        enableSorting: false,
                        field: 'leiXing',
                        minWidth: 200
                    },
                    {
                        name: "商品",
                        enableSorting: false,
                        field: 'product',
                        minWidth: 200
                    },
                    {
                        name: "数量",
                        enableSorting: false,
                        field: 'amount',
                        minWidth: 200
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

                        vm.getAuditLogs();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        requestParams.skipCount = (pageNumber - 1) * pageSize;
                        requestParams.maxResultCount = pageSize;

                        getInventories();
                    });
                },
                data: []
            };

            vm.getInventories = function () {
                vm.loading = true;
                inventoryService.getInventories({
                    skipCount: requestParams.skipCount,
                    maxResultCount: requestParams.maxResultCount,
                    sorting: requestParams.sorting
                }).success(function (result) {
                    vm.gridOptions.totalItems = result.totalCount;
                    vm.gridOptions.data = result.items;
                }).finally(function () {
                    vm.loading = false;
                });
            }

            vm.getInventories();
        }
    ]);
})();