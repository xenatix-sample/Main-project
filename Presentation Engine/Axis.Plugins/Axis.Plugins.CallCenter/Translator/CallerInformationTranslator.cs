using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.CallCenter.Translator
{
    public static class CallerInformationTranslator
    {

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static CallerInformationViewModel ToViewModel(this CallerInformationModel model)
        {
            if (model == null)
                return null;

            var entity = new CallerInformationViewModel
            {
                CallCenterHeaderID = model.CallCenterHeaderID,
                ReferralAgencyID = model.ReferralAgencyID,
                ProviderID = model.ProviderID,
                CallCenterTypeID = model.CallCenterTypeID,
                CallerContactID = model.CallerID,
                ClientContactID = model.ContactID,
                ContactTypeID=model.ContactTypeID,
                CallStartTime = model.CallStartTime,
                CallEndTime = model.CallEndTime,
                CallStatusID = model.CallStatusID,
                ProgramUnitID = model.ProgramUnitID,
                CountyID = model.CountyID,
                CallPriorityID = model.CallCenterPriorityID,
                SuicideHomicideID = model.SuicideHomicideID,
                DateOfIncident = model.DateOfIncident,
                ReasonCalled = model.ReasonCalled,
                Disposition = model.Disposition,
                OtherInformation = model.OtherInformation,
                FollowUpRequired = model.FollowUpRequired,
                Comments = model.Comments,
                IsCallerClientSame = (model.CallerID == model.ContactID),
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = DateTimeOffset.Parse(model.ModifiedOn.ToString()).UtcDateTime,
                IsLinkedToContact = model.IsLinkedToContact,
                ParentCallCenterHeaderID = model.ParentCallCenterHeaderID
            };
            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Response<CallerInformationViewModel> ToViewModel(this Response<CallerInformationModel> model)
        {
            if (model == null)
                return null;

            var entity = model.CloneResponse<CallerInformationViewModel>();
            var callerInformationModel = new List<CallerInformationViewModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate(CallerInformationModel callerInfoModel)
                {
                    var transformedModel = callerInfoModel.ToViewModel();
                    callerInformationModel.Add(transformedModel);
                });

                entity.DataItems = callerInformationModel;
            }

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static CallerInformationModel ToModel(this CallerInformationViewModel model)
        {
            if (model == null)
                return null;
            if (model.IsCallerClientSame)
            {
                model.ClientContactID = model.CallerContactID;
            }

            var entity = new CallerInformationModel
            {
                CallCenterHeaderID = model.CallCenterHeaderID,
                ReferralAgencyID = model.ReferralAgencyID,
                ProviderID = model.ProviderID,
                CallCenterTypeID = model.CallCenterTypeID,
                CallerID = model.CallerContactID,
                ContactID = model.ClientContactID,
                ContactTypeID= model.ContactTypeID,
                CallStartTime = model.CallStartTime,
                CallEndTime = model.CallEndTime,
                CallStatusID = model.CallStatusID,
                ProgramUnitID = model.ProgramUnitID,
                FollowUpRequired = model.FollowUpRequired,
                CountyID = model.CountyID,
                CallCenterPriorityID = model.CallPriorityID,
                SuicideHomicideID = model.SuicideHomicideID,
                DateOfIncident = model.DateOfIncident,
                ReasonCalled = model.ReasonCalled,
                Disposition = model.Disposition,
                OtherInformation = model.OtherInformation,
                Comments = model.Comments,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                IsLinkedToContact = model.IsLinkedToContact,
                ParentCallCenterHeaderID = model.ParentCallCenterHeaderID
            };
            return entity;
        }

    }
}
