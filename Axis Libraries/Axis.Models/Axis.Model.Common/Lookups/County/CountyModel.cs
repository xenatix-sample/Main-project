
namespace Axis.Model.Common
{
    public class CountyModel : BaseEntity
    {
        public int CountyID { get; set; }
        public int StateProvinceID { get; set; }
        public string CountyName { get; set; }
        public string StateProvinceName { get; set; }
        public long OrganizationID { get; set; }
    }
}
