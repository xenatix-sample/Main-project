using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using modelBinding = System.Web.Http.ModelBinding;
using System.Web.Mvc;
using Axis.Model.Common;

namespace Axis.Helpers.Validation
{
    public static class ValidationHelper
    {
        public static string GetExpressionPath<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
        {
            return ExpressionHelper.GetExpressionText(expression);
        }

        public static void AddErrors<TModel>(Expression<Func<TModel, TModel>> modelExpression,
            modelBinding.ModelStateDictionary modelState, Response<TModel> response)
        {
            string modelPrefix = ((MemberExpression)modelExpression.Body).Member.Name + ".";
            if (response.ServerValidationErrors == null)
                response.ServerValidationErrors = new List<ServerValidationError>();
            response.ServerValidationErrors.AddRange(
                modelState.Select(delegate(KeyValuePair<string, modelBinding.ModelState> error)
                {
                    return new ServerValidationError
                    {
                        Key = error.Key.StartsWith(modelPrefix) ? error.Key.Substring(modelPrefix.Length) : error.Key,
                        Value = string.Join(", ", error.Value.Errors.Select(x => x.ErrorMessage))
                    };
                }));
        }
    }
}
