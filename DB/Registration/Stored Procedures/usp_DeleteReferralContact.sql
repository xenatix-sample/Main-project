-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteReferralContact]
-- Author:		John Crossen
-- Date:		10/09/2015
--
-- Purpose:		To remove referral contact
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/09/2015    TFS#2670        Created by John Crossen
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE usp_DeleteReferralContact
	@ReferralContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ReferralContact', @ReferralContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ReferralContact]
	SET [IsActive] = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralContactID = @ReferralContactID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ReferralContact', @AuditDetailID, @ReferralContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
	
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
