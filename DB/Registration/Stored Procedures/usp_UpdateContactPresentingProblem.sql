-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateContactPresentingProblem]
-- Author:		Scott Martin
-- Date:		03/28/2016
--
-- Purpose:		Update Contact Presenting Problem Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Scott Martin	Initial creation.
-- 03/31/2016	Scott Martin	Removed OrganizationID and replaced it with PresentingProblemTypeID
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/16/2016	Kyle Campbell	TFS #14793	Add Change Log proc call
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactPresentingProblem]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactPresentingProblemID BIGINT,
	@ContactID BIGINT,
	@PresentingProblemTypeID SMALLINT NULL,
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPresentingProblem', @ContactPresentingProblemID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	UPDATE [Registration].[ContactPresentingProblem]
	SET [ContactID] = @ContactID,
		[PresentingProblemTypeID] = @PresentingProblemTypeID,
		[EffectiveDate] = @EffectiveDate,
		[ExpirationDate] = @ExpirationDate,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		[SystemModifiedOn] = GETUTCDATE()
	WHERE
		ContactPresentingProblemID = @ContactPresentingProblemID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPresentingProblem', @AuditDetailID, @ContactPresentingProblemID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
	EXEC Auditing.usp_AddContactDemographicChangeLog @TransactionLogID, @ContactID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


