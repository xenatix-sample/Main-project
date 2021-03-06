-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateReferralOutcomeDetail]
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
-- 03/08/2016	Kyle Campbell	TFS #7000	Removed AppointmentDate and AppointmentTime
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_UpdateReferralOutcomeDetail]
	@ReferralOutcomeDetailID BIGINT,
	@ReferralHeaderID BIGINT,
	@FollowupExpected BIT,
	@FollowupProviderID INT,
	@FollowupDate DATE,
	@FollowupOutcome NVARCHAR(500),
	@IsAppointmentNotified BIT,
	@AppointmentNotificationMethod NVARCHAR(100),
	@Comments NVARCHAR(500),
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralOutcomeDetails', @ReferralOutcomeDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE [Registration].[ReferralOutcomeDetails]
	SET [ReferralHeaderID] = @ReferralHeaderID,
		[FollowupExpected] = @FollowupExpected,
		[FollowupProviderID] = @FollowupProviderID,
		[FollowupDate] = @FollowupDate,
		[FollowupOutcome] = @FollowupOutcome,
		[IsAppointmentNotified] = @IsAppointmentNotified,
		[AppointmentNotificationMethod] = @AppointmentNotificationMethod,
		[Comments] = @Comments,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralOutcomeDetailID = @ReferralOutcomeDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralOutcomeDetails', @AuditDetailID, @ReferralOutcomeDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


