
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetReferralDispositionsStatus]
-- Author:		Gaurav Gupta
-- Date:		01/04/2016
--
-- Purpose:		Gets the list of  disposition Status lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2016	Gaurav Gupta		TFS# N/A - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetReferralDispositionsStatus]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ReferralDispositionStatusID],
		[ReferralDispositionStatus],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Reference].[ReferralDispositionStatus]
	WHERE
		[IsActive] = 1
	ORDER BY
		[ReferralDispositionStatus]


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO

