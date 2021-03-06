
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetUserIdentifierType]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Gets the lookup values for UserIdentifierType
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Sumana Sangapu    Initial creation. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetUserIdentifierType]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		UserIdentifierTypeID, UserIdentifierType
		FROM		[Reference].[UserIdentifierType] 
		WHERE		IsActive = 1
		ORDER BY	UserIdentifierType ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END