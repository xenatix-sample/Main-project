using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Plugins.ECI.Models.ECIDemographics;

namespace Axis.Plugins.ECI.Repository.ECIDemographic
{
    public interface IECIDemographicRepository
    {
        Task<Response<ECIContactDemographicsViewModel>> GetContactDemographics(long contactID);
        Response<ECIContactDemographicsViewModel> AddContactDemographics(ECIContactDemographicsViewModel contact);
        Response<ECIContactDemographicsViewModel> UpdateContactDemographics(ECIContactDemographicsViewModel contact);
    }
}
