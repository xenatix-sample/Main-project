angular.module('xenatixApp')
    .controller('assessmentsGridController', ['$scope', '$stateParams', '$filter', '$q', 'assessmentsGridService', 'lookupService', 'assessmentPrintService', 'callCenterAssessmentPrintService', 'callerInformationService', 'registrationService', 'contactPhoneService', 'serviceRecordingService',
        function ($scope, $stateParams, $filter, $q, assessmentsGridService, lookupService, assessmentPrintService, callCenterAssessmentPrintService, callerInformationService, registrationService, contactPhoneService, serviceRecordingService) {
            /******* variables ********/
            var assessmentsTable = $('#assessmentsTable');

            /******* private functions ********/
            var loadGridData = function () {
                var contactID = $stateParams.ContactID;
                assessmentsGridService.getAssessmentListByContactID(contactID).then(function (assesssmentResponseList) {
                    if (hasData(assesssmentResponseList.data)) {
                        assessmentsTable.bootstrapTable('load', assesssmentResponseList.data.DataItems);
                    }
                    else {
                        assessmentsTable.bootstrapTable('removeAll');
                    }

                });
            };

            var getOrganizationText = function (type, id) {
                var organization = $filter('filter')(lookupService.getOrganizationByDataKey(type), { ID: id }, true);
                return organization.length > 0 && organization[0].Name ? organization[0].Name : '';
            }

            var initializeBootstrapTable = function () {
                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    columns: [
                        {
                            field: 'AssessmentName',
                            sortable: true,
                            title: 'Assessment'
                        },
                        {
                            field: 'Source',
                            sortable: true,
                            title: 'Source'
                        },
                        {
                            field: 'ServiceStartDate',
                            sortable: true,
                            title: 'Date and Time',
                            formatter: function (value, row, index) {
                                return (value ? $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY hh:mm A', 'useLocal') : '');
                            }

                        },
                        {
                            field: 'OrganizationID',
                            sortable: true,
                            title: 'Program Unit',
                            formatter: function (value, row, index) {
                                return (value ? getOrganizationText('ProgramUnit', value) : '');
                            }
                        },
                        {
                            field: 'ProviderID',
                            sortable: true,
                            title: 'Provider',
                            formatter: function (value, row, index) {
                                return (value ? lookupService.getText('Users', value) : '');
                            }
                        },
                          {
                              field: 'CallStatus',
                              sortable: true,
                              title: 'Status'
                          },
                          {
                              field: 'AssessmentID',
                              title: '',
                              formatter: function (value, row, index) {
                                  return ('<span class="text-nowrap">' +
                                      '<a data-default-no-action href="javascript:void(0)"  ng-click="onPrintAssessmentReport(' + row.AssessmentID + ',' + row.ResponseID + ',' + row.CallCenterHeaderID + ',' + row.OrganizationID + ',' + row.ServiceRecordingID + ',' + row.ServiceRecordingSourceID + ')"  id="areset" name="areset" security permission-key="Chart-Chart-Assessment" permission="read"  title="Print" space-key-press>' +
                                      '<i class="fa fa-print fa-fw padding-left-small padding-right-small"></i></a></span>');
                              }
                          }
                    ]
                };


            };

            var onPrintReportReceived = function (resp, callCenterHeaderID, programUnitID, assessmentID) {
                $scope.reportModel = angular.extend({}, $scope.contactInformation, resp);
                $scope.reportModel.ReportName = resp.ReportName.split(' ').join('');
                var deferred = $q.defer();
                callerInformationService.get(callCenterHeaderID).then(function (callerInformationData) {
                    if (hasData(callerInformationData)) {
                        var callerInformationDataItem = callerInformationData.DataItems[0];
                        var callerID = callerInformationDataItem.CallerContactID;
                        var clientID = callerInformationDataItem.ClientContactID;
                        $scope.reportModel.incidentID = callerInformationDataItem.CallCenterHeaderID.toString();
                        $scope.reportModel.incidentDate = callerInformationDataItem.DateOfIncident ? ($filter('toMMDDYYYYDate')(new Date(callerInformationDataItem.DateOfIncident), 'MM/DD/YYYY', 'useLocal')).toString() : '';
                        $scope.reportModel.callReason = callerInformationDataItem.ReasonCalled || '';

                        $scope.reportModel.startTime = ($filter('toMMDDYYYYDate')(callerInformationDataItem.CallStartTime, 'hh:mm A', 'useLocal')).toString();
                        $scope.reportModel.dateCrisisReceived = callerInformationDataItem.CallStartTime ? ($filter('toMMDDYYYYDate')(new Date(callerInformationDataItem.CallStartTime), 'MM/DD/YYYY', 'useLocal')).toString() : '';

                        $scope.reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', programUnitID);
                        $scope.reportModel.screener = lookupService.getText('Users', callerInformationDataItem.ProviderID);
                        $scope.reportModel.screeningDate = ($filter('toMMDDYYYYDate')(new Date(callerInformationDataItem.CallStartTime), 'MM/DD/YYYY', 'useLocal')).toString();
                        $scope.reportModel.screeningTime = ($filter('toMMDDYYYYDate')(callerInformationDataItem.CallStartTime, 'hh:mm A', 'useLocal')).toString();
                        $scope.reportModel.callDate = ($filter('toMMDDYYYYDate')(new Date(callerInformationDataItem.CallStartTime), 'MM/DD/YYYY', 'useLocal')).toString();
                        $scope.reportModel.callTime = ($filter('toMMDDYYYYDate')(callerInformationDataItem.CallStartTime, 'hh:mm A', 'useLocal')).toString();

                        var serviceRecordingSoourceID = (assessmentsGridService.getAssessmentSource(assessmentID) === "Crisis Line") ? SERVICE_RECORDING_SOURCE.CallCenter : SERVICE_RECORDING_SOURCE.LawLiaison;
                        var promises = [];
                        promises.push(serviceRecordingService.getServiceRecording(callCenterHeaderID, serviceRecordingSoourceID));
                        promises.push(registrationService.get(callerID));
                        promises.push(registrationService.get(clientID));
                        $q.all(promises).then(function (data) {
                            if (hasData(data[0])) {
                                var serviceRecording = data[0].DataItems[0];
                                $scope.reportModel.provider = lookupService.getText("Users", serviceRecording.UserID);
                                if (serviceRecording.ServiceStartDate) {
                                    $scope.reportModel.startTime = $filter('toMMDDYYYYDate')(serviceRecording.ServiceStartDate, 'hh:mm A', 'useLocal');
                                    $scope.reportModel.startDate = $filter('toMMDDYYYYDate')(serviceRecording.ServiceStartDate, 'MM/DD/YYYY', 'useLocal');
                                }
                                if (serviceRecording.ServiceEndDate) {
                                    $scope.reportModel.endTime = $filter('toMMDDYYYYDate')(serviceRecording.ServiceEndDate, 'hh:mm A', 'useLocal');
                                    $scope.reportModel.endDate = $filter('toMMDDYYYYDate')(serviceRecording.ServiceEndDate, 'MM/DD/YYYY', 'useLocal');
                                }
                            }

                            if (hasData(data[1])) {
                                var callerResponseDataItem = data[1].DataItems[0];
                                if (hasData(data[2])) {
                                    var contactInformation = data[2].DataItems[0];
                                    $scope.reportModel.mrn = contactInformation.MRN ? (contactInformation.MRN).toString() : '';
                                }
                                $scope.reportModel.callerFirstName = callerResponseDataItem.FirstName;
                                $scope.reportModel.callerLastName = callerResponseDataItem.LastName;
                                contactPhoneService.get(callerID, callerResponseDataItem.ContactTypeID).then(function (callerPhoneResponse) {
                                    if (hasData(callerPhoneResponse)) {
                                        var callerPhoneResponseDataItem = callerPhoneResponse.DataItems[0];
                                        $scope.reportModel.callerContactNumber = ($filter('toPhone')(callerPhoneResponseDataItem.Number)).toString();
                                    }
                                    $scope.reportModel.HasLoaded = true;
                                    $('#reportModal').modal('show');
                                    deferred.resolve();
                                });
                            }
                        });
                    }
                });

                return deferred.promise;
            };

            var getPrintReportData = function (assessmentID, responseID, callCenterHeaderID, programUnitId) {
                callCenterAssessmentPrintService.getPrintData($stateParams.ContactID, assessmentID, responseID).then(function (data) {
                    onPrintReportReceived(data, callCenterHeaderID, programUnitId, assessmentID);
                });
            };

            var init = function () {
                initializeBootstrapTable();
                loadGridData();
            }

            /****** Initilize  *****/
            init();

            $scope.onPrintAssessmentReport = function (assessmentID, responseID, callCenterHeaderID, programUnitId, serviceRecordingId, sourceId) {
                if (lookupService.getText('ServiceRecordingSource', sourceId) == 'ContactForms') {    //TODO: replce magic string from lookupService
                    return assessmentPrintService.initReports(assessmentID, responseID, ASSESSMENT_SECTION.ServiceCoordinationAssessmentDADS8647).then(function (data) {
                        $scope.reportModel = data;
                        $scope.reportModel.HasLoaded = true;
                        $('#reportModal').modal('show');
                    });
                } else if (!$scope.contactInformation) {
                    callCenterAssessmentPrintService.getContactReprotInformation($stateParams.ContactID).then(function (contactData) {
                        $scope.contactInformation = contactData;
                        return getPrintReportData(assessmentID, responseID, callCenterHeaderID, programUnitId);
                    });
                }
                else {
                    return getPrintReportData(assessmentID, responseID, callCenterHeaderID, programUnitId);
                }
            };
        }]);

