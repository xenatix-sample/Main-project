using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    public static class LettersTranslator
    {
        public static LettersViewModel ToViewModel(this LettersModel entity)
        {
            if (entity == null)
                return null;
            var model = new LettersViewModel
            {
                ContactLettersID = entity.ContactLettersID,
                ContactID = entity.ContactID,
                AssessmentID = entity.AssessmentID,
                ResponseID = entity.ResponseID,
                SentDate = entity.SentDate,
                UserID = entity.UserID,
                ProviderName = entity.ProviderName,
                LetterOutcomeID = entity.LetterOutcomeID,
                Comments = entity.Comments,
                ModifiedOn = entity.ModifiedOn
            };
            return model;
        }

        public static Response<LettersViewModel> ToViewModel(this Response<LettersModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<LettersViewModel>();
            var lettersList = new List<LettersViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(LettersModel lettersModel)
                {
                    var transformedModel = lettersModel.ToViewModel();
                    lettersList.Add(transformedModel);
                });

                model.DataItems = lettersList;
            }

            return model;
        }

        public static LettersModel ToModel(this LettersViewModel model)
        {
            if (model == null)
                return null;
            var entity = new LettersModel
            {
                ContactLettersID = model.ContactLettersID,
                ContactID = model.ContactID,
                AssessmentID = model.AssessmentID,
                ResponseID = model.ResponseID,
                SentDate = model.SentDate,
                UserID = model.UserID,
                ProviderName = model.ProviderName,
                LetterOutcomeID = model.LetterOutcomeID,
                Comments = model.Comments,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}
