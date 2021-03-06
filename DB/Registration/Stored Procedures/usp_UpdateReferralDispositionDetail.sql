-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateReferralDispositionDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Update Referral Disposition Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-- 12/17/2015   Gaurav Gupta    Remove @ID as output parameter as this is not required for update
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_UpdateReferralDispositionDetail]
	@ReferralDispositionDetailID BIGINT,
	@ReferralHeaderID BIGINT,
	@ReferralDispositionID INT,
	@ReasonForDenial NVARCHAR(500),
	@ReferralDispositionOutcomeID INT,
	@AdditionalNotes NVARCHAR(500),
	@UserID INT,
	@DispositionDate DATE,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralDispositionDetails', @ReferralDispositionDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE [Registration].[ReferralDispositionDetails]
	SET [ReferralHeaderID] = @ReferralHeaderID,
		[ReferralDispositionID] = @ReferralDispositionID,
		[ReasonforDenial] = @ReasonForDenial,
		[ReferralDispositionOutcomeID] = @ReferralDispositionOutcomeID,
		[AdditionalNotes] = @AdditionalNotes,
		[UserID] = @UserID,
		[DispositionDate] = @DispositionDate,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralDispositionDetailID = @ReferralDispositionDetailID;
	
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralDispositionDetails', @AuditDetailID, @ReferralDispositionDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


