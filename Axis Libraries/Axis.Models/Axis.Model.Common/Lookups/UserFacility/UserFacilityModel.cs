namespace Axis.Model.Common
{
  public  class UserFacilityModel : BaseEntity
    {
        public long UserFacilityID { get; set; }
        public int FacilityID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
