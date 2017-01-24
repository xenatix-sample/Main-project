(function () {
    angular.module('xenatixApp')
    .factory('eciTileService', ["$q", '$filter', 'roleSecurityService', 'screeningService', 'ifspService', 'assessmentService', 'eligibilityDeterminationService', 'registrationService', 'progressNoteService', 'lookupService',
    function ($q, $filter, roleSecurityService, screeningService, ifspService, assessmentService, eligibilityDeterminationService, registrationService, progressNoteService, lookupService) {

       

        function get(contactID, listLimit) {
            var dfd = $q.defer();
            var tilePromises = [];
            registrationService.get(contactID).then(function (demographic) {
                if (demographic !== undefined && demographic !== null && demographic.DataItems.length > 0) {
                    // this should just exist as a place holder until we have a rules engine in place and link programs to tiles
                    if (demographic.DataItems[0].ClientTypeID === 1) {
                        tilePromises.push(getScreeningTiles(contactID, listLimit));
                        tilePromises.push(getEligibilityTiles(contactID, listLimit));
                        tilePromises.push(getIFSPTiles(contactID, listLimit));
                        tilePromises.push(getECINotesTiles(contactID));

                        $q.all(tilePromises).then(function(tileData) {
                            var tileInfo = $filter('filter')(tileData.slice(0, tilePromises.length), function(item) { return item != null });
                            dfd.resolve(tileInfo);
                        });
                    } else {
                        dfd.resolve(null);
                    }
                } else {
                    //the client will be an eci client under the assumption that the reg svc does not return data and the eci tile service is available, place the tilepromises in a shared method
                    tilePromises.push(getScreeningTiles(contactID, listLimit));
                    tilePromises.push(getEligibilityTiles(contactID, listLimit));
                    tilePromises.push(getIFSPTiles(contactID, listLimit));
                    tilePromises.push(getECINotesTiles(contactID));

                    $q.all(tilePromises).then(function(tileData) {
                        var tileInfo = $filter('filter')(tileData.slice(0, tilePromises.length), function(item) { return item != null });
                        dfd.resolve(tileInfo);
                    });
                }
            });
            return dfd.promise;
        };

        getScreeningTiles = function (contactID, listLimit) {
            var permissionKey = 'ECI-Screening-Screening';
            var tileInfoModel = null;
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                var url = "screenings";
                var tileAddDetails = [];
                var lookups = $filter('limitTo')(screeningService.getLookups('ScreeningName'), listLimit);

                angular.forEach(lookups, function (obj, indx) {
                    addDetail = { Label: '', Value: obj.Name, State: "patientprofile.chart.screenings.screening.header", Params: { ContactID: contactID, ScreenAssessmentID: obj.ID, ScreeningTypeID: obj.ScreeningTypeID } };
                    tileAddDetails.push(addDetail);
                });

                tileInfoModel = initTiles("Screening", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);

                screeningService.getList(contactID).then(function (screening) {
                    var result = [];
                    if ((screening.DataItems != null) && (screening.DataItems.length > 0)) {
                        var screenDetail = screening.DataItems[0];

                        if (checkModel(screenDetail.ScreeningDate)) {
                            var formattedDate = $filter('toMMDDYYYYDate')(screenDetail.ScreeningDate, 'MM/DD/YYYY');
                            result.push(getTileDetailsModel(null, formattedDate, "Screening Date"));
                        }
                        if (checkModel(screenDetail.ScreeningType)) {
                            result.push(getTileDetailsModel(null, screenDetail.ScreeningType, "Screening Type"));
                        }
                        if (checkModel(screenDetail.ScreeningName)) {
                            result.push(getTileDetailsModel(null, screenDetail.ScreeningName, "Screening Name"));
                        }
                        if (checkModel(screenDetail.ScreeningScore)) {
                            result.push(getTileDetailsModel(null, screenDetail.ScreeningScore, "Screening Score"));
                        }
                        if (checkModel(screenDetail.ScreeningResult)) {
                            result.push(getTileDetailsModel(null, screenDetail.ScreeningResult, "Screening Result"));
                        }

                    }

                    angular.forEach(screening.DataItems, function (item) {
                        assessmentService.getAssessmentSections(item.AssessmentID).then(function (data) {
                            if (data.ResultCode === 0) {
                                var params = {
                                    ContactID: contactID,
                                    ScreeningID: item.ScreeningID,
                                    SectionID: data.DataItems[0].AssessmentSectionID,
                                    ResponseID: item.ResponseID
                                };
                            }
                            tileInfoModel.EditDetails.push({
                                modifiedDate: $filter('toMMDDYYYYDate')(item.ScreeningDate, 'MM/DD/YYYY'), item: (item.AssessmentName), state: 'patientprofile.chart.screenings.screening.section', contactID: null, id: item.ScreeningID, params: params
                            });
                        });
                    });
                    tileInfoModel.TileDetails = result;
                    tileInfoModel.IsLoaded = true;
                },
             function (errorStatus) {
                 tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                 tileInfoModel.IsLoaded = true;
             });
            }
            return tileInfoModel;
        }
        getEligibilityTiles = function (contactID, listLimit) {
            var tileInfoModel = null;
            var permissionKey = 'ECI-Eligibility-Eligibility'
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                var lookups = $filter('limitTo')(screeningService.getLookups('EligibilityCategory'), listLimit);
                var tileAddDetails = [];
                var url = "eligibilities";
                angular.forEach(lookups, function (obj, indx) {
                    addDetail = { Label: '', Value: obj.Name, State: "patientprofile.chart.eligibilities", Params: { ContactID: contactID, EligibilityCategoryID: obj.ID } };
                    tileAddDetails.push(addDetail);
                });
                tileInfoModel = initTiles("Eligibility Statement", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);

                eligibilityDeterminationService.get(contactID).then(function (eligibility) {
                    var result = [];
                    if ((eligibility.DataItems != null) && (eligibility.DataItems.length > 0)) {
                        var model = eligibility.DataItems[0];

                        var formattedDate = $filter('toMMDDYYYYDate')(model.EligibilityDate, 'MM/DD/YYYY');
                        result.push(getTileDetailsModel(null, formattedDate, "Eligibility Date"));

                        if (checkModel(model.EligibilityTypeID)) {
                            result.push(getTileDetailsModel('EligibilityType', model.EligibilityTypeID, "Eligibility Type"));
                        }
                        if (checkModel(model.EligibilityCategoryID)) {
                            result.push(getTileDetailsModel('EligibilityCategory', model.EligibilityCategoryID, "Eligibility Category"));
                        }
                    }
                    angular.forEach(eligibility.DataItems, function (item) {
                        var params = {}
                        params.EligibilityID = item.EligibilityID;
                        tileInfoModel.EditDetails.push({
                            modifiedDate: $filter('toMMDDYYYYDate')(item.EligibilityDate, 'MM/DD/YYYY'), LookUpType: 'EligibilityType', item: (item.EligibilityTypeID), state: "patientprofile.chart.eligibilities.eligibility.calculation", contactID: null, id: item.EligibilityID, params: params
                        });
                    });
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
        getIFSPTiles = function (contactID, listLimit) {
            var tileInfoModel = null;
            var permissionKey = 'ECI-IFSP-IFSP';
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                var url = "ifsps";
                var lookups = $filter('limitTo')(screeningService.getLookups('IFSPType'), listLimit);
                var tileAddDetails = [];

                angular.forEach(lookups, function (obj, indx) {
                    addDetail = { Label: '', Value: obj.Name, State: "patientprofile.chart.ifsps", Params: { ContactID: contactID, IFSPTypeID: obj.ID } };
                    tileAddDetails.push(addDetail);
                });

                tileInfoModel = initTiles("IFSP", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);

                ifspService.getList(contactID).then(function (ifsp) {
                    var result = [];
                    if ((ifsp.DataItems != null) && (ifsp.DataItems.length > 0)) {
                        var model = ifsp.DataItems[0];

                        if (checkModel(model.IFSPMeetingDate)) {
                            var formattedDate = $filter('toMMDDYYYYDate')(model.IFSPMeetingDate, 'MM/DD/YYYY');
                            result.push(getTileDetailsModel(null, formattedDate, "Meeting Date"));
                        }
                        if (checkModel(model.IFSPType)) {
                            result.push(getTileDetailsModel(null, model.IFSPType, "IFSP Type"));
                        }
                    }
                    angular.forEach(ifsp.DataItems, function (item) {
                        assessmentService.getAssessmentSections(item.AssessmentID).then(function (data) {
                            if (data.ResultCode === 0) {
                                var params = {
                                    ContactID: contactID,
                                    IFSPID: item.IFSPID,
                                    SectionID: data.DataItems[0].AssessmentSectionID,
                                    ResponseID: item.ResponseID
                                };
                            }
                            tileInfoModel.EditDetails.push({
                                modifiedDate: $filter('toMMDDYYYYDate')(item.IFSPMeetingDate, 'MM/DD/YYYY'), item: (item.IFSPType), state: "patientprofile.chart.ifsps.ifsp.section", contactID: null, id: item.IFSPID, params: params
                            });
                        });
                    });
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
        getECINotesTiles = function (contactID) {
            var permissionKey = 'ECI-ProgressNote-ProgressNote';
            var tileInfoModel = null;
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                var url = "initialcontact";
                var tileAddDetails = [];

                var lookups = lookupService.getLookupsByType('ProgressNoteType');
                angular.forEach(lookups, function (obj, indx) {
                    addDetail = { Label: '', Value: obj.Name, State: "patientprofile.chart." + getStateName(obj.Name), Params: { ContactID: contactID, NoteTypeID: obj.ID } }; //add the new state param
                    tileAddDetails.push(addDetail);
                });

                ////Add Initial Contact Attempt Note //
                //var addDetail = { Label: '', Value: 'Initial Contact Attempt Note', State: "initialnote.contactattemptnote", Params: { ContactID: contactID } };
                //tileAddDetails.push(addDetail);
                ////Add Initial Contact Note //
                //addDetail = { Label: '', Value: 'Initial Contact Note', State: "initialnote.contactnote", Params: { ContactID: contactID } };
                //tileAddDetails.push(addDetail);

                tileInfoModel = initTiles("Progress Notes", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE, { NoteTypeID: 2 });
                tileInfoModel.TileDetails = [];
                var dfd = $q.defer();
                progressNoteService.getList(0, contactID).then(function (notesData) {
                    var result = [];
                    if ((notesData.DataItems != null) && (notesData.DataItems.length > 0)) {
                        var model = notesData.DataItems[0];

                        if (checkModel(model.TakenBy)) {
                            result.push(getTileDetailsModel("Users", model.TakenBy, "Taken By"));
                        }
                        if (checkModel(model.TakenTime)) {
                            var formattedDate = $filter('toMMDDYYYYDate')(model.TakenTime, 'MM/DD/YYYY', 'useLocal');
                            result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                        }
                        if (checkModel(model.NoteTypeID)) {
                            result.push(getTileDetailsModel('ProgressNoteType', model.NoteTypeID, 'Note Type'));
                        }
                        url = getStateName(lookupService.getText('ProgressNoteType', model.NoteTypeID));
                        tileInfoModel.Url = buildURL(contactID, url, { NoteTypeID: model.NoteTypeID });
                        tileInfoModel.TileDetails = result;

                    }
                    angular.forEach(notesData.DataItems, function (item) {
                        tileInfoModel.EditDetails.push({
                            modifiedDate: $filter('toMMDDYYYYDate')(item.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                            item: lookupService.getText("Users", item.TakenBy),
                            state: 'patientprofile.chart.' + getStateName(lookupService.getText('ProgressNoteType', item.NoteTypeID)),
                            contactID: contactID,
                            id: item.NoteHeaderID,
                            params: { ContactID: contactID, NoteTypeID: item.NoteTypeID,NoteHeaderID:item.NoteHeaderID }
                        });
                    });
                    tileInfoModel.IsLoaded = true;
                    dfd.resolve(tileInfoModel);
                },
                function (errorStatus) {
                    tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                    tileInfoModel.IsLoaded = true;
                    dfd.resolve(tileInfoModel);
                });
            }
            return dfd.promise;
        }
        getStateName = function (strName) {
            return angular.lowercase(strName.replaceAll(' ', ''))
        };

        return {
            get: get
        };
    }]);


}());