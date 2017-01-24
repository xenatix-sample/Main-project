-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetEmail]
-- Author:		Saurabh Sahu
-- Date:		08/10/2015
--
-- Purpose:		Get Contact Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2015	Saurabh Sahu		Initial draft.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetEmail]
	@EmailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT 
			e.EmailID,
			e.Email,
			e.IsActive
		FROM 
			Core.Email e
		WHERE 
			e.EmailID = @EmailID AND e.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END