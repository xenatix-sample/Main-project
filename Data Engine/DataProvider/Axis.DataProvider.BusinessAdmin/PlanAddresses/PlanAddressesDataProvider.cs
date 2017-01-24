
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Data.Repository;
using System.Data.SqlClient;
using System.Collections.Generic;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.Payors;
using Axis.Model.Address;
using System.Xml.Linq;
using System.Linq;

namespace Axis.DataProvider.BusinessAdmin.PlanAddresses
{
    public class PlanAddressesDataProvider : IPlanAddressesDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class 

        #region Constructors

        public PlanAddressesDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods
        
        public Response<PlanAddressesModel> GetPlanAddresses(int payorPlanID)
        {
            var payorsRepository = _unitOfWork.GetRepository<PlanAddressesModel>(SchemaName.Registration);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("PayorPlanID", payorPlanID) };
            var payorDetails = payorsRepository.ExecuteStoredProc("usp_GetPayorAddressDetailsByPayorPlanID", procParams);
            return payorDetails;
        }

       
        public Response<PlanAddressesModel> GetPlanAddress(int payorAddressID)
        {
            var payorsRepository = _unitOfWork.GetRepository<PlanAddressesModel>(SchemaName.Registration);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("PayorAddressID", payorAddressID) };
            var payorDetails = payorsRepository.ExecuteStoredProc("usp_GetPayorAddressDetailsByPayorAddressID", procParams);
            return payorDetails;
        }

        
        public Response<PlanAddressesModel> AddPlanAddress(PlanAddressesModel payorDetails)
        {
            var payorPlanIDParameter = new SqlParameter("PayorPlanID", payorDetails.PayorPlanID);
            var electronicPayorIDParameter= new SqlParameter("ElectronicPayorID", payorDetails.ElectronicPayorID );
            var contactIDParameter= new SqlParameter("ContactID", payorDetails.ContactID);
            var requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(payorDetails));
            var payorsParameters = new List<SqlParameter>() { payorPlanIDParameter, requestXMLValueParam , electronicPayorIDParameter , contactIDParameter };
            var payorsRepository = _unitOfWork.GetRepository<PlanAddressesModel>(SchemaName.Registration);
            return payorsRepository.ExecuteNQStoredProc("usp_AddPayorAddressDetails", payorsParameters, idResult: true);
        }


        public Response<PlanAddressesModel> UpdatePlanAddress(PlanAddressesModel payorDetails)
        {
            var electronicPayorIDParameter = new SqlParameter("ElectronicPayorID", payorDetails.ElectronicPayorID);
            var contactIDParameter = new SqlParameter("ContactID", payorDetails.ContactID);
            var requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(payorDetails));
            var payorsParameters = new List<SqlParameter>() {requestXMLValueParam, electronicPayorIDParameter, contactIDParameter };
            var payorsRepository = _unitOfWork.GetRepository<PlanAddressesModel>(SchemaName.Registration);
            return payorsRepository.ExecuteNQStoredProc("usp_UpdatePayorAddressDetails", payorsParameters);
        }




        #endregion

        /// <summary>
        /// Addresseses to XML.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public string AddressesToXML(PlanAddressesModel planAddresses)
        {
            var xEle = new XElement("RequestXMLValue",
                
                 new XElement("Address",
                               new XElement("AddressID", planAddresses.AddressID),
                               new XElement("PayorAddressID", planAddresses.PayorAddressID),
                               new XElement("AddressTypeID", planAddresses.AddressTypeID),
                               new XElement("Line1", planAddresses.Line1),
                               new XElement("Line2", planAddresses.Line2),
                               new XElement("City", planAddresses.City),
                               new XElement("StateProvince", planAddresses.StateProvince),
                               new XElement("County", planAddresses.County),
                               new XElement("Zip", planAddresses.Zip),
                               new XElement("EffectiveDate", planAddresses.EffectiveDate ?? null),
                               new XElement("ExpirationDate", planAddresses.ExpirationDate ?? null),
                               new XElement("ModifiedOn", planAddresses.ModifiedOn ?? DateTime.Now)
                           ));

            return xEle.ToString();
        }
    }
}
