using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Admin.Models
{
    public class StaffManagementViewModel : BaseViewModel
    {
        public long ContactID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}