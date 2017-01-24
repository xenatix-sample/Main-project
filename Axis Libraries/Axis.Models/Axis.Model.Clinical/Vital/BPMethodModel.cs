using Axis.Model.Common;
using System;

namespace Axis.Model.Clinical.Vital
{
    /// <summary>
    /// Model for BPMethod
    /// </summary>
  public class BPMethodModel:BaseEntity
    {
      /// <summary>
        /// BPMethodID
      /// </summary>
      public int BPMethodID { get; set; }

      /// <summary>
      /// BPMethod
      /// </summary>
      public string BPMethod { get; set; }


    }
}
