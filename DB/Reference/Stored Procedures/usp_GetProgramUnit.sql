
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetProgramUnit]
-- Author:		Gurpreet Singh
-- Date:		02/01/2016
--
-- Purpose:		Gets the list of Program Unit lookup  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/01/2016	Gurpreet Singh	Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetProgramUnit]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			ProgramUnitID, 
			ProgramUnit
		FROM [Reference].[ProgramUnit] 
		WHERE IsActive = 1
		ORDER BY ProgramUnit  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO

