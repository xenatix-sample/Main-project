using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.ESignature;
using Axis.Model.Common;
using Axis.Model.ESignature;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Financial Summary Data Provider - Prepare Financial Assessment Report based on HouseHold, Payor & Self Pay
    /// </summary>
    public class FinancialSummaryDataProvider : IFinancialSummaryDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        private IESignatureDataProvider eSignatureDataProvider = null;

        /// <summary>
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public FinancialSummaryDataProvider(IUnitOfWork unitOfWork, IESignatureDataProvider eSignatureDataProvider)
        {
            this.unitOfWork = unitOfWork;
            this.eSignatureDataProvider = eSignatureDataProvider;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the financial assessment report.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("FinancialSummaryID", financialSummaryID) };
            var financialSummaryRepository = unitOfWork.GetRepository<FinancialSummaryModel>(SchemaName.Registration);
            var financialSummaryResults = financialSummaryRepository.ExecuteStoredProc("usp_GetFinancialSummaryByContactID", procsParameters);

            if (financialSummaryResults.DataItems != null && financialSummaryResults.DataItems.Count > 0)
            {
                financialSummaryResults.DataItems.ForEach(delegate(FinancialSummaryModel financialSummary)
                {
                    FinancialSummaryXmlToModel(financialSummary);

                    var financialSummaryConfirmations = GetFinancialSummaryConfirmationStatement(financialSummary.FinancialSummaryID);
                    if (financialSummaryConfirmations.DataItems != null && financialSummaryConfirmations.DataItems.Count > 0)
                        financialSummary.FinancialSummaryConfirmationStatements = financialSummaryConfirmations.DataItems;

                    // Get Contact/Staff Signature
                    var contactSignature = GetSignature(financialSummary.FinancialSummaryID, (int)DocumentType.FinancialAssessment);
                    if (contactSignature.DataItems != null && contactSignature.DataItems.Count > 0)
                    {
                        financialSummary.ClientSignature = contactSignature.DataItems.FirstOrDefault(clientSig => clientSig.EntityId == financialSummaryResults.DataItems.FirstOrDefault().ContactID && clientSig.EntityTypeId==(int)EntityType.Contact);
                        financialSummary.StaffSignature = contactSignature.DataItems.FirstOrDefault(staffSig => staffSig.EntityId == financialSummaryResults.DataItems.FirstOrDefault().UserID && staffSig.EntityTypeId == (int)EntityType.User);
                        financialSummary.LegalAuthRepresentativeSignature = contactSignature.DataItems.FirstOrDefault(staffSig => staffSig.EntityId == financialSummaryResults.DataItems.FirstOrDefault().UserID && staffSig.EntityTypeId == (int)EntityType.ExternalUser);
                    }
                });
            }

            return financialSummaryResults;
        }
        /// <summary>
        /// Gets the all financial assessment reports.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> GetFinancialSummaries(long contactID)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var financialSummaryRepository = unitOfWork.GetRepository<FinancialSummaryModel>(SchemaName.Registration);
            var financialSummaryResults = financialSummaryRepository.ExecuteStoredProc("usp_GetFinancialSummaries", procsParameters);
            if (financialSummaryResults.DataItems != null && financialSummaryResults.DataItems.Count > 0)
            {
                financialSummaryResults.DataItems.ForEach(delegate(FinancialSummaryModel financialSummary)
                {
                    financialSummary = FinancialSummaryXmlToModel(financialSummary);

                    var financialSummaryConfirmations = GetFinancialSummaryConfirmationStatement(financialSummary.FinancialSummaryID);
                    if (financialSummaryConfirmations.DataItems != null && financialSummaryConfirmations.DataItems.Count > 0)
                        financialSummary.FinancialSummaryConfirmationStatements = financialSummaryConfirmations.DataItems;

                    // Get Contact/Staff Signature
                    var contactSignature = GetSignature(financialSummary.FinancialSummaryID, (int)DocumentType.FinancialAssessment);
                    if (contactSignature.DataItems != null && contactSignature.DataItems.Count > 0)
                    {
                        financialSummary.ClientSignature = contactSignature.DataItems.FirstOrDefault(clientSig => clientSig.EntityId == financialSummaryResults.DataItems.FirstOrDefault().ContactID && clientSig.EntityTypeId == (int)EntityType.Contact);
                        financialSummary.StaffSignature = contactSignature.DataItems.FirstOrDefault(staffSig => staffSig.EntityId == financialSummaryResults.DataItems.FirstOrDefault().UserID && staffSig.EntityTypeId == (int)EntityType.User);
                    }
                });
            }

            return financialSummaryResults;
        }

        /// <summary>
        /// Adds the financial assessment report.
        /// </summary>
        /// <param name="financialSummary">The financial assessment report.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> AddFinancialSummary(FinancialSummaryModel financialSummary)
        {
            var repository = unitOfWork.GetRepository<FinancialSummaryModel>(SchemaName.Registration);

            var spParameters = BuildFinancialSummarySpParams(financialSummary, false);
            var response = unitOfWork.EnsureInTransaction<Response<FinancialSummaryModel>>(repository.ExecuteNQStoredProc, "usp_AddFinancialSummary", spParameters, forceRollback: financialSummary.ForceRollback ?? false, idResult: true);

            financialSummary.FinancialSummaryConfirmationStatements.RemoveAll(item => item == null);
            financialSummary.FinancialSummaryID = response.ID;
            financialSummary.FinancialSummaryConfirmationStatements.ForEach(delegate(FinancialSummaryConfirmationStatementModel finalcialSummaryConfirmationStatement)
            {
                if (finalcialSummaryConfirmationStatement != null && finalcialSummaryConfirmationStatement.SignatureBLOB != null)
                {
                    finalcialSummaryConfirmationStatement.FinancialSummaryID = response.ID;
                    AddFinancialSummaryConfirmationStatement(finalcialSummaryConfirmationStatement);
                }
            });

            // Save Client Signature
            if (financialSummary.ClientSignature != null && financialSummary.ClientSignature.SignatureBlob != null)
            {
                AddSignature(financialSummary.ClientSignature.SignatureBlob,
                        financialSummary.FinancialSummaryID,
                        (int)DocumentType.FinancialAssessment,
                        null,
                        financialSummary.ContactID,
                        financialSummary.ClientSignature.EntityName,
                        (int)EntityType.Contact,
                        financialSummary.ClientSignature.ModifiedOn);
            }

            // Save Staff Signature
            if (financialSummary.StaffSignature != null && financialSummary.StaffSignature.SignatureBlob != null)
            {
                AddSignature(financialSummary.StaffSignature.SignatureBlob,
                        financialSummary.FinancialSummaryID,
                        (int)DocumentType.FinancialAssessment,
                        null,
                        financialSummary.UserID,
                        financialSummary.StaffSignature.EntityName,
                        (int)EntityType.User,
                        financialSummary.StaffSignature.ModifiedOn);
            }

            // Save Legally Authorized Representative Signature
            if (financialSummary.LegalAuthRepresentativeSignature != null && financialSummary.LegalAuthRepresentativeSignature.SignatureBlob != null)
            {
                AddSignature(financialSummary.LegalAuthRepresentativeSignature.SignatureBlob,
                        financialSummary.FinancialSummaryID,
                        (int)DocumentType.FinancialAssessment,
                        null,
                        financialSummary.UserID,
                        financialSummary.LegalAuthRepresentativeSignature.EntityName,
                        (int)EntityType.ExternalUser,
                        financialSummary.StaffSignature.ModifiedOn);
            }

            return response;
        }

        /// <summary>
        /// Updates the financial assessment.
        /// </summary>
        /// <param name="financialSummary">The financial assessment report.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> UpdateFinancialSummary(FinancialSummaryModel financialSummary)
        {
            var repository = unitOfWork.GetRepository<FinancialSummaryModel>(SchemaName.Registration);
            var spParameters = BuildFinancialSummarySpParams(financialSummary, true);
            var response = unitOfWork.EnsureInTransaction<Response<FinancialSummaryModel>>(repository.ExecuteNQStoredProc, "usp_UpdateFinancialSummary", spParameters, forceRollback: financialSummary.ForceRollback ?? false);

            if (financialSummary.FinancialSummaryConfirmationStatements != null)
            {
                financialSummary.FinancialSummaryConfirmationStatements.RemoveAll(item => item == null);

                financialSummary.FinancialSummaryConfirmationStatements.ForEach(delegate(FinancialSummaryConfirmationStatementModel finalcialSummaryConfirmationStatement)
                {
                    if (finalcialSummaryConfirmationStatement != null && finalcialSummaryConfirmationStatement.SignatureBLOB != null)
                    {
                        if (finalcialSummaryConfirmationStatement.FinancialSummaryConfirmationStatementID == 0)
                        {
                            finalcialSummaryConfirmationStatement.FinancialSummaryID = financialSummary.FinancialSummaryID;
                            AddFinancialSummaryConfirmationStatement(finalcialSummaryConfirmationStatement);
                        }
                        else
                            UpdateFinancialSummaryConfirmationStatement(finalcialSummaryConfirmationStatement);
                    }
                });
            }

            // Save Client Signature
            if (financialSummary.ClientSignature != null && financialSummary.ClientSignature.SignatureBlob != null && financialSummary.ClientSignature.EntitySignatureId == 0)
            {
                AddSignature(financialSummary.ClientSignature.SignatureBlob,
                        financialSummary.FinancialSummaryID,
                        (int)DocumentType.FinancialAssessment,
                        null,
                        financialSummary.ContactID,
                        financialSummary.ClientSignature.EntityName,
                        (int)EntityType.Contact,
                        financialSummary.ClientSignature.ModifiedOn);
            }

            // Save Staff Signature
            if (financialSummary.StaffSignature != null && financialSummary.StaffSignature.SignatureBlob != null && financialSummary.StaffSignature.EntitySignatureId == 0)
            {
                AddSignature(financialSummary.StaffSignature.SignatureBlob,
                        financialSummary.FinancialSummaryID,
                        (int)DocumentType.FinancialAssessment,
                        null,
                        financialSummary.UserID,
                        financialSummary.StaffSignature.EntityName,
                        (int)EntityType.User,
                        financialSummary.StaffSignature.ModifiedOn);
            }

            // Save Legally Authorized Representative Signature
            if (financialSummary.LegalAuthRepresentativeSignature != null && financialSummary.LegalAuthRepresentativeSignature.SignatureBlob != null && financialSummary.LegalAuthRepresentativeSignature.EntitySignatureId == 0)
            {
                AddSignature(financialSummary.LegalAuthRepresentativeSignature.SignatureBlob,
                        financialSummary.FinancialSummaryID,
                        (int)DocumentType.FinancialAssessment,
                        null,
                        financialSummary.UserID,
                        financialSummary.LegalAuthRepresentativeSignature.EntityName,
                        (int)EntityType.ExternalUser,
                        financialSummary.StaffSignature.ModifiedOn);
            }
            return response;
        }

        /// <summary>
        /// Gets the financial summary confirmation statement.
        /// </summary>
        /// <param name="financialSummaryID">The financial summary identifier.</param>
        /// <returns></returns>
        public Response<FinancialSummaryConfirmationStatementModel> GetFinancialSummaryConfirmationStatement(long financialSummaryID)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("FinancialSummaryID", financialSummaryID) };
            var financialSummaryRepository = unitOfWork.GetRepository<FinancialSummaryConfirmationStatementModel>(SchemaName.Registration);
            var financialSummaryResults = financialSummaryRepository.ExecuteStoredProc("usp_GetFinancialSummaryConfirmationStatement", procsParameters);

            financialSummaryResults.DataItems.ForEach(delegate(FinancialSummaryConfirmationStatementModel model)
            {
                var documentEntitySignatures = GetSignature(model.FinancialSummaryConfirmationStatementID, (int)DocumentType.ConfirmationStatement);
                if (documentEntitySignatures.DataItems != null && documentEntitySignatures.DataItems.Count > 0)
                {
                    model.EntitySignatureID = documentEntitySignatures.DataItems.FirstOrDefault().EntitySignatureId;
                    model.SignatureBLOB = documentEntitySignatures.DataItems.FirstOrDefault().SignatureBlob;
                }
            });

            return financialSummaryResults;
        }

        /// <summary>
        /// Adds the financial summary confirmation statement.
        /// </summary>
        /// <param name="financialSummaryConfirmationStatement"></param>
        /// <returns></returns>
        public Response<FinancialSummaryConfirmationStatementModel> AddFinancialSummaryConfirmationStatement(FinancialSummaryConfirmationStatementModel financialSummaryConfirmationStatement)
        {
            var repository = unitOfWork.GetRepository<FinancialSummaryConfirmationStatementModel>(SchemaName.Registration);

            var spParameters = BuildFinancialSummaryConfirmationStatementSpParams(financialSummaryConfirmationStatement, false);
            var response = unitOfWork.EnsureInTransaction<Response<FinancialSummaryConfirmationStatementModel>>(repository.ExecuteNQStoredProc, "usp_AddFinancialSummaryConfirmationStatement", spParameters, forceRollback: financialSummaryConfirmationStatement.ForceRollback ?? false, idResult: true);

            financialSummaryConfirmationStatement.FinancialSummaryConfirmationStatementID = response.ID;
            AddSignature(financialSummaryConfirmationStatement.SignatureBLOB,
                financialSummaryConfirmationStatement.FinancialSummaryConfirmationStatementID,
                (int)DocumentType.ConfirmationStatement,
                financialSummaryConfirmationStatement.EntitySignatureID,
                financialSummaryConfirmationStatement.ContactID,
                null,
                (int)EntityType.Contact,
                financialSummaryConfirmationStatement.ModifiedOn);

            return response;
        }

        /// <summary>
        /// Updates the financial summary confirmation statement.
        /// </summary>
        /// <param name="financialSummaryConfirmationStatement"></param>
        /// <returns></returns>
        public Response<FinancialSummaryConfirmationStatementModel> UpdateFinancialSummaryConfirmationStatement(FinancialSummaryConfirmationStatementModel financialSummaryConfirmationStatement)
        {
            var repository = unitOfWork.GetRepository<FinancialSummaryConfirmationStatementModel>(SchemaName.Registration);
            var spParameters = BuildFinancialSummaryConfirmationStatementSpParams(financialSummaryConfirmationStatement, true);
            var response = unitOfWork.EnsureInTransaction<Response<FinancialSummaryConfirmationStatementModel>>(repository.ExecuteNQStoredProc, "usp_UpdateFinancialSummaryConfirmationStatement", spParameters, forceRollback: financialSummaryConfirmationStatement.ForceRollback ?? false);

            if (financialSummaryConfirmationStatement.EntitySignatureID == null || financialSummaryConfirmationStatement.EntitySignatureID == 0)
            {
                AddSignature(financialSummaryConfirmationStatement.SignatureBLOB,
                    financialSummaryConfirmationStatement.FinancialSummaryConfirmationStatementID,
                    (int)DocumentType.ConfirmationStatement,
                    financialSummaryConfirmationStatement.EntitySignatureID,
                    financialSummaryConfirmationStatement.ContactID,
                    null,
                    (int)EntityType.Contact,
                    financialSummaryConfirmationStatement.ModifiedOn);
            }

            return response;
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <param name="financialSummaryConfirmationStatement">The financial summary confirmation statement.</param>
        /// <returns></returns>
        private Response<DocumentEntitySignatureModel> GetSignature(long documentID, int documentTypeID)
        {
            var documentEntitySignatures = eSignatureDataProvider.GetDocumentSignatures(documentID, documentTypeID);

            return documentEntitySignatures;
        }

        /// <summary>
        /// Adds the signature.
        /// </summary>
        /// <param name="financialSummaryConfirmationStatement">The financial summary confirmation statement.</param>
        private void AddSignature(byte[] signatureBLOB, long documentID, int documentTypeID, long? entitySignatureID, long? entityID, string entityName, int entityTypeID, DateTime? modifiedOn)
        {
            // Add Signature
            var signatureResponse = eSignatureDataProvider.AddSignature(new EntitySignatureModel()
            {
                SignatureBlob = signatureBLOB,
                ModifiedOn = modifiedOn,
                IsActive = true
            });

            // Add Save Document Signature
            if (signatureResponse.ResultCode == 0)
            {
                var documentSignatureResponse = eSignatureDataProvider.SaveDocumentSignature(new DocumentEntitySignatureModel()
                {
                    DocumentId = documentID,
                    DocumentTypeId = documentTypeID,
                    EntitySignatureId = entitySignatureID,
                    EntityId = entityID,
                    EntityName = entityName,
                    EntityTypeId = entityTypeID,
                    SignatureID = signatureResponse.ID,
                    IsActive = true,
                    ModifiedOn = modifiedOn
                });
            }
        }

        /// <summary>
        /// Builds the financial assessment report sp parameters.
        /// </summary>
        /// <param name="financialSummary">The financial assessment report.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildFinancialSummarySpParams(FinancialSummaryModel financialSummary, bool isUpdate)
        {
            var FinancialAssessmentXML = FinancialSummaryModelToXml(financialSummary);
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("FinancialSummaryID", financialSummary.FinancialSummaryID));

            spParameters.Add(new SqlParameter("ContactID", financialSummary.ContactID));
            spParameters.Add(new SqlParameter("OrganizationID", (object)financialSummary.OrganizationID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FinancialAssessmentXML", (object)FinancialAssessmentXML ?? DBNull.Value));
            spParameters.Add(new SqlParameter("DateSigned", (object)financialSummary.DateSigned ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EffectiveDate", (object)financialSummary.EffectiveDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AssessmentEndDate", (object)financialSummary.AssessmentEndDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ExpirationDate", (object)financialSummary.ExpirationDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SignatureStatusID", (object)financialSummary.SignatureStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("UserID", (object)financialSummary.UserID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("UserPhoneID", (object)financialSummary.UserPhoneID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("CredentialID", (object)financialSummary.CredentialID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", financialSummary.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        /// <summary>
        /// Builds the financial summary confirmation statement sp parameters.
        /// </summary>
        /// <param name="financialSummaryConfirmationStatement">The financial summary confirmation statement.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildFinancialSummaryConfirmationStatementSpParams(FinancialSummaryConfirmationStatementModel financialSummaryConfirmationStatement, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("FinancialSummaryConfirmationStatementID", financialSummaryConfirmationStatement.FinancialSummaryConfirmationStatementID));

            spParameters.Add(new SqlParameter("FinancialSummaryID", financialSummaryConfirmationStatement.FinancialSummaryID));
            spParameters.Add(new SqlParameter("ConfirmationStatementID", (object)financialSummaryConfirmationStatement.ConfirmationStatementID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("DateSigned", (object)financialSummaryConfirmationStatement.DateSigned ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SignatureStatusID", (object)financialSummaryConfirmationStatement.SignatureStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", financialSummaryConfirmationStatement.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        /// <summary>
        /// Financials the summary model to XML.
        /// </summary>
        /// <param name="financialSummary">The financial summary XML.</param>
        /// <returns></returns>
        private string FinancialSummaryModelToXml(FinancialSummaryModel financialSummary)
        {
            var xmlString =
                new XElement("FinancialSummary",
                    new XElement("EmploymentStatusID", financialSummary.EmploymentStatusID != null ? financialSummary.EmploymentStatusID : null),
                     new XElement("LookingForWork", financialSummary.LookingForWork != null ? financialSummary.LookingForWork : null),
                     new XElement("FAStaffName", financialSummary.FAStaffName != null ? financialSummary.FAStaffName : null),
                     new XElement("FAStaffCredential", financialSummary.FAStaffCredential != null ? financialSummary.FAStaffCredential : null),
                     new XElement("FAStaffPhone", financialSummary.FAStaffPhone != null ? financialSummary.FAStaffPhone : null),
                     new XElement("FAStaffExtension", financialSummary.FAStaffExtension != null ? financialSummary.FAStaffExtension : null),

                      financialSummary.FinancialAssessment != null ?
                            new XElement("FinancialAssessment",
                            new XElement("FinancialAssessmentID", financialSummary.FinancialAssessment.FinancialAssessmentID),
                            new XElement("TotalIncome", financialSummary.FinancialAssessment.TotalIncome),
                            new XElement("TotalExpenses", financialSummary.FinancialAssessment.TotalExpenses),
                            new XElement("TotalExtraOrdinaryExpenses", financialSummary.FinancialAssessment.TotalExtraOrdinaryExpenses),
                            new XElement("FamilySize", financialSummary.FinancialAssessment.FamilySize),
                            new XElement("AdjustedGrossIncome", financialSummary.FinancialAssessment.AdjustedGrossIncome)) : null,
                        new XElement("FinancialAssessmentDetails",
                            financialSummary.FinancialAssessmentDetails != null ?
                            from financialAssessmentDetails in financialSummary.FinancialAssessmentDetails
                            select
                            new XElement("FinancialAssessmentDetail",
                                new XElement("CategoryTypeID", financialAssessmentDetails.CategoryTypeID),
                                new XElement("Amount", financialAssessmentDetails.Amount),
                                new XElement("CategoryID", financialAssessmentDetails.CategoryID),
                                new XElement("FinanceFrequencyID", financialAssessmentDetails.FinanceFrequencyID))
                            : null),
                            new XElement("Payors",
                                financialSummary.Payors != null ?
                                from payors in financialSummary.Payors
                                select
                                new XElement("Payor",
                                    new XElement("PolicyHolderName", payors.PolicyHolderName != null ? payors.PolicyHolderName : null),
                                    new XElement("EffectiveDate", payors.EffectiveDate != null ? payors.EffectiveDate : null),
                                    new XElement("ExpirationDate", payors.ExpirationDate != null ? payors.ExpirationDate : null),
                                    new XElement("PayorName", payors.PayorName),
                                    new XElement("RelationshipTypeID", payors.RelationshipTypeID != null ? payors.RelationshipTypeID : null),
                                    new XElement("PlanID", payors.PlanID != null ? payors.PlanID : null),
                                    new XElement("PlanName", payors.PlanName != null ? payors.PlanName : null),
                                    new XElement("GroupID", payors.GroupID != null ? payors.GroupID : null),
                                    new XElement("PolicyID", payors.PolicyID != null ? payors.PolicyID : null),
                                    new XElement("GroupName", payors.GroupName != null ? payors.GroupName : null))
                                : null),
                                financialSummary.SelfPay != null ?
                                 new XElement("SelfPay",
                                     new XElement("ISChildInConservatorship", financialSummary.SelfPay.ISChildInConservatorship),
                                     new XElement("IsNotAttested", financialSummary.SelfPay.IsNotAttested),
                                     new XElement("IsEnrolledInPublicBenefits", financialSummary.SelfPay.IsEnrolledInPublicBenefits),
                                     new XElement("IsRequestingReconsideration", financialSummary.SelfPay.IsRequestingReconsideration),
                                     new XElement("IsNotGivingConsent", financialSummary.SelfPay.IsNotGivingConsent),
                                     new XElement("IsOtherChildEnrolled", financialSummary.SelfPay.IsOtherChildEnrolled),
                                     new XElement("IsApplyingForPublicBenefits", financialSummary.SelfPay.IsApplyingForPublicBenefits),
                                     new XElement("IsReconsiderationOfAdjustment", financialSummary.SelfPay.IsReconsiderationOfAdjustment),
                                     new XElement("MonthlyAbilities",
                                     from selfPay in financialSummary.SelfPay.MonthlyAbilities
                                     select
                                     new XElement("MonthlyAbility",
                                         new XElement("SelfPayID", selfPay.SelfPayID),
                                         new XElement("OrganizationDetailID", selfPay.OrganizationDetailID),
                                         new XElement("ContactID", selfPay.ContactID),
                                         new XElement("SelfPayAmount", selfPay.SelfPayAmount),
                                         new XElement("IsPercent", selfPay.IsPercent),
                                         new XElement("EffectiveDate", selfPay.EffectiveDate),
                                         new XElement("ExpirationDate", selfPay.ExpirationDate))))
                                       : null);
            return xmlString.ToString();
        }

        /// <summary>
        /// Financials the summary XML to model.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        private FinancialSummaryModel FinancialSummaryXmlToModel(FinancialSummaryModel financialSummary)
        {
            XDocument doc = XDocument.Parse(financialSummary.FinancialAssessmentXML);
            var copyOfFinancialSummary = new FinancialSummaryModel();
            copyOfFinancialSummary = (from root in doc.Descendants("FinancialSummary")
                                      select new FinancialSummaryModel()
                                      {
                                          EmploymentStatusID = root.Element("EmploymentStatusID").IsEmpty ? (int?)null : (int)root.Element("EmploymentStatusID"),
                                          LookingForWork = root.Element("LookingForWork").IsEmpty ? (bool?)null : (bool)root.Element("LookingForWork"),
                                          FAStaffName = root.Element("FAStaffName").IsEmpty ? null : (string)root.Element("FAStaffName"),
                                          FAStaffCredential = root.Element("FAStaffCredential").IsEmpty ? null : (string)root.Element("FAStaffCredential"),
                                          FAStaffPhone = root.Element("FAStaffPhone").IsEmpty ? null : (string)root.Element("FAStaffPhone"),
                                          FAStaffExtension = root.Element("FAStaffExtension") != null ? (root.Element("FAStaffExtension").IsEmpty ? null : (string)root.Element("FAStaffExtension")) : null,


                                          FinancialAssessment = (from financialAssessment in root.Descendants("FinancialAssessment")
                                                                 where root.Descendants("FinancialAssessment").Any()
                                                                 select new FinancialAssessmentModel()
                                                                 {
                                                                     FinancialAssessmentID = financialAssessment.Element("FinancialAssessmentID") != null ? (long)financialAssessment.Element("FinancialAssessmentID") : 0, // FinancialAssessmentID does not exists for old xml, so take care them
                                                                     TotalIncome = (decimal)financialAssessment.Element("TotalIncome"),
                                                                     TotalExpenses = (decimal)financialAssessment.Element("TotalExpenses"),
                                                                     TotalExtraOrdinaryExpenses = (decimal)financialAssessment.Element("TotalExtraOrdinaryExpenses"),
                                                                     AdjustedGrossIncome = (decimal)financialAssessment.Element("AdjustedGrossIncome"),
                                                                     FamilySize = (int)financialAssessment.Element("FamilySize")
                                                                 }).FirstOrDefault(),
                                          FinancialAssessmentDetails = (from financialAssessmentDetail in root.Descendants("FinancialAssessmentDetail")
                                                                        where root.Descendants("FinancialAssessmentDetail").Any()
                                                                        select new FinancialAssessmentDetailsModel()
                                                                        {
                                                                            CategoryTypeID = (int)financialAssessmentDetail.Element("CategoryTypeID"),
                                                                            CategoryID = (int)financialAssessmentDetail.Element("CategoryID"),
                                                                            Amount = (decimal)financialAssessmentDetail.Element("Amount"),
                                                                            FinanceFrequencyID = (int)financialAssessmentDetail.Element("FinanceFrequencyID")
                                                                        }).ToList(),
                                          Payors = (from payors in root.Descendants("Payor")
                                                    where root.Descendants("Payor").Any()
                                                    select new BenefitModel()
                                                    {
                                                        PolicyHolderName = payors.Element("PolicyHolderName").IsEmpty ? null : (string)payors.Element("PolicyHolderName"),
                                                        EffectiveDate = (payors.Element("EffectiveDate") == null || payors.Element("EffectiveDate").IsEmpty) ? (DateTime?)null : (DateTime)payors.Element("EffectiveDate"),
                                                        ExpirationDate = (payors.Element("ExpirationDate") == null || payors.Element("ExpirationDate").IsEmpty) ? (DateTime?)null : (DateTime)payors.Element("ExpirationDate"),
                                                        PayorName = (string)payors.Element("PayorName"),
                                                        RelationshipTypeID = payors.Element("RelationshipTypeID").IsEmpty ? (int?)null : (int)payors.Element("RelationshipTypeID"),
                                                        PlanID = payors.Element("PlanID").IsEmpty ? null : (string)payors.Element("PlanID"),
                                                        PlanName = payors.Element("PlanName").IsEmpty ? null : (string)payors.Element("PlanName"),
                                                        GroupID = payors.Element("GroupID").IsEmpty ? null : (string)payors.Element("GroupID"),
                                                        GroupName = payors.Element("GroupName").IsEmpty ? null : (string)payors.Element("GroupName"),
                                                        PolicyID = payors.Element("PolicyID").IsEmpty ? null : (string)payors.Element("PolicyID")
                                                    }).ToList(),
                                          SelfPay = (from selfPay in root.Descendants("SelfPay")
                                                     where root.Descendants("SelfPay").Any()
                                                     select new FinancialSummarySelfPayModel()
                                                     {
                                                         MonthlyAbilities = (from monthlyAbility in selfPay.Descendants("MonthlyAbility")
                                                                             where selfPay.Descendants("MonthlyAbility").Any()
                                                                             select new MonthlyAbilityToPayModel()
                                                                             {
                                                                                 SelfPayID = (long)monthlyAbility.Element("SelfPayID"),
                                                                                 OrganizationDetailID = (long)monthlyAbility.Element("OrganizationDetailID"),
                                                                                 ContactID = (long)monthlyAbility.Element("ContactID"),
                                                                                 SelfPayAmount = (monthlyAbility.Element("SelfPayAmount") == null || monthlyAbility.Element("SelfPayAmount").IsEmpty) ? (decimal?)null : (decimal)monthlyAbility.Element("SelfPayAmount"),
                                                                                 IsPercent = (monthlyAbility.Element("IsPercent") == null || monthlyAbility.Element("IsPercent").IsEmpty) ? (bool?)null : (bool)monthlyAbility.Element("IsPercent"),
                                                                                 EffectiveDate = ( monthlyAbility.Element("EffectiveDate") == null || monthlyAbility.Element("EffectiveDate").IsEmpty) ? (DateTime?)null : (DateTime)monthlyAbility.Element("EffectiveDate"),
                                                                                 ExpirationDate = monthlyAbility.Element("ExpirationDate") == null || (monthlyAbility.Element("ExpirationDate").IsEmpty) ? (DateTime?)null : (DateTime)monthlyAbility.Element("ExpirationDate")
                                                                             }
                                                                           ).ToList(),
                                                         ISChildInConservatorship = selfPay.Element("ISChildInConservatorship").IsEmpty ? false : (bool)selfPay.Element("ISChildInConservatorship"),
                                                         IsNotAttested = selfPay.Element("IsNotAttested").IsEmpty ? false : (bool)selfPay.Element("IsNotAttested"),
                                                         IsEnrolledInPublicBenefits = selfPay.Element("IsEnrolledInPublicBenefits").IsEmpty ? false : (bool)selfPay.Element("IsEnrolledInPublicBenefits"),
                                                         IsRequestingReconsideration = selfPay.Element("IsRequestingReconsideration").IsEmpty ? false : (bool)selfPay.Element("IsRequestingReconsideration"),
                                                         IsNotGivingConsent = selfPay.Element("IsNotGivingConsent").IsEmpty ? false : (bool)selfPay.Element("IsNotGivingConsent"),
                                                         IsOtherChildEnrolled = selfPay.Element("IsOtherChildEnrolled").IsEmpty ? false : (bool)selfPay.Element("IsOtherChildEnrolled"),
                                                         IsApplyingForPublicBenefits = selfPay.Element("IsApplyingForPublicBenefits").IsEmpty ? false : (bool)selfPay.Element("IsApplyingForPublicBenefits"),
                                                         IsReconsiderationOfAdjustment = selfPay.Element("IsReconsiderationOfAdjustment").IsEmpty ? false : (bool)selfPay.Element("IsReconsiderationOfAdjustment")
                                                     }).FirstOrDefault()
                                      }).FirstOrDefault();

            financialSummary.EmploymentStatusID = copyOfFinancialSummary.EmploymentStatusID;
            financialSummary.LookingForWork = copyOfFinancialSummary.LookingForWork;
            financialSummary.FAStaffName = copyOfFinancialSummary.FAStaffName;
            financialSummary.FAStaffCredential = copyOfFinancialSummary.FAStaffCredential;
            financialSummary.FAStaffPhone = copyOfFinancialSummary.FAStaffPhone;
            financialSummary.FAStaffExtension = copyOfFinancialSummary.FAStaffExtension;
            financialSummary.FinancialAssessment = copyOfFinancialSummary.FinancialAssessment;
            financialSummary.FinancialAssessmentDetails = copyOfFinancialSummary.FinancialAssessmentDetails;
            financialSummary.Payors = copyOfFinancialSummary.Payors;
            financialSummary.SelfPay = copyOfFinancialSummary.SelfPay;

            return financialSummary;
        }

        #endregion Helpers
    }
}
