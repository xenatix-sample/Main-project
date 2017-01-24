angular.module('xenatixApp')
    .controller('eligibilityDeterminationController', [
        '$scope', '$state', '$q', '$stateParams', '$modal', 'eligibilityDeterminationService', 'eciDemographicService', 'eligibilityCalculationService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$timeout',
        function ($scope, $state, $q, $stateParams, $modal, eligibilityDeterminationService, eciDemographicService, eligibilityCalculationService, alertService, lookupService, $filter, $rootScope, formService, $timeout) {

            $scope.init = function () {
                $scope.contactID = $stateParams.ContactID;
                $scope.initLookups();
                $scope.eligibilityID = 0;
                $scope.setDefaultDatePickerSettings();
                $scope.initEligibility();
                $scope.eligibilityTable = $('#eligibilityTable');
                $scope.initializeBootstrapTable();
                $scope.eligibility.EligibilityCategoryID = parseInt($stateParams.EligibilityCategoryID);
                $scope.get();
                //$scope.getContactEligibilityMembers();
            };

            $scope.initLookups = function() {
                $scope.EligibilityDurationList = lookupService.getLookupsByType('EligibilityDuration');
                $scope.EligibilityCategoryList = lookupService.getLookupsByType('EligibilityCategory');
                $scope.EligibilityTypeList = lookupService.getLookupsByType('EligibilityType');
                $scope.MemberList = lookupService.getLookupsByType('Users');
            };

            $scope.initEligibility = function () {
                $scope.eligibility = {};
                $scope.eligibility.ContactID = $scope.contactID;
                $scope.eligibility.EligibilityDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY');
                $scope.initMembers();
            };

            $scope.initMembers = function () {
                $scope.Members = [{ UserID: 0, Name: '' }];
                $scope.selectedMembers = [{ UserID: 0, Name: '', CredentialAbbreviation: '' }];
                //$scope.contactEligibilityMembers = [{ UserID: 0, Name: '', CredentialAbbreviation: '' }];
            };

            $scope.reset = function () {
                if ($scope.ctrl.eligibilityForm !== undefined && $scope.ctrl.eligibilityForm !== null) {
                $rootScope.formReset($scope.ctrl.eligibilityForm, $scope.ctrl.eligibilityForm.name);
                }
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
                }

                //Clear the typeahead
                $scope.Members = [{UserID: 0, Name: ''}];
            };

            $scope.removeMember = function(member) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedMembers.length; i++) {
                    if ($scope.selectedMembers[i].UserID === member.UserID) {
                        idx = i;
                    }
                }
                if (idx > -1) {
                    $scope.selectedMembers.splice(idx, 1);
                }
            };

            $scope.get = function () {
              return  eligibilityDeterminationService.get($scope.contactID).then(function (data) {
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.eligibilityTable.bootstrapTable('load', data.DataItems);
                           
                            //$scope.eligibility.AdjustedAge = $filter('toAdjustedAge')(data.DataItems[0].DOB);
                        } else {
                            $scope.eligibilityTable.bootstrapTable('removeAll');
                           
                        }
                    } else {
                        alertService.error('Error while getting eligibility data');
                    }
                    if (data.ResultMessage === 'OFFLINE') {
                        //do anything special needed when offline
                    }
                    $scope.reset();
                },
                function (errorStatus) {
                    alertService.error('Error while getting eligibility data: ' + errorStatus);
                });
            };

            $scope.getEligibility = function (eligibilityID) {
                eligibilityDeterminationService.getEligibility($scope.contactID, eligibilityID).then(function (data) {
                    if (data.ResultCode === 0) {
                        if(data != undefined && data.DataItems != undefined && data.DataItems.length > 0)
                        {
                            $scope.eligibilityID = data.DataItems[0].EligibilityID;
                            //emulate the action done when selecting a single record on the management screen
                            $scope.prepRowEditData(data.DataItems[0]);
                        }
                        else {
                            alertService.error('Unable to get eligibility data');
                        }
                    }
                    else {
                        alertService.error('Error while getting eligibility data');
                    }
                    if (data.ResultMessage === 'OFFLINE') {
                        //do anything special needed when offline
                    }
                },
                function (errorStatus) {
                    alertService.error('Error while getting eligibility data: ' + errorStatus);
                });
            };

            getText = function (value, list) {
                if (value) {
                    var formattedValue = lookupService.getSelectedTextById(value, list);
                    if (formattedValue != undefined && formattedValue.length > 0) {
                        return formattedValue[0].Name;
                    } else
                        return '';
                } else
                    return '';
            };

            //split the successful result methods into a single call in the then function
            $scope.saveEligibility = function (isNext) {
                if (!("EligibilityID" in $scope.eligibility))
                    $scope.eligibility.EligibilityID = 0;

                if ($scope.eligibility.EligibilityID === 0 || $scope.eligibility.EligibilityID === undefined) {
                    eligibilityDeterminationService.add($scope.eligibility).then(function (data) {
                        if (data.data.ResultMessage === 'OFFLINE') {
                            
                        }

                        if (data.data.ResultCode !== 0) {
                            alertService.error('Error while saving eligibility data');
                        } else {
                            alertService.success('Eligibility determination saved successfully');
                            if (isNext) {
                                $scope.edit(data.data.ID, null); //go to the calc screen
                            }
                            $scope.reset();
                            $scope.init();    
                        }
                    },
                    function (errorStatus) {
                        alertService.error('Error while eligibility saving data: ' +errorStatus);
                    });
                } else {
                    eligibilityDeterminationService.update($scope.eligibility).then(function (data) {
                        if (data.data.ResultMessage === 'OFFLINE') {
                        
                        }

                        if (data.data.ResultCode !== 0) {
                            alertService.error('Error while saving eligibility data');
                        } else {
                            alertService.success('Eligibility determination saved successfully');
                            if (isNext) {
                                $scope.edit($scope.eligibility.EligibilityID, null); //go to the calc screen
                            }
                            $scope.reset();
                        }
                    },
                    function (errorStatus) {
                        alertService.error('Error while saving eligibility data: ' +errorStatus);
                    });
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!hasErrors) {
                    $scope.eligibility.Members = [{}];//declare property before adding any members
                    $scope.eligibility.Members.splice(0, $scope.eligibility.Members.length);
                    $filter('filter') ($scope.selectedMembers, function(item) {
                        if (item.UserID > 0) {
                            $scope.eligibility.Members.push(item.UserID);
                        }
                    });

                    if ($scope.eligibility.Members.length > 0) {
                        $scope.saveEligibility(isNext);
                    } else {
                        alertService.error('Please add a team member');
                    }
                }
            };

            $scope.prepRowEditData = function (row) {
                //load the saved members for this eligibility record
                $scope.selectedMembers = [{ UserID: 0, Name: '', CredentialAbbreviation: '' }];

                //use $scope.MemberList to get the rest of the data
                $filter('filter')(row.Members, function (item) {
                    var memberData = $scope.MemberList[$scope.MemberList.map(function (el) { return el.ID; }).indexOf(item)];
                    $scope.selectedMembers.push({ UserID: memberData.ID, Name: memberData.Name, CredentialAbbreviation: memberData.CredentialAbbreviation });
                });
                $scope.eligibility = row;
                $scope.eligibility.EligibilityDate = $filter('toMMDDYYYYDate')($scope.eligibility.EligibilityDate, 'MM/DD/YYYY');

                $scope.reset();
                $scope.$digest();
            };

            $scope.edit = function (eligibilityID, event) {
                if (event !== null && event !== undefined) {
                    event.stopPropagation();
                }
                $state.go("patientprofile.chart.eligibilities.eligibility.calculation", {EligibilityID: eligibilityID});
            };

            $scope.view = function (eligibilityID, event) {
                $state.go("patientprofile.chart.eligibilities.eligibility.report", { EligibilityID: eligibilityID });
            };

            $scope.deactivate = function (eligibilityID, event) {
                event.stopPropagation();
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        eligibilityDeterminationService.deactivate($scope.contactID, eligibilityID).then(function (response) {
                            if (response.ResultCode === 0) {
                                alertService.success('Eligibility has successfully been deactivated.');
                                $scope.get();
                            } else {
                                alertService.error('Error while deactivating the eligibility record.');
                            }
                        });
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
                            field: 'EligibilityDate',
                            title: 'Eligibility Date',
                            formatter: function (value, row, index) {
                                return $filter('toMMDDYYYYDate')(row["EligibilityDate"], 'MM/DD/YYYY', 'useLocal');
                            }
                        },
                        {
                            field: 'EligibilityTypeID',
                            title: 'Type',
                            formatter: function (value, row, index) {
                                return getText(value, $scope.EligibilityTypeList);
                            }
                        },
                        {
                            field: 'EligibilityCategoryID',
                            title: 'Category',
                            formatter: function (value, row, index) {
                                return getText(value, $scope.EligibilityCategoryList);
                            }
                        },
                        {
                            field: 'DOB',
                            title: 'Adjusted Age',
                                formatter: function (value, row, index) {
                                    if (value === null || value === undefined) {
                                        //for offline viewing
                                        return $filter('toAdjustedAge')($scope.header.DOB);
                                    } else {
                                        return $filter('toAdjustedAge') (value);
                                    }       
                            }
                        },
                        {
                            field: 'EligibilityDurationID',
                            title: 'Duration',
                            formatter: function (value, row, index) {
                                return getText(value, $scope.EligibilityDurationList);
                            }
                        },
                        {
                            field: 'EligibilityID',
                            title: '',
                            formatter: function (value, row, index) {
                                return (
                                    '<span class="text-nowrap">' +

                                    '<a data-default-action ui-sref="patientprofile.chart.eligibilities.eligibility.calculation(' +
                                    '{ EligibilityID: ' + value + '})" ' +
                                    'alt="Edit Eligibility" security permission-key="ECI-Eligibility-Eligibility" permission="update" space-key-press>' +
                                    '<i  title="Edit" class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') +

                                    '<a href="javascript:void(0)" data-default-no-action ng-click="view(' + value + ', $event)" id="viewEligibility" name="viewEligibility" title="Print View"' +
                                    'space-key-press><i class="fa fa-print fa-fw"></i></a>'
                                    +
                                    '<a href="javascript:void(0)" data-default-no-action ng-click="deactivate(' + value + ', $event)" id="deleteEligibility" name="deleteEligibility" title="Deactivate" ' +
                                    'space-key-press><i class="fa fa-trash fa-fw"></i></a>' +

                                    '</span>';
                            }
                        }
                    ]
                };
            };

            $scope.setDefaultDatePickerSettings = function () {
                $scope.opened = false;
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.formats =['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
                $scope.format = $scope.formats[3];
                $scope.endDate = new Date();
                $scope.startDate = new Date();
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.initHeader = function () {
                $scope.ContactID = $stateParams.ContactID;
                $scope.getEligibility($stateParams.EligibilityID);
            };

            $scope.initNotes = function () {
                $scope.eligibilityID = 0;
                //make call to get notes using eligibilityid
                //set model value $scope.eligibility.Notes
                $('#notes').focus();
                eligibilityDeterminationService.getEligibilityNote($stateParams.EligibilityID).then(function (data) {
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.eligibility.Notes = data.DataItems[0].Notes;
                            if ($scope.eligibility.Notes != null) {
                                $scope.eligibilityID = data.DataItems[0].EligibilityID;
                            }
                            else {
                                $scope.eligibilityID = 0;
                            }
                        }
                    }
                    else {
                        alertService.error('Error while loading eligibility notes');
                    }
                    if (data.ResultMessage === 'OFFLINE') {
                        //do anything special needed when offline
                    }

                    $scope.ctrl.eligibilityForm.$setPristine();
                },
                function (errorStatus) {
                    alertService.error('Error while loading eligibility notes: ' + errorStatus);
                });
            };

            $scope.saveEligibilityNote = function (isNext, isMandatory, hasErrors) {
                if(!hasErrors){
                    $scope.eligibility.EligibilityID = $stateParams.EligibilityID;
                    eligibilityDeterminationService.saveEligibilityNote($scope.eligibility).then(function (data) {
                        if (data.data.ResultCode === 0) {
                            alertService.success('Eligibility note saved sucessfully');
                            if (isNext) {
                                $state.go("patientprofile.chart.eligibilities.eligibility.report", {EligibilityID: $stateParams.EligibilityID});
                            }
                        }
                        else {
                            alertService.error('Error while saving eligibility notes');
                        }
                        if (data.ResultMessage === 'OFFLINE') {
                            //do anything special needed when offline
                        }
                        $scope.ctrl.eligibilityForm.$setPristine();
                    },
                   function (errorStatus) {
                       alertService.error('Error while saving eligibility notes: ' + errorStatus);
                   });
                }
            };

            $scope.setAgeFields = function (dob, gestationalAge) {
                var today = moment();
                var tmpdob = moment(dob);
                var ageInMonths = today.diff(tmpdob, 'months');
                $scope.reportModel.ChronologicalAge = ageInMonths;
                $scope.reportModel.AdjustedAge = ageInMonths;
                $scope.calculateGestationalAge();
            };

            $scope.initReport = function() {
                //get the data to populate the report
                $scope.reportModel = {};

                //fields from the header
                $scope.reportModel.FullName = $scope.header.FullName;
                //chronologicalage and AdjustedAge

                //notes
                eligibilityDeterminationService.getEligibilityNote($stateParams.EligibilityID).then(function(data) {
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.reportModel.Notes = data.DataItems[0].Notes || '';
                        } else {
                            $scope.reportModel.Notes = '';
                        }
                    }

                    var promises = [];

                    promises.push(
                    //get eligibility
                        eligibilityDeterminationService.getEligibility($stateParams.ContactID, $stateParams.EligibilityID).then(function (data) {
                            if (data.ResultCode === 0) {
                                if (data.DataItems != undefined && data.DataItems.length > 0) {
                                    $scope.reportModel.EligibilityDate = $filter('toMMDDYYYYDate')(data.DataItems[0].EligibilityDate, 'MM/DD/YYYY');
                                    $scope.reportModel.Duration = getText(data.DataItems[0].EligibilityDurationID, $scope.EligibilityDurationList);
                                    $scope.reportModel.EligibilityCategory = getText(data.DataItems[0].EligibilityCategoryID, $scope.EligibilityCategoryList);

                                    //set team members
                                    $scope.loadTeamMembers(data.DataItems[0].Members);
                                }
                            }
                        })
                    );//geteligibility

                    promises.push(
                        //eci demographic svc - gestationalage/adj/gest
                        eciDemographicService.get($stateParams.ContactID).then(function (regdata) {
                            if((regdata.DataItems && regdata.DataItems.length === 1)) {
                                $scope.reportModel.DOB = $filter('toMMDDYYYYDate')(regdata.DataItems[0].DOB, 'MM/DD/YYYY');
                                $scope.reportModel.GestationalAge = regdata.DataItems[0].GestationalAge;
                            }

                            $scope.setAgeFields($scope.reportModel.DOB, $scope.reportModel.GestationalAge);
                        })
                    );

                    promises.push(
                        //getcalculations
                         eligibilityCalculationService.get($stateParams.EligibilityID).then(function (data) {
                            if (data.ResultCode === 0) {
                                if (data.DataItems.length > 0) {
                                    //populate all of the calculation-based fields
                                    $scope.reportModel.AAE = data.DataItems[0].AAE || '';
                                    $scope.reportModel.AIAE = data.DataItems[0].AIAE || '';
                                    $scope.reportModel.AIRS = data.DataItems[0].AIRS || '';
                                    $scope.reportModel.AMD = data.DataItems[0].AMD || '';
                                    $scope.reportModel.AMRS = data.DataItems[0].AMRS || '';
                                    $scope.reportModel.APD = data.DataItems[0].APD || '';
                                    $scope.reportModel.CD = data.DataItems[0].CD || '';
                                    $scope.reportModel.CPD = data.DataItems[0].CPD || '';
                                    $scope.reportModel.CognitiveAE = data.DataItems[0].CognitiveAE || '';
                                    $scope.reportModel.CommAE = data.DataItems[0].CommAE || '';
                                    $scope.reportModel.CommMD = data.DataItems[0].CommMD || '';
                                    $scope.reportModel.CommPD = data.DataItems[0].CommPD || '';
                                    $scope.reportModel.ECAE = data.DataItems[0].ECAE || '';
                                    $scope.reportModel.ECMD = data.DataItems[0].ECMD || '';
                                    $scope.reportModel.ECPD = data.DataItems[0].ECPD || '';
                                    $scope.reportModel.ECRS = data.DataItems[0].ECRS || '';
                                    $scope.reportModel.FMAE = data.DataItems[0].FMAE || '';
                                    $scope.reportModel.FMRS = data.DataItems[0].FMRS || '';
                                    $scope.reportModel.FPMAE = data.DataItems[0].FPMAE || '';
                                    $scope.reportModel.FPMD = data.DataItems[0].FPMD || '';
                                    $scope.reportModel.FPMPD = data.DataItems[0].FPMPD || '';
                                    $scope.reportModel.GMAE = data.DataItems[0].GMAE || '';
                                    $scope.reportModel.GMPD = data.DataItems[0].GMPD || '';
                                    $scope.reportModel.GMRS = data.DataItems[0].GMRS || '';
                                    $scope.reportModel.PCAE = data.DataItems[0].PCAE || '';
                                    $scope.reportModel.PCRS = data.DataItems[0].PCRS || '';
                                    $scope.reportModel.PMAE = data.DataItems[0].PMAE || '';
                                    $scope.reportModel.PMRS = data.DataItems[0].PMRS || '';
                                    $scope.reportModel.PersonalSocialAE = data.DataItems[0].PersonalSocialAE || '';
                                    $scope.reportModel.PersonalSocialMD = data.DataItems[0].PersonalSocialMD || '';
                                    $scope.reportModel.PersonalSocialPD = data.DataItems[0].PersonalSocialPD || '';
                                    $scope.reportModel.RAAE = data.DataItems[0].RAAE || '';
                                    $scope.reportModel.RARS = data.DataItems[0].RARS || '';
                                    $scope.reportModel.RCAE = data.DataItems[0].RCAE || '';
                                    $scope.reportModel.RCRS = data.DataItems[0].RCRS || '';
                                    $scope.reportModel.SCAE = data.DataItems[0].SCAE || '';
                                    $scope.reportModel.SCRS = data.DataItems[0].SCRS || '';
                                    $scope.reportModel.SRAE = data.DataItems[0].SRAE || '';
                                    $scope.reportModel.SRRS = data.DataItems[0].SRRS || '';
                                }
                            }  
                        }));

                     $q.all(promises).finally(function() {$scope.reportModel.HasLoaded = true;});
                });//notes
            };

            $scope.loadTeamMembers = function (eligibilityMembers) {
                $scope.reportModel.TeamMembers =[{ UserID: 0, Name : '', CredentialAbbreviation: ''}];
                $filter('filter')(eligibilityMembers, function (item) {
                    var memberData = $scope.MemberList[$scope.MemberList.map(function (el) { return el.ID; }).indexOf(item)];
                    $scope.reportModel.TeamMembers.push({ UserID: memberData.ID, Name: memberData.Name, CredentialAbbreviation: memberData.CredentialAbbreviation });
                });
                
                var tmpTM = "";
                angular.forEach($scope.reportModel.TeamMembers, function (item) {
                    var tmpCred = '';
                    if (item.UserID > 0) {
                        if (item.CredentialAbbreviation !== undefined && item.CredentialAbbreviation !== null) {
                                tmpCred = item.CredentialAbbreviation;
                        }
                        if (tmpTM.length === 0) {
                            if (tmpCred.length === 0) {
                                tmpTM = tmpTM + item.Name;
                            } else {
                                tmpTM = tmpTM + item.Name + '(' + tmpCred + ')';
                            }                
                        } else {
                            if (tmpCred.length === 0) {
                                tmpTM = tmpTM + ', ' + item.Name;
                            } else {
                                tmpTM = tmpTM + ', ' + item.Name + '(' + tmpCred + ')';
                            }
                        }
                    }
                });

                $scope.reportModel.TeamMembers = tmpTM;
            };

            $scope.calculateGestationalAge = function () {
                var dob = moment(dob);
                var today = moment();
                var ageInMonths = today.diff(dob, 'months');

                if ($scope.reportModel.ChronologicalAge <= 18) {
                    if ($scope.reportModel.GestationalAge !== null && $scope.reportModel.GestationalAge !== undefined && $scope.reportModel.GestationalAge > 0) {
                        if ($scope.reportModel.GestationalAge < 37) {
                            var prematureWeeks = 37 -$scope.reportModel.GestationalAge;
                            var adjustedWeeks = ($scope.reportModel.ChronologicalAge * 4) -prematureWeeks;
                            $scope.reportModel.AdjustedAge = (adjustedWeeks / 4).toFixed();
                        }
                        else {
                            $scope.reportModel.AdjustedAge = ageInMonths;
                        }
                    }
                    else {
                        $scope.reportModel.AdjustedAge = ageInMonths;
                    }
                }
            };

            //call on load
            $scope.init();
            $scope.$on('showDetails', function (event, args) {
                $scope.get().then(function () {
                            setGridItem($scope.eligibilityTable, 'EligibilityID', args.id);
                });
            });
        }
    ]);