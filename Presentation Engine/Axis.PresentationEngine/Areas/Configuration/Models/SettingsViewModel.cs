using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Configuration.Models
{
    public class SettingsViewModel : BaseViewModel
    {
        public int SettingID { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public int SettingTypeID { get; set; }
        public string SettingType { get; set; }
        public int? EntityID { get; set; }
        public int? SettingValuesID { get; set; }
    }
}