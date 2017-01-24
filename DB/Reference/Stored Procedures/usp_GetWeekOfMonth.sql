-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetWeekOfMonth]
-- Author:		Chad Roberts
-- Date:		03/14/2016
--
-- Purpose:		Gets the weeks that are in any given month
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/14/2016	Chad Roberts		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetWeekOfMonth]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

	SELECT
		WeekOfMonthID,
		Name
	FROM
		Reference.[WeekOfMonth]

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END