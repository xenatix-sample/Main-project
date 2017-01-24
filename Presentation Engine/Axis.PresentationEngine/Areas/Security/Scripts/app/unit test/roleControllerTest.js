angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('role controller', function () {
    var scope, roleServiceMock, q, alertServiceMock;
    /* declare our mocks out here
     * so we can use them through the scope 
     * of this describe block.
     */
    var obj = {
        DataItems: [{
            test: 123,
            DOB: '/DATE(1437737993)/',
            FullCodeDNR: false,
            MailPermission: true,
            ContactID: 1
        }]
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
        { Name: "Create", PermissionID: 1, Selected: false },
        { Name: "Read", PermissionID: 2, Selected: true },
        { Name: "Delete", PermissionID: 3, Selected: false },
        { Name: "Append", PermissionID: 4, Selected: true },
        { Name: "Share", PermissionID: 6, Selected: true }
        ]
    };
    // This function will be called before every "it" block.
    // This should be used to "reset" state for your tests.

    beforeEach(function () {
        // load the module you're testing.
        module('xenatixApp');
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, roleService, alertService, $injector) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        alertServiceMock = alertService;
        roleServiceMock = roleService;
        $httpBackend = $injector.get('$httpBackend');
        var getRolesDeferred = q.defer();
        var getRoleIDDeferred = q.defer();
        var AssignedPermissionDeferred = q.defer();
        var getModuleByRoleIDDeferred = q.defer();
        var getFeatureByModuleIdDeferred = q.defer();
        var removeDeferred = q.defer();
        var addDeferred = q.defer();
        var updateDeferred = q.defer();
        var assignRolePermissionDeferred = q.defer();
        // Create a "spy object" for our someService.
        // This will isolate the controller we're testing from
        // any other code.
        // we'll set up the returns for this later 
        $httpBackend.whenGET(/.*/).respond('login');

        spyOn(roleServiceMock, 'getRoles').andReturn(getRolesDeferred.promise);
        getRolesDeferred.resolve(obj);
        spyOn(roleServiceMock, 'getRoleById').andReturn(getRoleIDDeferred.promise);
        getRoleIDDeferred.resolve(obj);
        spyOn(roleServiceMock, 'getAssignedPermissionByModuleId').andReturn(AssignedPermissionDeferred.promise);
        AssignedPermissionDeferred.resolve(obj);
        spyOn(roleServiceMock, 'getModuleByRoleId').andReturn(getModuleByRoleIDDeferred.promise);
        getModuleByRoleIDDeferred.resolve(obj);
        spyOn(roleServiceMock, 'getFeatureByModuleId').andReturn(getFeatureByModuleIdDeferred.promise);
        getFeatureByModuleIdDeferred.resolve(moduleFeature);
        spyOn(roleServiceMock, 'remove').andReturn(removeDeferred.promise);
        removeDeferred.resolve('Deleted');
        spyOn(roleServiceMock, 'assignModuleToRole').andReturn(removeDeferred.promise);
        removeDeferred.resolve('Assigned');
        spyOn(roleServiceMock, 'update').andReturn(updateDeferred.promise);
        updateDeferred.resolve('updated');
        spyOn(roleServiceMock, 'assignRolePermission').andReturn(assignRolePermissionDeferred.promise);
        assignRolePermissionDeferred.resolve('assigned');
        obj.ResultCode = 0;
        spyOn(roleServiceMock, 'add').andReturn(addDeferred.promise);
        addDeferred.resolve(obj);


        spyOn(alertServiceMock, 'success').andReturn('success');
        // INJECT! This part is critical
        // $rootScope - injected to create a new $scope instance.
        // $controller - injected to create an instance of our controller.
        // $q - injected so we can create promises for our mocks.
            // create a scope object for us to use.
            // set up the returns for our someServiceMock
            // $q.when('weee') creates a resolved promise to "weee".
            // this is important since our service is async and returns
            // a promise.

            // now run that scope through the controller function,
            // injecting any services or other injectables we need.
            // **NOTE**: this is the only time the controller function
            // will be run, so anything that occurs inside of that
            // will already be done before the first spec.
            ctrl = $controller('roleController', {
                $scope: scope,
                roleService: roleServiceMock,
                alertService: alertServiceMock
            });
    }));

     /* Test 1: The simplest of the simple.
     * here we're going to test that some things were 
     * populated when the controller function whas evaluated. */
    it('Get Roles', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.getRoles()
        scope.$apply();
        expect(scope.roles).toEqual(obj.DataItems);
    });
    it('Get Role By ID', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.getRoleById();
        scope.$apply();
        expect(scope.newRole).toEqual(obj.DataItems[0]);
    });
    it('Get Assigned Permission By ModuleId', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.getAssignedPermissionByModuleId(1);
        scope.$apply();
        expect(scope.modulePermissions).toEqual({ ModuleID: 1, Permissions: obj.DataItems });
    });
    it('Get Module By RoleId', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.getModuleByRoleId(1);
        scope.$apply();
        expect(scope.modules).toEqual(obj.DataItems);
    });
    it('Get Feature Module By RoleId', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.getFeatureByModuleId(1);
        scope.$apply();
        expect(scope.moduleFeatures).toEqual(moduleFeature.DataItems);
    });
    it('Get Feature Permissions', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.getFeatureByModuleId(1);
        scope.$apply();
        console.log(scope.featurePermissionNames);
        expect(scope.featurePermissionNames).toEqual(jasmine.any(Object));
        expect(Object.keys(scope.featurePermissionNames).length).toBeGreaterThan(0);
    });
    it('Assign Role Permission', function () {
        scope.roleId = 1;
        scope.moduleFeatures = moduleFeature.DataItems;
        scope.modulePermissions = permissions;
        scope.assignRolePermission();
        scope.$apply();
        expect(scope.moduleFeatures).toEqual(moduleFeature.DataItems);
    });
    it('Assign Module To Role', function () {
        scope.roleId = 1;
        scope.modules = [{ Selected: true }, { Selected: true }, { Selected: false }];
        scope.assignModuleToRole();
        scope.$apply();
        expect(scope.roles).toEqual(obj.DataItems);
    });
    it('Add', function () {
        scope.setMode(1);
        scope.roleId = 5;
        scope.newRole = {};
        scope.save(false, false, false);
        scope.$apply();
        expect(scope.newRole.RoleID).toEqual(scope.roleId);
        expect(scope.roles).toEqual(obj.DataItems);
    });
    it('Update', function () {
        scope.setMode(1);
        scope.newRole = {};
        scope.newRole.RoleID = 1;
        scope.save(false, false, false);
        scope.$apply();
        expect(scope.roles).toEqual(obj.DataItems);
        expect(scope.newRole).toEqual(obj.DataItems[0]);
    });
    it('Set Mode', function () {
        scope.setMode(1);
        expect(scope.mode).toEqual(1);
    });
    it('Cancel', function () {
        scope.newRole = obj;
        scope.cancel();
        expect(scope.newRole).toEqual({});
    });
    it('Remove', function () {
        scope.remove(1);
        scope.$apply();
        expect(scope.roles).toEqual(obj.DataItems);
    });
});