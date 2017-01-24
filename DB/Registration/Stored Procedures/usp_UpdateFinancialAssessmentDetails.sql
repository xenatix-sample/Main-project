-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateFinancialAssessmentDetails]
-- Author:		Sumana Sangapu
-- Date:		08/07/2015
--
-- Purpose:		Add Financial Assessment Details in Finance Screen
--
-- Notes:		Update the Financial Assessment Details based on the AssessmentID, AssessmentDate
--				FinancialAssessment table and retrieve the AssessmentID
--
-- Depends:		Registration.Contact
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/07/2015   Sumana Sangapu	634 - Initial Creation
-- 08/24/2015   Suresh Pandey	Added variable @AssessmentDate,@FinancialAssessmentID and @ContactID for Calling calculation logic SP after update.				
--								Added @Family to update family size if family size is changed
-- 08/30/2015	Suresh Pandey	Adding @TotalIncome ,@TotalExpenses,@AdjustedGrossIncome as parameter to accept value from UI.
-- 09/21/2015	Suresh Pandey	remove parameter and change update condition  
-- 09/25/2015	D. Christopher	Separated CRUD between Assessment and Assessment Details
--
/*
 exec   [Registration].[usp_UpdateFinancialAssessmentDetails] 2,'200.00',2,'01/08/2015',2,0,'08/06/2015',1,1,'',''
 exec   [Registration].[usp_UpdateFinancialAssessmentDetails] 3,'100.00',2,'01/08/2015',2,0,'08/06/2015',1,1,'',''

*/
-- 12/16/2015	Scott Martin	Added audit logging
-- 12/23/2015	Scott Martin	Added RelationshipTypeID
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateFinancialAssessmentDetails]
	@FinancialAssessmentDetailID BIGINT,
	@CategoryTypeID INT,
	@Amount DECIMAL (15, 2) = 0.00,
	@FinanceFrequencyID INT,
	@CategoryID INT,
	@RelationshipTypeID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;
						
	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Registration.FinancialAssessment FA INNER JOIN Registration.FinancialAssessmentDetails FAD ON FA.FinancialAssessmentID = FAD.FinancialAssessmentDetailsID WHERE FAD.FinancialAssessmentDetailsID = @FinancialAssessmentDetailID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'FinancialAssessmentDetails', @FinancialAssessmentDetailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[FinancialAssessmentDetails]
	SET Amount = @Amount,
		FinanceFrequencyID = @FinanceFrequencyID,
		CategoryID = @CategoryID,
		CategoryTypeID = @CategoryTypeID,
		RelationshipTypeID = @RelationshipTypeID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		FinancialAssessmentDetailsID = @FinancialAssessmentDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'FinancialAssessmentDetails', @AuditDetailID, @FinancialAssessmentDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END