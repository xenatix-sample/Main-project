-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetReferralOutcomeDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Get a specific referral outcome
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-- 03/08/2016	Kyle Campbell	TFS #7000	Removed AppointmentDate and AppointmentTime
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_GetReferralOutcomeDetail]
	@ReferralOutcomeDetailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ReferralOutcomeDetailID],
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
		[ModifiedOn]
	FROM
		[Registration].[ReferralOutcomeDetails]
	WHERE
		ReferralOutcomeDetailID = @ReferralOutcomeDetailID
		AND IsActive = 1;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


