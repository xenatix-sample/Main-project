
angular.module("xenatixApp")
    .controller("ifspController", [
        "$scope", "$filter", "ifspService", "alertService", "lookupService", "$stateParams", "$state", "formService", 'assessmentService', "$rootScope", '$q', 'registrationService', '$location', '$anchorScroll', 'collateralService',
        function ($scope, $filter, ifspService, alertService, lookupService, $stateParams, $state, formService, assessmentService, $rootScope, $q, registrationService, $location, $anchorScroll, collateralService) {
            $scope.inputType = {
                Button: 1,
                Checkbox: 2,
                Radio: 3,
                Textbox: 4,
                Select: 5,
                MultiSelect: 6,
                None: 7,
                DatePicker: 8,
                TextArea: 9
            };

            $scope.ifspAssessmentID = 3;
            $scope.collateralContactTypeID = CONTACT_TYPE.Family_Relationship;

            $scope.init = function () {
                $scope.AssessmentID = $scope.ifspAssessmentID; //TODO: For the love of God ...
                $scope.ContactID = $stateParams.ContactID;
                $scope.initLookups();
                $scope.setDefaultDatePickerSettings();
                $scope.newIFSP();
                $scope.ifsp.IFSPTypeID = parseInt($stateParams.IFSPTypeID);
                $scope.initMembers();
                $scope.collaterals = [];
                $scope.selectedParentGuardians = [];
                $scope.DefaultParentGuardians = [];
                $scope.initParentGuardians();
                $scope.newResponse = {};
                $scope.ifspTable = $("#ifspTable");
                $scope.initializeBootstrapTable();
                $scope.getIFSPMembers();
                $scope.getIFSPParentGuardians();
                $scope.MembersChanged = false; // b/c form is not getting made dirty when the selected members change
                $scope.ParentGuardiansChanged = false; // b/c form is not getting made dirty when the selected members change
                $scope.getList();
            };

            $scope.initHeader = function () {
                $scope.ContactID = $stateParams.ContactID;
                $scope.get($stateParams.IFSPID);
                $location.hash('top');
                $anchorScroll();
            };

            $scope.saveHeader = function (isNext, mandatory, hasErrors, keepForm) {
                $scope.save(isNext, mandatory, hasErrors, true, $scope.headerNext);
            };

            $scope.headerNext = function () {
                var nextState = $("li[data-state-name='patientprofile.chart.ifsps.ifsp.header']").next();
                $state.go(nextState.attr('data-state-name'), $.extend({}, $stateParams, { SectionID: Math.abs(nextState.attr('data-state-key')), ResponseID: $scope.ifsp.ResponseID }));
            };

            $scope.initReport = function () {
                $scope.isReportReady = false;

                $scope.ContactID = $stateParams.ContactID;
                registrationService.get($scope.ContactID).then(function (data) {
                    $scope.reportModel = {
                        childName: data.DataItems[0].FirstName + ' ' + data.DataItems[0].LastName,
                        childDOB: $filter('toMMDDYYYYDate')(data.DataItems[0].DOB, 'MM/DD/YYYY'),
                        clientID: data.DataItems[0].ContactID < 0 ? 'Pending' : data.DataItems[0].MRN.toString()
                    };

                    $scope.get($stateParams.IFSPID).then(function () {
                        $scope.reportModel.ifspDate = $scope.ifsp.IFSPMeetingDate;

                        $scope.reportModel.parentGuardian = '';
                        // build parent/guardian comma delimited list
                        ifspService.getIFSPParentGuardians($scope.ContactID).then(function (parentGuardians) {
                            angular.forEach(parentGuardians.DataItems, function (parentGuardian) {
                                // we only care about the parent guardians for this particular ifsp report
                                if (parentGuardian.IFSPID === $stateParams.IFSPID)
                                    $scope.reportModel.parentGuardian += $scope.reportModel.parentGuardian === '' ? parentGuardian.Name : ', ' + parentGuardian.Name;
                            });
                        });

                        assessmentService.getAssessmentSections($scope.AssessmentID).then(function (data) {
                            var promises = [];
                            angular.forEach(data.DataItems, function (section) {
                                var deferred = $q.defer();
                                promises.push(deferred.promise);
                                assessmentService.getAssessmentQuestions(section.AssessmentSectionID).then(function (qData) {
                                    assessmentService.getAssessmentResponseDetails($stateParams.ResponseID, section.AssessmentSectionID).then(function (rData) {
                                        rData = rData.data;
                                        angular.forEach(rData.DataItems, function (responseDetail) {
                                            var inputTypeId = $filter('filter')(qData.DataItems, function (question) { return parseInt(question.QuestionID) === parseInt(responseDetail.QuestionID); })[0].InputTypeID;
                                            if (inputTypeId === $scope.inputType.Textbox || inputTypeId === $scope.inputType.TextArea || inputTypeId === $scope.inputType.DatePicker) {
                                                if (!(responseDetail.QuestionID in $scope.reportModel))
                                                    $scope.reportModel[responseDetail.QuestionID] = {};
                                                $scope.reportModel[responseDetail.QuestionID][responseDetail.OptionsID] = responseDetail.ResponseText;
                                            } else if (inputTypeId === $scope.inputType.Radio || inputTypeId === $scope.inputType.Select) {
                                                $scope.reportModel[responseDetail.QuestionID] = responseDetail.OptionsID;
                                            } else if (inputTypeId === $scope.inputType.Checkbox || inputTypeId === $scope.inputType.MultiSelect) {
                                                if (!(responseDetail.QuestionID in $scope.reportModel))
                                                    $scope.reportModel[responseDetail.QuestionID] = {};
                                                $scope.reportModel[responseDetail.QuestionID][responseDetail.OptionsID] = true;
                                            }
                                        });
                                        deferred.resolve();
                                    });
                                });
                            });
                            $q.all(promises).finally(function () {
                                $scope.isReportReady = true;
                            });
                        });
                    });
                });
            };

            var checkFormStatus = function () {
                $scope.$watch('ctrl.ifspForm.$valid', function (newValue) {
                    $rootScope.$broadcast('patientprofile.ifsps', { validationState: ($scope.ContactID !== 0 || newValue) ? 'valid' : 'invalid' });
                });
            };

            $scope.initLookups = function () {
                //TODO: Update UI to populate from variables populated
                $scope.IFSPTypeList = lookupService.getLookupsByType('IFSPType');
            };

            $scope.setDefaultDatePickerSettings = function () {
                $scope.opened = false;
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
                $scope.format = $scope.formats[3];
            };

            $scope.initMembers = function () {
                $scope.Members = [{ UserID: 0, Name: '' }];
                $scope.selectedMembers = [{ UserID: 0, Name: '', CredentialAbbreviation: '' }];
                $scope.resetForm();
            };

            $scope.initParentGuardians = function () {
                collateralService.get($scope.ContactID, $scope.collateralContactTypeID, false).then(function (data) {
                    $scope.ParentGuardians = [{ ContactID: 0, Name: '' }];
                    angular.forEach(data.DataItems, function (coll) {
                        var item = { ContactID: coll.ContactID, Name: coll.FirstName + ' ' + coll.LastName }
                        $scope.collaterals.push(item);

                        // default Parent/Guardians that are living w/ this contact
                        if (coll.LivingWithClientStatus) {
                            $scope.selectedParentGuardians.push(item);
                            $scope.DefaultParentGuardians.push(item); // to persist the full defaulted list
                        }
                    });
                });
            }

            $scope.resetForm = function () {
                if ($scope != null && $scope.ctrl != null && $scope.ctrl.ifspForm != null)
                    $rootScope.formReset($scope.ctrl.ifspForm, $scope.ctrl.ifspForm.name);
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.selectMemberName = function (item) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedMembers.length; i++) {
                    if ($scope.selectedMembers[i].UserID === item.ID) {
                        idx = i;
                    }
                }
                if (idx === -1) {
                    $scope.selectedMembers.push({ UserID: item.ID, Name: item.Name, CredentialAbbreviation: item.CredentialAbbreviation });
                    $scope.MembersChanged = true;
                }

                $scope.Members = [{ UserID: 0, Name: '' }];
            };

            $scope.removeMember = function (member) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedMembers.length; i++) {
                    if ($scope.selectedMembers[i].UserID === member.UserID) {
                        idx = i;
                    }
                }
                if (idx > -1) {
                    $scope.selectedMembers.splice(idx, 1);
                    $scope.MembersChanged = true;
                }
            };

            $scope.selectParentGuardian = function (item) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedParentGuardians.length; i++) {
                    if ($scope.selectedParentGuardians[i].ContactID === item.ContactID) {
                        idx = i;
                    }
                }
                if (idx === -1) {
                    $scope.selectedParentGuardians.push({ ContactID: item.ContactID, Name: item.Name });
                    $scope.ParentGuardiansChanged = true;
                }

                $scope.ParentGuardians = [{ ContactID: 0, Name: '' }];
            };

            $scope.removeParentGuardian = function (parentGuardian) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedParentGuardians.length; i++) {
                    if ($scope.selectedParentGuardians[i].ContactID === parentGuardian.ContactID) {
                        idx = i;
                    }
                }
                if (idx > -1) {
                    $scope.selectedParentGuardians.splice(idx, 1);
                    $scope.ParentGuardiansChanged = true;
                }
            };

            $scope.getList = function () {
                $scope.isLoading = true;
                return ifspService.getList($scope.ContactID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.ifspList = data.DataItems;
                        $scope.ifspTable.bootstrapTable('load', $scope.ifspList);
                    } else {
                        $scope.ifspList = [];
                        $scope.ifspTable.bootstrapTable('removeAll');
                    }
                    $scope.resetForm();
                    checkFormStatus();
                },
                    function (errorStatus) {
                        alertService.error('Unable to get ifsp: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.get = function (ifspID) {
                $scope.isLoading = true;

                return ifspService.get($scope.ContactID, ifspID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {

                        var dataPromises = [];

                        if ($scope.ifspMembers === undefined) {
                            dataPromises.push($scope.getIFSPMembers());
                        }
                        if ($scope.ifspParentGuardians === undefined) {
                            dataPromises.push($scope.getIFSPParentGuardians());
                        }

                        $q.all(dataPromises).finally(function () {
                            $scope.prepRowEditData(data.DataItems[0]);
                        });

                    } else {
                        alertService.error('Unable to get IFSP!');
                    }

                    $scope.resetForm();
                    checkFormStatus();
                },
                function (errorStatus) {
                    alertService.error('Unable to get IFSP: ' + errorStatus);
                }).finally(function () {
                    $scope.isLoading = false;
                });
            };


            $scope.editIFSP = function () {
                $scope.isNext = false;
            }

            $scope.editAssessment = function (ifspID, assessmentID, responseID, event) {
                event.stopPropagation();
                $scope.navigateToIfspAssessment(ifspID, assessmentID, responseID);
            }

            $scope.navigateToIfspAssessment = function (ifspID, assessmentID, responseID) {
                // Create params object
                var params = {
                    ContactID: $scope.ContactID,
                    IFSPID: ifspID,
                    AssessmentID: assessmentID,
                    ResponseID: responseID
                };

                // Use the assessment service to navigate to correct section
                assessmentService.navigateToSection('patientprofile.chart.ifsps.ifsp.section', params);
            }

            $scope.print = function (ifspID, responseID, event) {
                event.stopPropagation();
                $state.go('patientprofile.chart.ifsps.ifsp.report', $.extend({}, $stateParams, { ContactID: $scope.ContactID, IFSPID: ifspID, ResponseID: responseID }));
            };

            $scope.getIFSPMembers = function () {
               return ifspService.getIFSPMembers($scope.ContactID).then(function (data) {
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.ifspMembers = data.DataItems;
                        }
                    }
                });
            };

            $scope.getIFSPParentGuardians = function () {
                return ifspService.getIFSPParentGuardians($scope.ContactID).then(function (data) {
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined) {
                            $scope.ifspParentGuardians = data.DataItems;
                        }
                    }
                });
            };

            $scope.initializeBootstrapTable = function () {
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
                            field: "IFSPMeetingDate",
                            title: "Meeting Date",
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter("toMMDDYYYYDate")(value, 'MM/DD/YYYY', 'useLocal');
                                    return formattedDate;
                                } else {
                                    return "";
                                }
                            }
                        },
                        {
                            field: "IFSPType",
                            title: "IFSP Type"
                        },
                        {
                            field: "IFSPFamilySignedDate",
                            title: "Completed Date",
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter("toMMDDYYYYDate")(value);
                                    return formattedDate;
                                } else {
                                    return "";
                                }
                            }
                        },
                        {
                            field: "IFSPID",
                            title: "",
                            formatter: function (value, row, index) {
                                return (
                                    '<span class="text-nowrap">' +
                                        
                                    '<a data-default-action ng-click="editAssessment(' + value + ',' + row.AssessmentID + ',' + row.ResponseID + ', $event)" ' +
                                    'alt="Edit IFSP" security permission-key="ECI-IFSP-IFSP" permission="update" space-key-press>' +
                                    '<i  title="Edit" class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') +

                                    '<a href="javascript:void(0)" data-default-no-action ng-click="print(' + value + ', ' + row.ResponseID + ', $event)" id="printIFSP" name="printIFSP" title="Print" security permission-key="ECI-IFSP-IFSP" permission="read" ' +
                                    'space-key-press><i class="fa fa-print fa-fw"></i></a>' +

                                    '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ', $event)" id="deleteScreening" name="deleteScreening" title="Delete" security permission-key="ECI-IFSP-IFSP" permission="delete" ' +
                                    ' space-key-press><i class="fa fa-trash fa-fw"></i></a>' +

                                    '</span>';
                            }
                        }
                    ]
                };
            };

            $scope.remove = function (ifspID, event) {
                event.stopPropagation();
                bootbox.confirm('Selected IFSP will be removed.\n Do you want to continue?', function (confirmed) {
                    if (confirmed) {
                        var tempIFSPList = $scope.ifspList;
                        $scope.ifspList = $filter('filter')($scope.ifspList, { IFSPID: '!' + ifspID });
                        ifspService.remove($scope.ContactID, ifspID).then(function (response) {
                            if (response.ResultCode === 0) {
                                alertService.success('IFSP has successfully been deleted.');
                                $scope.getList();
                                $scope.newIFSP();
                            } else {
                                alertService.error('Unable to delete IFSP.');
                                $scope.ifspList = tempIFSPList;
                            }
                        });
                    }
                });
            };

            $scope.next = function () {
                if ($scope.ContactID != 0 &&
                    $scope.ContactID != undefined &&
                    $scope.ContactID != null &&
                    $scope.ifsp.IFSPID != 0 &&
                    $scope.ifsp.IFSPID != undefined &&
                    $scope.ifsp.IFSPID) {

                    $scope.navigateToIfspAssessment($scope.ifsp.IFSPID, $scope.ifsp.AssessmentID, $scope.ifsp.ResponseID);
                } else {
                    alertService.error('Please select an IFSP with an IFSP Type provided, before proceeding to the next screen');
                }
            };

            $scope.saveIFSP = function (isAdd) {
                if (isAdd) {
                    var deferred = $q.defer();
                    $scope.ensureResponse().then(function (responseID) {
                        $scope.ifsp.ResponseID = responseID;
                        ifspService.add($scope.ifsp).then(function (response) {
                            $scope.newResponse = {};
                            deferred.resolve(response);
                        },
                            function () {
                                deferred.reject();
                            });
                    }, function () {
                        deferred.reject();
                    });
                    return deferred.promise;
                } else {
                    return ifspService.update($scope.ifsp);
                }
            };

            $scope.ensureResponse = function () {
                var deferred = $q.defer();
                if (($scope.newResponse !== undefined) && (($scope.newResponse.ResponseID !== 0) && ($scope.newResponse.AssessmentID === $scope.AssessmentID))) {
                    $scope.promiseNoOp().then(function () {
                        deferred.resolve($scope.newResponse.ResponseID);
                    });
                } else {
                    var assessmentResponse = {
                        ResponseID: 0,
                        ContactID: $scope.ContactID,
                        AssessmentID: $scope.AssessmentID
                    };
                    assessmentService.addAssessmentResponse(assessmentResponse).then(function (response) {
                        if (response.data.ResultCode === 0) {
                            $scope.newResponse = { ResponseID: response.data.ID, AssessmentID: $scope.AssessmentID };
                            deferred.resolve(response.data.ID);
                        } else {
                            deferred.reject();
                        }
                    }, function () {
                        deferred.reject();
                    });
                }
                return deferred.promise;
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                if (isNext && next === undefined) {
                    next = function () { $scope.next(); }
                }
                if (!mandatory && isNext && hasErrors) {
                    next();
                }

                if (!hasErrors) {
                    $scope.ifsp.Members = $filter('filter')($scope.selectedMembers, function (item) {
                        if (item.UserID !== 0) {
                            return item;
                        }
                    });

                    $scope.ifsp.ParentGuardians = $filter('filter')($scope.selectedParentGuardians, function (item) {
                        if (item.ContactID !== 0) {
                            return item;
                        }
                    });

                    if (formService.isDirty($scope.ctrl.ifspForm.name) || $scope.MembersChanged || $scope.ParentGuardiansChanged) {
                        var isAdd = ($scope.ifsp.IFSPID === 0 || $scope.ifsp.IFSPID === undefined);
                        var lstTxtById = lookupService.getSelectedTextById($scope.ifsp.IFSPTypeID, $scope.IFSPTypeList);

                        if (lstTxtById != undefined && lstTxtById.length > 0)
                            $scope.ifsp.IFSPType = lstTxtById[0].Name;

                        if ($scope.ifsp.Members.length > 0) {
                            $scope.saveIFSP(isAdd).then(function (response) {
                                if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                                    if (('data' in response)) {
                                        if (response.data.ResultCode == 0) {
                                            if ($scope.ifsp != undefined && $scope.ifsp.IFSPID != undefined) {
                                                var successMessage = 'IFSP has been ' + (isAdd ? 'added' : 'updated') + ' successfully.';
                                            }
                                            $scope.ifsp.IFSPID = (($scope.ifsp !== undefined) && ($scope.ifsp.IFSPID !== undefined) && ($scope.ifsp.IFSPID != 0))
                                                ? $scope.ifsp.IFSPID : response.data.ID;
                                            alertService.success(successMessage);
                                            $scope.MembersChanged = false;
                                            $scope.ctrl.ifspForm.$setPristine();

                                            ifspService.getList($scope.ContactID).then(function () {
                                                if (!keepForm) {
                                                    $scope.postSave(isAdd);
                                                    $scope.getIFSPMembers();
                                                    $scope.getIFSPParentGuardians();
                                                }
                                                if (isNext)
                                                    next();
                                            });
                                        } else {
                                            alertService.error('Unable to save IFSP');
                                        }
                                    }
                                }
                            });
                        } else {
                            alertService.error('Please add a team member');
                        }

                    } else if (isNext) {
                        next();
                    }
                }
            };

            $scope.postSave = function (isAdd) {
                $scope.getList().then(function () {
                    if (isAdd) // we only want to clear the screen and reset focus is the user was adding a new IFSP
                        $scope.newIFSP();
                });
            };

            $scope.newIFSP = function () {
                $scope.resetForm();
                $scope.$parent['autoFocusifspMeetingDate'] = true;
                $scope.ifsp = {
                    ContactID: $scope.ContactID,
                    IFSPID: 0,
                    AssessmentID: $scope.AssessmentID,
                    IFSPTypeID: 0,
                    IFSPType: '',
                    IFSPMeetingDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY'),
                    IFSPFamilySignedDate: null,
                    MeetingDelayed: false,
                    ReasonForDelayID: null,
                    Comments: '',
                    ResponseID: 0,
                    Members: [],
                    ParentGuardians: []
                };
                $scope.selectedMembers = [];
                $scope.selectedParentGuardians = [];
                $scope.noParentGuardianResults = false;
                angular.forEach($scope.DefaultParentGuardians, function (item) { $scope.selectedParentGuardians.push({ ContactID: item.ContactID, Name: item.Name }); });
            };

            $scope.$watch(function () { return $scope.ctrl.ifspForm }, function () {
                $scope.ctrl.ifspForm.parentGuardian.$setValidity("parse",true);               
            }, true);

            $scope.prepRowEditData = function (row) {
                $scope.ifsp = row;
                $scope.ifsp.IFSPMeetingDate = $filter('toMMDDYYYYDate')($scope.ifsp.IFSPMeetingDate, 'MM/DD/YYYY');
                $scope.selectedMembers = [{ UserID: 0, Name: '', CredentialAbbreviation: '' }];
                $scope.selectedParentGuardians = [{ ContactID: 0, Name: '' }];

                //load the saved members for this record
                for (var i = 0; $scope.ifspMembers && (i < $scope.ifspMembers.length) ; i++) {
                    if ($scope.ifspMembers[i].IFSPID === row.IFSPID) {
                        $scope.selectedMembers.push({ UserID: $scope.ifspMembers[i].UserID, Name: $scope.ifspMembers[i].Name, CredentialAbbreviation: $scope.ifspMembers[i].CredentialAbbreviation });
                    }
                }

                if (row.Members != undefined && row.Members !== null && row.Members.length > 0) {
                    $scope.selectedMembers = $filter('filter')(row.Members, function (item) {
                        return item;
                    });
                }

                $scope.ifsp.Members = $filter('filter')($scope.selectedMembers, function (item) {
                    if (item.UserID !== 0) {
                        return item;
                    }
                });

                //load the saved parent guardians for this record
                for (var i = 0; $scope.ifspParentGuardians && (i < $scope.ifspParentGuardians.length) ; i++) {
                    if ($scope.ifspParentGuardians[i].IFSPID === row.IFSPID) {
                        $scope.selectedParentGuardians.push({ ContactID: $scope.ifspParentGuardians[i].ContactID, Name: $scope.ifspParentGuardians[i].Name });
                    }
                }

                if (row.ParentGuardians != undefined && row.ParentGuardians !== null && row.ParentGuardians.length > 0) {
                    $scope.selectedParentGuardians = $filter('filter')(row.ParentGuardians, function (item) {
                        return item;
                    });
                }

                $scope.ifsp.ParentGuardians = $filter('filter')($scope.selectedParentGuardians, function (item) {
                    if (item.ContactID !== 0) {
                        return item;
                    }
                });

                $scope.resetForm();
                if (!$scope.$$phase) {
                    $scope.$digest();
                }
            };

            $scope.$on('showDetails', function (event, args) {
                $scope.get().then(function () {
                    setGridItem($scope.ifspTable, 'IFSPID', args.id);
                });
            });

            $scope.init();

        }]);
