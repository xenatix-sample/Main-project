(function () {
    angular.module("xenatixApp")
    .factory("consentsPrintService", ['$stateParams', '$q', 'assessmentService', '$filter', 'lookupService', 'WorkflowHeaderService', function ($stateParams, $q, assessmentService, $filter, lookupService, WorkflowHeaderService) {

        var initReports = function (assessmentID, sectionID, responseID,workflowDataKey,workflowHeaderID) {
            var assessmentID = (assessmentID != undefined) ? assessmentID : $stateParams.AssessmentID;
            var sectionID = (sectionID != undefined) ? sectionID : $stateParams.SectionID;
            var responseID = (responseID != undefined) ? responseID : $stateParams.ResponseID;
            var reportDataModel = {
                HasLoaded: false
            };
            var deferred = $q.defer();
            assessmentService.getAssessment(assessmentID).then(function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    var assmnt = data.DataItems[0];
                    reportDataModel.ReportHeader = assmnt.AssessmentName;
                    reportDataModel.ReportName = assmnt.AssessmentName;// assmnt.AssessmentName.replaceAll(' ', '');
                }
            });
            if (workflowDataKey && workflowHeaderID) {
                WorkflowHeaderService.GetWorkflowHeader(workflowDataKey, workflowHeaderID).then(function (data) {
                    if (data) {
                        headerDetails = data.DataItems[0];
                        reportDataModel.mrn = headerDetails.MRN;
                        var suffix = lookupService.getText("Suffix", headerDetails.SuffixID);
                        reportDataModel.clientName = headerDetails.FirstName + (headerDetails.Middle ? ' ' + headerDetails.Middle : '') + ' ' + headerDetails.LastName + (suffix ? ' ' + suffix : '');;
                        if (headerDetails.DOB)
                            reportDataModel.dob = ($filter('formatDate')(headerDetails.DOB, 'MM/DD/YYYY')).toString();
                        reportDataModel.medicaidNumber = headerDetails.MedicaidID || 'N/A';
                    }
                });
            }

            assessmentService.getAssessmentQuestions(sectionID).then(function (qData) {
                assessmentService.getAssessmentResponseDetails(responseID, sectionID).then(function (rData) {
                    rData = rData.data;
                    angular.forEach(rData.DataItems, function (responseDetail) {
                        var inputTypeId = $filter('filter')(qData.DataItems, function (question) { return parseInt(question.QuestionID) === parseInt(responseDetail.QuestionID); })[0].InputTypeID;
                        if (inputTypeId === INPUT_TYPE.Textbox || inputTypeId === INPUT_TYPE.TextArea || inputTypeId === INPUT_TYPE.DatePicker || inputTypeId === INPUT_TYPE.TimePicker ||
                            inputTypeId === INPUT_TYPE.Hidden || inputTypeId === INPUT_TYPE.Label || inputTypeId === INPUT_TYPE.Comments || inputTypeId === INPUT_TYPE.CheckboxList || inputTypeId === INPUT_TYPE.DSignature) {
                            if (!(responseDetail.QuestionID in reportDataModel))
                                reportDataModel[responseDetail.QuestionID] = {};
                            reportDataModel[responseDetail.QuestionID] = responseDetail.ResponseText;

                        }
                        else if (inputTypeId === INPUT_TYPE.Radio || inputTypeId === INPUT_TYPE.MultiSelect || inputTypeId === INPUT_TYPE.Checkbox) {
                            if (!(responseDetail.QuestionID in reportDataModel))
                                reportDataModel[responseDetail.QuestionID] = {};
                            reportDataModel[responseDetail.QuestionID][responseDetail.OptionsID] = true;
                        }
                        else if (inputTypeId === INPUT_TYPE.Select) {
                            reportDataModel[responseDetail.QuestionID]= responseDetail.Options;
                        }
                        else if (inputTypeId === INPUT_TYPE.ExtendedDropdown) {
                            reportDataModel[responseDetail.QuestionID] = getTextFromLookup(qData.DataItems, responseDetail.QuestionID, responseDetail.ResponseText);
                        }
                        else if (inputTypeId === INPUT_TYPE.ESignature) {
                            reportDataModel[responseDetail.QuestionID] = responseDetail.SignatureBLOB;
                        }
                    });
                    deferred.resolve(reportDataModel);
                });
            });
            return deferred.promise;
        }

        var getTextFromLookup = function (qData, questionID, value) {
            try {
                var i = 0, len = qData.length;
                for (; i < len; i++) {
                    if (+qData[i].QuestionID == questionID) {
                        var lookUpType = JSON.parse(qData[i].Attributes)["lookup-name"];
                        return lookupService.getText(lookUpType, parseInt(value));
                    }
                }
            } catch (e) {
                return '';
            }
        }

        return {
            initReports: initReports
        }
    }]);
}())
