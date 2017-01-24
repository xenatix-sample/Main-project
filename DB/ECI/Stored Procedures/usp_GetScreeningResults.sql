
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_GetScreeningResults]
-- Author:		Sumana Sangapu
-- Date:		10/07/2015
--
-- Purpose:		Lookup for Screening Results
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2015	Sumana Sangapu TFS# 2620 - Initial creation.
 -----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [ECI].[usp_GetScreeningResults]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = OBJECT_NAME(@@PROCID) + ' executed successfully'

	BEGIN TRY	
		SELECT		ScreeningResultsID as ScreeningResultID, ScreeningResult
		FROM		[ECI].[ScreeningResults]  
		WHERE		IsActive = 1
		ORDER BY	ScreeningResult  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END