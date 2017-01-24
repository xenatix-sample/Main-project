
/// <reference path="//cdnjs.cloudflare.com/ajax/libs/jasmine/1.3.1/jasmine.js" />
/// <reference path="//cdnjs.cloudflare.com/ajax/libs/jasmine/1.3.1/jasmine-html.js" />

/// <reference path="jquery-1.10.2.js" />
/// <reference path="angular.js" />
/// <reference path="angular-mocks.js" />
/// <reference path="../../../../../Axis.PresentationEngine/Scripts/library/angular-ui-router/angular-ui-router.min.js" />
/// <reference path="../../../../../Axis.PresentationEngine/Scripts/library/angular-ui/ui-bootstrap-tpls.js" />
/// <reference path="../../../../../Axis.PresentationEngine/Scripts/app/helpers/common.js" />
/// <reference path="../../../../../Axis.PresentationEngine/Scripts/library/angular" />
/// <reference path="../../../../../Axis.PresentationEngine/Scripts/plugins" />
/// <reference path="../../../../../Axis.PresentationEngine/Scripts/app/app.js" />
/// <reference path="../controllers/roomScheduleController.js" />


angular.module('xenatixApp.settings', [])
    .constant('settings', {
        "webApiBaseUrl": "http://localhost:5050"
});



describe("roomScheduleController", function () {

    var scope, resourceServiceMock, q, lookupServiceMock;
    var $controller;

    beforeEach(function () {
        module('xenatixApp');
    });

    beforeEach(inject(function (_$rootScope_, _$controller_, _$q_, _$filter_, resourceService, _$stateParams_, lookupService) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        scope = _$rootScope_.$new();
        q = _$q_;
        resourceServiceMock = resourceService;
        lookupServiceMock = lookupService;
        $httpBackend = $injector.get('$httpBackend');

        var getMasterDeferred = q.defer();
        var getDeferred = q.defer();
        var addDeferred = q.defer();
        var editDeferred = q.defer();
        var removeDeferred = q.defer();

        $controller("roomScheduleController", {
            $scope: scope,                     
            resourceService: resourceServiceMock,
            lookupService: lookupServiceMock,
        });
    }));

    it("should add room availability", function () {

        var $scope = {};
        var controller = $controller("roomScheduleController", {
            $scope: scope,
            resourceService: resourceServiceMock,
            lookupService: lookupServiceMock,
        });

        $scope.AddRoomAvailability();
        expect($scope.addedRoomAvailabiliyID).toBe(6);
    });
});             


//describe("general", function () {
//    it("A basic test", function () {
//        expect(true).toBeTruthy();
//        var value = "hello";
//        expect("hello").toEqual(value);
//    });
//});

//describe("stringLib", function () {
//    it("will get vowel count", function () {
//        var count = stringLib.vowels("hello");
//        expect(count).toEqual(2);
//    });
//});

//describe("mathLib", function () {
//    beforeEach(function () {
//        console.log("beforeEach");
//    });
//    it("will add 5 to number", function () {
//        var res = mathLib.add5(10);
//        expect(res).toEqual(15);
//    });

//    it("will multiply 5 to number", function () {
//        var res = mathLib.mult5(10);
//        expect(res).toEqual(55);
//    });
//});