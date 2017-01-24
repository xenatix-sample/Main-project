namespace Axis.Model.Phone
{
    public class UserPhoneModel : PhoneModel
    {
        public int UserID { get; set; }
        public long UserPhoneID { get; set; }
        public bool IsPrimary { get; set; }
        public int? PhonePermissionID { get; set; }
    }
}
