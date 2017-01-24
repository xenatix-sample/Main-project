-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetReferralDispositionDetails]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Get a list of referral disposition details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/16/2015	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_GetReferralDispositionDetails]
	@ReferralHeaderID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ReferralDispositionDetailID],
		[ReferralHeaderID],
		[ReferralDispositionID],
		[ReasonforDenial],
		[ReferralDispositionOutcomeID],
		[AdditionalNotes],
		[UserID],
		[DispositionDate],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Registration].[ReferralDispositionDetails]
	WHERE
		ReferralHeaderID = @ReferralHeaderID
		AND IsActive = 1;

	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END