-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateReferralContact]
-- Author:		John Crossen
-- Date:		10/09/2015
--
-- Purpose:		To update referral information
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/09/2015    TFS#2670        Created by John Crossen
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.usp_UpdateReferralContact
	@ReferralContactID BIGINT,
	@ReferralID BIGINT,
	@ContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SET NOCOUNT ON;
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralContact', @ReferralContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ReferralContact
	SET ReferralID = @ReferralID,
		ContactID = @ContactID,
		IsActive = 1,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralContactID = @ReferralContactID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralContact', @AuditDetailID, @ReferralContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


