

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteCrisisLine]
-- Author:		John Crossen
-- Date:		01/27/2016
--
-- Purpose:		Delete for Crisis Line
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/27/2016	John Crossen   5714	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE CallCenter.usp_DeleteCrisisLine

@CallCenterHeaderID BIGINT,
@ModifiedBy INT,
@ModifiedOn DATETIME,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT

AS

BEGIN
DECLARE @AuditPost XML,
		@AuditID BIGINT;

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY

	UPDATE CallCenter.CallCenterHeader SET IsActive=0, ModifiedBy=@ModifiedBy, @ModifiedOn=@ModifiedOn 
	WHERE CallCenterHeaderID=@CallCenterHeaderID

	UPDATE CallCenter.CrisisCall SET IsActive=0 , ModifiedBy=@ModifiedBy, @ModifiedOn=@ModifiedOn 
	WHERE CallCenterHeaderID=@CallCenterHeaderID


	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

