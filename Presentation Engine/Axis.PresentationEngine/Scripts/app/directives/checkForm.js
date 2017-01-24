// by using a directive for form valiation, we are making a declarative, rather than imperative, solution to error handling
angular.module('xenatixApp')
    .directive('checkForm', ['$rootScope', '$timeout', 'alertService', 'formService', function ($rootScope, $timeout, alertService, formService) {
    		return {
    				restrict: 'A',
    				replace: false,
    				scope: {
    						onSave: '&',
    						onPrint: '&',
    						name: '@'
    				},
    				require: '^form',
    				link: function (scope, elem, attr, controller) {
    						//clear default form
    						$rootScope.defaultFormName = null;
    						var timer;

    						var save = function (isNext, mandatory, hasErrors, keepForm) {
    								if (!isNext && !hasErrors && !formService.isAnyFormDirty()) {
    										alertService.warning("No change detected! Please make some changes before saving.");
    										return;
    								}

    								if (scope.onSave !== undefined && !$rootScope.isRunning) {
    										scope.onSave({
    												isNext: isNext, isMandatory: mandatory, hasErrors: hasErrors, keepForm: keepForm
    										});
    								}
    						};

    						// function the directive exposes to validate a form, that acts in between the user taking a save action, and the form's custom logic to perform the saving of a screen
    						var submit = function (isNext, mandatory, keepForm, isReadOnly, isPrint) {

    								if (timer) {
    										return;
    								}

    								timer = $timeout(function () {
    										$timeout.cancel(timer);
    										timer = null;
    								}, 1000);

    								// ng-form is not making parent form invalid like it is supposed to, but the parent's $error object is populated w/ errors
    								if (scope.formController !== undefined && (scope.formController.$invalid || Object.keys(scope.formController.$error).length)) {

    										// we don't want to display validation errors on screens that are optional, and the user is moving away from the next screen
    										// Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
    										// and if user don't do any modification then user can move to next screen.
    										if (!mandatory && isNext && !scope.formController.modified) {
    												// we still need to let the screen's save handle moving to the next screen
    												save(isNext, mandatory, true, keepForm);

    												return true;
    										}

    										// make sure we can check the errors to know what to display
    										if (scope.formController.$error === undefined || scope.formController.$error === null) {
    												return false; // unable to determine if there is an error, so don't continue
    										}

    										// if there are errors, but the form is not mandatory, go ahead and skip displaying errors, and let the save know what is happening 
    										// so it knows not to save, but can go to next
    										// Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
    										// and if user don't don any modification then user can move to next screen.
    										if (!mandatory && !scope.formController.modified) {
    												save(isNext, mandatory, true, keepForm);
    										}

    										if (!isReadOnly) {
    												var fieldToFocus = null;
    												iterateErrorFields(scope.formController.$error.required, fieldToFocus, 'required', alertService);
    												iterateErrorFields(scope.formController.$error.date, fieldToFocus, 'date', alertService);
    												iterateErrorFields(scope.formController.$error.invalidDate, fieldToFocus, 'invalidDate', alertService);
    												iterateErrorFields(scope.formController.$error.lessThanMinValidDate, fieldToFocus, 'lessThanMinValidDate', alertService);
    												iterateErrorFields(scope.formController.$error.futureDate, fieldToFocus, 'futureDate', alertService);
    												iterateErrorFields(scope.formController.$error.pastDate, fieldToFocus, 'pastDate', alertService);
    												iterateErrorFields(scope.formController.$error.greaterThanDate, fieldToFocus, 'greaterThanDate', alertService);
    												iterateErrorFields(scope.formController.$error.lessThanDate, fieldToFocus, 'lessThanDate', alertService);
    												iterateErrorFields(scope.formController.$error.mask, fieldToFocus, 'mask', alertService);
    												iterateErrorFields(scope.formController.$error.pattern, fieldToFocus, 'pattern', alertService);
    												iterateErrorFields(scope.formController.$error.passwordPattern, fieldToFocus, 'passwordPattern', alertService);
    												iterateErrorFields(scope.formController.$error.min, fieldToFocus, 'min', alertService);
    												iterateErrorFields(scope.formController.$error.max, fieldToFocus, 'max', alertService);
    												iterateErrorFields(scope.formController.$error.minimum, fieldToFocus, 'minimum', alertService);
    												iterateErrorFields(scope.formController.$error.minlength, fieldToFocus, 'minlength', alertService);
    												iterateErrorFields(scope.formController.$error.maximum, fieldToFocus, 'maximum', alertService);
    												iterateErrorFields(scope.formController.$error.maxlength, fieldToFocus, 'maxlength', alertService);
    												iterateErrorFields(scope.formController.$error.editable, fieldToFocus, 'editable', alertService);      //For typeahead errors

    												// Check for errors on disabled fields...if all errors come from disabled fields, save the form
    												// NOTE: this is for 'date' only for now, need to add more with specific selector logic
    												// NOTE: by only checking for date errors, isAllDisabled is true for other errors, allowing save to be called w/o errors in some cases
    												var isAllDisabled = checkForAllDisabledCtrls(scope.formController.$error.date) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.invalidDate) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.lessThanMinValidDate) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.futureDate) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.pastDate) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.greaterThanDate) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.lessThanDate) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.required) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.mask) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.pattern) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.passwordPattern) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.min) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.max) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.minimum) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.minlength) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.maximum) &&
																																				checkForAllDisabledCtrls(scope.formController.$error.maxlength) && 
                                                                                                                                                checkForAllDisabledCtrls(scope.formController.$error.editable);
    												if (isAllDisabled) {
    														save(isNext, mandatory, false, keepForm);
    												}
    										}
    										else { //will call when form is in readonly mode so, can move to next screeen.
    												save(isNext, mandatory, false, keepForm);
    										}
    								}

    								else if (scope.formController !== undefined && scope.formController.$valid) {
    										if (isPrint && scope.onPrint !== undefined && !$rootScope.isRunning) {
    												scope.onPrint({ isNext: isNext, isMandatory: mandatory, hasErrors: false, keepForm: keepForm });
    										}
    										else {
    												save(isNext, mandatory, false, keepForm);
    										}
    										return true;
    								}
    								return false;
    						};

    						$rootScope.safeSubmit = function (isNext, mandatory, keepForm, isReadOnly) {
    								submit(isNext, mandatory, keepForm, isReadOnly);
    						}

    						$rootScope.safePrint = function (isNext, mandatory, keepForm, isReadOnly) {
    								submit(isNext, mandatory, keepForm, isReadOnly, true);
    						}

    						scope.formController = controller;
    				}
    		}
    }]);

