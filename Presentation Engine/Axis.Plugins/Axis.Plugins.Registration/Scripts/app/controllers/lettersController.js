(function () {
    angular.module('xenatixApp')
        .controller('lettersController', ['$scope', '$stateParams', '$filter', '$state', 'assessmentService', 'lettersService', 'lookupService', 'intakeFormsService', 'assessmentPrintService', 'alertService', 'serviceRecordingService', '$rootScope', 'orderByFilter', 'navigationService', 'cacheService','WorkflowHeaderService',
    function ($scope, $stateParams, $filter, $state, assessmentService, lettersService, lookupService, intakeFormsService, assessmentPrintService, alertService, serviceRecordingService, $rootScope, orderBy, navigationService, cacheService, WorkflowHeaderService) {
        var adjustedTime = "00:00:01";
        $scope.lettersList = [];
        $scope.CallMinStartDate = new Date(70, 1, 1);
        $scope.intakeFormAssessmentID = ASSESSMENT_TYPE.IDDIntakeForms;
        var lettersTable;
        var flyoutElement = $('#patientProfileFlyout');
        $scope.permissionKey = $state.current.data.permissionKey;
        var isLetter = ($state.current.name.indexOf('letters') > -1);
        var maxSignValidationDays = 7;
        $scope.$parent['autoFocus'] = isLetter ? false : true;
        $scope.$parent['autoFocusLettersDropdown'] = isLetter ? true : false;
        var userId;
        var init = function () {
            $scope.isLetter = ($state.current.name.indexOf('letters') > -1);
            lettersTable = $('#lettersTable');
            initializeBootstrapTable();
            assessmentService.getAssessment().then(function (data) {
                if (hasData(data)) {
                    $scope.lettersList = $filter('filter')(data.DataItems, { DocumentTypeID: $scope.isLetter ? DOCUMENT_TYPE.Letter : DOCUMENT_TYPE.Form }, true);
                }
            });
            $scope.contactID = $stateParams.ContactID;
            loadGridData();
        }

        var formsGridOptions = {
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            showColumns: true,
            data: [],
            undefinedText: '',
            columns: [
                {
                    field: 'DocumentStatusID',
                    title: 'Status',
                    formatter: function (value, row, index) {
                        if (value === DOCUMENT_STATUS.Draft)
                            return "Draft";
                        else if (row["IsVoided"])
                            return "Void"
                        else
                            return "Completed";
                    }
                },
                {
                    field: 'ServiceStartDate',
                    title: 'Date',
                    formatter: function (value, row, index) {
                        return value ? $filter('formatDate')(value) : '';
                    }
                },
                {
                    field: "ServiceStartDate",
                    title: "Time",
                    formatter: function (value, row, index) {
                        return (!value ? '' :  (searchString(value, adjustedTime) ? '' : $filter('formatDate')(value, 'hh:mm A')))
                    }
                },
                {
                    field: "Duration",
                    title: "Duration",
                    formatter: function (value, row, index) {
                        return calculateDuration(row['ServiceStartDate'], row['ServiceEndDate']);
                    }
                },
                {
                    field: "UserID",
                    title: "Provider",
                    formatter: function (value, row, index) {
                        return lookupService.getText('Users', value);
                    }
                },
                {
                    field: 'ContactFormsID',
                    title: 'Actions',
                    formatter: function (value, row, index) {

                        var isDraft = (row["DocumentStatusID"] == DOCUMENT_STATUS.Draft);
                        var isComplete = (row["DocumentStatusID"] == DOCUMENT_STATUS.Completed);
                        var isVoided = row["IsVoided"];
                        return ('<span class="text-nowrap">' +
                               (isDraft ? '<a href="javascript:void(0)" data-default-action id="editForms" security permission-key="Intake-IDDForms-Forms" permission="update" ng-click="navigateToSection(' + row["AssessmentID"] + ',' + row["ResponseID"] + ',' + row["ContactFormsID"] + ', \'edit\',' + row["DocumentStatusID"] + ',' + row["IsVoided"] + ')" name="editLetters" data-toggle="modal" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' :
                                          '<a href="javascript:void(0)" data-default-action security permission-key="Intake-IDDForms-Forms" permission="read" id="viewForms"  ng-click="navigateToSection(' + row["AssessmentID"] + ',' + row["ResponseID"] + ',' + row["ContactFormsID"] + ', \'view\',' + row["DocumentStatusID"] + ',' + row["IsVoided"] + ')" name="viewForms" title=' + (isVoided ? 'View' : 'Edit') + ' space-key-press><i class="fa fa-' + (isVoided ? 'eye' : 'pencil') + ' fa-fw"></i></a>') +
                                      ((!isVoided && row["DocumentStatusID"] === DOCUMENT_STATUS.Completed) ? createLinkForVoidService(row) : '') +
                                        (isDraft ? '<a href="javascript:void(0)" data-default-no-action security permission-key="Intake-IDDForms-Forms" permission="delete" ng-click="deleteSection(' + value + ')" id="deleteLetter" name="deleteLetter" title="Delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>' : '') + '</span>');
                    }
                }
            ]
        }

        //Create link for Void service
        var createLinkForVoidService = function (row) {
            var isSevenDaysOld = $filter('getDaysDifference')(row.ServiceEndDate, moment.utc()) <= maxSignValidationDays;
            if ((userId === row.UserID) && isSevenDaysOld) {
                return '<a data-default-no-action href="javascript:void(0)" alt="Void Service" ng-click="voidFlyout(' + row.ServiceRecordingID + ',' + row.NoteHeaderID + ',' + row.ContactFormsID + ',' + row.ResponseID + ')"' +
                                    'data-module="callcenter" data-feature="crisisline" data-action="append" space-key-press>' +
                                        '<i  title="Void Service" class="fa fa-ban fa-fw" /></a>';
            }
            else {
                return '<a data-default-no-action href="javascript:void(0)" security permission-key="Intake-IDDForms-Void" permission="create" alt="Void Service" ng-click="voidFlyout(' + row.ServiceRecordingID + ',' + row.NoteHeaderID + ',' + row.ContactFormsID + ',' + row.ResponseID + ')"' +
                                        'data-module="callcenter" data-feature="crisisline" data-action="append" space-key-press>' +
                                            '<i  title="Void Service" class="fa fa-ban fa-fw" /></a>';
            }
        };

        var objVoidModel = function (serviceRecordingID, noteHeaderID, ID, responseID) {
            return {
                ServiceRecordingID: serviceRecordingID,
                ID: ID,
                AssessmentResponseID: responseID,
                NoteHeaderID: noteHeaderID,
                AssessmentID: ASSESSMENT_TYPE.IDDIntakeForms,
                PermissionVoidKey: $state.current.data.permissionVoidKey
            };
        };
        $scope.voidFlyout = function (serviceRecordingID, noteHeaderID, ID, responseID) {
            if (!flyoutElement.hasClass('active'))
                openVoidFlyout(serviceRecordingID, noteHeaderID, ID, responseID);
        };
        var openVoidFlyout = function (serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID) {
            $scope.voidModel = objVoidModel(serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID);
            $scope.$parent.getVoidModel($scope.voidModel);
            flyoutElement.addClass('active');
            $rootScope.defaultFormName = 'vs.voidServiceForm';
        };


        navigationService.get().then(function (data) {
            if (data && data.DataItems && data.DataItems.length > 0) {
                var user = data.DataItems[0];
                userId = user.UserID;
            }
        });

        $scope.$on('voidServiceReloadGrid', function (event, args) {
            intakeFormsService.getIntakeForm(args.ID).then(function (intakeForms) {
                if (hasData(intakeForms)) {
                    var intakeFormsData = intakeForms.DataItems[0];
                    intakeFormsData.DocumentStatusID = DOCUMENT_STATUS.Void;
                    intakeFormsService.update(intakeFormsData).then(function () {
                        if (args.isCreateCopy) {
                            intakeFormsData.ContactFormsID = null;
                            intakeFormsData.DocumentStatusID = DOCUMENT_STATUS.Draft;
                            intakeFormsData.ModifiedOn = new Date();
                            intakeFormsData.ResponseID = args.AssessmentResponseID;
                            intakeFormsService.add(intakeFormsData).then(function (intakeResponseData) {
                                serviceRecordingService.getServiceRecording(args.ID, SERVICE_RECORDING_SOURCE.IDDForms).then(function (serviceRecordingData) {
                                    if (hasData(serviceRecordingData)) {
                                        serviceRecordingData = serviceRecordingData.DataItems[0];
                                        serviceRecordingData.ServiceRecordingHeaderID = null;
                                        serviceRecordingData.ParentServiceRecordingID = serviceRecordingData.ServiceRecordingID;
                                        serviceRecordingData.ServiceRecordingID = null;
                                        serviceRecordingData.SourceHeaderID = intakeResponseData.data.ID;
                                        //serviceRecordingData.UserID = userId;
                                        angular.forEach(serviceRecordingData.AdditionalUserList, function (item) {
                                            item.ServiceRecordingAdditionalUserID = 0;
                                        });
                                        angular.forEach(serviceRecordingData.AttendedList, function (item) {
                                            item.ServiceRecordingAttendeeID = 0;
                                        });
                                        serviceRecordingService.addServiceRecording(serviceRecordingData).then(function () {
                                            alertService.success("Service has been void successfully.");
                                            loadGridData();
                                        });
                                    }
                                });
                            });
                        }
                        else {
                            alertService.success("Service has been void successfully.");
                            loadGridData();
                        }
                    });
                }
            });

        });

        var letterGridOptions = {
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            showColumns: true,
            data: [],
            undefinedText: '',
            columns: [
                        {
                            field: 'AssessmentID',
                            title: 'IDD Intake Letter Name',
                            formatter: function (value, row, index) {
                                return (value ? lookupService.getText('Assessment', value) : "");
                            }
                        },
                        {
                            field: 'ProviderName',      //TODO: Get from ID, else will not work in offline
                            title: 'Provider'
                        },
                        {
                            field: 'SentDate',
                            title: 'Sent Date',
                            formatter: function (value, row, index) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                            }
                        },
                        {
                            field: 'LetterOutcomeID',
                            title: 'Outcome',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return lookupService.getText('LetterOutcome', value);
                                }
                                else {
                                    return "";
                                }
                            }
                        },
                        {
                            field: 'Comments',
                            title: 'Comments'
                        },
                        {
                            field: "ContactLettersID",
                            title: "Actions",
                            formatter: function (value, row, index) {
                                return ('<a href="javascript:void(0)" data-default-action id="editLetters" security permission-key="Intake-IDDLetters-Letters" permission="update" ng-click="navigateToSection(' + row["AssessmentID"] + ',' + row["ResponseID"] + ',' + row["ContactLettersID"] + ', \'edit\')" name="editLetters" data-toggle="modal" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>') +
                                    (row["SentDate"] ? ''
                                    : '<a href="javascript:void(0)" data-default-no-action security permission-key="Intake-IDDLetters-Letters" permission="delete" ng-click="deleteSection(' + value + ')" id="deleteSection" name="deleteLetter" title="Delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>')
                                    + '<a href="javascript:void(0)" data-default-no-action security permission-key="Intake-IDDLetters-Letters" permission="read" id="printLetter" name="printLetter" ng-click="printSection(' + row["AssessmentID"] + ',' + row["ResponseID"] + ',' + value + ')" title="Print" space-key-press><i class="fa fa-print fa-fw"></i></a>';
                            }
                        }
            ]
        };
        var initializeBootstrapTable = function () {
            $scope.tableoptions = ($scope.isLetter) ? letterGridOptions : formsGridOptions;
            applyDropupOnGrid(true);
        };

        $scope.navigateToSection = function (assessmentID, responseId, contactLettersID, readOnly, DocumentStatusID, isVoided) {
            var params = {
                ContactID: $scope.contactID,
                AssessmentID: assessmentID,
                ResponseID: responseId ? responseId : 0,
                ReadOnly: readOnly ? readOnly : 'edit'
            };
            if ($scope.isLetter) {
                params.ContactLettersID = contactLettersID
            }
            else {
                params.ContactFormsID = contactLettersID;
                params.DocumentStatusID = DocumentStatusID;
            }
            if (isVoided)
                cacheService.add('IsVoidedRecord', true);
            else
                cacheService.add('IsVoidedRecord', false);

            var stateUrl = $scope.isLetter ? 'patientprofile.intake.navi.letters.letternavi.lettersSection' : params.ContactFormsID ? 'formservice' : 'initializeformservice';
            assessmentService.navigateToSection(stateUrl, params);
        };

        var loadGridData = function () {
            $scope.isLetter && lettersService.get($scope.contactID).then(function (data) {
                if (hasData(data)) {
                    var gridData = data.DataItems;
                    lettersTable.bootstrapTable('load', gridData);
                }
                else {
                    lettersTable.bootstrapTable('removeAll');
                }
            });
            !$scope.isLetter && intakeFormsService.get($scope.contactID).then(function (data) {
                if (hasData(data)) {
                    var gridData = orderBy(data.DataItems, ['DocumentStatusID', '-ServiceStartDate']);
                    lettersTable.bootstrapTable('load', gridData);
                }
                else {
                    lettersTable.bootstrapTable('removeAll');
                }
            });
        };


        $scope.deleteSection = function (intakeGridID) {
            bootbox.confirm("Are you sure you want to delete this item?", function (result) {
                if (result === true) {
                    $scope.isLetter && lettersService.remove($scope.contactID, intakeGridID).then(function (data) {
                        if (data.ResultCode == 0) {
                            alertService.success('Letter deleted successfully.');
                            loadGridData();
                        }
                    }, function (errorStatus) {
                        alertService.error('Error while deleting the letter: ' + errorStatus);
                    });
                    !$scope.isLetter && intakeFormsService.remove($scope.contactID, intakeGridID).then(function (data) {
                        if (data.ResultCode == 0) {
                            alertService.success('Forms deleted successfully.');
                            loadGridData();
                        }
                    }, function (errorStatus) {
                        alertService.error('Error while deleting the form: ' + errorStatus);
                    });
                }
            });
        };

        $scope.printSection = function (assessmentID, responseID, contatLettersID) {
            let workflowDataKey = GetContactLettersWorkFlowDataKey(assessmentID);;
            let workflowHeaderID = contatLettersID;
            $scope.isLetter && assessmentPrintService.initReports(assessmentID, responseID, undefined, workflowDataKey, contatLettersID).then(onPrintReportReceived.bind(this));
            !$scope.isLetter && assessmentPrintService.initReports(assessmentID, responseID, undefined, workflowDataKey, contatLettersID).then(onPrintReportReceived.bind(this));
        }

        var onPrintReportReceived = function (data) {
            
            $scope.reportModel = data;
            $scope.reportModel.HasLoaded = true;
            $('#reportModal').modal('show');
        };

        $scope.nextSection = function (optionalAssessmentID) {
            var assessmentID = optionalAssessmentID ? optionalAssessmentID : $scope.letterID; // letterID is AssessmentID
            $scope.navigateToSection(assessmentID, 0, 0, 'edit', 0);
        };


        $scope.addNew = function () {
            $scope.letterID = null;
        };
        init();

    }]);
}());
