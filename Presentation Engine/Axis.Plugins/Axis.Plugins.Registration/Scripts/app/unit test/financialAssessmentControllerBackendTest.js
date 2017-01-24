angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('FinancialAssessmentController', function () {
    var scope, financialAssessmentService, deferred, q, alertServiceMock, $httpBackend;
    var saveData = {
        ContactID: 1,
        FinancialAssessmentID: 1,
        AssessmentDate: "Wed Aug 05 2015 00:00:00 GMT+0530 (India Standard Time)",
        TotalIncome: 300000,
        TotalExpenses: 30000,
        AdjustedGrossIncome: 5000,
        FamilySize: 2
    };
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
        $httpBackend.whenGET(/.*/).passThrough();
        $httpBackend.whenPUT(/.*/).passThrough();
        $controller('FinancialAssessmentController', {
            $scope: scope,
            alertService: alertServiceMock,
            $httpBackend: $httpBackend
        });

    }));
    it('Get FinancialAssessment Data', function () {
        scope.get(1);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.financialAssessment).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.financialAssessment).length).toBeGreaterThan(0);
        });
    });
    it('Edit FinancialAssessment Data', function () {
        saveData.ContactID = 29;
        scope.copyOfFinancialAssessment = saveData;
        scope.save(false, false, false);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.copyOfFinancialAssessment.ContactID).toEqual(29);
            expect(Object.keys(scope.copyOfFinancialAssessment).length).toBeGreaterThan(0);
        });
    });
    it('Add New FinancialAssessment Data', function () {
        scope.copyOfFinancialAssessment = saveData;
        scope.save();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.copyOfFinancialAssessment.ContactID).not.toEqual(0);
            expect(Object.keys(scope.copyOfFinancialAssessment).length).toBeGreaterThan(0);
        });
    });
});