using Axis.Model;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.CallCenter.Translator
{
    public static class CallCenterProgressNoteTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static CallCenterProgressNoteViewModel ToViewModel(this CallCenterProgressNoteModel model)
        {
            if (model == null)
                return null;

            var entity = new CallCenterProgressNoteViewModel
            {
                CallCenterHeaderID = model.CallCenterHeaderID,
                NatureofCall = model.NatureofCall,
                CallTypeID = model.CallTypeID,
                ProgressNoteID=model.ProgressNoteID,
                CallTypeOther = model.CallTypeOther,
                FollowupPlan = model.FollowupPlan,
                ClientStatusID = model.ClientStatusID,
                ClientProvider = model.ClientProvider,
                Comments = model.Comments,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                NoteHeaderID = model.NoteHeaderID,
                ReferralAgencyID=model.ReferralAgencyID,
                OtherReferralAgency = model.OtherReferralAgency,
                ClientProviderID = model.ClientProviderID,
                DescribeOther = model.DescribeOther,
                BehavioralCategoryID = model.BehavioralCategoryID,
                CallStartTime = model.CallStartTime,
                Disposition = model.Disposition,
                ProviderID = model.ProviderID,
                CallCenterTypeID = model.CallCenterTypeID,
                CallerContactID = model.CallerID,
                ClientContactID = model.ContactID,
                ContactTypeID = model.ContactTypeID,
                CallEndTime = model.CallEndTime,
                CallStatusID = model.CallStatusID,
                ProgramUnitID = model.ProgramUnitID,
                CountyID = model.CountyID,
                CallPriorityID = model.CallCenterPriorityID,
                SuicideHomicideID = model.SuicideHomicideID,
                DateOfIncident = model.DateOfIncident,
                ReasonCalled = model.ReasonCalled,
                OtherInformation = model.OtherInformation,
                FollowUpRequired = model.FollowUpRequired,
                IsCallerClientSame = (model.CallerID == model.ContactID),
                NoteAdded = model.NoteAdded,
                IsLinkedToContact = model.IsLinkedToContact,
                ParentCallCenterHeaderID = model.ParentCallCenterHeaderID,
                IsCallerSame = model.IsCallerSame,
                NewCallerID = model.NewCallerID
            };
            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Response<CallCenterProgressNoteViewModel> ToViewModel(this Response<CallCenterProgressNoteModel> model)
        {
            if (model == null)
                return null;

            var entity = model.CloneResponse<CallCenterProgressNoteViewModel>();
            var CallCenterProgressNoteModel = new List<CallCenterProgressNoteViewModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate(CallCenterProgressNoteModel callerInfoModel)
                {
                    var transformedModel = callerInfoModel.ToViewModel();
                    CallCenterProgressNoteModel.Add(transformedModel);
                });

                entity.DataItems = CallCenterProgressNoteModel;
            }

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static CallCenterProgressNoteModel ToModel(this CallCenterProgressNoteViewModel model)
        {
            if (model == null)
                return null;

            var entity = new CallCenterProgressNoteModel
            {
                CallCenterHeaderID = model.CallCenterHeaderID,
                NatureofCall = model.NatureofCall,
                ProgressNoteID=model.ProgressNoteID,
                CallTypeID = model.CallTypeID,
                CallTypeOther = model.CallTypeOther,
                FollowupPlan = model.FollowupPlan,
                ClientStatusID = model.ClientStatusID,
                ReferralAgencyID = model.ReferralAgencyID,
                OtherReferralAgency = model.OtherReferralAgency,
                ClientProvider = model.ClientProvider,
                Comments = model.Comments,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                NoteHeaderID = model.NoteHeaderID,
                ClientProviderID = model.ClientProviderID,
                DescribeOther = model.DescribeOther,
                BehavioralCategoryID = model.BehavioralCategoryID,
                CallStartTime = model.CallStartTime,
                Disposition = model.Disposition,
                ProviderID = model.ProviderID,
                CallCenterTypeID = model.CallCenterTypeID,
                CallerID = model.CallerContactID,
                ContactID = model.ClientContactID,
                ContactTypeID= model.ContactTypeID,
                CallEndTime = model.CallEndTime,
                CallStatusID = model.CallStatusID,
                ProgramUnitID = model.ProgramUnitID,
                FollowUpRequired = model.FollowUpRequired,
                CountyID = model.CountyID,
                CallCenterPriorityID = model.CallPriorityID,
                SuicideHomicideID = model.SuicideHomicideID,
                DateOfIncident = model.DateOfIncident,
                ReasonCalled = model.ReasonCalled,
                OtherInformation = model.OtherInformation,
                NoteAdded = model.NoteAdded,
                IsLinkedToContact = model.IsLinkedToContact,
                ParentCallCenterHeaderID = model.ParentCallCenterHeaderID,
                IsCallerSame = model.IsCallerSame,
                NewCallerID = model.NewCallerID
            };
            return entity;
        }
    }
}
