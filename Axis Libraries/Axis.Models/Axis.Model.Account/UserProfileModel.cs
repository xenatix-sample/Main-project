using Axis.Model.Address;
using Axis.Model.Common;
using Axis.Model.Common.User;
using Axis.Model.Email;
using Axis.Model.Phone;
using System.Collections.Generic;

namespace Axis.Model.Account
{
    public class UserProfileModel : BaseEntity
    {
        /// <summary>
        /// Handle the initialization of the four common list objects
        /// </summary>
        public UserProfileModel()
        {
            Addresses = new List<UserAddressModel>();
            Emails = new List<UserEmailModel>();
            Phones = new List<UserPhoneModel>();
            SecurityQuestions = new List<UserSecurityQuestionModel>();
        }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool IsTemporaryPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserGUID { get; set; }
        public bool ADFlag { get; set; }
        public string SecurityAnswer1 { get; set; }
        public string SecurityAnswer2 { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string PrintSignature { get; set; }
        public string CurrentDigitalPassword { get; set; }
        public string NewDigitalPassword { get; set; }
        public string ConfirmDigitalPassword { get; set; }
        public List<UserSecurityQuestionModel> SecurityQuestions { get; set; } 
        public List<UserAddressModel> Addresses { get; set; }
        public List<UserEmailModel> Emails { get; set; }
        public List<UserPhoneModel> Phones { get; set; }

        public string ADUserPasswordResetMessage { get; set; }
        public byte[] ThumbnailBLOB { get; set; }
    }
}
