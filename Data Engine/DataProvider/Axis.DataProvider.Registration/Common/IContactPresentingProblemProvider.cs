using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;

namespace Axis.DataProvider.Registration.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactPresentingProblemProvider
    {
        Response<ContactPresentingProblemModel> GetContactPresentingProblem(long contactID);

        Response<ContactPresentingProblemModel> AddContactPresentingProblem(ContactPresentingProblemModel presentingProblem);

        Response<ContactPresentingProblemModel> UpdateContactPresentingProblem(ContactPresentingProblemModel presentingProblem);

        Response<ContactPresentingProblemModel> DeleteContactPresentingProblem(long contactPresentingProblemID, DateTime modifiedOn);
    }
}