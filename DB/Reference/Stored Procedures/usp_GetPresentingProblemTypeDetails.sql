-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPresentingProblemTypeDetails]
-- Author:		Scott Martin
-- Date:		03/31/2016
--
-- Purpose:		Gets the list of ServiceType lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/31/2016	Scott Martin	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetPresentingProblemTypeDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		PresentingProblemTypeID,
		PresentingProblemType  
	FROM
		[Reference].[PresentingProblemType] 
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