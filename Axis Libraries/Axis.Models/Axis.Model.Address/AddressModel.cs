using Axis.Model.Common;
using Axis.Helpers.Validation;

namespace Axis.Model.Address
{
    /// <summary>
    ///
    /// </summary>
    public class AddressModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the address identifier.
        /// </summary>
        /// <value>
        /// The address identifier.
        /// </value>
        public long? AddressID { get; set; }

        /// <summary>
        /// Gets or sets the address type identifier.
        /// </summary>
        /// <value>
        /// The address type identifier.
        /// </value>
        public int? AddressTypeID { get; set; }

        /// <summary>
        /// Gets or sets the line1.
        /// </summary>
        /// <value>
        /// The line1.
        /// </value>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the line2.
        /// </summary>
        /// <value>
        /// The line2.
        /// </value>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [CommonValidations.AlphaOnly(ErrorMessage = "Invalid city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state province.
        /// </summary>
        /// <value>
        /// The state province.
        /// </value>
        public int? StateProvince { get; set; }

        /// <summary>
        /// Gets or sets the county.
        /// </summary>
        /// <value>
        /// The county.
        /// </value>
        public int? County { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>
        /// The zip.
        /// </value>
        [CommonValidations.PostalCode]
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the name of the apt complex.
        /// </summary>
        /// <value>
        /// The name of the apt complex.
        /// </value>
        public string ComplexName { get; set; }

        /// <summary>
        /// Gets or sets the gate code.
        /// </summary>
        /// <value>
        /// The gate code.
        /// </value>
        public string GateCode { get; set; }
    }
}