namespace Axis.Model.Admin
{
    public class StaffManagementModel : BaseEntity
    {
        public long ContactID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
