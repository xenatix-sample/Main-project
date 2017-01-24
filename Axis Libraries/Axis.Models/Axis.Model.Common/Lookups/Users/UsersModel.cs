namespace Axis.Model.Common
{
    public class UsersModel : BaseEntity
    {
        public int UserID { get; set; }
        public long? PhoneID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public int FacilityID { get; set; }
        public int ProgramID { get; set; }
        public int CredentialID { get; set; }
        public string CredentialAbbreviation { get; set; }
    }
}