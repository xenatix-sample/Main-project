using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common
{
    public class Request<T>
    {
        public T Param { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SoryColumn { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}
