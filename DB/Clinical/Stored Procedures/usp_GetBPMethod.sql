
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetBPMethod]
-- Author:		John Crossen
-- Date:		11/05/2015
--
-- Purpose:		Get list of BP Methods
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/05/2015	John Crossen	TFS# 2895 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetBPMethod]

@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

SELECT BPMethodID,
	   BPMethod
	   
 FROM Clinical.[BPMethod]
 WHERE IsActive=1

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END