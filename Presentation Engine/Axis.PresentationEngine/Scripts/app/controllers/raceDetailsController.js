(function () {
    angular.module('xenatixApp')
        .controller('raceDetailsController', ['$scope', '$filter', '$rootScope', 'contactRaceService', '$q', 'alertService', function ($scope, $filter, $rootScope, contactRaceService, $q, alertService) {
            var declineRaceOption = RACE.DeclineToSpecify;
            $scope.noRaceDetailResults = false;
            $scope.raceDetailsChanged = false;
            $scope.raceDetail = '';
            $scope.raceLookUpData = $rootScope.getLookupsByType('Race');
            var initRace = function () {
                $scope.selectedRaceDetails = [];
            }
            initRace();

            var setRaceDetailsChanged = function () {
                $scope.raceDetailsChanged = true;
                $scope.selectedRaceName = $filter('filter')($scope.selectedRaceDetails, { IsActive: true }, true).map(function (race) { return race.Race; }).join(',');
            };

            $scope.selectRaceDetail = function (item) {
                var isDeclinedExists = $filter('filter')($scope.selectedRaceDetails, { RaceID: declineRaceOption, IsActive: true }, true);
                if (isDeclinedExists && isDeclinedExists.length > 0 && item.ID != declineRaceOption) {
                    alertService.error('Invalid selection due to selection of Decline to specify option');
                }
                else if (isDeclinedExists && isDeclinedExists.length == 0 && item.ID != declineRaceOption) {
                    var idx = -1;
                    for (var i = 0; i < $scope.selectedRaceDetails.length; i++) {
                        if ($scope.selectedRaceDetails[i].RaceID === item.ID) {
                            idx = i;
                            break;
                        }
                    }
                    if (idx === -1) {
                        $scope.selectedRaceDetails.push({ ContactRaceID: 0, Race: item.Name, RaceID: item.ID, ContactID: $scope.contactID, IsActive: true });
                    }
                    else if ($scope.selectedRaceDetails[idx].IsActive == undefined || $scope.selectedRaceDetails[idx].IsActive == null || $scope.selectedRaceDetails[idx].IsActive == false) {
                        $scope.selectedRaceDetails[idx].IsActive = true;
                    }
                    setRaceDetailsChanged();
                }
                else if (isDeclinedExists && item.ID == declineRaceOption) {
                    var activeRaceExists = $filter('filter')($scope.selectedRaceDetails, { IsActive: true }, true);
                    if (activeRaceExists && activeRaceExists.length > 0) {
                        bootbox.confirm("You will lose other race entered.\n Do you want to continue?", function (result) {
                            if (result == true) {
                                for (var i = 0; i < $scope.selectedRaceDetails.length; i++) {
                                    $scope.selectedRaceDetails[i].IsActive = false;
                                }
                                $scope.selectedRaceDetails.push({ ContactRaceID: 0, Race: item.Name, RaceID: item.ID, ContactID: $scope.contactID, IsActive: true });
                                setRaceDetailsChanged();
                            }
                        });
                    }
                    else {
                        for (var i = 0; i < $scope.selectedRaceDetails.length; i++) {
                            $scope.selectedRaceDetails[i].IsActive = false;
                        }
                        $scope.selectedRaceDetails.push({ ContactRaceID: 0, Race: item.Name, RaceID: item.ID, ContactID: $scope.contactID, IsActive: true });
                        setRaceDetailsChanged();
                    }
                }
                $scope.raceDetail = '';
                setRaceErrorClass();
            };

            $scope.removeRaceDetail = function (raceDetail) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedRaceDetails.length; i++) {
                    if ($scope.selectedRaceDetails[i].RaceID === raceDetail.RaceID) {
                        idx = i;
                        break;
                    }
                }
                if (idx > -1) {
                    if ($scope.selectedRaceDetails[idx].ContactRaceID != 0) {
                        $scope.selectedRaceDetails[idx].IsActive = false;
                    }
                    else
                        $scope.selectedRaceDetails.splice(idx, 1);
                    setRaceDetailsChanged();
                }
                setRaceErrorClass();
            };
            
            setRaceErrorClass = function () {
                var raceErrorBlock = $('#raceErrorBlock');
                if (raceErrorBlock != undefined && raceErrorBlock != null) {
                    if (raceErrorBlock.hasClass('has-error'))
                        raceErrorBlock.removeClass('has-error');
                    if ($scope.selectedRaceDetails.length === 0)
                        raceErrorBlock.addClass('has-error');
                    else {
                        var activeRace = $filter('filter')($scope.selectedRaceDetails, function (data) {
                            return data.IsActive;
                        });
                        if (activeRace.length === 0)
                            raceErrorBlock.addClass('has-error');
                    }
                }
            };
            $scope.promises = [];
            $scope.getRace = function (contactID) {
                initRace();
                var raceDeferred = $q.defer();
                $scope.promises.push(raceDeferred.promise);
                contactRaceService.get(contactID).then(function (data) {
                    if (hasData(data)) {
                        $scope.selectedRaceDetails = data.DataItems;

                        for (var i = 0; i < $scope.selectedRaceDetails.length; i++) {
                            for (var j = 0; j < $scope.raceLookUpData.length; j++) {
                                if ($scope.selectedRaceDetails[i].RaceID == $scope.raceLookUpData[j].ID) {
                                    $scope.selectedRaceDetails[i].Race = $scope.raceLookUpData[j].Name;
                                    break;
                                }
                            }
                        }
                    }
                   raceDeferred.resolve(data);
                },
                function (errorStatus) {
                    alertService.error('Unable to get race: ' + errorStatus);
                });
            };

            $scope.saveRace = function (contactID) {
                var races = $filter('filter')($scope.selectedRaceDetails, function (item) {
                    if (item.RaceID !== 0 && (item.IsActive || item.ContactRaceID != 0)) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
                var deferred = $q.defer();
                $scope.taskArray = [];
                if (($scope.raceDetailsChanged) && races != undefined && races != null && races.length > 0) {
                    
                    angular.forEach(races, function (race, i) {
                        race.ContactID = contactID;
                        if (race.ContactRaceID == 0) {
                            $scope.taskArray.push([contactRaceService.addContactRace, [race]]);
                        }
                        else if (race.IsActive == false && race.ContactRaceID && race.ContactRaceID != 0) {
                            $scope.taskArray.push([contactRaceService.remove, [race.ContactRaceID, race.ContactID]]);
                        }
                    });
                }
                deferred.resolve(races);
                return deferred.promise;
            };

        }]);
})();