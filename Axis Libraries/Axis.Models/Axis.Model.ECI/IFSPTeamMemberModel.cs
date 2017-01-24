namespace Axis.Model.ECI
{
    public class IFSPTeamMemberModel : ECIBaseModel
    {
        public long IFSPID { get; set; }
        public int UserID { get; set; }
        public string CredentialAbbreviation { get; set; }
        public string Name { get; set; } 
    }
}