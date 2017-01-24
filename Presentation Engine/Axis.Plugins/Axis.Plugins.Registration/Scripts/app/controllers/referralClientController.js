angular.module('xenatixApp')
    .controller('referralClientController', [
        '$scope', '$state', '$q', '$stateParams', '$rootScope', 'formService', 'lookupService', '$filter', 'referralClientInformationService', 'alertService', 'referralHeaderService', 'globalObjectsService', '$controller',
function ($scope, $state, $q, $stateParams, $rootScope, formService, lookupService, $filter, referralClientInformationService, alertService, referralHeaderService, globalObjectsService, $controller)
{
            $controller('baseContactController', { $scope: $scope });
            $scope.PhoneAccessCode = $scope.PHONE_ACCESS.ConditionalRequired | $scope.PHONE_ACCESS.Number;;
            $scope.EnableFilter = true;
            $scope.permissionKey = $state.current.data.permissionKey;
            var resetForm = function ()
            {
                $rootScope.formReset($scope.ctrl.referralClientForm);
            };

            $scope.getLookupsByType = function (typeName)
            {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.get = function (ReferralHeaderID)
            {
                $scope.isLoading = true;
                return referralClientInformationService.getReferralClientInformation(ReferralHeaderID).then(function (data)
                {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0)
                    {
                        var referralClientInformation = data.DataItems[0];
                        $scope.AdditionalDetails = referralClientInformation.referralClientAdditionalDetails;
                        $scope.Demographics = referralClientInformation.clientDemographicsModel;
                        $scope.permissionID = $scope.Demographics.ContactID;
                        $scope.Addresses = referralClientInformation.Addresses;
                        if ($scope.Addresses.length == 0)
                            initAddresses();

                        $scope.Phones = referralClientInformation.Phones;
                        if ($scope.Phones.length == 0)
                            initPhones();
                        setPlusMinusButtons($scope.Phones)

                        $scope.Concern = referralClientInformation.Concern;
                        resetForm();
                    }
                    else {
                        $scope.permissionID = 0;
                    }
                },
                    function (errorStatus)
                    {
                        alertService.error('Unable to get clientInformation: ' + errorStatus);
                    }).finally(function ()
                    {
                         if($scope.isReadOnlyForm) {
                            $scope.Phones = $filter("filter") ($scope.Phones, function (obj) {
                                obj.ShowPlusButton = false;
                                obj.ShowMinusButton = false;
                                return obj;
                            });
                         }

                        $rootScope.$broadcast('referrals.client',
                        {
                            validationState: (($scope.Demographics != null && $scope.Demographics.ContactID != undefined && $scope.Demographics.ContactID !=0) ? 'valid': 'warning') });
                        $scope.isLoading = false;
                    });
            };


            initReferralClientInformation = function ()
            {
                $scope.Header = {};
                initAdditionalDetails();
                initDemographics();
                initConcerns();
                initAddresses();
                initPhones();
            };

            initAdditionalDetails = function ()
            {
                $scope.AdditionalDetails = {};
            };

            initDemographics = function ()
            {
                $scope.Demographics = {};
            };

            initConcerns = function ()
            {
                $scope.Concern = {};
                };

            initAddresses = function ()
            {
                $scope.Addresses = [{
                    ContactAddressID: 0,
                    ContactID: 0,
                    AddressID: 0,
                    AddressTypeID: null,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: 0,
                    County: 0,
                    Zip: '',
                    MailPermissionID: 1,
                    IsPrimary: true,
                    IsGateCode: false,
                    IsComplexName: false
                }];
            };

            initPhones = function () {
                $scope.Phones = [];
                $scope.Phones.push(objPhone());
            };
            objPhone = function ()
            {
                var obj = {
                    Index: null,
                    ContactPhoneID: 0,
                    ContactID: 0,
                    PhoneID: 0,
                    PhoneTypeID: null,
                    Number: '',
                    PhonePermissionID: null,
                    IsPrimary: true,
                    ShowPlusButton: true,
                    ShowMinusButton: true,
                    IsActive: true
                };
                return obj;
            }

            $scope.addNewPhone = function ()
            {
                globalObjectsService.setViewContent();
                $scope.Phones = $filter("filter")($scope.Phones, function (obj)
                {
                    obj.ShowPlusButton = false;
                    return obj;
                });
                var priPhones = $filter("filter")($scope.Phones, function (obj) {
                    return (obj.IsPrimary == true);
                });
                var phoneToAdd = objPhone();
                if (priPhones && priPhones.length > 0)
                    phoneToAdd.IsPrimary = false;
                $scope.Phones.push(phoneToAdd);
            }

            $scope.removePhone = function (removeIndex)
            {
                globalObjectsService.setViewContent();
                var activePhones = $filter('filter')($scope.Phones, function (data)
                {
                    return data.IsActive === true;
                });
                var filteredPhones = $filter('filter')(activePhones, function (item, index)
                {
                    if (removeIndex == index)
                    {
                        if (item.ContactPhoneID !== 0)
                        {
                            item.IsActive = false;
                            return item;
                        }
                    }
                    else
                    {
                        return item;
                    }
                });

                $scope.Phones = filteredPhones;
                setPlusMinusButtons($scope.Phones);
            }

            setPlusMinusButtons = function (items)
            {
                var filterdItems = $filter('filter')(items, function (data)
                {
                    return data.IsActive === true;
                });

                angular.forEach(filterdItems, function (data, index)
                {
                    data.ShowMinusButton = true;
                    if (index == filterdItems.length - 1)
                        data.ShowPlusButton = true;
                });
            }

        

            $scope.init = function ()
            {
                $scope.$parent['autoFocus'] = true;
                $scope.ReferralHeaderID = $stateParams.ReferralHeaderID;
                initReferralClientInformation();
                $scope.immunizationStatuses = lookupService.getLookupsByType("ImmunizationStatus");
                $scope.referralConcernTypes = lookupService.getLookupsByType("ReferralProblemType");
                $scope.referralPriorities = lookupService.getLookupsByType("ReferralPriority");
                $scope.initReferralParent($scope.ReferralHeaderID,$stateParams.ContactID);
                if (($scope.ReferralHeaderID !== 0) && ($scope.ReferralHeaderID != null) && ($scope.ReferralHeaderID != undefined))
                {
                    $scope.ECI = true;
                    $scope.get($scope.ReferralHeaderID);
                    checkFormStatus();
                }
               
            };

            checkFormStatus = function () {
                $scope.$watch('ctrl.referralClientForm.$valid', function (newValue) {
                    if (newValue !== undefined) {
                        var id = 0;
                        if ($scope.Demographics != null && $scope.Demographics.ContactID != null) {
                            id = $scope.Demographics.ContactID;
                        }
                        $rootScope.$broadcast('referrals.client', {
                            validationState: ((id != null && id > 0) || newValue ? 'valid' : 'warning') });
                }
               
                });
            };

            $scope.saveClientInformation = function (isUpdate, referralClientInformation)
            {
                
                if (!isUpdate)
                {
                    return referralClientInformationService.addReferralClientInformation(referralClientInformation)
                }
                else
                {
                    return referralClientInformationService.updateReferralClientInformation(referralClientInformation);
                }
            };

            $scope.postSave = function (response, action, isNext)
            {
                var data = response;
                if (data.ResultCode !== 0)
                {
                    alertService.error(data.ResultMessage);
                    return $scope.promiseNoOp();
                } else
                {
                    alertService.success('Client Information has been ' + action + ' successfully.');
                    if (isNext)
                    {
                        $scope.next();
                    }
                    else
                    {
                        $scope.init();
                        return true;
                    }
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors)
            {
                if (!mandatory && isNext && hasErrors){
                    $scope.next();
                }

                if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)){
                    var isDirty = formService.isDirty();

                    if (!isDirty && isNext) {
                        $scope.next();
                    }
                    else if (isDirty){
                        var deferred = $q.defer();

                        var Addresses = $filter('filter')($scope.Addresses, function (item)
                        {
                            return item.Line1 !== '' || item.Line2 !== '' || item.City !== '' || (item.StateProvince !== '' && item.StateProvince != 0) || (item.County !== '' && item.County != 0) || item.Zip !== '';
                        });

                        var Phones = $filter('filter')($scope.Phones, function (item)
                        {
                            return item.Number !== '';
                        });

                        if (Phones.length == 0)
                        {
                            alertService.error('Please select contact method');
                            return;
                        }

                        if (!$scope.AdditionalDetails.IsHousingProgram) {
                            $scope.AdditionalDetails.HousingDescription = '';
                        }

                        var referralClientInformation = {};
                        referralClientInformation.clientDemographicsModel = $scope.Demographics;
                        referralClientInformation.clientDemographicsModel.ContactTypeID = 5;
                        referralClientInformation.referralClientAdditionalDetails = $scope.AdditionalDetails;
                        referralClientInformation.Concern = $scope.Concern;
                        referralClientInformation.Addresses = Addresses;
                        referralClientInformation.Phones = Phones;
                        referralClientInformation.referralClientAdditionalDetails.ReferralHeaderID = $scope.ReferralHeaderID;
                        referralClientInformation.ReferralHeaderID = $scope.ReferralHeaderID;
                        referralClientInformation.ContactID = $stateParams.ContactID;
                        var isUpdate = $scope.Demographics.ContactID != undefined && $scope.Demographics.ContactID !== 0 && $scope.Demographics.ContactID != null;
                        $scope.saveClientInformation(isUpdate, referralClientInformation).then(function (response)
                        {
                            var action = isUpdate ? 'updated' : 'added';
                            if (!isUpdate)
                                $scope.Demographics.ContactID = response.ID;
                            $scope.postSave(response, action, isNext);
                            deferred.resolve(response);
                        },
                        function (errorStatus)
                        {
                            alertService.error('OOPS! Something went wrong');
                            deferred.reject();
                        },
                        function (notification)
                        {
                            alertService.warning(notification);
                        }
                        );
                    }
                }
            }

            $scope.next = function () {
                    var params = {
                        ReferralHeaderID: $scope.ReferralHeaderID,
                        ContactID: $stateParams.ContactID
                    }
                    $state.go("referrals.disposition", params);
            };

            $scope.init();

           
 }]);
