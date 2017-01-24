angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('ContactBenefitController', function () {
    var scope, contactBenefitService, deferred, q, alertServiceMock, $httpBackend;
    var saveData = {
        ContactID: 0,
        ContactPayorID: 1,
        ContactPayorTypeID: 1,
        PayorPlanID: 1,
        Deductable: 4,
        EffectiveDate: "Wed Aug 05 2015 00:00:00 GMT+0530 (India Standard Time)",
        ExpirationDate: "Wed Aug 05 2015 00:00:00 GMT+0530 (India Standard Time)",
        AddRetroDate: "Wed Aug 05 2015 00:00:00 GMT+0530 (India Standard Time)",
        ID1: "RAS345",
        ID2: "RAS453"
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
        $controller('ContactBenefitController', {
            $scope: scope,
            alertService: alertServiceMock,
            $httpBackend: $httpBackend
        });

    }));
    it('Get Benefit Data', function () {
        scope.get(1);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newBenefit).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.newBenefit).length).toBeGreaterThan(0);
        });
    });
    it('Edit Benefit Data', function () {
        saveData.ContactID = 29;
        scope.newBenefit = saveData;
        scope.save(false, false, false);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newBenefit.ContactID).toEqual(29);
            expect(Object.keys(scope.newBenefit).length).toBeGreaterThan(0);
        });
    });
    it('Add New Benefit Data', function () {
        scope.newBenefit = saveData;
        scope.save(false, false, false);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newBenefit.ContactID).not.toEqual(0);
            expect(Object.keys(scope.newBenefit).length).toBeGreaterThan(0);
        });
    });
});