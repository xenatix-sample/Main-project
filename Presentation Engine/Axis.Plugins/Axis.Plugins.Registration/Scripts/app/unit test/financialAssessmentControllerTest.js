angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('FinancialAssessmentController', function () {
    var scope, financialAssessmentServiceMock, q, alertServiceMock;
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
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, financialAssessmentService, alertService, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        alertServiceMock = alertService;
        financialAssessmentServiceMock = financialAssessmentService;
        $httpBackend = $injector.get('$httpBackend');
        //mock Services
        var getDeferred = q.defer();
        var addDeferred = q.defer();
        var editDeferred = q.defer();
        var removeDeferred = q.defer();
        $httpBackend.whenGET(/.*/).respond('login');
        spyOn(financialAssessmentServiceMock, 'get').andReturn(getDeferred.promise);
        getDeferred.resolve(obj);
        spyOn(financialAssessmentServiceMock, 'add').andReturn(addDeferred.promise);
        addDeferred.resolve({ ID: id });
        spyOn(financialAssessmentServiceMock, 'update').andReturn(editDeferred.promise);
        editDeferred.resolve({ ID: id });
        spyOn(financialAssessmentServiceMock, 'remove').andReturn(removeDeferred.promise);
        removeDeferred.resolve(obj);
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(window, 'confirm').andReturn(true);
        // Declare controller with mocked services
        $controller('FinancialAssessmentController', {
            $scope: scope,
            financialAssessmentService: financialAssessmentServiceMock,
            alertService: alertServiceMock,
        });

    }));
    it('Get FinancialAssessment', function () {
        scope.get(1);
        scope.$apply();
        expect(scope.financialAssessment).toEqual(jasmine.any(Object));
        expect(Object.keys(scope.financialAssessment).length).toBeGreaterThan(0);
    });
    it('Save FinancialAssessment', function () {
        scope.financialAssessment = obj;
        scope.save(false, false, false);
        scope.$apply();
        expect(scope.financialAssessment.ContactID).toEqual(id);
    });
    it('Edit FinancialAssessment', function () {
        scope.financialAssessment = obj;
        scope.edit();
        scope.$apply();
        expect(scope.financialAssessment.ContactID).toEqual(id);
    });
    it('Cancel', function () {
        scope.financialAssessment = obj;
        scope.cancel();
        var check = { Phones: [], MailPermission: '0', FullCodeDNR: '0', IsPregnant: '0' };
        expect(scope.financialAssessment).toEqual(check);
    });
    it('Remove', function () {
        scope.financialAssessment = obj;
        scope.remove(obj);
        expect(scope.financialAssessment).toEqual(obj);
    });
});