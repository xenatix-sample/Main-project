angular.module("xenatixApp")
    .factory("lawLiaisonFollowUpService", ["$http", "$q", '$filter', "settings", "offlineData", 'callerInformationService', 'callCenterProgressNoteService', '$stateParams', 'navigationService', function ($http, $q, $filter, settings, offlineData, callerInformationService, callCenterProgressNoteService, $stateParams, navigationService) {
        function followUp(callCenterHeaderID) {
            var progressNoteModel = {
                CallCenterHeaderID: 0,
                FollowupPlan: '',
                Comments: '',
                NatureofCall: '',
                ModifiedOn: new Date(),
            }
            var dfd = $q.defer();
            callerInformationService.get(callCenterHeaderID).then(function (response) {
                var model = angular.copy(response.DataItems[0]);
                if (model) {
                    navigationService.get().then(function (data) {
                        if (hasData(data)) {
                            model.CallCenterHeaderID = null;
                            model.ParentCallCenterHeaderID = callCenterHeaderID;
                            model.CallStartTime = $filter('formatDate')(new Date(), 'MM/DD/YYYY hh:mm:ss A');
                            model.ProviderID = data.DataItems[0].UserID;
                            callerInformationService.add(model).then(function (callCenterData) {
                                callCenterProgressNoteService.get(callCenterHeaderID).then(function (progressNoteData) {
                                    if (hasData(progressNoteData) && progressNoteData.DataItems[0].ProgressNoteID) {
                                        var parentProgressNote = angular.copy(progressNoteData.DataItems[0]);
                                        progressNoteModel.CallCenterHeaderID = callCenterData.ID;
                                        progressNoteModel.FollowupPlan = parentProgressNote.FollowupPlan;
                                        progressNoteModel.Comments = parentProgressNote.Comments;
                                        progressNoteModel.NatureofCall = parentProgressNote.NatureofCall;
                                        progressNoteModel.ModifiedOn = new Date(parentProgressNote.ModifiedOn);
                                        progressNoteModel.CallStartTime = $filter('formatDate')(new Date(), 'MM/DD/YYYY hh:mm:ss A');
                                        callCenterProgressNoteService.add(progressNoteModel).then(function (data) {
                                            dfd.resolve(callCenterData.ID);
                                        });
                                    }
                                    else {
                                        dfd.resolve(null);
                                    }
                                });
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