using System;

namespace Axis.Model.Common
{
    public class ScreeningNameModel : BaseEntity
    {
        public long ScreeningNameID { get; set; }
        public string ScreeningName { get; set; }
        public Int16? ScreeningTypeID { get; set; }
    }
}