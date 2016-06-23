(function () {
    appModule.controller('common.views.layout.header', [
        '$rootScope', '$scope', '$uibModal', 'appSession', 'abp.services.app.hostSettings', 'appUserNotificationHelper', 'abp.services.app.notification',
        function ($rootScope, $scope, $uibModal, appSession, hostSettingsService, appUserNotificationHelper, notificationService) {
            var vm = this;
            vm.settings = null;
            $scope.$on('$includeContentLoaded', function () {
                Layout.initHeader();
            });

            vm.notifications = [];
            vm.unreadNotificationCount = 0;

            vm.getSettings = function () {
                hostSettingsService.getAllSettings()
                    .success(function (result) {
                        vm.settings = result;
                    }).finally(function () {
                    });
            };
            

            vm.getShownUserName = function () {
                    return appSession.user.surname;
            };

            vm.changePassword = function () {
                $uibModal.open({
                    templateUrl: '~/App/main/views/profile/changePassword.cshtml',
                    controller: 'common.views.profile.changePassword as vm',
                    backdrop: 'static'
                });
            };

            vm.loadNotifications = function () {
                notificationService.getUserNotifications({
                    maxResultCount: 3
                }).success(function (result) {
                    vm.unreadNotificationCount = result.unreadCount;
                    vm.notifications = [];
                    $.each(result.items, function (index, item) {
                        vm.notifications.push(appUserNotificationHelper.format(item));
                    });
                });
            }

            vm.setAllNotificationsAsRead = function () {
                appUserNotificationHelper.setAllAsRead();
            };

            vm.setNotificationAsRead = function (userNotification) {
                appUserNotificationHelper.setAsRead(userNotification.userNotificationId);
            }

            vm.openNotificationSettingsModal = function () {
                appUserNotificationHelper.openSettingsModal();
            };

            abp.event.on('abp.notifications.received', function (userNotification) {
                appUserNotificationHelper.show(userNotification);
                vm.loadNotifications();
            });

            abp.event.on('app.notifications.refresh', function () {
                vm.loadNotifications();
            });

            abp.event.on('app.notifications.read', function (userNotificationId) {
                for (var i = 0; i < vm.notifications.length; i++) {
                    if (vm.notifications[i].userNotificationId == userNotificationId) {
                        vm.notifications[i].state = 'READ';
                    }
                }

                vm.unreadNotificationCount -= 1;
            });

            function init() {
                vm.getSettings();
                vm.loadNotifications();
            }

            init();

            
        }
    ]);
})();