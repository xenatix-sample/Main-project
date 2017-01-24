angular.module('xenatixApp.settings', [])
        .constant('settings', {
            "webApiBaseUrl": "http://localhost:5050"
        });
describe('forgot password controller', function () {
    var scope, forgottenPasswordServiceMock, q, alertServiceMock;
    /* declare our mocks out here
     * so we can use them through the scope 
     * of this describe block.
     */
    var obj = {
        DataItems: [{
            ADFlag: true,
            ADUserPasswordResetMessage: 'Test Message'
        }]
    };
    
    // This function will be called before every "it" block.
    // This should be used to "reset" state for your tests.
    beforeEach(function () {
        // load the module you're testing.
        module('xenatixApp');
    });
    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, forgottenPasswordService, alertService, $injector) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        q = _$q_;
        alertServiceMock = alertService;
        forgottenPasswordServiceMock = forgottenPasswordService;
        $httpBackend = $injector.get('$httpBackend');
        var sendResetLinkDeferred = q.defer();
        // Create a "spy object" for our someService.
        // This will isolate the controller we're testing from
        // any other code. we'll set up the returns for this later 
        $httpBackend.whenGET(/.*/).respond('login');

        spyOn(forgottenPasswordService, 'sendResetLink').andReturn(sendResetLinkDeferred.promise);
        sendResetLinkDeferred.resolve(obj);

        spyOn(alertServiceMock, 'success').andReturn('success');
        // INJECT! This part is critical
        // $rootScope - injected to create a new $scope instance.
        // $controller - injected to create an instance of our controller.
        // $q - injected so we can create promises for our mocks.
            // create a scope object for us to use.
            // set up the returns for our someServiceMock
            // $q.when('weee') creates a resolved promise to "weee".
            // this is important since our service is async and returns
            // a promise.

            // now run that scope through the controller function,
            // injecting any services or other injectables we need.
            // **NOTE**: this is the only time the controller function
            // will be run, so anything that occurs inside of that
            // will already be done before the first spec.
            ctrl = $controller('forgotPasswordController', {
                $scope: scope,
                forgotPasswordService: forgotPasswordServiceMock,
                alertService: alertServiceMock
            });
    }));

     /* Make sure we get a custom message back instead of sending an email */
    it('Send Email Link AD User', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.sendResetLink(emailADUser)
        scope.$apply();
        expect(scope.resetObject).toEqual(obj.DataItems);
    });
    /* Make sure we do not get a custom message back instead of sending an email */
    it('Send Email Link Non-AD User', function () {
        //just assert. $scope was set up in beforeEach() (above)
        scope.sendResetLink(emailNonADUser);
        scope.$apply();
        expect(scope.newRole).toEqual(obj.DataItems[0]);
    });
});