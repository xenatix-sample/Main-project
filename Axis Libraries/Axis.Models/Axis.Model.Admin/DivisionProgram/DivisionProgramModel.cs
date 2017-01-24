using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Model.Admin
{
    public class DivisionProgramModel : DivisionProgramBaseModel
    {
        public DivisionProgramModel()
        {
            Programs = new List<ProgramsModel>();
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

        /// <summary>
        /// Gets or sets the Programs
        /// </summary>
        public List<ProgramsModel> Programs { get; set; }
    }

    public class ProgramsModel : DivisionProgramBaseModel
    {
        public ProgramsModel()
        {
            ProgramUnits = new List<DivisionProgramBaseModel>();
        }
        /// <summary>
        /// Gets or sets the ProgramUnits
        /// </summary>
        public List<DivisionProgramBaseModel> ProgramUnits { get; set; }
    }

    public class DivisionProgramBaseModel : BaseEntity
    {
        /// <summary>
        /// Identity ID
        /// </summary>
        public long MappingID { get; set; }

        /// <summary>
        /// Get Name
        /// </summary>
        public string Name { get; set; }

    }
}
