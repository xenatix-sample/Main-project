using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common.Lookups.Assessment
{
   public class AssessmentModel:BaseEntity
    {
       /// <summary>
       /// Assessment ID
       /// </summary>
       public long AssessmentID { get; set; }
       
       /// <summary>
       /// Assessment Name
       /// </summary>
       public string AssessmentName { get; set; }
    }
}
