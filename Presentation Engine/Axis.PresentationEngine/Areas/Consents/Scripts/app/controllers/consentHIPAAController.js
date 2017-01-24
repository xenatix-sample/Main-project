(function () {
    angular.module('xenatixApp')
        .controller('consentHIPAAController', ['$controller', '$scope', 'eSignatureService', 'alertService', '$stateParams', '$filter', 'navigationService', 'consentsPrintService', 'registrationService', 'contactBenefitService', '$state',
                 function ($controller, $scope, eSignatureService, alertService, $stateParams, $filter, navigationService, printService, registrationService, contactBenefitService, $state) {
                     $scope.stopAutoRun = true;
                     $scope.eSignature = [];
                     $scope.CurrentUser;
                     $scope.SIGN_TYPE = {
                         Individual: 0,
                         Authorized: 1,
                         Staff: 2,
                         StaffLast: 3
                     }
                     var userId = 0;
                     // Topaz init
                     $scope.OnTopazReady = function (newValues, oldValues, x) {
                         if (newValues !== oldValues && newValues == true) {
                             initSign($scope.SIGN_TYPE.Individual);
                             initSign($scope.SIGN_TYPE.Authorized);
                             initSign($scope.SIGN_TYPE.Staff);
                             initSign($scope.SIGN_TYPE.StaffLast);
                         }
                     };

                     var initSign = function (signIndx) {
                         $scope.eSignature[signIndx].topazModel.Init();
                         $scope.eSignature[signIndx].topazModel.ImageCallback = $scope.sigImageCallback.bind({}, $scope.eSignature[signIndx].topazModel);
                         $scope.eSignature[signIndx].topazModel.NoPointsCallback = $scope.noPointsCallback;
                     }

                     navigationService.get().then(function (data) {
                         if (data && data.DataItems && data.DataItems.length > 0) {
                             var user = data.DataItems[0];
                             userId = user.UserID;
                             $scope.CurrentUser = user.UserFullName;
                         }
                     });

                     registrationService.get($stateParams.ContactID).then(function (data) {
                         if (data && data.DataItems && data.DataItems.length > 0) {
                             $scope.registrationData = data.DataItems[0];
                             $scope.contactName = $scope.registrationData.FirstName + ' ' + $scope.registrationData.LastName;
                         };
                     });

                     var topazModel = function (name) {
                         var model = {
                             modelNumber: '',
                             b64ImageData: '',
                             DeviceMessage: 'You do not have application permissions to electronically sign!',
                             name: name
                         };
                         var retObj = { topazModel: model };
                         return retObj;
                     };

                     $controller('assessmentController', { $scope: $scope });

                     var init = function () {

                         $scope.eSignature.push(topazModel('IndividualSignature'));
                         $scope.eSignature.push(topazModel('LegallyAuthorizedSignature'))
                         $scope.eSignature.push(topazModel('StaffSignature'));
                         $scope.eSignature.push(topazModel('StaffSignatureFinal'));

                         $scope.getAssessmentQuestions().then(function (data) {
                             var todayDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                             if (!$scope.responses)
                                 $scope.responses = {};
                             if ($stateParams.ResposeID != 0) {
                                 initializeResponse(856);//Printed Name of Individual
                                 initializeResponse(857);//Date Signed
                                 initializeResponse(860);//Date Signed(Printed Name of Legally Authorized Representative)
                                 initializeResponse(863);//Printed Name of Staff
                                 initializeResponse(864);//Date Signed(Printed Name of Staff)
                                 initializeResponse(869);//Printed Name of Staff
                                 initializeResponse(870);//Date Signed(Printed Name of Staff)


                                 setInitializeValues(856, 17, $scope.contactName);
                                 setInitializeValues(857, 17, todayDate);
                                 setInitializeValues(860, 17, todayDate);
                                 setInitializeValues(863, 17, $scope.CurrentUser);
                                 setInitializeValues(864, 17, todayDate);
                                 setInitializeValues(869, 17, $scope.CurrentUser);
                                 setInitializeValues(870, 17, todayDate);
                             }


                             //need to check 
                             //TODO: setting optionID parameter as hardcode for now. Revisit and make it dynamic from response
                             setSignature(858, 3702, $scope.SIGN_TYPE.Individual);
                             setSignature(861, 3703, $scope.SIGN_TYPE.Authorized);
                             setSignature(865, 3701, $scope.SIGN_TYPE.Staff);
                             setSignature(871, 3701, $scope.SIGN_TYPE.StaffLast);

                         });

                         $scope.$watch('isTopazReady', $scope.OnTopazReady);
                     }

                     var initializeResponse = function (questionID) {
                         if (!$scope.responses[questionID])
                             $scope.responses[questionID] = {};
                     }
                     // TODO: get optionID dynamically depending on QuestionID
                     var setInitializeValues = function (questionID, optionID, value) {
                         if (!$scope.responses[questionID][optionID]) {
                             $scope.responses[questionID][optionID] = value;
                         }
                     }

                     var setSignature = function (questionID, optionID, signIndx) {
                         if ($scope.responses[questionID] && $scope.responses[questionID][optionID]) {
                             $scope.eSignature[signIndx].topazModel.b64ImageData = $scope.responses[questionID][optionID];
                         }
                     }

                     var setSignatureToUpdate = function (questionID, signModel, optionIndx) {
                         if (!$scope.responses[questionID]) {
                             $scope.responses[questionID] = {};
                         }
                         if (signModel.topazModel.b64ImageData && signModel.topazModel.b64ImageData.length > 1) {
                             $scope.responses[questionID][optionIndx] = signModel.topazModel.b64ImageData;
                             formatSignDate(questionID);
                         }
                         else {
                             removeNotRequiredData(questionID);
                         }


                     };

                     var formatSignDate = function (questionID) {
                         var currentDate = new Date();
                         switch (questionID) {
                             case 858: {
                                 if ($scope.responses[857])
                                     $scope.responses[857][17] = currentDate;
                                 break;
                             }
                             case 861: {
                                 if ($scope.responses[860])
                                     $scope.responses[860][17] = currentDate;;
                                 break;
                             }
                             case 865: {
                                 if ($scope.responses[864])
                                     $scope.responses[864][17] = currentDate;;
                                 break;
                             }
                             case 871: {
                                 if ($scope.responses[870])
                                     $scope.responses[870][17] = currentDate;;
                                 break;
                             }
                         }
                     };

                     var removeNotRequiredData = function (questionID) {
                         switch (questionID) {
                             case 858: {
                                 delete $scope.responses[questionID];
                                 delete $scope.responses[856];
                                 delete $scope.responses[857];
                                 break;
                             }
                             case 861: {
                                 delete $scope.responses[questionID];
                                 delete $scope.responses[859];
                                 delete $scope.responses[860];
                                 break;
                             }
                             case 865: {
                                 delete $scope.responses[questionID];
                                 delete $scope.responses[863];
                                 delete $scope.responses[864];
                                 break;
                             }
                             case 871: {
                                 delete $scope.responses[questionID];
                                 delete $scope.responses[869];
                                 delete $scope.responses[870];
                                 break;
                             }

                         }
                     };

                     $scope.onPrintReport = function () {
                         $scope.save(false, false).then(function () {
                             printService.initReports().then(function (data) {
                                 $scope.reportModel = data;
                                 //$scope.reportModel = {};
                                 if ($scope.reportModel['856'] && $scope.reportModel['856']['17'])
                                     $scope.reportModel['856'] = $scope.reportModel['856']['17'];
                                 if ($scope.reportModel['857'] && $scope.reportModel['857']['17'])
                                     $scope.reportModel['857'] = $scope.reportModel['857']['17'];
                                 if ($scope.reportModel['862'] && $scope.reportModel['862']['17'])
                                     $scope.reportModel['862'] = $scope.reportModel['862']['17'];
                                 if ($scope.reportModel['860'] && $scope.reportModel['860']['17'])
                                     $scope.reportModel['860'] = $scope.reportModel['860']['17'];
                                 if ($scope.reportModel['863'] && $scope.reportModel['863']['17'])
                                     $scope.reportModel['863'] = $scope.reportModel['863']['17'];
                                 if ($scope.reportModel['864'] && $scope.reportModel['864']['17'])
                                     $scope.reportModel['864'] = $scope.reportModel['864']['17'];
                                 if ($scope.reportModel['869'] && $scope.reportModel['869']['17'])
                                     $scope.reportModel['869'] = $scope.reportModel['869']['17'];
                                 if ($scope.reportModel['870'] && $scope.reportModel['870']['17'])
                                     $scope.reportModel['870'] = $scope.reportModel['870']['17'];
                                 if ($scope.reportModel['859'] && $scope.reportModel['859']['17'])
                                     $scope.reportModel['859'] = $scope.reportModel['859']['17'];
                                 if ($scope.reportModel['868'] && $scope.reportModel['868']['17'])
                                     $scope.reportModel['868'] = $scope.reportModel['868']['17'];

                                 if ($scope.registrationData.MRN)
                                     $scope.reportModel.mrn = $scope.registrationData.MRN;
                                 $scope.reportModel.clientName = $scope.contactName;
                                 if ($scope.registrationData.DOB)
                                     $scope.reportModel.dob = $filter('toMMDDYYYYDate')($scope.registrationData.DOB, 'MM/DD/YYYY', 'useLocal');
                                 $scope.reportModel.medicaidNumber = 'N/A';
                                 if ($scope.responses[865] && $scope.responses[865][3701])
                                     $scope.reportModel.staffSigUri = $scope.responses[865][3701];
                                 if ($scope.responses[861] && $scope.responses[861][3703])
                                     $scope.reportModel.repSigUri = $scope.responses[861][3703];
                                 if ($scope.responses[858] && $scope.responses[858][3702])
                                     $scope.reportModel.contactSigUri = $scope.responses[858][3702];
                                 if ($scope.responses[871] && $scope.responses[871][3701])
                                     $scope.reportModel.staffSigUriNoSign = $scope.responses[871][3701];

                                 contactBenefitService.get($stateParams.ContactID).then(function (data) {
                                     if (data && data.DataItems.length > 0) {
                                         var payors = $filter('filter')(data.DataItems, function (itm) {
                                             return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                                         })
                                         if (payors && payors.length > 0) {
                                             $scope.reportModel.medicaidNumber = payors[0].PolicyID;
                                         }
                                     }
                                     $scope.reportModel.HasLoaded = true;
                                     $('#reportModal').modal('show');
                                 });
                             });
                         })
                     };

                     $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                         setSignatureToUpdate(858, $scope.eSignature[$scope.SIGN_TYPE.Individual], 3702);
                         setSignatureToUpdate(861, $scope.eSignature[$scope.SIGN_TYPE.Authorized], 3703);
                         setSignatureToUpdate(865, $scope.eSignature[$scope.SIGN_TYPE.Staff], 3701);
                         setSignatureToUpdate(871, $scope.eSignature[$scope.SIGN_TYPE.StaffLast], 3701);

                         return $scope.saveAssessment(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                             if (isNext && !hasErrors)
                                 $scope.next();
                             if (response != "-1") {
                                 alertService.success('Consent response have been saved successfully.');
                                 angular.extend($stateParams, { ResponseID: $scope.responseId });
                                 $state.transitionTo($state.current.name, $stateParams);
                             }
                             else {
                                 alertService.error('Consent response have not been saved');
                             }
                         });
                     }

                     $scope.noPointsCallback = function () {
                         $scope.save(false, true);
                     };

                     $scope.sigImageCallback = function (item, str) {
                         item.b64ImageData = str;
                     };

                     init();
                 }]);
}());