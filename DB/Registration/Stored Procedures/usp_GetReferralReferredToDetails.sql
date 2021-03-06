-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetReferralReferredToDetails]
-- Author:		Scott Martin
-- Date:		12/15/2015
--
-- Purpose:		Get a list of referral 'referred to' details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015	Scott Martin	Initial creation.
-- 12/18/2015   Satish Singh	Added ReferredDateTime, ProgramID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetReferralReferredToDetails]
	@ReferralHeaderID BIGINT,
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
		[OrganizationID],
		[ReferredDateTime],
		[ActionTaken],
		[Comments],
		[UserID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Registration].[ReferralReferredToDetails]
	WHERE
		ReferralHeaderID = @ReferralHeaderID
		AND IsActive = 1;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END