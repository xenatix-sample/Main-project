angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('registrationController', function () {
    var scope, deferred, q, alertServiceMock, $httpBackend;
    var roleId = 1;
    var saveData = {
        Age: 0,
        AlternateID: "121212",
        DOB: "Wed Aug 05 2015 00:00:00 GMT+0530 (India Standard Time)",
        DOBStatusID: 1,
        FirstName: "asasas",
        FullCodeDNR: "true",
        GenderID: 1,
        LastName: "asasas",
        ReferralSourceID: 4,
        SSN: "212121212",
        SSNStatusID: 3,
        SchoolDistrict: "1",
        SmokingStatusID: 8,
        Suffix: "mr"
    };
    var moduleFeature = {
        DataItems: [{
            Description: "Demography",
            FeatureID: 1,
            FeatureName: "Demography",
            ModuleFeatureID: 1,
            ModuleId: 1,
            ModuleName: "Registration",
            Permissions: [
                { Name: "Create", PermissionID: 1, Selected: true },
                { Name: "Read", PermissionID: 2, Selected: false },
                { Name: "Delete", NotMapped: true, PermissionID: 3, Selected: true },
                { Name: "Append", PermissionID: 4, Selected: false },
                { Name: "Share", NotMapped: true, PermissionID: 6, Selected: true }

            ]
        }, {
            Description: "Allergy",
            FeatureID: 2,
            FeatureName: "Allergy",
            ModifiedBy: 1,
            ModifiedOn: null,
            ModuleFeatureID: 2,
            ModuleId: 1,
            ModuleName: "Registration",
            Permissions: [
                { Name: "Create", PermissionID: 1, Selected: false },
                { Name: "Read", PermissionID: 2, Selected: true },
                { Name: "Delete", PermissionID: 3, Selected: false },
                { Name: "Append", PermissionID: 4, Selected: true },
                { Name: "Share", PermissionID: 6, Selected: false }
            ]
        }]
    };
    var permissions = {
            ModuleID: 1,
            permission: [
            { Name: "Create", PermissionID: 1,  Selected: false },
            { Name: "Read", PermissionID: 2, Selected: true },
            { Name: "Delete", PermissionID: 3,  Selected: false },
            { Name: "Append", PermissionID: 4, Selected: true },
            { Name: "Share", PermissionID: 6, Selected: true }
    ]};
    beforeEach(function () {
        module('xenatixApp');
        module('ngMockE2E');
        angular.mock.module(function ($provide) {

            //retrieve the $httpBackend from module ng and override $delegate from ngMockE2E
            angular.injector(['ng'])
            .invoke(function ($httpBackend) {
                $provide.value('$delegate', $httpBackend);
            });

            //retrieve the $httpBackend from module ng and override $delegate from ngMockE2E
            angular.injector(['ngMockE2E'])
            .invoke(['$httpBackend', function (_$httpBackend_) {
                $httpBackend = _$httpBackend_;
            }]);

            $provide.value('$httpBackend', $httpBackend);
        });
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, alertService, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        $httpBackend = $injector.get('$httpBackend');
        alertServiceMock = alertService;
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(alertServiceMock, 'error').andReturn('error');
        $httpBackend.whenGET(/.*/).passThrough();
        $httpBackend.whenPOST(/.*/).passThrough();
        $httpBackend.whenDELETE(/.*/).passThrough();
        $controller('roleController', {
            $scope: scope,
            alertService: alertServiceMock,
            $httpBackend: $httpBackend
        });

    }));
    it('Get Role By ID', function () {
        scope.roleId = roleId;
        scope.getRoleById();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newRole).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.newRole).length).toBeGreaterThan(0);
        });
    });
    it('Get Role By Name', function () {
        scope.roleName = 'Test';
        scope.getRoles();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading1;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.roles).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.roles).length).toBeGreaterThan(0);
        });
    });
    it('Get Feature By ModuleId', function () {
        scope.roleId = roleId;
        scope.getFeatureByModuleId(1);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.moduleFeatures).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.moduleFeatures).length).toBeGreaterThan(0);
        });
    });
    it('Get Feature Permissions', function () {
        scope.roleId = roleId;
        scope.getFeatureByModuleId(1);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.featurePermissionNames).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.featurePermissionNames).length).toBeGreaterThan(0);
        });
    });
    it('Get Module By RoleID', function () {
        scope.roleId = roleId;
        scope.getModuleByRoleId();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            console.log(scope.roles);
            expect(scope.modulesAssignedToRole).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.modulesAssignedToRole).length).toBeGreaterThan(0);
        });
    });
    it('Assign Role Permission', function () {
        scope.roleId = roleId;
        scope.moduleFeatures = moduleFeature.DataItems;
        scope.modulePermissions = permissions;
        scope.assignRolePermission();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.moduleFeatures).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.moduleFeatures).length).toBeGreaterThan(0);
        });
    });
    /*it('Remove Role', function () {
        scope.remove(5);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.roles).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.roles).length).toBeGreaterThan(0);
        });
    });*/
    it('Add Role', function () {
        scope.roleId = 0;
        scope.newRole = { Name: "asas12", Description: "asasas12", RoleID: 0 };
        scope.save(false, false, false);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newRole.RoleID).not.toEqual(0);
        });
    });
    it('Update Role', function () {
        scope.roleId = 0;
        scope.newRole = {RoleID: 11, Name: "test1234", Description: "test1234", ModifiedBy: 1};
        scope.save();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.roles).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.roles).length).toBeGreaterThan(0);
        });
    });
});