var appModule = angular.module('app', [
    "ui.router",
    "ui.bootstrap",
    'ui.utils',
    "ui.jq",
    "ui.grid",
    'ui.grid.pagination',
    "oc.lazyLoad",
    "ngSanitize",
    'angularMoment',
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
App.setAssetsPath(abp.appPath + 'metronic/');
appModule.factory('settings', ['$rootScope', function ($rootScope) {
    // supported languages
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        },
        assetsPath: abp.appPath + 'metronic',
        globalPath: abp.appPath + 'metronic/global',
        layoutPath: abp.appPath + 'metronic/frontend'
    };

    $rootScope.settings = settings;

    return settings;
}]);

//Configuration for Angular UI routing.
appModule.config([
    '$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/');

        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/front/game/index.cshtml',
                menu: '',
            })
            .state('liushui', {
                url: '/liushui',
                templateUrl: '/front/game/liushui.cshtml',
                menu: ''
            })
            .state('inventory', {
                url: '/inventory',
                templateUrl: '/front/game/inventory.cshtml',
                menu: ''
            })
            .state('jiaoban', {
                url: '/jiaoban',
                templateUrl: '/front/game/jiaoban.cshtml',
                menu: ''
            })
            .state('report', {
                url: '/report',
                templateUrl: '/front/game/report.cshtml',
                menu: ''
            });
    }
]);

appModule.run(["$rootScope", "settings", "$state", "$uibModalStack", "i18nService", function ($rootScope, settings, $state, $uibModalStack, i18nService) {
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