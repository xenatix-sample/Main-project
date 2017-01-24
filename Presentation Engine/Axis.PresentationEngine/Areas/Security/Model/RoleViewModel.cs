using Axis.PresentationEngine.Helpers.Model;
using System;
namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class RoleViewModel : BaseViewModel
    {
        public long RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
}