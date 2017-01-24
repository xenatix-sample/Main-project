-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetTrackingFieldDetails]
-- Author:		Scott Martin
-- Date:		06/17/2016
--
-- Purpose:		Gets the list of TrackingField lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/17/2016	Scott Martin	- Initial creation.
-- 01/24/2017	Kyle Campbell	TFS #22100	Changed to sort alphabetically
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetTrackingFieldDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		TrackingFieldID,
		TrackingField  
	FROM
		[Reference].[TrackingField] 
	WHERE
		IsActive = 1
	ORDER BY
		[TrackingField]
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END