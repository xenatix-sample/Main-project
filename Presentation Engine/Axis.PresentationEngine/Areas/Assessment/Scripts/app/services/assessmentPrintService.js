(function () {
    angular.module("xenatixApp")
    .factory("assessmentPrintService", ['$stateParams', '$q', 'assessmentService', '$filter', 'lookupService', 'WorkflowHeaderService', function ($stateParams, $q, assessmentService, $filter, lookupService, WorkflowHeaderService) {
        //This method will take assessmentID and responseID and will get all the sections in assessment and push the response to report model
        var initReports = function (assessmentID, responseID, sectionID, workflowDataKey, workflowHeaderID) {
            var assessmentID = (assessmentID != undefined) ? assessmentID : $stateParams.AssessmentID;
            var responseID = (responseID != undefined) ? responseID : $stateParams.ResponseID;
            var reportDataModel = {
                HasLoaded: false
            };
            var adjustedTime = '12:00:01';
           
           
            var deferred = $q.defer();
            if (isItContactLettersWorkflow(workflowDataKey) || isItContactFormssWorkflow(workflowDataKey) || (workflowDataKey && workflowHeaderID)) {
                WorkflowHeaderService.GetWorkflowHeader(workflowDataKey, workflowHeaderID).then(function (data) {
                    if (data) {
                        let headerDetails = data.DataItems[0];
                        reportDataModel.MRN = headerDetails.MRN;
                        var suffix = lookupService.getText("Suffix", headerDetails.SuffixID);
                        reportDataModel.clientName = headerDetails.FirstName + (headerDetails.Middle ? ' ' + headerDetails.Middle : '') + ' ' + headerDetails.LastName + (suffix ? ' ' + suffix : '');;
                        reportDataModel.dob = ($filter('formatDate')(headerDetails.DOB, 'MM/DD/YYYY')).toString();
                        reportDataModel.medicaidNumber = headerDetails.MedicaidID || 'N/A';
                    }
                });
            }
            assessmentService.getAssessment(assessmentID).then(function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    var assmnt = data.DataItems[0];
                    reportDataModel.ReportHeader = assmnt.AssessmentName;
                    if (sectionID) {
                        //get section name by section id
                        assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                            if (hasData(data)) {
                                assessmentSection = $filter('filter')(data.DataItems, { AssessmentSectionID: sectionID }, true);
                                var sectionName = assessmentSection[0].SectionName
                                reportDataModel.ReportName = assmnt.AssessmentName + ' ' + sectionName;
                            }
                        })
                    }
                    else
                        reportDataModel.ReportName = assmnt.AssessmentName;
                }
            });

            assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                if (data.ResultCode === 0) {
                    angular.forEach(data.DataItems, function (item, index) {
                        var sectionID = item.AssessmentSectionID;
                        assessmentService.getAssessmentQuestions(sectionID).then(function (qData) {
                            assessmentService.getAssessmentResponseDetails(responseID, sectionID).then(function (rData) {
                                rData = rData.data;
                                angular.forEach(rData.DataItems, function (responseDetail) {
                                    var inputTypeId = $filter('filter')(qData.DataItems, function (question) { return parseInt(question.QuestionID) === parseInt(responseDetail.QuestionID); })[0].InputTypeID;
                                    if (inputTypeId === INPUT_TYPE.Textbox || inputTypeId === INPUT_TYPE.TextArea || inputTypeId === INPUT_TYPE.DatePicker ||
                                        inputTypeId === INPUT_TYPE.Hidden || inputTypeId === INPUT_TYPE.Label || inputTypeId === INPUT_TYPE.Comments || inputTypeId === INPUT_TYPE.CheckboxList || inputTypeId === INPUT_TYPE.DSignature) {
                                        if (!(responseDetail.QuestionID in reportDataModel))
                                            reportDataModel[responseDetail.QuestionID] = {};
                                        reportDataModel[responseDetail.QuestionID] = responseDetail.ResponseText;
                                    }
                                    else if (inputTypeId === INPUT_TYPE.TimePicker) {
                                        if (!(responseDetail.QuestionID in reportDataModel))
                                            reportDataModel[responseDetail.QuestionID] = {};
                                        reportDataModel[responseDetail.QuestionID] = searchString(responseDetail.ResponseText, adjustedTime) ? "" : ($filter('toMMDDYYYYDate')(responseDetail.ResponseText, 'hh:mm A', 'useLocal')).toString()
                                    }
                                    else if (inputTypeId === INPUT_TYPE.Radio) {
                                        if (!(responseDetail.QuestionID in reportDataModel))
                                            reportDataModel[responseDetail.QuestionID] = {};
                                        reportDataModel[responseDetail.QuestionID][responseDetail.OptionsID] = true;
                                    }
                                    else if (inputTypeId === INPUT_TYPE.MultiSelect || inputTypeId === INPUT_TYPE.Checkbox) {
                                        if (!(responseDetail.QuestionID in reportDataModel))
                                            reportDataModel[responseDetail.QuestionID] = {};
                                        //reportDataModel[responseDetail.QuestionID] = true;
                                        reportDataModel[responseDetail.QuestionID][responseDetail.OptionsID] = true;
                                    }
                                    else if (inputTypeId === INPUT_TYPE.ExtendedDropdown) {
                                        reportDataModel[responseDetail.QuestionID] = getTextFromLookup(qData.DataItems, responseDetail.QuestionID, responseDetail.ResponseText);
                                    }
                                    else if (inputTypeId === INPUT_TYPE.Select) {
                                        var questionData = $filter('filter')(qData.DataItems, function (question) { return parseInt(question.QuestionID) === parseInt(responseDetail.QuestionID); });
                                        if (questionData && questionData.length > 0 && questionData[0].Attributes) {
                                            var childAttribute = parseJSON(questionData[0].Attributes);
                                            if (childAttribute && 'multi-select' in childAttribute) {
                                                reportDataModel[responseDetail.QuestionID] = {};
                                                reportDataModel[responseDetail.QuestionID][responseDetail.OptionsID] = responseDetail.Options;
                                            }
                                            else {
                                                reportDataModel[responseDetail.QuestionID] = responseDetail.Options;
                                            }
                                        }
                                        else {
                                            reportDataModel[responseDetail.QuestionID] = responseDetail.Options;
                                        }
                                    }
                                    else if (inputTypeId === INPUT_TYPE.ESignature) {
                                        reportDataModel[responseDetail.QuestionID] = responseDetail.SignatureBLOB;
                                    }
                                });
                                if (index == data.DataItems.length - 1) {
                                    deferred.resolve(reportDataModel);
                                }   
                            });
                        });
                    });
                }
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

        var isItContactLettersWorkflow = function (workflowDataKey) {
            if (workflowDataKey === 'Intake-IDDLetters-DidNotKeepAppointmentLetter' || workflowDataKey === 'Intake-IDDLetters-Intake10DayLetterNotification' || workflowDataKey === 'Intake-IDDLetters-IDDIntakeNewAppointmentLetter')
            {
                return true;
            }
            else { return false;}
        }

        var isItContactFormssWorkflow = function (workflowDataKey) {
            if (workflowDataKey === 'Intake-IDDForms-Forms') {
                return true;
            }
            else { return false; }
        }

        return {
            initReports: initReports
        }
    }]);
}())
