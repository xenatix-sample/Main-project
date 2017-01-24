angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
angular.module('xenatixApp')
    .factory('roleSecurityService', function () {
        return { applySecurity: true }
    });

describe('contactEmailController', function () {
    var scope, contactEmailServiceMock, q, alertServiceMock, ctrl, formServiceMock;
    var obj = {
        DataItems: [{
            Email: 'test@xenatix.com',
            EmailID: 1,
            EmailPermissionID: 1,
            IsPrimary: true,
            ContactEmailID: 1
        }]
    };
    var formHtml = '<form role="form" name="ctrl.emailForm"></form>';
    var id = 1;
    beforeEach(function () {
        module('xenatixApp', function (lazyLoaderProvider) {
            lazyLoaderProvider = lazyLoaderProvider;
        });
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, _$compile_, contactEmailService, alertService, $injector, formService) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;      
        scope = $rootScope.$new();

        q = _$q_;
        var compile = _$compile_;
        alertServiceMock = alertService;
        contactEmailServiceMock = contactEmailService;
        formServiceMock = formService;

        $httpBackend = $injector.get('$httpBackend');
        //mock Services
        var getDeferred = q.defer();
        var addDeferred = q.defer();
        var editDeferred = q.defer();
        var removeDeferred = q.defer();
        $httpBackend.whenGET(/.*/).respond('login');
        spyOn(contactEmailServiceMock, 'get').andReturn(getDeferred.promise);
        getDeferred.resolve(obj);
        spyOn(contactEmailServiceMock, 'addUpdate').andReturn(addDeferred.promise);
        addDeferred.resolve({ ID: id });
        spyOn(contactEmailServiceMock, 'remove').andReturn(removeDeferred.promise);
        removeDeferred.resolve(obj);
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(formServiceMock, 'isDirty').andReturn(true);
        spyOn(window, 'confirm').andReturn(true);
        scope.ctrl = {};
        // Declare controller with mocked services
        ctrl = $controller('contactEmailController', {
            $scope: scope,
            contactEmailService: contactEmailServiceMock,
            alertService: alertServiceMock,
            formService: formServiceMock
        });
        compile(formHtml)(scope);

    }));
    it('Get Contact', function () {
        scope.get(1);
        scope.$apply();
        expect(scope.emailList).toEqual(jasmine.any(Object));
        expect(scope.emailList.length).toBeGreaterThan(0);
    });
    it('Save Contact', function () {        
        scope.Emails = obj.DataItems;
        scope.save();
        scope.$apply();       
        expect(scope.emailList.length).toBeGreaterThan(0);
    });   
    it('Remove', function () {
        event = scope.$emit("click");
        expect(event).toBeDefined();
        scope.remove(id,event);
        scope.$apply();
        expect(scope.emailList.length).toBeGreaterThan(0);
    });
});