using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Data.Repository.Schema;

namespace Axis.DataProvider.Admin
{
    public class UserCredentialDataProvider : IUserCredentialDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public UserCredentialDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<UserCredentialModel> GetUserCredentials(int userID)
        {
            var userCredentialRepository = _unitOfWork.GetRepository<UserCredentialModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = userCredentialRepository.ExecuteStoredProc("usp_GetUserCredentials", procParams);

            return result;
        }

        public Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID)
        {
            var userCredentialsWithServiceIDRepository = _unitOfWork.GetRepository<UserCredentialModel>(SchemaName.Reference);
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            SqlParameter moduleComponentIDParam = new SqlParameter("ModuleComponentID", moduleComponentID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, moduleComponentIDParam };
            var result = userCredentialsWithServiceIDRepository.ExecuteStoredProc("usp_GetCredentialModuleComponentDetailsByUserID", procParams);

            return result;
        }

        public Response<UserModel> SaveUserCredentials(UserModel user)
        {
            var userRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", user.UserID);
            SqlParameter rolesParam = new SqlParameter("CredentialsXMLValue", GenerateCredentialXML(user.Credentials));
            rolesParam.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, rolesParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction(userRepository.ExecuteNQStoredProc,
                "usp_SaveUserCredentials", procParams,
                forceRollback: user.ForceRollback.GetValueOrDefault(false));

            return result;
        }

        #endregion

        #region Private Methods

        private string GenerateCredentialXML(List<UserCredentialModel> userCredentials)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings {OmitXmlDeclaration = true};

                using (var XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("CredentialsXMLValue");

                    if (userCredentials != null)
                    {
                        foreach (var credential in userCredentials)
                        {
                            XWriter.WriteStartElement("Credential");

                            XWriter.WriteElementString("CredentialID", credential.CredentialID.ToString());
                            XWriter.WriteElementString("LicenseNbr", credential.LicenseNbr);
                            if (credential.LicenseIssueDate != null)
                                XWriter.WriteElementString("EffectiveDate", credential.LicenseIssueDate.ToString());
                            if (credential.LicenseExpirationDate != null)
                                XWriter.WriteElementString("ExpirationDate", credential.LicenseExpirationDate.ToString());
                            if(credential.StateIssuedByID != 0 && credential.StateIssuedByID != null)
                                XWriter.WriteElementString("StateIssuedByID", credential.StateIssuedByID.ToString());

                            XWriter.WriteEndElement();
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
