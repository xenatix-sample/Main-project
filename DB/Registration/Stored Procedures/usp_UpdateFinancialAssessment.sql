-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateFinancialAssessment]
-- Author:		Demetrios Christopher
-- Date:		09/25/2015
--
-- Purpose:		Update Financial Assessment in Finance Screen
--
-- Notes:		Update the Financial Assessment based on the AssessmentID
--
-- Depends:		Registration.Contact
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/25/2015   Demetrios Christopher - Initial Creation
--
/*
 exec   [Registration].[usp_UpdateFinancialAssessment] 2,2,'01/08/2015',1,1,'',''
 exec   [Registration].[usp_UpdateFinancialAssessment] 2,3,'01/08/2015',1,1,'',''

*/
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/27/2016	Scott Martin	Reordered ModifiedOn/By params
-- 11/23/2016	Gaurav Gupta	Update Assessment date
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateFinancialAssessment]
	@FinancialAssessmentID BIGINT,
	@AssessmentDate DATE,
	@TotalIncome DECIMAL(15,2),
	@TotalExpenses DECIMAL(15,2),
	@TotalExtraOrdinaryExpenses DECIMAL(15,2),
	@TotalOther DECIMAL(15,2),
	@AdjustedGrossIncome DECIMAL(15,2),
	@FamilySize TINYINT,
	@ExpirationDate DATE,
	@ExpirationReasonID INT,
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
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Registration.FinancialAssessment WHERE FinancialAssessmentID = @FinancialAssessmentID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'FinancialAssessment', @FinancialAssessmentID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE [Registration].[FinancialAssessment]
	SET	FamilySize = @FamilySize,
	    AssessmentDate=@AssessmentDate,
		ExpirationDate = @ExpirationDate,
		ExpirationReasonID=@ExpirationReasonID,
		TotalIncome = @TotalIncome,
		TotalExpenses = @TotalExpenses,
		TotalExtraOrdinaryExpenses = @TotalExtraOrdinaryExpenses,
		TotalOther = @TotalOther,
		AdjustedGrossIncome = @AdjustedGrossIncome,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		FinancialAssessmentID = @FinancialAssessmentID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'FinancialAssessment', @AuditDetailID, @FinancialAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END

