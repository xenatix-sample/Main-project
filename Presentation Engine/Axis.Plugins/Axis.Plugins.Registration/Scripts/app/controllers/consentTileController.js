(function () {
    angular.module('xenatixApp')
        .controller('consentTileController', [
            '$scope', 'alertService', '$stateParams', 'consentService', '$filter', '$q', 'consentsService', 'assessmentService', 'roleSecurityService', 'httpLoaderInterceptor',
            function ($scope, alertService, $stateParams, consentService, $filter, $q, consentsService, assessmentService, roleSecurityService, httpLoaderInterceptor) {
                httpLoaderInterceptor.ignore(true);

                var returnModel = [];
                var tileInfo = [];
                var permissionKey = ConsentsPermissionKey.Consents_Assessment_Agency;

                $scope.listLimit = 5;

                var getHippa = function (contactID) {
                    var tileInfoModel = initTiles("HIPAA", "consentView", contactID, true);
                    consentService.getConsentSignature(contactID).then(function (consent) {
                        var tileDetailsModel = {};
                        if ((consent.DataItems != null) && (consent.DataItems.length > 0) && (consent.DataItems[0].SignatureId != undefined && consent.DataItems[0].SignatureId != null)) {
                            var formattedDate = $filter('toMMDDYYYYDate')(consent.DataItems[0].ModifiedOn, 'MM/DD/YYYY', 'useLocal');
                            tileDetailsModel.CustomMessage = "The consent was signed on " + formattedDate;
                            tileInfoModel.ShowShortcuts = false;
                        } else {
                            tileDetailsModel.CustomMessage = "The HIPAA Consent has not been signed.";
                        }

                        tileInfoModel.TileDetails = new Array(tileDetailsModel);
                        tileInfoModel.IsLoaded = true;
                        tileInfo.push(tileInfoModel);
                    },
                      function (errorStatus) {
                          tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                          tileInfoModel.IsLoaded = true;
                      });

                    return tileInfoModel;
                }

                var getConsent = function (contactID) {
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var tileInfoModel = initTiles("Agency", "agency.agencyView", contactID, true, null, permissionKey, PERMISSION.CREATE);
                        consentsService.get(contactID).then(function (consent) {
                            var tileDetailsModel = [], tileEditModel = [];
                            if (hasData(consent)) {
                                var consentsList = [];
                                assessmentService.getAssessment().then(function (data) {
                                    if (hasData(data)) {
                                        consentsList = $filter('filter')(data.DataItems, { DocumentTypeID: 3 }, true);

                                        tileDetailsModel.push({ CustomMessage: null, Value: "Recent" });

                                    angular.forEach(consent.DataItems, function (item, index) {
                                        if (index < 5) {
                                            var date = item.EffectiveDate ? "  ( " + $filter('toMMDDYYYYDate')(item.EffectiveDate, 'MM/DD/YYYY', 'useLocal') + " )" : "";
                                            var assessmentFilter = $filter('filter')(consentsList, { AssessmentID: item.AssessmentID }, true);
                                            var consentName = assessmentFilter[0].AssessmentName;
                                            var consentText = consentName + ' ' + date;
                                            tileDetailsModel.push({ CustomMessage: null, Label: consentText });
                                        }
                                        var params = {
                                            AssessmentID: item.AssessmentID,
                                            SectionID: item.AssessmentSectionID,
                                            ResponseID: item.ResponseID,
                                            ContactConsentID: item.ContactConsentID
                                        };

                                            tileEditModel.push({
                                                id: item.ContactConsentID,
                                                modifiedDate: date,
                                                state: "agencyViewSection",
                                                contactID: contactID,
                                                item: item.ConsentName,
                                                params: params
                                            });
                                        });
                                    }
                                });
                            } else {
                                tileDetailsModel.push({ CustomMessage: "Consents have not been provided" });
                            }

                            tileInfoModel.TileDetails = tileDetailsModel;
                            tileInfoModel.EditDetails = tileEditModel;
                            tileInfoModel.ShowShortcuts = true;
                            tileInfoModel.IsLoaded = true;
                        },
                          function (errorStatus) {
                              tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                              tileInfoModel.IsLoaded = true;
                          });
                    }
                    return tileInfoModel;
                }

                $scope.get = function (contactID) {
                    var queue = [];
                    var tileModel = [];
                    queue.push(getConsent(contactID));
                    //queue.push(getHippa(contactID));
                    $q.all(queue).then(function (infoModels) {
                        tileModel.push({ SectionName: "Consent", TileInfo: infoModels });
                        tileModel[0].TileInfo = removeNullFromArray(tileModel[0].TileInfo);
                        $scope.Sections = tileModel;
                    });
                };

                $scope.get($stateParams.ContactID);
            }
        ]);
}());