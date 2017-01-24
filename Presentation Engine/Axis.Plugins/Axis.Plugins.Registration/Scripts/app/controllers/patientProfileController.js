angular.module('xenatixApp')
    .controller('patientProfileController', [
        '$scope', '$state', '$q', 'alertService', 'lookupService', '$filter', '$stateParams', 'registrationService', 'collateralService', 'additionalDemographyService', 'navigationService', 'photoService', 'contactPhotoService', '$injector', 'contactBenefitService', 'isECIClient', '$rootScope', 'formService', 'contactEmailService', 'contactPhoneService', 'contactAddressService', 'contactAliasService', 'contactRaceService', 'referralAdditionalDetailService', 'admissionService', 'selfPayService', 'financialAssessmentService', 'referralHeaderService', 'contactRelationshipService', 'contactSSNService', 'dischargeService',
        function ($scope, $state, $q, alertService, lookupService, $filter, $stateParams, registrationService, collateralService, additionalDemographyService, navigationService, photoService, contactPhotoService, $injector, contactBenefitService, isECIClient, $rootScope, formService, contactEmailService, contactPhoneService, contactAddressService, contactAliasService, contactRaceService, referralAdditionalDetailService, admissionService, selfPayService, financialAssessmentService, referralHeaderService, contactRelationshipService, contactSSNService, dischargeService) {
            var strECI = DIVISION_NAME.ECS;
            var colorAllergiesIDlimit = 4
            $scope.image = null;
            $scope.thumbnail = null;
            $scope.contactPhotos = [];
            $scope.picture = null;
            $scope.profilePicture = null;
            $scope.takeBy = null;
            $scope.isSaving = false;
            $scope.isECIClient = isECIClient;
            var collateralEffectiveDate = 'CollateralEffectiveDate';
            var collateralExpirationDate = 'CollateralExpirationDate';
            $scope.voidModel = {};
            $scope.isVoidFlyout = false;
            $rootScope.isPhotoChangeFlyout = false;
            $scope.showHideView = function (hideView, showView) {
                var viewHide = angular.element('#' + hideView);
                var viewShow = angular.element('#' + showView);
                appendClass(viewHide, 'hide');
                appendClass(viewShow, 'show');
                deleteClass(viewHide, 'show');
                deleteClass(viewShow, 'hide');
            }
            var printModel = {};
            var appendClass = function (elem, className) {
                if (!elem.hasClass(className))
                    elem.addClass(className);
            }

            var deleteClass = function (elem, className) {
                if (elem.hasClass(className))
                    elem.removeClass(className);
            }

            var resetForm = function () {
                if ($scope.ctrl && $scope.ctrl.photoForm)
                    $rootScope.formReset($scope.ctrl.photoForm);
            };

            $scope.init = function () {
                $scope.contactID = $stateParams.ContactID;
                if ($scope.GenderList == undefined || $scope.GenderList == null)
                    $scope.GenderList = $scope.getLookupsByType('Gender');

                if (!$scope.ClientTypeList)
                    $scope.ClientTypeList = $scope.getLookupsByType('ClientType');

                if ($scope.LegalStatusList == undefined || $scope.LegalStatusList == null)
                    $scope.LegalStatusList = $scope.getLookupsByType('LegalStatus');

                if ($scope.clientIdentifierTypeList == undefined || $scope.clientIdentifierTypeList == null)
                    $scope.clientIdentifierTypeList = $scope.getLookupsByType('ClientIdentifierType');

                if ($injector.has('allergyService')) {
                    $scope.AllergyList = $scope.getLookupsByType('Allergy');
                }

                if ($injector.has('drugService')) {
                    $scope.DrugList = $scope.getDrugData();
                }

                navigationService.get().then(function (data) {
                    $scope.takeBy = data.DataItems[0].UserID;
                });
                $scope.getContactPhotos();

                $scope.getPatientProfileData();
            };

            //To get Patient Profile Data( 
            $scope.getPatientProfileData = function () {
                $scope.isLoading = true;
                var demographicData = null;
                var isECIClient = false;
                $scope.showDeceased = false;
                //return statement goes here
                return registrationService.get($scope.contactID).then(function (data) {
                    if (data == null || data.DataItems.length === 0) {
                        //no data so let's try the ecidemographicservice
                        if ($injector.has('eciDemographicService')) {
                            var eciDemographicService = $injector.get('eciDemographicService');
                            eciDemographicService.get($scope.contactID).then(function (ecidata) {
                                if (ecidata !== null && ecidata !== undefined && ecidata.DataItems.length > 0) {
                                    demographicData = ecidata.DataItems[0];
                                    isECIClient = true;
                                    $scope.setTileData(demographicData, isECIClient);
                                }
                            });
                        }
                    } else {
                        demographicData = data.DataItems[0];
                        if (demographicData.ClientTypeID === $scope.ClientTypeList.filter(function (obj) { return obj.Name === strECI; })[0].ID) {
                            isECIClient = true;
                        }
                        $scope.setTileData(demographicData, isECIClient);
                    }
                },//registrationservice.get
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                });
            };

            $scope.setTileData = function (demographicData, isECIClient) {
                $scope.header = demographicData;
                //everything from here on in shared method
                if ($scope.header.DOB != null) {
                    $scope.header.Age = $filter('ageToShow')(
                        $filter('toMMDDYYYYDate')($scope.header.DOB, 'MM/DD/YYYY'),
                        $scope.header.DeceasedDate ? $filter('toMMDDYYYYDate')($scope.header.DeceasedDate, 'MM/DD/YYYY') : null
                    );
                    $scope.header.DOB = $filter('toMMDDYYYYDate')($scope.header.DOB, 'MM/DD/YYYY');
                    $scope.header.AdjustedAge = $filter('toAdjustedAgeCaluclation')($scope.header.DOB, $scope.header.GestationalAge);
                }
                //TFS#3207-Hiding header details shown only for ECI user.
                //$scope.header.showCompleteHeader = lookupService.getText("ClientType", $scope.header.ClientTypeID) === strECI ? true : false;

                var careID = $filter('filter')(demographicData.ClientAlternateIDs, { ClientIdentifierTypeID: Constant_ID.CareID }, true)[0];
                if (careID)
                    $scope.header.CareID = careID.AlternateID;



                //TFS#5113                
                var tKidsId = $filter('filter')(demographicData.ClientAlternateIDs, { ClientIdentifierTypeID: Constant_ID.TKidsID }, true)[0];
                if (tKidsId)
                    $scope.header.TKIDSID = tKidsId.AlternateID

                $scope.header.showCompleteHeader = false;
                var contactTypeText = isECIClient ? 'Family Relationship' : 'Collateral';
                var contactTypeId = $scope.getLookupsByType('ContactType').filter(function (obj) { return obj.Name === contactTypeText; })[0].ID;
                collateralService.get($scope.contactID, contactTypeId, false).then(function (data) {
                    printModel.collateral = data.DataItems;
                    var larCollateralDataItems = $filter('filter')(data.DataItems, { IsLAR: true }, true);
                    if (hasDetails(larCollateralDataItems)) {
                        var allLAR = $filter('orderBy')(filterFutureOrExpiredRecords(larCollateralDataItems, collateralExpirationDate, collateralEffectiveDate), "-CollateralEffectiveDate", true);
                        var larToShow = allLAR[0];
                        if (larToShow) {
                            $scope.header.LARName = larToShow.FirstName + ' ' + larToShow.LastName + (allLAR.length > 1 ? ' ...' : '');
                            $scope.header.LARPhone = hasDetails(larToShow.Phones) && larToShow.Phones[0].Number ? larToShow.Phones[0].Number : null;
                        }
                    }
                    data.DataItems = $filter('orderBy')(data.DataItems, "ModifiedOn", true);
                    var emergencyCollateralList = filterFutureOrExpiredRecords(data.DataItems, collateralExpirationDate, collateralEffectiveDate).filter(function (obj) {
                        return obj.EmergencyContact === true;
                    });
                    if (hasDetails(emergencyCollateralList)) {
                        var emergencyCollateral = emergencyCollateralList[0];
                        $scope.header.EmergencyContactFirstName = emergencyCollateral.FirstName;
                        $scope.header.EmergencyContactLastName = emergencyCollateral.LastName;

                        //[bug:12535] remove IsPrimary filter, since there is no way to select primary phone in collateral screen.
                        var ecPhone = emergencyCollateral.Phones[0]; //.filter(function (obj) { return obj.IsPrimary === true })[0];
                        if (ecPhone) {
                            $scope.header.EmergencyContactPhoneNumber = $filter('toPhone')(ecPhone.Number);
                        }
                    }

                    if (!isECIClient) {
                        additionalDemographyService.getAdditionalDemographic($scope.contactID).then(function (data) {
                            var additionalDemo = data;//.DataItems[0];
                            if (hasData(data))
                                printModel.additionalDemo = data.DataItems[0];
                            $scope.handleAdditionalDemographics(additionalDemo);
                            $scope.handleAllergies();
                            $scope.handlePayors();
                        });
                    } else {
                        //call the eci additional demo service
                        if ($injector.has('eciAdditionalDemographicService')) {
                            var eciAdditionalDemographicService = $injector.get('eciAdditionalDemographicService');
                            eciAdditionalDemographicService.getAdditionalDemographic($scope.contactID).then(function (eciData) {
                                var additionalDemo = eciData;//.DataItems[0];
                                if (hasData(eciData))
                                    printModel.additionalDemo = eciData.DataItems[0];
                                $scope.handleAdditionalDemographics(additionalDemo);
                                $scope.handleAllergies();
                                $scope.handlePayors();
                            });
                        }
                    }
                });
            };

            $scope.handlePayors = function () {
                $scope.header.MedicaidId = 'N/A';
                contactBenefitService.get($scope.contactID).then(function (data) {
                    if (data && data.DataItems.length > 0) {
                        var payors = $filter('filter')(data.DataItems, function (itm) {
                            return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                        })
                        if (payors && payors.length > 0) {
                            $scope.header.MedicaidId = payors[0].PolicyID;
                        }
                    }
                });
            }

            $scope.handleAllergies = function () {
                if ($injector.has('allergyService')) {
                    var allergyService = $injector.get('allergyService');
                    allergyService.getTopAllergies($scope.contactID).then(function (data) {
                        var allergyData = null;
                        if (data != null && data.DataItems.length > 0) {
                            allergyData = data.DataItems;
                            allergyData = $filter('xenUnique')(allergyData, 'AllergyName');
                            allergyData = allergyData.sort(function (a, b) { return (a['AllergySeverityID'] < b['AllergySeverityID'] ? 1 : -1); });
                        } else {
                            allergyData = [{
                                ContactAllergyID: 0, ContactAllergyDetailID: 0, AllergyID: 0, AllergyName: 'Allergies have not been provided', AllergyTypeID: 0, AllergySeverityID: 0, AllergySeverity: '', SortOrder: 0
                            }];
                        }
                        $scope.header.AllergyData = allergyData;
                        $scope.header.ColorAllergySeverityID = colorAllergiesIDlimit;
                        $scope.header.AllergyUrl = '';
                        //$scope.header.AllergyUrl = 'patientprofile.chart.intake.allergy({ ContactID: ' + $scope.contactID + ', AllergyTypeID: 1 })'
                        $scope.postGet();
                    }, function (error) {
                        $scope.postGet();
                    });
                } else {
                    $scope.postGet();
                }
            };

            $scope.handleAdditionalDemographics = function (additionalDemo) {
                if (additionalDemo && additionalDemo.DataItems && additionalDemo.DataItems.length > 0) {
                    var data = additionalDemo.DataItems[0];
                    $scope.header.LegalStatusID = data.LegalStatusID;
                    if (additionalDemo.ResultMessage === 'OFFLINE' && (data.MRN === null || data.MRN === undefined)) {
                        $scope.header.MRN = "PENDING";
                    } else {
                        $scope.header.MRN = data.MRN;
                        $scope.header.PrimaryLanguage = getLookupFieldById(languageList, data.PrimaryLanguageID);
                        $scope.header.SchoolDistrict = lookupService.getText("SchoolDistrict", data.SchoolDistrictID);
                        $scope.header.Race = lookupService.getText("Race", data.RaceID);
                    }
                    if (data.ReferralDispositionStatusID && data.ReferralDispositionStatusID != 0)
                        $scope.header.DispositionStatus = lookupService.getText("ReferralDispositionStatus", data.ReferralDispositionStatusID);
                    if (data.ReportingUnitID && data.ReportingUnitID != 0)
                        $scope.header.ReportingUnit = lookupService.getText("ClientType", data.ReportingUnitID);
                    if (data.ServiceCoordinatorID && data.ServiceCoordinatorID != 0) {
                        var users = $scope.getLookupsByType('Users').filter(function (obj) { return obj.ID === data.ServiceCoordinatorID; })[0];
                        if (users != undefined) {
                            $scope.header.ServiceCoordinator = users.Name;
                            $scope.header.ServiceCoordinatorPhone = users.Number;
                        }
                    }
                } else if (!$scope.header.MRN) {
                    $scope.header.MRN = "PENDING";
                }
            };

            $scope.postGet = function () {
                var fullName = $scope.header.FirstName;
                if ($scope.header.Middle != null)
                    fullName = fullName + ' ' + $scope.header.Middle;
                if ($scope.header.LastName != null)
                    fullName = fullName + ' ' + $scope.header.LastName;
                if ($scope.header.SuffixID)
                    fullName = fullName + ' ' + lookupService.getText("Suffix", $scope.header.SuffixID);
                if ($scope.header.IsDeceased && $scope.header.DeceasedDate)
                    $scope.showDeceased = true;
                $scope.header.FullName = fullName;

                if (!$scope.header.EmergencyContactFirstName) {
                    $scope.header.EmergencyContactName = '';
                }
                else {
                    var ecFullName = $scope.header.EmergencyContactFirstName;
                    if ($scope.header.EmergencyContactLastName !== null || $scope.header.EmergencyContactLastName !== undefined)
                        ecFullName = ecFullName + ' ' + $scope.header.EmergencyContactLastName;
                    $scope.header.EmergencyContactName = ecFullName;
                }

                if ($scope.header.ClientTypeID > 0)
                    $scope.header.ClientType = getText($scope.header.ClientTypeID, $scope.ClientTypeList);

                if ($scope.header.GenderID > 0)
                    $scope.header.Gender = getText($scope.header.GenderID, $scope.GenderList);

                if ($scope.header.PreferredGenderID != undefined && $scope.header.PreferredGenderID > 0)
                    $scope.header.PreferredGender = getText($scope.header.PreferredGenderID, $scope.GenderList);
                else
                    $scope.header.PreferredGender = '';

                if ($scope.header && $scope.header.LegalStatusID > 0)
                    $scope.header.LegalStatus = getText($scope.header.LegalStatusID, $scope.LegalStatusList);
                $scope.viewURL = $scope.header.ClientType == strECI ? "/Registration/ECIPatientHeader" : "/Registration/PatientHeader";        //Set url for loading header UI
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.getDrugData = function () {
                return $injector.get('drugService').getDrugData();
            };

            getText = function (value, list) {
                if (value) {
                    var formattedValue = lookupService.getSelectedTextById(value, list);
                    if (formattedValue != undefined && formattedValue.length > 0)
                        return formattedValue[0].Name;
                    else
                        return '';
                } else

                    return '';
            };

            $scope.viewLoaded = function () {
                offCanvasNav.init();
            };

            // Patient Profile Photo
            $scope.save = function () {
                savePhoto().then(function (response) {
                    if (response != undefined) {
                        alertService.success('Profile photo is saved successfully.');
                        $scope.picture = null;
                        $scope.isSaving = false;
                        $scope.getContactPhotoById(response);
                    }
                });
            };

            $scope.getContactPhotos = function () {
                return contactPhotoService.getContactPhoto($scope.contactID).then(function (contactPhotoResponse) {
                    var defer = $q.defer();
                    var promises = [];
                    if (contactPhotoResponse.DataItems.length > 0) {
                        angular.forEach(contactPhotoResponse.DataItems, function (contactPhoto) {
                            promises.push(photoService.getPhoto(contactPhoto.PhotoID).then(function (photoResponse) {
                                var photo = photoResponse.DataItems[0];
                                var contactThumbnail = {
                                    ContactPhotoID: contactPhoto.ContactPhotoID,
                                    ContactID: contactPhoto.ContactID,
                                    PhotoID: contactPhoto.PhotoID,
                                    ThumbnailBLOB: photo.ThumbnailBLOB,
                                    IsPrimary: contactPhoto.IsPrimary
                                };
                                return contactThumbnail;
                            }));
                        });
                        return $q.all(promises).then(function (photoes) {
                            $scope.contactPhotos = photoes;
                            $scope.profilePicture = $filter('filter')(photoes, {
                                IsPrimary: true
                            })[0];
                            resetForm();
                        });
                    }
                    else {
                        $scope.contactPhotos = [];
                        $scope.profilePicture = null;
                        $scope.preview = false;
                        resetForm();
                    }
                });
            };

            $scope.getContactPhotoById = function (contactPhotoId) {
                return contactPhotoService.getContactPhotoById($scope.contactID, contactPhotoId).then(function (contactPhotoResponse) {
                    var defer = $q.defer();
                    var promises = [];
                    if (contactPhotoResponse.DataItems.length > 0) {
                        var contactPhoto = contactPhotoResponse.DataItems[0];
                        promises.push(photoService.getPhoto(contactPhoto.PhotoID).then(function (photoResponse) {
                            var photo = photoResponse.DataItems[0];
                            var contactThumbnail = {
                                ContactPhotoID: contactPhoto.ContactPhotoID,
                                ContactID: contactPhoto.ContactID,
                                PhotoID: contactPhoto.PhotoID,
                                ThumbnailBLOB: photo.ThumbnailBLOB,
                                IsPrimary: contactPhoto.IsPrimary
                            };

                            return contactThumbnail;
                        }));
                        return $q.all(promises).then(function (photoes) {
                            var contactPhotos = angular.copy($scope.contactPhotos);

                            var isExists = $filter('filter')(contactPhotos, {
                                ContactPhotoID: contactPhotoId
                            }).length > 0 ? true : false;

                            if (isExists) {
                                angular.forEach(contactPhotos, function (contactThumbnailsResponse) {
                                    if (contactThumbnailsResponse.ContactPhotoID == contactPhotoId) {
                                        contactThumbnailsResponse.ThumbnailBLOB = photoes[0].ThumbnailBLOB;
                                        contactThumbnailsResponse.IsPrimary = photoes[0].IsPrimary;
                                    }
                                });
                            }
                            else {
                                contactPhotos.push(photoes[0]);
                            }
                            $scope.contactPhotos = contactPhotos;
                            $scope.profilePicture = $filter('filter')($scope.contactPhotos, {
                                IsPrimary: true
                            })[0];
                            resetForm();
                        });
                    }
                    else {
                        $scope.contactPhotos = [];
                        $scope.profilePicture = null;
                        resetForm();
                    }
                });
            };

            $scope.delete = function (contactPhotoId) {
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        contactPhotoService.deleteContactPhoto($scope.contactID, contactPhotoId).then(function (contactPhotoThumbnails) {
                            if (contactPhotoThumbnails.ResultCode === 0) {
                                alertService.success('Photo has been deleted successfully.');
                                var contactPhotos = angular.copy($scope.contactPhotos);
                                var filteredContactThumbnails = $filter('filter')(contactPhotos, { ContactPhotoID: '!' + contactPhotoId });
                                $scope.contactPhotos = filteredContactThumbnails;
                                if ($scope.profilePicture && $scope.profilePicture.ContactPhotoID == contactPhotoId)
                                    $scope.profilePicture = null;
                                resetForm();
                            } else {
                                alertService.error('Unable to delete photo.');
                            }
                        });
                    }
                });
            };

            $scope.setPreviewAsPhoto = function () {
                savePhoto().then(function (contactPhotoId) {
                    setAsPhoto(contactPhotoId).then(function () {
                        alertService.success('Photo set as profile picture.');
                        $scope.isSaving = false;

                        angular.forEach($scope.contactPhotos, function (thumbnail) {
                            thumbnail.IsPrimary = false;
                        });

                        $scope.getContactPhotoById(contactPhotoId);
                    });
                });
            };

            $scope.setAsPhoto = function (contactPhotoId) {
                setAsPhoto(contactPhotoId).then(function () {
                    alertService.success('Photo set as profile picture.');
                    $scope.isSaving = false;

                    angular.forEach($scope.contactPhotos, function (thumbnail) {
                        thumbnail.IsPrimary = false;
                    });

                    $scope.getContactPhotoById(contactPhotoId);
                });
            };

            $scope.donotUsePhoto = function () {
                // Get all contactPhoto by contactId
                contactPhotoService.getContactPhoto($scope.contactID).then(function (contactPhotoResponse) {
                    var contactPhotos = contactPhotoResponse.DataItems;
                    // Get primary contactPhoto & set IsPrimary = false
                    var primaryContactPhoto = $filter('filter')(contactPhotos, {
                        IsPrimary: true
                    })[0];

                    if (primaryContactPhoto != undefined) {
                        primaryContactPhoto.IsPrimary = false;
                        contactPhotoService.updateContactPhoto(primaryContactPhoto).then(function (response) {
                            alertService.success('No photo is set as profile picture.');
                            $scope.picture = null;
                            $scope.getContactPhotos();
                        });
                    }
                });
            }

            var savePhoto = function () {
                if ($scope.isSaving)
                    return $scope.promiseNoOp();

                if (!$scope.picture || $.isEmptyObject($scope.picture)) {
                    alertService.error('Select or take picture to save.');
                    return $scope.promiseNoOp();
                }

                $scope.picture.TakenBy = $scope.takeBy;
                $scope.picture.TakenTime = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY HH:mm');

                $scope.isSaving = true;
                if ($scope.picture.PhotoID != undefined && $scope.picture.PhotoID != 0) {
                    return photoService.updatePhoto($scope.picture).then(function () {
                        return $scope.picture.ContactPhotoID;
                    });
                }
                else {
                    return photoService.addPhoto($scope.picture).then(function (response) {
                        var contactPhoto = {
                            ContactID: $scope.contactID,
                            PhotoID: response.data.ID
                        };

                        return contactPhotoService.addContactPhoto(contactPhoto).then(function (contactPhotoResponse) {
                            return contactPhotoResponse.ID;
                        });
                    });
                }
            }

            var setAsPhoto = function (contactPhotoId) {
                // Get all contactPhoto by contactId
                return contactPhotoService.getContactPhoto($scope.contactID).then(function (contactPhotoResponse) {
                    var contactPhotos = contactPhotoResponse.DataItems;

                    // Get primary contactPhoto & set IsPrimary = false
                    var primaryContactPhoto = $filter('filter')(contactPhotos, {
                        IsPrimary: true
                    })[0];

                    // Get contactPhoto by contactPhotoId & set IsPrimary = true
                    var contactPhotoToPromary = $filter('filter')(contactPhotos, {
                        ContactPhotoID: contactPhotoId
                    })[0];

                    if (contactPhotoToPromary != undefined) {
                        contactPhotoToPromary.IsPrimary = true;
                        return contactPhotoService.updateContactPhoto(contactPhotoToPromary).then(function (response) {
                            if (primaryContactPhoto != undefined && primaryContactPhoto.ContactPhotoID != contactPhotoToPromary.ContactPhotoID) {
                                primaryContactPhoto.IsPrimary = false;
                                return contactPhotoService.updateContactPhoto(primaryContactPhoto);
                            }
                        });
                    }
                });
            }


            $scope.getVoidModel = function (voidModel) {
                $scope.voidModel = angular.merge(getVoidSeviceDefaultModel(), voidModel);
                $rootScope.isVoidedFlyout = true;
                $scope.isPhotoChangeFlyout = false;
            };

            var getVoidSeviceDefaultModel = function () {
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
                    Comments: ""
                };
            };

            //adding or removing class is moved to HTML in angular way
            $scope.openPhotoChange = function (e) {
                $rootScope.defaultFormName = 'ctrl.photoForm';
                $rootScope.isVoidedFlyout = false;
                $scope.isPhotoChangeFlyout = true;                
            };
    
            // setting "isPhotoChangeFlyout" to close the photo flyout.           
            $scope.closePhotoChange = function (e) {
                var isDirty = formService.isDirty();
                if (isDirty) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            closePhotoChangeFlyout();                            
                            $scope.getContactPhotos();
                        }
                    });
                }
                else {
                    closePhotoChangeFlyout();
                }
            };
            var closePhotoChangeFlyout = function () {
                $rootScope.defaultFormName = getDefaultFormName();
                $('#patientProfileFlyout').removeClass('active'); //BUGFIX 12017
                $scope.isPhotoChangeFlyout = false;
            };
            var getLookupTextById = function (lookupType, ID) {
                return ID && lookupType ? lookupService.getText(lookupType, ID) : '';
            }
            var getDateOrEmpty = function (date) {
                return date ? $filter('toMMDDYYYYDate')(date) : '';
            }
            var stateProvinceList = $scope.getLookupsByType('StateProvince');
            var suffixList = $scope.getLookupsByType('Suffix');
            var languageList = $scope.getLookupsByType('Language');
            var emailPermissionList = $scope.getLookupsByType('EmailPermission');
            var addressTypeList = $scope.getLookupsByType('AddressType');
            var countyList = $scope.getLookupsByType('County');
            var phoneTypeList = $scope.getLookupsByType('PhoneType');
            var phonePermissionList = $scope.getLookupsByType('PhonePermission');
            var raceList = $scope.getLookupsByType('Race');
            var relationshipTypeList = $scope.getLookupsByType('RelationshipType');
            var employmentStatusList = $scope.getLookupsByType('EmploymentStatus');
            var veteranStatusList = $scope.getLookupsByType('VeteranStatus');
            $scope.closeReport = function () {
                delete $scope.reportModel;
            }
            $scope.printHeader = function () {
                var dfd = $q.defer();
                var contactTypeID
                var reportModel = {
                    ReportHeader: 'Contact Profile',
                    ReportName: 'Contact Profile'
                };
                if ($scope.header) {
                    reportModel.mrn = getTextorEmpty($scope.header.MRN);
                    reportModel.clientName = getTextorEmpty($scope.header.FirstName) + ' ' + getTextorEmpty($scope.header.LastName);
                    reportModel.dob = getDateOrEmpty($scope.header.DOB);
                    reportModel.medicaidNumber = getTextorEmpty($scope.header.MedicaidId);

                    //Demographics
                    if ($scope.header.ContactPresentingProblem) {
                        reportModel.presentingProblem = getLookupTextById('PresentingProblemType', $scope.header.ContactPresentingProblem.PresentingProblemTypeID);
                        reportModel.effectiveDate = getDateOrEmpty($scope.header.ContactPresentingProblem.EffectiveDate);
                        reportModel.expirationDate = getDateOrEmpty($scope.header.ContactPresentingProblem.ExpirationDate);
                    }
                    reportModel.division = getLookupFieldById($scope.ClientTypeList, $scope.header.ClientTypeID);
                    reportModel.firstName = getTextorEmpty($scope.header.FirstName);
                    reportModel.lastName = getTextorEmpty($scope.header.LastName);
                    reportModel.middleName = getTextorEmpty($scope.header.Middle);
                    reportModel.preferredName = getTextorEmpty($scope.header.PreferredName);
                    reportModel.prefix = getLookupTextById('PrefixType', $scope.header.TitleID);
                    reportModel.suffix = getLookupFieldById(suffixList, $scope.header.SuffixID);
                    reportModel.gender = getLookupFieldById($scope.GenderList, $scope.header.GenderID);
                    reportModel.preferredGender = getLookupFieldById($scope.GenderList, $scope.header.PreferredGenderID);
                    reportModel.dobStatus = getLookupTextById('DOBStatus', $scope.header.DOBStatusID);
                    reportModel.ssnStatus = getLookupTextById('SSNStatus', $scope.header.SSNStatusID);
                    reportModel.driverLicense = getTextorEmpty($scope.header.DriverLicense);
                    reportModel.driverLicenseState = getLookupFieldById(stateProvinceList, $scope.header.DriverLicenseStateID)

                    var otherIdJson = [];
                    angular.forEach($scope.header.ClientAlternateIDs, function (alternateId) {
                        otherIdJson.push({
                            otherIDType: getLookupFieldById($scope.clientIdentifierTypeList, alternateId.ClientIdentifierTypeID),
                            otherIDNumber: getTextorEmpty(alternateId.AlternateID),
                            effectiveDate: getDateOrEmpty(alternateId.EffectiveDate),
                            expirationDate: getDateOrEmpty(alternateId.ExpirationDate)
                        });
                    });
                    reportModel.otherID = otherIdJson;

                    var contactMethod = $filter('filter')(lookupService.getLookupsByType('ContactMethod'), { IsSystem: true, ID: $scope.header.ContactMethodID }, true);
                    if (contactMethod.length > 0 && contactMethod[0].Name)
                        reportModel.preferredContactMethod = contactMethod[0].Name;
                }
                ////Additional Demographics
                if (printModel.additionalDemo) {
                    reportModel.schoolDistrict = getLookupTextById('SchoolDistrict', printModel.additionalDemo.SchoolDistrictID);
                    reportModel.ethnicity = getLookupTextById('Ethnicity', printModel.additionalDemo.EthnicityID);
                    reportModel.legalStatus = getLookupFieldById($scope.LegalStatusList, printModel.additionalDemo.LegalStatusID);
                    reportModel.maritalStatus = getLookupTextById('MaritalStatus', printModel.additionalDemo.MaritalStatusID);
                    reportModel.preferredLanguage = getLookupFieldById(languageList, printModel.additionalDemo.PrimaryLanguageID);
                    reportModel.secondaryLanguage = getLookupFieldById(languageList, printModel.additionalDemo.SecondaryLanguageID);
                    reportModel.interpreterRequired = printModel.additionalDemo.InterpreterRequired;
                    reportModel.citizenship = getLookupTextById('Citizenship', printModel.additionalDemo.CitizenshipID);
                    reportModel.livingArrangement = getLookupTextById('LivingArrangement', printModel.additionalDemo.LivingArrangementID);
                    reportModel.militaryStatus = getLookupFieldById(veteranStatusList, printModel.additionalDemo.VeteranStatusID);
                    reportModel.employmentStatus = getLookupFieldById(employmentStatusList, printModel.additionalDemo.EmploymentStatusID);
                    reportModel.lastPrimaryPlaceOfEmployment = getTextorEmpty(printModel.additionalDemo.PlaceOfEmployment);
                    reportModel.employmentBeginDate = getDateOrEmpty(printModel.additionalDemo.EmploymentBeginDate);
                    reportModel.employmentEndDate = getDateOrEmpty(printModel.additionalDemo.EmploymentEndDate);
                    reportModel.lastSchoolAttended = getTextorEmpty(printModel.additionalDemo.SchoolAttended);
                    reportModel.schoolBeginDate = getDateOrEmpty(printModel.additionalDemo.SchoolBeginDate);
                    reportModel.schoolEndDate = getDateOrEmpty(printModel.additionalDemo.SchoolEndDate);
                    reportModel.highestEducation = getLookupTextById('EducationStatus', printModel.additionalDemo.EducationStatusID);
                    reportModel.religion = getLookupTextById('Religion', printModel.additionalDemo.ReligionID);
                    reportModel.smokingStatus = getLookupTextById('SmokingStatus', printModel.additionalDemo.SmokingStatusID);
                    reportModel.advancedDirectiveYes = printModel.additionalDemo.AdvancedDirective;
                    reportModel.advancedDirectiveNo = (printModel.additionalDemo.AdvancedDirective != undefined) ? !printModel.additionalDemo.AdvancedDirective : '';
                    reportModel.livingWill = printModel.additionalDemo.LivingWill ? 'Yes' : (printModel.additionalDemo.LivingWill == false) ? 'No' : '';
                    reportModel.powerOfAttorney = printModel.additionalDemo.PowerOfAttorney ? 'Yes' : (printModel.additionalDemo.PowerOfAttorney == false) ? 'No' : '';

                    //ECI Additional Demography
                    reportModel.printECI = $scope.isECIClient && hasAnyECIData();
                    reportModel.reportingUnit = getLookupFieldById($scope.ClientTypeList, printModel.additionalDemo.ReportingUnitID);
                    reportModel.serviceCoordinator = getLookupTextById('Users', printModel.additionalDemo.ServiceCoordinatorID);
                    reportModel.serviceCoordinatorPhone = getTextorEmpty($scope.header.ServiceCoordinatorPhone);
                    reportModel.isCPSInvolvedYes = printModel.additionalDemo.IsCPSInvolved;
                    reportModel.isCPSInvolvedNo = (printModel.additionalDemo.IsCPSInvolved != undefined) ? !printModel.additionalDemo.IsCPSInvolved : '';
                    reportModel.isChildHospitalizedYes = printModel.additionalDemo.IsChildHospitalized;
                    reportModel.isChildHospitalizedNo = (printModel.additionalDemo.IsChildHospitalized != undefined) ? !printModel.additionalDemo.IsChildHospitalized : '';
                    reportModel.expectedDishcargeDate = getDateOrEmpty(printModel.additionalDemo.ExpectedHospitalDischargeDate);
                    reportModel.isTransferYes = printModel.additionalDemo.IsTransfer;
                    reportModel.isTransferNo = (printModel.additionalDemo.IsTransfer != undefined) ? !printModel.additionalDemo.IsTransfer : '';
                    reportModel.eciProgramTransferredFrom = getTextorEmpty(printModel.additionalDemo.TransferFrom);
                    reportModel.transferredInDate = getDateOrEmpty(printModel.additionalDemo.TransferDate);
                    reportModel.isOutOfServiceArea = printModel.additionalDemo.IsOutOfServiceArea;
                    reportModel.birthWeightLbs = getValueOrEmpty(printModel.additionalDemo.BirthWeightLbs);
                    reportModel.birthWeightOz = getValueOrEmpty(printModel.additionalDemo.BirthWeightOz);
                }
                

                var printArr = [];
                printArr.push(contactAliasService.get($scope.contactID));
                printArr.push(contactAddressService.get($scope.contactID, $scope.header.ContactTypeID));
                printArr.push(contactPhoneService.get($scope.contactID, $scope.header.ContactTypeID));
                printArr.push(contactEmailService.get($scope.contactID, $scope.header.ContactTypeID));
                printArr.push(contactRaceService.get($scope.contactID));
                printArr.push(referralAdditionalDetailService.getReferral($scope.contactID));
                printArr.push(contactBenefitService.get($scope.contactID));
                printArr.push(admissionService.get($scope.contactID));
                printArr.push(selfPayService.getSelfPay($scope.contactID));
                printArr.push(financialAssessmentService.get($scope.contactID, 0));
                printArr.push(getCollateralDetails(reportModel));
                printArr.push(contactSSNService.get($scope.contactID));
                $q.all(printArr).then(function (resp) {
                    //For contact alias
                    if (hasData(resp[0])) {
                        var alias = resp[0].DataItems;
                        reportModel.alias = [];
                        angular.forEach(alias, function (item) {
                            reportModel.alias.push({
                                aliasFirst: getTextorEmpty(item.AliasFirstName),
                                aliasLast: getTextorEmpty(item.AliasLastName),
                                aliasMiddle: getTextorEmpty(item.AliasMiddle),
                                aliasSuffix: getLookupFieldById(suffixList, item.SuffixID)
                            });
                        });
                    }
                    var promiseArr = [];
                    //for contact address
                    if (hasData(resp[1])) {
                        var addresses = getPrimaryOrLatestDataItems(resp[1].DataItems);
                        reportModel.addresses = [];
                        angular.forEach(addresses, function (item) {
                            reportModel.addresses.push({
                                addressPermissions: getLookupFieldById(emailPermissionList, item.MailPermissionID),
                                primary: item.IsPrimary,
                                effectiveDate: getDateOrEmpty(item.EffectiveDate),
                                expirationDate: getDateOrEmpty(item.ExpirationDate),
                                addressType: getLookupFieldById(addressTypeList, item.AddressTypeID),
                                addressLine1: getTextorEmpty(item.Line1),
                                addressLine2: getTextorEmpty(item.Line2),
                                city: getTextorEmpty(item.City),
                                state: getLookupFieldById(stateProvinceList, item.StateProvince),
                                county: getLookupFieldById(countyList, item.County),
                                postalCode: getTextorEmpty(item.Zip),
                                complexName: getTextorEmpty(item.ComplexName),
                                gateCode: getTextorEmpty(item.GateCode)
                            });
                        });
                    }

                    //for contact phone
                    if (hasData(resp[2])) {
                        var phones = getPrimaryOrLatestDataItems(resp[2].DataItems);
                        reportModel.phones = [];
                        angular.forEach(phones, function (phone) {
                            reportModel.phones.push({
                                phoneType: getLookupFieldById(phoneTypeList, phone.PhoneTypeID),
                                phoneNumber: getTextorEmpty(phone.Number),
                                extension: getTextorEmpty(phone.Extension),
                                primary: phone.IsPrimary,
                                phonePermissions: getLookupFieldById(phonePermissionList, phone.PhonePermissionID),
                                effectiveDate: getDateOrEmpty(phone.EffectiveDate),
                                expirationDate: getDateOrEmpty(phone.ExpirationDate)
                            });
                        });
                    }

                    //for contact email
                    if (hasData(resp[3])) {
                        var emails = getPrimaryOrLatestDataItems(resp[3].DataItems);
                        reportModel.emails = [];
                        angular.forEach(emails, function (email) {
                            reportModel.emails.push({
                                email: getTextorEmpty(email.Email),
                                emailPermissions: getLookupFieldById(emailPermissionList, email.EmailPermissionID),
                                primary: email.IsPrimary,
                                effectiveDate: getDateOrEmpty(email.EffectiveDate),
                                expirationDate: getDateOrEmpty(email.ExpirationDate)
                            });
                        });
                    }

                    //for race
                    if (hasData(resp[4])) {
                        var races = resp[4].DataItems;
                        reportModel.races = [];
                        angular.forEach(races, function (item) {
                            reportModel.races.push({ race: getLookupFieldById(raceList, item.RaceID) });
                        });
                    }

                    //for referrals
                    if (hasData(resp[5])) {

                        var referrals = getPrimaryOrLatestDataItems(resp[5].DataItems);
                        reportModel.referrals = [];
                        var ct = 0;
                        angular.forEach(referrals, function (referral) {
                            var referralobj = {};
                            referralobj.referralComments = getTextorEmpty(referral.AdditionalConcerns);
                            reportModel.referrals.push(referralobj);
                            promiseArr.push(getReferralDetails(referral.ReferralHeaderID, referral.HeaderContactID, reportModel.referrals[ct]));
                            ct++;
                        })
                    }

                    //for contact benefit/Payor
                    if (hasData(resp[6])) {
                        var payors = getPrimaryOrLatestDataItems(resp[6].DataItems);
                        reportModel.payor = [];
                        promiseArr.push(getPayorDetails(payors, reportModel));
                    }

                    //for admission/discharge
                    if (hasData(resp[7])) {
                        var admissions = resp[7].DataItems;
                        reportModel.admissionDischarge = [];
                        promiseArr.push(getAdmissionDetails(admissions, reportModel));
                    }

                    //for self pay
                    if (hasData(resp[8])) {
                        var selfPays = resp[8].DataItems;
                        reportModel.selfPay = [];
                        angular.forEach(selfPays, function (selfPay) {
                            var item = {
                                selfPayDivision: selfPay.OrganizationDetailID ? getOrganizationText('Division', selfPay.OrganizationDetailID) : '',
                                selfPayAmount: selfPay.IsPercent ? $filter('number')(selfPay.SelfPayAmount, 2) + '%' : '$' + $filter('number')(selfPay.SelfPayAmount, 2),
                                selfPayAmountType: selfPay.IsPercent ? 'Percentage' : 'Dollar',
                                selfPayEffectiveDate: getDateOrEmpty(selfPay.EffectiveDate),
                                selfPayExpirationDate: getDateOrEmpty(selfPay.ExpirationDate),
                                overrideChildConservatorship: selfPay.ISChildInConservatorship,
                                overrideECIFamilyChoice: selfPay.IsNotAttested,
                                overrideEnrolledPublicBenefits: selfPay.IsEnrolledInPublicBenefits,
                                overrideFamilyChoiceDecliningConsent: selfPay.IsRequestingReconsideration,
                                overrideFamilyChoiceNoConsent: selfPay.IsNotGivingConsent,
                                overrideSecondChildEnrolled: selfPay.IsOtherChildEnrolled,
                                overrideWillApplyPublicBenefits: selfPay.IsApplyingForPublicBenefits,
                                overrideReconsiderationOfAdjustment: selfPay.IsReconsiderationOfAdjustment
                            }
                            reportModel.selfPay.push(item);
                        });
                    }

                    //for household income
                    if (hasData(resp[9])) {
                        var financialAssessment = resp[9].DataItems[0];
                        promiseArr.push(getHouseholdDetails(financialAssessment, reportModel));
                    }

                    //for SSN full
                    if (hasData(resp[11])) {
                        reportModel.ssn = getTextorEmpty(resp[11].DataItems[0]);
                    }

                    $q.all(promiseArr).then(function (respPromise) {
                        reportModel.HasLoaded = true;
                        $scope.reportModel = reportModel;
                        setEmptyFlags();
                        dfd.resolve(reportModel);
                        $('#reportHeaderModal').modal('show');
                    });

                });
                return dfd.promise;
            };

            var setEmptyFlags = function () {
                // Setup conditional print flags for all items
                $scope.reportModel.printCollateral = hasAnyCollateralData();
                $scope.reportModel.printReferrals = hasAnyReferralData();
                $scope.reportModel.printPayors = hasAnyPayorData();
                $scope.reportModel.printHouseholdIncome = hasAnyIncomeData();
                $scope.reportModel.printSelfPay = hasAnySelfPayData();
                $scope.reportModel.printAdmissionDischarge = hasAnyAdmissionData();
            }

            var hasAnyECIData = function () {
                if (printModel.additionalDemo.ReportingUnitID != null && printModel.additionalDemo.ReportingUnitID != '' ||
                    printModel.additionalDemo.ServiceCoordinatorID != null && printModel.additionalDemo.ServiceCoordinatorID != '' ||
                    $scope.header.ServiceCoordinatorPhone != null && $scope.header.ServiceCoordinatorPhone != '' ||
                    printModel.additionalDemo.IsCPSInvolved != null && printModel.additionalDemo.IsCPSInvolved != '' ||
                    printModel.additionalDemo.IsChildHospitalized != null && printModel.additionalDemo.IsChildHospitalized != '' ||
                    printModel.additionalDemo.ExpectedHospitalDischargeDate != null && printModel.additionalDemo.ExpectedHospitalDischargeDate != '' ||
                    printModel.additionalDemo.IsTransfer != null && printModel.additionalDemo.IsTransfer != '' ||
                    printModel.additionalDemo.TransferFrom != null && printModel.additionalDemo.TransferFrom != '' ||
                    printModel.additionalDemo.TransferDate != null && printModel.additionalDemo.TransferDate != '' ||
                    printModel.additionalDemo.IsOutOfServiceArea != null && printModel.additionalDemo.IsOutOfServiceArea != '' ||
                    printModel.additionalDemo.BirthWeightLbs != null && printModel.additionalDemo.BirthWeightLbs != '' ||
                    printModel.additionalDemo.BirthWeightOz != null && printModel.additionalDemo.BirthWeightOz != '')
                    return true;
                else
                    return false;
            }

            var hasAnyCollateralData = function () {
                if ($scope.reportModel.collateral != null && $scope.reportModel.collateral.length > 0)
                    return true;
                else
                    return false;
            }

            var hasAnyReferralData = function () {
                if ($scope.reportModel.referrals != null && $scope.reportModel.referrals.length > 0)
                    return true;
                else
                    return false;
            }

            var hasAnyPayorData = function () {
                if ($scope.reportModel.payor != null && $scope.reportModel.payor.length > 0)
                    return true;
                else
                    return false;
            }

            var hasAnyIncomeData = function () {
                if ($scope.reportModel.adjustedGrossIncome != null && $scope.reportModel.adjustedGrossIncome != '' ||
                    $scope.reportModel.totalIncome != null && $scope.reportModel.totalIncome != '' ||
                    $scope.reportModel.totalExpenses != null && $scope.reportModel.totalExpenses != '' ||
                    $scope.reportModel.totalExtraordinaryExpenses != null && $scope.reportModel.totalExtraordinaryExpenses != '' ||
                    $scope.reportModel.totalOther != null && $scope.reportModel.totalOther != '' ||
                    $scope.reportModel.assessmentDate != null && $scope.reportModel.assessmentDate != '' ||
                    $scope.reportModel.familySize != null && $scope.reportModel.familySize != '' ||
                    $scope.reportModel.householdIncomeExpirationDate != null && $scope.reportModel.householdIncomeExpirationDate != '' ||
                    $scope.reportModel.householdIncomeExpirationReason != null && $scope.reportModel.householdIncomeExpirationReason != '' ||
                    $scope.reportModel.familyMember != null && $scope.reportModel.familyMember.length > 0)
                    return true;
                else
                    return false;
            }

            var hasAnySelfPayData = function () {
                if ($scope.reportModel.selfPay != null && $scope.reportModel.selfPay.length > 0)
                    return true;
                else
                    return false;
            }

            var hasAnyAdmissionData = function () {
                if ($scope.reportModel.admissionDischarge != null && $scope.reportModel.admissionDischarge.length > 0)
                    return true;
                else
                    return false;
            }

            var getOrganizationText = function (type, id) {
                var organization = $filter('filter')($rootScope.getOrganizationByDataKey(type), { ID: id }, true);
                return organization.length > 0 && organization[0].Name ? organization[0].Name : '';
            }

            var getPayorDetails = function (payors, reportModel) {
                var dfd = $q.defer();
                var payorPromise = [];
                angular.forEach(payors, function (payor) {
                    var payorModel = {
                        payor: getTextorEmpty(payor.PayorName),
                        payorCode: getValueOrEmpty(payor.PayorCode),
                        electronicPayorID: getTextorEmpty(payor.ElectronicPayorID),
                        policyID: getValueOrEmpty(payor.PolicyID),
                        planName: getTextorEmpty(payor.PlanName),
                        planID: getTextorEmpty(payor.PlanID),
                        groupName: getTextorEmpty(payor.GroupName),
                        groupID: getTextorEmpty(payor.GroupID),
                        addressLine1: getTextorEmpty(payor.Line1),
                        addressLine2: getTextorEmpty(payor.Line2),
                        city: getTextorEmpty(payor.City),
                        state: getLookupFieldById(stateProvinceList, payor.StateProvince),
                        county: getLookupFieldById(countyList, payor.County),
                        postalCode: getTextorEmpty(payor.Zip),
                        effectiveDate: getDateOrEmpty(payor.EffectiveDate),
                        expirationDate: getDateOrEmpty(payor.ExpirationDate),
                        addRetroDate: getDateOrEmpty(payor.AddRetroDate),
                        billingContactFirstName: getTextorEmpty(payor.BillingFirstName),
                        billingContactMiddleName: getTextorEmpty(payor.BillingMiddleName),
                        billingContactLastName: getTextorEmpty(payor.BillingLastName),
                        billingContactSuffix: getLookupFieldById(suffixList, payor.BillingSuffixID),
                        additionalInformation: getTextorEmpty(payor.AdditionalInformation),
                        policyHolderNameMatch: payor.HasPolicyHolderSameCardName ? 'Yes' : (payor.HasPolicyHolderSameCardName == false) ? 'No' : '',
                        policyHolderEmployer: getTextorEmpty(payor.PolicyHolderEmployer),
                        policyHolderFirstName: getTextorEmpty(payor.PolicyHolderFirstName),
                        policyHolderMiddleName: getTextorEmpty(payor.PolicyHolderMiddleName),
                        policyHolderLastName: getTextorEmpty(payor.PolicyHolderLastName),
                        policyHolderSuffix: getLookupFieldById(suffixList, payor.PolicyHolderSuffixID)
                    }

                    payorPromise.push(getPolicyholderDetails(payor, payorModel, reportModel));
                });

                if (hasDetails(payorPromise)) {
                    $q.all(payorPromise).then(function (payorResp) {
                        angular.forEach(payorResp, function (item) {
                            reportModel.payor.push(item);
                        });
                        dfd.resolve(true);
                    });
                } else {
                    dfd.resolve(true);
                }
                return dfd.promise;
            };

            var getPolicyholderDetails = function (payor, payorModel, reportModel) {
                var dfd = $q.defer();
                if (payor.PolicyHolderID === $scope.contactID) { //Policy holder is self
                    //payorModel.policyHolder = reportModel.clientName ? "Self (" + reportModel.clientName + ")" : '';
                    payorModel.policyHolderRelationship = "Self";
                    payorModel.contactMatchInsuranceCard = '',
                    dfd.resolve(payorModel);
                } else {    //Policy holder is some other
                    payorModel.contactMatchInsuranceCard = hasSameCard(payor) ? 'Yes' : 'No',
                    contactRelationshipService.get(payor.PolicyHolderID, $scope.contactID).then(function (relationshipData) {
                        if (hasData(relationshipData)) {
                            var familyGroupRelations = $filter('filter')(relationshipData.DataItems, function (obj) {
                                return obj.RelationshipGroupID === RELATIONSHIP_TYPE_GROUPID.Family;
                            }, true);
                            if (familyGroupRelations && familyGroupRelations.length > 0) {
                                payorModel.policyHolderRelationship = getLookupFieldById(relationshipTypeList, familyGroupRelations[0].RelationshipTypeID);
                                //payorModel.policyHolder = payorModel.policyHolderRelationship + " (" + policyHolder.FirstName + " " + policyHolder.LastName + ")";
                                dfd.resolve(payorModel);
                            } else {
                                payorModel.policyHolderRelationship = '';
                                dfd.resolve(payorModel);
                            }
                        } else {
                            payorModel.policyHolderRelationship = '';
                            dfd.resolve(payorModel);
                        }
                    });
                }
                return dfd.promise;
            };

            var getAdmissionDetails = function (admissions, reportModel) {
                var dfd = $q.defer();
                var admissionPromise = [];
                angular.forEach(admissions, function (admission) {
                    var admn = {
                        company: admission.CompanyID ? getOrganizationText('Company', admission.CompanyID) : '',
                        division: admission.DivisionID ? getOrganizationText('Division', admission.DivisionID) : '',
                        program: admission.ProgramID ? getOrganizationText('Program', admission.ProgramID) : '',
                        programUnit: admission.ProgramUnitID ? getOrganizationText('ProgramUnit', admission.ProgramUnitID) : '',
                        programUnitStatus: admission.IsDischarged ? 'Inactive' : 'Active',
                        admissionDateTime: getDateOrEmpty(admission.EffectiveDate),
                        dischargeDate: getDateOrEmpty(admission.DischargeDate),
                        dischargeSignedBy: getTextorEmpty(admission.SignedByEntityName),
                        dischargeDateSigned: getDateOrEmpty(admission.DateSigned),
                        effectiveDate: getDateOrEmpty(admission.EffectiveDate),
                        ModifiedOn: admission.ModifiedOn
                    };
                    admissionPromise.push(getDischarge(admission, admn, reportModel));
                });
                if (hasDetails(admissionPromise)) {
                    $q.all(admissionPromise).then(function (admnResp) {
                        admnResp = getPrimaryOrLatestDataItems(admnResp, 'dischargeDate', 'effectiveDate')
                        angular.forEach(admnResp, function (item) {
                            reportModel.admissionDischarge.push(item);
                        });
                        dfd.resolve(true);
                    });
                } else {
                    dfd.resolve(true);
                }
                return dfd.promise;
            }

            var getDischarge = function (admission, admn, reportModel) {
                var dfd = $q.defer();
                if (admission.DischargeDate) {
                    dischargeService.getDischargeNote(admission.ContactDischargeNoteID).then(function (disch) {
                        if (hasData(disch)) {
                            var dischargeNote = disch.DataItems[0];
                            admn.dischargeReason = getLookupTextById('DischargeReason', dischargeNote.DischargeReasonID);
                        }
                        else {
                            admn.dischargeReason = '';
                        }
                        dfd.resolve(admn);
                    });
                } else {
                    admn.dischargeReason = '';
                    dfd.resolve(admn);
                }
                return dfd.promise;
            };

            var getHouseholdDetails = function (financialAssessment, reportModel) {
                var dfd = $q.defer();
                reportModel.adjustedGrossIncome = $filter('currency')(getValueOrEmpty(financialAssessment.AdjustedGrossIncome));
                reportModel.totalIncome = $filter('currency')(getValueOrEmpty(financialAssessment.TotalIncome));
                reportModel.totalExpenses = $filter('currency')(getValueOrEmpty(financialAssessment.TotalExpenses));
                reportModel.totalExtraordinaryExpenses = $filter('currency')(getValueOrEmpty(financialAssessment.TotalExtraOrdinaryExpenses));
                reportModel.totalOther = $filter('currency')(getValueOrEmpty(financialAssessment.TotalOther));
                reportModel.assessmentDate = getDateOrEmpty(financialAssessment.AssessmentDate);
                reportModel.familySize = getValueOrEmpty(financialAssessment.FamilySize);
                reportModel.householdIncomeExpirationDate = getDateOrEmpty(financialAssessment.ExpirationDate);
                reportModel.householdIncomeExpirationReason = getLookupTextById('ExpirationReason', financialAssessment.ExpirationReasonID);
                financialAssessmentService.getDetails($scope.contactID, financialAssessment.FinancialAssessmentID).then(function (members) {
                    if (hasData(members)) {
                        reportModel.familyMember = [];
                        angular.forEach(members.DataItems, function (member) {
                            var familymem = {
                                familyMember: getLookupFieldById(relationshipTypeList, member.RelationshipTypeID),
                                amount: $filter('currency')(getValueOrEmpty(member.Amount)),
                                frequency: getTextorEmpty(member.Frequency),
                                type: getLookupTextById('CategoryType', member.CategoryTypeID),
                                category: getLookupTextById('Category', member.CategoryID)
                            };
                            reportModel.familyMember.push(familymem);
                        });
                        dfd.resolve(true);
                    }
                    else {
                        dfd.resolve(true);
                    }
                });
                return dfd.promise;
            }

            var getReferralDetails = function (headerID, headerContactID, referralobj) {
                var dfd = $q.defer();
                var pArray = [];
                pArray.push(referralHeaderService.getReferralHeader(headerID, headerContactID));
                pArray.push(registrationService.get(headerContactID));

                $q.all(pArray).then(function (refResp) {
                    //referral header service
                    var referralDetail = (hasData(refResp[0])) ? refResp[0].DataItems[0] : {};
                    referralobj.referralDate = getDateOrEmpty(referralDetail.ReferralDate);
                    referralobj.referralSource = getTextorEmpty(getLookupTextById('ReferralSource', referralDetail.ReferralSourceID));
                    referralobj.howDidReferrerHear = getTextorEmpty(getLookupTextById('ReferralOrigin', referralDetail.ReferralOriginID));
                    referralobj.referrerNameOfOrganization = getTextorEmpty(referralDetail.OtherOrganization);

                    //referral contact
                    var referralContact = (hasData(refResp[1])) ? refResp[1].DataItems[0] : {};
                    referralobj.referrerFirstName = getTextorEmpty(referralContact.FirstName);
                    referralobj.referrerLastName = getTextorEmpty(referralContact.LastName);
                    referralobj.referrerMiddleName = getTextorEmpty(referralContact.Middle);
                    referralobj.referrerPrefix = getTextorEmpty(getLookupTextById('PrefixType', referralContact.TitleID));
                    referralobj.referrerSuffix = getTextorEmpty(getLookupFieldById(suffixList, referralContact.SuffixID));
                    if (referralContact) {
                        var pSubArray = [];
                        pSubArray.push(contactAddressService.get(headerContactID, referralContact.ContactTypeID));
                        pSubArray.push(contactEmailService.get(headerContactID, referralContact.ContactTypeID));
                        pSubArray.push(contactPhoneService.get(headerContactID, referralContact.ContactTypeID));
                        $q.all(pSubArray).then(function (refSubResp) {
                            //referral address
                            if (hasData(refSubResp[0])) {
                                var refAddr = refSubResp[0].DataItems[0];
                                referralobj.referrerAddressType = getTextorEmpty(getLookupFieldById(addressTypeList, refAddr.AddressTypeID));
                                referralobj.referrerAddressLine1 = getTextorEmpty(refAddr.Line1);
                                referralobj.referrerAddressLine2 = getTextorEmpty(refAddr.Line2);
                                referralobj.referrerCity = getTextorEmpty(refAddr.City);
                                referralobj.referrerState = getTextorEmpty(getLookupFieldById(stateProvinceList, refAddr.StateProvince));
                                referralobj.referrerCounty = getTextorEmpty(getLookupFieldById(countyList, refAddr.County));
                                referralobj.referrerPostalCode = getTextorEmpty(refAddr.Zip);
                            }
                            //referral email
                            referralobj.referrerEmail = [];
                            if (hasData(refSubResp[1])) {
                                var refEmails = refSubResp[1].DataItems;
                                angular.forEach(refEmails, function (refEmail) {
                                    referralobj.referrerEmail.push({
                                        email: getTextorEmpty(refEmail.Email),
                                        emailPermissions: getTextorEmpty(getLookupFieldById(emailPermissionList, refEmail.EmailPermissionID))
                                    });
                                })
                            }
                            else {
                                referralobj.referrerEmail.push({
                                    email: '',
                                    emailPermissions: ''
                                });
                            }

                            //referral phone
                            referralobj.referrerPhone = [];
                            if (hasData(refSubResp[2])) {
                                var refPhones = refSubResp[2].DataItems;
                                angular.forEach(refPhones, function (refPhone) {
                                    referralobj.referrerPhone.push({
                                        phoneType: getTextorEmpty(getLookupFieldById(phoneTypeList, refPhone.PhoneTypeID)),
                                        phoneNumber: getTextorEmpty(refPhone.Number),
                                        phonePermissions: getTextorEmpty(getLookupFieldById(phonePermissionList, refPhone.PhonePermissionID))
                                    });
                                });
                            }
                            else {
                                referralobj.referrerPhone.push({
                                    phoneType: '',
                                    phoneNumber: '',
                                    phonePermissions: ''
                                });
                            }
                        }).finally(function () {
                            dfd.resolve(true);
                        });
                    } else {
                        dfd.resolve(true);
                    }

                });
                return dfd.promise;
            };

            var getCollateralDetails = function (reportModel) {
                var dfd = $q.defer();
                var pushProm = [];
                angular.forEach(getPrimaryOrLatestDataItems(printModel.collateral), function (collateral) {
                    pushProm.push(pushCollateralDetails(collateral, reportModel));
                });
                $q.all(pushProm).then(function (collResp) {
                    angular.forEach(collResp, function (item) {
                        reportModel.collateral.push(item);
                    });
                    dfd.resolve(true);
                })
                return dfd.promise;
            };

            var pushCollateralDetails = function (collateral, reportModel) {
                var dfdPushCol = $q.defer();
                var collateralArr = [];
                reportModel.collateral = [];
                var collateralItem = {
                    firstName: getTextorEmpty(collateral.FirstName),
                    lastName: getTextorEmpty(collateral.LastName),
                    middleName: getTextorEmpty(collateral.Middle),
                    suffix: getLookupFieldById(suffixList, collateral.SuffixID),
                    gender: getLookupFieldById($scope.GenderList, collateral.GenderID),
                    dob: getDateOrEmpty(collateral.DOB),
                    livingWithContact: collateral.LivingWithClientStatus ? 'Yes' : 'No',
                    receiveCorrespondence: getLookupTextById("ReceiveCorrespondence", collateral.ReceiveCorrespondenceID),
                    employmentStatus: getLookupFieldById(employmentStatusList, collateral.EmploymentStatusID),
                    militaryStatus: getLookupFieldById(veteranStatusList, collateral.VeteranStatusID),
                    ssn: getTextorEmpty(collateral.SSN),
                    driverLicense: getTextorEmpty(collateral.DriverLicense),
                    driverLicenseState: getLookupFieldById(stateProvinceList, collateral.DriverLicenseStateID),
                    effectiveDate: getDateOrEmpty(collateral.CollateralEffectiveDate),
                    expirationDate: getDateOrEmpty(collateral.CollateralExpirationDate)
                };

                var collateralOtherIdJson = [];
                angular.forEach(collateral.ClientAlternateIDs, function (alternateId) {
                    collateralOtherIdJson.push({
                        otherIDType: getLookupFieldById($scope.clientIdentifierTypeList, alternateId.ClientIdentifierTypeID),
                        otherIDNumber: getTextorEmpty(alternateId.AlternateID),
                        otherIDEffectiveDate: getDateOrEmpty(alternateId.EffectiveDate),
                        otherIDExpirationDate: getDateOrEmpty(alternateId.ExpirationDate)
                    });
                });
                if (!hasDetails(collateral.ClientAlternateIDs)) {
                    collateralOtherIdJson.push({
                        otherIDType: '',
                        otherIDNumber: '',
                        otherIDEffectiveDate: '',
                        otherIDExpirationDate: ''
                    });
                }
                collateralItem.otherID = collateralOtherIdJson;

                //For Email
                if (hasDetails(collateral.Emails)) {
                    var email = collateral.Emails[0];
                    collateralItem.email = getTextorEmpty(email.Email);
                    collateralItem.emailPermissions = getLookupFieldById(emailPermissionList, email.EmailPermissionID);
                }
                else {
                    collateralItem.email = '';
                    collateralItem.emailPermissions = ''
                }

                //For collateral address
                if (hasDetails(collateral.Addresses)) {
                    var addr = collateral.Addresses[0];
                    collateralItem.addressPermissions = getLookupFieldById(emailPermissionList, addr.MailPermissionID);
                    collateralItem.addressType = getLookupFieldById(addressTypeList, addr.AddressTypeID);
                    collateralItem.addressLine1 = getTextorEmpty(addr.Line1);
                    collateralItem.addressLine2 = getTextorEmpty(addr.Line2);
                    collateralItem.city = getTextorEmpty(addr.City);
                    collateralItem.state = getLookupFieldById(stateProvinceList, addr.StateProvince);
                    collateralItem.county = getLookupFieldById(countyList, addr.County);
                    collateralItem.postalCode = getTextorEmpty(addr.Zip);
                }
                else {
                    collateralItem.addressPermissions = '';
                    collateralItem.addressType = '';
                    collateralItem.addressLine1 = '';
                    collateralItem.addressLine2 = '';
                    collateralItem.city = '';
                    collateralItem.state = '';
                    collateralItem.county = '';
                    collateralItem.postalCode = '';
                }

                //For collateral phone
                if (hasDetails(collateral.Phones)) {
                    var phone = collateral.Phones[0];
                    collateralItem.phoneType = getLookupFieldById(phoneTypeList, phone.PhoneTypeID);
                    collateralItem.phoneNumber = getTextorEmpty(phone.Number);
                    collateralItem.phonePermissions = getLookupFieldById(phonePermissionList, phone.PhonePermissionID);
                }
                else {
                    collateralItem.phoneType = '';
                    collateralItem.phoneNumber = '';
                    collateralItem.phonePermissions = '';
                }

                collateralItem.collateralTypeRelationship = [];
                contactRelationshipService.get(collateral.ContactID, $scope.contactID).then(function (response) {
                    if (hasData(response)) {
                        var contactRelationships = response.DataItems;
                        angular.forEach(contactRelationships, function (item) {
                            collateralItem.collateralTypeRelationship.push({
                                collateralType: item.RelationshipGroupID ? $filter('filter')(lookupService.getLookupsByType("CollateralType"), { RelationshipGroupID: item.RelationshipGroupID }, true)[0].Name : '',
                                policyHolder: item.IsPolicyHolder ? 'Yes' : 'No',
                                collateralRelationship: getLookupFieldById(relationshipTypeList, item.RelationshipTypeID),
                                collateralTypeEffectiveDate: getDateOrEmpty(item.EffectiveDate),
                                collateralTypeExpirationDate: getDateOrEmpty(item.ExpirationDate)
                            });
                        })
                        dfdPushCol.resolve(collateralItem);
                    }
                    else {
                        dfdPushCol.resolve(collateralItem);
                    }
                });
                return dfdPushCol.promise;
            };
            var policyHolderInfo = function (contactID) {
                var possiblePolicyHolders = [];
                possiblePolicyHolders.push($scope.header);
                angular.forEach(printModel.collateral, function (item) {
                    possiblePolicyHolders.push(item);
                })
                //$scope.header.push(printModel.collateral);
                var result = $.grep(possiblePolicyHolders, function (e) {
                    return e.ContactID == contactID;
                })[0];
                return result;
            };
            var hasSameCard = function (model) {
                var billingContact = policyHolderInfo(model.ContactID);
                return (model.BillingFirstName === billingContact.FirstName
               && model.BillingMiddleName === billingContact.Middle
               && model.BillingLastName === billingContact.LastName
               && model.BillingSuffixID === billingContact.SuffixID)
            };
            $scope.init();
        }
    ]);
