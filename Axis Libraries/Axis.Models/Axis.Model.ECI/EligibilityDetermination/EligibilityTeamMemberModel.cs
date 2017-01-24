using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.ECI.EligibilityDetermination
{
    public class EligibilityTeamMemberModel : ECIBaseModel
    {
        public long EligibilityID { get; set; }
        public int UserID { get; set; }
        public string CredentialAbbreviation { get; set; }
        public string Name { get; set; }
    }
}
