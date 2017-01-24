using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Translator
{
    public static class ClientMergeTranslator
    {
        public static ClientMergeModel ToModel(this ClientMergeViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ClientMergeModel
            {
                ParentMRN = entity.ParentMRN,
                ChildMRN = entity.ChildMRN
            };

            return model;
        }

        public static ClientMergeViewModel ToViewModel(this ClientMergeModel entity)
        {
            if (entity == null)
                return null;

            var model = new ClientMergeViewModel
            {
                ParentMRN = entity.ParentMRN,
                ChildMRN = entity.ChildMRN,
            };

            return model;
        }

        public static Response<ClientMergeViewModel> ToViewModel(this Response<ClientMergeModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ClientMergeViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (ClientMergeModel clientMergeModel)
                {
                    var transformedModel = clientMergeModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<ClientMergeViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }
    }
}