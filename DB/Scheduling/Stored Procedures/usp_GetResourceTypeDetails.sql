-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetResourceTypeDetails]
-- Author:		John Crossen
-- Date:		09/11/2015
--
-- Purpose:		Gets the list of Resource Type lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2271 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetResourceTypeDetails]
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT	ResourceTypeID, ResourceType
		FROM [Scheduling].[ResourceType]  
		WHERE  IsActive = 1
		ORDER BY ResourceType ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


