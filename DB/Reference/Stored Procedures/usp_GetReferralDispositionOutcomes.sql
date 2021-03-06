-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetReferralDispositionOutcomes]
-- Author:		Scott Martin
-- Date:		12/13/2015
--
-- Purpose:		Get list of Referral Disposition Outcomes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/13/2015	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetReferralDispositionOutcomes]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ReferralDispositionOutcomeID],
		[ReferralDispositionOutcome],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Reference].[ReferralDispositionOutcome]
	WHERE
		[IsActive] = 1
	ORDER BY
		[ReferralDispositionOutcome]


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END