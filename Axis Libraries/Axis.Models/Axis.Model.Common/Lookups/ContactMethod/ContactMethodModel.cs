namespace Axis.Model.Common
{
    public class ContactMethodModel : BaseEntity
    {
        public int ContactMethodID { get; set; }
        public string ContactMethod { get; set; }
        public bool IsSystem { get; set; }
    }
}
