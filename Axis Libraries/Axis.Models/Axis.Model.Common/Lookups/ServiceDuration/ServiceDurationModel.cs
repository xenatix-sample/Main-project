namespace Axis.Model.Common
{
    public class ServiceDurationModel : BaseEntity
    {
        public int ServiceDurationID { get; set; }
        public int ServiceDurationStart { get; set; }
        public int? ServiceDurationEnd { get; set; }
        public string ServiceDurationDisplay { get; set; }
        public int SortOrder { get; set; }

    }
}