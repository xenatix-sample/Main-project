angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('ContactBenefitController', function () {
    var scope, benefitServiceMock, q, alertServiceMock;
    var obj = {
        DataItems: [{
            test: 123,
            DOB: '/DATE(1437737993)/',
            FullCodeDNR: false,
            MailPermission: true,
            ContactID: 1,
            Addresses: {
                city: 'delhi',
                country: 'India'
            }
        }]
    };
    var id = 20;
    beforeEach(function () {
        module('xenatixApp');
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, contactBenefitService, alertService, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        alertServiceMock = alertService;
        benefitServiceMock = contactBenefitService;
        $httpBackend = $injector.get('$httpBackend');
        //mock Services
        var getDeferred = q.defer();
        var addDeferred = q.defer();
        var editDeferred = q.defer();
        var removeDeferred = q.defer();
        $httpBackend.whenGET(/.*/).respond('login');
        spyOn(benefitServiceMock, 'get').andReturn(getDeferred.promise);
        getDeferred.resolve(obj);
        spyOn(benefitServiceMock, 'add').andReturn(addDeferred.promise);
        addDeferred.resolve({ ID: id });
        spyOn(benefitServiceMock, 'update').andReturn(editDeferred.promise);
        editDeferred.resolve({ ID: id });
        spyOn(benefitServiceMock, 'remove').andReturn(removeDeferred.promise);
        removeDeferred.resolve(obj);
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(window, 'confirm').andReturn(true);
        // Declare controller with mocked services
        $controller('ContactBenefitController', {
            $scope: scope,
            contactBenefitService: benefitServiceMock,
            alertService: alertServiceMock,
        });

    }));
    it('Get Contact', function () {
        scope.get(1);
        scope.$apply();
        expect(scope.newBenefit).toEqual(jasmine.any(Object));
        expect(Object.keys(scope.newBenefit).length).toBeGreaterThan(0);
    });
    it('Save Contact', function () {
        scope.newBenefit = obj;
        scope.save(false, false, false);
        scope.$apply();
        expect(scope.newBenefit.ContactID).toEqual(id);
    });
    it('Edit Contact', function () {
        scope.newBenefit = obj;
        scope.edit();
        scope.$apply();
        expect(scope.newBenefit.ContactID).toEqual(id);
    });
    it('Cancel', function () {
        scope.newBenefit = obj;
        scope.cancel();
        var check = { Phones: [], MailPermission: '0', FullCodeDNR: '0', IsPregnant: '0' };
        expect(scope.newBenefit).toEqual(check);
    });
    it('Remove', function () {
        scope.newBenefit = obj;
        scope.remove(obj);
        expect(scope.newBenefit).toEqual(obj);
    });
});