using System;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;

namespace Axis.Service.Clinical.PresentIllness
{
    public interface IPresentIllnessService
    {
        Response<PresentIllnessModel> GetHPIBundle(long contactID);
        Response<PresentIllnessModel> GetHPI(long HPIID);
        Response<PresentIllnessDetailModel> GetHPIDetail(long HPIID);
        Response<PresentIllnessModel> DeleteHPI(long HPIID, DateTime modifiedOn);
        Response<PresentIllnessDetailModel> DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn);
        Response<PresentIllnessModel> AddHPI(PresentIllnessModel hpi);
        Response<PresentIllnessDetailModel> AddHPIDetail(PresentIllnessDetailModel hpi);
        Response<PresentIllnessModel> UpdateHPI(PresentIllnessModel hpi);
        Response<PresentIllnessDetailModel> UpdateHPIDetail(PresentIllnessDetailModel hpi);
    }
}

