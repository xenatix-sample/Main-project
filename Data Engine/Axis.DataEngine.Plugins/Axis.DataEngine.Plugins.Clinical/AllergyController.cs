using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.Allergy;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class AllergyController : BaseApiController
    {
        #region Class Variables

        readonly IAllergyDataProvider _allergyDataProvider = null;

        #endregion

        #region Constructors

        public AllergyController(IAllergyDataProvider allergyDataProvider)
        {
            _allergyDataProvider = allergyDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetAllergyBundle(long contactID, Int16 allergyTypeID)
        {
            var allergyResponse = _allergyDataProvider.GetAllergyBundle(contactID, allergyTypeID);
            return new HttpResult<Response<ContactAllergyModel>>(allergyResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetAllergy(long contactAllergyID)
        {
            var allergyResponse = _allergyDataProvider.GetAllergy(contactAllergyID);
            return new HttpResult<Response<ContactAllergyModel>>(allergyResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID)
        {
            var allergyResponse = _allergyDataProvider.GetAllergyDetails(contactAllergyID, allergyTypeID);
            return new HttpResult<Response<ContactAllergyDetailsModel>>(allergyResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetTopAllergies(long contactID)
        {
            var allergyResponse = _allergyDataProvider.GetTopAllergies(contactID);
            return new HttpResult<Response<ContactAllergyHeaderModel>>(allergyResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteAllergy(long contactAllergyID, DateTime modifiedOn)
        {
            var allergyResponse = _allergyDataProvider.DeleteAllergy(contactAllergyID, modifiedOn);
            return new HttpResult<Response<ContactAllergyModel>>(allergyResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn)
        {
            var allergyResponse = _allergyDataProvider.DeleteAllergyDetail(contactAllergyDetailID, reasonForDeletion, modifiedOn);
            return new HttpResult<Response<ContactAllergyDetailsModel>>(allergyResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddAllergy(ContactAllergyModel allergy)
        {
            var allergyResponse = _allergyDataProvider.AddAllergy(allergy);
            return new HttpResult<Response<ContactAllergyModel>>(allergyResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            var allergyResponse = _allergyDataProvider.AddAllergyDetail(allergy);
            return new HttpResult<Response<ContactAllergyDetailsModel>>(allergyResponse, Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateAllergy(ContactAllergyModel allergy)
        {
            var allergyResponse = _allergyDataProvider.UpdateAllergy(allergy);
            return new HttpResult<Response<ContactAllergyModel>>(allergyResponse, Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            var allergyResponse = _allergyDataProvider.UpdateAllergyDetail(allergy);
            return new HttpResult<Response<ContactAllergyDetailsModel>>(allergyResponse, Request);
        }

        #endregion
    }
}
