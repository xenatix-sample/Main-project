using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Model;
using Axis.Plugins.Registration.Model;
using Axis.Model.Email;
using System.Collections.Generic;

namespace Axis.Plugins.Registration
{
    public interface IContactEmailRepository
    {
        Response<ContactEmailViewModel> GetEmails(long contactID, int contactTypeID);
        Response<ContactEmailViewModel> AddUpdateEmail(List<ContactEmailViewModel> email);
        Response<ContactEmailViewModel> DeleteEmail(long contactEmailID, DateTime modifiedOn);
    }
}
