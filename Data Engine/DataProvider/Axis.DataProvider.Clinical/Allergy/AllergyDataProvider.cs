using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;

namespace Axis.DataProvider.Clinical.Allergy
{
    public class AllergyDataProvider : IAllergyDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public AllergyDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<ContactAllergyModel> GetAllergyBundle(long contactID, Int16 allergyTypeID)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyModel>(SchemaName.Clinical);

            SqlParameter contactIDParam = new SqlParameter("ContactID", contactID);
            SqlParameter allergyTypeIDParam = new SqlParameter("AllergyTypeID", allergyTypeID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam, allergyTypeIDParam };
            var allergyBundleResult = allergyRepository.ExecuteStoredProc("usp_GetContactAllergyByContactID", procParams);

            //We are only getting the most recent/active bundle
            return allergyBundleResult;
        }

        public Response<ContactAllergyModel> GetAllergy(long contactAllergyID)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyModel>(SchemaName.Clinical);

            SqlParameter contactAllergyIDParam = new SqlParameter("ContactAllergyID", contactAllergyID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactAllergyIDParam };
            var allergyResult = allergyRepository.ExecuteStoredProc("usp_GetContactAllergy", procParams);

            return allergyResult;
        }

        public Response<ContactAllergyDetailsModel> GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyDetailsModel>(SchemaName.Clinical);

            SqlParameter contactAllergyIDParam = new SqlParameter("ContactAllergyID", contactAllergyID);
            SqlParameter allergyTypeIDParam = new SqlParameter("AllergyTypeID", allergyTypeID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactAllergyIDParam, allergyTypeIDParam };
            var allergyResult = allergyRepository.ExecuteStoredProc("usp_GetAllergyDetails", procParams);
            var allergySymptomsResult = GetContactAllergyDetailSymptoms(contactAllergyID);

            //update to a linq query after testing
            if (allergySymptomsResult.DataItems.Count > 0)
            {
                foreach (var allergy in allergyResult.DataItems)
                {
                    var tmpAllergyDetailID = allergy.ContactAllergyDetailID;
                    foreach (var symptom in allergySymptomsResult.DataItems.Where(x => x.ContactAllergyDetailID == tmpAllergyDetailID))
                    {
                        allergy.Symptoms.Add(symptom.AllergySymptomID);
                    }
                }
            }

            return allergyResult;
        }

        public Response<ContactAllergyModel> DeleteAllergy(long contactAllergyID, DateTime modifiedOn)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyModel>(SchemaName.Clinical);

            SqlParameter contactAllergyIDParam = new SqlParameter("ContactAllergyID", contactAllergyID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactAllergyIDParam, modifiedOnParam };
            var allergyResult = _unitOfWork.EnsureInTransaction<Response<ContactAllergyModel>>(allergyRepository.ExecuteNQStoredProc, "usp_DeleteContactAllergy", procParams);

            return allergyResult;
        }

        public Response<ContactAllergyDetailsModel> DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyDetailsModel>(SchemaName.Clinical);

            SqlParameter contactAllergyDetailIDParam = new SqlParameter("ContactAllergyDetailID", contactAllergyDetailID);
            SqlParameter reasonForDeletionParam = new SqlParameter("ReasonForDeletion", reasonForDeletion);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactAllergyDetailIDParam, reasonForDeletionParam, modifiedOnParam };
            var allergyResult = _unitOfWork.EnsureInTransaction<Response<ContactAllergyDetailsModel>>(allergyRepository.ExecuteNQStoredProc, "usp_DeleteContactAllergyDetail", procParams);

            return allergyResult;
        }

        public Response<ContactAllergyModel> AddAllergy(ContactAllergyModel allergy)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyModel>(SchemaName.Clinical);

            SqlParameter encounterIDParam = new SqlParameter("EncounterID", allergy.EncounterID ?? (object)DBNull.Value);
            SqlParameter contactIDParam = new SqlParameter("ContactID", allergy.ContactID);
            SqlParameter allergyTypeIDParam = new SqlParameter("AllergyTypeID", allergy.AllergyTypeID);
            SqlParameter nkaParam = new SqlParameter("NoKnownAllergy", allergy.NoKnownAllergy);
            SqlParameter takenByParam = new SqlParameter("TakenBy", allergy.TakenBy);
            SqlParameter takenTimeParam = new SqlParameter("TakenTime", allergy.TakenTime);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", allergy.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { encounterIDParam, contactIDParam, allergyTypeIDParam, nkaParam, takenByParam, takenTimeParam, modifiedOnParam };
            var allergyResult = _unitOfWork.EnsureInTransaction<Response<ContactAllergyModel>>(allergyRepository.ExecuteNQStoredProc, "usp_AddContactAllergy", procParams, false, true);

            return allergyResult;
        }

        public Response<ContactAllergyDetailsModel> AddAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyDetailsModel>(SchemaName.Clinical);

            SqlParameter allergySymptomXmlParam = new SqlParameter("AllergySymptomXML", GenerateRequestXml(allergy.Symptoms));
            allergySymptomXmlParam.DbType = DbType.Xml;
            SqlParameter contactAllergyIDParam = new SqlParameter("ContactAllergyID", allergy.ContactAllergyID);
            SqlParameter allergyIDParam = new SqlParameter("AllergyID", allergy.AllergyID);
            SqlParameter severityIDParam = new SqlParameter("SeverityID", allergy.SeverityID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", allergy.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { allergySymptomXmlParam, contactAllergyIDParam, allergyIDParam, severityIDParam, modifiedOnParam };
            var allergyResult = _unitOfWork.EnsureInTransaction<Response<ContactAllergyDetailsModel>>(allergyRepository.ExecuteNQStoredProc, "usp_AddContactAllergyDetail", procParams, false, true);

            return allergyResult;
        }

        public Response<ContactAllergyModel> UpdateAllergy(ContactAllergyModel allergy)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyModel>(SchemaName.Clinical);

            SqlParameter contactAllergyIDParam = new SqlParameter("ContactAllergyID", allergy.ContactAllergyID);
            SqlParameter encounterIDParam = new SqlParameter("EncounterID", allergy.EncounterID ?? (object)DBNull.Value);
            SqlParameter allergyTypeIDParam = new SqlParameter("AllergyTypeID", allergy.AllergyTypeID);
            SqlParameter noKnownAllergyParam = new SqlParameter("NoKnownAllergy", allergy.NoKnownAllergy);
            SqlParameter takenByParam = new SqlParameter("TakenBy", allergy.TakenBy);
            SqlParameter takenTimeParam = new SqlParameter("TakenTime", allergy.TakenTime);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", allergy.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactAllergyIDParam, encounterIDParam, allergyTypeIDParam, noKnownAllergyParam, takenByParam, takenTimeParam, modifiedOnParam };
            var allergyResult = _unitOfWork.EnsureInTransaction<Response<ContactAllergyModel>>(allergyRepository.ExecuteNQStoredProc, "usp_UpdateContactAllergy", procParams);

            return allergyResult;
        }

        public Response<ContactAllergyDetailsModel> UpdateAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyDetailsModel>(SchemaName.Clinical);

            SqlParameter allergySymptomXmlParam = new SqlParameter("AllergySymptomXML", GenerateRequestXml(allergy.Symptoms));
            allergySymptomXmlParam.DbType = DbType.Xml;
            SqlParameter contactAllergyIDParam = new SqlParameter("ContactAllergyDetailID", allergy.ContactAllergyDetailID);
            SqlParameter allergyIDParam = new SqlParameter("AllergyID", allergy.AllergyID);
            SqlParameter severityIDParam = new SqlParameter("SeverityID", allergy.SeverityID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", allergy.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { allergySymptomXmlParam, contactAllergyIDParam, allergyIDParam, severityIDParam, modifiedOnParam };
            var allergyResult = _unitOfWork.EnsureInTransaction<Response<ContactAllergyDetailsModel>>(allergyRepository.ExecuteNQStoredProc, "usp_UpdateContactAllergyDetail", procParams);

            return allergyResult;
        }

        public Response<ContactAllergyHeaderModel> GetTopAllergies(long contactID)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyHeaderModel>(SchemaName.Clinical);

            SqlParameter contactIDParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam };
            var allergyResult = allergyRepository.ExecuteStoredProc("usp_GetTopSevereAllergyDetailsByContactID", procParams);

            return allergyResult;
        }
        #endregion

        #region Private Methods

        private Response<ContactAllergyDetailSymptomModel> GetContactAllergyDetailSymptoms(long contactAllergyID)
        {
            var allergyRepository = _unitOfWork.GetRepository<ContactAllergyDetailSymptomModel>(SchemaName.Clinical);
            SqlParameter contactAllergyIDParam = new SqlParameter("ContactAllergyID", contactAllergyID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactAllergyIDParam };
            var allergySymptomsResult = allergyRepository.ExecuteStoredProc("usp_GetContactAllergyDetailSymptoms", procParams);

            return allergySymptomsResult;
        }

        private string GenerateRequestXml(List<int> symptoms)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("SymptomDetails");

                    if (symptoms != null)
                    {
                        foreach (int allergySymptomID in symptoms)
                        {
                            XWriter.WriteElementString("AllergySymptomID", allergySymptomID.ToString());
                        }
                    }

                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        #endregion
    }
}
