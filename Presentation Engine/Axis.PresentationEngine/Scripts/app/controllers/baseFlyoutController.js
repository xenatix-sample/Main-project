(function () {
    angular.module('xenatixApp')
        .controller('baseFlyoutController', ['$scope', 'lookupService', '$injector', '$filter', function ($scope, lookupService, $injector, $filter) {
            var contactTypeID = 1;
            $scope.contactModel = {};
            var setDemographicScope = function (contactID, registrationData) {
                var model = {};
                model.FirstName = registrationData.FirstName;
                model.LastName = registrationData.LastName;
                model.MiddleName = registrationData.MiddleName;

                model.ProgramUnit = lookupService.getText('ClientType', registrationData.ClientTypeID);
                model.DOB = registrationData.DOB ? $filter('toMMDDYYYYDate')(registrationData.DOB, 'MM/DD/YYYY') : '';
                model.Gender = lookupService.getText('Gender', registrationData.GenderID);
                model.PrimaryContactMethod = lookupService.getText('ContactMethod', registrationData.ContactMethodID)
                model.MRN = registrationData.MRN;
                var profileDefaultImg = registrationData.GenderID == 1 ? "profile_male.svg" : "profile_female.svg";
                model.ThumbnailBLOB = "Images/" + profileDefaultImg;
                contactTypeID = registrationData.ContactTypeID;
                $scope.contactModel = model;
                //Call Photo
                if ($injector.has('contactPhotoService')) {
                    var contactPhotoService = $injector.get('contactPhotoService');
                    contactPhotoService.getContactPhoto(contactID).then(function (contactPhotoResponse) {
                        if (hasData(contactPhotoResponse)) {
                            var profilePic = $filter('filter')(contactPhotoResponse.DataItems, { IsPrimary: true }, true);
                            if (profilePic && profilePic.length > 0) {
                                if ($injector.has('photoService')) {
                                    var photoService = $injector.get('photoService');
                                    photoService.getPhoto(profilePic[0].PhotoID).then(setPhoto);
                                }
                            }
                        }
                    })
                }

                //Call Phone
                if ($injector.has('contactPhoneService')) {
                    var contactPhoneService = $injector.get('contactPhoneService');
                    contactPhoneService.get(contactID, contactTypeID).then(setPhone);
                }
                //Call Email
                if ($injector.has('contactEmailService')) {
                    var contactEmailService = $injector.get('contactEmailService');
                    contactEmailService.get(contactID, contactTypeID).then(setEmail);
                }

                //Call Address
                if ($injector.has('contactAddressService')) {
                    var contactAddressService = $injector.get('contactAddressService');
                    contactAddressService.get(contactID, contactTypeID).then(setAddress);
                }
                //Call Admission
                if ($injector.has('admissionService')) {
                    var admissionService = $injector.get('admissionService');
                    admissionService.get(contactID).then(setContactStatus);
                }
                
            }
            var clientTypeList = lookupService.getLookupsByType('ClientType');
            var setPhone = function (phoneModel) {
                if (hasData(phoneModel)) {
                    $scope.contactModel.Phone = $filter('toPhone')(getPrimaryOrLatestData(phoneModel.DataItems)[0].Number); // phoneModel.DataItems[0].Number;
                }
            }

            var setEmail = function (emailModel) {
                if (hasData(emailModel)) {
                    $scope.contactModel.Email = emailModel.DataItems[0].Email;
                }
            }

            var setAddress = function (addressModel) {

                if (hasData(addressModel)) {
                    addModel = getPrimaryOrLatestData(addressModel.DataItems)[0];
                    var addressLineFormat = ((checkModel(addModel.Line1)) ? addModel.Line1 + " " : "") +
                                    ((checkModel(addModel.Line2)) ? addModel.Line2 : "");

                    if (checkModel(addressLineFormat)) {
                        $scope.contactModel.AddressLineFormat = addressLineFormat;
                    }

                    var addressCityFormat = ((checkModel((addModel.City)) ? addModel.City : "") + ((checkModel(addModel.StateProvince) && (addModel.StateProvince > 0)) ?
                    ((checkModel(addModel.City)) ? ", " + lookupService.getText("StateProvince", addModel.StateProvince) :
                        lookupService.getText("StateProvince", addModel.StateProvince)) : "") + ((checkModel(addModel.Zip)) ? " " + addModel.Zip : ""));
                    if (checkModel(addressCityFormat)) {
                        $scope.contactModel.AddressCityFormat = addressCityFormat
                    }

                }
            }

            var setContactStatus = function (addmissionModel) {
                if (hasData(addmissionModel)) {
                    var contactStatus = $filter('filter')(addmissionModel.DataItems, function (item) {
                        return item.DataKey == 'Company' && item.IsDischarged == false;
                    }, true)
                    $scope.contactModel.Status = contactStatus.length ? 'Active' : 'Inactive';
                }
            }

            var setPhoto = function (photoModel) {
                if (hasData(photoModel)) {
                    var data = photoModel.DataItems[0];
                    $scope.contactModel.ThumbnailBLOB = "data:image/jpeg;base64," + photoModel.DataItems[0].ThumbnailBLOB;
                }
            }

            openFlyout = function (contactID) {
                if ($injector.has('registrationService')) {
                    $('.row-offcanvas').addClass('active');
                    var registrationService = $injector.get('registrationService');
                    return registrationService.get(contactID).then(function (data) {
                        if (data == null || data.DataItems.length === 0) {
                            //no data so let's try the ecidemographicservice
                            if ($injector.has('eciDemographicService')) {
                                var eciDemographicService = $injector.get('eciDemographicService');
                                eciDemographicService.get(contactID).then(function (ecidata) {
                                    if (ecidata !== null && ecidata !== undefined && ecidata.DataItems.length > 0) {
                                        demographicData = ecidata.DataItems[0];
                                        return setDemographicScope(contactID, demographicData);
                                    }
                                });
                            }
                        } else {
                            demographicData = data.DataItems[0];
                            return setDemographicScope(contactID, demographicData);
                        }
                    });

                }
            }

            $scope.$on('openFlyout', function (event, args) {
                if (args && args.ContactID)
                    openFlyout(args.ContactID);
            });
        }]);
})();
