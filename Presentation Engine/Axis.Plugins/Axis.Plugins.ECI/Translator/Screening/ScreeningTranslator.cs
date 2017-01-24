using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Model;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Plugins.ECI.Translator
{
    public static class ScreeningTranslator
    {
        public static Response<ScreeningViewModel> ToViewModel(this Response<ScreeningModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ScreeningViewModel>();
            model.DataItems = new List<ScreeningViewModel>();
            if (entity.DataItems != null)
                model.DataItems = entity.DataItems.Select(screeningModel =>
                    new ScreeningViewModel
                    {
                        ContactID = screeningModel.ContactID,
                        ScreeningID = screeningModel.ScreeningID,
                        ProgramID = screeningModel.ProgramID,
                        ProgramName = screeningModel.ProgramName,
                        InitialContactDate = screeningModel.InitialContactDate,
                        InitialServiceCoordinatorID = screeningModel.InitialServiceCoordinatorID,
                        InitialServiceCoordinator = screeningModel.InitialServiceCoordinator,
                        PrimaryServiceCoordinatorID = screeningModel.PrimaryServiceCoordinatorID,
                        PrimaryServiceCoordinator = screeningModel.PrimaryServiceCoordinator,
                        ScreeningDate = screeningModel.ScreeningDate,
                        ScreeningTypeID = screeningModel.ScreeningTypeID,
                        ScreeningType = screeningModel.ScreeningType,
                        AssessmentID = screeningModel.AssessmentID,
                        AssessmentName = screeningModel.AssessmentName,
                        ScreeningResultsID = screeningModel.ScreeningResultsID,
                        ScreeningResult = screeningModel.ScreeningResult,
                        ScreeningScore = screeningModel.ScreeningScore,
                        ScreeningStatusID = screeningModel.ScreeningStatusID,
                        ScreeningStatus = screeningModel.ScreeningStatus,
                        SubmittedByID = screeningModel.SubmittedByID,
                        SubmittedBy = screeningModel.SubmittedBy,
                        ResponseID = screeningModel.ResponseID ?? 0,
                        SectionID = screeningModel.SectionID ?? 0
                    }
                ).ToList();
            return model;
        }

        public static ScreeningModel ToModel(this ScreeningViewModel model)
        {
            if (model == null)
                return null;

            var result = new ScreeningModel
            {
                ContactID = model.ContactID,
                ScreeningID = model.ScreeningID,
                ProgramID = model.ProgramID,
                ProgramName = model.ProgramName,
                InitialContactDate = model.InitialContactDate,
                InitialServiceCoordinatorID = model.InitialServiceCoordinatorID,
                InitialServiceCoordinator = model.InitialServiceCoordinator,
                PrimaryServiceCoordinatorID = model.PrimaryServiceCoordinatorID,
                PrimaryServiceCoordinator = model.PrimaryServiceCoordinator,
                ScreeningDate = model.ScreeningDate,
                ScreeningTypeID = model.ScreeningTypeID,
                ScreeningType = model.ScreeningType,
                AssessmentID = model.AssessmentID,
                AssessmentName = model.AssessmentName,
                ScreeningResultsID = model.ScreeningResultsID,
                ScreeningResult = model.ScreeningResult,
                ScreeningScore = model.ScreeningScore,
                ScreeningStatusID = model.ScreeningStatusID,
                ScreeningStatus = model.ScreeningStatus,
                SubmittedByID = model.SubmittedByID,
                SubmittedBy = model.SubmittedBy,
                ResponseID = model.ResponseID ?? 0,
                SectionID = model.SectionID ?? 0,
                ModifiedOn = model.ModifiedOn
            };

            return result;
        }
    }
}