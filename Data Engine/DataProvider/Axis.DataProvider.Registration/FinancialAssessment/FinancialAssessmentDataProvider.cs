using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Data provider for Financial Assessment
    /// </summary>
    public class FinancialAssessmentDataProvider : IFinancialAssessmentDataProvider
    {
        #region initializations

        IUnitOfWork unitOfWork = null;
        ILogger logger = null;

        public FinancialAssessmentDataProvider(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// To get the financial assessment from FinancialAssessment table and Financial Assessment  details from FinancialAssessmentDetails (income or expenses) for contact id
        /// </summary>
        /// <param name="contactID">client contact id</param> 
        /// <param name="financialAssessmentID">financialAssessmentID</param>
        /// <returns>Financial assessment details</returns>
        public Response<FinancialAssessmentModel> GetFinancialAssessment(long contactID, long financialAssessmentID)
        {
            //Financial Assessment 
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID), new SqlParameter("FinancialAssessmentID", financialAssessmentID) };
            var financialAssessmentRepository = unitOfWork.GetRepository<FinancialAssessmentModel>(SchemaName.Registration);
            var financialAssessmentResults = financialAssessmentRepository.ExecuteStoredProc("usp_GetFinancialAssessment", procsParameters);

            if (financialAssessmentResults == null || financialAssessmentResults.ResultCode != 0) return financialAssessmentResults;

            //for getting Financial Assessment Detail and list of assessment
            if (financialAssessmentResults.DataItems != null && financialAssessmentResults.DataItems != null & financialAssessmentResults.DataItems.Count > 0)
            {
                financialAssessmentResults.DataItems[0].FinancialAssessmentDetails = GetFinancialAssessmentDetails(financialAssessmentResults.DataItems[0].FinancialAssessmentID).DataItems;
            }
            return financialAssessmentResults;
        }

        /// <summary>
        /// To get the financial assessment details (income or expenses) for contact id of FinancialAssessment
        /// </summary>
        /// <param name="financialAssessmentID">financialAssessmentID</param>
        /// <returns>Financial assessment Details</returns>
        private Response<FinancialAssessmentDetailsModel> GetFinancialAssessmentDetails(long financialAssessmentID)
        {
            var financialAssessmentRepository = unitOfWork.GetRepository<FinancialAssessmentDetailsModel>(SchemaName.Registration);

            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("FinancialAssessmentID", financialAssessmentID) };

            return financialAssessmentRepository.ExecuteStoredProc("usp_GetFinancialAssessmentDetails", procsParameters);
        }

        /// <summary>
        /// To Add the financial assessment for contact id of FinancialAssessmentModel
        /// </summary>
        /// <param name="financialAssessment">New financial Assessment model</param>
        /// <returns>Financial assessment</returns>
        public Response<FinancialAssessmentModel> AddFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            List<SqlParameter> procsParameters = BuildFinancialAssessmentSpParams(financialAssessment);
            var financialAssessmentRepository = unitOfWork.GetRepository<FinancialAssessmentModel>(SchemaName.Registration);
            Response<FinancialAssessmentModel> spResults = new Response<FinancialAssessmentModel>();
            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {
                    spResults = financialAssessmentRepository.ExecuteNQStoredProc("usp_AddFinancialAssessment", procsParameters, idResult: true);
                    if (spResults.ResultCode != 0)
                    {
                        spResults.ResultCode = spResults.ResultCode;
                        spResults.ResultMessage = spResults.ResultMessage;
                        return spResults;
                    }
                    if (!financialAssessment.ForceRollback.GetValueOrDefault(false))
                        unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
            return spResults;
        }

        /// <summary>
        /// To update the financial assessment for contact id  of FinancialAssessmentModel
        /// </summary>
        /// <param name="financialAssessment">Updated financial Assessment model</param>
        /// <returns>Financial assessment</returns>
        public Response<FinancialAssessmentModel> UpdateFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            var procsParameters = BuildFinancialAssessmentSpParams(financialAssessment);
            var financialAssessmentRepository = unitOfWork.GetRepository<FinancialAssessmentModel>(SchemaName.Registration);
            Response<FinancialAssessmentModel> spResults = new Response<FinancialAssessmentModel>();
            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {
                    spResults = financialAssessmentRepository.ExecuteNQStoredProc("usp_UpdateFinancialAssessment", procsParameters);
                    if (spResults.ResultCode != 0)
                    {
                        spResults.ResultCode = spResults.ResultCode;
                        spResults.ResultMessage = spResults.ResultMessage;
                    }
                    if (!financialAssessment.ForceRollback.GetValueOrDefault(false))
                        unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
            return spResults;
        }

        /// <summary>
        /// To Add the financial assessment details (income or expenses) for contact id of FinancialAssessmentDetailsModel
        /// </summary>
        /// <param name="financialAssessment">New financial Assessment Details model</param>
        /// <returns>Financial assessment</returns>
        public Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails)
        {
            List<SqlParameter> procsParameters = BuildFinancialAssessmentDetailsSpParams(financialAssessmentDetails);
            var financialAssessmentRepository = unitOfWork.GetRepository<FinancialAssessmentDetailsModel>(SchemaName.Registration);
            Response<FinancialAssessmentDetailsModel> spResults = new Response<FinancialAssessmentDetailsModel>();

            if (financialAssessmentDetails.CategoryID > 0 && financialAssessmentDetails.CategoryTypeID > 0 &&
                financialAssessmentDetails.FinanceFrequencyID > 0 && financialAssessmentDetails.Amount != null)
            {
                using (var transactionScope = unitOfWork.BeginTransactionScope())
                {
                    try
                    {
                        spResults = financialAssessmentRepository.ExecuteNQStoredProc("usp_AddFinancialAssessmentDetails", procsParameters, idResult: true);
                        if (spResults.ResultCode != 0)
                        {
                            spResults.ResultCode = spResults.ResultCode;
                            spResults.ResultMessage = spResults.ResultMessage;
                            return spResults;
                        }
                        if (!financialAssessmentDetails.ForceRollback.GetValueOrDefault(false))
                            unitOfWork.TransactionScopeComplete(transactionScope);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        spResults.ResultCode = -1;
                        spResults.ResultMessage = "An unexpected error has occurred!";
                    }
                }
            }

            return spResults;
        }

        /// <summary>
        /// To update the financial assessment details (income or expenses) for contact id  of FinancialAssessmentDetailsModel
        /// </summary>
        /// <param name="financialAssessment">Updated financial Assessment Details model</param>
        /// <returns>Financial assessment</returns>
        public Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails)
        {
            var procsParameters = BuildFinancialAssessmentDetailsSpParams(financialAssessmentDetails);
            var financialAssessmentRepository = unitOfWork.GetRepository<FinancialAssessmentDetailsModel>(SchemaName.Registration);
            Response<FinancialAssessmentDetailsModel> spResults = new Response<FinancialAssessmentDetailsModel>();
            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {

                    spResults = financialAssessmentRepository.ExecuteNQStoredProc("usp_UpdateFinancialAssessmentDetails", procsParameters);
                    if (spResults.ResultCode != 0)
                    {
                        spResults.ResultCode = spResults.ResultCode;
                        spResults.ResultMessage = spResults.ResultMessage;
                    }
                    if (!financialAssessmentDetails.ForceRollback.GetValueOrDefault(false))
                        unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
            return spResults;
        }

        /// <summary>
        /// To delete the financial assessment details (income or expenses)
        /// </summary>
        /// <param name="financialAssessmentDetailID">financialAssessmentDetailID</param>
        /// <returns>true if successfully deleted else false</returns>
        public Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("FinancialAssessmentDetailsID", financialAssessmentDetailID), new SqlParameter("ModifiedOn", modifiedOn) };

            var financialAssessmentRepository = unitOfWork.GetRepository<FinancialAssessmentModel>(SchemaName.Registration);
            var spResults = financialAssessmentRepository.ExecuteNQStoredProc("usp_DeleteFinancialAssessmentDetails", procsParameters);
            Response<bool> resultSet = new Response<bool>();
            resultSet.ResultCode = spResults.ResultCode;
            resultSet.ResultMessage = spResults.ResultMessage;
            return resultSet;

        }

        #endregion exposed functionality

        #region Helpers
        /// <summary>
        /// To crate sql parameter list for financial assessment model 
        /// </summary>
        /// <param name="financialAssessment">financial assessment model</param>
        /// <returns>sql parameter list</returns>
        private List<SqlParameter> BuildFinancialAssessmentSpParams(FinancialAssessmentModel financialAssessment)
        {
            var spParameters = new List<SqlParameter>();

            if (financialAssessment.FinancialAssessmentID > 0) // Update, not Add
                spParameters.Add(new SqlParameter("FinancialAssessmentID", financialAssessment.FinancialAssessmentID));
            else
            {
                spParameters.AddRange(new List<SqlParameter>
                {
                    new SqlParameter("ContactID", financialAssessment.ContactID)
                    
                });
            }

            spParameters.AddRange(new List<SqlParameter>
            {
                new SqlParameter("AssessmentDate", (object) financialAssessment.AssessmentDate ?? DBNull.Value),
                new SqlParameter("TotalIncome", financialAssessment.TotalIncome),
                new SqlParameter("TotalExpenses", financialAssessment.TotalExpenses),
                new SqlParameter("TotalExtraOrdinaryExpenses", financialAssessment.TotalExtraOrdinaryExpenses),
                new SqlParameter("TotalOther", financialAssessment.TotalOther),
                new SqlParameter("AdjustedGrossIncome", financialAssessment.AdjustedGrossIncome),
                new SqlParameter("FamilySize", financialAssessment.FamilySize),
                new SqlParameter("ExpirationDate", (object) financialAssessment.ExpirationDate ?? DBNull.Value),
                new SqlParameter("ExpirationReasonID", (object) financialAssessment.ExpirationReasonID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", financialAssessment.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }

        /// <summary>
        /// To crate sql parameter list for financial assessment details model 
        /// </summary>
        /// <param name="financialAssessmentDetail">financial assessment details model</param>
        /// <returns>sql parameter list</returns>
        private List<SqlParameter> BuildFinancialAssessmentDetailsSpParams(
            FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            var spParameters = new List<SqlParameter>();

            spParameters.Add(financialAssessmentDetail.FinancialAssessmentDetailsID > 0 // Update, not Add
                ? new SqlParameter("FinancialAssessmentDetailID", financialAssessmentDetail.FinancialAssessmentDetailsID)
                : new SqlParameter("FinancialAssessmentID", financialAssessmentDetail.FinancialAssessmentID)
                );

            spParameters.AddRange(new List<SqlParameter>
            {
                new SqlParameter("CategoryTypeID", (object) financialAssessmentDetail.CategoryTypeID ?? DBNull.Value),
                new SqlParameter("Amount", (object) financialAssessmentDetail.Amount ?? DBNull.Value),
                new SqlParameter("FinanceFrequencyID",(object) financialAssessmentDetail.FinanceFrequencyID ?? DBNull.Value),
                new SqlParameter("CategoryID", (object) financialAssessmentDetail.CategoryID ?? DBNull.Value),
                new SqlParameter("RelationshipTypeID", (object) financialAssessmentDetail.RelationshipTypeID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", financialAssessmentDetail.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }

        #endregion Helpers
    }
}
