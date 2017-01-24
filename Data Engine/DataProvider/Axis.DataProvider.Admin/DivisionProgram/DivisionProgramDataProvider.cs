using System;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Xml;

namespace Axis.DataProvider.Admin.DivisionProgram
{
    public class DivisionProgramDataProvider : IDivisionProgramDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public DivisionProgramDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods
        public Response<DivisionProgramModel> GetDivisionPrograms(int userID)
        {
            var repository = _unitOfWork.GetRepository<DivisionProgramDataModel>(SchemaName.Core);
            var procParams = new List<SqlParameter> { new SqlParameter("UserID", userID) };
            var divisionProgramResult = repository.ExecuteStoredProc("usp_GetUserOrgDetailsMapping", procParams);

            var result = divisionProgramResult.CloneResponse<DivisionProgramModel>();
            result.ID = userID;
            if (result.ResultCode != 0)
                return result;

            result.DataItems = new List<DivisionProgramModel>();

            var groupByDivisions = from userDivision in divisionProgramResult.DataItems
                                   group userDivision by userDivision.DivisionMappingID;

            foreach (var groupDivision in groupByDivisions)
            {
                var userDivisionProgram = new DivisionProgramModel();
                userDivisionProgram.userID = userID;
                userDivisionProgram.MappingID = groupDivision.Key;
                    
                var groupByPrograms = groupDivision.GroupBy(x => x.ProgramMappingID);

                foreach (var groupByProgram in groupByPrograms)
                {
                    var userPrograms = new ProgramsModel();
                    userPrograms.MappingID = groupByProgram.Key;
                    foreach (var item in groupByProgram)
                    {
                        userDivisionProgram.Name = item.Division;
                        userDivisionProgram.CompanyMappingID = item.CompanyMappingID;
                        userDivisionProgram.CompanyName = item.Company;
                        userPrograms.Name = item.Program;
                        var userProgramUnit = new DivisionProgramBaseModel();
                        userProgramUnit.MappingID = item.PUMappingID;
                        userProgramUnit.Name = item.PU;
                        userPrograms.ProgramUnits.Add(userProgramUnit);
                    }
                    userDivisionProgram.Programs.Add(userPrograms);
                }

                userDivisionProgram.Programs = userDivisionProgram.Programs.OrderBy(x => x.Name).ToList();
                result.DataItems.Add(userDivisionProgram);
            }
            return result;
        }

        public Response<DivisionProgramModel> SaveDivisionProgram(DivisionProgramModel divisionProgram)
        {
            var repository = _unitOfWork.GetRepository<DivisionProgramModel>(SchemaName.Core);
            SqlParameter divisionXMLParam = new SqlParameter("UserOrgDetailsXML", GenerateRequestXml(divisionProgram));
            divisionXMLParam.DbType = System.Data.DbType.Xml;

            List<SqlParameter> procParams = new List<SqlParameter>() { divisionXMLParam};
            var result = _unitOfWork.EnsureInTransaction<Response<DivisionProgramModel>>(repository.ExecuteNQStoredProc, "usp_UpdateUserOrgDetailsMapping", procParams, adonResult: false);

            return result;
        }
        #endregion

        #region Helpers
        private string GenerateRequestXml(DivisionProgramModel divisionProgram)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("UserOrgDetails");

                    WriteToXML(XWriter, divisionProgram.userID, divisionProgram.MappingID, divisionProgram.IsActive);

                    if (divisionProgram.Programs != null)
                    {
                        foreach (ProgramsModel userProgram in divisionProgram.Programs)
                        {
                            WriteToXML(XWriter, divisionProgram.userID, userProgram.MappingID, userProgram.IsActive);

                            foreach (DivisionProgramBaseModel userProgramUnit in userProgram.ProgramUnits)
                            {
                                WriteToXML(XWriter, divisionProgram.userID, userProgramUnit.MappingID, userProgramUnit.IsActive);
                            }
                        }
                    }
                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        public void WriteToXML(XmlWriter XWriter, int userID, long MappingID, bool? IsActive)
        {
            XWriter.WriteStartElement("UserOrgDetails");
            XWriter.WriteElementString("UserID", userID.ToString());
            XWriter.WriteElementString("MappingID", MappingID.ToString());
            XWriter.WriteElementString("IsActive", IsActive.ToString());
            XWriter.WriteElementString("ModifiedBy", 1.ToString());
            XWriter.WriteElementString("ModifiedOn", DateTime.Now.ToString());
            XWriter.WriteEndElement();
        }

        #endregion
    }
}
