(function () {
    angular.module('xenatixApp')
    .controller('chartTileController', [
        '$scope', '$q', '$stateParams', '$injector', 'lookupService', '$filter', '$rootScope', 'navigationService', '_', 'roleSecurityService', 'httpLoaderInterceptor',
    function ($scope, $q, $stateParams, $injector, lookupService, $filter, $rootScope, navigationService, _, roleSecurityService, httpLoaderInterceptor) {
        httpLoaderInterceptor.ignore(true);

        $scope.tileInfo = [];
        $scope.listLimit = 5;
        $scope.loggedInUser = {};
        var MAX_CHAR_LENGTH = 78;
        $scope.getText = function (lookUpType, value) {
            return lookupService.getText(lookUpType, value);
        };

        $scope.get = function (contactID) {
            var tilePromises = [];
            var hasClinicalService = false;
            var hasEciService = false;
            var tileModel = [];
            var tilePromise = [];
            tilePromise.push(getServiceTile(contactID));
            tilePromise.push(getAssessmentTile(contactID));
            $q.all(tilePromise).then(function (data) {
                data = removeNullFromArray(data);
                tileModel.push({ SectionName: "Chart", TileInfo: data })
                $scope.Sections = tileModel;
            });
        };

        var getAssessmentTile = function (contactID) {
            var permissionKey = ChartPermissionKey.Chart_Chart_Assessment;
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                if ($injector.has('assessmentsGridService')) {
                    var tileInfoModel = initTiles("Assessments", "assessmentsGrid", contactID, false, null, permissionKey, PERMISSION.READ);
                    var assessmentsGridService = $injector.get('assessmentsGridService');
                    assessmentsGridService.getAssessmentListByContactID(contactID).then(function (data) {
                        var result = [];
                        if (hasData(data.data)) {
                            var maxLnth = data.data.DataItems.length < $scope.listLimit ? data.data.DataItems.length : $scope.listLimit;
                            for (var ctr = 0; ctr < maxLnth; ctr++) {
                                var dataObj = data.data.DataItems[ctr];
                                var displayStr = '';
                                if (checkModel(dataObj.AssessmentName)) {
                                    displayStr = dataObj.AssessmentName + ', ' + (checkModel(dataObj.Source) ? dataObj.Source + ', ' : '');
                                }
                                if (checkModel(dataObj.ServiceStartDate)) {
                                    displayStr = displayStr + $filter('toMMDDYYYYDate')(dataObj.ServiceStartDate, 'MM/DD/YYYY', 'useLocal') + ', '
                                }
                                if (checkModel(dataObj.CallStatus)) {
                                    displayStr = displayStr.replace(/,\s*$/, "") + ' (' + dataObj.CallStatus + ')'
                                }
                                displayStr = displayStr.replace(/,\s*$/, "");
                                displayStr = _.trunc(displayStr, {
                                    'length': MAX_CHAR_LENGTH, 'separator': ','
                                });
                                result.push(getTileDetailsModel(null, displayStr, ""));
                            }
                        }
                        tileInfoModel.TileDetails = result;
                        if (tileInfoModel.TileDetails.length == 0)
                            tileInfoModel.TileDetails = [{ CustomMessage: "Assessments have not been provided." }];
                    },
                  function (errorStatus) {
                      tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                  }).finally(function () {
                      tileInfoModel.ShowShortcuts = false;
                      tileInfoModel.HideEdit = true;
                      tileInfoModel.IsLoaded = true;
                  });
                }
            }
            return tileInfoModel;
        }
        var getServiceTile = function (contactID) {
            var tileInfoModel = null;
            var permissionKey = ChartPermissionKey.Chart_Chart_RecordedServices;
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                if ($injector.has('serviceRecordingService')) {
                    var recordingService = $injector.get('serviceRecordingService');
                    tileInfoModel = initTiles("Recorded Services", "recordedservices", contactID, false, null, permissionKey, PERMISSION.READ);
                    recordingService.getServiceRecordings(contactID).then(function (data) {
                        var serviceDetails = [];
                        if (hasDetails(data)) {
                            var maxLnth = data.length < $scope.listLimit ? data.length : $scope.listLimit;
                            for (var ctr = 0; ctr < maxLnth; ctr++) {
                                var dataObj = data[ctr];
                                var displayStr = '';
                                if (checkModel(dataObj.ServiceItemID)) {
                                    displayStr = recordingService.getText('RecordingServices', dataObj.ServiceItemID, 'ServiceName', 'ServiceID') + ', '
                                }
                                if (checkModel(dataObj.ServiceRecordingSourceID)) {
                                    displayStr = displayStr + recordingService.getText('ServiceRecordingSource', dataObj.ServiceRecordingSourceID, "DisplayText") + ', '
                                }
                                if (checkModel(dataObj.ServiceStartDate)) {
                                    displayStr = displayStr + $filter('toMMDDYYYYDate')(dataObj.ServiceStartDate, 'MM/DD/YYYY', 'useLocal') + ', '
                                }

                                displayStr = displayStr.replace(/,\s*$/, "") + ' (' + recordingService.getStatus(dataObj) + ')'


                                displayStr = _.trunc(displayStr, { 'length': MAX_CHAR_LENGTH, 'separator': ',' });
                                serviceDetails.push(getTileDetailsModel(null, displayStr, ""));
                            }
                        }
                        tileInfoModel.TileDetails = serviceDetails;
                        if (tileInfoModel.TileDetails.length == 0)
                            tileInfoModel.TileDetails = [{ CustomMessage: "Recorded Services have not been provided." }];
                    },
                    function (errorStatus) {
                        tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                    }).finally(function () {
                        tileInfoModel.ShowShortcuts = false;
                        tileInfoModel.HideEdit = true;
                        tileInfoModel.IsLoaded = true;
                    });
                }
            }
            return tileInfoModel;
        }
        $scope.get($stateParams.ContactID);
    }]);
}());
