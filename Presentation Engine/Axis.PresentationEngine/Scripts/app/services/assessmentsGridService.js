(function () {
    angular.module('xenatixApp')
    .factory('assessmentsGridService', ["$http", "$q", 'assessmentService', 'callerInformationService', function ($http, $q, assessmentService, callerInformationService) {

        var ASSESSMENTS_SOURCES = {
            CrisisLine: {
                CrisisAssessment: ASSESSMENT_TYPE.CrisisAssessment,
                ColumbiaSuicideSeverityRatingScale: ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale,
                CrisisAdultScreening: ASSESSMENT_TYPE.CrisisAdultScreening,
                CrisisChildScreening: ASSESSMENT_TYPE.CrisisChildScreening
            },
            LawLiaison: {
                LawLiaisonScreening: ASSESSMENT_TYPE.LawLiaisonScreening,
                LawLiaisonProgressNote: ASSESSMENT_TYPE.LawLiaisonProgressNote
            }
        };


        function getAssessmentListByContactID(contactID) {
            return assessmentService.getAssessmentResponseListByContactID(contactID);
        };

        function getAssessmentSource(assessmentID) {
            for (var assessmentSourceKey in ASSESSMENTS_SOURCES) {
                for (var assessmentKey in ASSESSMENTS_SOURCES[assessmentSourceKey]) {
                    if (ASSESSMENTS_SOURCES[assessmentSourceKey][assessmentKey] === assessmentID) {
                        //return string with space before capital letter
                        return assessmentSourceKey.toString().replace(/([a-z])([A-Z])/g, "$1 $2");
                    }
                }
            }
        };

        return {
            getAssessmentSource: getAssessmentSource,
            getAssessmentListByContactID: getAssessmentListByContactID
        };
    }])

}());