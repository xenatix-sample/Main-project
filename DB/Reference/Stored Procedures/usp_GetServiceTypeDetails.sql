-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceTypeDetails]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Gets the list of ServiceType lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceTypeDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		ServiceTypeID,
		ServiceType  
	FROM
		[Reference].[ServiceType] 
	WHERE
		IsActive = 1
	ORDER BY
		[SortOrder]
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END