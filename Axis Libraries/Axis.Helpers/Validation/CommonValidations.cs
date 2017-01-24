using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Axis.Helpers.Validation
{
    public class CommonValidations
    {
        #region Validation Attributes

        public class PostalCode : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                
                if (value != null && value.ToString().Trim().Length > 0)
                {                  
                        string val = value.ToString();
                        string pattern = @"^(\d{5}-\d{4}|\d{5})$";

                        if (!Regex.IsMatch(val, pattern))
                        {
                            return new ValidationResult("Invalid postal code");
                        }                    
                }

                return ValidationResult.Success;
            }
        }

        public class AlphaOnly : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    string val = value.ToString();
                    string pattern = @"^[a-zA-Z\. ]*$";

                    if (!Regex.IsMatch(val, pattern))
                    {
                        return new ValidationResult("Only alpha characters are allowed");
                    }
                }

                return ValidationResult.Success;
            }
        }

        #endregion
    }
}
