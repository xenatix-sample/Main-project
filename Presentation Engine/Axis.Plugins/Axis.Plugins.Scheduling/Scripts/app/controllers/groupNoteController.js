angular.module('xenatixApp')
    .controller('groupNoteController', ['$scope', '$q', '$modal', '$rootScope', '$filter', 'groupSchedulingService', '$injector', '$stateParams', 'alertService', 'lookupService', 'formService', 'appointmentService', 'registrationService', 'eSignatureService', '$state',
        function ($scope, $q, $modal, $rootScope, $filter, groupSchedulingService, $injector, $stateParams, alertService, lookupService, formService, appointmentService, registrationService, eSignatureService, $state) {

            var GROUP_NOTE_TYPE = 'Group Note';
            var INDIVIDUAL_NOTE_TYPE = 'Appointment Individual Note';
            var APPT_STATUS_NO_SHOW = 'No Show';
            var APPT_STATUS_YES = 'Check In';
            var RESOURCE_TYPE_USER = 'User';
            var RESOURCE_TYPE_PROVIDER = 'Co-Provider';
            var RESOURCE_TYPE_CONTACT = 'Contact';
            var GROUP_TYPE_ID = 1;
            var MAX_ENTITYID = 3;
            var DEFAULT_ENTITYPE_ID = 2;

        $scope.init = function () {
            $scope.GroupID = $stateParams.GroupID;
            $scope.AppointmentID = null;//$stateParams.AppointmentID;
            $scope.SaveItemsCounter = 0;
            $scope.GroupMembers = [];       // Group members w/Present flag, Name, Program Unit, ContactID, IndividualNote

            // TODO: One of these will have to be removed!!
            $scope.GroupNoteIDs = [];       // List of appt note ids, one for each group member - update the group note for each of these
            $scope.GroupNoteID = 0;         // ApptNoteID for the this group note 
            ///////////////////////////////////////////////

            $scope.GroupNoteText = '';      // Group note text
            $scope.IsGroupNoteDirty = false; // Signifies a change in group note text field
            $scope.ProgramUnit = '';        // Program unit name
            $scope.ProgramID = 0;           // Program id for the program unit
            $scope.GroupNoteTypeID = 0;     // Group note type id for 'Group Note'
            $scope.IsDigitallySigned = false;
            $scope.IsSignatureLocked = false;
            $scope.IsPwdMatch = true;

            // Individual note screen
            $scope.EditItem = {};
            $scope.EditItem.EditName = '';           // Name of group member being edited
            $scope.EditItem.EditNoteText = '';       // Note text of group member being edited
            $scope.EditItem.EditNoteId = 0;          // Individual note id
            $scope.EditItem.EditContactId = 0;
            $scope.EditItem.EditIsDigitallySigned = false;
            $scope.EditItem.EditIsSignatureLocked = false;
            $scope.EditItem.EditIsPwdMatch = true;
            $scope.EditItem.EditIsIndividualNoteDirty = false;

            // Get note type id for 'Group Note' from lookup
            $scope.GroupNoteTypeID = $filter('filter')(lookupService.getLookupsByType('ReferenceNoteType'), { Name: GROUP_NOTE_TYPE }, true)[0].ID;

            // Get note type id for 'Group Note' and 'Individual Group Note' from lookup
            $scope.GroupDocumentTypeID = $filter('filter')(lookupService.getLookupsByType('DocumentType'), { DocumentType: GROUP_NOTE_TYPE }, true)[0].DocumentTypeID;
            $scope.IndividualDocumentTypeID = $filter('filter')(lookupService.getLookupsByType('DocumentType'), { DocumentType: INDIVIDUAL_NOTE_TYPE }, true)[0].DocumentTypeID;

            // Get the appt status id for 'No Show' and 'Yes' from lookup
            $scope.AppointmentStatusNoShowID = $filter('filter')(lookupService.getLookupsByType('AppointmentStatus'), { AppointmentStatus: APPT_STATUS_NO_SHOW }, true)[0].AppointmentStatusID;
            $scope.AppointmentStatusYesID = $filter('filter')(lookupService.getLookupsByType('AppointmentStatus'), { AppointmentStatus: APPT_STATUS_YES }, true)[0].AppointmentStatusID;

            // Get the resource types for 'Contact', 'User' and 'Co-Provider'
            var restypes = $filter('filter')(lookupService.getLookupsByType('ResourceType'), function (item) {
                return item.Name == RESOURCE_TYPE_CONTACT || item.Name == RESOURCE_TYPE_PROVIDER || item.Name == RESOURCE_TYPE_USER;
            });
            $scope.ResourceTypeIDContact = $filter('filter')(restypes, { Name: RESOURCE_TYPE_CONTACT }, true)[0].ID;
            $scope.ResourceTypeIDs = [];
            angular.forEach(restypes, function (item, key) {
                $scope.ResourceTypeIDs.push(item.ID);
            })
            
            //// Get esigature plugin 'eSignatureService'
            //if ($injector.has('eSignatureService'))
            //    $scope.eSignatureService = $injector.get('eSignatureService');
            //else {
            //    bootbox.alert("Esignature Service is not available, please load the ESignature plugin!");
            //    return;
            //}

            // Get userDetailService 
            if ($injector.has('userDetailService'))
                $scope.userDetailService = $injector.get('userDetailService');
            else {
                bootbox.alert("User Detail Service is not available!");
                return;
            }

            // Get data
            $scope.get();
        }

        $scope.get = function () {
            $scope.isLoading = true;

            // 1. Get the appointment id from the the group id
            groupSchedulingService.getAppointmentByGroupID($scope.GroupID).then(function (response) {
                if (response && response.DataItems) {
                    if (response.DataItems.length > 0) {
                        $scope.AppointmentID = response.DataItems[0].AppointmentID;

                        // 2. Get all apptResources for appt id
                        $scope.getResourcesForApptId();

                        // 3. Get the appointment and program info
                        $scope.getAppointmentForApptId();

                        // 4. Get the group note for the GroupID
                        $scope.getGroupNote();
                    } else {
                        alertService.error('Error while loading the appointment');
                    }   
                } else {
                    alertService.error('Error while loading the appointment');
                }
            });
        }

        $scope.getSignatureStatus = function (id, type, isgroup, item) {
            // Get the signature status for a group OR individual note
            eSignatureService.getDocumentSignatures(type, id).then(function (response) {
                if (response.ResultCode === 0 && response.DataItems != null) {
                    if (response.DataItems.length > 0) {
                        if (isgroup) {
                            $scope.IsDigitallySigned = true;
                            $scope.IsSignatureLocked = true;
                        } else {
                            $scope.EditItem.EditIsDigitallySigned = true;
                            $scope.EditItem.EditIsSignatureLocked = true;
                        }
                    }
                    else 
                        if (isgroup)
                            $scope.IsDigitallySigned = false;
                        else {
                            $scope.EditItem.EditIsDigitallySigned = false;
                            $scope.EditItem.EditIsSignatureLocked = false;
                        }

                    if (!isgroup) {
                        // Set individual note $scope vars
                        $scope.EditItem.EditName = item.Name;
                        $scope.EditItem.EditNoteText = (item.IndividualNoteText == '' || item.IndividualNoteText == null) ? $scope.GroupNoteText : item.IndividualNoteText;
                        $scope.EditItem.EditNoteId = item.IndividualNoteId;
                        $scope.EditItem.EditContactId = item.ContactID;
                        $scope.EditItem.EditResourceTypeID = item.ResourceTypeID;

                        // Left side fly-out to edit the member
                        angular.element('.row-offcanvas').addClass('active');
                    }
                    $scope.isLoading = false;
                } else {
                    $scope.isLoading = false;
                    alertService.error('Error while retrieving the group note! Please reload the page and try again.');
                }
            })
        }

            $scope.getGroupNote = function() {
                // Get the group note and id
                appointmentService.getAppointmentNote($scope.AppointmentID, null, $scope.GroupID, null).then(function(response) {
                    if (response.ResultCode === 0 && response.DataItems != null) {
                        if (response.DataItems.length > 0) {
                            var obj = { stateName: $state.current.name, validationState: 'valid' };
                            $rootScope.groupAppointmentRightNavigationHandler(obj);
                            $scope.GroupNoteText = response.DataItems[0].NoteText;
                            $scope.GroupNoteID = response.DataItems[0].AppointmentNoteID;
                            $scope.getSignatureStatus($scope.GroupNoteID, $scope.GroupDocumentTypeID, true);
                        } else {
                            var obj = { stateName: $state.current.name, validationState: 'warning' };
                            $rootScope.groupAppointmentRightNavigationHandler(obj);
                        }
                    } else {
                        var obj = { stateName: $state.current.name, validationState: 'warning' };
                        $rootScope.groupAppointmentRightNavigationHandler(obj);
                        alertService.error('Error while retrieving the group note! Please reload the page and try again.');
                        $scope.isLoading = false;
                    }
                });
            };
            
        $scope.getAppointmentForApptId = function () {
            

            // Get the program id for this appointment
            groupSchedulingService.getAppointmentByGroupID($scope.GroupID).then(function (response) {
          
                
                if (response.ResultCode === 0 && response.DataItems != null && response.DataItems.length > 0) {
                    $scope.ProgramID = response.DataItems[0].ProgramID;

                    // Get the program unit from the program lookup and match against the programID
                  
                    $scope.ProgramUnit = $filter('filter')($filter('securityFilter')(lookupService.getOrganizationByDataKey('ProgramUnit'), 'ProgramUnit', 'ID', SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment), { ID: $scope.ProgramID }, true)[0].Name;
                    
                } else {
                    alertService.error('Error while retrieving the appointment! Please reload the page and try again.');
                }
            })
        }

        $scope.getResourcesForApptId = function () {
            
            // This returns list of apptResources w/resourceId being each member's
            // CONTACTID, the 'isnoshow' (PRESENT) flag for each member AND the APPTRESOURCEID of member
            groupSchedulingService.getGroupSchedulingResource($scope.GroupID).then(function (response) {
                if (response.ResultCode === 0 && response.DataItems != null) {

                        angular.forEach(response.DataItems, function (item) {

                            // Load group member data for each one IFF it's a resource type we want
                            if (jQuery.inArray(item.ResourceTypeID, $scope.ResourceTypeIDs)>=0) {

                                $scope.GroupMembers.push({
                                    ContactID: item.ResourceID,
                                    AppointmentResourceID: item.AppointmentResourceID,
                                    ResourceTypeID: item.ResourceTypeID,
                                    Present: 'Select',
                                    IsIndividualNoteDirty: false,
                                    IsPresenceDirty: false,
                                    IndividualNoteText: '',
                                    IndividualNoteId: 0,
                                    Name: '',
                                    AppointmentStatusDetailID:0
                                });
                            }
                        });


                        // Now go through and fill the rest related to contact ID/ user ID
                        angular.forEach($scope.GroupMembers, function (item) {

                            
                            // Get the appt status for each member 

                            
                            appointmentService.getAppointmentStatusDetail($scope.AppointmentID, item.AppointmentResourceID).then(function (response) {
                                if (response.ResultCode === 0 && response.DataItems != null) {
                                    if (response.DataItems.length > 0) {
                                        var id = response.DataItems[0].AppointmentStatusID;
                                        if (id == $scope.AppointmentStatusNoShowID)
                                            item.Present = APPT_STATUS_NO_SHOW;
                                        else if (id == $scope.AppointmentStatusYesID)
                                            item.Present = APPT_STATUS_YES;
                                        item.AppointmentStatusDetailID = response.DataItems[0].AppointmentStatusDetailID;
                                        item.PresentID = id;
                                    }
                                    else {
                                        item.Present = 'Select';
                                        item.AppointmentStatusDetailID = 0;
                                        item.PresentID = 0;
                                    }
                                } else {
                                    alertService.error('Error while retrieving the appointment status detail! Please reload the page and try again.');
                                }
                            })

                                                     
                            
                            // Get member info for each apptResource above using the id returned BUT based on type of resource
                            if (item.ResourceTypeID == $scope.ResourceTypeIDContact) {

                                // Get the individual note for each contact 
                                appointmentService.getAppointmentNote($scope.AppointmentID, item.ContactID, null, null).then(function (response) {
                                    if (response.ResultCode === 0 && response.DataItems != null){
                                        if (response.DataItems.length > 0) {
                                            item.IndividualNoteText = response.DataItems[0].NoteText;
                                            item.IndividualNoteId = response.DataItems[0].AppointmentNoteID;
                                        }
                                    } else {
                                        alertService.error('Error while retrieving the contact appointment note! Please reload the page and try again.');
                                    }
                                })

                                // Get contact details
                                registrationService.get(item.ContactID).then(function (response) {
                                    if (response.ResultCode === 0 && response.DataItems != null && response.DataItems.length > 0) {
                                        item.Name = response.DataItems[0].FirstName + ' ' + response.DataItems[0].LastName;
                                    } else {
                                        alertService.error('Error while retrieving contact demographics! Please reload the page and try again.');
                                    }
                                    $scope.isLoading = false;
                                })
                            }
                            else if (jQuery.inArray(item.ResourceTypeID, $scope.ResourceTypeIDs)>=0) {
                                // Get the individual note for each user/provider - here contactID is userID
                                appointmentService.getAppointmentNote($scope.AppointmentID, null, null, item.ContactID).then(function (response) {
                                    if (response.ResultCode === 0 && response.DataItems != null) {
                                        if (response.DataItems.length > 0) {
                                            item.IndividualNoteText = response.DataItems[0].NoteText;
                                            item.IndividualNoteId = response.DataItems[0].AppointmentNoteID;
                                        }
                                    } else {
                                        alertService.error('Error while retrieving the user/provider appointment note! Please reload the page and try again.');
                                    }
                                })

                                // Get user details via userdetailservice, here contactID is userID
                                $scope.userDetailService.get(item.ContactID).then(function (response) {
                                    if (response.ResultCode === 0 && response.DataItems != null && response.DataItems.length > 0) {
                                        item.Name = response.DataItems[0].FirstName + ' ' + response.DataItems[0].LastName;
                                    } else {
                                        alertService.error('Error while retrieving user details! Please reload the page and try again.');
                                    }
                                    $scope.isLoading = false;
                                })
                            }

                        });

                } else {
                    alertService.error('Error while retrieving appointment resources! Please reload the page and try again.');
                    $scope.isLoading = false;
                }
            });
        }

        $scope.getApptResource = function (item) {
            var apptresource = {};
            apptresource.AppointmentResourceID = item.AppointmentResourceID;
            apptresource.IsNoShow = (item.Present == 'Yes') ? false : true;
            apptresource.AppointmentID = 1; // Doesn't matter!
            apptresource.ResourceTypeID = 1; // Doesn't matter!
            apptresource.ResourceID = 1; // Doesn't matter!
            return apptresource;
        }

        $scope.getApptStatus = function (item, isupdate) {
            var apptstatus = {};
            apptstatus.AppointmentResourceID = item.AppointmentResourceID;
            apptstatus.AppointmentID = $scope.AppointmentID;
            if (item.Present == APPT_STATUS_NO_SHOW)
                item.PresentID = $scope.AppointmentStatusNoShowID;
            else if (item.Present == APPT_STATUS_YES)
                item.PresentID = $scope.AppointmentStatusYesID;
            apptstatus.AppointmentStatusID = item.PresentID;
            if (isupdate) apptstatus.AppointmentStatusDetailID = item.AppointmentStatusDetailID;
            return apptstatus;
        }

        $scope.getApptNoteObject = function(isgroup, isupdate){
            // Item is each item from $scope.GroupMembers
            var apptNote = {};
            if (isupdate) {
                apptNote.AppointmentNoteID = (isgroup) ? $scope.GroupNoteID : $scope.EditItem.EditNoteId;
            }
            if (isgroup) {
                apptNote.GroupID = $scope.GroupID;
                apptNote.ContactID = 0;
                apptNote.UserID = 0;
            }
            else {
                apptNote.GroupID = 0;
                if ($scope.EditItem.EditResourceTypeID == $scope.ResourceTypeIDContact) {
                    apptNote.ContactID = $scope.EditItem.EditContactId;
                    apptNote.UserID = 0;
                }
                else {
                    apptNote.ContactID = 0;
                    apptNote.UserID = $scope.EditItem.EditContactId;
                }
            }
            apptNote.AppointmentID = $scope.AppointmentID;
            apptNote.NoteTypeID = $scope.GroupNoteTypeID;
            apptNote.NoteText = (isgroup) ? $scope.GroupNoteText : $scope.EditItem.EditNoteText;
            return apptNote;
        }

        $scope.getSignatureModel = function (id, type, entityTypeID, entityID) {
            var signature = {};
            signature.DocumentID = id;
            signature.DocumentTypeID = type;
            signature.EntityTypeID = entityTypeID;
            signature.EntityID = entityID;
            signature.SignatureBlob = '';
            return signature;
        }

        $scope.saveSignatureStatus = function (id, type, isgroup, entityTypeID, entityID) { 
            eSignatureService.saveDocumentSignature($scope.getSignatureModel(id, type, entityTypeID, entityID)).then(function (response) {
                if (response.data.ResultCode === 0) {
                    alertService.success('Signature status saved successfully!');
                    if (isgroup)
                        $scope.IsSignatureLocked = true;
                    else {
                        $scope.EditItem.EditIsSignatureLocked = true;

                        if ($scope.ctrl.groupNoteForm) {
                            $scope.ctrl.groupNoteForm.$setPristine();
                        }
                        formService.reset();
                    }
                } else {
                    alertService.error('Error while saving signature! Please reload the page and try again.');
                }
            });
        }

        $scope.draftIndividual = function (isNext, mandatory) {
            if ($scope.EditItem.EditIsDigitallySigned) {
                bootbox.confirm("Save the note in DRAFT mode? Digital signature entered will be ignored.", function (result) {
                    if (result === true) {
                        $scope.EditItem.EditIsDigitallySigned = false;
                        var q = $q.defer();
                        $scope.isLoading = true;

                        // INDIVIDUAL NOTE            
                        $scope.saveIndividualNote(true);

                        return q.promise;
                    }
                });
            } else {
                var q = $q.defer();
                $scope.isLoading = true;

                // INDIVIDUAL NOTE            
                $scope.saveIndividualNote(true);

                return q.promise;
            }
        }

        $scope.submitIndividual = function (isNext, mandatory) {            

            // 1. Check the digital signature - stop and display warning that it doesn't match
            if (!$scope.EditItem.EditIsDigitallySigned) {
                if ($scope.EditItem.EditNoteText === '') {
                    alertService.warning('Please enter additional note details!');
                    return;
                }
                bootbox.confirm("No digital signature entered, save the note in DRAFT mode?", function (result) {
                    if (result === true) {
                        $scope.draftIndividual(isNext, mandatory);
                        return;
                    }
                    else
                        return;
                });
            }
            else {
                if (!$scope.EditItem.EditIsPwdMatch) {
                    bootbox.alert("The digital password you entered does not match the stored value. Please try again.");
                    return;
                }

                // 2. TODO: Only allow the primary to sign the note - the 'isPrimary' flag comes from groupSchedulingResource table

                // 3. Save
                var q = $q.defer();               

                // INDIVIDUAL NOTE
                $scope.saveIndividualNote(false);

                return q.promise;
            }
        }

        $scope.draft = function (isNext, mandatory) {
            if ($scope.ctrl.groupNoteForm.$dirty) {

                // Don't allow 'Select' state
                var selectitems = $filter('filter')($scope.GroupMembers, function (item) {
                    return item.Present == 'Select';
                });
                if (selectitems.length > 0) {
                    bootbox.alert("Please select a valid presence state for every member!");
                    return;
                }

                if ($scope.IsDigitallySigned) {
                    bootbox.confirm("Save the note in DRAFT mode? Digital signature entered will be ignored.", function (result) {
                        if (result === true) {
                            $scope.IsDigitallySigned = false;
                            var q = $q.defer();
                            $scope.isLoading = true;

                            // GROUP NOTE
                            $scope.saveGroupNote(true);

                            // NO SHOW STATUS
                            $scope.saveIndividualPresentStatus();

                            return q.promise;
                        }
                    });
                } else {
                    var q = $q.defer();
                    $scope.isLoading = true;

                    // GROUP NOTE
                    $scope.saveGroupNote(true);

                    // NO SHOW STATUS
                    $scope.saveIndividualPresentStatus();

                    return q.promise;
                }


            } else if (!$scope.ctrl.groupNoteForm.$dirty && isNext) {
                $scope.handleNextState();
            } else {
                return $scope.promiseNoOp();
            }
        }

        $scope.submit = function (isNext, mandatory) {
            // Don't allow 'Select' state
            var selectitems = $filter('filter')($scope.GroupMembers, function (item) {
                return item.Present == 'Select';
            });
            if (selectitems.length > 0) {
                bootbox.alert("Please select a valid presence state for every member!");
                return;
            }

            // 1. Check the digital signature - stop and display warning that it doesn't match
            if (!$scope.IsDigitallySigned) {
                bootbox.confirm("No digital signature entered, save the note in DRAFT mode?", function (result) {
                    if (result === true) {
                        $scope.draft(isNext, mandatory);
                        return;
                    }
                    else
                        return;
                });
            }
            else {
                if (!$scope.IsPwdMatch) {
                    bootbox.alert("The digital password you entered does not match the stored value. Please try again.");
                    return;
                }

                // 2. TODO: Only allow the primary to sign the note            

                // 3. Save
                if ($scope.ctrl.groupNoteForm.$dirty) {

                    var q = $q.defer();
                    $scope.isLoading = true;

                    // GROUP NOTE
                    $scope.saveGroupNote(false);

                    // NO SHOW STATUS
                    $scope.saveIndividualPresentStatus();

                    return q.promise;

                } else if (!$scope.ctrl.groupNoteForm.$dirty && isNext) {
                    $scope.handleNextState();
                } else {
                    return $scope.promiseNoOp();
                }
            }
        }

        $scope.saveGroupNote = function (isdraft) {

            if ($scope.GroupNoteText != '') {

                if ($scope.GroupNoteID == 0) {
                    // Add group note
                    $scope.saveAppointmentNote(true, false, isdraft);
                }
                else {
                    // Update group note
                    $scope.saveAppointmentNote(true, true, isdraft);
                }
            }
        }

        $scope.saveIndividualNote = function (isdraft) {

            // Just save what's currently in scope for an indidual
            if ($scope.EditItem.EditNoteText != '') {
                $scope.isLoading = true;
                if ($scope.EditItem.EditNoteId == 0) {
                    // Add individual note
                    $scope.saveAppointmentNote(false, false, isdraft);
                }
                else {
                    // Update individual note
                    $scope.saveAppointmentNote(false, true, isdraft);
                }
            } else {
                //prevent the screen from being stuck with the spinner...isLoading = true
                $scope.isLoading = false;
            }
        }

        $scope.saveIndividualPresentStatus = function () {
                    
            // Get count of items to save
            var dirtypresence = $filter('filter')($scope.GroupMembers, function (data) {
                return data.IsPresenceDirty == true;
            });
            $scope.ItemsToSave = dirtypresence.length;
            $scope.SaveItemsCounter = 0;

            // If there's no change in presence, just throw up success
            $scope.VerifySuccess();

            if ($scope.ItemsToSave > 0) {
                angular.forEach($scope.GroupMembers, function (item, key) {
                    // For each 'dirty' member (on change to Present dropdown), update 'no show' status for this member/appt resource (updateapptnoshow)
                    if (item.IsPresenceDirty)
                        $scope.updateAppointmentNoShow(item);
                });
            }
        }

        $scope.VerifySuccess = function () {
            if ($scope.SaveItemsCounter == $scope.ItemsToSave) {
                alertService.success('All group items saved successfully!');
                $scope.SaveItemsCounter = 0;
                $scope.ItemsToSave = 0;

                if ($scope.ctrl.groupNoteForm) {
                    $scope.ctrl.groupNoteForm.$setPristine();
                }
                formService.reset();
            }
        }

        $scope.updateAppointmentNoShow = function (item) {           

            if (item.AppointmentStatusDetailID == 0) {
                appointmentService.addAppointmentStatusDetail($scope.getApptStatus(item, false)).then(function (response) {
                    if (response.ResultCode === 0) {
                        item.AppointmentStatusDetailID = response.ID;
                        $scope.SaveItemsCounter++;
                        $scope.VerifySuccess();
                    } else {
                        alertService.error('Error while adding an appointment status detail! Please reload the page and try again.');
                    }
                    item.IsPresenceDirty = false;
                });
            }
            else {
                appointmentService.updateAppointmentStatusDetail($scope.getApptStatus(item, true)).then(function (response) {
                    if (response.ResultCode === 0) {
                        $scope.SaveItemsCounter++;
                        $scope.VerifySuccess();
                    } else {
                        alertService.error('Error while updating an appointment status detail! Please reload the page and try again.');
                    }
                    item.IsPresenceDirty = false;
                });
            }
        }

            $scope.saveAppointmentNote = function(isgroup, isupdate, isdraft) {

                var item = $scope.getApptNoteObject(isgroup, isupdate);
                if (!isupdate) {
                    appointmentService.addAppointmentNote(item).then(function(response) {
                        if (response.ResultCode === 0) {
                            var obj = { stateName: $state.current.name, validationState: 'valid' };
                            $rootScope.groupAppointmentRightNavigationHandler(obj);

                            if (isgroup) {
                                $scope.GroupNoteID = response.ID;

                                // Now save signature status
                                if ($scope.IsDigitallySigned && !isdraft)
                                    $scope.saveSignatureStatus($scope.GroupNoteID, $scope.GroupDocumentTypeID, true, GROUP_TYPE_ID, $scope.GroupID);
                            } else {
                                // Find the member by contactid and set the note id/text
                                var member = $filter('filter')($scope.GroupMembers, function(data) {
                                    if (data.ContactID == $scope.EditItem.EditContactId)
                                        return data;
                                });
                                member[0].IndividualNoteId = response.ID;
                                member[0].IndividualNoteText = $scope.EditItem.EditNoteText;
                                $scope.EditItem.EditNoteId = response.ID;

                                // Now save signature status
                                if ($scope.EditItem.EditIsDigitallySigned && !isdraft)
                                    $scope.saveSignatureStatus($scope.EditItem.EditNoteId, $scope.IndividualDocumentTypeID, false, $scope.EditItem.EditResourceTypeID > MAX_ENTITYID ? DEFAULT_ENTITYPE_ID : $scope.EditItem.EditResourceTypeID, $scope.EditItem.EditContactId);
                            }
                            $scope.isLoading = false;
                            var txt = (isgroup) ? 'Group note' : 'Note';
                            alertService.success(txt + ' saved successfully!');
                        } else {
                            $scope.isLoading = false;
                            alertService.error('Error while adding an appointment note! Please reload the page and try again.');
                        }
                    });
                } else {
                    appointmentService.updateAppointmentNote(item).then(function(response) {
                        if (response.ResultCode === 0) {
                            if (isgroup) {
                                // Now save signature status
                                if ($scope.IsDigitallySigned && !isdraft)
                                    $scope.saveSignatureStatus($scope.GroupNoteID, $scope.GroupDocumentTypeID, true, GROUP_TYPE_ID, $scope.GroupID);
                            } else {

                                // Find the member by contactid and set the note text
                                var member = $filter('filter')($scope.GroupMembers, function(data) {
                                    if (data.ContactID == $scope.EditItem.EditContactId)
                                        return data;
                                });
                                member[0].IndividualNoteText = $scope.EditItem.EditNoteText;

                                // Now save signature status
                                if ($scope.EditItem.EditIsDigitallySigned && !isdraft)
                                    $scope.saveSignatureStatus($scope.EditItem.EditNoteId, $scope.IndividualDocumentTypeID, false, $scope.EditItem.EditResourceTypeID > MAX_ENTITYID ? DEFAULT_ENTITYPE_ID : $scope.EditItem.EditResourceTypeID, $scope.EditItem.EditContactId);
                            }

                            $scope.isLoading = false;
                            var txt = (isgroup) ? 'Group note' : 'Note';
                            alertService.success(txt + ' updated successfully!');
                        } else {
                            $scope.isLoading = false;
                            alertService.error('Error while updating an appointment note! Please reload the page and try again.');
                        }
                    });
                }
            };

            $scope.editGroupMember = function(item) {
                $('[ui-view="navigation"]').css('display', 'none');
                $scope.getSignatureStatus(item.IndividualNoteId, $scope.IndividualDocumentTypeID, false, item);
            };


            $scope.onDigitalSignClick = function() {

            };

            $scope.onGroupNoteTextChange = function() {
                $scope.IsGroupNoteDirty = true;
            };

        $scope.onEditInidividualNoteTextChange = function () {
            $scope.EditIsIndividualNoteDirty = true;
        }

        $scope.onPresentChange = function (item) {
            item.IsPresenceDirty = true;
        }

        $scope.onIndividualNoteTextChange = function (item) {
            item.IsIndividualNoteDirty = true;
        }

        $scope.closeFlyout = function () {
            $('[ui-view="navigation"]').css('display', '')
            angular.element('.row-offcanvas').removeClass('active');
        };

        $scope.init();
    }]);