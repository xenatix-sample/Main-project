-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteReferralHeader]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Delete Referral Header information n Referral Requester Screen
--
-- Notes:		
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015  Sumana Sangapu - Initial Creation
-- 12/16/2015	Scott Martin	Added Audit Logging with Delete Reason
-- 12/16/2015	Gurpreet Singh	Corrected order of parameters
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteReferralHeader]
	@ReferralHeaderID	BIGINT,
	@ReasonForDelete NVARCHAR(MAX),
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ReferralHeader', @ReferralHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE 
		[Registration].[ReferralHeader]
	SET 
		IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE 
		ReferralHeaderID = @ReferralHeaderID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ReferralHeader', @AuditDetailID, @ReferralHeaderID, @ReasonForDelete, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END