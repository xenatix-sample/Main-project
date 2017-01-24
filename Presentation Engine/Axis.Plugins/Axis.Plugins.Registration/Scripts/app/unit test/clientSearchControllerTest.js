angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('clientSearchController', function () {
    var scope, clientSearchServiceMock, q, alertServiceMock;
    var obj = {
        DataItems: [{
            test: 123,
            DOB: '/DATE(1437737993)/',
            FullCodeDNR: false,
            MailPermission: true,
            ContactID: 1
        }]
    };
    beforeEach(function () {
        module('xenatixApp');
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, clientSearchService, alertService, $injector) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        alertServiceMock = alertService;
        clientSearchServiceMock = clientSearchService;
        $httpBackend = $injector.get('$httpBackend');
        //mock Services
        var getMasterDeferred = q.defer();

        $httpBackend.whenGET(/.*/).respond('login');
        spyOn(clientSearchServiceMock, 'getClientSummary').andReturn(getMasterDeferred.promise);
        getMasterDeferred.resolve(obj);

        spyOn(alertServiceMock, 'success').andReturn('success');
        // Declare controller with mocked services
        $controller('clientSearchController', {
            $scope: scope,
            clientSearchService: clientSearchServiceMock,
            alertService: alertServiceMock,
        });
    }));
    it('Get Client Summary', function () {
        scope.getClientSummary('search');
        scope.$apply();
        expect(scope.contacts).toEqual(obj.DataItems);
    });
});