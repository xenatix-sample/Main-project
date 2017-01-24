(function () {
angular.module('xenatixApp')
    .controller('recordingServiceListController', ['$q', '$filter', '$injector', 'alertService', '$stateParams', 'serviceRecordingService', 'recordingServicePrintService', 'assessmentPrintService', 'roleSecurityService', 'assessmentService', 'lookupService', 'registrationService', '$scope', '_', 'lawLiaisonscreeningService', 'callCenterProgressNoteService', 'benefitsAssistanceProgressNoteService', 'callCenterAssessmentPrintService', 'httpLoaderInterceptor',
    function ($q, $filter, $injector, alertService, $stateParams, serviceRecordingService, printService, assessmentPrintService, roleSecurityService, assessmentService, lookupService, registrationService, $scope, _, lawLiaisonscreeningService, callCenterProgressNoteService, bapnService, callCenterAssessmentPrintService, httpLoaderInterceptor) {

        var self = this;
        var serviceRecording = [];
        self.navigationItems = [];
        var printInProgress = false;
        self.options = [];
        self.criteria = [];
        var durationOptions;
        self.selectedItems = [];
        var dataItems = '';
        var Criteria = function (title, field) {
            this.title = title.title;
            this.field = field;
        };

        var Option = function (item, callStatus) {
            this.value = item[self.selectedCriteria] || callStatus;
            this.text = filters(self.selectedCriteria, item);
            this.isSelected = true;
        }

        var init = function () {
            durationOptions = lookupService.getLookupsByType('Duration');
            initializeBootstrapTable();
            self.criteria = $.map(columns, function (item, indx) {
                if (item.title && item.field != Columns.DATE_AND_TIME) {
                    if (item.field == Columns.SERVICE_END_DATE) {
                        return new Criteria(item, Columns.DURATION)
                    }
                    else {
                        return new Criteria(item, item.field)
                    }
                }
            });
        };

        var Columns = {
            SERVICE_END_DATE: 'DurationToDisplay',
            DOCUMENT_STATUS_ID: 'DocumentStatusID',
            CALL_STATUS_ID: 'CallStatusID',
            DATE_AND_TIME: 'ServiceStartDate',
            STATUS_NAME: 'StatusName',
            DURATION: 'ServiceDurationID'
        }

        self.onCriteriaSelect = function () {
            self.options = [];
            if (self.selectedCriteria) {
                if (angular.equals(self.selectedCriteria, Columns.DURATION)) {
                    $.map(dataItems, function (item, indx) {
                        if (item[self.selectedCriteria]) {
                            self.options = durationOptions;
                        }
                    })
                } else {                    
                    self.options = $.map(dataItems, function (item, indx) {
                        if (item[self.selectedCriteria] && self.selectedCriteria != Columns.DOCUMENT_STATUS_ID) {
                            return new Option(item)
                        }
                        else if (self.selectedCriteria == Columns.DOCUMENT_STATUS_ID && (item[self.selectedCriteria] || item[Columns.CALL_STATUS_ID])) {
                            return new Option(item, item[Columns.CALL_STATUS_ID])
                        }
                    })
                    self.options = $filter('orderBy')(self.options, 'text');
                }
            }
            else {
                self.selectedItems = [];
            }
            self.options = _.distinct(self.options, 'value');
        }

        var filters = function (column, item) {
            switch (column) {
                case "ServiceItemID":
                    return serviceRecordingService.getText('RecordingServices', item[column]);
                case "ServiceRecordingSourceID":
                    return serviceRecordingService.getText('ServiceRecordingSource', item[column], "DisplayText");
                case "ServiceStartDate":
                    return item[column] ? $filter('toMMDDYYYYDate')(item[column], 'MM/DD/YYYY hh:mm', 'useLocal') : ''
                case "ServiceEndDate":
                    return serviceRecordingService.calcDuration(item.ServiceStartDate, item[column]);
                case "OrganizationID":
                    return serviceRecordingService.getOrganizationText('ProgramUnit', item[column], item.ServiceRecordingSourceID);
                case "UserID":
                    return serviceRecordingService.getText('Users', item[column]);
                case "DocumentStatusID":
                    return serviceRecordingService.getStatus(item);
                case "Source":
                case "Service":
                case "Status":
                    return item[column];                
                default:
                    return '';
            }
        }

        $scope.onMultiSelect = function () {
            var filterdData = [];
            if (self.selectedItems.length > 0) {   //if any Criteria selected
                angular.forEach(dataItems, function (data) {
                    $filter('filter')(self.selectedItems, function (item) {
                        if (data[self.selectedCriteria] == item.value && self.selectedCriteria != Columns.DOCUMENT_STATUS_ID) {
                            filterdData.push(data);
                        }
                        else if (data[Columns.STATUS_NAME] == item.text && self.selectedCriteria == Columns.DOCUMENT_STATUS_ID) {
                            filterdData.push(data);
                        }
                    })
                });
                $("#serviceTable").bootstrapTable('load', filterdData);
            }
            else if (self.selectedItems.length == 0 && self.selectedCriteria) {
                $("#serviceTable").bootstrapTable('load', filterdData);// if  any Criteria selected and no multiselect
            }
            else {
                $("#serviceTable").bootstrapTable('load', dataItems);// if nothing is selected
            }
        }

        var calcDuration = function (startDate, endDate) {
            var now = moment(startDate);
            var then = moment(endDate);
            return (moment.duration(then.diff(now)) / 60) / 1000;  //return in minutes
        }

        $scope.onDateChange = function (startDate, endDate) {
            self.selectedCriteria = '';
            self.selectedItems = [];
            self.options = [];
            get($filter('toMMDDYYYYDate')(startDate, 'MM/DD/YYYY', 'useLocal'), $filter('toMMDDYYYYDate')(endDate, 'MM/DD/YYYY', 'useLocal'));
        }

        var get = function (startDate, endDate) {
            var contactId = $stateParams.ContactID;
            return serviceRecordingService.getServiceRecordings(contactId, startDate, endDate).then(function (data) {
                if (hasDetails(data)) {
                    dataItems = data;
                    $("#serviceTable").bootstrapTable('load', data);
                } else {
                    $("#serviceTable").bootstrapTable('removeAll');
                }
            }, function (errorStatus) {
                alertService.error('Unable to connect to server');
            });
        };

        var initializeBootstrapTable = function () {
            self.tableoptions = {
                pagination: true,
                pageSize: 10,
                pageList: [10, 25, 50, 100],
                search: false,
                showColumns: true,
                data: [],
                undefinedText: '',
                columns: columns
            };
        };

        var columns = [

                    {
                        field: 'Source',
                        sortable: true,
                        title: 'Source',
                        formatter: function (value, row) {
                            row.Source = serviceRecordingService.getText('ServiceRecordingSource', row.ServiceRecordingSourceID, "DisplayText")
                            return row.Source;
                        }
                    },
                    {
                        field: 'Service',
                        sortable: true,
                        title: 'Service',
                        formatter: function (value, row) {
                            row.Service = serviceRecordingService.getText('RecordingServices', row.ServiceItemID, 'ServiceName', 'ServiceID');
                            return row.Service;
                        }
                    },

                    {
                        field: 'ServiceStartDate',
                        sortable: true,
                        title: 'Date and Time',
                        formatter: function (value, row, index) {
                            row.DateAndTime = value ? toStringDateTime(value) : ''
                            return row.DateAndTime;
                        }
                    },
                    {
                        field: 'DurationToDisplay',
                        sortable: true,
                        title: 'Duration',
                        formatter: function (value, row, index) {                         
                            row.DurationToDisplay = formatDurationToSort(row.Duration);
                            row.CalculatedDuration = calculateDuration(row.ServiceStartDate, row.ServiceEndDate);
                            return row.CalculatedDuration;
                        }
                    },
                    {
                        field: 'OrganizationID',
                        sortable: true,
                        title: 'Program Unit',
                        sortName: 'programUnit',
                        formatter: function (value, row, index) {
                            if (value && row.ServiceRecordingSourceID) {
                                row.programUnit = serviceRecordingService.getOrganizationText('ProgramUnit', value, row.ServiceRecordingSourceID);
                                return row.programUnit;
                            } else {
                                row.programUnit = '';
                                return row.programUnit;
                            }
                        }
                    },
                    {
                        field: 'UserID',
                        sortable: true,
                        title: 'Provider',
                        formatter: function (value, row, index) {
                            return serviceRecordingService.getText('Users', value);
                        }
                    },
                    {
                        field: 'Status',
                        sortable: true,
                        title: 'Status',                   
                        formatter: function (value, row) {
                            row.Status = serviceRecordingService.getStatus(row);
                            return row.Status;
                        }
                    },
                    {
                        field: 'SourceHeaderID',
                        title: '',
                        formatter: function (value, row, index) {
                            var status = serviceRecordingService.getStatus(row);
                            return '<a href="javascript:void(0)" data-default-action id="print" name="print" ng-click="ctrl.print(' + value + ', ' + row["ServiceRecordingSourceID"] + ',' +
                                row["ServiceRecordingID"] + ',\'' + status.toString() + '\')" title="Print" security permission-key="Chart-Chart-RecordedServices" permission="read" space-key-press><i class="fa fa-print fa-fw"></i></a>';
                        }
                    }
        ]
        
      
        self.print = function (sourceHeaderID, serviceRecordingSourceID, serviceRecordingID, serviceStatus) {
            var tableData = $("#serviceTable").bootstrapTable('getData');
            var dataFlyout = $filter('filter')(tableData, function (item) {
                return (item.ServiceRecordingSourceID === serviceRecordingSourceID && item.ServiceRecordingID === serviceRecordingID)
            });

            if (dataFlyout.length > 0) {      //check if filter returns data. It should return 1 row always
                self.flyoutModel = dataFlyout[0];
                getNavigationItems(serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus).then(function (data) {
                    self.navItems = data;
                }).finally(function () {
                    self.isServiceFlyout = true;
                    $('#serviceFlyoutCanvas').addClass('active');
                });
            }
        }

        var isSigned = function (items) {
            var staffSig = false;
            angular.forEach(items, function (value, key) {
                if (value && value.OptionsID == 3701) {
                    staffSig = true;
                }
            });
            return staffSig;
        }

        var callCenterNoteModel = {};

        var getCrisisNavItems = function (serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission, hasAssessmentPemission) {
            var navItems = [], promiseArr = [];
            var dfd = $q.defer();
            var signedOnly, qArr = [];
            var callerInformationService = $injector.get('callerInformationService');
            var callCenterProgressNoteService = $injector.get('callCenterProgressNoteService');
            var eSignatureService = $injector.get('eSignatureService');
            var currentIndex = 1;
            if (callerInformationService) {
                qArr.push(callerInformationService.get(sourceHeaderID));      //To check call status
                qArr.push(callerInformationService.getCallCenterAssessmentResponse(sourceHeaderID, 0));    //will get all the responses for the current call center workflow
                qArr.push(callCenterProgressNoteService ? callCenterProgressNoteService.get(sourceHeaderID) : 1);
                $q.all(qArr).then(function (data) {
                    if (hasData(data[0])) {
                        var callerData = data[0].DataItems[0];
                        if (callerData.CallStatusID === CALL_STATUS.COMPLETE || callerData.CallStatusID === CALL_STATUS.VOID || callerData.DocumentStatusID == DOCUMENT_STATUS.Completed || callerData.DocumentStatusID == DOCUMENT_STATUS.Void) {
                            signedOnly = true;
                        }
                        //data[1] will get all the responses for the current call center workflow
                        var columbia, crisis, adult, child, progressNote;
                        if (hasData(data[1])) {
                            columbia = $filter('filter')(data[1].DataItems, { AssessmentID: ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale }, true);
                            crisis = $filter('filter')(data[1].DataItems, { AssessmentID: ASSESSMENT_TYPE.CrisisAssessment }, true);
                            adult = $filter('filter')(data[1].DataItems, { AssessmentID: ASSESSMENT_TYPE.CrisisAdultScreening }, true);
                            child = $filter('filter')(data[1].DataItems, { AssessmentID: ASSESSMENT_TYPE.CrisisChildScreening }, true);
                            progressNote = $filter('filter')(data[1].DataItems, { AssessmentID: ASSESSMENT_TYPE.CrisisLineProgressNote }, true);
                        }
                        
                        promiseArr.push(hasDetails(columbia) ? assessmentService.getAssessmentResponseDetails(columbia[0].ResponseID, ASSESSMENT_SECTION.ColumbiaSuicideScale) : 1);
                        promiseArr.push(hasDetails(crisis) ? assessmentService.getAssessmentResponseDetails(crisis[0].ResponseID, ASSESSMENT_SECTION.CrisisAssessment) : 1);
                        promiseArr.push(hasDetails(adult) ? assessmentService.getAssessmentResponseDetails(adult[0].ResponseID, ASSESSMENT_SECTION.AdultScreening) : 1);
                        promiseArr.push(hasDetails(child) ? assessmentService.getAssessmentResponseDetails(child[0].ResponseID, ASSESSMENT_SECTION.ChildScreening) : 1);
                        promiseArr.push((hasData(data[2]) && data[2].DataItems[0].ProgressNoteID && signedOnly) ? eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenterProgressNote, sourceHeaderID) : 1);     //check if note signed only if required
                        callCenterNoteModel = data[2];
                        navItems.push({ hasPermission: true, currentIndex: currentIndex, title: 'Service', printType: 'service', sourceHeaderID: sourceHeaderID, serviceRecordingSourceID: serviceRecordingSourceID, serviceRecordingID: serviceRecordingID });

                        $q.all(promiseArr).then(function (response) {
                            if (response[0] !== 1 && hasData(response[0].data) && (!signedOnly || (signedOnly && isSigned(response[0].data.DataItems)))) {
                                if (hasAssessmentPemission)
                                    currentIndex += 1;
                                navItems.push({
                                    hasPermission: hasAssessmentPemission,
                                    currentIndex: hasAssessmentPemission ? currentIndex : null,
                                    printType: 'columbiaAssessment',
                                    sourceHeaderID: sourceHeaderID,
                                    assessmentID: ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale,
                                    title: 'Columbia Suicide Scale',
                                    responseID: columbia[0].ResponseID,
                                    sectionID: ASSESSMENT_SECTION.ColumbiaSuicideScale
                                });
                            }
                            if (response[1] !== 1 && hasData(response[1].data) && (!signedOnly || (signedOnly && isSigned(response[1].data.DataItems)))) {
                                if (hasAssessmentPemission)
                                    currentIndex += 1;
                                navItems.push({
                                    hasPermission: hasAssessmentPemission,
                                    currentIndex: hasAssessmentPemission ? currentIndex : null,
                                    printType: 'crisisAssessment',
                                    sourceHeaderID: sourceHeaderID,
                                    assessmentID: ASSESSMENT_TYPE.CrisisAssessment,
                                    title: 'Crisis Assessment',
                                    responseID: crisis[0].ResponseID,
                                    sectionID: ASSESSMENT_SECTION.CrisisAssessment
                                });
                            }
                            if (response[2] !== 1 && hasData(response[2].data) && (!signedOnly || (signedOnly && isSigned(response[2].data.DataItems)))) {
                                if (hasAssessmentPemission)
                                    currentIndex += 1;
                                navItems.push({
                                    hasPermission: hasAssessmentPemission,
                                    currentIndex: hasAssessmentPemission ? currentIndex: null,
                                    printType: 'adultScreening',
                                    sourceHeaderID: sourceHeaderID,
                                    assessmentID: ASSESSMENT_TYPE.CrisisAdultScreening,
                                    title: 'Adult Screening',
                                    responseID: adult[0].ResponseID,
                                    sectionID: ASSESSMENT_SECTION.AdultScreening
                                });
                            }
                            if (response[3] !== 1 && hasData(response[3].data) && (!signedOnly || (signedOnly && isSigned(response[3].data.DataItems)))) {
                                if (hasAssessmentPemission)
                                    currentIndex += 1;
                                navItems.push({
                                    hasPermission: hasAssessmentPemission,
                                    currentIndex: hasAssessmentPemission ? currentIndex: null,
                                    printType: 'childScreening',
                                    sourceHeaderID: sourceHeaderID,
                                    assessmentID: ASSESSMENT_TYPE.CrisisChildScreening,
                                    title: 'Child Screening',
                                    responseID: child[0].ResponseID,
                                    sectionID: ASSESSMENT_SECTION.ChildScreening
                                });
                            }
                            if ((!signedOnly && hasData(data[2]) && data[2].DataItems[0].ProgressNoteID) || (signedOnly && hasData(response[4]))) {
                                if (hasNotePermission)
                                    currentIndex += 1;
                                navItems.push({
                                    hasPermission: hasNotePermission,
                                    currentIndex: hasNotePermission ? currentIndex : null,
                                    signature: hasData(response[4]) ? response[4].DataItems[0] : 0,
                                    printType: 'crisisProgressNote',
                                    sourceHeaderID: sourceHeaderID,
                                    assessmentID: ASSESSMENT_TYPE.CrisisLineProgressNote,
                                    title: 'Progress Note',
                                    responseID: progressNote[0].ResponseID,
                                    sectionID: ASSESSMENT_SECTION.CrisisLineProgressNote
                                });
                            }
                            dfd.resolve(navItems);
                        });
                    } else {
                        dfd.resolve(navItems);
                    }
                }).catch(function (error)
                {
                    console.log(error);
                });
            } else {
                dfd.resolve(navItems);
            }
            return dfd.promise;
        };

        var populateLawLiaisonNavItem = function (navItems, data, signedOnly, currentIndex, responseId, sourceHeaderID, hasNotePermission) {
            var dfd = $q.defer();
            var hasLLPermission = roleSecurityService.hasPermission(CallCenterPermissionKey.CallCenter_LawLiaison, PERMISSION.READ);
            if (!hasLLPermission)
            {
                dfd.resolve(navItems);
                return dfd.promise;
            }
            if (hasData(data) && data.DataItems[0].ProgressNoteID) {
                if (signedOnly) {
                    var eSignatureService = $injector.get('eSignatureService');
                    eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenterProgressNote, sourceHeaderID).then(function (response) {
                        if (hasData(response)) {
                            if (hasNotePermission)
                                currentIndex += 1;
                            navItems.push({
                                hasPermission: hasNotePermission,
                                currentIndex : hasNotePermission ? currentIndex: null,
                                printType: 'lawLiaisonProgressNote',
                                assessmentID: ASSESSMENT_TYPE.LawLiaisonProgressNote,
                                signature: response.DataItems[0],
                                title: 'Progress Note',
                                sourceHeaderID: sourceHeaderID,
                                responseID: responseId,
                                sectionID: ASSESSMENT_SECTION.LawLiaisonProgressNote
                            });
                        }
                        dfd.resolve(navItems);
                    });
                } else {
                    if (hasNotePermission)
                        currentIndex += 1;
                    navItems.push({
                        hasPermission: hasNotePermission,
                        currentIndex : hasNotePermission ? currentIndex: null,
                        printType: 'lawLiaisonProgressNote',
                        sourceHeaderID: sourceHeaderID,
                        assessmentID: ASSESSMENT_TYPE.LawLiaisonProgressNote,
                        title: 'Progress Note',
                        responseID: responseId,
                        sectionID: ASSESSMENT_SECTION.LawLiaisonProgressNote
                    });
                    dfd.resolve(navItems);
                }
            } else {
                dfd.resolve(navItems);
            }
            return dfd.promise;
        };

        var getLawLiaisonNavItems = function (serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission, hasAssessmentPemission) {
            var navItems = [], promiseArr = [];
            var dfd = $q.defer();
            var signedOnly, qArr = [];

            var currentIndex = 1;
            if (serviceStatus == 'Completed' || serviceStatus == 'Void') {
                signedOnly = true;
            }

            var callerInformationService = $injector.get('callerInformationService');
            var callCenterProgressNoteService = $injector.get('callCenterProgressNoteService');
            navItems.push({
                hasPermission: true,
                currentIndex: currentIndex,
                title: 'Service',
                printType: 'service',
                sourceHeaderID: sourceHeaderID,
                serviceRecordingSourceID: serviceRecordingSourceID,
                serviceRecordingID: serviceRecordingID
            });
            if (callerInformationService && callCenterProgressNoteService) {
                qArr.push(callerInformationService.getCallCenterAssessmentResponse(sourceHeaderID, 0));    //As assessment data is mandatory in Screening, so this call will return value if it is saved
                qArr.push(callCenterProgressNoteService ? callCenterProgressNoteService.get(sourceHeaderID) : 1);
                $q.all(qArr).then(function (data) {
                    var screening, lawProgressNote, screeningResponseId, noteResponseId;;
                    if (hasData(data[0])) {
                        screening = $filter('filter')(data[0].DataItems, { AssessmentID: ASSESSMENT_TYPE.LawLiaisonScreening }, true);
                        lawProgressNote = $filter('filter')(data[0].DataItems, { AssessmentID: ASSESSMENT_TYPE.LawLiaisonProgressNote }, true);
                        if (hasDetails(screening)) {
                            screeningResponseId = screening[0].ResponseID;
                            assessmentService.getAssessmentResponseDetails(screeningResponseId, ASSESSMENT_SECTION.LawLiaisonScreening).then(function (resp) {
                                if (hasData(resp.data)) {
                                    if(hasAssessmentPemission)
                                        currentIndex += 1;
                                    navItems.push({
                                        hasPermission: hasAssessmentPemission,
                                        currentIndex: hasAssessmentPemission ? currentIndex: null,
                                        printType: 'lawLiaisonScreening',
                                        sourceHeaderID: sourceHeaderID,
                                        assessmentID: ASSESSMENT_TYPE.LawLiaisonScreening,
                                        title: 'Law Liaison Screening',
                                        responseID: screeningResponseId,
                                        sectionID: ASSESSMENT_SECTION.LawLiaisonScreening
                                    });
                                }
                                if (hasDetails(lawProgressNote)) {
                                    noteResponseId = lawProgressNote[0].ResponseID;
                                }
                                populateLawLiaisonNavItem(navItems, data[1], signedOnly, currentIndex, noteResponseId, sourceHeaderID, hasNotePermission).then(function (navigationItem) {
                                    dfd.resolve(navigationItem);
                                });
                            });
                        } else {
                            if (hasDetails(lawProgressNote)) {
                                noteResponseId = lawProgressNote[0].ResponseID;
                            }
                            populateLawLiaisonNavItem(navItems, data[1], signedOnly, currentIndex, noteResponseId, sourceHeaderID, hasNotePermission).then(function (navigationItem) {
                                dfd.resolve(navigationItem);
                            });
                        }
                    } else {
                        populateLawLiaisonNavItem(navItems, data[1], signedOnly, currentIndex, noteResponseId, sourceHeaderID, hasNotePermission).then(function (navigationItem) {
                            dfd.resolve(navigationItem);
                        });
                    }
                    callCenterNoteModel = data[1];
                });

            }

            return dfd.promise;
        };

        var getIDDFormNavItems = function (serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission, hasAssessmentPemission) {
            var navItems = [], promiseArr = [];
            var dfd = $q.defer();
            var signedOnly;
            var currentIndex = 1;
            if (serviceStatus == 'Completed' || serviceStatus == 'Void') {
                signedOnly = true;
            }
            navItems.push({
                hasPermission: true,
                currentIndex: currentIndex,
                title: 'Service',
                printType: 'service',
                sourceHeaderID: sourceHeaderID,
                serviceRecordingSourceID: serviceRecordingSourceID,
                serviceRecordingID: serviceRecordingID
            });
            var intakeFormsService = $injector.get('intakeFormsService');
            intakeFormsService.getIntakeForm(sourceHeaderID).then(function (data) {
                if (hasData(data) && data.DataItems[0].ResponseID) {
                    promiseArr.push(assessmentService.getAssessmentResponseDetails(data.DataItems[0].ResponseID, ASSESSMENT_SECTION.IDDFormsNote));
                    promiseArr.push(assessmentService.getAssessmentResponseDetails(data.DataItems[0].ResponseID, ASSESSMENT_SECTION.IdentificationofPreferencesDADS8648));
                    promiseArr.push(assessmentService.getAssessmentResponseDetails(data.DataItems[0].ResponseID, ASSESSMENT_SECTION.InterestListforGRServices));
                    promiseArr.push(assessmentService.getAssessmentResponseDetails(data.DataItems[0].ResponseID, ASSESSMENT_SECTION.InterestListQuestionnaireDADS8577));
                    promiseArr.push(assessmentService.getAssessmentResponseDetails(data.DataItems[0].ResponseID, ASSESSMENT_SECTION.ServiceCoordinationAssessmentDADS8647));
                    $q.all(promiseArr).then(function (response) {
                        if (hasData(response[0].data) && (!signedOnly || (signedOnly && isSigned(response[0].data.DataItems)))) {
                            if (hasNotePermission)
                                currentIndex += 1;
                            navItems.push({
                                hasPermission: hasNotePermission,
                                currentIndex: hasNotePermission ? currentIndex : null,
                                printType: 'assessment',
                                assessmentID: ASSESSMENT_TYPE.IDDIntakeForms,
                                title: 'General Note',
                                sourceHeaderID: sourceHeaderID,
                                responseID: data.DataItems[0].ResponseID,
                                sectionID: ASSESSMENT_SECTION.IDDFormsNote
                            });     //(this is the "Note" within the IDD Intake section workflow)
                        }
                        if (hasData(response[1].data) && (!signedOnly || (signedOnly && isSigned(response[1].data.DataItems)))) {
                            if(hasNotePermission)
                                currentIndex += 1;
                            navItems.push({
                                hasPermission: hasNotePermission,
                                currentIndex: hasNotePermission ? currentIndex : null,
                                printType: 'assessment',
                                sourceHeaderID: sourceHeaderID,
                                assessmentID: ASSESSMENT_TYPE.IDDIntakeForms,
                                title: 'Identification of Preferences - DADS 8648',
                                responseID: data.DataItems[0].ResponseID,
                                sectionID: ASSESSMENT_SECTION.IdentificationofPreferencesDADS8648
                            });
                        }
                        if (hasData(response[2].data) && (!signedOnly || (signedOnly && isSigned(response[2].data.DataItems)))) {
                            if(hasNotePermission)
                                currentIndex += 1;
                            navItems.push({
                                hasPermission: hasNotePermission,
                                currentIndex: hasNotePermission ? currentIndex : null,
                                printType: 'assessment',
                                sourceHeaderID: sourceHeaderID,
                                assessmentID: ASSESSMENT_TYPE.IDDIntakeForms,
                                title: 'Interest List for GR Services',
                                responseID: data.DataItems[0].ResponseID,
                                sectionID: ASSESSMENT_SECTION.InterestListforGRServices
                            });
                        }
                        if (hasData(response[3].data) && (!signedOnly || (signedOnly && isSigned(response[3].data.DataItems)))) {
                            if (hasNotePermission)
                                currentIndex += 1;
                            navItems.push({
                                hasPermission: hasNotePermission,
                                currentIndex: hasNotePermission ? currentIndex : null,
                                printType: 'assessment',
                                sourceHeaderID: sourceHeaderID,
                                assessmentID: ASSESSMENT_TYPE.IDDIntakeForms,
                                title: 'Interest List Questionnaire - DADS 8577',
                                responseID: data.DataItems[0].ResponseID,
                                sectionID: ASSESSMENT_SECTION.InterestListQuestionnaireDADS8577
                            });
                        }
                        if (hasData(response[4].data) && (isSigned(response[4].data.DataItems))) {
                            if (hasAssessmentPemission)
                                currentIndex += 1;
                            navItems.push({
                                hasPermission: hasAssessmentPemission,
                                currentIndex: hasAssessmentPemission ? currentIndex : null,
                                printType: 'assessment',
                                sourceHeaderID: sourceHeaderID,
                                assessmentID: ASSESSMENT_TYPE.IDDIntakeForms,
                                title: 'Service Coordination Assessment - DADS 8647',
                                responseID: data.DataItems[0].ResponseID,
                                sectionID: ASSESSMENT_SECTION.ServiceCoordinationAssessmentDADS8647
                            });
                        }
                        dfd.resolve(navItems);
                    });
                } else {
                    dfd.resolve(navItems);
                }
            });
            return dfd.promise;
        };

        var getBAPNNavItems = function (serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission) {
            var dfd = $q.defer();
            var signedOnly, navItems = [], promiseArr = [], currentIndex = 1;
            if (serviceStatus == 'Completed' || serviceStatus == 'Void') {
                signedOnly = true;
            }
            navItems.push({ hasPermission: true, currentIndex: currentIndex, title: 'Service', printType: 'service', sourceHeaderID: sourceHeaderID, serviceRecordingSourceID: serviceRecordingSourceID, serviceRecordingID: serviceRecordingID });
            bapnService.get($stateParams.ContactID, sourceHeaderID).then(function (response) {
                if (hasData(response) && response.DataItems[0].ResponseID) {
                    assessmentService.getAssessmentResponseDetails(response.DataItems[0].ResponseID, BAPN_SECTIONS.Note).then(function (data) {
                        if (hasData(data.data)) {
                            if (signedOnly) {
                                //check if signed
                                if (isSigned(data.data.DataItems)) {
                                    if (hasNotePermission)
                                        currentIndex += 1;
                                    navItems.push({
                                        hasPermission: hasNotePermission,
                                        currentIndex: hasNotePermission ? currentIndex : null,
                                        printType: 'bapnNote',
                                        sourceHeaderID: sourceHeaderID,
                                        assessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                                        title: 'General Note',
                                        responseID: response.DataItems[0].ResponseID,
                                        sectionID: ASSESSMENT_SECTION.BAPNNote
                                    });   //(this is the "Note" within the BAPN section workflow)
                                    if (hasNotePermission)
                                        currentIndex += 1;
                                    navItems.push({
                                        hasPermission: hasNotePermission,
                                        currentIndex: hasNotePermission ? currentIndex : null,
                                        printType: 'bapn',
                                        sourceHeaderID: sourceHeaderID,
                                        assessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                                        title: 'Benefits Assistance Progress Note',
                                        responseID: response.DataItems[0].ResponseID
                                        //sectionID: ASSESSMENT_SECTION.BAPNNote
                                    });
                                    dfd.resolve(navItems);
                                } else {
                                    if (hasNotePermission)
                                        currentIndex += 1;
                                    navItems.push({
                                        hasPermission: hasNotePermission,
                                        currentIndex: hasNotePermission ? currentIndex : null,
                                        printType: 'bapn',
                                        sourceHeaderID: sourceHeaderID,
                                        assessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                                        title: 'Benefits Assistance Progress Note',
                                        responseID: response.DataItems[0].ResponseID
                                        //sectionID: ASSESSMENT_SECTION.BAPNNote
                                    });
                                    dfd.resolve(navItems);
                                }
                            } else {
                                if (hasNotePermission)
                                    currentIndex += 1;
                                navItems.push({
                                    hasPermission: hasNotePermission,
                                    currentIndex: hasNotePermission ? currentIndex : null,
                                    printType: 'bapnNote',
                                    sourceHeaderID: sourceHeaderID,
                                    assessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                                    title: 'General Note',
                                    responseID: response.DataItems[0].ResponseID,
                                    sectionID: ASSESSMENT_SECTION.BAPNNote
                                });   //(this is the "Note" within the BAPN section workflow)
                                if (hasNotePermission)
                                    currentIndex += 1;
                                navItems.push({
                                    hasPermission: hasNotePermission,
                                    currentIndex: hasNotePermission ? currentIndex : null,
                                    printType: 'bapn',
                                    sourceHeaderID: sourceHeaderID,
                                    assessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                                    title: 'Benefits Assistance Progress Note',
                                    responseID: response.DataItems[0].ResponseID
                                });
                                dfd.resolve(navItems);
                            }
                        } else {
                            if (hasNotePermission)
                                currentIndex += 1;
                            navItems.push({
                                hasPermission: hasNotePermission,
                                currentIndex: hasNotePermission ? currentIndex : null,
                                printType: 'bapn',
                                sourceHeaderID: sourceHeaderID,
                                assessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                                title: 'Benefits Assistance Progress Note',
                                responseID: response.DataItems[0].ResponseID
                            });
                            dfd.resolve(navItems);
                        }
                    });
                } else {
                    if (hasNotePermission)
                        currentIndex += 1;
                    navItems.push({
                        hasPermission: hasNotePermission,
                        currentIndex: hasNotePermission ? currentIndex : null,
                        printType: 'bapn',
                        sourceHeaderID: sourceHeaderID,
                        assessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                        title: 'Benefits Assistance Progress Note',
                        responseID: response.DataItems[0].ResponseID
                    });
                    dfd.resolve(navItems);
                }
            });
            return dfd.promise;
        };

        var getNavigationItems = function (serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus) {
            var hasNotePermission = roleSecurityService.hasPermission(ChartPermissionKey.Chart_Chart_Note, PERMISSION.READ);
            var hasAssessmentPemission = roleSecurityService.hasPermission(ChartPermissionKey.Chart_Chart_Assessment, PERMISSION.READ);
            var dfd = $q.defer();
            switch (serviceRecordingSourceID) {
                case SERVICE_RECORDING_SOURCE.CallCenter:
                    getCrisisNavItems(serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission, hasAssessmentPemission).then(function (navItems) {
                        self.navigationItems = navItems;
                        angular.extend(self.navigationItems, { workflowDataKey: WORKFLOWHEADER_DATAKEY.Callcenter_CrisisLine });
                        dfd.resolve(navItems);
                    });
                    break;
                case SERVICE_RECORDING_SOURCE.LawLiaison:
                    getLawLiaisonNavItems(serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission, hasAssessmentPemission).then(function (navItems) {
                        self.navigationItems = navItems;
                        angular.extend(self.navigationItems, { workflowDataKey: WORKFLOWHEADER_DATAKEY.Callcenter_Lawliaison });
                        dfd.resolve(navItems);
                    });
                    break;
                case SERVICE_RECORDING_SOURCE.IDDForms:
                    getIDDFormNavItems(serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission, hasAssessmentPemission).then(function(navItems) {
                        self.navigationItems = navItems;
                        angular.extend(self.navigationItems, { workflowDataKey: WORKFLOWHEADER_DATAKEY.Intake_IDD_Forms });
                        dfd.resolve(navItems);
                    });
                    break;
                case SERVICE_RECORDING_SOURCE.BAPN:
                    getBAPNNavItems(serviceRecordingSourceID, serviceRecordingID, sourceHeaderID, serviceStatus, hasNotePermission).then(function (navItems) {
                        self.navigationItems = navItems;
                        angular.extend(self.navigationItems, { workflowDataKey: WORKFLOWHEADER_DATAKEY.BAPN_BAPN });
                        dfd.resolve(navItems);
                    });
                    break;
            }
            return dfd.promise;
        };

        self.onReportPrint = function (printObj) {
            if (printInProgress)
                return;
            printInProgress = true;
            httpLoaderInterceptor.ignore(false);
            self.currentIndex = printObj.currentIndex;
            self.printableNavigationItems = Math.max.apply(null, self.navigationItems.map(function (item) { return item.currentIndex; }));
            //self.printableNavigationItems = self.navigationItems.filter(function (item) { return item.currentIndex != null }).length;
            var item = $filter('filter')(self.navigationItems, { currentIndex: self.currentIndex }, true);
            if (hasDetails(item)){
                switch (item[0].printType) {
                    case 'service':
                        printService.initPrint(item[0].sourceHeaderID, item[0].serviceRecordingSourceID, item[0].serviceRecordingID, self.navigationItems.workflowDataKey).then(onPrintReportReceived.bind(this, item[0]));
                        break;
                    case 'crisisAssessment':
                    case 'columbiaAssessment':
                    case 'adultScreening':
                    case 'childScreening':
                        getCallCenterModel(item[0]).then(onPrintReportReceived.bind(this, item[0]));
                        break;
                    case 'assessment':
                        assessmentPrintService.initReports(item[0].assessmentID, item[0].responseID, item[0].sectionID, self.navigationItems.workflowDataKey, item[0].sourceHeaderID).then(onPrintReportReceived.bind(this, item[0]));
                        break;
                    case 'lawLiaisonScreening': case 'crisisProgressNote': case 'lawLiaisonProgressNote':
                        onPrintReportReceived(item[0], {});
                        break;
                    case 'bapnNote':
                        var serviceStartDate = $filter('formatDate')(self.flyoutModel.ServiceStartDate);
                        return bapnService.initReport(item[0].assessmentID, item[0].responseID, item[0].sectionID, $stateParams.ContactID, serviceStartDate, self.navigationItems.workflowDataKey, item[0].sourceHeaderID).then(onPrintReportReceived.bind(this, item[0]));
                        break;
                    case 'bapn':
                        var serviceStartDate = $filter('formatDate')(self.flyoutModel.ServiceStartDate);
                        return bapnService.initReport(item[0].assessmentID, item[0].responseID, item[0].sectionID, $stateParams.ContactID, serviceStartDate, true, self.flyoutModel.ServiceRecordingID, self.navigationItems.workflowDataKey, item[0].sourceHeaderID).then(onPrintReportReceived.bind(this, item[0]));
                        break;
                }
            }
        };

        var onPrintReportReceived = function (printObj, data) {
            $('#reportModal').modal('show');
            getReportModel(printObj, data).then(function (response) {
                response.HasLoaded = true;
                response.ReportHeader = printObj.title;
                self.reportModel = response;
                httpLoaderInterceptor.ignore(true);
                printInProgress = false;
            });
        };
        
        var getReportModel = function (printObj, data) {
            var dfd = $q.defer(), reportModel = {}, promiseArr = [];
            data.HasLoaded = true;
            switch (printObj.printType) {
                case 'crisisProgressNote': case 'lawLiaisonProgressNote':
                    return callCenterProgressNoteService.initReport(printObj.assessmentID, printObj.responseID, printObj.sectionID, printObj.sourceHeaderID, printObj.signature, printObj.printType, self.flyoutModel,undefined, self.navigationItems.workflowDataKey);
                    break;
                case 'lawLiaisonScreening':
                    var serviceDate = $filter('formatDate')(self.flyoutModel.ServiceStartDate);
                    var parentPromiseArr = [];
                    var callerInformationService = $injector.get('callerInformationService');
                    parentPromiseArr.push(registrationService.get($stateParams.ContactID));
                    parentPromiseArr.push(callerInformationService.get(printObj.sourceHeaderID));
                    return $q.all(parentPromiseArr).then(function (response) {
                        var callerInformation, callerDetails;
                        if (hasData(response[0])) {
                            callerInformation = response[0].DataItems[0];
                        }
                        if (hasData(response[1])) {
                            callerDetails = response[1].DataItems[0];
                        }

                        return lawLiaisonscreeningService.initReport(printObj.assessmentID, printObj.responseID, printObj.sectionID, printObj.sourceHeaderID, $stateParams.ContactID, serviceDate, callerInformation, callerDetails, undefined, undefined, undefined, self.navigationItems.workflowDataKey);
                    });
                    break;
                case 'columbiaAssessment':
                    data.ReportName = 'Columbia-SuicideSeverityRatingScale';
                    dfd.resolve(data);
                    break;
                case 'crisisAssessment':
                    data.ReportName = 'CrisisAssessment';
                    dfd.resolve(data);
                    break;
                case 'childScreening':
                    data.ReportName = 'CrisisChildScreening';
                    dfd.resolve(data);
                    break;
                case 'adultScreening':
                    data.ReportName = 'CrisisAdultScreening';
                    dfd.resolve(data);
                    break;
                default:
                    //populate header data
                    dfd.resolve(data);
                    break;
            }
            return dfd.promise;
        }

        var getCallCenterModel = function (printObj) {
            var dfd = $q.defer(), promiseArr = [], data = {};
            promiseArr.push(callCenterAssessmentPrintService.getPrintData($stateParams.ContactID, printObj.assessmentID, printObj.responseID, printObj.sectionID));
            promiseArr.push(callCenterAssessmentPrintService.getContactReprotInformation($stateParams.ContactID, printObj.sourceHeaderID, self.navigationItems.workflowDataKey));
            $q.all(promiseArr).then(function (response) {
                if (response[0]) {
                    angular.merge(data, response[0]);
                }
                if (response[1]) {
                    angular.merge(data, response[1]);
                }
                dfd.resolve(data);
            });
            return dfd.promise;
        }

        self.movePreviousDocument = function () {
            if (self.currentIndex > 1 && !printInProgress)
            self.onReportPrint({ currentIndex: self.currentIndex - 1 });
        };

        self.moveNextDocument = function () {
            if (self.printableNavigationItems > self.currentIndex && !printInProgress)
            self.onReportPrint({ currentIndex: self.currentIndex + 1 });
        };

        init();
    }]);
})();