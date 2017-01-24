angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
angular.module('xenatixApp')
    .factory('roleSecurityService', function () {
        return { applySecurity: true }
    });
describe('Referral Disposition Detail', function () {
    var scope, referralDispositionServiceMock, q, alertServiceMock;
    var referralDispositionDetailID = 1;
    var referralHeaderID = 1;
    var obj = {
        DataItems: [{
            ReferralHeaderID: referralHeaderID,
            ReferralDispositionID: 2,
            ReferralDispositionOutcomeID: 1,
            UserID: 1,
            AdditionalNotes: "Additional Note Test"
        }]
    };
    var formHtml = '<form role="form" name="ctrl.referralDispositionForm"></form>';
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
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, _$compile_, referralDispositionService, alertService, $injector) {

        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        var compile = _$compile_;
        alertServiceMock = alertService;
        referralDispositionServiceMock = referralDispositionService;
        $httpBackend = $injector.get('$httpBackend');

        //mock Services
        var getReferralDispositionDetailDeferred = q.defer();
        var addUpdateReferralDispositionDetailDeferred = q.defer();

        $httpBackend.whenGET(/.*/).respond('login');

        // Get referral dispoistion detail Promise
        spyOn(referralDispositionServiceMock, 'getReferralDispositionDetail').andReturn(getReferralDispositionDetailDeferred.promise);
        getReferralDispositionDetailDeferred.resolve(obj);


        // Add referral disposition detail promise
     
        spyOn(referralDispositionServiceMock, 'addReferralDispositionDetail').andReturn(addUpdateReferralDispositionDetailDeferred.promise);
        addUpdateReferralDispositionDetailDeferred.resolve(function (data) { ReferralDispositionDetailID: referralDispositionDetailID });

        // update referral disposition detail promise
        spyOn(referralDispositionServiceMock, 'updateReferralDispositionDetail').andReturn(addUpdateReferralDispositionDetailDeferred.promise);
        addUpdateReferralDispositionDetailDeferred.resolve(function (data) { ReferralDispositionDetailID: referralDispositionDetailID });
        
        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(window, 'confirm').andReturn(true);

        // Declare controller with mocked services
        $controller('referralDispositionController', {
            $scope: scope,
            referralDispositionService: referralDispositionServiceMock,
            alertService: alertServiceMock,
        });
        compile(formHtml)(scope);
    }));
    it('Get Referral Disposition Detail', function () {
        scope.ReferralHeaderID = referralHeaderID;
        scope.get();
        scope.$apply();
        expect(scope.referralDispositionDetail).toEqual(jasmine.any(Object));
        expect(Object.keys(scope.referralDispositionDetail).length).toBeGreaterThan(0);
    });
    it('Add Disposition Details', function () {
        scope.referralDispositionDetail = obj.DataItems[0];
        var isUpdate = false;
        scope.saveReferralDispositionDetail(isUpdate);
        scope.$apply();
        expect(referralDispositionDetailID).not.toBe(undefined);
    });
    it('update Disposition Details', function () {
        scope.referralDispositionDetail = obj.DataItems[0];
        var isUpdate = true;
        scope.saveReferralDispositionDetail(isUpdate);
        scope.$apply();
        expect(referralDispositionDetailID).not.toBe(undefined);
    });
});