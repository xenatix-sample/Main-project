
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Scheduling].[usp_GetCancelReason]
-- Author:		Satish Singh
-- Date:		02/11/2016
--
-- Purpose:		Gets the list of Cancel reasons
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetCancelReason]	
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			ReasonID,
			Reason
		FROM 
			Scheduling.CancelReason
	    WHERE 
			 ISNULL(IsActive,1) = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END