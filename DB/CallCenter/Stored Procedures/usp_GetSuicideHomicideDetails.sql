
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSuicideHomicideDetails]
-- Author:		Sumana Sangapu
-- Date:		01/18/2016
--
-- Purpose:		Gets the list of SuicideHomicide lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2016	Sumana Sangapu	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetSuicideHomicideDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		SuicideHomicideID, [SuicideHomicide]  
		FROM		[CallCenter].[SuicideHomicide] 
		WHERE		IsActive = 1
		ORDER BY	[SuicideHomicide] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END