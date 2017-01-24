-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateReferralConcernDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Update Referral Concern details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu - Initial Creation
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/06/2016  Satish Singh Removed AdditionalConcerns = @AdditionalConcerns,
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateReferralConcernDetails]
	@ReferralConcernDetailID BIGINT,
	@ReferralAdditionalDetailID BIGINT,
	@ReferralConcernID INT,
	@Diagnosis NVARCHAR(1000),
	@ReferralPriorityID INT,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralConcernDetails', @ReferralConcernDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	UPDATE [Registration].[ReferralConcernDetails]
	SET ReferralAdditionalDetailID = @ReferralAdditionalDetailID, 
		ReferralConcernID = @ReferralConcernID, 				
		Diagnosis = @Diagnosis, 
		ReferralPriorityID = @ReferralPriorityID, 
		IsActive = 1,
		ModifiedBy = @ModifiedBy, 
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralConcernDetailID = @ReferralConcernDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralConcernDetails', @AuditDetailID, @ReferralConcernDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
