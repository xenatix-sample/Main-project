using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Model;

namespace Axis.Plugins.ECI.Translator
{
    /// <summary>
    /// Translator to convert Model to ViewModel and vice versa
    /// </summary>
    public static class IFSPDetailTranslator
    {
        /// <summary>
        /// Converts IFSPDetailModel to IFSPDetailViewModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IFSPDetailViewModel ToViewModel(this IFSPDetailModel entity)
        {
            if (entity == null)
                return null;
            var model = new IFSPDetailViewModel
            {
                IFSPID = entity.IFSPID,
                AssessmentID = entity.AssessmentID,
                IFSPTypeID = entity.IFSPTypeID,
                IFSPType = entity.IFSPType,
                IFSPMeetingDate = entity.IFSPMeetingDate,
                IFSPFamilySignedDate = entity.IFSPFamilySignedDate,
                MeetingDelayed = entity.MeetingDelayed,
                ReasonForDelayID = entity.ReasonForDelayID,
                Comments = entity.Comments,
                ContactID = entity.ContactID,
                Members = entity.Members,
                ParentGuardians = entity.ParentGuardians,
                ResponseID = entity.ResponseID,
                SectionID = entity.SectionID,
                ModifiedOn = entity.ModifiedOn
            };
            return model;
        }

        /// <summary>
        /// Converts Response object of IFSPDetailModel tp Response object of IFSPDetailViewModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Response<IFSPDetailViewModel> ToViewModel(this Response<IFSPDetailModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<IFSPDetailViewModel>();
            var ifspList = new List<IFSPDetailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(IFSPDetailModel ifspModel)
                {
                    var transformedModel = ifspModel.ToViewModel();
                    ifspList.Add(transformedModel);
                });
                model.DataItems = ifspList;
            }
            return model;
        }

        /// <summary>
        /// Converts IFSPDetailViewModel to IFSPDetailModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IFSPDetailModel ToModel(this IFSPDetailViewModel model)
        {
            if (model == null)
                return null;

            var entity = new IFSPDetailModel
            {
                IFSPID = model.IFSPID,
                AssessmentID = model.AssessmentID,
                IFSPTypeID = model.IFSPTypeID,
                IFSPType = model.IFSPType,
                IFSPMeetingDate = model.IFSPMeetingDate,
                IFSPFamilySignedDate = model.IFSPFamilySignedDate,
                MeetingDelayed = model.MeetingDelayed,
                ReasonForDelayID = model.ReasonForDelayID,
                Comments = model.Comments,
                ContactID = model.ContactID,
                Members = model.Members,
                ParentGuardians = model.ParentGuardians,
                ResponseID = model.ResponseID,
                SectionID = model.SectionID,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}