using System;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.PresentIllness;

namespace Axis.Plugins.Clinical.Repository.PresentIllness
{
    public interface IPresentIllnessRepository
    {
        Response<PresentIllnessViewModel> GetHPIBundle(long contactID);
        Response<PresentIllnessDetailViewModel> GetHPIDetails(long hpiID);
        Response<PresentIllnessViewModel> GetHPI(long hpiID);
        Response<PresentIllnessViewModel> DeleteHPI(long hpiID, DateTime modifiedOn);
        Response<PresentIllnessViewModel> AddHPI(PresentIllnessViewModel hpi);        
        Response<PresentIllnessViewModel> UpdateHPI(PresentIllnessViewModel hpi);
        Response<PresentIllnessDetailViewModel> DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn);
        Response<PresentIllnessDetailViewModel> AddHPIDetail(PresentIllnessDetailViewModel hpi);
        Response<PresentIllnessDetailViewModel> UpdateHPIDetail(PresentIllnessDetailViewModel hpi);
    }
}
