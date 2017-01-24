
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetProgram]
-- Author:		John Crossen
-- Date:		08/24/2015
--
-- Purpose:		Gets the list of  Program lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/25/2015	John Crossen		TFS# 1487 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE [Reference].[usp_GetProgram]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[ProgramID], [ProgramName] 
		FROM		[Reference].[Program]
		WHERE		IsActive = 1
		ORDER BY	[ProgramName]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


