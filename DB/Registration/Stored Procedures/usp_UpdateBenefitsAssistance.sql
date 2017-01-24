-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateBenefitsAssistance]
-- Author:		Scott Martin
-- Date:		05/19/2016
--
-- Purpose:		Update Benefits Assistance
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Scott Martin	Initial creation.
-- 05/26/2016	Gurpreet Singh	Removed update for User Id as it will be only added not updated
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateBenefitsAssistance]
	@BenefitsAssistanceID BIGINT,
	@ContactID BIGINT,
	@DateEntered DATETIME,
	@UserID INT,
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@DocumentStatusID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'BenefitsAssistance', @BenefitsAssistanceID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[BenefitsAssistance]
	SET ContactID = @ContactID,
		DateEntered = @DateEntered,
		--UserID = @UserID,
		AssessmentID = @AssessmentID,
		ResponseID = @ResponseID,
		DocumentStatusID = @DocumentStatusID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		BenefitsAssistanceID = @BenefitsAssistanceID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'BenefitsAssistance', @AuditDetailID, @BenefitsAssistanceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO