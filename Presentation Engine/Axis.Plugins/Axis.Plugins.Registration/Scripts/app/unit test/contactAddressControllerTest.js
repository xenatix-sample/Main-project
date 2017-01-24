angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
angular.module('xenatixApp')
    .factory('roleSecurityService', function () {
        return { applySecurity: true }
    });

describe('contactAddressController', function () {
    var scope, contactAddressServiceMock, q, alertServiceMock, ctrl, formServiceMock;
    var obj = {
        DataItems: [{
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
        }]
    };
    var formHtml = '<form role="form" name="ctrl.emailForm"></form>';
    var id = 1;
    beforeEach(function () {
        module('xenatixApp', function (lazyLoaderProvider) {
            lazyLoaderProvider = lazyLoaderProvider;
        });
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, _$compile_, contactAddressService, alertService, $injector, formService) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();

        q = _$q_;
        var compile = _$compile_;
        alertServiceMock = alertService;
        contactAddressServiceMock = contactAddressService;
        formServiceMock = formService;

        $httpBackend = $injector.get('$httpBackend');
        //mock Services
        var getDeferred = q.defer();
        var addDeferred = q.defer();
        var editDeferred = q.defer();
        var removeDeferred = q.defer();
        $httpBackend.whenGET(/.*/).respond('login');
        spyOn(contactAddressServiceMock, 'get').andReturn(getDeferred.promise);
        getDeferred.resolve(obj);
        spyOn(contactAddressServiceMock, 'addUpdate').andReturn(addDeferred.promise);
        addDeferred.resolve({ ID: id });
        spyOn(contactAddressServiceMock, 'remove').andReturn(removeDeferred.promise);
        removeDeferred.resolve(obj);
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(formServiceMock, 'isDirty').andReturn(true);
        spyOn(window, 'confirm').andReturn(true);
        scope.ctrl = {};
        // Declare controller with mocked services
        ctrl = $controller('contactAddressController', {
            $scope: scope,
            contactAddressService: contactAddressServiceMock,
            alertService: alertServiceMock,
            formService: formServiceMock
        });
        compile(formHtml)(scope);

    }));
    it('Get Contact', function () {
        scope.get(1);
        scope.$apply();
        expect(scope.addressList).toEqual(jasmine.any(Object));
        expect(scope.addressList.length).toBeGreaterThan(0);
    });
    it('Save Contact', function () {
        scope.Addresses = obj.DataItems;
        scope.save();
        scope.$apply();
        expect(scope.addressList.length).toBeGreaterThan(0);
    });
    it('Remove', function () {
        event = scope.$emit("click");
        expect(event).toBeDefined();
        scope.remove(id, event);
        scope.$apply();
        expect(scope.addressList.length).toBeGreaterThan(0);
    });
});