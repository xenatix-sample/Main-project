using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Admission model
    /// </summary>
    public class AdmissionModal : BaseEntity
    {
        /// <summary>
        /// Identity of admission
        /// </summary>
        public long ContactAdmissionID { get; set; }

        /// <summary>
        /// Contact id
        /// </summary>
        public long ContactID { get; set; }

        /// <summary>
        /// Effective date and time
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Admitting userid
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// is documentation complete
        /// </summary>
        public bool IsDocumentationComplete { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public string Comments { get; set; }


        /// <summary>
        /// Company
        /// </summary>
        public long CompanyID { get; set; }

        /// <summary>
        /// Division
        /// </summary>
        public long? DivisionID { get; set; }

        /// <summary>
        /// Program
        /// </summary>
        public long? ProgramID { get; set; }

        /// <summary>
        /// ProgramUnit
        /// </summary>
        public long? ProgramUnitID { get; set; }

        /// <summary>
        /// IsDischarged
        /// </summary>
        public bool? IsDischarged { get; set; }


        /// <summary>
        /// IsCompanyActiveForProgramUnit
        /// </summary>
        public int IsCompanyActiveForProgramUnit { get; set; }


        /// <summary>
        /// IsProgramUnitActiveForCompany
        /// </summary>
        public int IsProgramUnitActiveForCompany { get; set; }

        /// <summary>
        /// Discharge date and time
        /// </summary>
        public DateTime? DischargeDate { get; set; }

        /// <summary>
        /// Data key
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// Company Active
        /// </summary>
        public bool IsCompanyActive { get; set; }

        /// <summary>
        /// Contact discharge note id
        /// </summary>
        public long? ContactDischargeNoteID { get; set; }

        /// <summary>
        /// Gets or sets the Signed by entity.
        /// </summary>
        /// <value>
        /// The signed by entity identifier.
        /// </value>
        public long? SignedByEntityID { get; set; }

        /// <summary>
        /// Gets or sets the Signed by name.
        /// </summary>
        /// <value>
        /// The signed by identifier.
        /// </value>
        public string SignedByEntityName { get; set; }

        /// <summary>
        /// Gets or sets the date signed identifier.
        /// </summary>
        /// <value>
        /// The date date signed identifier.
        /// </value>
        public DateTime? DateSigned { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the admission reason.
        /// </summary>
        /// <value>
        /// The signed by admission reason identifier.
        /// </value>
        public int AdmissionReasonID { get; set; }
        

    }
}
