(function () {
    angular.module('xenatixApp')
    .controller('lawLiaisonSummaryController', ['$filter', 'alertService', '$stateParams', '$state', 'formService', 'lawLiaisonSummaryService', '$scope', 'lookupService', 'roleSecurityService', 'navigationService', 'voidService', 'lawLiaisonFollowUpService', 'cacheService', '$rootScope', 'callerInformationService',
    function ($filter, alertService, $stateParams, $state, formService, lawLiaisonSummaryService, $scope, lookupService, roleSecurityService, navigationService, voidService, lawLiaisonFollowUpService, cacheService, $rootScope, callerInformationService) {

        var self = this;
        var maxSignValidationDays = 7;
        var flyoutElement = $('.row-offcanvas');
        self.searchTypeFilter = "2";//default value
        self.searchText = "";
        var CONTACT_FLYOUT = 'CONTACT';
        var SERVICEVOID_FLYOUT = 'SERVICEVOID';
        self.init = function () {
            self.CallCenterSummaryTable = $("#callCenterSummaryTable");
            self.parentTable = $("#parentTable");
            self.childTable = $("#childTable");
            self.initializeBootstrapTable();
            self.searchCallCenter(' ');
            $scope.$parent['autoFocus'] = true;
            $scope.$parent['voidFocus'] = false;
            $scope.$evalAsync(function () {
                applyDropupOnGrid(true);
            });
            offCanvasNav.init();
            $scope.dataEntryErrorID = 2;
            $scope.displayFlyout = CONTACT_FLYOUT;
        };
        var permission_key = "LawLiaison-LawLiaison-LawLiaison";
        var hasEditPermission = roleSecurityService.hasPermission(permission_key, PERMISSION.UPDATE);
        self.incommingOutgoingFilterChange = function () {
            alert(self.searchTypeFilter);
        };

        var objVoidModel = function (serviceRecordingID, ID, contactID) {
            var obj = {
                ServiceRecordingVoidID: 0,
                IsCallCenterVoided: true,
                ServiceRecordingID: serviceRecordingID,
                ServiceRecordingVoidReasonID: null,
                IsCreateCopyToEditHide: false,
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
                ID: ID,
                ContactID: contactID,
                Comments: "",
                NoteHeaderID: ID
            };
            return obj;
        }

        self.searchCallCenter = function (searchStr) {
            //hit get method of services, consolidate the data and display in the grid
            if (flyoutElement.hasClass('active'))
                flyoutElement.removeClass('active');
            searchStr = searchStr == undefined ? "" : searchStr;
            navigationService.get().then(function (data) {
                if (data.DataItems != undefined && data.DataItems.length > 0) {
                    var user = data.DataItems[0];
                    lawLiaisonSummaryService.get(searchStr, user.UserID, Number(self.searchTypeFilter)).then(function (data) {
                        if (data && data.DataItems) {
                            self.CallCenterSummary = data.DataItems;
                            self.CallCenterSummaryTable.bootstrapTable('load', self.CallCenterSummary);
                        } else {
                            self.CallCenterSummary = [];
                            self.CallCenterSummaryTable.bootstrapTable('removeAll');
                        }
                    });
                }
            });
        };

        self.loadTableData = function (callCenterHeaderID) {
            lawLiaisonSummaryService.getLawLiaisonIncident(callCenterHeaderID).then(function (data) {
                if (hasData(data)) {
                    self.ParentData = $filter('filter')(data.DataItems, function (item) { return item.CallCenterHeaderID }, true);
                    self.parentTable.bootstrapTable('load', self.ParentData);
                    self.ChildData = data.DataItems[0].RelatedItems;
                    self.childTable.bootstrapTable('load', self.ChildData);
                } else {
                    self.ParentData = [];
                    self.parentTable.bootstrapTable('removeAll');
                    self.ChildData = [];
                    self.childTable.bootstrapTable('removeAll');
                }
            });
        }

        self.remove = function (callerHeaderId, parentCallCenterHeaderID, followUpRequired, isVoided, serviceRecordingID) {
            // Parent call log
            // Can not delete parent call log if there are active (any status other than VOID) services at parent level or child level.
            var hasActiveService = serviceRecordingID && !isVoided ? true : false;
            if (!parentCallCenterHeaderID) {
                var allChilds = $filter('filter')(self.CallCenterSummary, function (item) {
                    return item.ParentCallCenterHeaderID == callerHeaderId;
                }, true);

                var activeChildsServices = $filter('filter')(allChilds, function (item) {
                    return item.ServiceRecordingID && item.IsVoided != true;
                }, true);

                if (allChilds.length != 0 && (hasActiveService || activeChildsServices.length != 0)) {
                    alertService.error('Can not delete call log if there are any active services at original incident or follow–up.');
                    return;
                }
            }

            bootbox.confirm("Selected Call Center will be deactivated.\n Do you want to continue?", function (result) {
                if (result) {
                    lawLiaisonSummaryService.remove(callerHeaderId).then(function (data) {
                        if (data && data.ResultCode >= 0) {
                            alertService.success("Call Center has been deleted successfully.");
                            self.searchCallCenter($('#txtCallCenterSummary').val());

                            if (angular.element('#lawLiaisonFollowUpHistoryModal').is(':visible')) {
                                self.closeFollowUpHistory();
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

        self.followUp = function (contactID, callCenterHeaderID, isCreatorAccess) {

            lawLiaisonFollowUpService.followUp(callCenterHeaderID).then(function (newCallCenterHeaderID) {
                cacheService.add('lawLiaisonFollowUp', {
                    followupRequired: true,
                    parentCallCenterHeaderID: callCenterHeaderID
                });
                cacheService.add('IsReadOnlyLLScreens', false);
                gotoLawEnforcement(contactID, newCallCenterHeaderID);
            });
        }

        self.edit = function (callCenterHeaderID, contactID, parentCallCenterHeaderID, isCreatorAccess, isVoided) {
            if (parentCallCenterHeaderID) {
                cacheService.add('lawLiaisonFollowUp', {
                    followupRequired: true,
                    parentCallCenterHeaderID: parentCallCenterHeaderID
                });
            }
            cacheService.add('IsReadOnlyLLScreens', hasEditPermission ? (!isCreatorAccess || isVoided) : true);

            gotoLawEnforcement(contactID, callCenterHeaderID);
        }

        function gotoLawEnforcement(contactID, callCenterHeaderID) {
            angular.extend($stateParams, {
                ContactID: contactID,
                CallCenterHeaderID: callCenterHeaderID
            });

            if (angular.element('#lawLiaisonFollowUpHistoryModal').is(':visible')) {
                $('#lawLiaisonFollowUpHistoryModal').on('hidden.bs.modal', function () {
                    $state.transitionTo('callcenter.lawliaison.lawenforcement', $stateParams);
                });
            }
            else {
                $state.transitionTo('callcenter.lawliaison.lawenforcement', $stateParams);
            }
        }

        self.openFollowUpHistory = function (callCenterHeaderID) {
            self.loadTableData(callCenterHeaderID);
            $('#lawLiaisonFollowUpHistoryModal').modal('show');
        }

        self.closeFollowUpHistory = function () {
            $('#lawLiaisonFollowUpHistoryModal').modal('hide');
        }

        var childTableOptions = {
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

        var parentTableOptions = {
            pagination: false,
            data: [],
            search: false,
            undefinedText: '',
            onClickRow: function (e, row, $element) {
                //self.prepRowEditData(e);
            },
        };
        var setTooltipTitle = function (rowData, toolTipData) {
            //Set the Tooltip data in null or undefined
            if (!toolTipData) {
                toolTipData = rowData;
            }

            //Create new DOM Node element
            var trDivElementRow = $('<div tooltip></div>').text(rowData);
            var toolTipDataShort = (toolTipData) ? toolTipData.substring(535, length) + "..." : "";
            trDivElementRow = trDivElementRow
                                .attr('data-toggle', 'tooltip')
                                .attr('title', toolTipDataShort)
                                .text(rowData);

            //Return the Node HTML text for the Grid table body
            return trDivElementRow[0].outerHTML;
        };

        var setNatureCallTooltipTitle = function (rowData, toolTipData) {
            //Set the Tooltip data in null or undefined
            if (!toolTipData) {
                toolTipData = rowData;
            }
            //Create new DOM Node element
            var trDivElementRow = $('<div tooltip data-html="true"></div>');
            var natureOfCallTollTip = $('<div></div>').html(getNatureOfCallToolTipTemplate(rowData));
            var toolTipDataShort = (rowData) ? JSON.stringify(rowData).substring(535, length) + "..." : "";
            trDivElementRow = trDivElementRow
                                .attr('data-toggle', 'tooltip')
                                .attr('title', natureOfCallTollTip[0].outerHTML)
                                .html(toolTipData);
            return trDivElementRow[0].outerHTML;
        };

        var getNatureOfCallToolTipTemplate = function (rowData) {
            var remaingLength = 500;//maximum tooltip length
            var conatinerElement = $('<ul class="nomargin padding-small">');
            angular.forEach(rowData, function (natureCallItem, key) {
                var commentedDate = $filter('formatDate')(natureCallItem.CommentDate, 'MM/DD/YYYY hh:mm:ss A');
                if (remaingLength > 0) {
                    var currentLength = (natureCallItem.UserName + commentedDate + natureCallItem.Comment).toString().length + 13;
                    var userNameDateLength = (natureCallItem.UserName + commentedDate).toString().length + 13;
                    var comment = natureCallItem.Comment.substring(0, (remaingLength - userNameDateLength));
                    if (remaingLength > userNameDateLength) {
                        remaingLength = remaingLength - currentLength;
                        conatinerElement.append('<li><span class="ase-comment-attr"><b>' + natureCallItem.UserName + '</b> commented on ' + commentedDate + '</span><p class="ase-comment-content">' + ((remaingLength > 0) ? comment : comment + " ...") + '</p></li>');
                    }
                }

            });
            conatinerElement.append('</ul>');
            return conatinerElement[0].outerHTML;
        };
        self.initializeBootstrapTable = function () {
            self.tableoptions = {
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
                field: "MRN",
                title: "MRN"
            },
            {
                field: "Status",
                title: "Status"
            },
            {
                field: "ServiceItemID",
                title: "Service",
                formatter: function (value, row) {
                    return value ? lookupService.getText(LOOKUPTYPE.RecordingServices, value) : "";
                }
            },
            {
                field: "ServiceStatusID",
                title: "Service Status",
                formatter: function (value, row) {
                    if (value)
                        return lookupService.getText(LOOKUPTYPE.ServiceStatus, value);
                    else
                        return "";
                }
            },
            {
                field: "CallDate",
                title: "Call Date",
                formatter: function (value, row) {
                    if (value) {
                        return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                    } else
                        return '';
                }
            },
            {
                field: "Caller",
                title: "Caller Name"
            },
            {
                field: "CallerContactNumber",
                title: "Caller Contact Number",
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
                formatter: function (value, row) {
                    return row.ClientFirstName;
                }

            },
            {
                field: "ClientLastName",
                title: "Contact Last Name",
                formatter: function (value, row) {
                    return row.ClientLastName;
                }

            },
            {
                field: "ReasonCalled",
                title: "Reason Called",
                sortable: true,
                formatter: function (value, row) {

                    var displayReasonData = (value && value.length > 60)
                            ? value.substring(0, 60) + "..."
                            : value;
                    return setTooltipTitle(displayReasonData, value);
                }
            },
            {
                field: "NatureofCall",
                title: "Nature of Service",
                sortable: true,
                formatter: function (value, row) {
                    var parsedData = "";
                    var natureofCall = "";
                    var comment = "";
                    if (value && value.length > 0) {
                        parsedData = parseJSON(value);
                        comment = parsedData ? parsedData[0].Comment : '';
                        natureofCall = (comment && comment.length > 60)
                                ? comment.substring(0, 60) + "..."
                                : comment;
                    }
                    return setNatureCallTooltipTitle(parsedData, natureofCall);
                }
            },           
            {
                field: "ParentCallCenterHeaderID",
                title: "Record Type",
                sortable: true,
                formatter: function (value, row) {
                    if (!value) {
                        return row.HasChild ? 'Original Incident' : '';
                    }
                    else {
                        return 'Follow–Up';
                    }
                    
                }
            },
            {
                field: "ProviderSubmittedBy",
                title: "Provider Submitted By",
                sortable: true
            },           
            {
                field: "CallCenterHeaderID",
                title: "",
                formatter: function (value, row, index) {
                    row.ClientTypeID = row.ClientTypeID == null ? 0 : row.ClientTypeID;
                    var isVoided = row["IsVoided"];
                    var hasChild = row["HasChild"] ? true : false;
                    return (
                        '<span class="text-nowrap pull-right">' +
                        createLinks(row) +
                        ((row["ParentCallCenterHeaderID"] && row.CallStatusID !== CALL_STATUS.COMPLETE) || (row.CallStatusID !== CALL_STATUS.COMPLETE && hasChild) ?
                        '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.openFollowUpHistory(' + row.CallCenterHeaderID + ')" alt="Follow up history" space-key-press>' +
                        '<i title="Incident History" class="fa fa-history fa-fw" /><span class="sr-only">Incident History</span></a>' : '') +

                        ((row["FollowUpRequired"] && !isVoided && row.CallStatusID !== CALL_STATUS.COMPLETE && row["ParentCallCenterHeaderID"] == null) ?
                        '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.followUp(' + row.ContactID + ',' + value + ',' + row.IsCreatorAccess + ')" alt="Follow up Required" space-key-press>' +
                        '<i title="Follow up Required" class="fa fa-flag" /></a>' : '') +

                        ((!isVoided && row.SignedOn) ? createLinkForVoidService(row) : '') +

                        (row.IsCreatorAccess ? ((!row.SignedOn || isVoided) ? '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.remove(' + row.CallCenterHeaderID + ',' + row.ParentCallCenterHeaderID + ',' + row.FollowUpRequired + ',' + row.IsVoided + ',' + row.ServiceRecordingID + ')"  alt="Remove" security permission-key="LawLiaison-LawLiaison-LawLiaison" permission="delete" space-key-press>' +
                        '<i title="Remove" class="fa fa-trash fa-fw" /></a>' : '') : '')
                        + '</span>');
                }
            }]
            };

            self.childTableOptions = childTableOptions;
            self.childTableOptions.columns = childTableCoulmn.columns;
            self.parentTableOptions = parentTableOptions;
            self.parentTableOptions.columns = parentTableCoulmn.columns;
        };

        var parentTableCoulmn = {
            columns: [
           {
               field: "MRN",
               title: "MRN",

           },
           {
               field: "CallCenterHeaderID",
               title: "Incident ID",

           },
           {
               field: "CallDate",
               title: "Call Date",
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

           },
           {
               field: "ClientName",
               title: "Client Name",
               formatter: function (value, row) {
                   return row.ClientFirstName + ' ' + row.ClientLastName;
               }

           },
           {
               field: "Priority",
               title: "Priority"
           },
           {
               field: "CallCenterHeaderID",
               title: "",
               formatter: function (value, row, index) {
                   row.ClientTypeID = row.ClientTypeID == null ? 0 : row.ClientTypeID;
                   return (
                           '<span class="text-nowrap pull-right">' +

                       (hasEditPermission ? '<a data-default-action class="padding-left-small padding-right-small" data-dismiss="modal" ng-click="ctrl.edit(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.ParentCallCenterHeaderID + ',' + row.IsCreatorAccess + ')" alt="update" security permission-key="LawLiaison-LawLiaison-LawLiaison" permission="update" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                               '<i title="' + (row.IsCreatorAccess ? 'update' : 'view') + '" class="fa ' + (row.IsCreatorAccess ? 'fa-pencil' : 'fa-eye') + ' fa-fw" /></a>' :
                       '<a data-default-no-action class="padding-left-small padding-right-small" data-dismiss="modal" ng-click="ctrl.edit(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.ParentCallCenterHeaderID + ',' + row.IsCreatorAccess + ')" alt="view" security permission-key="LawLiaison-LawLiaison-LawLiaison" permission="read" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                               '<i title="view" class="fa fa-eye fa-fw" /></a>') +

                           (row.IsCreatorAccess ? (!row.SignedOn ? '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="ctrl.remove(' + row.CallCenterHeaderID + ',' + null + ',' + true + ',' + row.IsVoided + ',' + row.ServiceRecordingID + ')" alt="Remove" ' +
                               'security permission-key="LawLiaison-LawLiaison-LawLiaison" permission="delete" space-key-press>' +
                               '<i title="Remove" class="fa fa-trash fa-fw" /></a>' : '') : '')
                   + '</span>');

               }
           }]
        }

        var childTableCoulmn = {
            columns: [
         {
             sortable: true,
             field: "ITEM",
             title: "ITEM",


         },
         {
             sortable: true,
             field: "TYPE",
             title: "TYPE",


         },
         {
             sortable: true,
             field: "DATE",
             title: "Date",
             formatter: function (value, row) {
                 if (value) {
                     return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                 } else
                     return '';
             },

         },
         {
             sortable: true,
             field: "Submitted By",
             title: "Submitted By",
             formatter: function (value, row) {
                 return row.FirstName + ' ' + row.LastName;
             },


         },
         {
             field: "CallCenterHeaderID",
             title: "",
             formatter: function (value, row, index) {
                 row.ClientTypeID = row.ClientTypeID == null ? 0 : row.ClientTypeID;
                 return (
                         '<span class="text-nowrap pull-right">' +
                         (hasEditPermission ? '<a data-default-action class="padding-left-small padding-right-small" data-dismiss="modal" ng-click="ctrl.edit(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.ParentCallCenterHeaderID + ',' + row.IsCreatorAccess + ')" alt="update" security permission-key="LawLiaison-LawLiaison-LawLiaison" permission="update" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                           '<i title="' + (row.IsCreatorAccess ? 'update' : 'view') + '" class="fa ' + (row.IsCreatorAccess ? 'fa-pencil' : 'fa-eye') + ' fa-fw" /></a></span>' :
                        '<a data-default-no-action class="padding-left-small padding-right-small" data-dismiss="modal" ng-click="ctrl.edit(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.ParentCallCenterHeaderID + ',' + row.IsCreatorAccess + ')" alt="view" security permission-key="LawLiaison-LawLiaison-LawLiaison" permission="read" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
                                '<i title="view" class="fa fa-eye fa-fw" /></a></span>'));
             },

         }
            ]
        }

        var getServiceRecordingStatus = function (value) {
            for (var key in CALL_STATUS) {
                if (CALL_STATUS[key] == value) {
                    return toTitleCase(key);
                }
            }
            return null;
        };



        $scope.voidFlyout = function (isSigned, serviceRecordingID, ID, contactID) {
            if (isSigned) {
                $scope.displayFlyout = SERVICEVOID_FLYOUT;
                if (!flyoutElement.hasClass('active'))
                    openVoidFlyout(serviceRecordingID, ID, contactID);
            }
            else {
                alertService.error("Can not void if there service recording is not signed.");
            }
        };

        var openVoidFlyout = function (serviceRecordingID, ID, contactID) {
            $scope.voidModel = objVoidModel(serviceRecordingID, ID, contactID);
            flyoutElement.addClass('active');
            $rootScope.defaultFormName = 'vs.voidServiceForm';
        };

        $scope.$on('voidServiceReloadGrid', function (event, args) {
            alertService.success("Service has been void successfully.");
            callerInformationService.updateModifiedOn($scope.voidModel.ID).then(function () {
                self.searchCallCenter($('#txtCallCenterSummary').val());
            });
        });

        //Create link for Void service
        var createLinkForVoidService = function (row) {
            var isSevenDaysOld = $filter('getDaysDifference')(row.ServiceEndDate, moment.utc()) <= maxSignValidationDays;
            var isSigned = (row.SignedOn) ? true : false;
            if (row.IsCreatorAccess && isSevenDaysOld) {
                return '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" alt="Void Service" security permission="create"  ng-click="voidFlyout(' + isSigned + ',' + row.ServiceRecordingID + ',' + row.CallCenterHeaderID + ',' + row.ContactID + ')"' +
                                            'data-action="append" space-key-press>' +
                                            '<i title="Void Service" class="fa fa-ban fa-fw" /></a>';
            }
            else {
                return '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" alt="Void Service" security permission-key="LawLiaison-LawLiaison-Void" permission="create"  ng-click="voidFlyout(' + isSigned + ',' + row.ServiceRecordingID + ',' + row.CallCenterHeaderID + ',' + row.ContactID + ')"' +
                            'data-module="callcenter" data-feature="crisisline" data-action="append" space-key-press>' +
                            '<i title="Void Service" class="fa fa-ban fa-fw" /></a>';
            }
        };

        self.init();
        var createLinks = function (row) {
            return (hasEditPermission ? '<a ' + (hasEditPermission ? 'data-default-action' : 'data-default-no-action') + ' ng-click="ctrl.edit(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.ParentCallCenterHeaderID + ',' + row.IsCreatorAccess + ',' + row.IsVoided + ' )" alt="edit" security class="padding-left-small padding-right-small" permission-key="' + permission_key + '" permission="update" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
            '<i title="' + ((!row.IsCreatorAccess || row.IsVoided) ? 'view' : 'edit') + '" class="fa ' + (row.IsCreatorAccess ? (row.IsVoided ? 'fa-eye' : 'fa-pencil') : 'fa-eye') + ' fa-fw" /></a>' :
                '<a data-default-no-action class="padding-left-small padding-right-small" ng-click="ctrl.edit(' + row.CallCenterHeaderID + ',' + row.ContactID + ',' + row.ParentCallCenterHeaderID + ',' + row.IsCreatorAccess + ',' + row.IsVoided + ')" alt="view" security permission-key="' + permission_key + '" permission="read" program-units="' + row.ProgramUnitID + '" on-action="openFlyout(' + row.ContactID + ')" space-key-press>' +
         '<i title="view" class="fa fa-eye fa-fw" /></a>');
        }
    }]);
}());
