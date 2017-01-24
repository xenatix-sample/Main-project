namespace Axis.Model.Common
{
    public class ClientTypeModel : BaseEntity
    {
        public int ClientTypeID { get; set; }
        public string Division { get; set; }
        public string ClientType { get; set; }
        public string RegistrationState { get; set; }
        public long OrganizationDetailID { get; set; }
    }
}
