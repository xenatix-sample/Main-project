-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSchedulingOccur]
-- Author:		John Crossen
-- Date:		10/02/2015
--
-- Purpose:		Gets the list of SchedulingOccur
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2583 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.[usp_GetSchedulingOccur]
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT	SchedulingOccurID, SchedulingOccur
		FROM Scheduling.SchedulingOccur  
		WHERE  IsActive = 1 
		ORDER BY SchedulingOccur ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


