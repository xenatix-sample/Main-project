-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetReferralReferredToDetail]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Get a specific referral 'referred to' detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.[usp_GetReferralReferredToDetail]
	@ReferredToDetailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ReferredToDetailID],
		[ReferralHeaderID],
		[ActionTaken],
		[Comments],
		[UserID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Registration].[ReferralReferredToDetails]
	WHERE
		ReferredToDetailID = @ReferredToDetailID
		AND IsActive = 1;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


