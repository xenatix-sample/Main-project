namespace Axis.Model.Common.Lookups.GroupService
{
    public class GroupServiceModel : BaseEntity
    {
        public int GroupTypeID { get; set; }
        public int ServicesID { get; set; }
        public string ServiceName { get; set; }
    }
}
