(function () {
    angular.module('xenatixApp')
        .controller('benefitsAssistanceProgressNoteController', ['$q', '$scope', '$state', '$stateParams', '$timeout', '$filter', 'assessmentService', 'benefitsAssistanceProgressNoteService', 'lookupService', 'alertService', 'navigationService', '$rootScope', 'serviceRecordingService', 'orderByFilter', 'cacheService', 'formService', 'WorkflowHeaderService',
    function ($q, $scope, $state, $stateParams, $timeout, $filter, assessmentService, benefitsAssistanceProgressNoteService, lookupService, alertService, navigationService, $rootScope, serviceRecordingService, orderBy, cacheService, formService, WorkflowHeaderService) {
        var benefitsAssistanceProgressNoteTable = $("#benefitsAssistanceProgressNoteTable");
        var ContactID = $stateParams.ContactID;
        var AssessmentID = ASSESSMENT_TYPE.BenefitAssessmentsProgressNote;
        var signatureStatusType = lookupService.getLookupsByType('SignatureStatus');
        $scope.permissionKey = $state.current.data.permissionKey;
        $scope.$parent['autoFocus'] = true;
        var adjustedTime = "00:00:01";
        var maxSignValidationDays = 7;
        var userId;
        var flyoutElement = $('#patientProfileFlyout');
        var initializeBootstrapTable = function () {
            $scope.tableoptions = {
                pagination: true,
                pageSize: 10,
                pageList: [10, 25, 50, 100],
                search: false,
                showColumns: true,
                data: [],
                undefinedText: '',
                onClickRow: function (e, row, $element) {
                    //self.prepRowEditData(e);
                },
                columns: [
                    {
                        field: "DocumentStatusID",
                        title: "Status",
                        formatter: function (value, row, index) {
                            if (value === DOCUMENT_STATUS.Draft)
                                return "Draft";
                            else if (value === DOCUMENT_STATUS.Completed)
                                return "Completed"
                            else
                                return "Void";
                        }
                    },
                    {
                        field: "ServiceStartDate",
                        title: "Date",
                        formatter: function (value, row, index) {
                            return value ? $filter('formatDate')(value) : '';
                        }
                    },
                    {
                        field: "ServiceStartDate",
                        title: "Time",
                        formatter: function (value, row, index) {
                            return (!value ? '' : (searchString(value, adjustedTime) ? '' : $filter('formatDate')(value, 'hh:mm A')))
                        }
                    },
                    {
                        field: "Duration",
                        title: "Duration",
                        formatter: function (value, row, index) {
                            //Calculate suration from row.StartTime and row.EndTime
                            return calculateDuration(row['ServiceStartDate'], row['ServiceEndDate']);
                        }
                    },
                    {
                        field: "ServiceItemID",
                        title: "Service Item",
                        formatter: function (value, row, index) {
                            return lookupService.getText('ServiceItem', value);
                        }
                    },
                    {

                        field: "TrackingFieldID",
                        title: "Tracking Field",
                        formatter: function (value, row, index) {
                            if (row["ServiceItemID"] === SERVICE_ITEM.Benefits_Assistance) {
                                return lookupService.getText('TrackingField', value);
                            }
                            else {
                                return "";
                            }
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
                        field: "ServiceRecordingID",
                        title: "Actions",
                        formatter: function (value, row, index) {
                            if (row) {

                                var isDraft = (row["DocumentStatusID"] == DOCUMENT_STATUS.Draft);
                                var isComplete = (row["DocumentStatusID"] == DOCUMENT_STATUS.Completed);
                                var isVoided = row["IsVoided"];
                                return (isDraft ? '<a href="javascript:void(0)" security permission-key="Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote" permission="update" data-default-action id="editEmail" ng-click="navigateToBAPN(' + row["ResponseID"] + ',' + row["BenefitsAssistanceID"] + ', \'edit\', ' + row["DocumentStatusID"] + ',' + row["IsVoided"] + ')" name="editbapn" data-toggle="modal" title="View/Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' :
                                          '<a href="javascript:void(0)" security permission-key="Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote" permission="read" data-default-action id="viewBapn"  ng-click="navigateToBAPN(' + row["ResponseID"] + ',' + row["BenefitsAssistanceID"] + ', \'view\', ' + row["DocumentStatusID"] + ',' + row["IsVoided"] + ')" name="viewBapn" title="View" space-key-press><i class="fa fa-' + (isComplete ? 'pencil' : 'eye') + ' fa-fw"></i></a>') +
                              ((!isVoided && row["DocumentStatusID"] === DOCUMENT_STATUS.Completed) ? createLinkForVoidService(row) : '') +
                                    (isDraft ? '<a href="javascript:void(0)" data-default-no-action security permission-key="Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote" permission="delete" ng-click="deleteBAPN(' + row["BenefitsAssistanceID"] + ')" id="deleteBapn" name="deleteBapn" title="Delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>' : '') +
                                    '<a href="javascript:void(0)" data-default-no-action security permission-key="Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote" permission="read" id="printBapn" name="printBapn" ng-click="printBapn(' + row["ResponseID"] + ',' + row["BenefitsAssistanceID"] + ',' + value + ')" title="Print" space-key-press><i class="fa fa-print fa-fw"></i></a>'
                            }
                            else {
                                return '';
                            }
                        }
                    }]
            };
        };

        var init = function () {
            get();
            initializeBootstrapTable();
            $scope.contactID = $stateParams.ContactID;
            navigationService.get().then(function (response) {
                if (response && response.DataItems && response.DataItems.length > 0) {
                    $scope.contactName = response.DataItems[0].UserFullName;
                    $scope.UserID = response.DataItems[0].UserID;
                }
            });
        };

        var get = function () {
            var dfd = $q.defer();
            benefitsAssistanceProgressNoteService.getByContactID($scope.contactID).then(function (data) {
                if (hasData(data)) {
                    var gridData = orderBy(data.DataItems, ['DocumentStatusID', '-ServiceStartDate']);
                    benefitsAssistanceProgressNoteTable.bootstrapTable('load', gridData);
                    dfd.resolve(data);
                }
                else {
                    benefitsAssistanceProgressNoteTable.bootstrapTable('removeAll');
                    dfd.resolve(data);
                }
            });
            return dfd.promise;
        };

        $scope.printBapn = function (responseID, benefitsAssistanceID, serviceRecordingID) {
            responseID = responseID ? responseID : ($stateParams.ResponseID ? $stateParams.ResponseID : 0);
            benefitsAssistanceID = benefitsAssistanceID ? benefitsAssistanceID : $stateParams.BenefitsAssistanceID;
            serviceRecordingService.getServiceRecording(benefitsAssistanceID, SERVICE_RECORDING_SOURCE.BAPN).then(function (data) {
                var serviceData, serviceDate, displaySignature;
                if (hasData(data)) {
                    serviceData = data.DataItems[0];
                    serviceDate = $filter('toMMDDYYYYDate')(serviceData.ServiceStartDate);
                    data.ServiceRecordingID = serviceData.ServiceRecordingID;
                    displaySignature = true;
                }

                benefitsAssistanceProgressNoteService.initReport(AssessmentID, responseID, undefined, $stateParams.ContactID, serviceDate, displaySignature, serviceData.ServiceRecordingID, $state.current.data.workflowDataKey, benefitsAssistanceID).then(function (reportModel) {
                  
                    reportModel.HasLoaded = true;
                    $scope.reportModel = reportModel;
                    $('#reportModalBAPN').modal('show');
                });
            });
        }

        $scope.deleteBAPN = function (id) {
            bootbox.confirm("Are you sure you want to delete this item?", function (result) {
                if (result === true) {
                    benefitsAssistanceProgressNoteService.remove(ContactID, id).then(function (data) {
                        if (data.ResultCode == 0) {
                            alertService.success('Benefits Assistance Progress Note deleted successfully.');
                            get();
                        }
                    });
                }
            });
        };

        $scope.initBAPN = function () {
            angular.extend($stateParams, {
                ContactID: ContactID,
                AssessmentID: AssessmentID,
                ResponseID: 0,
                BenefitsAssistanceID: 0,
                ReadOnly: 'edit',
                DocumentStatusID: 0,
                SectionID: 0

            });
            $scope.Goto('initializeBapnService', $stateParams);
        };

        $scope.navigateToBAPN = function (responseId, benefitsAssistanceID, readOnly, documentStatusID, isVoided) {
            // Create params object
            var params = {
                ContactID: ContactID,
                AssessmentID: AssessmentID,
                ResponseID: responseId ? responseId : 0,
                BenefitsAssistanceID: benefitsAssistanceID,
                ReadOnly: readOnly ? readOnly : 'edit',
                DocumentStatusID: documentStatusID ? documentStatusID : 0
            };
            if (isVoided)
                cacheService.add('IsVoidedRecord', true);
            else
                cacheService.add('IsVoidedRecord', false);
            // Use the assessment service to navigate to correct section
            assessmentService.navigateToSection('bapnService', params);
        };

        var objVoidModel = function (serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID) {
            return {
                ServiceRecordingID: serviceRecordingID,
                ID: benefitsAssistanceID,
                AssessmentResponseID: responseID,
                NoteHeaderID: noteHeaderID,
                AssessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                PermissionVoidKey: $state.current.data.permissionVoidKey
            };
        };

        navigationService.get().then(function (data) {
            if (data && data.DataItems && data.DataItems.length > 0) {
                var user = data.DataItems[0];
                userId = user.UserID;
            }
        });

        $scope.$on('voidServiceReloadGrid', function (event, args) {
            benefitsAssistanceProgressNoteService.get($scope.contactID, args.ID).then(function (benefitsAssistanceProgressNote) {
                if (hasData(benefitsAssistanceProgressNote)) {
                    var benefitsAssistanceProgressNoteData = benefitsAssistanceProgressNote.DataItems[0];
                    benefitsAssistanceProgressNoteData.DocumentStatusID = DOCUMENT_STATUS.Void;
                    benefitsAssistanceProgressNoteService.update(benefitsAssistanceProgressNoteData).then(function () {
                        if (args.isCreateCopy) {
                            benefitsAssistanceProgressNoteData.BenefitsAssistanceID = null;
                            benefitsAssistanceProgressNoteData.DocumentStatusID = DOCUMENT_STATUS.Draft;
                            benefitsAssistanceProgressNoteData.ModifiedOn = new Date();
                            benefitsAssistanceProgressNoteData.ResponseID = args.AssessmentResponseID;
                            benefitsAssistanceProgressNoteService.add(benefitsAssistanceProgressNoteData).then(function (bapnResponseData) {
                                serviceRecordingService.getServiceRecording(args.ID, SERVICE_RECORDING_SOURCE.BAPN).then(function (serviceRecordingData) {
                                    if (hasData(serviceRecordingData)) {
                                        serviceRecordingData = serviceRecordingData.DataItems[0];
                                        serviceRecordingData.ServiceRecordingHeaderID = null;
                                        serviceRecordingData.ParentServiceRecordingID = args.ServiceRecordingID;
                                        serviceRecordingData.ServiceRecordingID = null;
                                        //serviceRecordingData.UserID = userId;
                                        serviceRecordingData.SourceHeaderID = bapnResponseData.data.ID;
                                        angular.forEach(serviceRecordingData.AdditionalUserList, function (item) {
                                            item.ServiceRecordingAdditionalUserID = 0;
                                        });
                                        angular.forEach(serviceRecordingData.AttendedList, function (item) {
                                            item.ServiceRecordingAttendeeID = 0;
                                        });
                                        serviceRecordingService.addServiceRecording(serviceRecordingData).then(function () {
                                            alertService.success("Service has been void successfully.");
                                            get();
                                        });
                                    }
                                });
                            });
                        }
                        else {
                            alertService.success("Service has been void successfully.");
                            get();
                        }
                    });
                }
            });

        });


        $scope.voidFlyout = function (serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID) {
            if (!flyoutElement.hasClass('active'))
                openVoidFlyout(serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID);
        };
        var openVoidFlyout = function (serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID) {
            $scope.voidModel = objVoidModel(serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID);
            $scope.$parent.getVoidModel($scope.voidModel);
            $rootScope.isVoidedFlyout = true; // we need to set this flag every time we try to open void flyout.
            flyoutElement.addClass('active');
            formService.reset();
            $rootScope.defaultFormName = 'vs.voidServiceForm';
        };
        //Create link for Void service
        createLinkForVoidService = function (row) {
            var isSevenDaysOld = $filter('getDaysDifference')(row.ServiceEndDate, moment.utc()) <= maxSignValidationDays;
            if (($scope.UserID === row.UserID) && isSevenDaysOld) {
                return '<a data-default-no-action href="javascript:void(0)" alt="Void Service" ng-click="voidFlyout(' + row.ServiceRecordingID + ',' + row.NoteHeaderID + ',' + row.BenefitsAssistanceID + ',' + row.ResponseID + ')"' +
                                             'data-module="callcenter" data-feature="crisisline" data-action="append" space-key-press>' +
                                                 '<i  title="Void Service" class="fa fa-ban fa-fw" /></a>';
            }
            else {
                return '<a data-default-no-action href="javascript:void(0)" security permission-key="Benefits-BenefitsAssistanceProgressNote-Void" permission="create" alt="Void Service" ng-click="voidFlyout(' + row.ServiceRecordingID + ',' + row.NoteHeaderID + ',' + row.BenefitsAssistanceID + ',' + row.ResponseID + ')"' +
                              'data-module="callcenter" data-feature="crisisline" data-action="append" space-key-press>' +
                                  '<i  title="Void Service" class="fa fa-ban fa-fw" /></a>';
            }
        };

        init();

    }]);
}());
