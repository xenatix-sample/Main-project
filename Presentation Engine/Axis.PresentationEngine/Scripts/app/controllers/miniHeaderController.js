(function () {
    angular.module('xenatixApp')
        .controller('miniHeaderController', ['$scope', 'alertService', 'lookupService', '$filter', '$state', '$q', '$stateParams', '$rootScope', '$injector',
            function ($scope, alertService, lookupService, $filter, $state, $q, $stateParams, $rootScope, $injector) {
                $scope.contactID = $stateParams.ContactID;

                $scope.getHeaderDetails = function () {
                    if ($scope.contactID && $scope.contactID != 0 && $injector.has('registrationService')) {
                        var registrationService = $injector.get('registrationService');
                        registrationService.get($scope.contactID).then(function (data) {
                            if (hasData(data)) {
                                if (data.DataItems[0].DOB)
                                    data.DataItems[0].DOB = $filter('toMMDDYYYYDate')(data.DataItems[0].DOB, 'MM/DD/YYYY');
                                setHeaderModel(data.DataItems[0]);
                                handlePayors(data.DataItems[0].ContactID);                                
                            }
                        },
                        function (errorStatus) {
                            alertService.error('Unable to connect to server : ' + errorStatus);
                        });
                    }
                };

                var setMRN = function (mrn) {
                    $scope.profileView.MRN = mrn;
                    $rootScope.$broadcast('quickRegMRN', { Data: $scope.profileView.MRN ? true : false });
                }

                var setHeaderModel = function (demographics) {
                    if (!$scope.profileView)
                        $scope.profileView = {};
                    $scope.profileView.isECIClient = false;
                    setMRN(demographics.MRN);
                    $scope.profileView.DOB = demographics.DOB;
                    $scope.contactID = demographics.ContactID;
                    var fullName = demographics.FirstName;
                    if (demographics.Middle)
                        fullName = fullName + ' ' + demographics.Middle;
                    if (demographics.LastName)
                        fullName = fullName + ' ' + demographics.LastName;
                    if (demographics.SuffixID)
                        fullName = fullName + ' ' + lookupService.getText("Suffix", demographics.SuffixID);
                    $scope.profileView.FullName = fullName;
                    if (demographics.GenderID) {
                        var headerGender = $filter('filter')(lookupService.getLookupsByType('Gender'), { ID: demographics.GenderID }, true);
                        if (headerGender)
                            $scope.profileView.GenderName = headerGender[0].Name;
                    }
                    if(!$scope.profileView.MedicaidId)
                        handlePayors(demographics.ContactID);
                    if (demographics.ClientTypeID === PROGRAM_TYPE.ECI) {
                        $scope.profileView.isECIClient = true;                        
                    }
                    else
                        getContactPhoto(demographics.ContactID);
                }

                var handlePayors = function (contactID) {
                    if ($injector.has('contactBenefitService')) {
                        var contactBenefitService = $injector.get('contactBenefitService');
                        contactBenefitService.get(contactID).then(function (data) {
                             setPayorModel(data.DataItems);
                        });
                    }
                };

                var setPayorModel = function (payorData) {
                    if (hasDetails(payorData)) {
                        var payors = $filter('filter')(payorData, function (itm) {
                            return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                        });
                        if (hasDetails(payors)) {
                            $scope.profileView.MedicaidId = payors[0].PolicyID;
                        }
                    }
                    if(!$scope.profileView.MedicaidId)
                        $scope.profileView.MedicaidId = 'N/A';
                }

                var getContactPhoto = function (contactID) {
                    if ($injector.has('contactPhotoService') && $injector.has('photoService')) {
                        var contactPhotoService = $injector.get('contactPhotoService');
                        var photoService = $injector.get('photoService');
                        contactPhotoService.getContactPhoto(contactID).then(function (contactPhotoResponse) {
                            if (hasData(contactPhotoResponse)) {
                                var defer = $q.defer();
                                var promises = [];
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
                                $q.all(promises).then(function (photoes) {
                                    $scope.profileView.profilePicture = $filter('filter')(photoes, {
                                        IsPrimary: true
                                    })[0];
                                });
                            }
                            else {
                                $scope.profileView.profilePicture = null;
                            }
                        });
                    }
                };

                $rootScope.$on('updateRegistrationData', function (event, args) {
                    if (args.Type && args.Type == 'payor')
                        setPayorModel(args.Data)
                    else if (args.Type && args.Type == 'MRN')
                        setMRN(args.Data)
                    else
                        setHeaderModel(args.Data)
                });

                $scope.getHeaderDetails();

            }]);
})();

