using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;
using Axis.PresentationEngine.Helpers.Translator;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Translator to convert view model to model and model to view model for Financial Assessment Screen
    /// </summary>
    public static class FinancialAssessmentTranslator
    {
        public static FinancialAssessmentViewModel ToViewModel(this FinancialAssessmentModel entity)
        {
            if (entity == null)
                return null;

            var model = new FinancialAssessmentViewModel
            {
                FinancialAssessmentID = entity.FinancialAssessmentID,
                ContactID = entity.ContactID,
                AssessmentDate = entity.AssessmentDate,
                TotalIncome = entity.TotalIncome,
                TotalExpenses = entity.TotalExpenses,
                TotalExtraOrdinaryExpenses = entity.TotalExtraOrdinaryExpenses,
                TotalOther = entity.TotalOther,
                AdjustedGrossIncome = entity.AdjustedGrossIncome,
                FamilySize = entity.FamilySize,
                ExpirationDate = entity.ExpirationDate,
                ExpirationReasonID = entity.ExpirationReasonID,
                ModifiedBy = entity.ModifiedBy,
                CreatedBy = entity.CreatedBy,
                ModifiedOn = entity.ModifiedOn,
                IsViewFinanicalAssessment=entity.IsViewFinanicalAssessment
            };

            return model;
        }

    public static Response<FinancialAssessmentViewModel> ToViewModel(this Response<FinancialAssessmentModel> entity)
    {
        if (entity == null)
            return null;

        var model = entity.CloneResponse<FinancialAssessmentViewModel>();
        var financialAssessmentView = new List<FinancialAssessmentViewModel>();

        if (entity.DataItems != null)
        {
            entity.DataItems.ForEach(delegate (FinancialAssessmentModel financialAssessment)
            {
                var transformedModel = financialAssessment.ToViewModel();
                financialAssessmentView.Add(transformedModel);
            });

            model.DataItems = financialAssessmentView;
        }

        return model;
    }

    public static FinancialAssessmentModel ToModel(this FinancialAssessmentViewModel model)
    {
        if (model == null)
            return null;

        var entity = new FinancialAssessmentModel
        {
            FinancialAssessmentID = model.FinancialAssessmentID,
            ContactID = model.ContactID,
            AssessmentDate = model.AssessmentDate,
            TotalIncome = model.TotalIncome,
            TotalExpenses = model.TotalExpenses,
            TotalExtraOrdinaryExpenses = model.TotalExtraOrdinaryExpenses,
            TotalOther = model.TotalOther,
            AdjustedGrossIncome = model.AdjustedGrossIncome,
            FamilySize = model.FamilySize,
            ExpirationDate = model.ExpirationDate,
            ExpirationReasonID = model.ExpirationReasonID,
            ModifiedBy = model.ModifiedBy,
            ModifiedOn = model.ModifiedOn,
            IsViewFinanicalAssessment = model.IsViewFinanicalAssessment
        };
        return entity;
    }

}
}
