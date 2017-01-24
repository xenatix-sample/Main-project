
angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('additionalDemographyController', function () {
    var scope, additionalDemographyServiceMock, q, alertServiceMock;
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
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, additionalDemographyService, alertService, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        alertServiceMock = alertService;
        additionalDemographyServiceMock = additionalDemographyService;
        $httpBackend = $injector.get('$httpBackend');
        //mock Services
        var getMasterDeferred = q.defer();
        var getDeferred = q.defer();
        var deleteDeferred = q.defer();
        var updateDeferred = q.defer();
        var addDeferred = q.defer();

        $httpBackend.whenGET(/.*/).respond('login');
        spyOn(additionalDemographyServiceMock, 'getAdditionalDemographicMasterData').andReturn(getMasterDeferred.promise);
        getMasterDeferred.resolve(obj);
        spyOn(additionalDemographyServiceMock, 'getAdditionalDemographic').andReturn(getDeferred.promise);
        getDeferred.resolve(obj);
        spyOn(additionalDemographyServiceMock, 'deleteAdditionalDemographic').andReturn(deleteDeferred.promise);
        deleteDeferred.resolve(obj);
        obj.ResultCode = 0;
        spyOn(additionalDemographyServiceMock, 'addAdditionalDemographic').andReturn(addDeferred.promise);
        addDeferred.resolve(obj);
        spyOn(additionalDemographyServiceMock, 'updateAdditionalDemographic').andReturn(updateDeferred.promise);
        updateDeferred.resolve(obj);

        spyOn(alertServiceMock, 'success').andReturn('success');
        spyOn(window, 'confirm').andReturn(true);
        // Declare controller with mocked services
        $controller('additionalDemographyController', {
            $scope: scope,
            additionalDemographyService: additionalDemographyServiceMock,
            alertService: alertServiceMock,
        });

    }));
    it('Get Master Data', function () {
        scope.getAdditionalDemographicMasterData();
        scope.$apply();
        expect(scope.additionalDemographicMaster).toEqual(obj.DataItems[0]);
        expect(Object.keys(scope.additionalDemographicMaster).length).toBeGreaterThan(0);
    });
    it('Get Data', function () {
        scope.get();
        scope.$apply();
        expect(scope.additionalDemographic).toEqual(obj.DataItems[0]);
        expect(Object.keys(scope.additionalDemographic).length).toBeGreaterThan(0);
    });
    it('Add', function () {
        scope.additionalDemographic = {};
        scope.additionalDemographic.AdditionalDemographicID = 0;
        scope.contactId = 1;
        scope.save(false, false, false);
        scope.$apply();
        expect(scope.additionalDemographic).toEqual(obj.DataItems[0]);
        expect(Object.keys(scope.additionalDemographic).length).toBeGreaterThan(0);
    });
    it('Update', function () {
        scope.additionalDemographic = {};
        scope.additionalDemographic.AdditionalDemographicID = 11;
        scope.contactId = 1;
        scope.save(false, false, false);
        scope.$apply();
        expect(scope.additionalDemographic).toEqual(obj.DataItems[0]);
        expect(Object.keys(scope.additionalDemographic).length).toBeGreaterThan(0);
    });
    it('Cancel', function () {
        scope.additionalDemographicForm = {
            $setPristine: function () { }
        };
        scope.contactName = 'Test';
        scope.additionalDemographic = obj;
        scope.cancel();
        expect(scope.additionalDemographic.Name).toEqual('Test');
    });
    it('Delete', function () {
        scope.additionalDemographic = obj;
        scope.delete(1);
        expect(scope.additionalDemographic).toEqual(obj);
        expect(Object.keys(scope.additionalDemographic).length).toBeGreaterThan(0);
    });

});