------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_DeleteFinancialAssessmentDetails]
-- Author:		Suresh Pandey
-- Date:		08/25/2015
--
-- Purpose:		Delete Financial Assessment Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/25/2015	Suresh Pandey	Initial creation.
-- 08/27/2015	Suresh Pandey	Added variable @AssessmentDate,@FinancialAssessmentID and @ContactID for Calling calculation logic SP after inactive.	 
--								Added select Result. if successfully inactive then return 1 else 0
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteFinancialAssessmentDetails]
	@FinancialAssessmentDetailsID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
	DECLARE @AssessmentDate DATE;
	DECLARE @FinancialAssessmentID BIGINT;
	DECLARE @ContactID BIGINT;	

	--To get the FinancialAssessmentID ,AssessmentDate ContactID and FamilySize for calculation sp
	SELECT
		@AssessmentDate = fa.AssessmentDate,
		@FinancialAssessmentID = fa.FinancialAssessmentID,
		@ContactID = fa.ContactID
	FROM
		[Registration].[FinancialAssessment] fa 
		INNER JOIN [Registration].[FinancialAssessmentDetails] fad
			ON fa.FinancialAssessmentID = fad.FinancialAssessmentID
	WHERE
		fad.FinancialAssessmentDetailsID = @FinancialAssessmentDetailsID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'FinancialAssessmentDetails', @FinancialAssessmentDetailsID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[FinancialAssessmentDetails]
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE 
		FinancialAssessmentDetailsID = @FinancialAssessmentDetailsID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'FinancialAssessmentDetails', @AuditDetailID, @FinancialAssessmentDetailsID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC [Registration].[usp_CalculateFinancialAssessmentTotals] @AssessmentDate, @FinancialAssessmentID, @ContactID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END