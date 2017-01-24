-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_DeleteContactPresentingProblem]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Delete Contact Presenting Problem
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin	Initial creation.
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/16/2016	Kyle Campbell	TFS #14793	Add Change Log proc call
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactPresentingProblem]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactPresentingProblemID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Registration.ContactPresentingProblem WHERE ContactPresentingProblemID = @ContactPresentingProblemID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactPresentingProblem', @ContactPresentingProblemID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	UPDATE [Registration].[ContactPresentingProblem]
	SET IsActive = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		[SystemModifiedOn] = GETUTCDATE()
	WHERE
		ContactPresentingProblemID = @ContactPresentingProblemID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactPresentingProblem', @AuditDetailID, @ContactPresentingProblemID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
  	EXEC Auditing.usp_AddContactDemographicChangeLog @TransactionLogID, @ContactID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
 	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


