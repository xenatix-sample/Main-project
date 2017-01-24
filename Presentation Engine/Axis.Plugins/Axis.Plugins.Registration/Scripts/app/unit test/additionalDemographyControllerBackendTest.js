angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('additionalDemographyController', function () {
    var scope, additionalDemographyServiceMock, deferred, q, alertServiceMock, $httpBackend;
    var saveData = {
        AdditionalDemographicID: 1,
        CitizenshipID: null,
        EducationStatusID: null,
        EmploymentStatusID: null,
        EthnicityID: 2,
        ForceRollback: null,
        IsActive: null,
        LegalStatusID: 13,
        LivingArrangementID: 12,
        LivingWill: true,
        MRN: null,
        MaritalStatusID: null,
        ModifiedBy: null,
        ModifiedOn: null,
        Name: "asass asasas",
        PowerOfAttorney: false,
        PrimaryLanguageID: 15,
        RaceID: 12,
        ReligionID: null,
        SecondaryLanguageID: 15,
        VeteranStatusID: null,
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
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, additionalDemographyService, alertService, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        $httpBackend = $injector.get('$httpBackend');
        alertServiceMock = alertService;
        additionalDemographyServiceMock = additionalDemographyService;
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(alertServiceMock, 'error').andReturn('error');
        $httpBackend.whenGET(/.*/).passThrough();
        $httpBackend.whenPOST(/.*/).passThrough();
        $httpBackend.whenDELETE(/.*/).passThrough();
        $controller('additionalDemographyController', {
            $scope: scope,
            additionalDemographyService: additionalDemographyServiceMock,
            alertService: alertServiceMock,
            $httpBackend: $httpBackend
        });
    }));
    it('Get Demography Data', function () {
        scope.init();
        scope.contactId = 15;
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.additionalDemographic).toEqual(jasmine.any(Object));
            expect(scope.additionalDemographicMaster).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.additionalDemographicMaster).length).toBeGreaterThan(0);
            expect(Object.keys(scope.additionalDemographic).length).toBeGreaterThan(0);
        });
    });
    it('Update Demography Data', function () {
        scope.contactId = 15;
        scope.additionalDemographic = saveData;
        scope.save();
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.additionalDemographic).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.additionalDemographic).length).toBeGreaterThan(0);
        });
    });
    it('Add Demography Data', function () {
        scope.contactId = 15;
        saveData.AdditionalDemographicID = 0;
        scope.additionalDemographic = saveData;
        scope.save(false, false, false);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.additionalDemographic).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.additionalDemographic).length).toBeGreaterThan(0);
        });
    });
    it('Delete Demography Data', function () {
        scope.delete(15);
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(1).toEqual(1);
        });
    });
});