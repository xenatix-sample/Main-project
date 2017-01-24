using Axis.Model.Common;

namespace Axis.Model.Setting
{
    sealed public class SettingModel : BaseEntity
    {
        public int SettingsID { get; set; }
        public string Settings { get; set; }
        public string Value { get; set; }
        public int SettingsTypeID { get; set; }
        public string SettingsType { get; set; }
        public int? EntityID { get; set; }
        public int? SettingValuesID { get; set; }
    }
}