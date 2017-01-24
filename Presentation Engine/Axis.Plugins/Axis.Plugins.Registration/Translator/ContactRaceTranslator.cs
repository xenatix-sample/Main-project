using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class ContactRaceTranslator
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactRaceModel ToModel(this ContactRaceViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactRaceModel
            {
                ContactRaceID = model.ContactRaceID,
                ContactID = model.ContactID,
                RaceID = model.RaceID,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback
            };

            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactRaceViewModel ToViewModel(this ContactRaceModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactRaceViewModel
            {
                ContactRaceID = entity.ContactRaceID,
                ContactID = entity.ContactID,
                RaceID = entity.RaceID,
                IsActive = entity.IsActive,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactRaceViewModel> ToViewModel(this Response<ContactRaceModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<ContactRaceViewModel>();
            var lstViewModel = new List<ContactRaceViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactRaceModel m)
                {
                    var transformedModel = m.ToViewModel();
                    lstViewModel.Add(transformedModel);
                });
                model.DataItems = lstViewModel;
            }
            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactRaceViewModel> ToModel(this Response<ContactRaceModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ContactRaceViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactRaceModel contactRace)
                {
                    var transformedModel = contactRace.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<ContactRaceViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static List<ContactRaceModel> ToModel(this List<ContactRaceViewModel> model)
        {
            if (model == null)
                return null;

            var contactRaceModel = new List<ContactRaceModel>();

            model.ForEach(delegate(ContactRaceViewModel contactRace)
            {
                var transformedModel = contactRace.ToModel();
                contactRaceModel.Add(transformedModel);
            });
            return contactRaceModel;
        }
    }
}