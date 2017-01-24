using Axis.Model.Address;
using Axis.Model.Email;
using Axis.Model.Phone;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Account.Model
{
    public class UserProfileViewModel : BaseViewModel
    {
        /// <summary>
        /// Handle the initialization of the three common list objects
        /// </summary>
        public UserProfileViewModel()
        {
            Addresses = new List<UserAddressModel>();
            Emails = new List<UserEmailModel>();
            Phones = new List<UserPhoneModel>();
        }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool IsTemporaryPassword { get; set; }
        public bool SaveSecurityQuestions { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserGUID { get; set; }
        public bool ADFlag { get; set; }
        public long UserSecurityQuestionID1 { get; set; }
        public long UserSecurityQuestionID2 { get; set; }
        public int SecurityQuestionID1 { get; set; }
        public int SecurityQuestionID2 { get; set; }
        public string SecurityAnswer1 { get; set; }
        public string SecurityAnswer2 { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string PrintSignature { get; set; }
        public string CurrentDigitalPassword { get; set; }
        public string NewDigitalPassword { get; set; }
        public string ConfirmDigitalPassword { get; set; }
        public List<UserAddressModel> Addresses { get; set; }
        public List<UserEmailModel> Emails { get; set; }
        public List<UserPhoneModel> Phones { get; set; }

        public string ADUserPasswordResetMessage { get; set; }
        public byte[] ThumbnailBLOB { get; set; }
    }
}