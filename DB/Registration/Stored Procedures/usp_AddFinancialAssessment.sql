-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddFinancialAssessment]
-- Author:		Demetrios Christopher
-- Date:		09/25/2015
--
-- Purpose:		Add Financial Assessment in Finance Screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Name			Description
-- 09/25/2015   Demetrios Christopher - Initial Creation
/*
  
  exec   [Registration].[usp_AddFinancialAssessment] 1,'2015-09-02',1, ...

*/
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddFinancialAssessment]
	@ContactID BIGINT,
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
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT  @ID = 0,
		@ResultCode = 0,
		@ResultMessage = 'executed successfully';

	BEGIN TRY
	INSERT [Registration].[FinancialAssessment]
	(
		ContactID,
		AssessmentDate,
		TotalIncome,
		TotalExpenses,
		TotalExtraOrdinaryExpenses,
		TotalOther,
		AdjustedGrossIncome,
		FamilySize,
		ExpirationDate,
		ExpirationReasonID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@AssessmentDate,
		@TotalIncome,
		@TotalExpenses,
		@TotalExtraOrdinaryExpenses,
		@TotalOther,
		@AdjustedGrossIncome,
		@FamilySize,
		@ExpirationDate,
		@ExpirationReasonID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialAssessment', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialAssessment', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
