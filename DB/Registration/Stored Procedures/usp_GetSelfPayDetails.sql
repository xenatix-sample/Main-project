-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetSelfPayDetails]
-- Author:		Scott Martin
-- Date:		04/06/2016
--
-- Purpose:		Get Contact SelfPay Records
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetSelfPayDetails]
	@SelfPayID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			[SelfPayID],
			[ContactID],
			[OrganizationDetailID],
			[SelfPayAmount],
			[IsPercent], 	
			[EffectiveDate],
			[ExpirationDate],
			[IsChildInConservatorship],
			[IsNotAttested],
			[IsApplyingForPublicBenefits],
			[IsEnrolledInPublicBenefits],
			[IsRequestingReconsideration],
			[IsNotGivingConsent],
			[IsOtherChildEnrolled],
			[IsReconsiderationOfAdjustment]
		FROM 
			Registration.SelfPay
		WHERE 
			SelfPayID = @SelfPayID
			AND IsActive = 1
		ORDER BY EffectiveDate DESC
	END TRY
	
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultCode = ERROR_MESSAGE()
	END CATCH
END
