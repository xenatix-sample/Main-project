-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetReferralResourceTypes]
-- Author:		Scott Martin
-- Date:		12/13/2015
--
-- Purpose:		Get list of Referral Resource Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/13/2015	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetReferralResourceTypes]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ReferralResourceTypeID],
		[ReferralResourceType],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Reference].[ReferralResourceType]
	WHERE
		[IsActive] = 1
	ORDER BY
		[ReferralResourceType]


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END