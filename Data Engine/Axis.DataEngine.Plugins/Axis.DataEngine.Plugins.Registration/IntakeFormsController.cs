using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
   public class IntakeFormsController : BaseApiController
   {
       readonly IIntakeFormsDataProvider _IntakeFormsDataProvider;

       /// <summary>
       /// Initializes a new instance of the <see cref="IntakeFormsController"/> class.
       /// </summary>
       /// <param name="IntakeFormsDataProvider">The IntakeForms data provider.</param>
       public IntakeFormsController(IIntakeFormsDataProvider IntakeFormsDataProvider)
       {
           _IntakeFormsDataProvider = IntakeFormsDataProvider;
       }

       /// <summary>
       /// Gets the intake form.
       /// </summary>
       /// <param name="contactFormsID">The contact forms identifier.</param>
       /// <returns>IHttpActionResult.</returns>
       [HttpGet]
       public IHttpActionResult GetIntakeForm(long contactFormsID)
       {
           return new HttpResult<Response<FormsModel>>(_IntakeFormsDataProvider.GetIntakeForm(contactFormsID), Request);
       }

       /// <summary>
       /// Gets the forms.
       /// </summary>
       /// <param name="contactID">The contact identifier.</param>
       /// <returns>IHttpActionResult.</returns>
       [HttpGet]
       public IHttpActionResult GetIntakeFormsByContactID(long contactID)
       {
           return new HttpResult<Response<FormsModel>>(_IntakeFormsDataProvider.GetIntakeFormsByContactID(contactID), Request);
       }

       /// <summary>
       /// Adds the forms.
       /// </summary>
       /// <param name="formsModel">The forms model.</param>
       /// <returns>IHttpActionResult.</returns>
       [HttpPost]
       public IHttpActionResult AddIntakeForms(FormsModel formsModel)
       {
           return new HttpResult<Response<FormsModel>>(_IntakeFormsDataProvider.AddIntakeForms(formsModel), Request);
       }

       /// <summary>
       /// Updates the forms.
       /// </summary>
       /// <param name="formsModel">The forms model.</param>
       /// <returns>IHttpActionResult.</returns>
       [HttpPut]
       public IHttpActionResult UpdateIntakeForms(FormsModel formsModel)
       {
           return new HttpResult<Response<FormsModel>>(_IntakeFormsDataProvider.UpdateIntakeForms(formsModel), Request);
       }

       /// <summary>
       /// Deletes the forms.
       /// </summary>
       /// <param name="contactFormsID">The contact forms identifier.</param>
       /// <param name="modifiedOn">The modified on.</param>
       /// <returns>IHttpActionResult.</returns>
       [HttpDelete]
       public IHttpActionResult DeleteIntakeForms(long contactFormsID, DateTime modifiedOn)
       {
           return new HttpResult<Response<FormsModel>>(_IntakeFormsDataProvider.DeleteIntakeForms(contactFormsID, modifiedOn), Request);
       }
   }
}
