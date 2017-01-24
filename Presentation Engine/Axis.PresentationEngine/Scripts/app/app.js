angular.module("xenatixApp", ['ui.router', 'xenatixApp.settings', 'ui.bootstrap', 'ngMask', 'ngInputModified', 'ui.mask', 'ngIdle', 'jcs.angular-http-batch', 'ngStorage'], function () { })
    .config(['$provide', '$logProvider', '$stateProvider', '$httpProvider', 'settings', '$controllerProvider', '$compileProvider', '$filterProvider', 'lazyLoaderProvider', 'inputModifiedConfigProvider', 'IdleProvider', 'KeepaliveProvider', 'httpBatchConfigProvider', 'httpLoaderInterceptorProvider',
        function ($provide, $logProvider, $stateProvider, $httpProvider, settings, $controllerProvider, $compileProvider, $filterProvider, lazyLoader, inputModifiedConfigProvider, IdleProvider, KeepaliveProvider, httpBatchConfigProvider, httpLoaderInterceptorProvider) {

        		// Debug Settings
        		$logProvider.debugEnabled(settings.enableJsDebug);
        		$compileProvider.debugInfoEnabled(settings.enableJsDebug);

        		// Adds controllerName variable to all controllers for the app
        		$provide.decorator('$controller', ['$delegate', function ($delegate) {
        				return function (constructor, locals, later, indent) {
        						if (typeof constructor === 'string' && !locals.$scope.controllerName) {
        								locals.$scope.controllerName = constructor;
        						}
        						return $delegate(constructor, locals, later, indent);
        				};
        		}]);


        		$provide.decorator("$q", ['$delegate', function ($delegate) {
        				var isPromiseLike = function (obj) { return obj && angular.isFunction(obj.then); }

        				function partial(func, args) {
        						return func.apply(null, args);
        				};

        				function serial(tasks) {
        						var prevPromise;
        						var error = new Error();
        						var resolveArray = [];

        						tasks.push([function () { return $delegate.when(resolveArray); }]);

        						angular.forEach(tasks, function (taskData, key) {
        								var task = taskData[0] || taskData;
        								var args = taskData[1] || [];
        								var success = task.success || task;
        								var fail = task.fail;
        								var notify = task.notify;
        								var nextPromise;

        								if (!prevPromise) {
        										nextPromise = partial(success, args);
        										if (!isPromiseLike(nextPromise)) {
        												error.message = "Task " + key + " did not return a promise.";
        												throw error;
        										}
        								}
        								else {
        										nextPromise = prevPromise.then(
																						function (data) {
																								if (!success) {
																										return data;
																								}
																								resolveArray.push(data);
																								var ret = partial(success, args);
																								if (!isPromiseLike(ret)) {
																										error.message = "Task " + key + " did not return a promise.";
																										throw error;
																								}
																								return ret;
																						},
																						function (reason) {
																								if (!fail) {
																										return $delegate.reject(reason);
																								}
																								resolveArray.push(reason);
																								var ret = fail(reason);
																								if (!isPromiseLike(ret)) {
																										error.message = "Fail for task " + key + " did not return a promise.";
																										throw error;
																								}
																								return ret;
																						},
																						notify);
        								}
        								prevPromise = nextPromise;
        						});

        						return prevPromise || $delegate.when();
        				};

        				$delegate.serial = serial;
        				return $delegate;
        		}]);

        		httpBatchConfigProvider.setAllowedBatchEndpoint(
														// root endpoint url
														'/data',
														// endpoint batch address
														'/data/batch',
														// optional configuration parameters
														{
																minimumBatchSize: 2,
																maxBatchedRequestPerCall: 100
														});

        		// configure Idle settings
        		IdleProvider.idle(10 * 60); // in minutes
        		IdleProvider.timeout(15); // in seconds
        		KeepaliveProvider.interval(10 * 60); // in minutes

        		inputModifiedConfigProvider
													.enableGlobally()
													.setModifiedClassName('ng-modified')
													.setNotModifiedClassName('ng-not-modified');

        		$httpProvider.useApplyAsync(true);

        		if (!$httpProvider.defaults.headers.get) {
        				$httpProvider.defaults.headers.get = {};
        		}

        		$httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        		$httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

        		// Loader confoguration - ignore loading for exception url
        		httpLoaderInterceptorProvider.exceptions('/xping');
        		httpLoaderInterceptorProvider.exceptions('template/datepicker/');
        		httpLoaderInterceptorProvider.exceptions('template/typeahead/');

        		$httpProvider.interceptors.unshift('httpLoaderInterceptor');

        		lazyLoader.setProviders($controllerProvider, $compileProvider, $provide, $filterProvider);
        		lazyLoader.setInvokeQueue(angular.module("xenatixApp")._invokeQueue);

        		$stateProvider
														.state('autosync', {
																url: '/AutoSync',
																template: '<sync-progress />',
																resolve: {
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/sync');
																		}]
																}
														})
														.state('autosync.progress', {
																url: '/Progress'
														})
														.state('offlineLogin', {
																url: '/Login',
																template: '' +
																				'<div class="container-fluid">' +
																				'   <div class="absolute-center">' +
																				'       <div class="row">' +
																				'           <div class="col-md-4 col-md-offset-4">' +
																				'               <h1 class="h2 text-center">Offline Session - Log Back In <a href="#" class="pull-right"><i class="fa fa-question-circle"></i></a></h1>' +
																				'           </div>' +
																				'       </div>' +
																				'       <div class="text-center form row">' +
																				'           <div class="form-group">' +
																				'               <div class="col-md-4 col-md-offset-4 padding-top-xsmall padding-bottom-xsmall">' +
																				'                   <label for="UserName" class="pull-left">User Name</label>' +
																				'                   <input type=textbox id="UserName" ng-model="$parent.offlineUserName" class="form-control text-center" autofocus="autofocus" />' +
																				'               </div>' +
																				'           </div>' +
																				'           <div class="form-group">' +
																				'               <div class="col-md-4 col-md-offset-4 padding-top-xsmall padding-bottom-xsmall">' +
																				'                   <label for="Password" class="pull-left">Password</label>' +
																				'                   <input type=password id="Password" ng-model="$parent.offlinePassword" class="form-control text-center" onkeypress="capLock(event)" />' +
																				'                   <div id="divCapsCheck" style="visibility:hidden">Caps Lock is on.</div>' +
																				'               </div>' +
																				'           </div>' +
																				'           <div class="form-group">' +
																				'               <div class="col-md-4 col-md-offset-4 text-center padding-top-xsmall padding-bottom-xsmall">' +
																				'                   <button type="button" name="Send" value="Send" id="Send" ng-click="$parent.authenticateOffline()" class="btn btn-default text-center width-100">Log Back In</button>' +
																				'               </div>' +
																				'           </div>' +
																				'       </div>' +
																				'   </div>' +
																				'</div>'
														})
														.state('dashboard', {
																url: '/Dashboard',
																templateUrl: '/Dashboard/Dashboard',
																controller: 'dashboardController as vm',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/dashboard');
																		}]
																}
														})
														.state('reports', {
																url: '/Reports',
																templateUrl: '/To/Do/Index'
														})
														.state('logout', {
																//url: '/Logout',
																templateUrl: '/Account/Account/Logoff',
																controller: function ($scope, $state) {
																		window.location.href = '../Account/Account';
																}
														})
														.state('admin', {
																url: '/Admin',
																templateUrl: '/Admin/Admin/Index',
																controller: 'adminController',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/admin');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-UserDetails'
																}
														})
														.state('role', {
																url: '/Role',
																templateUrl: '/Security/Role/Index',
																controller: 'roleController',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/role');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-RoleManagement-RoleDetails'
																}
														})
														.state('siteadministration', {
																title: 'Site Administration',
																url: '/SiteAdministration',
																templateUrl: '/Admin/Admin/SiteAdminMain',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/admin');
																		}]
																}
														})
														.state('siteadministration.staffmanagement', {
																title: 'Staff Management',
																url: '/StaffManagement',
																templateUrl: '/Admin/StaffManagement/Index',
																controller: 'staffManagementController as ctrl',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/staffManagement');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-UserDetails'
																}
														})
														.state('siteadministration.configuration', {
																title: 'Configuration',
																url: '/Configuration',
																templateUrl: '/Configuration/Config/Index',
																controller: 'configController as ctrl',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/configuration');
																		}]
																}
														})
														.state('siteadministration.configuration.locations', {
																title: "Locations",
																url: "/Locations",
																templateUrl: '/Configuration/Locations/Index',
																controller: 'locationsController as ctrl',
																views: {
																		'@siteadministration.configuration': {
																				templateUrl: '/Configuration/Locations/Index'
																		},
																		'navigation@siteadministration': {
																				templateUrl: '/Configuration/Config/Navigation'
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/locations');
																		}]
																}
														})
														.state('siteadministration.configuration.locations.details', {
																url: '/{LocationID:int}',
																views: {
																		'@siteadministration.configuration': {
																				template: '<div locationsdetailsuiview ui-view></div>'
																		},
																		'navigation@siteadministration': {
																				template: '<div class="container-fluid nomargin nopadding">' +
																												'<div>' +
																																'<div class="col-md-12 col-lg-12 right-nav">' +
																																				'<div work-flows-set work-flow-active-option="locationsComplete" work-flow-ready="workFlowReady" work-flow-model="workFlowModel">' +
																										'</div></div></div></div>',
																				controller: "locationsNavigationController as ctrl"
																		}
																},
																cache: false,
																resolve: {
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/locationsNavigation');
																		}]
																}
														})
														.state('siteadministration.configuration.locations.details.general', {
																title: '{{LocationName}}',
																url: '/General',
																views: {
																		'@siteadministration.configuration': {
																				templateUrl: '/Configuration/LocationInfo/Index',
																				controller: "locationInfoController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/locationInfo');
																		}]
																}
														})
														.state('siteadministration.configuration.locations.details.roomschedule', {
																title: '{{LocationName}}/Room Schedule',
																url: '/RoomSchedule',
																views: {
																		'@siteadministration.configuration': {
																				templateUrl: '/Configuration/RoomSchedule/Index',
																				controller: "roomScheduleController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/roomschedule');
																		}]
																}
														})
														.state('siteadministration.configuration.locations.details.blockedtime', {
																title: '{{LocationName}}/Blocked Time',
																url: '/BlockedTime',
																views: {
																		'@siteadministration.configuration': {
																				templateUrl: '../BlockedTime/Index',
																				controller: "blockTimeController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/blockedtime');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-PluginConfiguration-PluginConfiguration'
																}
														})
														.state('siteadministration.usermanagement', {
																url: "/UserManagement",
																templateUrl: '/Admin/Admin/Index',
																controller: 'adminController as ctrl',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/admin');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-UserDetails'
																}
														})
														.state('siteadministration.rolemanagement', {
																title: 'Role Management',
																url: "/RoleManagement",
																templateUrl: '/Security/Role/Index',
																controller: 'roleController as ctrl',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/role');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-RoleManagement-RoleDetails'
																}
														})
														.state('siteadministration.rolemanagement.role', {
																abstract: true,
																views: {
																		'@siteadministration': {
																				template: '<div ui-view></div>'
																		},
																		'navigation@siteadministration': {
																				templateUrl: '/Security/Role/RoleNavigation',
																				controller: "RoleNavigationController as ctrl"
																		}
																},
																cache: false,
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/role');
																		}]
																}
														})
														.state('siteadministration.rolemanagement.role.roledetails', {
																title: 'Role Details',
																url: '/{RoleId:int}/RoleDetails',
																params: {
																		RoleId: { value: 0 }
																},
																views: {
																		'@siteadministration': {
																				templateUrl: '/Security/Role/RoleDetails',
																				controller: "roleModulesController as ctrl"
																		}
																},

																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/role');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-RoleManagement-RoleDetails'
																}
														})
														.state('siteadministration.rolemanagement.role.assignmodules', {
																title: 'Assign Modules',
																url: '/{RoleId:int}/AssignModules',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Security/Role/AssignModules',
																				controller: 'roleModulesController as ctrl',
																		},
																		'navigation@siteadministration': {
																				templateUrl: '/Security/Role/RoleNavigation'
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/role');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-RoleManagement-Assignmodules'
																}
														})
														.state('siteadministration.rolemanagement.role.assignmoduledetails', {
																title: 'Assign Module Details',
																url: '/{RoleId:int}/AssignModuleDetails?ModuleID',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Security/Role/AssignModuleDetails',
																				controller: 'roleManagementController as ctrl'
																		},
																		'navigation@siteadministration': {
																				templateUrl: '/Security/Role/RoleNavigation',
																				controller: "RoleNavigationController as ctrl"

																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/role');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-RoleManagement-Assignmodules'
																}
														})
														.state('siteadministration.sitesettings', {
																url: "/SiteSettings",
																templateUrl: '/Configuration/Settings/Index',
																controller: 'settingsController as ctrl',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/settings');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-Settings-Settings'
																}

														})
														.state('siteadministration.plugins', {
																url: "/Plugins",
																templateUrl: '/Configuration/Plugins/List'
														})
														.state('siteadministration.staffmanagement.user', {
																url: '/{UserID:int}',
																//'/:UserID?:int',//working example
																params: {
																		UserID: { value: 0 }
																},
																views: {
																		'@siteadministration': {
																				template: '<div ui-view></div>'
																		},
																		'navigation@siteadministration': {
																				template: '<div class="container-fluid nomargin nopadding">' +
																																'<div>' +
																																				'<div class="col-md-12 col-lg-12 right-nav">' +
																																								'<div work-flows-set work-flow-active-option="staffManagementComplete" work-flow-ready="workFlowReady" work-flow-model="workFlowModel">' +
																														'</div></div></div></div>',
																				controller: "staffManagementNavigationController as ctrl"
																		},
																		'header@siteadministration': {
																				templateUrl: '/Admin/UserHeader/UserHeader',
																				controller: 'userHeaderController as ctrl'
																		}
																},
																cache: false,
																resolve: {
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/staffManagementNavigation');
																		}]
																}
														})
														.state('siteadministration.staffmanagement.user.details', {
																title: 'User Details',
																url: '/Details',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/UserDetail/Index',
																				controller: "userDetailController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userDetail');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-UserDetails'
																}
														})
														.state('siteadministration.staffmanagement.user.roles', {
																title: 'User Roles',
																url: '/Roles',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/UserRole/Index',
																				controller: "userRoleController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userRole');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-RoleManagement-RoleDetails'
																}
														})
														.state('siteadministration.staffmanagement.user.credentials', {
																title: 'Credentials',
																url: '/Credentials',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/UserCredential/Index',
																				controller: "userCredentialController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userCredential');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-Credentials'
																}
														})
														.state('siteadministration.staffmanagement.user.divisionprogram', {
																title: 'Division & Programs',
																url: '/DivisionProgram',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/DivisionProgram/Index',
																				controller: "divisionProgramController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/divisionprogram');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-DivisionPrograms'
																}
														})
														.state('siteadministration.staffmanagement.user.scheduling', {
																title: 'Scheduling',
																url: '/Scheduling',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/UserScheduling/Index',
																				controller: "userSchedulingController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userscheduling');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-Scheduling'
																}
														})
														.state('siteadministration.staffmanagement.user.blockedtime', {
																title: 'Blocked Time',
																url: '/UserBlockedTime',
																views: {
																		'@siteadministration': {
																				templateUrl: '../BlockedTime/Index',
																				controller: "blockTimeController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/blockedtime');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-BlockedTime'
																}
														})
														.state('siteadministration.staffmanagement.user.directreports', {
																title: 'Direct Reports',
																url: '/DirectReports',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/UserDirectReports/Index',
																				controller: "userDirectReportsController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/UserDirectReports');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-DirectReports'
																}
														})
														.state('siteadministration.staffmanagement.user.profile', {
																title: 'User Profile',
																url: '/UserProfile',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Account/UserProfile/UserProfile',
																				controller: "userProfileController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userProfile');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-UserProfile'
																}
														})
														.state('siteadministration.staffmanagement.user.photos', {
																title: 'User Photos',
																url: '/Photos',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/UserPhoto/Index',
																				controller: "userPhotoController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userPhoto');
																		}],
																		photoScriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/photo');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-UserPhoto'
																}
														})
														.state('siteadministration.staffmanagement.user.additionaldetails', {
																title: 'Additional Details',
																url: '/AdditionalDetails',
																views: {
																		'@siteadministration': {
																				templateUrl: '/Admin/UserAdditionalDetails/Index',
																				controller: "userAdditionalDetailsController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/UserAdditionalDetails');
																		}]
																}
														})
														.state('businessadministration', {
																title: 'Business Administration',
																url: '/BusinessAdministration',
																templateUrl: '/BusinessAdmin/BusinessAdmin/Index',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/admin');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-UserPhoto' //This appears incorrect//This appears incorrect
																}
														})
														.state('businessadministration.clientmerge', {
																title: 'Client Merge',
																url: '/ClientMerge',
																templateUrl: '/BusinessAdmin/ClientMerge/Index',
																controller: 'clientMergeController as ctrl',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/clientMerge');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-StaffManagement-DivisionPrograms' //This appears incorrect
																}
														})
														.state('businessadministration.configuration', {
																title: 'Configuration',
																url: '/Configuration',
																templateUrl: '/BusinessAdmin/Configuration/Index',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}]
																},
														})
														.state('businessadministration.configuration.payors', {
																title: 'Payors',
																url: '/Payors',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PayorsConfiguration/Payors',
																				controller: "payorsController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="payorsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "payorsNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/payors');
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Payors
																}
														})
														.state('businessadministration.configuration.payors.initial', {
																title: 'Payor Plan',
																url: '/Payor',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PayorsConfiguration/PayorPlan',
																				controller: "payorPlansController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="payorsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "payorsNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/payorPlan');
																		}],
																		payorDetailsData: function () { return [] }
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Payors
																}
														})
														.state('businessadministration.configuration.payors.payorplans', {
																title: 'Payor Plan',
																url: '/Payor/{PayorID:int}',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PayorsConfiguration/PayorPlan',
																				controller: "payorPlansController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="payorsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "payorsNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/payorPlan');
																		}],
																		payorDetailsData: ['$state', '$stateParams', '$injector', '$timeout', '$q', 'helperService', function ($state, $stateParams, $injector, $timeout, $q, helperService) {
																				var dfd = $q.defer()
																				$timeout(function () {
																						var payorsService = $injector.get('payorsService');
																						return payorsService.getPayorByID($stateParams.PayorID).then(function (data) {
																								if (hasData(data)) {
																										var payorPlanState = "businessadministration.configuration.payors.payorplans";
																										helperService.replaceStateTitle(payorPlanState, data.DataItems[0].PayorName);
																										return dfd.resolve(data);
																								}
																								else {
																										alertService.error('OOPS! Something went wrong');
																										return dfd.resolve([]);;
																								}
																						},
																				function (errorStatus) {
																						alertService.error('OOPS! Something went wrong');
																						return dfd.resolve([]);;
																				});
																				})
																				return dfd.promise;
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Payors
																}
														})
														.state('businessadministration.configuration.payors.payorplans.initial', {
																title: 'Plan Details',
																url: '/PlanDetails',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PayorsConfiguration/PlanDetails',
																				controller: "planAddressesController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="payorsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "payorsNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/planDetails');
																		}],
																		planDetailsData: function () { return [] }
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Payors
																}
														})
														.state('businessadministration.configuration.payors.payorplans.plandetails', {
																title: 'Plan Details',
																url: '/PlanDetails/{PayorPlanID:int}',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PayorsConfiguration/PlanDetails',
																				controller: "planAddressesController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="payorsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "payorsNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/planDetails');
																		}],
																		planDetailsData: ['$state', '$stateParams', '$timeout', '$q', '$injector', 'helperService', function ($state, $stateParams, $timeout, $q, $injector, helperService) {
																				var dfd = $q.defer();
																				$timeout(function () {
																						var payorPlansService = $injector.get('payorPlansService');
																						return payorPlansService.getPayorPlanByID($stateParams.PayorPlanID).then(function (data) {
																								if (hasData(data)) {
																										var planDetailsState = "businessadministration.configuration.payors.payorplans.plandetails";
																										helperService.replaceStateTitle(planDetailsState, data.DataItems[0].PlanName);
																										return dfd.resolve(data);
																								}
																								else {
																										alertService.error('OOPS! Something went wrong');
																										return dfd.resolve([]);
																								}
																						},
																						function (errorStatus) {
																								alertService.error('OOPS! Something went wrong');
																								return dfd.resolve([]);
																						});
																				});
																				return dfd.promise;
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Payors
																}
														})
														.state('businessadministration.configuration.payors.payorplans.plandetails.initial', {
																title: 'Plan Address Details',
																url: '/AddressDetails',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PayorsConfiguration/PlanAddress',
																				controller: "planAddressController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="payorsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "payorsNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/planAddressDetails');
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Payors
																}
														})
														.state('businessadministration.configuration.payors.payorplans.plandetails.addressdetails', {
																title: 'Plan Address Details',
																url: '/AddressDetails/{PayorAddressID:int}',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PayorsConfiguration/PlanAddress',
																				controller: "planAddressController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="payorsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "payorsNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/planAddressDetails');
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Payors
																}
														})
														.state('businessadministration.configuration.organizationStructure', {
																title: 'Organization Structure',
																url: '',
																views: {
																		'@businessadministration': {
																				template: '<div ui-view></div>'
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="organizationStructureWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "organizationStructureNavigationController as ctrl"
																		},
																},
																resolve: {
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/organizationStructure');
																		}]
																}
														})
														.state('businessadministration.configuration.organizationStructure.company', {
																title: 'Company',
																url: '/Company',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/OrganizationStructure/Company',
																				controller: "companyConfigurationController as ctrl"
																		},
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/companyConfiguration');
																		}]
																}
														})
														.state('businessadministration.configuration.organizationStructure.division', {
																title: 'Divisions',
																url: '/Division',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/OrganizationStructure/Division',
																				controller: "divisionConfigurationController as ctrl"
																		},
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/divisionConfiguration');
																		}]
																}
														})
														.state('businessadministration.configuration.organizationStructure.programs', {
																title: 'Programs',
																url: '/Programs',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/OrganizationStructure/Programs',
																				controller: "programConfigurationController as ctrl"
																		},
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/programConfiguration');
																		}]
																}
														})
														.state('businessadministration.configuration.organizationStructure.programUnits', {
																title: 'Program Units',
																url: '/ProgramUnits',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/OrganizationStructure/ProgramUnits',
																				controller: "programUnitConfigurationController as ctrl"
																		},
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/programUnitConfiguration');
																		}]
																},
														})

														.state('businessadministration.configuration.services', {
																title: 'Services',
																url: '/Services',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/ServiceConfiguration/Services',
																				controller: "servicesController as ctrl"
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="servicesWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "servicesNavigationController as ctrl"
																		},
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/services');
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Company
																}
														})
														.state('businessadministration.configuration.services.servicesNavigation', {
																abstract: true,
																url: '',
																views: {
																		'@businessadministration': {
																				template: '<div ui-view></div>'
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="servicesWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "servicesNavigationController as ctrl"
																		},
																}
														})
														.state('servicedefinitioninitial', {
																title: 'Service Definition',
																parent: 'businessadministration.configuration.services.servicesNavigation',
																url: '/ServiceDefinition/Initial',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/ServiceConfiguration/ServiceDefinition',
																				controller: "serviceDefinitionController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/servicedefinition');
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Company
																}
														})
														.state('servicedefinition', {
																title: 'Service Definition',
																parent: 'businessadministration.configuration.services.servicesNavigation',
																url: '/ServiceDefinition/{ServicesID:int}',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/ServiceConfiguration/ServiceDefinition',
																				controller: "serviceDefinitionController as ctrl"
																		}
																},
																resolve: {
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/servicedefinition');
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Company
																}
														})
														.state('servicedetails', {
																title: 'Service Details',
																parent: 'businessadministration.configuration.services.servicesNavigation',
																url: '/ServiceDetails/{ServicesID:int}',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/ServiceConfiguration/ServiceDetails',
																				controller: "serviceDetailsController as ctrl"
																		}
																},
																resolve: {
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/servicedetails');
																		}]
																},
																data: {
																		permissionKey: BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Company
																}
														})
														.state('businessadministration.clientmerge.clientmergeNavigation', {
																abstract: true,
																url: '',
																views: {
																		'@businessadministration': {
																				template: '<div ui-view></div>'
																		},
																		'navigation@businessadministration': {
																				template: '<xen-workflows work-flow-options="clientMergeWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
																				controller: "clientMergeNavigationController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/clientmergeNavigation');
																		}]
																},
																data: {
																		permissionKey: 'BusinessAdministration-ClientMerge-ClientMerge'
																}
														})
														.state('businessadministration.clientmerge.clientmergeNavigation.potentialmatches', {
																title: 'Potential Matches',
																url: '/PotentialMatches',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PotentialMatches/Index',
																				controller: 'potentialMatchesController as ctrl',
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/PotentialMatches');
																		}]
																},
																data: {
																		permissionKey: 'BusinessAdministration-ClientMerge-ClientMerge'
																}

														})
														.state('businessadministration.clientmerge.clientmergeNavigation.potentialmatches.compareRecords', {
																title: 'Compare Records',
																url: '/CompareRecords/{contactID:int}/{compareContactID:int}',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/PotentialMatches/CompareRecords',
																				controller: 'compareRecordsController as ctrl',
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/CompareRecords');
																		}]
																},
																data: {
																		permissionKey: 'BusinessAdministration-ClientMerge-ClientMerge'
																}
														})
														.state('businessadministration.clientmerge.clientmergeNavigation.mergedContacts', {
																title: 'Merged Contacts',
																url: '/MergedContacts',
																views: {
																		'@businessadministration': {
																				templateUrl: '/BusinessAdmin/MergedContacts/Index',
																				controller: 'mergedContactsController as ctrl',
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/MergedContacts');
																		}]
																},
																data: {
																		permissionKey: 'BusinessAdministration-ClientMerge-ClientMerge'
																}
														})
														.state('userprofile', {
																url: '/UserProfile',
																templateUrl: '/Account/UserProfile/UserProfileMain',
																controller: 'userProfileController as ctrl',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userProfile');
																		}]
																}
														})
														.state('myprofile', {
																url: '/MyProfile',
																title: 'My Profile',
																templateUrl: 'Account/UserProfile/MyProfileMain',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/myProfile');
																		}]
																}
														})
														.state('myprofile.nav', {
																views: {
																		'navigation@myprofile': {
																				template: '<div class="container-fluid nomargin nopadding"><div><div class="col-md-12 col-lg-12 right-nav"><div work-flows-set work-flow-active-option="myProfileComplete" work-flow-ready="workFlowReady" work-flow-model="workFlowModel"></div></div></div></div>',
																				controller: 'myProfileNavigationController as navctrl'
																		},
																		'header@myprofile': {
																				templateUrl: '/Admin/UserHeader/UserHeader',
																				controller: 'profileHeaderController as headerctrl'
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/myProfileNav');
																		}]
																}
														})
														.state('myprofile.nav.profile', {
																title: 'User Profile',
																url: '/UserProfile',
																views: {
																		'@myprofile': {
																				templateUrl: '/Account/UserProfile/UserProfile',
																				controller: "userProfileController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userProfile');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-UserProfile'
																}
														})
														//ToDo: The following myprofile.nav states are yet to be developed and these states are just placeholders for now
														.state('myprofile.nav.security', {
																title: 'Security',
																url: '/Security',
																views: {
																		'@myprofile': {
																				templateUrl: '/Account/UserSecurity/Index',
																				controller: "userSecurityController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/usersecurity');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-SecurityDigitalSignature'
																}
														})
														.state('myprofile.nav.digitalsignature', {
																title: 'Digital Signature',
																url: '/DigitalSignature',
																views: {
																		'@myprofile': {
																				templateUrl: '/Account/UserDigitalSignature/Index',
																				controller: "userDigitalSignatureController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userDigitalSignatue');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-SecurityDigitalSignature'
																}
														})
														.state('myprofile.nav.credentials', {
																title: 'Credentials',
																url: '/Credentials',
																views: {
																		'@myprofile': {
																				templateUrl: '/Admin/UserCredential/Index',
																				controller: "userCredentialController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userCredential');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-Credentials'
																}
														})
														.state('myprofile.nav.divisionprograms', {
																title: 'Division & Programs',
																url: '/DivisionProgram',
																views: {
																		'@myprofile': {
																				templateUrl: '/Admin/DivisionProgram/Index',
																				controller: "divisionProgramController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/divisionprogram');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-DivisionPrograms'
																}
														})
														.state('myprofile.nav.scheduling', {
																title: 'Scheduling',
																url: '/Scheduling',
																views: {
																		'@myprofile': {
																				templateUrl: '/Account/UserScheduling/Index',
																				controller: "userSchedulingController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userscheduling');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-Scheduling'
																}
														})
														.state('myprofile.nav.directreports', {
																title: 'Direct Reports',
																url: '/DirectReports',
																views: {
																		'@myprofile': {
																				templateUrl: '/Admin/UserDirectReports/Index',
																				controller: "userDirectReportsController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/UserDirectReports');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-DirectReports'
																}
														})
														.state('myprofile.nav.userphotos', {
																title: 'User Photos',
																url: '/UserPhotos',
																views: {
																		'@myprofile': {
																				templateUrl: '/Admin/UserPhoto/Index',
																				controller: "userPhotoController as ctrl"
																		}
																},
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/userPhoto');
																		}],
																		photoScriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/photo');
																		}]
																},
																data: {
																		permissionKey: 'SiteAdministration-MyProfile-UserPhoto'
																}
														})
														.state('patientprofile.chart', {
																title: 'Chart',
																url: '/Charts',
																templateUrl: '/Tiles/Index',
																resolve: {
																		pinkyPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
																		}],
																		referencePromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/assessmentsGrid');
																		}],
																		scriptPromise: ['$http', function ($http) {
																				return lazyLoader.getScriptPromise($http, '/bundles/chartTileController');
																		}]
																},
																controller: "chartTileController as ctrl",
																data: {
																		permissionKey: ChartPermissionKey.Chart_Chart_Assessment
																}
														});
        }
    ])
    .run(['$rootScope', '$state', '$modal', '$stateParams', '$http', '$timeout', '$compile', '$document', '$injector', 'roleSecurityService', 'credentialSecurityService', 'registrationService', 'offlineData', 'alertService', 'formService', '$q', 'assessmentService', 'Idle', 'lookupService', 'globalObjectsService', '$filter', 'navigationService', 'settings', 'httpLoaderInterceptor', 'auditService',
        function ($rootScope, $state, $modal, $stateParams, $http, $timeout, $compile, $document, $injector, roleSecurityService, credentialSecurityService, registrationService, offlineData, alertService, formService, $q, assessmentService, Idle, lookupService, globalObjectsService, $filter, navigationService, settings, httpLoaderInterceptor, auditService) {

        		// BEGIN - Tdle Timeout

        		// start watching when the app runs. also starts the Keepalive service by default.
        		Idle.watch();

        		var model = null;
        		var CONFIG = {
        				controllerAction: "/Lookup/Lookup/",
        				offlineApiUrl: '/Lookup/GetLookupItems/'
        		};

        		//Injecting settings in rootScope so that it can be accessible in entire application.
        		$rootScope.applicationSettings = settings;
        		function getLookupItems() {
        				var dfd = $q.defer();
        				$http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetLookupItems', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl) }).success(function (data, status, header, config) {
        						dfd.resolve(data);
        				}).error(function (data, status, header, config) {
        						dfd.reject(status);
        				});
        				return dfd.promise;
        		};

        		$timeout(function () {
        				getLookupItems().then(function (data) {
        						LOOKUPTYPE = {};
        						for (var i = 0; i < data.length; ++i)
        								LOOKUPTYPE[data[i]] = data[i];
        				}, function (errorStatus) {
        						alertService("Error while getting look up data");
        				});
        		}, 2000);

        		function closeModals() {
        				if (model) {
        						$rootScope.$evalAsync(function () {
        								model.close();
        								model = null;
        						});
        				}
        		}
        		// $(document.body)
        		// .on('show.bs.modal', function () {
        		//     $rootScope.isPrinting = true;
        		// })
        		// .on('hidden.bs.modal', function () {
        		//    $rootScope.isPrinting = false;
        		//});
        		$rootScope.$on('IdleStart', function () {
        				// the user appears to have gone idle
        				closeModals();

        				model = $modal.open({
        						templateUrl: 'idle-timeout.html',
        						windowClass: 'modal-danger'
        				});

        				$timeout(function () {
        						angular.element("#idleTimeout").parents(':eq(2)').css("z-index", "9999");
        				});
        		});

        		$rootScope.$on('IdleTimeout', function () {
        				// the user has timed out (meaning idleDuration + timeout has passed without any activity)
        				closeModals();
        				$state.go('logout');
        		});

        		$rootScope.$on('IdleEnd', function () {
        				// the user has come back from AFK and is doing stuff. if you are warning them, you can use this to hide the dialog
        				console.log('Event > onIdleEnd');
        				if (model)
        						closeModals();
        		});

        		$rootScope.$on('Keepalive', function () {
        				// do something to keep the user's session alive
        				console.log('Event > onkeepalive');
        				if (model)
        						closeModals();
        		});

        		// END - Tdle Timeout

        		// BEGIN UI-Router State-related

        		// *********** For debugging purposes only  ****************
        		/*
										$rootScope.$on("$stateChangeError", console.log.bind(console));
										$rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
														console.log('went from ', fromState, ' to ', toState);
										});
										*/

        		$rootScope.openFlyout = function (contactID) {
        				$rootScope.$broadcast('openFlyout', { ContactID: contactID });
        		};
        		$rootScope.$state = $state;
        		$rootScope.$stateParams = $stateParams;
        		$rootScope.locationsComplete = { workFlowEnableAction: null };
        		$rootScope.staffManagementComplete = { workFlowEnableAction: null };
        		$rootScope.myProfileComplete = { workFlowEnableAction: null };

        		$rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
        				var defaultValidationState = ' ';
        				httpLoaderInterceptor.ignore(false);

        				/*********** on state change close floyout *********/
        				var patientFlyoutElement = $('#patientProfileFlyout');
        				if (patientFlyoutElement.hasClass('active')) {
        						patientFlyoutElement.removeClass('active');
        						$rootScope.isVoidedFlyout = false;
        				}

        				//myprofile validationhandler
        				if ((toState !== null && toState != undefined) && toState.name && (toState.name.indexOf('myprofile.nav') === 0)) {
        						if ((!((fromState != null) && (fromState.name.indexOf('myprofile.nav') === 0))) || ((fromState != null) && (!fromState.name))) {
        								$timeout(function () {
        										var workflowActions = $("ul li[data-state-name^=myprofile]").map(function () { return this.attributes['data-state-name'].value; }).get();
        										$http.get('/Account/UserProfile/NavigationValidationStates', { params: { workflowActions: workflowActions } })
																						.then(function (response) {
																								var data = eval(response.data);
																								for (var state in data) {
																										var stateObject = { stateName: state, validationState: data[state] };
																										$rootScope.myProfileRightNavigationHandler(stateObject);
																										if (workflowActions.indexOf(state) >= 0)
																												workflowActions.splice(workflowActions.indexOf(state), 1);
																								}
																						}).finally(function () {
																								for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
																										$rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
																								}
																						});
        								});
        						}
        						$document.scrollTop(0);
        				}

        				//siteadministration location detail validation handler
        				if ((toState != null) && toState.name && (toState.name.indexOf('siteadministration.configuration.locations.details.general') === 0)) {
        						console.log('Eventr > $stateChangeSuccess > fromState: ' + fromState);
        						console.log('Eventr > $stateChangeSuccess > toState: ' + toState);

        						if (((fromState != null) && fromState.name && (fromState.name.indexOf('siteadministration.configuration.locations') === 0) &&
														(fromParams.UserID === null || fromParams.UserID === undefined || fromParams.UserID === 0)) || ((fromState != null) && (!fromState.name))) {
        								var userID = toParams.UserID ? toParams.UserID : 0;

        								$timeout(function () {
        										//todo : remove after validation call complete
        										var stateObject = { stateName: "siteadministration.configuration.locations.details.general", validationState: 'valid' };
        										$rootScope.locationRightNavigationHandler(stateObject);

        										var workflowActions = $("ul li[data-state-name^=siteadministration]").map(function () { return this.attributes['data-state-name'].value; }).get();
        										if (userID !== 0) {
        												$http.get('/Admin/UserDetail/NavigationValidationStates', { params: { userID: userID, workflowActions: workflowActions } })
																								.then(function (response) {
																										var data = eval(response.data);
																										for (var state in data) {
																												var stateObject = { stateName: state, validationState: data[state] };
																												$rootScope.locationRightNavigationHandler(stateObject);
																												if (workflowActions.indexOf(state) >= 0)
																														workflowActions.splice(workflowActions.indexOf(state), 1);
																										}
																								})
																								.finally(function () { // whether it succeeded or not, stop any remaining spinners ...
																										for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
																												$rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
																										}
																								});
        										}
        										else if (!((fromState != null) && fromState.name && (fromState.name === 'siteadministration.configuration.locations.details.general'))) {
        												//default everything to a neutral state and stop all spinners
        												for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
        														$rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
        												}
        										}
        								});

        						}
        						$document.scrollTop(0);
        				}


        				//siteadministration validation handler
        				if ((toState != null) && toState.name && (toState.name.indexOf('siteadministration.staffmanagement.user') === 0)) {
        						if (((fromState != null) && fromState.name && (fromState.name.indexOf('siteadministration.staffmanagement') === 0) &&
																		(fromParams.UserID === null || fromParams.UserID === undefined || fromParams.UserID === 0)) || ((fromState != null) && (!fromState.name))) {
        								var userID = toParams.UserID ? toParams.UserID : 0;

        								$timeout(function () {
        										var workflowActions = $("ul li[data-state-name^=siteadministration]").map(function () { return this.attributes['data-state-name'].value; }).get();
        										if (userID !== 0) {
        												if ($injector.has('appointmentService')) {
        														var appointmentService = $injector.get('appointmentService');
        														var isBlockedTimeValid = false;

        														appointmentService.getAppointmentByResource(userID, 2).then(function (response) {
        																if (response.DataItems != null && response.DataItems.length > 0) {
        																		isBlockedTimeValid = true;
        																}
        																$http.get('/Admin/UserDetail/NavigationValidationStates', { params: { userID: userID, workflowActions: workflowActions } })
																												.then(function (response) {
																														var data = eval(response.data);
																														for (var state in data) {
																																var stateObject = { stateName: state, validationState: data[state] };
																																if (state.indexOf('siteadministration.staffmanagement.user.blockedtime') >= 0) {
																																		stateObject.validationState = isBlockedTimeValid ? 'valid' : stateObject.validationState;
																																}
																																$rootScope.staffManagementRightNavigationHandler(stateObject);
																																if (workflowActions.indexOf(state) >= 0)
																																		workflowActions.splice(workflowActions.indexOf(state), 1);
																														}
																												})
																												.finally(function () { // whether it succeeded or not, stop any remaining spinners ...
																														for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
																																$rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
																														}
																												});
        														});
        												}
        												else {
        														$http.get('/Admin/UserDetail/NavigationValidationStates', { params: { userID: userID, workflowActions: workflowActions } })
																										.then(function (response) {
																												var data = eval(response.data);
																												for (var state in data) {
																														var stateObject = { stateName: state, validationState: data[state] };
																														$rootScope.staffManagementRightNavigationHandler(stateObject);
																														if (workflowActions.indexOf(state) >= 0)
																																workflowActions.splice(workflowActions.indexOf(state), 1);
																												}
																										})
																										.finally(function () { // whether it succeeded or not, stop any remaining spinners ...
																												for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
																														$rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
																												}
																										});
        												}
        										}
        										else if (!((fromState != null) && fromState.name && (fromState.name === 'siteadministration.staffmanagement.user.details'))) {
        												//default everything to a neutral state and stop all spinners
        												for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
        														$rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
        												}
        										}
        								});
        						}
        						$document.scrollTop(0);
        				}
        		});

        		$rootScope.locationRightNavigationHandler = function (stateDetail) {
        				if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(stateDetail.stateName)) {
        						if (stateDetail.stateName === "siteadministration.configuration.locations.details.general" && stateDetail.validationState === "valid") {
        								$rootScope.$broadcast(stateDetail.stateName, { validationState: stateDetail.validationState });
        								$rootScope.locationsComplete = { workFlowEnableAction: 0 };
        						}
        						$rootScope.workflowActions[stateDetail.stateName].validationState = stateDetail.validationState;
        				}
        				$rootScope.$broadcast(stateDetail.stateName, { validationState: stateDetail.validationState });
        		};

        		$rootScope.myProfileRightNavigationHandler = function (stateDetail) {
        				$rootScope.$broadcast(stateDetail.stateName, { validationState: stateDetail.validationState });
        				$rootScope.myProfileComplete = { workFlowEnableAction: 0 };
        		};

        		$rootScope.staffManagementRightNavigationHandler = function (stateDetail) {
        				if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(stateDetail.stateName)) {
        						if ((stateDetail.stateName === "siteadministration.staffmanagement.user.details" || stateDetail.EnableAllWorkFlow === true) && stateDetail.validationState === "valid") {
        								$rootScope.staffManagementComplete = { workFlowEnableAction: 0 };
        						}
        						$rootScope.workflowActions[stateDetail.stateName].validationState = stateDetail.validationState;
        				}
        				$rootScope.$broadcast(stateDetail.stateName, { validationState: stateDetail.validationState });
        		};

        		// END UI-Router State-related

        		// BEGIN Offline Capability -related

        		//$q.all([offlineData.initialize($http)]);

        		$rootScope.syncNow = function () {
        				offlineData.syncNow();
        		};

        		$rootScope.setFakeOffline = function (value) {
        				$http.defaults.headers.common['X-Fake-Offline'] = value.toString();
        		};

        		$rootScope.connectivityStatus = {
        				IsOnline: offlineData.isOnline(),
        				LastSync: new Date(),
        				IsSynching: false
        		};

        		var offlineListener = function () {
        				$rootScope.connectivityStatus.IsOnline = offlineData.isOnline();
        				if (offlineData.isOnline())
        						alertService.success('The application can once again reach the server. You are now working in online mode.');
        				else
        						alertService.warning('The application is unable to reach the server. You are now working in offline mode.');
        		};

        		var syncListener = function (event) {
        				$rootScope.connectivityStatus.IsSynching = event.detail.isSynching;
        				if (event.detail.isSynching) {
        						$rootScope.connectivityStatus.LastSync = new Date();
        						$state.transitionTo('autosync', {}, { notify: false }); // change the url
        				}
        		};

        		//window.addEventListener(offlineData.stateChangedEventName, offlineListener);
        		//window.addEventListener(offlineData.syncChangedEventName, syncListener);

        		navigationService.get();        //Added so user data can be cached for offline use

        		// END Offline Capability -related

        		// BEGIN Form Service -related
        		$rootScope.setform = function (value, formName) {
        				formService.initForm(value, formName);
        		};

        		$rootScope.formReset = function (frmObj, formName) {
        				$rootScope.$evalAsync(function () {
        						frmObj && angular.isFunction(frmObj.$setPristine) && frmObj.$setPristine();
        						formService.reset(formName);
        				});
        		};

        		$rootScope.onCancel = function (callBack) {
        				var isDirty = formService.isDirty();
        				if (isDirty) {
        						bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
        								callBack(result);
        						});
        				}
        				else {
        						callBack(true);
        				}
        		}

        		//This is used to toggle the theme of the application based on the theme-swapping button
        		$rootScope.useDarkTheme = true;

        		// END Form Service -related

        		// BEGIN Miscellaneous

        		$rootScope.promiseNoOp = function () {
        				var deferred = $q.defer();

        				$timeout(function () {
        						deferred.resolve(angular.noop());
        				});

        				return deferred.promise;
        		};

        		$rootScope.resetMask = function (elem, scope) {
        				$rootScope.$evalAsync(function () {
        						$timeout(function () {
        								$compile(elem)(scope);
        						});
        				});
        		};

        		$rootScope.triggerTypeahead = function (element) {
        				$('[name="' + element + '"]').focus();
        				$('[name="' + element + '"]').trigger('forcelyOpenTypeaheadPopup');
        		};

        		// Typeahead comparator on empty or match
        		$rootScope.comparatorOnEmptyOrMatch = function (actual, expected) {
        				if (expected == ' ') {
        						return true;
        				}
        				if (actual && expected)
        						return (actual.toString()).toLowerCase().indexOf(expected.toLowerCase()) > -1;
        		};

        		$rootScope.globalConstant = {
        				"emailPattern": /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/i
        		};

        		$rootScope.$on('$stateChangeStart', function () {
        				globalObjectsService.isViewContentLoaded = false;
        		});

        		$rootScope.$on('$locationChangeSuccess', function (evt, pageUrl) {
        				if (pageUrl.indexOf('AutoSync') > 0 && pageUrl.indexOf('Progress') < 0) {
        						$state.go('autosync.progress');// load sync progress page
        				}

        				if ($state.current.data && $state.current.data.permissionKey) {
        						var contactID = $stateParams.ContactID ? $stateParams.ContactID : null;
        						var pageAudit = {
        								DataKey: $state.current.data.permissionKey,
        								ActionTypeID: SCREEN_ACTIONTYPES.View,
        								ContactID: contactID
        						}

        						auditService.addScreenAudit(pageAudit);
        				}
        				// for App Insights telemetry - appInsights variable is created in _Layout view
        				appInsights.trackPageView($state.current.name);
        		});

        		$rootScope.$on('$viewContentLoaded',
														function (event, viewConfig) {
																formService.reset();
																credentialSecurityService.applyCredentialSecurity(true);
																//adding attribute autocomplete to all the input elements of page - to disable the autofill feature of browser
																//timeout is added to force it to run at end - for partial views
																$timeout(function () {
																		$("input").attr("autocomplete", "off");
																}, 0);
														});

        		$rootScope.setFocusToGrid = function (gridId, e) {
        				var grid = $('#' + gridId);
        				if (grid != null && grid.bootstrapTable('getData').length > 0) {
        						if (e != undefined) {
        								e.preventDefault();
        						}
        						grid.triggerHandler('focus');
        				}
        		};

        		$rootScope.showTileDetails = function (state, contactID, editID, params) {
        				if (params == undefined) {

        						$state.go(state, { ContactID: contactID }).then(function () {
        								$rootScope.$broadcast('showDetails', { id: editID });
        						});
        				}
        				else {
        						if (angular.lowercase(state).indexOf('allergy') >= 0) {
        								$state.go(state, params).then(function () {
        										$rootScope.$broadcast('showDetails', { id: editID });
        								});
        						}
        						else
        								$state.go(state, params);
        				}
        		}

        		// END Miscellaneous

        		// Showtime! Default redirect to Contact screen.
        		if ($state.name === undefined)
        				$state.go("contacts");

        		$rootScope.TileColumnViewStates = [];

        		$rootScope.promiseWrap = function (promiseAction) {
        				var deferred = $q.defer();
        				promiseAction.then(function () {
        						deferred.resolve();
        				});
        				return deferred.promise;
        		};

        		$rootScope.progressPctComplete = 0;
        		$rootScope.progressStatus = 0;

        		$rootScope.validateClientIdentifierTypeID = function (alternateID, clientAlternateIDs) {
        				var currentID = alternateID.ClientIdentifierTypeID;
        				if (currentID && currentID > 0) {
        						var clientAlternateIds = angular.copy(clientAlternateIDs);
        						alternateID.ExpirationReasonID = alternateID.ExpirationDate ? alternateID.ExpirationReasonID : '';
        						var newCollection = $filter('filter')(clientAlternateIds, function (data) {
        								data.EffectiveDate = data.EffectiveDate ? $filter("formatDate")(data.EffectiveDate) : "";
        								data.ExpirationDate = data.ExpirationDate ? $filter("formatDate")(data.ExpirationDate) : "";
        								return data.IsActive === true && (data.ClientIdentifierTypeID === Number(currentID));
        						});

        						var nullCollection = $filter('filter')(newCollection, function (item) {
        								return !item.ExpirationDate || item.ExpirationDate == "";
        						});

        						if (nullCollection.length > 1) {
        								alertService.error("Other type id is already selected");
        								alternateID.ClientIdentifierTypeID = 0;
        								return false;
        						}

        						var nonNullCollection = $filter('filter')(newCollection, function (item) {
        								return item.ExpirationDate && item.ExpirationDate != "";
        						});

        						var orderedCollection = $filter('orderBy')(nonNullCollection, 'ExpirationDate');

        						if (nullCollection.length == 1) {
        								orderedCollection = orderedCollection.concat(nullCollection);
        						}

        						var items = $filter('filter')(orderedCollection, function (item, i) {
        								if (item.ExpirationDate && (orderedCollection[i + 1])) {
        										var expDate = new Date(item.ExpirationDate);
        										var nextEffDate = new Date(orderedCollection[i + 1].EffectiveDate);
        										return !expDate || expDate >= nextEffDate;
        								}
        								return false;
        						});

        						if (items.length > 0) {
        								alertService.error("Other type id is already selected");
        								alternateID.ClientIdentifierTypeID = 0;
        								return false;
        						}
        				}
        		};

        		$rootScope.resolvePermission = function (value) {
        				if (value == undefined && value == null)
        						return PERMISSION.NONE;
        				else if (value != 0)
        						return PERMISSION.UPDATE;
        				else
        						return PERMISSION.CREATE;
        		};

        		$rootScope.getLookupsByType = function (lookupType) {
        				return lookupService.getLookupsByType(lookupType)
        		};

        		$rootScope.getOrganizationByDataKey = function (dataKey, parentID, noSecurity) {
        				return lookupService.getOrganizationByDataKey(dataKey, parentID, noSecurity);
        		};

        		$rootScope.getPreferredContactMethod = function () {
        				return $filter('filter')(lookupService.getLookupsByType('ContactMethod'), { IsSystem: true });
        		}

        		var progressWatch = $rootScope.$watch('progressPctComplete', function (newValue) {
        				if (newValue === 100) {
        						progressWatch();
        						$timeout(function () { $rootScope.progressStatus = 1; }, 4000);
        				}
        		});

        		if (window.applicationCache) {
        				window.applicationCache.addEventListener('progress', function (e) {
        						if ((e.loaded) && (e.total)) {
        								$rootScope.progressPctComplete = Math.floor((e.loaded * 100) / e.total);
        						}
        				});

        				window.applicationCache.addEventListener('error', function (e) {
        						if (('reason' in e) && (e.reason === 'resource')) {
        								$rootScope.progressStatus = -1;
        								alertService.error('An unexpected error occurred while saving the application for offline use. The application cannot function in offline mode. Please contact Application Support.');
        						} else if (('reason' in e) && (e.reason === 'manifest')) {
        								$rootScope.progressPctComplete = 100;
        						}
        				});

        				window.applicationCache.addEventListener('noupdate', function (e) {
        						$rootScope.progressPctComplete = 100;
        				});
        		}
        }
    ]);
