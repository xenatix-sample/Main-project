-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetProgramClientIdentifiers]
-- Author:		Vishal Joshi
-- Date:		01/06/2016
--
-- Purpose:		Gets the list of Program and requied client identifiers 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetProgramClientIdentifiers]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[ClientTypeID], [ClientIdentifierTypeID] 
		FROM		[Reference].[ProgramClientIdentifier]
		WHERE		IsActive = 1
		ORDER BY	[ClientTypeID]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO