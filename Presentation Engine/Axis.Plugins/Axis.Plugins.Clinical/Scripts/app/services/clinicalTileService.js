(function () {
    angular.module('xenatixApp')
        .factory('clinicalTileService', [
            "$q", "noteService", "reviewOfSystemsService", "clinicalAssessmentService", "chiefComplaintService", "allergyService", 'vitalService', "$filter", "lookupService", 'roleSecurityService', "socialRelationshipService", "presentIllnessService", "$injector", "medicalHistoryService",
            function ($q, noteService, reviewOfSystemsService, clinicalAssessmentService, chiefComplaintService, allergyService, vitalService, $filter, lookupService, roleSecurityService, socialRelationshipService, presentIllnessService, $injector, medicalHistoryService) {
                var restrictedTextLength = 40;
                function get(contactID) {
                    var dfd = $q.defer();

                    //if ($injector.has('registrationService')) {
                    //    var registrationService = $injector.get('registrationService');
                    //    registrationService.get(contactID).then(function (data) {
                    //        var isSocialRequired = (data.DataItems[0].ClientTypeID === 3 || data.DataItems[0].ClientTypeID === 4);
                    //        getTiles(contactID, isSocialRequired, dfd);
                    //    });
                    //}
                    //else {
                    //    getTiles(contactID, false, dfd);
                    //}
                    getTiles(contactID, false, dfd);

                    return dfd.promise;
                };

                getTiles = function (contactID, isSocialRequired, dfd) {
                    var tilePromises = [];
                    tilePromises.push(getChiefComplaintTiles(contactID));
                    tilePromises.push(getVitalsTiles(contactID));
                    tilePromises.push(getAllergiesTiles(contactID));

                    tilePromises.push(getReviewOfSystemTiles(contactID));
                    tilePromises.push(getMedicalHistoryTiles(contactID));
                    //if (isSocialRequired)
                    tilePromises.push(getSocialRelationship(contactID));
                    tilePromises.push(getPresentIllness(contactID));
                    tilePromises.push(getAssessmentTiles(contactID));

                    tilePromises.push(getNotesTiles(contactID));

                    $q.all(tilePromises).then(function (tileData) {
                        var tileInfo = $filter('filter')(tileData.slice(0, tilePromises.length), function (item) { return item != null });
                        dfd.resolve(tileInfo);
                    });
                }

                getNotesTiles = function (contactID) {
                    var permissionKey = 'Clinical-Note-Note';
                    var tileInfoModel = null;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var url = "intake.note";
                        var tileAddDetails = [];

                        var lookups = lookupService.getLookupsByType('NoteType');
                        angular.forEach(lookups, function (obj, indx) {
                            addDetail = { Label: '', Value: obj.Name, State: "patientprofile.chart.intake.note", Params: { ContactID: contactID, NTypeID: obj.ID } }; //add the new state param
                            tileAddDetails.push(addDetail);
                        });

                        tileInfoModel = initTiles("Notes", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                        tileInfoModel.TileDetails = [];
                        noteService.getNotes(contactID).then(function (notesData) {
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
                                    result.push(getTileDetailsModel('NoteType', model.NoteTypeID, 'Note Type'));
                                }

                                tileInfoModel.TileDetails = result;
                            }
                            angular.forEach(notesData.DataItems, function (item) {
                                tileInfoModel.EditDetails.push({
                                    modifiedDate: $filter('toMMDDYYYYDate')(item.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                                    item: lookupService.getText("Users", item.TakenBy),
                                    state: "patientprofile.chart.intake.note",
                                    contactID: contactID,
                                    id: item.NoteID
                                });
                            });
                            tileInfoModel.IsLoaded = true;
                        },
                        function (errorStatus) {
                            tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                            tileInfoModel.IsLoaded = true;
                        });
                    }
                    return tileInfoModel;
                }

                getAllergiesTiles = function (contactID) {
                    var url = "intake.allergy";
                    var permissionKey = 'Clinical-Allergy-Allergy';
                    var tileAddDetails = [];
                    var allergyTypeID = 1;
                    var tileInfoModel = null;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var lookups = lookupService.getLookupsByType('AllergyType');
                        angular.forEach(lookups, function (obj, indx) {
                            addDetail = { Label: '', Value: obj.Name, State: "patientprofile.chart.intake." + angular.lowercase(obj.Name.replaceAll(' ', '')), Params: { ContactID: contactID, AllergyTypeID: obj.ID } }; //add the new state param for allergytypeid
                            tileAddDetails.push(addDetail);
                        });
                        var defaultParams = { AllergyTypeID: 1 };
                        tileInfoModel = initTiles("Allergies", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE, defaultParams);
                        tileInfoModel.TileDetails = [];

                        //last bundle, but need to account for all allergy types
                        allergyService.getAllergyBundle(contactID, allergyTypeID).then(function (bundleData) {
                            var result = [];
                            var tmpTakenBy = 0;
                            var tmpTakenOn = '';
                            if (bundleData.ResultCode === 0) {
                                if (bundleData.DataItems.length > 0) {
                                    var bundleModel = $filter('orderBy')(bundleData.DataItems, 'TakenTime')[0];
                                    tmpTakenBy = bundleModel.TakenBy;
                                    tmpTakenOn = bundleModel.TakenTime;
                                    if (checkModel(bundleModel.TakenBy)) {
                                        result.push(getTileDetailsModel("Users", bundleModel.TakenBy, "Taken By"));
                                    }
                                    if (checkModel(bundleModel.TakenTime)) {
                                        var formattedDate = $filter('toMMDDYYYYDate')(bundleModel.TakenTime, 'MM/DD/YYYY', 'useLocal');
                                        result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                                    }
                                    if (checkModel(bundleModel.AllergyTypeID)) {
                                        result.push(getTileDetailsModel("AllergyType", bundleModel.AllergyTypeID, "Allergy Type"));
                                    }

                                    tileInfoModel.TileDetails = result;

                                    allergyService.getAllergyDetails(contactID, bundleModel.ContactAllergyID, allergyTypeID).then(function (data) {
                                        if (data.ResultCode === 0 && data.DataItems.length > 0) {
                                            angular.forEach(data.DataItems, function (item) {
                                                var params = {};
                                                params.AllergyTypeID = allergyTypeID;
                                                var allergyLookupName = (allergyTypeID === 1 ? "Allergy" : "Drug");
                                                tileInfoModel.EditDetails.push({
                                                    modifiedDate: $filter('toMMDDYYYYDate')(tmpTakenOn, 'MM/DD/YYYY', 'useLocal'),
                                                    item: lookupService.getText(allergyLookupName, item.AllergyID),
                                                    state: "patientprofile.chart.intake.allergy",
                                                    contactID: contactID,
                                                    id: item.ContactAllergyDetailID,
                                                    params: params
                                                });
                                            });
                                        }
                                        else {
                                            var params = {};
                                            params.AllergyTypeID = allergyTypeID;
                                            var allergyLookupName = (allergyTypeID === 1 ? "Allergy" : "Drug");
                                            tileInfoModel.EditDetails.push({
                                                modifiedDate: $filter('toMMDDYYYYDate')(tmpTakenOn, 'MM/DD/YYYY', 'useLocal'),
                                                item: lookupService.getText(allergyLookupName, "Allergy"),
                                                state: "patientprofile.chart.intake.allergy",
                                                contactID: contactID,
                                                id: 0,
                                                params: params
                                            });
                                        }
                                    });
                                }
                                tileInfoModel.IsLoaded = true;
                            } else {
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                                tileInfoModel.IsLoaded = true;
                            }
                        });
                    }
                    return tileInfoModel;
                }

                getAssessmentTiles = function (contactID) {
                    var permissionKey = 'Clinical-Assessment-Assessment';
                    var tileInfoModel = null;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var tileAddDetails = [];
                        var url = "intake.clinicalAssessment";
                        tileInfoModel = initTiles("Assessment", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                        tileInfoModel.TileDetails = [];

                        clinicalAssessmentService.getClinicalAssessmentByContact(contactID).then(function (clinicalAssessmentData) {
                            var result = [];
                            if ((clinicalAssessmentData.DataItems != null) && (clinicalAssessmentData.DataItems.length > 0)) {
                                var model = clinicalAssessmentData.DataItems[0];

                                if (checkModel(model.TakenBy)) {
                                    result.push(getTileDetailsModel("Users", model.TakenBy, "Taken By"));
                                }
                                if (checkModel(model.TakenTime)) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(model.TakenTime, 'MM/DD/YYYY', 'useLocal');
                                    result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                                }

                                if (checkModel(model.AssessmentID)) {
                                    result.push(getTileDetailsModel("ClinicalAssessment", model.AssessmentID, "Assessment"));
                                }

                                tileInfoModel.TileDetails = result;
                            }
                            angular.forEach(clinicalAssessmentData.DataItems, function (item) {
                                tileInfoModel.EditDetails.push({
                                    modifiedDate: $filter('toMMDDYYYYDate')(item.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                                    item: lookupService.getText("Users", item.TakenBy),
                                    state: "patientprofile.chart.intake.clinicalAssessment",
                                    contactID: contactID,
                                    id: item.ClinicalAssessmentID
                                });
                            });
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                                tileInfoModel.IsLoaded = true;
                            });
                    }
                    return tileInfoModel;
                }

                getChiefComplaintTiles = function (contactID) {
                    var permissionKey = 'Clinical-ChiefComplaint-ChiefComplaint';
                    var tileInfoModel = null;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var tileAddDetails = [];
                        var url = "intake.chiefcomplaint";
                        var tileInfoModel = initTiles("Chief Complaint", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                        tileInfoModel.TileDetails = [];
                        chiefComplaintService.getList(contactID).then(function (complaintData) {
                            var result = [];
                            if ((complaintData.DataItems != null) && (complaintData.DataItems.length > 0)) {
                                var model = complaintData.DataItems[0];
                                if (checkModel(model.TakenBy)) {
                                    result.push(getTileDetailsModel("Users", model.TakenBy, "Taken By"));
                                }
                                if (checkModel(model.TakenTime)) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(model.TakenTime, 'MM/DD/YYYY', 'useLocal');
                                    result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                                }

                                if (checkModel(model.Complaint)) {
                                    if (model.Complaint.length > restrictedTextLength)
                                        model.Complaint = model.Complaint.substr(0, restrictedTextLength) + '...';
                                    result.push(getTileDetailsModel(null, model.Complaint, "Complaint"));
                                }

                                tileInfoModel.TileDetails = result;

                                angular.forEach(complaintData.DataItems, function (item) {
                                    tileInfoModel.EditDetails.push({
                                        modifiedDate: $filter('toMMDDYYYYDate')(item.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                                        item: lookupService.getText("Users", item.TakenBy),
                                        state: "patientprofile.chart.intake.chiefcomplaint",
                                        contactID: contactID,
                                        id: item.ChiefComplaintID
                                    });
                                });
                            }
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server.", "Error")];
                                tileInfoModel.IsLoaded = true;
                            });
                    }
                    return tileInfoModel;
                }

                getVitalsTiles = function (contactID) {
                    var permissionKey = 'Clinical-Vitals-Vitals';
                    var tileInfoModel = null;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var url = "intake.vitals";
                        var tileAddDetails = [];
                        tileInfoModel = initTiles("Vitals", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                        tileInfoModel.TileDetails = [];

                        vitalService.getContactVital(contactID).then(function (vitalsData) {
                            var result = [];

                            if ((vitalsData.DataItems != null) && (vitalsData.DataItems.length > 0)) {
                                var model = $filter('orderBy')(vitalsData.DataItems, '-TakenTime')[0];

                                if (checkModel(model.TakenBy)) {
                                    result.push(getTileDetailsModel("Users", model.TakenBy, "Taken By"));
                                }
                                if (checkModel(model.TakenTime)) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(model.TakenTime, 'MM/DD/YYYY', 'useLocal');
                                    result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                                }

                                result.push(getTileDetailsModel(null, model.HeightFeet + '\'' + model.HeightInches + '", ' + model.WeightLbs + ' lbs ' + model.WeightOz + ' oz, ' + model.BMI, 'Height / Weight / BMI'));

                                if (checkModel(model.Systolic) && checkModel(model.Diastolic)) {
                                    result.push(getTileDetailsModel(null, model.Systolic + ' / ' + model.Diastolic, 'Blood Pressure'));
                                }
                                if (checkModel(model.Glucose)) {
                                    result.push(getTileDetailsModel(null, model.Glucose, 'Glucose'));
                                }

                                tileInfoModel.TileDetails = result;
                            }
                            angular.forEach(vitalsData.DataItems, function (item) {
                                tileInfoModel.EditDetails.push({
                                    modifiedDate: $filter('toMMDDYYYYDate')(item.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                                    item: lookupService.getText("Users", item.TakenBy),
                                    state: "patientprofile.chart.intake.vitals",
                                    contactID: contactID,
                                    id: item.VitalID
                                });
                            });
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                                tileInfoModel.IsLoaded = true;
                            });
                    }
                    return tileInfoModel;
                }

                getReviewOfSystemTiles = function (contactID) {
                    var permissionKey = 'Clinical-ReviewOfSystems-ReviewofSystems';
                    var tileInfoModel = null;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var url = "intake.reviewOfSystems";
                        var tileAddDetails = [];
                        tileInfoModel = initTiles("Review Of Systems", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                        tileInfoModel.TileDetails = [];

                        reviewOfSystemsService.getReviewOfSystemsByContact(contactID).then(function (reviewOfSystemsData) {
                            var result = [];
                            if ((reviewOfSystemsData.DataItems != null) && (reviewOfSystemsData.DataItems.length > 0)) {
                                var model = reviewOfSystemsData.DataItems[0];

                                if (checkModel(model.ReviewdBy)) {
                                    result.push(getTileDetailsModel("Users", model.ReviewdBy, "Taken By"));
                                }
                                if (checkModel(model.DateEntered)) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(model.DateEntered, 'MM/DD/YYYY', 'useLocal');
                                    result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                                }

                                tileInfoModel.TileDetails = result;
                            }
                            angular.forEach(reviewOfSystemsData.DataItems, function (item) {
                                tileInfoModel.EditDetails.push({
                                    modifiedDate: $filter('toMMDDYYYYDate')(item.DateEntered, 'MM/DD/YYYY', 'useLocal'),
                                    item: lookupService.getText("Users", item.ReviewdBy),
                                    state: "patientprofile.chart.intake.reviewOfSystems",
                                    contactID: contactID,
                                    id: item.RoSID
                                });
                            });
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                                tileInfoModel.IsLoaded = true;
                            });
                    }
                    return tileInfoModel;
                }

                getSocialRelationship = function (contactID) {
                    var url = "intake.socialrelationship";
                    var permissionKey = 'Clinical-SocialRelationshipHistory-SocialRelationshipHistory';
                    var tileAddDetails = [];
                    var tileInfoModel = initTiles("Social Relationship", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                    tileInfoModel.TileDetails = [];

                    socialRelationshipService.getSocialRelationships(contactID).then(function (data) {
                        var result = [];
                        if ((data.DataItems != null) && (data.DataItems.length > 0)) {
                            var model = data.DataItems[0];

                            if (checkModel(model.TakenBy)) {
                                result.push(getTileDetailsModel("Users", model.TakenBy, "Taken By"));
                            }
                            if (checkModel(model.TakenTime)) {
                                var formattedDate = $filter('toMMDDYYYYDate')(model.TakenTime, 'MM/DD/YYYY', 'useLocal');
                                result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                            }

                            tileInfoModel.TileDetails = result;
                        }
                        angular.forEach(data.DataItems, function (item) {
                            tileInfoModel.EditDetails.push({
                                modifiedDate: $filter('toMMDDYYYYDate')(item.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                                item: lookupService.getText("Users", item.TakenBy),
                                state: "patientprofile.chart.intake.socialrelationship.socialrelationshiphistory",
                                contactID: contactID,
                                id: item.SocialRelationshipID,
                                params: { contactID: contactID, socialRelationshipID: item.SocialRelationshipID, takenBy: item.TakenBy, takenTime: item.TakenTime }
                            });
                        });
                        tileInfoModel.IsLoaded = true;
                    },
                           function (errorStatus) {
                               tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                               tileInfoModel.IsLoaded = true;
                           });

                    return tileInfoModel;
                }

                getPresentIllness = function (contactID) {
                    var url = "intake.presentillness";
                    var permissionKey = 'Clinical-PresentIllness-PresentIllness';
                    var tileAddDetails = [];
                    var tileInfoModel = initTiles("History of Present Illness", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                    tileInfoModel.TileDetails = [];

                    //last bundle, but need to account for all hpis
                    presentIllnessService.getHPIBundle(contactID).then(function (bundleData) {
                        var result = [];
                        var tmpTakenBy = 0;
                        var tmpTakenOn = '';
                        if (bundleData.ResultCode === 0) {
                            if (bundleData.DataItems.length > 0) {
                                var bundleModel = $filter('orderBy')(bundleData.DataItems, 'TakenTime', true)[0];
                                tmpTakenBy = bundleModel.TakenBy;
                                tmpTakenOn = bundleModel.TakenTime;
                                if (checkModel(bundleModel.TakenBy)) {
                                    result.push(getTileDetailsModel("Users", bundleModel.TakenBy, "Taken By"));
                                }
                                if (checkModel(bundleModel.TakenTime)) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(bundleModel.TakenTime, 'MM/DD/YYYY', 'useLocal');
                                    result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                                }

                                tileInfoModel.TileDetails = result;
                                presentIllnessService.getHPIDetails(bundleModel.HPIID).then(function (data) {
                                    if (data.ResultCode === 0) {
                                        angular.forEach(data.DataItems, function (item) {
                                            tileInfoModel.EditDetails.push({
                                                modifiedDate: $filter('toMMDDYYYYDate')(tmpTakenOn, 'MM/DD/YYYY', 'useLocal'),
                                                item: lookupService.getText("Users", tmpTakenBy),
                                                state: "patientprofile.chart.intake.presentillness",
                                                contactID: contactID,
                                                id: item.HPIDetailID
                                            });
                                        });
                                    }
                                });
                            }
                            tileInfoModel.IsLoaded = true;
                        } else {
                            tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                            tileInfoModel.IsLoaded = true;
                        }
                    });

                    return tileInfoModel;
                }

                getMedicalHistoryTiles = function (contactID) {
                    var url = "intake.medicalhistory";
                    var permissionKey = 'Registration-Registration-Demographics';
                    var tileAddDetails = [];
                    var tileInfoModel = initTiles("Medical History", url, contactID, true, tileAddDetails, permissionKey, PERMISSION.CREATE);
                    tileInfoModel.TileDetails = [];

                    medicalHistoryService.getMedicalHistoryBundle(contactID).then(function (medicalHistoryData) {
                        var result = [];
                        var detailsArray = [];

                        if ((medicalHistoryData.DataItems != null) && (medicalHistoryData.DataItems.length > 0)) {
                            var model = $filter('orderBy')(medicalHistoryData.DataItems, '-TakenTime')[0];

                            if (checkModel(model.TakenBy)) {
                                result.push(getTileDetailsModel("Users", model.TakenBy, "Taken By"));
                            }

                            if (checkModel(model.TakenTime)) {
                                var formattedDate = $filter('toMMDDYYYYDate')(model.TakenTime, 'MM/DD/YYYY', 'useLocal');
                                result.push(getTileDetailsModel(null, formattedDate, "Taken Time"));
                            }

                            tileInfoModel.TileDetails = result;
                        }

                        angular.forEach(medicalHistoryData.DataItems, function (item) {
                            detailsArray.push([medicalHistoryService.getAllMedicalConditions, [item.MedicalHistoryID, contactID]]);
                            tileInfoModel.EditDetails.push({
                                modifiedDate: $filter('toMMDDYYYYDate')(item.DateEntered, 'MM/DD/YYYY', 'useLocal'),
                                item: lookupService.getText("Users", item.ReviewdBy),
                                state: "patientprofile.chart.intake.medicalhistory",
                                contactID: contactID,
                                id: item.MedicalHistoryID
                            });
                        });

                        $q.serial(detailsArray);

                        tileInfoModel.IsLoaded = true;
                    },
                    function (errorStatus) {
                        tileInfoModel.TileDetails = [getTileDetailsModel("Error", "Unable to connect to server", "Error")];
                        tileInfoModel.IsLoaded = true;
                    });

                    return tileInfoModel;
                }

                return {
                    get: get
                };
            }]);
}());