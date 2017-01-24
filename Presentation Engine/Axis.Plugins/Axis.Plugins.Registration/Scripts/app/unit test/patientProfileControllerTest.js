angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
angular.module('xenatixApp')
    .factory('roleSecurityService', function () {
        return { applySecurity: true }
    });

describe('PatientProfileController', function () {
    var scope, patientProfileServiceMock, q, alertServiceMock;
    var id = 1;
    var obj = {
        DataItems: [{
            ContactID: 1,
            ClientTypeID: 1,
            FirstName: "FirstName",
            Middle: "MiddleName",
            LastName: "LastName",
            GenderID: 1,            
            PreferredName: "PreferredName"
        }]
    };

    beforeEach(function () {
        module('xenatixApp');
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, patientProfileService, $injector) {
        // create a scope object for us to use.
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        patientProfileServiceMock = patientProfileService;
        $httpBackend = $injector.get('$httpBackend');
        //mock Services
        var getDeferred = q.defer();
        $httpBackend.whenGET(/.*/).respond('login');
        spyOn(patientProfileServiceMock, 'get').andReturn(getDeferred.promise);
        getDeferred.resolve(obj);
       
        // Declare controller with mocked services
        ctrl = $controller('patientProfileController', {
            $scope: scope,
            patientProfileService: patientProfileServiceMock,
            alertService: alertServiceMock,
        });
       
    }));
    it('Get PatientProfile', function () {
        scope.contactID = 1;
        scope.getPatientProfileData();
        scope.$apply();
        expect(scope.header).toEqual(jasmine.any(Object));
        expect(Object.keys(scope.header).length).toBeGreaterThan(0);
    });
    
});