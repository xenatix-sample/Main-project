-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateReferralReferredToDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Update Referral 'Referred To' Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-- 12/18/2015   Satish Singh	Added ProgramID and ReferredDateTime, removed ID (not required)
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_UpdateReferralReferredToDetail]
	@ReferredToDetailID BIGINT,
	@ReferralHeaderID BIGINT,
	@OrganizationID INT,
	@ReferredDateTime DATETIME,
	@ActionTaken NVARCHAR(500),
	@Comments NVARCHAR(500),
	@UserID INT,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralReferredToDetails', @ReferredToDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE [Registration].[ReferralReferredToDetails]
	SET	[ReferralHeaderID] = @ReferralHeaderID,
		[ActionTaken] = @ActionTaken,
		[Comments] = @Comments,
		[UserID] = @UserID,
		[ModifiedBy] = @ModifiedBy,
		[OrganizationID] = @OrganizationID,
		[ReferredDateTime] = @ReferredDateTime,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferredToDetailID = @ReferredToDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralReferredToDetails', @AuditDetailID, @ReferredToDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


