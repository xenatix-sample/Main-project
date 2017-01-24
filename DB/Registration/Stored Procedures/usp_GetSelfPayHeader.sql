-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetSelfPayHeader]
-- Author:		Scott Martin
-- Date:		04/06/2016
--
-- Purpose:		Get Contact SelfPay Header Records
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetSelfPayHeader]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			SelfPayHeaderID,
			ContactID,
			ISChildInConservatorship,
			IsNotAttested,
			IsApplyingForPublicBenefits,
			IsEnrolledInPublicBenefits,
			IsRequestingReconsideration,
			IsNotGivingConsent,
			IsOtherChildEnrolled,
			IsReconsiderationOfAdjustment
		FROM 
			Registration.SelfPayHeader
		WHERE 
			ContactID = @ContactID
			AND IsActive = 1
	END TRY
	
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultCode = ERROR_MESSAGE()
	END CATCH
END
