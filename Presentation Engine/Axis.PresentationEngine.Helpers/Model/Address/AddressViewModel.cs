namespace Axis.PresentationEngine.Helpers.Model
{
    public class AddressViewModel : BaseViewModel
    {
        public long ContactID { get; set; }
        public string AddressType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Zip { get; set; }
        public string AptComplexName { get; set; }
        public string GateCode { get; set; }
        public string MailPermissions { get; set; }
    }
}
