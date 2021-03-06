-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddReferralOutcomeDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Add Referral Outcome Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 03/08/2016	Kyle Campbell	TFS #7000	Removed AppointmentDate and AppointmentTime
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_AddReferralOutcomeDetail]
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
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Registration.ReferralAdditionalDetails WHERE ReferralHeaderID = @ReferralHeaderID;
	
	INSERT INTO [Registration].[ReferralOutcomeDetails]
	(
		[ReferralHeaderID],
		[FollowupExpected],
		[FollowupProviderID],
		[FollowupDate],
		[FollowupOutcome],
		[IsAppointmentNotified],
		[AppointmentNotificationMethod],
		[Comments],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ReferralHeaderID,
		@FollowupExpected,
		@FollowupProviderID,
		@FollowupDate,
		@FollowupOutcome,
		@IsAppointmentNotified,
		@AppointmentNotificationMethod,
		@Comments,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralOutcomeDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralOutcomeDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


