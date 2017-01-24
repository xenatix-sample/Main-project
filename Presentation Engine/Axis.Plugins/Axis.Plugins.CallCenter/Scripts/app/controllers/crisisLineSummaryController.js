(function () {
    angular.module('xenatixApp')
    .controller('crisisLineSummaryController', ['$filter', 'alertService', '$stateParams', '$state', 'formService', 'crisisLineSummaryService', '$scope', 'lookupService', 'roleSecurityService', 'navigationService', 'voidService', 'callerInformationService', '$q', '$compile', "cacheService", '$timeout', '$modal', '$rootScope', 'crisisLineFollowUpService',
        function ($filter, alertService, $stateParams, $state, formService, crisisLineSummaryService, $scope, lookupService, roleSecurityService, navigationService, voidService, callerInformationService, $q, $compile, cacheService, $timeout, $modal, $rootScope, crisisLineFollowUpService) {
            var enableApprovalsCheckbox = false;
            var self = this;
            var defaultVoidReasonId = 4;
            var maxSignValidationDays = 7;
            var followUpFilteringId = 7;
            var flyoutElement = $('.row-offcanvas');
            self.btnCaption = 'Approvals';
            var CONTACT_FLYOUT = 'CONTACT';
            var SERVICEVOID_FLYOUT = 'SERVICEVOID';
            self.searchTypeFilter = "1";//default value
            self.isNormalView = true;
            self.searchText = "";
            var user = null;
            var providerDetails = null;
            var SERVICE_SIGN = {
                Signed: "Signed",
                NotSigned: "Not Signed"
            }
            var satusData = lookupService.getLookupsByType('CallStatus');
            self.init = function () {
                $scope.isApprover = roleSecurityService.hasPermission('CrisisLine-CrisisLine-Approver', PERMISSION.CREATE);
                self.followupHistoryTable = $("#followupHistoryTable");
                self.CallCenterSummaryTable = $("#callCenterSummaryTable");
                self.initializeBootstrapTable();
                self.CallCenterSummaryTable.on('all.bs.table', function (e, name, args) {
                    $('.fixed-table-body').scrollLeft(0);
                });
                if (self.CallCenterSummaryTable) {
                    (self.CallCenterSummaryTable).on('post-body.bs.table', function (e, name, args) {
                        if ((angular.element("#callCenterSummaryTable tbody input:checkbox:enabled").length == 0))
                            angular.element("#callCenterSummaryTable thead input:checkbox").attr('disabled', true);
                        else
                            angular.element("#callCenterSummaryTable thead input:checkbox").attr('disabled', !enableApprovalsCheckbox);
                    });
                };

                navigationService.get().then(function (userData) {
                    if (hasData(userData)) {
                        user = userData.DataItems[0];
                        providerDetails = $filter('filter')(lookupService.getLookupsByType('Users'), { ID: user.UserID }, true)[0]
                        self.searchCallCenter(' ');
                    }
                });

                $scope.$parent['autoFocus'] = true;
                $scope.$parent['voidFocus'] = false;
                $scope.$evalAsync(function () {
                    applyDropupOnGrid(true);
                });
                offCanvasNav.init();
                $scope.dataEntryErrorID = 2;
                $scope.displayFlyout = CONTACT_FLYOUT;
            };
            var permission_key = "CrisisLine-CrisisLine-CrisisLine";
            var hasEditPermission = roleSecurityService.hasPermission(permission_key, PERMISSION.UPDATE);

            self.confirm = function () {
                var tabledata = self.CallCenterSummaryTable.bootstrapTable('getData');
                var dataToUpdate = $filter('filter')(tabledata, function (item) { return item.selected === true });
                if (dataToUpdate.length) {
                    bootbox.confirm({
                        size: 'small',
                        message: "Are you sure you would like to proceed with approving the selected call log(s)?",
                        callback: function (result) {
                            if (result) {
                                approveRecords(dataToUpdate);
                            }

                        }
                    })
                }
                else {
                    alertService.error("Please select at least one record to approve.");
                }
            };

            self.statusFilter = function () {
                //[Bug:12185]. do not apply any filters . let user see all call status.
                //if (!self.isNormalView)
                //    return $filter('filter')(satusData, function (item) { return item.ID != CALL_STATUS.COMPLETE }, true);
                //else
                return satusData;
            }
            self.reviewSelected = function () {
                var dataToReview = [];
                var tabledata = self.CallCenterSummaryTable.bootstrapTable('getData');
                $filter('filter')(tabledata, function (item) {
                    if (item.selected === true) {
                        return dataToReview.push({
                            CallCenterHeaderID: item.CallCenterHeaderID, ContactID: item.ContactID
                        });
                    }
                });
                if (dataToReview.length) {
                    var reviewState = 'callcenter.crisisline.callerinformation';
                    var parentState = 'callcenter.crisisline';
                    cacheService.add('reviewFollowup', true);
                    var state = $state.get(parentState)
                    state.data.approvalData = dataToReview;
                    $state.go(reviewState, dataToReview[0]);
                } else {
                    alertService.error("Please select atleast one record to review.");
                }

            }
            var approveRecords = function (dataToUpdate) {
                var approvalPromises = [];

                //get all selected module
                $filter('filter')(dataToUpdate, function (item) {
                    item.CallStatusID = CALL_STATUS.COMPLETE;
                    approvalPromises.push(updateCallStatus(item.CallCenterHeaderID));
                });

                $q.all(approvalPromises).then(function () {
                    //Update local data as well
                    var tabledata = [];
                    var gridData = self.CallCenterSummaryTable.bootstrapTable('getData');
                    angular.extend(self.CallCenterSummary, gridData);
                    tabledata = $filter('filter')(gridData, function (item) { return item.CallStatusID != CALL_STATUS.COMPLETE }, true);
                    self.CallCenterSummaryTable.bootstrapTable('removeAll');
                    self.CallCenterSummaryTable.bootstrapTable('load', tabledata);
                    alertService.success('Selected records approved successfully.');
                    self.searchCallCenter($('#txtCallCenterSummary').val());

                });

            }

            var updateCallStatus = function (callCenterHeaderID) {
                var dfd = $q.defer();
                callerInformationService.get(callCenterHeaderID).then(function (data) {
                    if (hasData(data)) {
                        var model = data.DataItems[0];
                        model.CallStatusID = CALL_STATUS.COMPLETE;
                        callerInformationService.update(model).then(function (updatedData) {
                            dfd.resolve(updatedData);
                        }, function (data) {
                            dfd.reject(status);
                        })
                    }
                }, function () {
                    dfd.reject(status);
                });
                return dfd.promise;
            };

            var objVoidModel = function (serviceRecordingID, noteHeaderID, ID, responseID, contactID) {

                return {
                    ServiceRecordingVoidID: 0,
                    ServiceRecordingVoidReasonID: null,
                    IsCreateCopyToEdit: false,
                    IncorrectOrganization: false,
                    IncorrectServiceType: false,
                    IncorrectServiceItem: false,
                    IncorrectServiceStatus: false,
                    IncorrectSupervisor: false,
                    IncorrectAdditionalUser: false,
                    IncorrectAttendanceStatus: false,
                    IncorrectStartDate: false,
                    IncorrectStartTime: false,
                    IncorrectEndDate: false,
                    IncorrectEndTime: false,
                    IncorrectDeliveryMethod: false,
                    IncorrectServiceLocation: false,
                    IncorrectRecipientCode: false,
                    IncorrectTrackingField: false,
                    Comments: "",
                    ServiceRecordingID: serviceRecordingID,
                    ID: ID,
                    ContactID: contactID,
                    AssessmentResponseID: responseID,
                    NoteHeaderID: ID,
                    IsCallCenterVoided: true,
                    permissionKey: $state.current.data.permissionKey
                };
            };

            self.followupTableLoad = function (callCenterHeaderID) {
                crisisLineSummaryService.getFollowUpSummary(callCenterHeaderID).then(function (data) {
                    if (data && data.DataItems) {
                        self.followupHistoryTable.bootstrapTable('destroy');
                        self.followupHistoryTable.bootstrapTable(self.followUpTableOptions);
                        self.followUpHistory = data.DataItems;
                        self.followupHistoryTable.bootstrapTable('load', angular.copy(self.followUpHistory));

                    } else {
                        self.followUpHistory = [];
                        self.followupHistoryTable.bootstrapTable('removeAll');
                    }
                });
            }

            self.searchCallCenter = function (searchStr) {
                //hit get method of services, consolidate the data and display in the grid
                if (flyoutElement.hasClass('active'))
                    flyoutElement.removeClass('active');
                searchStr = searchStr == undefined ? "" : searchStr;
                if (!$scope.ProviderSubmittedBy)
                    $scope.ProviderSubmittedBy = providerDetails;

                crisisLineSummaryService.get(searchStr, user.UserID, Number(self.searchTypeFilter), $scope.CallStatusID, (searchStr.trim() != "") ? 0 : $scope.ProviderSubmittedBy.ID, !self.isNormalView).then(function (crisisData) {
                    if (crisisData && crisisData.DataItems) {
                        self.CallCenterSummary = crisisData.DataItems;
                        self.CallCenterSummaryTable.bootstrapTable('load', self.CallCenterSummary);
                    } else {
                        self.CallCenterSummary = [];
                        self.CallCenterSummaryTable.bootstrapTable('removeAll');
                    }
                });
            };

            //Filter the Grid Data
            self.filterData = function () {
                enableApprovalsCheckbox = false;
                if (!$scope.ProviderSubmittedBy)
                    $scope.ProviderSubmittedBy = { ID: 0 };
                self.searchCallCenter($('#txtCallCenterSummary').val());
            };

            //Whether to be included the data in Search filter
            var isFollowup = function (callCenterSummaryItem, callStatusID, isFollowupRequired) {
                return callCenterSummaryItem.CallStatusID != CALL_STATUS.COMPLETE && (callCenterSummaryItem.FollowUpRequired === isFollowupRequired
                        || callCenterSummaryItem.CallStatus === lookupService.getText("CallStatus", callStatusID));
            }

            self.changeView = function (view) {
                self.isNormalView = view;
                if (view) {
                    self.btnCaption = 'Approvals';
                    self.tableoptions.columns = tableOptionsNormal.columns;
                }
                else {
                    self.btnCaption = 'Summary';
                    self.tableoptions.columns = tableOptionsApprovals.columns;
                }
                self.CallCenterSummaryTable.bootstrapTable('destroy');
                self.CallCenterSummaryTable.bootstrapTable(self.tableoptions);
                $scope.CallStatusID = null;
                $scope.ProviderSubmittedBy = null;
                self.searchCallCenter($('#txtCallCenterSummary').val());
            };

            self.followUp = function (ContactID, CallerHeaderID) {
                crisisLineFollowUpService.followUp(CallerHeaderID).then(function (newCallCenterHeaderID) {
                    cacheService.add('FollowUp', true);
                    angular.extend($stateParams, {
                        ContactID: ContactID,
                        CallCenterHeaderID: newCallCenterHeaderID,
                        ReadOnly: "edit"
                    });
                    $state.transitionTo('callcenter.crisisline.callerinformation', $stateParams);
                });
                if (roleSecurityService.hasPermission('CrisisLine-CrisisLine-Approver', PERMISSION.UPDATE))
                    cacheService.add('IsManagerAccess', true);
                else
                    cacheService.add('IsManagerAccess', false);
                cacheService.add('IsCreatorAccess', true);
            }

            $scope.cancelModal = function () {
                $('#followupHistoryModal').modal('hide');
            };



            self.remove = function (CallerHeaderId, isIncident) {
                bootbox.hideAll();
                bootbox.confirm("Selected Call Center will be deactivated.\n Do you want to continue?", function (result) {
                    if (result) {
                        crisisLineSummaryService.remove(CallerHeaderId).then(function (data) {
                            if (data && data.ResultCode >= 0) {
                                alertService.success("Call Center has been deleted successfully.");
                                self.searchCallCenter($('#txtCallCenterSummary').val());
                                if (isIncident) {
                                    self.followupTableLoad(CallerHeaderId);
                                }
                            } else {
                                alertService.error("OOPS! Something went wrong");
                            }
                        }, function (errorStatus) {
                            alertService.error("OOPS! Something went wrong");
                        });
                    }
                });
            };

            $scope.voidService = function () {
                var voidModel = $scope.voidModel;
                if (voidModel.ServiceRecordingVoidReasonID === $scope.dataEntryErrorID) {
                    var isAnyCheckBoxChecked = false;
                    for (var prop in voidModel) {
                        if (prop.indexOf('Incorrect') >= 0 && voidModel[prop] === true) {
                            isAnyCheckBoxChecked = true; break;
                        }
                    }
                    if (!isAnyCheckBoxChecked) {
                        alertService.error('Please select at least one error reason');
                        return;
                    }
                }
                if (voidModel.ServiceRecordingID && voidModel.ServiceRecordingID != 0 && voidModel.ServiceRecordingVoidReasonID > 0) {
                    voidService.voidRecordedService(voidModel).then(function (resp) {
                        if (resp && resp.ResultCode >= 0) {
                            self.searchCallCenter($('#txtCallCenterSummary').val());
                            alertService.success("Service has been void successfully.");
                        } else {
                            alertService.error("OOPS! Something went wrong");
                        }
                    }, function (error) {
                        alertService.error(resp);
                    });
                }
            }

            var followUpHistoryCoulmns = {
                columns: [

               {
                   field: "MRN",
                   title: "MRN",
                   sortable: true
               },
               {
                   field: "CallCenterHeaderID",
                   title: "Incident ID",
                   sortable: true
               },
               {
                   field: "CallDate",
                   title: "Call Date",
                   sortable: true,
                   formatter: function (value, row) {
                       if (value) {
                           return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                       } else
                           return '';
                   }
               },
               {
                   field: "Caller",
                   title: "Caller Name",
                   sortable: true
               },
              {
                  field: "ClientName",
                  title: "Client Name",
                  sortable: true,
                  formatter: function (value, row) {
                      return row.ClientFirstName + ' ' + row.ClientLastName;
                  }

              },
                 {
                     field: "CallCenterHeaderID",
                     title: "",
                     formatter: function (value, row, index) {
                         row.ClientTypeID = row.ClientTypeID == null ? 0 : row.ClientTypeID;
                         return (
                                 '<span class="text-nowrap pull-right">' +

                                (hasEditPermission ? '<a data-default-action class="padding-left-small padding-right-small"  data-dismiss="modal" ng-click="ctrl.followUpNavigate(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.IsManagerAccess + ',' + row.IsCreatorAccess + ' )" alt="update" security permission-key="CrisisLine-CrisisLine-CrisisLine" permission="update" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                                      '<i title="' + ((row.IsManagerAccess || row.IsCreatorAccess || $scope.isApprover) ? 'edit' : 'view') + '" class="fa ' + ((row.IsManagerAccess || row.IsCreatorAccess || $scope.isApprover) ? 'fa-pencil' : 'fa-eye') + ' fa-fw" /></a>' :
                                     '<a data-default-no-action class="padding-left-small padding-right-small" data-dismiss="modal" ng-click="ctrl.followUpNavigate(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.IsManagerAccess + ',' + row.IsCreatorAccess + ' )" alt="view" security permission-key="CrisisLine-CrisisLine-CrisisLine" permission="read" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                                 '<i title="view" class="fa fa-eye fa-fw" /></a>') +

                                 ((row.IsManagerAccess || row.IsCreatorAccess) ? ('<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.remove(' + row.CallCenterHeaderID + ',' + true + ')" alt="Remove" ' +
                                     'security permission-key="CrisisLine-CrisisLine-CrisisLine" permission="delete" space-key-press>' +
                                     '<i title="Remove" class="fa fa-trash fa-fw" /></a>') : '')
                         + '</span>');
                     }
                 }
                ]
            }

            var tableOptionsNormal = {
                columns: [
                {
                    field: "SignedOn",
                    title: " SIGNED?",
                    sortable: true,
                    formatter: function (value, row) {
                        return (row.SignedOn ? SERVICE_SIGN.Signed : SERVICE_SIGN.NotSigned)
                    }

                },
                {
                    field: "MRN",
                    title: "MRN",
                    sortable: true
                },
                {
                    field: "CallCenterHeaderID",
                    title: "Incident ID",
                    sortable: true
                },
                {
                    field: "CallDate",
                    title: "BEGIN DATE/TIME",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return toStringDateTime(value);
                        } else
                            return '';
                    }
                },
                {
                    field: "EndDate",
                    title: "End Date/Time",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return toStringDateTime(value);
                        } else
                            return '';
                    }
                },
                {
                    field: "Duration",
                    title: "Duration",
                    sortable: true
                },
                {
                    field: "Caller",
                    title: "Caller Name",
                    sortable: true
                },
                {
                    field: "CallerContactNumber",
                    title: "Caller Contact Number",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return $filter('toPhone')(value);
                        } else
                            return '';
                    }
                },

                {
                    field: "ClientFirstName",
                    title: "Contact First Name",
                    sortable: true,
                },
                {
                    field: "ClientLastName",
                    title: "Contact Last Name",
                    sortable: true,
                },
                //[Bug:12433]. remove field
                //{
                //    field: "CallDate",
                //    title: "Date of Incident",
                //    sortable: true,
                //    formatter: function (value, row) {
                //        if (value) {
                //            return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                //        } else
                //            return '';
                //    },
                //    visible: false
                //},
                {
                    field: "CallStatus",
                    title: "Call Status",
                    sortable: true
                },
                {
                    field: "FollowUpRequired",
                    title: "Follow up Required",
                    sortable: true,
                    formatter: function (value, row) {
                        return value ? "Yes" : "No";
                    }
                },
                {
                    field: "ParentCallCenterHeaderID",
                    title: "History",
                    sortable: true,
                    formatter: function (value, row) {
                        return value ? "Yes" : "No";
                    }
                },

                {
                    field: "ProviderSubmittedBy",
                    title: "Provider Submitted By",
                    sortable: true
                },
                {
                    field: "ProgramUnitID",
                    title: "Program Unit",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.ProgramUnit, value);
                        else
                            return "";
                    }
                },
                {
                    field: "CallCenterHeaderID",
                    title: "",
                    formatter: function (value, row, index) {
                        row.ClientTypeID = row.ClientTypeID == null ? 0 : row.ClientTypeID;
                        var isVoided = row["IsVoided"];
                        //&& row.IsSignedByUser && row.SignedOn
                        return (
                                '<span class="text-nowrap pull-right">' +

                              ((row["ParentCallCenterHeaderID"] && row.CallStatusID !== CALL_STATUS.COMPLETE) ?
                            '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.openFollowUpHistory(' + row.CallCenterHeaderID + ')" alt="Follow up histpry" ' +
                                        'space-key-press>' +
                                                    '<i  title="Incident History" class="fa fa-history fa-fw" /><span class="sr-only">Incident History</span></a>' : '') +

                                ((row["FollowUpRequired"]) ?
                            '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.followUp(' + row.ContactID + ',' + value + ')" alt="Follow up Required" ' +
                                        'space-key-press>' +
                                                    '<i  title="Follow up Required" class="fa fa-flag fa-fw" /></a>' : '') +
                            createLinks(row) +
                               ((!isVoided && row["CallStatusID"] === CALL_STATUS.COMPLETE) ? createLinkForVoidService(row) : '') +

                                ((row.IsManagerAccess || row.IsCreatorAccess) ? (row.CallStatusID === CALL_STATUS.PENDING ? '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.remove(' + value + ')" alt="Remove" ' +
                                    'security permission-key="CrisisLine-CrisisLine-CrisisLine" permission="delete" space-key-press>' +
                                    '<i title="Remove" class="fa fa-trash fa-fw" /></a>' : '') : '')
                        + '</span>');
                    }
                }
                ]
            };
            self.initializeBootstrapTable = function () {
                self.tableoptions = tableDefaultOptions
                self.tableoptions.columns = tableOptionsNormal.columns;

                self.followUpTableOptions = FollowUptableOptions;
                self.followUpTableOptions.columns = followUpHistoryCoulmns.columns;
            };

            self.openFollowUpHistory = function (callCenterHeaderID) {
                self.followupTableLoad(callCenterHeaderID);
                $('#followupHistoryModal').modal('show');
            }

            var addCacheService = function (isManagerAccess, isCreatorAccess) {
                if (isManagerAccess)
                    cacheService.add('IsManagerAccess', true);
                else
                    cacheService.add('IsManagerAccess', false);
                if (isCreatorAccess)
                    cacheService.add('IsCreatorAccess', true);
                else
                    cacheService.add('IsCreatorAccess', false);
            };

            self.followUpNavigate = function (CallCenterHeaderID, contactID, isManagerAccess, isCreatorAccess) {
                addCacheService(isManagerAccess, isCreatorAccess);
                $('#followupHistoryModal').modal('hide');
                angular.extend($stateParams, {
                    ContactID: contactID,
                    CallCenterHeaderID: CallCenterHeaderID,
                    ReadOnly: (isManagerAccess || isCreatorAccess) ? "edit" : "view"
                })
                $('#followupHistoryModal').on('hidden.bs.modal', function () {
                    $state.transitionTo('callcenter.crisisline.callerinformation', $stateParams);
                });
            }

            var setTooltipTitle = function (rowData, toolTipData) {
                //Set the Tooltip data in null or undefined
                if (!toolTipData) {
                    toolTipData = rowData;
                }

                //Create new DOM Node element
                var trDivElementRow = $('<div tooltip></div>').text(rowData);
                var toolTipDataShort = toolTipData.substring(535, length) + "...";
                trDivElementRow = trDivElementRow
                                    .attr('data-toggle', 'tooltip')
                                    .attr('title', toolTipDataShort)
                                    .text(rowData);

                //Return the Node HTML text for the Grid table body
                return trDivElementRow[0].outerHTML;
            };

            var tableDefaultOptions = {
                pagination: true,
                pageSize: 10,
                pageList: [10, 20, 30],
                search: false,
                showColumns: true,
                data: [],
                undefinedText: '',
                onClickRow: function (e, row, $element) {
                    //self.prepRowEditData(e);
                },
            };

            var FollowUptableOptions = {
                pagination: true,
                pageSize: 5,
                pageList: [10, 20, 30],
                search: false,
                showColumns: true,
                data: [],
                undefinedText: '',
                onClickRow: function (e, row, $element) {
                    //self.prepRowEditData(e);
                },
            };

            var tableOptionsApprovals = {
                columns: [
                    {
                        field: "selected",
                        checkbox: true,
                        formatter: function (value, row) {
                            if (!row["EndDate"] || !row.SignedOn || (row["EndDate"] && (row["CallStatusID"] === CALL_STATUS.COMPLETE || row["CallStatusID"] === CALL_STATUS.VOID))) {
                                return { disabled: true }
                            }
                            else {
                                enableApprovalsCheckbox = true;
                                return value;
                            }
                        }
                    },
                    {
                        field: "SignedOn",
                        title: " SIGNED?",
                        sortable: true,
                        formatter: function (value, row) {
                            return (row.SignedOn ? SERVICE_SIGN.Signed : SERVICE_SIGN.NotSigned)
                        }

                    },
                   {
                       field: "CountyID",
                       title: "County",
                       sortable: true,
                       formatter: function (value, row) {
                           if (value)
                               return lookupService.getText(LOOKUPTYPE.County, value);
                           else
                               return "";
                       }
                   },
                {
                    field: "ProgramUnitID",
                    title: "Program Unit",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.ProgramUnit, value);
                        else
                            return "";
                    }
                },
                //[Bug:11787]. remove 'Follow up required', 'History' fields.
                //{
                //    field: "FollowUpRequired",
                //    title: "Follow Up Required",
                //    sortable: true,
                //    formatter: function (value, row) {
                //        return (value === true) ? "Yes" : "No";
                //    }
                //},

                //{
                //    field: "ParentCallCenterHeaderID",
                //    title: "History",
                //    sortable: true,
                //    formatter: function (value, row) {
                //        return value ? "No" : "Yes";
                //    }
                //},
                {
                    field: "ProviderSubmittedBy",
                    title: "Provider Submitted By",
                    sortable: true,
                },
                //[bug:12186]. change BeginDate to CallDate.
                {
                    field: "CallDate",
                    title: "BEGIN DATE/TIME",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return toStringDateTime(value);
                        } else
                            return '';
                    }
                },
                {
                    field: "EndDate",
                    title: "End Date/Time",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return toStringDateTime(value);
                        } else
                            return '';
                    }
                },
                 {
                     field: "Duration",
                     title: "Duration",
                     sortable: true
                 },
                {
                    field: "ServiceItemID",
                    title: "Service Item",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.Service, value);
                        else
                            return "";
                    }
                },
                {
                    field: "ServiceStatusID",
                    title: "Service Status",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.ServiceStatus, value);
                        else
                            return "";
                    }

                },
                {
                    field: "Caller",
                    title: "Caller's Name",
                    sortable: true
                },
                {
                    field: "MRN",
                    title: "MRN",
                    sortable: true
                },
                //[Bug:11787]. show full name in one field. 
                //{
                //    field: "ClientLastName",
                //    title: "Client Last Name",
                //    sortable: true

                //},

                {
                    field: "ClientFirstName",
                    title: "Contact Name",
                    sortable: true,
                    formatter: function (value, row) {
                        return row.ClientLastName + ', ' + row.ClientFirstName;
                    }

                },

                {
                    field: "RecipientCodeID",
                    title: "Recipient Code",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.RecipientCode, value);
                        else
                            return "";
                    }

                },
                {
                    field: "AttendanceStatusID",
                    title: "Attendance Code",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.AttendanceStatus, value);
                        else
                            return "";
                    }

                },
                {
                    field: "SuicideHomicideID",
                    title: "S/H",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.SHID, value);
                        else
                            return "";
                    }
                },
                {
                    field: "CallCenterPriorityID",
                    title: "Priority",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value)
                            return lookupService.getText(LOOKUPTYPE.CallCenterPriority, value);
                        else
                            return "";
                    }

                },
                {
                    field: "CallerContactNumber",
                    title: "Phone Number",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return $filter('toPhone')(value);
                        } else
                            return '';
                    }

                },
                {
                    field: "ProgressNote",
                    title: "Progress Note",
                    sortable: true,
                    formatter: function (value, row) {
                        return row.NoteHeaderID ? 'YES' : 'NO';
                    }
                },
                {
                    field: "ReasonCalled",
                    title: "Reason Called",
                    sortable: true,
                    formatter: function (value, row) {

                        var displayReasonData = (value && value.length > 60)
                                ? value.substring(0, 60)
                                : value;
                        return setTooltipTitle(displayReasonData, value);
                    }
                },
                {
                    field: "Disposition",
                    title: "Disposition",
                    sortable: true,
                    formatter: function (value, row) {
                        var val;
                        try {
                            if (value && JSON.parse(value).length > 0) {
                                val = JSON.parse(value)[0].Comment;
                            }
                            else
                                val = value == '""' ? '' : value;
                        } catch (e) {
                            val = value;
                        }
                        finally {
                            var displayDispositionData = (val && val.length > 60)
                                                                            ? val.substring(0, 60)
                                                                            : val;

                            return setTooltipTitle(displayDispositionData, val);
                        }
                    }
                },
                {
                    field: "OtherInformation",
                    title: "Other Information",
                    sortable: true,
                    formatter: function (value, row) {

                        var displayOtherInfoData = (value && value.length > 60)
                                ? value.substring(0, 60)
                                : value;
                        return setTooltipTitle(displayOtherInfoData, value);
                    }
                },
                {
                    field: "CallCenterHeaderID",
                    title: "",
                    formatter: function (value, row, index) {
                        row.ClientTypeID = row.ClientTypeID == null ? 0 : row.ClientTypeID;
                        var isVoided = row["IsVoided"];
                        return (

                            '<span class="text-nowrap pull-right">' +

                               ((row["ParentCallCenterHeaderID"] && row.CallStatusID !== CALL_STATUS.COMPLETE) ?
                            '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.openFollowUpHistory(' + row.CallCenterHeaderID + ')" alt="Follow up history" ' +
                                        'space-key-press>' +
                                                    '<i  title="Incident History" class="fa fa-history fa-fw" /><span class="sr-only">Incident History</span></a>' : '') +

                                ((row["FollowUpRequired"]) ?
                            '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.followUp(' + row.ContactID + ',' + value + ')" alt="Follow up Required" ' +
                                        'space-key-press>' +
                                                    '<i  title="Incident History" class="fa fa-flag fa-fw" /><span class="sr-only">Follow up Required</span></a>' : '') +
                            createLinks(row) +
                            ((!isVoided && row["CallStatusID"] === CALL_STATUS.COMPLETE) ? createLinkForVoidService(row) : '') +
                            ((row.IsManagerAccess || row.IsCreatorAccess) ? (row.CallStatusID === CALL_STATUS.PENDING ? '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.remove(' + value + ')" alt="Remove" ' +
                                    'security permission-key="CrisisLine-CrisisLine-CrisisLine" permission="delete" space-key-press>' +
                                    '<i title="Remove" class="fa fa-trash fa-fw" /></a>' : '') : '')
                        + '</span>');
                    }
                }]
            };



            self.init();
            var createLinks = function (row) {
                var sref = 'callcenter.crisisline.callerinformation({CallCenterHeaderID:' + row.CallCenterHeaderID + ',ContactID:' + row.ContactID + ', ReadOnly:\'#mode\'  })';
                return createLink(row, hasEditPermission, permission_key, sref);
            }

            //Create the Action link based on the Arguments passed
            var createLink = function (row, hasEditPermission, permission_key, sref) {
                var mode = row.IsManagerAccess || row.IsCreatorAccess || $scope.isApprover ? 'edit' : (row.IsCreatorAccess ? (row.CallStatusID === CALL_STATUS.COMPLETE || row.CallStatusID === CALL_STATUS.VOID ? 'view' : 'edit') : 'view');
                return (hasEditPermission ? '<a href=#  ' + (hasEditPermission ? 'data-default-action' : 'data-default-no-action') + ' ng-click="navigateTo(' + row.CallStatusID + ',' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.IsManagerAccess + ',' + row.IsCreatorAccess + ')" alt="' + mode + '" security permission-key="' + permission_key + '" permission="update" class="padding-left-small padding-right-small" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                            '<i title="' + mode + '" class="fa ' + (row.IsManagerAccess || row.IsCreatorAccess || $scope.isApprover ? 'fa-pencil' : 'fa-eye') + ' fa-fw" /></a>' :
                 '<a ng-click="navigateTo(' + row.CallStatusID + ',' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.IsManagerAccess + ',' + row.IsCreatorAccess + ')" href=#  data-default-no-action alt="' + mode + '" security permission-key="' + permission_key + '" permission="read"  class="padding-left-small padding-right-small" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                         '<i title="' + mode + '" class="fa fa-eye fa-fw" /></a>')
            }

            $scope.navigateTo = function (callStatusID, callCenterHeaderID, contactID, isManagerAccess, isCreatorAccess) {
                var mode = hasEditPermission ? (isManagerAccess ? 'edit' : (isCreatorAccess ? (callStatusID === CALL_STATUS.COMPLETE || callStatusID === CALL_STATUS.VOID ? 'view' : 'edit') : 'view')) : "view";
                if (mode === 'view')
                    cacheService.add('IsReadOnlyScreens', true);
                addCacheService(isManagerAccess, isCreatorAccess);
                $scope.Goto("callcenter.crisisline.callerinformation", { CallCenterHeaderID: callCenterHeaderID, ContactID: contactID, ReadOnly: mode });
            };

            $scope.$on('voidServiceReloadGrid', function (event, args) {
                alertService.success("Service has been void successfully.");
                self.searchCallCenter($('#txtCallCenterSummary').val());
            });

            $scope.voidFlyout = function (isSigned, serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID, contactID) {
                if (isSigned) {
                    $scope.displayFlyout = SERVICEVOID_FLYOUT;
                    if (!flyoutElement.hasClass('active'))
                        openVoidFlyout(serviceRecordingID, noteHeaderID, benefitsAssistanceID, responseID, contactID);
                }
                else {
                    alertService.error("Can not void if there service recording is not signed.");
                }
            };

            var openVoidFlyout = function (serviceRecordingID, noteHeaderID, ID, responseID, contactID) {
                $scope.voidModel = objVoidModel(serviceRecordingID, noteHeaderID, ID, responseID, contactID);
                flyoutElement.addClass('active');
                formService.reset();
                $rootScope.defaultFormName = 'vs.voidServiceForm';
            };
            //Create link for Void service
            var createLinkForVoidService = function (row) {
                var isSigned = (row.SignedOn) ? true : false;
                return '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)"  alt="Void Service" security permission-key="CrisisLine-CrisisLine-Approver" permission="create"  ng-click="voidFlyout(' + isSigned + ',' + row.ServiceRecordingID + ',' + row.NoteHeaderID + ',' + row.CallCenterHeaderID + ',' + row.ResponseID + ',' + row.ContactID + ')"' +
                                        'data-module="callcenter" data-feature="crisisline" data-action="append" space-key-press>' +
                                        '<i  title="Void Service" class="fa fa-ban fa-fw" /></a>';
            };
        }]);
}());
