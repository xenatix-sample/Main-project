angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });

angular.module('xenatixApp')
    .factory('roleSecurityService', function () {
        return { applySecurity: true }
    });

describe('patientProfileController', function () {
    var scope, patientProfileService, deferred, q, $httpBackend;
   
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
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        $httpBackend = $injector.get('$httpBackend');
        $httpBackend.whenGET(/.*/).passThrough();
        $controller('patientProfileController', {
            $scope: scope,
            $httpBackend: $httpBackend
        });

    }));
    it('Get Patient Profile', function () {
        scope.contactID = 1;
        scope.header = {};
        scope.getPatientProfileData(1);
        scope.$apply();
        waitsFor(function () {
            return Object.keys(scope.header).length != 0;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.header).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.header).length).toBeGreaterThan(0);
        });
    });
  
});