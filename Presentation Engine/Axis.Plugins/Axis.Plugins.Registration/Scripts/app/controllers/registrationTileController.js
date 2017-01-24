(function () {
    angular.module('xenatixApp')
        .controller('registrationTileController', [
            '$scope', '$filter', 'alertService', 'lookupService', '$stateParams', '$q', '$injector', 'additionalDemographyService', 'registrationService', 'contactBenefitService', 'collateralService', 'contactAddressService', 'contactEmailService', 'contactPhoneService', 'roleSecurityService', 'referralAdditionalDetailService', 'referralHeaderService', 'admissionService', 'contactRaceService', 'httpLoaderInterceptor',
            function ($scope, $filter, alertService, lookupService, $stateParams, $q, $injector, additionalDemographyService, registrationService, contactBenefitService, collateralService, contactAddressService, contactEmailService, contactPhoneService, roleSecurityService, referralAdditionalDetailService, referralHeaderService, admissionService, contactRaceService, httpLoaderInterceptor) {
                httpLoaderInterceptor.ignore(true);
                $scope.tileInfo = [];
                $scope.listLimit = 5;
                var demographicAddresses = [];
                var demographicEmails = [];
                var demographicPhones = [];
                var restrictedTextLength = 40;
                var contactMethodMsg = "Preferred Contact Method";
                var preferredMethodID;
                var defaultExpirationProp = 'ExpirationDate';
                var defaultEffectiveonProp = 'EffectiveDate';
                var collateralExpirationProp = 'CollateralExpirationDate';
                var collateralEffectiveProp = 'CollateralEffectiveDate';

                var getadditionalDemographyTile = function (contactID) {
                    var permissionKey = GeneralPermissionKey.General_General_AdditionalDemographics;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var tileInfoModel = initTiles("Additional Demographics", "additional", contactID, false, null, permissionKey, PERMISSION.CREATE);

                        additionalDemographyService.getAdditionalDemographic(contactID).then(function (additionalDemographic) {
                            var result = [];
                            if ((additionalDemographic.DataItems != null) && (additionalDemographic.DataItems.length > 0) && additionalDemographic.DataItems[0].AdditionalDemographicID != 0) {
                                var model = additionalDemographic.DataItems[0];

                                if (checkModel(model.SchoolDistrictID)) {
                                    result.push(getTileDetailsModel("SchoolDistrict", model.SchoolDistrictID, "School District"));
                                }

                                // for Race
                                contactRaceService.get(contactID).then(function (raceLookupList) {
                                    if (raceLookupList.DataItems.length > 0) {
                                        var selectedRaceList = raceLookupList.DataItems;
                                        var raceNameList = '';
                                        var raceLookUpList = lookupService.getLookupsByType("Race");
                                        for (var i = 0; i < selectedRaceList.length; i++) {
                                            for (var j = 0; j < raceLookUpList.length; j++) {
                                                if (selectedRaceList[i].RaceID == raceLookUpList[j].ID) {
                                                    raceNameList += raceLookUpList[j].Name + ', ';
                                                    break;
                                                }
                                            }
                                        }
                                        result.push(getTileDetailsModel(null, raceNameList.substring(0, raceNameList.length - 2), "Race"));
                                    }
                                });

                                if (checkModel(model.EthnicityID)) {
                                    if (parseInt(model.EthnicityID) == Other_TYPE.EthnicityID) {
                                        if (model.OtherEthnicity && model.OtherEthnicity != "")
                                            result.push(getTileDetailsModel(null, model.OtherEthnicity, "Ethnicity"));
                                        else
                                            result.push(getTileDetailsModel(null, "Other", "Ethnicity"));
                                    }
                                    else
                                        result.push(getTileDetailsModel("Ethnicity", model.EthnicityID, "Ethnicity"));
                                }
                                if (checkModel(model.LegalStatusID)) {
                                    if (parseInt(model.LegalStatusID) == Other_TYPE.LegalStatusID) {
                                        if (model.OtherLegalstatus && model.OtherLegalstatus != "")
                                            result.push(getTileDetailsModel(null, model.OtherLegalstatus, "Legal Status"));
                                        else
                                            result.push(getTileDetailsModel(null, "Other", "Legal Status"));
                                    }
                                    else
                                        result.push(getTileDetailsModel("LegalStatus", model.LegalStatusID, "Legal Status"));
                                }

                                if (checkModel(model.MaritalStatusID)) {
                                    result.push(getTileDetailsModel("MaritalStatus", model.MaritalStatusID, "Marital Status"));
                                }
                            }
                            tileInfoModel.TileDetails = result;
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                                tileInfoModel.IsLoaded = true;
                            });
                    }
                    return tileInfoModel;
                };
                var referralModel = {};
                var getReferralTile = function (contactID) {
                    var permissionKey = GeneralPermissionKey.General_General_Referral;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var tileInfoModel = initTiles("Referral", "referral", contactID, true, null, permissionKey, PERMISSION.CREATE);

                        referralAdditionalDetailService.getReferralDetails(contactID).then(function (referralData) {
                            var result = [];
                            if ((referralData.DataItems != null) && (referralData.DataItems.length > 0)) {
                                referralModel = $filter('orderBy')(referralData.DataItems, 'ModifiedOn', true)[0];
                                var headerModel = {};
                                var contactModel = {};
                                var headerID = referralModel.ReferralHeaderID;
                                var headerContactID = referralModel.ContactID;
                                referralHeaderService.getReferralHeader(headerID, headerContactID).then(function (headerData) {
                                    if ((headerData.DataItems != null) && (headerData.DataItems.length > 0)) {
                                        headerModel = headerData.DataItems[0];
                                    }
                                    registrationService.get(headerContactID).then(function (contactData) {
                                        if ((contactData.DataItems != null) && (contactData.DataItems.length > 0)) {
                                            contactModel = contactData.DataItems[0];
                                            tileInfoModel.ShowShortcuts = true;
                                            var formattedDate = $filter('toMMDDYYYYDate')(headerModel.ReferralDate, 'MM/DD/YYYY');
                                            result.push(getTileDetailsModel(null, formattedDate, "Referral Date"))
                                            if (checkModel(contactModel.FirstName)) {
                                                result.push(getTileDetailsModel(null, contactModel.FirstName + ' ' + ((contactModel.LastName == null) ? '' : contactModel.LastName), "Referral Name"));
                                            }

                                            if (checkModel(referralModel.ReferralConcern)) {
                                                if (referralModel.ReferralConcern.length > restrictedTextLength)
                                                    referralModel.ReferralConcern = referralModel.ReferralConcern.substr(0, restrictedTextLength) + '...';
                                                result.push(getTileDetailsModel(null, referralModel.ReferralConcern, "Concern"));
                                            }
                                            tileInfoModel.TileDetails = result;
                                            tileInfoModel.IsLoaded = true;
                                        }
                                        angular.forEach(referralData.DataItems, function (item) {
                                            
                                            tileInfoModel.EditDetails.push({
                                                modifiedDate: $filter('toMMDDYYYYDate')(item.ModifiedOn, 'MM/DD/YYYY'),
                                                item: getTextorEmpty(getLookupTextById('ReferralSource', item.ReferralSourceID)),
                                                state: "patientprofile.general.referral",
                                                contactID: contactID,
                                                params: { ReferralContactID: item.ContactID, ReferralHeaderID: item.ReferralHeaderID }
                                            });
                                        });

                                    }).finally(function () {
                                        tileInfoModel.IsLoaded = true;
                                    });

                                }).finally(function () {
                                    tileInfoModel.IsLoaded = true;
                                });
                            }
                        },
                            function (errorStatus) {
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                                tileInfoModel.IsLoaded = true;

                            }).finally(function () {
                                tileInfoModel.IsLoaded = true;
                            });
                    }
                    return tileInfoModel;
                };

                var getLookupTextById = function (lookupType, ID) {
                    return ID && lookupType ? lookupService.getText(lookupType, ID) : '';
                }

                var getAddressTile = function (contactID) {
                    var permissionKey = GeneralPermissionKey.General_General_Address;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var addressInfoModel = initTiles("Address", "address", contactID, true, null, permissionKey, PERMISSION.CREATE);

                        var resultAddress = [];
                        var contactAddresses = [];
                        contactAddressService.get(contactID).then(function (contactAddress) {
                            if (((contactAddress.DataItems != null) && (contactAddress.DataItems.length > 0)) || (demographicAddresses != null && demographicAddresses.length > 0)) {
                                if (contactAddress.DataItems.length > 0 && demographicAddresses.length > 0) {
                                    contactAddress.DataItems = $.grep(contactAddress.DataItems, function (itm) {
                                        return itm.ContactAddressID != demographicAddresses[0].ContactAddressID;
                                    });
                                    var mergedAddresses = contactAddress.DataItems.concat(demographicAddresses);
                                } else if (contactAddress.DataItems.length > 0) {
                                    var mergedAddresses = contactAddress.DataItems;
                                } else {
                                    var mergedAddresses = demographicAddresses;
                                }

                                // Code added to remove records which are expired. 
                                mergedAddresses = filterFutureOrExpiredRecords(mergedAddresses, defaultExpirationProp, defaultEffectiveonProp);

                                contactAddresses = $filter('filter')(mergedAddresses, {
                                    IsPrimary: true
                                }).length > 0 ?
                                    angular.copy($filter('filter')(mergedAddresses, {
                                        IsPrimary: true
                                    }))
                                    : angular.copy($filter('orderBy')(mergedAddresses, 'EffectiveDate', true));
                                if (preferredMethodID == PREFERRED_CONTACT_METHOD.Mail)
                                    resultAddress.push(getTileDetailsModel(null, null, contactMethodMsg));
                                if ((contactAddresses != null) && (contactAddresses.length > 0)) {

                                    var addModel = contactAddresses[0];

                                    if (checkModel(addModel.AddressTypeID)) {
                                        resultAddress.push(getTileDetailsModel("AddressType", addModel.AddressTypeID, "Address"));
                                    } else {
                                        resultAddress.push(getTileDetailsModel(null, "", "Address"));
                                    }

                                    var addressLineFormat = ((checkModel(addModel.Line1)) ? addModel.Line1 + " " : "") +
                                    ((checkModel(addModel.Line2)) ? addModel.Line2 : "");

                                    if (checkModel(addressLineFormat)) {
                                        resultAddress.push(getTileDetailsModel(null, addressLineFormat, ""));
                                    }

                                    var addressCityFormat = ((checkModel((addModel.City)) ? addModel.City : "") + ((checkModel(addModel.StateProvince) && (addModel.StateProvince > 0)) ?
                                    ((checkModel(addModel.City)) ? ", " + $scope.getText("StateProvince", addModel.StateProvince) :
                                        $scope.getText("StateProvince", addModel.StateProvince)) : "") + ((checkModel(addModel.Zip)) ? " " + addModel.Zip : ""));

                                    if (checkModel(addressCityFormat)) {
                                        resultAddress.push(getTileDetailsModel(null, addressCityFormat, ""));
                                    }

                                    //Add the Primary Flag details
                                    resultAddress.push(getTileDetailsModel(null,
                                            (checkModel(addModel.IsPrimary) && addModel.IsPrimary === true ? 'Yes' : 'No'),
                                            "Primary"));

                                    //Add the EFfectiveDate details
                                    //  [Bug:11724] removed 'useLocal' from date formatting filter.
                                    resultAddress.push(getTileDetailsModel(null,
                                        (checkModel(addModel.EffectiveDate)
                                            ? $filter('toMMDDYYYYDate')(addModel.EffectiveDate, 'MM/DD/YYYY')
                                            : ''),
                                        "Effective Date"));

                                    //addresss
                                    if (resultAddress.length > 0) {
                                        addressInfoModel.TileDetails = resultAddress;
                                    }
                                }
                                angular.forEach(mergedAddresses, function (item) {
                                    var address = ((checkModel(item.Line1)) ? item.Line1 + " " : "") +
                                    ((checkModel(item.Line2)) ? item.Line2 : "")
                                    addressInfoModel.EditDetails.push({
                                        modifiedDate: $filter('toMMDDYYYYDate')(item.ModifiedOn, 'MM/DD/YYYY', 'useLocal'),
                                        item: address,
                                        state: "patientprofile.general.address",
                                        contactID: contactID,
                                        id: item.ContactAddressID
                                    });
                                });
                            }
                            addressInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                addressInfoModel.IsLoaded = true;
                                addressInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];

                            });
                    }
                    return addressInfoModel;
                };

                var getEmailTile = function (contactID) {
                    var permissionKey = GeneralPermissionKey.General_General_Email;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var emailInfoModel = initTiles("Email", "emails", contactID, true, null, permissionKey, PERMISSION.CREATE);
                        var resultEmail = [];
                        var contactEmails = [];

                        contactEmailService.get(contactID).then(function (contactEmail) {
                            if (((contactEmail.DataItems != null) && (contactEmail.DataItems.length > 0)) || (demographicEmails != null && demographicEmails.length > 0)) {
                                if (contactEmail.DataItems.length > 0 && demographicEmails.length > 0) {
                                    contactEmail.DataItems = $.grep(contactEmail.DataItems, function (itm) {
                                        return itm.ContactEmailID != demographicEmails[0].ContactEmailID;
                                    })
                                    var mergedEmails = contactEmail.DataItems.concat(demographicEmails);
                                } else if (contactEmail.DataItems.length > 0) {
                                    var mergedEmails = contactEmail.DataItems;
                                } else {
                                    var mergedEmails = demographicEmails;
                                }

                                // Code added to remove records which are expired. 
                                mergedEmails = filterFutureOrExpiredRecords(mergedEmails, defaultExpirationProp, defaultEffectiveonProp);

                                contactEmails = $filter('filter')(mergedEmails, { IsPrimary: true }).length > 0 ?
                                    angular.copy($filter('filter')(mergedEmails, { IsPrimary: true }))
                                    : angular.copy($filter('orderBy')(mergedEmails, 'EffectiveDate', true));


                                if ((contactEmails != null) && (contactEmails.length > 0)) {
                                    var eMailModel = contactEmails[0];
                                    if (preferredMethodID == PREFERRED_CONTACT_METHOD.Email)
                                        resultEmail.push(getTileDetailsModel(null, null, contactMethodMsg));

                                    resultEmail.push(getTileDetailsModel(null, eMailModel.Email, "Email"));

                                    //Add the Primary Flag details
                                    resultEmail.push(getTileDetailsModel(null,
                                                        (checkModel(eMailModel.IsPrimary) && eMailModel.IsPrimary === true
                                                            ? 'Yes'
                                                            : 'No'),
                                                        "Primary"));

                                    if (checkModel(eMailModel.EmailPermissionID)) {
                                        resultEmail.push(getTileDetailsModel("EmailPermission", eMailModel.EmailPermissionID, "Email Permission"));
                                    }

                                    resultEmail.push(getTileDetailsModel(null,
                                                        (checkModel(eMailModel.EffectiveDate)
                                                            ? $filter('toMMDDYYYYDate')(eMailModel.EffectiveDate, 'MM/DD/YYYY')
                                                            : ''),
                                                        "Effective Date"));

                                    //email
                                    if (resultEmail.length > 0)
                                        emailInfoModel.TileDetails = resultEmail;
                                }
                                angular.forEach(mergedEmails, function (item) {
                                    emailInfoModel.EditDetails.push({
                                        modifiedDate: $filter('toMMDDYYYYDate')(item.ModifiedOn, 'MM/DD/YYYY', 'useLocal'),
                                        item: item.Email,
                                        state: "patientprofile.general.emails",
                                        contactID: contactID,
                                        id: item.ContactEmailID
                                    });
                                });

                            }
                            emailInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                emailInfoModel.IsLoaded = true;
                                emailInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];

                            });
                    }
                    return emailInfoModel;
                };



                var getPhoneTile = function (contactID) {
                    var permissionKey = GeneralPermissionKey.General_General_Phone;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var phoneInfoModel = initTiles("Phone", "phone", contactID, true, null, permissionKey, PERMISSION.CREATE);
                        var resultPhone = [];
                        var contactPhones = [];

                        contactPhoneService.get(contactID, 1).then(function (contactPhone) {
                            if (((contactPhone.DataItems != null) && (contactPhone.DataItems.length > 0)) || (demographicPhones != null && demographicPhones.length > 0)) {
                                if (contactPhone.DataItems.length > 0 && demographicPhones.length > 0) {
                                    contactPhone.DataItems = $.grep(contactPhone.DataItems, function (itm) {
                                        return itm.ContactPhoneID != demographicPhones[0].ContactPhoneID;
                                    });
                                    var mergedPhones = contactPhone.DataItems.concat(demographicPhones);
                                } else if (contactPhone.DataItems.length > 0) {
                                    var mergedPhones = contactPhone.DataItems;
                                } else {
                                    var mergedPhones = demographicPhones;
                                }

                                // Code added to remove records which are expired. 
                                mergedPhones = filterFutureOrExpiredRecords(mergedPhones, defaultExpirationProp, defaultEffectiveonProp);

                                contactPhones = $filter('filter')(mergedPhones, { IsPrimary: true }).length > 0 ?
                                    angular.copy($filter('filter')(mergedPhones, {
                                        IsPrimary: true
                                    }))
                                    : angular.copy($filter('orderBy')(mergedPhones, 'EffectiveDate', true));

                                if ((contactPhones != null) && (contactPhones.length > 0)) {
                                    var phoneModel = contactPhones[0];

                                    if (preferredMethodID == PREFERRED_CONTACT_METHOD.Phone)
                                        resultPhone.push(getTileDetailsModel(null, null, contactMethodMsg));

                                    if (checkModel(phoneModel.PhoneTypeID)) {
                                        resultPhone.push(getTileDetailsModel("PhoneType", phoneModel.PhoneTypeID, "Phone Type"));
                                    }
                                    resultPhone.push(getTileDetailsModel(null, $filter('toPhone')(phoneModel.Number), "Phone"));

                                    //Add the Primary Flag details
                                    resultPhone.push(getTileDetailsModel(null,
                                                        (checkModel(phoneModel.IsPrimary) && phoneModel.IsPrimary === true ? 'Yes' : 'No'),
                                                        "Primary"));
                                    
                                    if (checkModel(phoneModel.PhonePermissionID)) {
                                        resultPhone.push(getTileDetailsModel("PhonePermission", phoneModel.PhonePermissionID, "Permission"));
                                    }

                                    //Add the EFfectiveDate details
                                    //  [Bug:11724] removed 'useLocal' from date formatting filter.
                                    resultPhone.push(getTileDetailsModel(null,
                                                        (checkModel(phoneModel.EffectiveDate)
                                                            ? $filter('toMMDDYYYYDate')(phoneModel.EffectiveDate, 'MM/DD/YYYY')
                                                            : ''),
                                                        "Effective Date"));

                                    //phone
                                    if (resultPhone.length > 0) {
                                        phoneInfoModel.TileDetails = resultPhone;
                                    }

                                }
                                angular.forEach(mergedPhones, function (item) {
                                    phoneInfoModel.EditDetails.push({
                                        modifiedDate: $filter('toMMDDYYYYDate')(item.ModifiedOn, 'MM/DD/YYYY', 'useLocal'),
                                        item: $filter('toPhone')(item.Number),
                                        state: "patientprofile.general.phone",
                                        contactID: contactID,
                                        id: item.ContactPhoneID
                                    });
                                });
                            }
                            phoneInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                phoneInfoModel.IsLoaded = true;
                                phoneInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];

                            });
                    }
                    return phoneInfoModel;
                };

                var getCollateralTile = function (contactID, contactTypeID) {
                    var permissionKey = GeneralPermissionKey.General_General_Collateral;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var gotostate = "collateral";
                        if (contactTypeID === CONTACT_TYPE.Family_Relationship) {
                            gotostate = "family";
                        }
                        var tileInfoModel = initTiles("Collateral", gotostate, contactID, true, null, permissionKey, PERMISSION.CREATE);
                        collateralService.get(contactID, contactTypeID, false).then(function (collateralData) {
                            var result = [];
                            if ((collateralData.DataItems != null) && collateralData.DataItems.length > 0) {
                                model = null;
                                collateralData.DataItems = filterFutureOrExpiredRecords(collateralData.DataItems, collateralExpirationProp);
                                if (collateralData.DataItems.length > 0)
                                {
                                    $.each(collateralData.DataItems, function (index, value) {
                                        if (collateralData.DataItems[index].EmergencyContact != undefined && collateralData.DataItems[index].EmergencyContact != null &&
                                            collateralData.DataItems[index].EmergencyContact != "") {
                                            model = collateralData.DataItems[index];
                                            return false;
                                        }
                                    });
                                }
                                if (model == null)
                                    model = collateralData.DataItems[collateralData.DataItems.length - 1];
                                if (model != null) {
                                    //Display the Collateral name instead of getting from the Lookup data
                                    result.push(getTileDetailsModel(null, model.FirstName + " " + model.LastName, "Collateral Name"));

                                    
                                    var collateralPrimaryPhone = $filter('filter')(model.Phones, {
                                        IsPrimary: true
                                    });
                                    if (collateralPrimaryPhone.length > 0) {
                                        result.push(getTileDetailsModel("", $filter('toPhone')(collateralPrimaryPhone[0].Number), "Primary Phone"));
                                    }

                                    var collateralPrimaryEmail = $filter('filter')(model.Emails, {
                                        IsPrimary: true
                                    });
                                    if (collateralPrimaryEmail.length > 0) {
                                        result.push(getTileDetailsModel("", collateralPrimaryEmail[0].Email, "Primary Email"));
                                    }

                                    result.push(getTileDetailsModel(null, model.EmergencyContact ? "Yes" : "No", "Emergency Contact"));
                                }
                                angular.forEach(collateralData.DataItems, function (item) {
                                    tileInfoModel.EditDetails.push({
                                        modifiedDate: $filter('toMMDDYYYYDate')(item.ModifiedOn, 'MM/DD/YYYY', 'useLocal'),
                                        item: (item.FirstName + " " + item.LastName),
                                        state: "patientprofile.general.collateral",
                                        contactID: contactID,
                                        id: item.ContactRelationshipID
                                    });
                                });
                            }
                            tileInfoModel.TileDetails = result;
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.IsLoaded = true;
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];

                            });
                    }
                    return tileInfoModel;
                };
                
                var getDemographyTile = function (contactID) {
                    var permissionKey = GeneralPermissionKey.General_General_Demographics;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var demographyInfoModel = initTiles("Demographics", "demographics", contactID, false, null, permissionKey, PERMISSION.CREATE);
                        var resultDemography = [];

                        registrationService.get(contactID).then(function (contactDemographic) {
                            if ((contactDemographic.DataItems != null) && (contactDemographic.DataItems.length > 0)) {
                                var model = contactDemographic.DataItems[0];
                                //Demography Tile
                                if (checkModel(model)) {
                                    var components = 0;
                                    if (checkModel(model.SSN)) {
                                        resultDemography.push(getTileDetailsModel(null, $filter('toMaskSSN')(model.SSN), "SSN"));
                                        components++;
                                    }
                                    if (checkModel(model.DriverLicense)) {
                                        resultDemography.push(getTileDetailsModel(null, model.DriverLicense, "Driver License"));
                                        components++;
                                    }

                                    // Code added to remove records which are expired. 
                                    model.ClientAlternateIDs = filterFutureOrExpiredRecords(model.ClientAlternateIDs, defaultExpirationProp, defaultEffectiveonProp);

                                    if (checkModel(model.ClientAlternateIDs)) {
                                        for (var i = 0; i < Math.min(5 - components, model.ClientAlternateIDs.length) ; i++) {
                                            var otherID = model.ClientAlternateIDs[i].ClientIdentifierTypeID || 0;
                                            resultDemography.push(getTileDetailsModel(null, model.ClientAlternateIDs[i].AlternateID || '', lookupService.getText("ClientIdentifierType", otherID)));
                                        }
                                    }

                                    /* // old implementation
                                    if (checkModel(model.AlternateID)) {
                                        resultDemography.push(getTileDetailsModel(null, model.AlternateID, "Other ID Number"));
                                    }
                                    */

                                    demographicAddresses = checkModel(model.Addresses) && (model.Addresses.length > 0) ? model.Addresses : [];
                                    demographicEmails = checkModel(model.Emails) && (model.Emails.length > 0) ? model.Emails : [];
                                    demographicPhones = checkModel(model.Phones) && (model.Phones.length > 0) ? model.Phones : [];
                                } else {
                                    demographicAddresses = demographicEmails = demographicPhones = [];
                                }
                                //demography
                                if (resultDemography.length > 0)
                                    demographyInfoModel.TileDetails = resultDemography;
                            }
                            demographyInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                demographyInfoModel.IsLoaded = true;
                                demographyInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];

                            });
                    }
                    return demographyInfoModel;
                };

                var Admission_Type = {
                    Company: 'ModifiedOn',
                    Discharge: 'DischargeDate',
                    Admission: 'EffectiveDate'
                }

                var getAdmissionDischargeTile = function (contactID) {
                    var permissionKey = GeneralPermissionKey.General_General_AdmissionDischarge;

                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {

                        var navigationStates = [{ Name: 'Admission', State: 'admissionDischarge.admission', PermissionKey: GeneralPermissionKey.General_General_Admission },
                                                { Name: 'DischargeProgramUnit', State: 'admissionDischarge.dischargeProgramUnit', PermissionKey: GeneralPermissionKey.General_General_ProgramUnitDischarge },
                                                { Name: 'DischargeCompany', State: 'admissionDischarge.dischargeCompany', PermissionKey: GeneralPermissionKey.General_General_CompanyDischarge }];
                        var state = "";
                        for (var i = 0; i < navigationStates.length; i++) {
                            if (roleSecurityService.hasPermission(navigationStates[i].PermissionKey, PERMISSION.READ)) {
                                state = navigationStates[i].State;
                                break;
                            }
                        }

                        var admissionDischargeInfoModel = initTiles("Admission / Discharge", state, contactID, true, null, permissionKey, PERMISSION.CREATE);
                        var result = [];
                        admissionService.get(contactID).then(function (admissionData) {
                            if (hasData(admissionData)) {

                                //Get Admission records
                                let admissionDetails = $filter('filter')(admissionData.DataItems, function (item) {
                                    return (item.DataKey == 'ProgramUnit');
                                });

                                //Get Discharged records
                                let dischargeData = $filter('filter')(admissionData.DataItems, function (item) {
                                    return (item.DataKey == 'ProgramUnit') && item.IsDischarged == true;
                                });

                                //Get Company records
                                let companyData = $filter('filter')(admissionData.DataItems, function (item) {
                                    return (item.DataKey == 'Company');
                                });

                                // For Admission
                                populateAdmnTileDetails(result, admissionDetails, Admission_Type.Admission, COUNT_NUMERIC.Two);

                                // For Discharge
                                populateAdmnTileDetails(result, dischargeData, Admission_Type.Discharge, COUNT_NUMERIC.Two);

                                //For Company
                                populateAdmnTileDetails(result, companyData, Admission_Type.Company, COUNT_NUMERIC.One);

                                // As per the user story 8780, clicking on edit icon admission page should display.
                                //Checks for read permission and will navigate to respective screen of 'Admission/Discharge/Discharge Company'. By default(if user have all permissions) always navigate to Admission.
                                if (state != "") {
                                    admissionDischargeInfoModel.EditDetails.push({
                                        state: "." + state,
                                        params: {
                                            ContactID: contactID
                                        }
                                    });
                                }
                                else
                                    admissionDischargeInfoModel.EditDetails = [];
                            }

                            admissionDischargeInfoModel.TileDetails = result;
                            admissionDischargeInfoModel.IsLoaded = true;
                        },
                          function (errorStatus) {
                              admissionDischargeInfoModel.IsLoaded = true;
                              tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                          });
                    }
                    return admissionDischargeInfoModel;
                };

                var populateAdmnTileDetails = function (result, modelData, orderBy, maxAllowed) {
                    var programName = '';
                    var programDate = '';

                    if (hasDetails(modelData)) {
                        modelData = $filter('orderBy')(modelData, orderBy == Admission_Type.Discharge ? Admission_Type.Company : orderBy, true);
                        angular.forEach(modelData, function (model, idx) {
                            if (idx < maxAllowed) {     //pick only top 2
                                programName = lookupService.getText("Organizations", (orderBy == Admission_Type.Company) ? model.CompanyID : model.ProgramUnitID);
                                programDate = $filter('toMMDDYYYYDate')(model.IsDischarged ? (orderBy == Admission_Type.Admission ? model.EffectiveDate : model.DischargeDate) : model.EffectiveDate, 'MM/DD/YYYY');
                                (orderBy == Admission_Type.Company) ?
                                    result.push(getTileDetailsModel(null, (model.IsDischarged ? 'Inactive at ' : 'Active at ') + programName + ' (' + programDate + ')', idx === 0 ? "Company" : '')) :
                                    result.push(getTileDetailsModel(null, programName + ' (' + programDate + ')', idx === 0 ? ((orderBy == Admission_Type.Admission) ? 'Admitted' : 'Discharged') : ''));
                            }
                        });
                    }
                }


                $scope.get = function (contactID) {
                    var isECIClient = false;
                    var strECI = DIVISION_NAME.ECS;
                    var registrationPromise = [];
                    var contactDemographic = null;
                    registrationPromise.push(registrationService.get(contactID));
                    $q.all(registrationPromise).then(function (demographic) {
                        if (demographic !== undefined && demographic !== null && demographic[0].DataItems.length > 0) {
                            contactDemographic = demographic[0];
                            preferredMethodID = contactDemographic.DataItems[0].ContactMethodID;
                            var tmpClientTypeID = demographic[0].DataItems[0].ClientTypeID;
                            if (getText(tmpClientTypeID, $scope.getLookupsByType('ClientType')) === strECI) {
                                if ($injector.has('eciRegistrationTileService')) {
                                    isECIClient = true;
                                }
                            }
                        }//if there is no data available in the regular registration service call then we will assume that the client is an eci patient if the service is available
                        else if ($injector.has('eciRegistrationTileService')) {
                            isECIClient = true;
                        }

                        var tileModel = [];
                            var tilePromises = [];

                            if (isECIClient) {
                                var eciRegistrationTileService = $injector.get('eciRegistrationTileService');
                                tilePromises.push(getAdmissionDischargeTile(contactID));
                                tilePromises.push(eciRegistrationTileService.get(contactID, $scope.listLimit));
                                tilePromises.push(getAddressTile(contactID));
                                tilePromises.push(getPhoneTile(contactID));
                                tilePromises.push(getEmailTile(contactID));
                                tilePromises.push(getCollateralTile(contactID, CONTACT_TYPE.Family_Relationship));
                                tilePromises.push(getReferralTile(contactID));
                                //tilePromises.push(getPayorsTile(contactID,true));
                                //tilePromises.push(getFinancialAssessmentTile(contactID));
                                //tilePromises.push(getSelfPaySectionTile(contactID));
                                $q.all(tilePromises).then(function (infoModels) {
                                    var dischargeModel = infoModels.slice(0, 1);
                                    var demographicModels = infoModels[1].slice(0, 2);
                                    var middleTileModels = infoModels.slice(2, 7);
                                    var finalProfileTileSet = dischargeModel.concat(demographicModels.concat(middleTileModels));
                                    tileModel.push({ SectionName: "Profile", TileInfo: finalProfileTileSet });
                                    //tileModel.push({ SectionName: "Financial", TileInfo: infoModels.slice(6) });
                                    tileModel[0].TileInfo = removeNullFromArray(tileModel[0].TileInfo);
                                    $scope.Sections = tileModel;
                                });
                            } else {

                                tilePromises.push(getAdmissionDischargeTile(contactID));
                                tilePromises.push(getDemographyTile(contactID));
                                tilePromises.push(getadditionalDemographyTile(contactID));
                                tilePromises.push(getAddressTile(contactID));
                                tilePromises.push(getPhoneTile(contactID));
                                tilePromises.push(getEmailTile(contactID));
                                tilePromises.push(getCollateralTile(contactID, CONTACT_TYPE.Collateral));
                                tilePromises.push(getReferralTile(contactID));
                                //tilePromises.push(getPayorsTile(contactID,false));
                                //tilePromises.push(getFinancialAssessmentTile(contactID));
                                //tilePromises.push(getSelfPaySectionTile(contactID));
                                $q.all(tilePromises).then(function (infoModels) {
                                    tileModel.push({ SectionName: "Profile", TileInfo: infoModels.slice(0, 8) });
                                    //tileModel.push({ SectionName: "Financial", TileInfo: infoModels.slice(8) });
                                    tileModel[0].TileInfo = removeNullFromArray(tileModel[0].TileInfo);
                                    $scope.Sections = tileModel;
                                });
                            }
                    });
                };

                $scope.getText = function (lookUpType, value) {
                    return lookupService.getText(lookUpType, value);
                }

                $scope.get($stateParams.ContactID);
            }
        ]);
}());