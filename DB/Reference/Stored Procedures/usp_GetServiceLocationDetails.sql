
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceLocationDetails]
-- Author:		Sumana Sangapu
-- Date:		01/18/2016
--
-- Purpose:		Gets the list of ServiceLocation lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	Sumana Sangapu	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceLocationDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ServiceLocationID, [ServiceLocation]  
		FROM		[Reference].[ServiceLocation] 
		WHERE		IsActive = 1
		ORDER BY	[ServiceLocation] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END