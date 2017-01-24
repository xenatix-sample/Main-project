using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Models
{
    public class DivisionProgramViewModel : DivisionProgramBaseViewModel
    {
        public DivisionProgramViewModel()
        {
            Programs = new List<ProgramsViewModel>();
        }
        /// <summary>
        /// Gets or sets the User ID
        /// </summary>
        public int userID { get; set; }

        /// <summary>
        /// Gets or sets the Company Mapping ID
        /// </summary>
        public int CompanyMappingID { get; set; }

        /// <summary>
        /// Get Name
        /// </summary>
        public string CompanyName { get; set; }

        public int RowCount { get; set; }

        /// <summary>
        /// Gets or sets the Programs
        /// </summary>
        public List<ProgramsViewModel> Programs { get; set; }
    }

    public class ProgramsViewModel : DivisionProgramBaseViewModel
    {
        public ProgramsViewModel()
        {
            ProgramUnits = new List<DivisionProgramBaseViewModel>();
        }

        public int RowCount { get; set; }
        /// <summary>
        /// Gets or sets the ProgramUnits
        /// </summary>
        public List<DivisionProgramBaseViewModel> ProgramUnits { get; set; }
    }

    public class DivisionProgramBaseViewModel : BaseEntity
    {
        /// <summary>
        /// Identity ID
        /// </summary>
        public long MappingID { get; set; }

        /// <summary>
        /// Get Name
        /// </summary>
        public string Name { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }

    }

}