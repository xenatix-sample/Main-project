angular.module('xenatixApp')
    .controller('consentsController', [
        '$http', '$scope', '$rootScope', '$filter', 'consentsService', 'alertService', 'assessmentService', '$stateParams', '$state', 'consentsPrintService', 'contactBenefitService', 'registrationService', 'lookupService', 'roleSecurityService','WorkflowHeaderService',
        function ($http, $scope, $rootScope, $filter, consentsService, alertService, assessmentService, $stateParams, $state, printService, contactBenefitService, registrationService, lookupService, roleSecurityService, WorkflowHeaderService) {

            $scope.consentsList = [];
            var hasEditPermission = roleSecurityService.hasPermission("Consents-Assessment-Agency", PERMISSION.UPDATE);
            var init = function () {
                $scope.consentsTable = $('#consentsTable');
                initializeBootstrapTable();
                $scope.permissionKey = $state.current.data.permissionKey;
                registrationService.get($stateParams.ContactID).then(function (data) {
                    if (data && data.DataItems && data.DataItems.length > 0) {
                        $scope.registrationData = data.DataItems[0];
                        $scope.contactName = $scope.registrationData.FirstName + ' ' + $scope.registrationData.LastName;
                    };
                });

                assessmentService.getAssessment().then(function (data) {
                    if (hasData(data)) {
                        $scope.consentsList = $filter('filter')(data.DataItems, { DocumentTypeID: 3 }, true);
                    }
                });

                get();
                $scope.initialChildState = '.user.details';
                $scope.$parent['autoFocus'] = true;
            };

            $('#consentsTable').on('all.bs.table', function (e, name, args) {
                $('.fixed-table-body').scrollLeft(0);
            });

            $scope.addNew = function () {
                $scope.consentID = "";
            }

            var get = function () {
                consentsService.get($stateParams.ContactID).then(function (data) {
                    if (data.ResultCode === 0) {
                        if (hasData(data)) {
                            $scope.consentsTable.bootstrapTable('load', data.DataItems);
                        } else {
                            $scope.consentsTable.bootstrapTable('removeAll');
                        }
                    } else {
                        alertService.error('Error while loading consents');
                    }
                });
            };

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
                    },
                    columns: [
                        {
                            field: 'ConsentName',
                            title: 'Consent Name'
                        },
                        {
                            field: 'SignatureStatusID',
                            title: 'Status',
                            formatter: function (value, row, index) {
                                return (row.ExpirationDate && isExpireDate(row.ExpirationDate)) ? 'Expired' : (value == SIGNATURE_STATUS.Signed) ? 'Signed' : 'Not Signed';
                            }
                        },
                        {
                            field: 'EffectiveDate',
                            title: 'Effective Date',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                } else {
                                    return "";
                                }
                            }
                        },
                        {
                            field: 'ExpirationDate',
                            title: 'Expiration Date',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                } else {
                                    return "";
                                }
                            }
                        },
                        {
                            field: 'ExpirationReasonID',
                            title: 'Expiration Reason',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return lookupService.getText("AssessmentExpirationReason", value);
                                }
                            }
                        },
                        {
                            field: 'ContactConsentID',
                            title: 'Actions',
                            formatter: function (value, row, index) {
                                return '<span class="text-nowrap">' +
                                    ((row.ExpirationDate || ((row.AssessmentID == ASSESSMENT_TYPE.EHRPhotoID || row.AssessmentID == ASSESSMENT_TYPE.ProtectedHealthInformationAmendment || row.AssessmentID == ASSESSMENT_TYPE.GeneralRelease) && row.SignatureStatusID == SIGNATURE_STATUS.Signed)) ? '' :
                                    '<a  data-default-no-action href="javascript:void(0)"  ng-click="expireConsent(' + row.AssessmentID + ',false, \'' + row.SignatureStatus + '\', ' + value + ',' + row.ResponseID + ')" id="expire" name="expire" security permission-key="Consents-Assessment-Agency" permission="update" title="Expire" space-key-press><i class="fa fa-ban fa-fw padding-left-small padding-right-small"></i></a>') +
                                    ((row.SignatureStatusID == SIGNATURE_STATUS.NotSigned && (row.ExpirationDate ? !isExpireDate(row.ExpirationDate) : true) && hasEditPermission) ?
                                    '<a data-default-action href="javascript:void(0)" ng-click="addEditConsent(' + row.AssessmentID + ',' + row.ResponseID + ', false, \'' + row.SignatureStatus + '\', ' + value + ', \'' + row.ExpirationDate + '\')"  id="edit" name="edit"  title="Edit" security permission-key="Consents-Assessment-Agency" permission="update" space-key-press><i class="fa fa-pencil fa-fw padding-left-small padding-right-small"></i></a>' :
                                    '<a  data-default-action href="javascript:void(0)"  ng-click="addEditConsent(' + row.AssessmentID + ',' + row.ResponseID + ', false, \'' + row.SignatureStatus + '\', ' + value + ', \'' + row.ExpirationDate + '\')" id="view" name="view" title="View" security permission-key="Consents-Assessment-Agency" permission="read" space-key-press><i class="fa fa-eye fa-fw padding-left-small padding-right-small"></i></a>') +
                                   '<a data-default-no-action href="javascript:void(0)" ng-click="onPrintReport(' + row.AssessmentID + ',' + row.AssessmentSectionID + ',' + row.ResponseID + ',' + value + ')"  id="areset" name="areset" security permission-key="Consents-Assessment-Agency" permission="read" title="Print" space-key-press><i class="fa fa-print fa-fw padding-left-small padding-right-small"></i></a>' +
                                    '</span>';
                            }
                        }
                    ]
                };
            };

            $scope.onPrintReport = function (assessmentID, assessmentSectionID, responseID, ContactConsentID) {
                $scope.workflowDataKey = GetContactConsentsWorkflowDataKey(assessmentID);
                $scope.workflowHeaderID = ContactConsentID;
                printService.initReports(assessmentID, assessmentSectionID, responseID ).then(onPrintReportReceived.bind(this));
                //printService.initReports(assessmentID, assessmentSectionID, responseID, GetContactConsentsWorkflowDataKey(assessmentID), ContactConsentID).then(onPrintReportReceived.bind(this));
            };

            function onPrintReportReceived(data) {
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

                WorkflowHeaderService.GetWorkflowHeader($scope.workflowDataKey, $scope.workflowHeaderID).then(function (data) {
                    if (data) {
                        headerDetails = data.DataItems[0];
                        $scope.reportModel.mrn = headerDetails.MRN;
                        var suffix = lookupService.getText("Suffix", headerDetails.SuffixID);
                        $scope.reportModel.clientName = headerDetails.FirstName + (headerDetails.Middle ? ' ' + headerDetails.Middle : '') + ' ' + headerDetails.LastName + (suffix ? ' ' + suffix : '');;
                        if (headerDetails.DOB)
                            $scope.reportModel.dob = ($filter('formatDate')(headerDetails.DOB, 'MM/DD/YYYY')).toString();
                        $scope.reportModel.medicaidNumber = headerDetails.MedicaidID || 'N/A';
                    }
                });

                //if ($scope.registrationData.MRN)
                //    $scope.reportModel.mrn = $scope.registrationData.MRN;
                //$scope.reportModel.clientName = $scope.contactName;
                //if ($scope.registrationData.DOB)
                //    $scope.reportModel.dob = $filter('formatDate')($scope.registrationData.DOB, 'MM/DD/YYYY');
                //$scope.reportModel.medicaidNumber = 'N/A';


                contactBenefitService.get($stateParams.ContactID).then(function (data) {
                    if (data && data.DataItems.length > 0) {
                        var payors = $filter('filter')(data.DataItems, function (itm) {
                            return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                        })
                        if (payors && payors.length > 0) {
                            //$scope.reportModel.medicaidNumber = payors[0].PolicyID;
                        }
                    }
                    $scope.reportModel.HasLoaded = true;
                    $('#reportModal').modal('show');
                });
            }

            $scope.nextSection = function () {
                var assessmentID = $scope.consentID; // consentID is AssessmentID
                var contactConsentID = $stateParams.ContactConsentID;
                $scope.addEditConsent(assessmentID, 0, true);
            }

            $scope.addEditConsent = function (assessmentID, responseID, isAddMode, signatureStatus, contactConsentID, expirationDate) {
                assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                    if (data && data.DataItems && data.DataItems.length > 0) {
                        var sectionID = data.DataItems[0].AssessmentSectionID;
                        var respID = isAddMode ? 0 : responseID;
                        gotoNext(assessmentID, sectionID, respID, signatureStatus, contactConsentID, expirationDate);
                    }
                });
            }

            $scope.expireConsent = function (assessmentID, isAddMode, signatureStatus, contactConsentID, responseID) {
                assessmentService.getAssessmentSections(ASSESSMENT_TYPE.ConsentExpiration).then(function (data) {
                    if (data && data.DataItems && data.DataItems.length > 0) {
                        var sectionID = data.DataItems[0].AssessmentSectionID;
                        angular.extend($stateParams, {
                            AssessmentID: ASSESSMENT_TYPE.ConsentExpiration,
                            SectionID: sectionID,
                            ResponseID: responseID,
                            ExpireAssessmentID: assessmentID,
                            ReadOnly: 'edit',
                            ContactConsentID: contactConsentID ? contactConsentID : 0
                        });
                        $scope.AssessmentID = ASSESSMENT_TYPE.ConsentExpiration;
                        $state.transitionTo('consentexpire', $stateParams);
                    }
                }); 
            };

            var gotoNext = function (assessmentID, sectionID, responseID, signatureStatus, contactConsentID, expirationDate) {
                angular.extend($stateParams, {
                    ContactConsentID: contactConsentID ? contactConsentID : 0,
                    AssessmentID: assessmentID,
                    SectionID: sectionID,
                    ResponseID: responseID,
                    ReadOnly: (signatureStatus == 'Signed' || (expirationDate && isExpireDate(expirationDate))) ? "view" : 'edit'
                });
                
                $state.transitionTo('agencyViewSection', $stateParams);
            }

            init();
        }
    ]);
