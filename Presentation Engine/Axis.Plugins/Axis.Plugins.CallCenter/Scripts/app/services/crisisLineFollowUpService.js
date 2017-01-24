angular.module("xenatixApp")
    .factory("crisisLineFollowUpService", ["$q", '$filter', 'callerInformationService', 'navigationService', 'registrationService', 'contactPhoneService', function ($q, $filter, callerInformationService, navigationService, registrationService, contactPhoneService) {
        function followUp(callCenterHeaderID) {
            var dfd = $q.defer();
            callerInformationService.get(callCenterHeaderID).then(function (response) {
                var childmodel = response.DataItems[0];
                var parentModel = angular.copy(response.DataItems[0]);
                if (childmodel) {
                    navigationService.get().then(function (data) {
                        if (hasData(data)) {
                            childmodel.CallCenterHeaderID = null;
                            childmodel.ParentCallCenterHeaderID = callCenterHeaderID;
                            childmodel.CallStartTime = $filter('formatDate')(new Date(), 'MM/DD/YYYY hh:mm:ss A');
                            childmodel.CallEndTime = "";
                            childmodel.ProviderID = data.DataItems[0].UserID;
                            childmodel.FollowUpRequired = true;
                            childmodel.Disposition = '';
                            childmodel.CallStatusID = CALL_STATUS.PENDING;
                            childmodel.OtherInformation = '';
                            childmodel.Comments = '';
                            parentModel.FollowUpRequired = 0;
                            callerInformationService.update(parentModel);
                            callerInformationService.add(childmodel).then(function (callCenterData) {
                                dfd.resolve(callCenterData.ID);
                            });
                        }
                    });
                }
                else {
                    dfd.reject(null)
                }
            });
            return dfd.promise;
        };

        return {
            followUp: followUp,
        };
    }]);