-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateReferralForwardedDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Update Referral Outcome Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_UpdateReferralForwardedDetail]
	@ReferralForwardedDetailID BIGINT,
	@ReferralHeaderID BIGINT,
	@OrganizationID INT,
	@SendingReferralToID INT,
	@Comments NVARCHAR(500),
	@UserID INT,
	@ReferralSentDate DATE,	
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralForwardedDetails', @ReferralForwardedDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE [Registration].[ReferralForwardedDetails]
	SET	[ReferralHeaderID] = @ReferralHeaderID,
		[OrganizationID] = @OrganizationID,
		[SendingReferralToID] = @SendingReferralToID,
		[Comments] = @Comments,
		[UserID] = @UserID,
		[ReferralSentDate] = @ReferralSentDate,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralForwardedDetailID = @ReferralForwardedDetailID;
			
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralForwardedDetails', @AuditDetailID, @ReferralForwardedDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


