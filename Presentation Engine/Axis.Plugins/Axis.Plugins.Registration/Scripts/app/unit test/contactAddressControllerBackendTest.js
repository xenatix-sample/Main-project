angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
angular.module('xenatixApp')
    .factory('roleSecurityService', function () {
        return { applySecurity: true }
    });
describe('contactAddressController', function () {
    var scope, deferred, q, alertServiceMock, $httpBackend, formServiceMock;
    var obj = [{
        AddressID: 3,
        AddressTypeID: 1,
        City: "test",
        ContactAddressID: 2,
        ContactID: 1,
        County: 282,
        IsPrimary: true,
        Line1: "test",
        Line2: "test",
        StateProvince: 2,
        Zip: "21212-1212"
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
        ctrl = $controller('contactAddressController', {
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
            return Object.keys(scope.addressList).length > 0;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.addressList).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.addressList).length).toBeGreaterThan(0);
        });
    });

    it('Remove email data', function () {
        scope.addressList = obj;
        scope.remove(3);
        scope.$apply();
        $('.modal-footer button[data-bb-handler="confirm"]').click();
        waitsFor(function () {
            return Object.keys(scope.addressList).length > 0;
        }, "call is done", 10500);
        runs(function () {
            expect(Object.keys(scope.addressList).length).toBeGreaterThan(0);
        });
    });

    it('Add/Update data', function () {
        scope.addressList = obj;
        scope.save();
        scope.$apply();
        waitsFor(function () {
            return Object.keys(scope.addressList).length > 0;
        }, "call is done", 10500);
        runs(function () {
            expect(Object.keys(scope.addressList).length).toBeGreaterThan(0);
        });
    });

});