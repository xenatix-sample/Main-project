
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Data.Repository;
using System.Data.SqlClient;
using System.Collections.Generic;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.Payors;

namespace Axis.DataProvider.BusinessAdmin.Payors
{
    public class PayorsDataProvider : IPayorsDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class 

        #region Constructors

        public PayorsDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods
        
        public Response<PayorsModel> GetPayors(string searchText)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            var payorsRepository = _unitOfWork.GetRepository<PayorsModel>(SchemaName.Reference);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("SearchText", searchText) };
            var payorDetails = payorsRepository.ExecuteStoredProc("usp_GetPayors", procParams);
            return payorDetails;
        }

        
        public Response<PayorsModel> AddPayor(PayorsModel payorDetails)
        {
            var payorsParameters = BuildPayorSpParams(payorDetails);
            var payorsRepository = _unitOfWork.GetRepository<PayorsModel>(SchemaName.Reference);
            return payorsRepository.ExecuteNQStoredProc("usp_AddPayorDetails", payorsParameters, idResult: true);
        }

        
        public Response<PayorsModel> UpdatePayor(PayorsModel payorDetails)
        {
            var payorsParameters = BuildPayorSpParams(payorDetails);
            var payorsRepository = _unitOfWork.GetRepository<PayorsModel>(SchemaName.Reference);
            return payorsRepository.ExecuteNQStoredProc("usp_UpdatePayorDetails", payorsParameters);
        }


        
        public Response<PayorsModel> GetPayorByID(int PayorId)
        {
            var payorsRepository = _unitOfWork.GetRepository<PayorsModel>(SchemaName.Reference);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("PayorID", PayorId) };
            var payorDetails = payorsRepository.ExecuteStoredProc("usp_GetPayorByID", procParams);
            return payorDetails;
        }

        #endregion

        private List<SqlParameter> BuildPayorSpParams(PayorsModel payorDetails)
        {
            var spParameters = new List<SqlParameter>();
            if (payorDetails.PayorID > 0)
                spParameters.Add(new SqlParameter("PayorID", payorDetails.PayorID));

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("PayorName", (object) payorDetails.PayorName ?? DBNull.Value),
                new SqlParameter("PayorCode", (object) payorDetails.PayorCode ?? DBNull.Value),
                new SqlParameter("PayorTypeID", (object) payorDetails.PayorTypeID ?? DBNull.Value),

                new SqlParameter("EffectiveDate", (object) payorDetails.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object) payorDetails.ExpirationDate ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object) payorDetails.ModifiedOn ?? DBNull.Value)

            });

            return spParameters;
        }

    }
}
