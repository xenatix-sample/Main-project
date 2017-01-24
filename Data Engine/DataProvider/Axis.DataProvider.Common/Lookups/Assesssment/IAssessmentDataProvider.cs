using Axis.Model.Common;
using Axis.Model.Common.Lookups.Assessment;

namespace Axis.DataProvider.Common.Lookups.Assesssment
{
   public interface IAssessmentDataProvider
    {
       Response<AssessmentModel> GetAssessment(int documentTypeID);
       Response<AssessmentModel> GetAssessmentList();
    }
}
