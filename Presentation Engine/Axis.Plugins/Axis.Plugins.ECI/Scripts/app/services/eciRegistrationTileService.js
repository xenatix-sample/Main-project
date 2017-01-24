(function () {
    angular.module('xenatixApp')
    .factory('eciRegistrationTileService', ["$q", '$filter', 'lookupService', 'eciDemographicService', 'eciAdditionalDemographicService', 'referralAdditionalDetailService', 'roleSecurityService',
    function ($q, $filter, lookupService, eciDemographicService, eciAdditionalDemographicService, referralAdditionalDetailService, roleSecurityService) {

        var getDemographyTile = function (contactID) {
            var permissionKey = 'General-General-Demographics';
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                var demographyInfoModel = initTiles("Demographics", "ecidemographics", contactID, false, null, permissionKey, PERMISSION.CREATE);
                var resultDemography = [];

                eciDemographicService.get(contactID).then(function (contactDemographic) {
                    if ((contactDemographic.DataItems != null) && (contactDemographic.DataItems.length > 0)) {
                        var model = contactDemographic.DataItems[0];
                        //Demography Tile
                        if (model != undefined && model != null) {
                            if (checkModel(model.GestationalAge)) {
                                resultDemography.push(getTileDetailsModel(null, model.GestationalAge, "Gestational Age"));
                            }

                            if (checkModel(model.ClientAlternateIDs)) {
                                for (var i = 0; i < Math.min(3, model.ClientAlternateIDs.length) ; i++) {
                                    var otherID = model.ClientAlternateIDs[i].ClientIdentifierTypeID || 0;
                                    resultDemography.push(getTileDetailsModel(null, model.ClientAlternateIDs[i].AlternateID || '', lookupService.getText("ClientIdentifierType", otherID)));
                                    //resultDemography.push(getTileDetailsModel("ClientIdentifierType", otherID, "Other ID"));
                                    //resultDemography.push(getTileDetailsModel(null, model.ClientAlternateIDs[0].AlternateID || '', "Other ID Number"));
                                }
                            }
                        }

                        if (resultDemography.length > 0)
                            demographyInfoModel.TileDetails = resultDemography;
                    }
                    demographyInfoModel.IsLoaded = true;
                },
                    function (errorStatus) {
                        demographyInfoModel.IsLoaded = true;
                        demographyInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];

                    });
            }
            return demographyInfoModel;
        };

        var getadditionalDemographyTile = function (contactID) {
            var permissionKey = 'General-General-AdditionalDemographics';
            if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                var tileInfoModel = initTiles("Additional Demographics", "eciadditional", contactID, false, null, permissionKey, PERMISSION.CREATE);

                eciAdditionalDemographicService.getAdditionalDemographic(contactID).then(function (additionalDemographic) {
                    var result = [];
                    if ((additionalDemographic.DataItems != null) && (additionalDemographic.DataItems.length > 0) && additionalDemographic.DataItems[0].AdditionalDemographicID != 0) {
                        var model = additionalDemographic.DataItems[0];

                        if (checkModel(model.SchoolDistrictID)) {
                            result.push(getTileDetailsModel("SchoolDistrict", model.SchoolDistrictID, "School District"));
                        }
                        if (checkModel(model.RaceID)) {
                            if (parseInt(model.RaceID) == Other_TYPE.RaceID) {
                                if (model.OtherRace && model.OtherRace != "")
                                    result.push(getTileDetailsModel(null, model.OtherRace, "Race"));
                                else
                                    result.push(getTileDetailsModel(null, "Other", "Race"));
                            }
                            else
                                result.push(getTileDetailsModel("Race", model.RaceID, "Race"));
                        }
                        if (checkModel(model.EthnicityID)) {

                            if (parseInt(model.EthnicityID) == Other_TYPE.EthnicityID) {
                                if (model.OtherEthnicity && model.OtherEthnicity != "")
                                    result.push(getTileDetailsModel(null, model.OtherEthnicity, "Ethnicity"));
                                else
                                    result.push(getTileDetailsModel(null, "Other", "Ethnicity"));
                            }
                            else
                                result.push(getTileDetailsModel("Ethnicity", model.EthnicityID, "Ethnicity"));
                        }
                        if (checkModel(model.ReferralDispositionStatusID)) {
                            result.push(getTileDetailsModel("ReferralDispositionStatus", model.ReferralDispositionStatusID, "Disposition Status"));
                        }
                    }
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


        function get(contactID, listLimit) {
            var dfd = $q.defer();
            var tilePromises = [];
            tilePromises.push(getDemographyTile(contactID));
            tilePromises.push(getadditionalDemographyTile(contactID));

            $q.all(tilePromises).then(function (tileData) {
                var tileInfo = $filter('filter')(tileData.slice(0, tilePromises.length), function (item) { return item != null });
                dfd.resolve(tileInfo);
            });
            
            return dfd.promise;
        };

        return {
            get: get
        };
    }]);
}());