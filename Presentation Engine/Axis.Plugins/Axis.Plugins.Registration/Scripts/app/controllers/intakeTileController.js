(function () {
    angular.module('xenatixApp')
        .controller('intakeTileController', ['$scope', '$q', '$stateParams', '$injector', '$filter', 'roleSecurityService', 'lettersService', 'lookupService', 'intakeFormsService', 'registrationService', 'assessmentService', 'httpLoaderInterceptor',
            function ($scope, $q, $stateParams, $injector, $filter, roleSecurityService, lettersService, lookupService, intakeFormsService, registrationService, assessmentService, httpLoaderInterceptor) {
                httpLoaderInterceptor.ignore(true);

                var getIntakeLettersTileData = function (contactID) {
                    var dfd = $q.defer();
                    var tileInfoModel = {
                        tileDetailsModel: [],
                        tileEditModel: []
                    };
                    lettersService.get(contactID).then(function (data) {
                        if (hasData(data)) {
                            var tileData = $filter('orderBy')(data.DataItems, function (item) {
                                return new Date(item.ModifiedOn);
                            }, true);
                            angular.forEach(tileData, function (item, key) {
                                if (key < 5) {
                                    tileInfoModel.tileDetailsModel.push(getTileDetailsModel(null, lookupService.getText('Assessment', item.AssessmentID) + " (" + (item.SentDate ? 'Sent on ' + $filter('toMMDDYYYYDate')(item.SentDate, 'MM/DD/YYYY', 'useLocal') : 'Not Sent') + ")", (key == 0) ? "Recent" : ''));
                                }
                                assessmentService.getAssessmentSections(item.AssessmentID).then(function (data) {
                                    if (hasData(data)) {
                                        var sectionID = data.DataItems[0].AssessmentSectionID;
                                        var params = {
                                            ContactID: item.ContactID,
                                            AssessmentID: item.AssessmentID,
                                            ResponseID: item.ResponseID,
                                            ContactLettersID: item.ContactLettersID,
                                            SectionID: sectionID,
                                            ReadOnly: 'edit'
                                        };
                                        tileInfoModel.tileEditModel.push({
                                            id: item.ContactLettersID,
                                            modifiedDate: $filter('toMMDDYYYYDate')(item.ModifiedOn, 'MM/DD/YYYY', 'useLocal'),
                                            state: "patientprofile.intake.navi.letters.letternavi.lettersSection",
                                            contactID: contactID,
                                            item: lookupService.getText('Assessment', item.AssessmentID),
                                            params: params
                                        });
                                    }
                                });
                            });
                        }
                        else {
                            tileInfoModel.tileDetailsModel.push({ CustomMessage: "IDD Intake letters have not been provided" });
                        }
                        dfd.resolve(tileInfoModel);
                    }, function (errorStatus) {
                        tileInfoModel.tileDetailsModel = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                        dfd.resolve(tileInfoModel);
                    });
                    return dfd.promise;
                };
                var getIntakeFormsTileData = function (contactID) {
                    var dfd = $q.defer();

                    var tileInfoModel = {
                        tileDetailsModel: [],
                        tileEditModel: []
                    };
                    intakeFormsService.get(contactID).then(function (data) {
                        if (hasData(data)) {
                            var tileData = $filter('orderBy')(data.DataItems, ['DocumentStatusID', '-ServiceStartDate']);
                            angular.forEach(tileData, function (item, index) {
                                if (index < 5) {
                                    var status = (item.DocumentStatusID == DOCUMENT_STATUS.Draft) ? "Draft" : (item.IsVoided) ? "Void" : "Completed";
                                    tileInfoModel.tileDetailsModel.push(getTileDetailsModel(null,(item.ServiceStartDate ? $filter('formatDate')(item.ServiceStartDate, 'MM/DD/YYYY'):'') + " (" + status + ")", (index == 0) ? "Recent" : ""));
                                }
                                assessmentService.getAssessmentSections(item.AssessmentID).then(function (data) {
                                    if (hasData(data)) {
                                        var sectionID = data.DataItems[0].AssessmentSectionID;
                                        var params = {
                                            ContactID: item.ContactID,
                                            AssessmentID: item.AssessmentID,
                                            ContactFormsID: item.ContactFormsID,
                                            ResponseID: item.ResponseID ? item.ResponseID : 0,
                                            DocumentStatusID: item.DocumentStatusID,
                                            SectionID: sectionID,
                                            ReadOnly: (item.DocumentStatusID == DOCUMENT_STATUS.Draft) ? "edit" : "view"
                                        };
                                        tileInfoModel.tileEditModel.push({
                                            id: item.ContactFormsID,
                                            modifiedDate: $filter('formatDate')(item.ModifiedOn, 'MM/DD/YYYY'),
                                            state: "formservice",
                                            contactID: contactID,
                                            item: lookupService.getText('Assessment', item.AssessmentID),
                                            params: params
                                        });
                                    }
                                });
                            });
                        }
                        else {
                            tileInfoModel.tileDetailsModel.push({ CustomMessage: "IDD Intake forms have not been provided" });
                        }
                        dfd.resolve(tileInfoModel);
                    }, function (errorStatus) {
                        tileInfoModel.tileDetailsModel = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                        dfd.resolve(tileDetailsModel);
                    });
                    return dfd.promise;
                };
                var loadIntakeTile = function () {
                    if (roleSecurityService.hasPermission(this.permissionKey, PERMISSION.READ)) {
                        var tileInfoModel = initTiles(this.tileName, this.tileUrl, $stateParams.ContactID, true, null, this.permissionKey, this.permissionType, this.stateParams, this.isParentState, this.tileDefaultUrl);
                        tileInfoModel.IsLoaded = true;
                        tileInfoModel.TileDetails = this.tileDetails;
                        tileInfoModel.EditDetails = this.tileEditModel;
                        return tileInfoModel;
                    }
                };
                var get = function (contactID) {
                    registrationService.get(contactID).then(function (registrationData) {
                        if (hasData(registrationData)) {
                            if (registrationData.DataItems[0].ClientTypeID != PROGRAM_TYPE.ECI) {
                                var tileModel = [];
                                $scope.listLimit = 5;
                                var tilePromises = [];
                                tilePromises.push(getIntakeFormsTileData($stateParams.ContactID));
                                tilePromises.push(getIntakeLettersTileData($stateParams.ContactID));
                                $q.all(tilePromises).then(function (tileInfoDetailsModel) {
                                    var tileInfoModels = [];
                                    assessmentService.getAssessmentSections(ASSESSMENT_TYPE.IDDIntakeForms).then(function (data) {
                                        var sectionId = 0;
                                        if(hasData(data))
                                            sectionId = data.DataItems[0].AssessmentSectionID;
                                        tileInfoModels.push(loadIntakeTile.bind({ permissionKey: LettersPermissionKey.Intake_IDDForms_Forms, tileName: "Forms", permissionType: PERMISSION.CREATE, tileDefaultUrl: "formnavi.forms", tileUrl: "initializeformservice", isParentState: true, stateParams: { DocumentStatusID: 0, AssessmentID: ASSESSMENT_TYPE.IDDIntakeForms, SectionID: sectionId, ReadOnly: 'edit', ResponseID: 0 }, tileDetails: tileInfoDetailsModel[0].tileDetailsModel, tileEditModel: tileInfoDetailsModel[0].tileEditModel })());
                                        tileInfoModels.push(loadIntakeTile.bind({ permissionKey: LettersPermissionKey.Intake_IDD_Letters, tileName: "Letters", permissionType: PERMISSION.CREATE, tileDefaultUrl: null, tileUrl: "navi.letters", isParentState: null, stateParams: null, tileDetails: tileInfoDetailsModel[1].tileDetailsModel, tileEditModel: tileInfoDetailsModel[1].tileEditModel })());
                                        tileModel.push({ SectionName: "IDD", TileInfo: tileInfoModels.slice(0) });
                                        tileModel[0].TileInfo = removeNullFromArray(tileModel[0].TileInfo);
                                        $scope.Sections = tileModel;
                                    });
                                });
                            }
                        }
                    });
                };

                get($stateParams.ContactID);
            }]);
}());