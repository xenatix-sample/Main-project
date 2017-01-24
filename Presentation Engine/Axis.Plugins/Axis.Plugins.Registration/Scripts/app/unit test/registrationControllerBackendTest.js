angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('registrationController', function () {
    var scope, registrationService, deferred, q, alertServiceMock, $httpBackend;
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
        $controller('RegistrationController', {
            $scope: scope,
            alertService: alertServiceMock,
            $httpBackend: $httpBackend
        });

    }));
    it('Get Registration Data', function () {
        scope.get(1);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newDemography).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.newDemography).length).toBeGreaterThan(0);
        });
    });
    it('Edit Registration Data', function () {
        saveData.ContactID = 29;
        scope.newDemography = saveData;
        scope.save(false, false, false);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newDemography.ContactID).toEqual(29);
            expect(Object.keys(scope.newDemography).length).toBeGreaterThan(0);
        });
    });
    it('Add New Registration Data', function () {
        scope.newDemography = saveData;
        scope.save();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.newDemography.ContactID).not.toEqual(0);
            expect(Object.keys(scope.newDemography).length).toBeGreaterThan(0);
        });
    });
});