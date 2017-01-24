using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Axis.Helpers.Validation
{
    public static class ValidationExtention
    {
        public static string ParseError(this ModelStateDictionary model)
        {
            return string.Join("; ", model.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
        }
    }
}
