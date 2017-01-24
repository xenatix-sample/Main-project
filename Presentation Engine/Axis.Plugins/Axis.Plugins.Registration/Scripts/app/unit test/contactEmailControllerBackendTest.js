angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
angular.module('xenatixApp')
    .factory('roleSecurityService', function () {
        return { applySecurity: true }
    });
describe('contactEmailController', function () {
    var scope, contactEmailServiceMock, deferred, q, alertServiceMock, $httpBackend, formServiceMock;
    var obj = [{
        Email: 'test@xenatix.com',
        EmailID: 1,
        EmailPermissionID: 1,
        IsPrimary: true,
        ContactEmailID: 1
    }];
    var id = 1;
    var formHtml = '<form role="form" name="ctrl.emailForm"></form>';
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
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, _$compile_, alertService, $injector, formService) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        var compile = _$compile_;
        $httpBackend = $injector.get('$httpBackend');
        alertServiceMock = alertService;
        formServiceMock = formService;
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(formServiceMock, 'isDirty').andReturn(true);
        $httpBackend.whenGET(/.*/).passThrough();
        $httpBackend.whenPOST(/.*/).passThrough();
        $httpBackend.whenDELETE(/.*/).passThrough();
        scope.ctrl = {};
        // Declare controller with mocked services
        ctrl = $controller('contactEmailController', {
            $scope: scope,
            alertService: alertServiceMock,
            $httpBackend: $httpBackend,
            formService: formServiceMock
        });
        compile(formHtml)(scope);

    }));
    it('Get email data', function () {
        scope.get(id);
        scope.$apply();
        waitsFor(function () {
            return Object.keys(scope.emailList).length > 0;
        }, "call is done", 10500);
        runs(function () {            
            expect(scope.emailList).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.emailList).length).toBeGreaterThan(0);
        });
    });

    it('Remove email data', function () {
        event = scope.$emit("click");
        expect(event).toBeDefined();
        scope.contactID = id;
        scope.remove(id, event);
        scope.$apply();
        $('.modal-footer button[data-bb-handler="confirm"]').click();
        waitsFor(function () {
            return Object.keys(scope.emailList).length > 0;
        }, "call is done", 10500);
        runs(function () {
            expect(Object.keys(scope.emailList).length).toBeGreaterThan(0);
        });
    });

    it('Add/Update data', function () {
        scope.Emails=obj;
        scope.contactID = id;
        scope.save();
        scope.$apply();        
        waitsFor(function () {
            return Object.keys(scope.emailList).length > 0;
        }, "call is done", 10500);
        runs(function () {
            expect(Object.keys(scope.emailList).length).toBeGreaterThan(0);
        });
    });
    
});