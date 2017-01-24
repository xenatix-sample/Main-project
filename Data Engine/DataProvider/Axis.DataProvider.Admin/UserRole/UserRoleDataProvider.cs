using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.DataProvider.Admin
{
    public class UserRoleDataProvider : IUserRoleDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public UserRoleDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            var userRoleRepository = _unitOfWork.GetRepository<UserRoleModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = userRoleRepository.ExecuteStoredProc("usp_GetUserRoles", procParams);

            return result;
        }

        public Response<UserModel> SaveUserRoles(UserModel user)
        {
            var userRoleRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", user.UserID);
            SqlParameter rolesParam = new SqlParameter("RolesXMLValue", GenerateRoleXML(user.Roles));
            rolesParam.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, rolesParam, modifiedOnParam };
            //var result = userRoleRepository.ExecuteNQStoredProc("usp_SaveUserRoles", procParams);
            var result = _unitOfWork.EnsureInTransaction(userRoleRepository.ExecuteNQStoredProc, "usp_SaveUserRoles", procParams,
                forceRollback: user.ForceRollback.GetValueOrDefault(false));

            return result;
        }

        #endregion

        #region Private Methods

        private string GenerateRoleXML(List<RoleModel> roles)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("RolesXMLValue");

                    if (roles != null)
                    {
                        foreach (RoleModel role in roles)
                        {
                            XWriter.WriteElementString("RoleID", role.RoleID.ToString());
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
