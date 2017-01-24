(function () {
    angular.module('xenatixApp')
   .factory('recordingServicePrintService', ['$q', '$filter', '$stateParams', 'serviceRecordingService', 'eSignatureService', 'registrationService', 'contactBenefitService', 'userProfileService', 'WorkflowHeaderService', 'lookupService',
       function ($q, $filter, $stateParams, serviceRecordingService, eSignatureService, registrationService, contactBenefitService, userProfileService, WorkflowHeaderService, lookupService) {

           var adjustedTime = "00:00:01";
           var initPrint = function (sourceHeaderID, serviceRecordingSourceID, serviceRecordingID, workflowDataKey,workflowHeaderID) {
               var dfd = $q.defer();
               var reportModel = {
                   ReportHeader: 'Recorded Services',
                   ReportName: 'ServiceRecording',
                   HasLoaded: false
               };

               reportModel.isBAPN = false;
               reportModel.isIntakeForms = false;
               reportModel.isCallCenter = false;
               serviceRecordingID = serviceRecordingID ? serviceRecordingID : 0;

               switch (serviceRecordingSourceID) {
                   case SERVICE_RECORDING_SOURCE.CallCenter:
                   case SERVICE_RECORDING_SOURCE.LawLiaison:
                       reportModel.isCallCenter = true;
                       break;
                   case SERVICE_RECORDING_SOURCE.BAPN:
                       reportModel.isBAPN = true;
                       break;
                   case SERVICE_RECORDING_SOURCE.IDDForms:
                       reportModel.isIntakeForms = true;
                       break;
               }

               var printDetails = [];

               printDetails.push(serviceRecordingService.getServiceRecording(sourceHeaderID, serviceRecordingSourceID));
               printDetails.push(eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.ServiceRecording, serviceRecordingID));
               printDetails.push(registrationService.get($stateParams.ContactID));
               printDetails.push(contactBenefitService.get($stateParams.ContactID));
               if (workflowDataKey && sourceHeaderID) {
                   printDetails.push(WorkflowHeaderService.GetWorkflowHeader(workflowDataKey, sourceHeaderID));
               }              

               $q.all(printDetails).then(function (data) {
                   if (hasDetails(data)) {
                       if (hasData(data[0])) {
                           var serviceRecording = data[0].DataItems[0];
                           if (serviceRecording.ServiceStartDate) {
                               reportModel.startTime = searchString(serviceRecording.ServiceStartDate, adjustedTime) ? "" : $filter('formatDate')(serviceRecording.ServiceStartDate, 'hh:mm A');
                               reportModel.startDate = $filter('formatDate')(serviceRecording.ServiceStartDate, 'MM/DD/YYYY');
                           }
                           if (serviceRecording.ServiceEndDate) {

                               reportModel.endTime = searchString(serviceRecording.ServiceEndDate, adjustedTime) ? "" : $filter('formatDate')(serviceRecording.ServiceEndDate, 'hh:mm A');
                               reportModel.endDate = $filter('formatDate')(serviceRecording.ServiceEndDate, 'MM/DD/YYYY');
                               //Duration only calculated if end date is avaialable
                               reportModel.duration = calculateDuration(serviceRecording.ServiceStartDate, serviceRecording.ServiceEndDate);
                           }
                           
                           reportModel.programUnit = serviceRecordingService.getText('WorkflowProgramUnit', serviceRecording.OrganizationID, null,'OrganizationID');
                          
                           reportModel.serviceType = serviceRecordingService.getText('ServiceType', serviceRecording.ServiceTypeID);
                           reportModel.serviceItem = serviceRecordingService.getText('RecordingServices', serviceRecording.ServiceItemID, 'ServiceName', 'ServiceID');
                           reportModel.serviceStatus = serviceRecordingService.getText('ServiceStatus', serviceRecording.ServiceStatusID);
                           reportModel.attendanceStatus = serviceRecordingService.getText('AttendanceStatus', serviceRecording.AttendanceStatusID);
                           reportModel.trackingField = serviceRecordingService.getText('TrackingField', serviceRecording.TrackingFieldID);
                           reportModel.recipient = serviceRecordingService.getText('RecordingRecipientCode', serviceRecording.RecipientCodeID);
                           reportModel.whoAttended = '';
                           angular.forEach(serviceRecording.AttendedList, function (item) {
                               reportModel.whoAttended += (reportModel.whoAttended && reportModel.whoAttended.length > 0) ? (', ' + item.Name) : item.Name;
                           });
                           reportModel.deliveryMethod = serviceRecordingService.getText('RecordingDeliveryMethod', serviceRecording.DeliveryMethodID);
                           reportModel.placeOfService = serviceRecordingService.getText('RecordingServiceLocation', serviceRecording.ServiceLocationID);
                           reportModel.sentToCMHC = serviceRecordingService.getText('ConversionStatus', serviceRecording.ConversionStatusID);
                           reportModel.providerOfService = serviceRecordingService.getText('Users', serviceRecording.UserID);
                           reportModel.supervisingProvider = serviceRecordingService.getText('Users', serviceRecording.SupervisorUserID);
                           reportModel.coprovider = '';
                           var visitedAdditionalUsers = {};
                           angular.forEach(serviceRecording.AdditionalUserList, function (item) {
                               if (!visitedAdditionalUsers[item.UserID]) {
                                   visitedAdditionalUsers[item.UserID] = true;
                                   reportModel.coprovider += (reportModel.coprovider && reportModel.coprovider.length > 0) ? (', ' + serviceRecordingService.getText('Users', item.UserID)) : serviceRecordingService.getText('Users', item.UserID);
                               }
                           });
                       }
                       var signPromise = [];
                       if (hasData(data[1])) {
                           var signatureDetails = data[1].DataItems[0];
                           reportModel.staffName = serviceRecordingService.getText('Users', signatureDetails.EntityId);
                           reportModel.dateSigned = $filter('formatDate')(signatureDetails.ModifiedOn, 'MM/DD/YYYY');
                           reportModel.credential = serviceRecordingService.getText('Credential', signatureDetails.CredentialID);
                           reportModel.staffSigUri = signatureDetails.SignatureBlob || "";
                           if (reportModel.isCallCenter) {
                               reportModel.isDigitalSignature = true;
                               signPromise.push(userProfileService.getByID(signatureDetails.EntityId, true));
                           }
                       }

                       if (workflowDataKey && sourceHeaderID) {
                           if (hasData(data[4])) {
                               var regDetails = data[4].DataItems[0];
                               if (regDetails.MRN)
                                   reportModel.mrn = regDetails.MRN;
                               var suffix = lookupService.getText("Suffix", regDetails.SuffixID);
                               reportModel.clientName = regDetails.FirstName + (regDetails.Middle ? ' ' + regDetails.Middle : '') + ' ' + regDetails.LastName + (suffix ? ' ' + suffix : '');
                               if (regDetails.DOB)
                                   reportModel.dob = $filter('formatDate')(regDetails.DOB, 'MM/DD/YYYY');
                               reportModel.medicaidNum = regDetails.MedicaidID || 'N/A';
                           }
                       }
                       else {
                           if (hasData(data[2])) {
                               var regDetails = data[2].DataItems[0];
                               if (regDetails.MRN)
                                   reportModel.mrn = regDetails.MRN;
                               reportModel.clientName = regDetails.FirstName + (regDetails.Middle ? ' ' + regDetails.Middle : '') + ' ' + regDetails.LastName;
                               if (regDetails.DOB)
                                   reportModel.dob = $filter('formatDate')(regDetails.DOB, 'MM/DD/YYYY');
                           }
                           reportModel.medicaidNum = 'N/A';
                           if (hasData(data[3])) {
                               var payors = $filter('filter')(data[3].DataItems, function (itm) {
                                   return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                               });
                               if (hasData(payors)) {
                                   reportModel.medicaidNumber = payors[0].PolicyID;
                               }
                           }
                       }

                       $q.all(signPromise).then(function (signData) {
                           if (hasData(signData[0])) {
                               var userData = signData[0].DataItems[0];
                               reportModel.isSigned = true;
                               reportModel.mySignature = userData.PrintSignature;
                           }
                       }).finally(function () {
                           dfd.resolve(reportModel);
                       });



                   } else {
                       dfd.resolve(null);
                   }
               });
               return dfd.promise;
           };

           return {
               initPrint: initPrint
           };
       }]);
}());
