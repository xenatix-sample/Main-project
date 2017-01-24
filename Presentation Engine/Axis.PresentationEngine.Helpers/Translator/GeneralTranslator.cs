using Axis.Model.Common;
using Axis.Model.Common.General;
using Axis.PresentationEngine.Helpers.Model.General;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Helpers.Translator
{
    public static class GeneralTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static KeyValueViewModel ToViewModel(this KeyValueModel entity)
        {
            if (entity == null)
                return null;

            var model = new KeyValueViewModel
            {
                Key = entity.Key,
                Value = entity.Value
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<KeyValueViewModel> ToViewModel(this Response<KeyValueModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<KeyValueViewModel>();
            var keyValueList = new List<KeyValueViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(KeyValueModel keyValue)
            {
                var transformModel = keyValue.ToViewModel();
                keyValueList.Add(transformModel);
            });
            model.DataItems = keyValueList;

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static KeyValueModel ToModel(this KeyValueViewModel model)
        {
            if (model == null)
                return null;
            var entity = new KeyValueModel
            {
                Key = model.Key,
                Value = model.Value
            };
            return entity;
        }
    }
}