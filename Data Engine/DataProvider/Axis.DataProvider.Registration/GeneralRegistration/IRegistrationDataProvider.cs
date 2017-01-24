using Axis.Model.Common;
using Axis.Model.Registration;
using System;

namespace Axis.DataProvider.Registration
{
    public interface IRegistrationDataProvider
    {
        Response<ContactDemographicsModel> GetContactDemographics(long contactID);
        Response<ContactDemographicsModel> AddContactDemographics(ContactDemographicsModel contact);
        Response<ContactDemographicsModel> UpdateContactDemographics(ContactDemographicsModel contact);
        Response<ContactDemographicsModel> GetClientSummary(string SearchCriteria,string ContactType);
        Response<ContactAddressModel> GetContactAddress(long contactID);
        Response<ContactDemographicsModel> VerifyDuplicateContacts(ContactDemographicsModel contact);
        Response<String> GetSSN(long contactID);
    }
}