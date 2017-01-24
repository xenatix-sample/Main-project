using Axis.Model.Common;
using System;

namespace Axis.Model.BusinessAdmin
{
    public class OrganizationModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the detail identifier.
        /// </summary>
        /// <value>
        /// The detail identifier.
        /// </value>
        public long? DetailID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the acronym.
        /// </summary>
        /// <value>
        /// The acronym.
        /// </value>
        public string Acronym { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the companies.
        /// </summary>
        /// <value>
        /// The companies.
        /// </value>
        public string Companies { get; set; }

        /// <summary>
        /// Gets or sets the divisions.
        /// </summary>
        /// <value>
        /// The divisions.
        /// </value>
        public string Divisions { get; set; }

        /// <summary>
        /// Gets or sets the programs.
        /// </summary>
        /// <value>
        /// The programs.
        /// </value>
        public string Programs { get; set; }

        /// <summary>
        /// Gets or sets the program units.
        /// </summary>
        /// <value>
        /// The program units.
        /// </value>
        public string ProgramUnits { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the is external.
        /// </summary>
        /// <value>
        /// The is external.
        /// </value>
        public bool? IsExternal { get; set; }
    }
}