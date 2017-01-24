-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteReferralConcernDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Delete Referral Concern details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu - Initial Creation
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteReferralConcernDetails]
	@ReferralConcernDetailID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT, 
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
			
	SELECT 	@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ReferralConcernDetails', @ReferralConcernDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ReferralConcernDetails]
	SET	    IsActive = 0,
			ModifiedBy = @ModifiedBy, 
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralConcernDetailID = @ReferralConcernDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ReferralConcernDetails', @AuditDetailID, @ReferralConcernDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 		
			 
	END TRY

	BEGIN CATCH
	SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
