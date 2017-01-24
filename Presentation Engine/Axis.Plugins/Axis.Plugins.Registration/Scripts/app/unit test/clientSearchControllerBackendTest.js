angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('clientSearchController', function () {
    var scope, clientSearchServiceMock, deferred, q, alertServiceMock, $httpBackend;
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
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, clientSearchService, alertService, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        $httpBackend = $injector.get('$httpBackend');
        alertServiceMock = alertService;
        clientSearchServiceMock = clientSearchService;
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(alertServiceMock, 'error').andReturn('error');
        $httpBackend.whenGET(/.*/).passThrough();
        $controller('clientSearchController', {
            $scope: scope,
            clientSearchService: clientSearchServiceMock,
            alertService: alertServiceMock,
            $httpBackend: $httpBackend
        });
    }));

    it('Get Client Summary', function () {
        scope.getClientSummary('fir');
        scope.$apply();
        waitsFor(function () {
            return !scope.isLoading;
        }, "call is done", 10500);
        runs(function () {
            expect(scope.contacts).toEqual(jasmine.any(Object));
            expect(Object.keys(scope.contacts).length).toBeGreaterThan(0)
        });
    });
});