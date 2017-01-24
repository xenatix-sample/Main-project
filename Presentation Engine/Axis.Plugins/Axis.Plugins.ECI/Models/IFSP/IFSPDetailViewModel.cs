using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.Plugins.ECI.Model
{
    public class IFSPDetailViewModel : ECIBaseViewModel
    {
        public IFSPDetailViewModel()
        {
            Members = new List<IFSPTeamMemberModel>();
        }

        public long IFSPID { get; set; }
        public int IFSPTypeID { get; set; }
        public string IFSPType { get; set; }
        public DateTime IFSPMeetingDate { get; set; }
        public DateTime? IFSPFamilySignedDate { get; set; }
        public bool MeetingDelayed { get; set; }
        public int? ReasonForDelayID { get; set; }
        public string Comments { get; set; }
        public long? AssessmentID { get; set; }
        public long? ResponseID { get; set; }
        public long? SectionID { get; set; }
        public List<IFSPTeamMemberModel> Members { get; set; }
        public List<IFSPParentGuardianModel> ParentGuardians { get; set; }

    }
}