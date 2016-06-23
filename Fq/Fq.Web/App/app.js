/* 'app' MODULE DEFINITION */
var appModule = angular.module("app", [
    "ui.router",
    "ui.bootstrap",
    'ui.utils',
    "ui.jq",
    'ui.grid',
    'ui.grid.pagination',
    "oc.lazyLoad",
    "ng.ueditor",
    "ngSanitize",
    'angularFileUpload',
    'daterangepicker',
    'angularMoment',
    'frapontillo.bootstrap-switch',
    'abp'
]);

appModule.config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        cssFilesInsertBefore: 'ng_load_plugins_before', // load the css files before a LINK element with this ID.
        debug: false,
        events: true,
        modules: []
    });
}]);

/* THEME SETTINGS */
App.setAssetsPath(abp.appPath + 'metronic/');
appModule.factory('settings', ['$rootScope', function ($rootScope) {
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 1000, // auto scroll to top on page load
            showLoginInfo: true
        },
        layoutImgPath: App.getAssetsPath() + 'admin/img/',
        layoutCssPath: App.getAssetsPath() + 'admin/css/',
        assetsPath: abp.appPath + 'metronic',
        globalPath: abp.appPath + 'metronic/global',
        layoutPath: abp.appPath + 'metronic/admin'
    };

    $rootScope.settings = settings;

    return settings;
}]);

/* ROUTE DEFINITIONS */

appModule.config([
    '$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {

        // Default route (overrided below if user has permission)
        $urlRouterProvider.otherwise("/dashboard");


        $stateProvider.state('notifications', {
            url: '/notifications',
            templateUrl: '~/App/main/views/notifications/index.cshtml'
        });

        //Welcome page
        $stateProvider.state('dashboard', {
            url: '/dashboard',
            templateUrl: '~/app/main/views/dashboard/index.cshtml'
        });

            $stateProvider.state('roles', {
                url: '/roles',
                templateUrl: '~/app/main/views/roles/index.cshtml'
            });
            $stateProvider.state('users', {
                url: '/users?filterText',
                templateUrl: '~/app/main/views/users/index.cshtml'
            });

            $stateProvider.state('auditLogs', {
                url: '/auditLogs',
                templateUrl: '~/app/main/views/auditLogs/index.cshtml'
            });

            $stateProvider.state('organizationUnits', {
                url: '/organizationUnits',
                templateUrl: '~/app/main/views/organizationUnits/index.cshtml',
                menu: 'Administration.OrganizationUnits'
            });

            $stateProvider.state('settings', {
                url: '/settings',
                templateUrl: '~/app/main/views/settings/index.cshtml'
            });
    }
]);

appModule.run(["$rootScope", "settings", "$state", "i18nService", "$uibModalStack", function ($rootScope, settings, $state, i18nService, $uibModalStack) {
    $rootScope.$state = $state;
    $rootScope.$settings = settings;
    $rootScope.$on("$stateChangeSuccess", function () {
        $uibModalStack.dismissAll();
    });

    i18nService.get(abp.localization.currentCulture.name) ? i18nService.setCurrentLang(abp.localization.currentCulture.name) : i18nService.setCurrentLang("en");

    $rootScope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
}]);