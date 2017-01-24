-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetMonth]
-- Author:		Chad Roberts
-- Date:		03/14/2016
--
-- Purpose:		Gets the months in each year
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/14/2016	Chad Roberts		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

create PROCEDURE [Reference].[usp_GetMonth]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

	SELECT
		MonthID,
		Name
	FROM
		Reference.[Month]

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END