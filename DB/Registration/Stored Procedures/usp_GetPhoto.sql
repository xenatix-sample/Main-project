----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPhoto]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Gets Photo Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/29/2015	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetPhoto]
	@PhotoID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT 
			PhotoID,
			PhotoBLOB,
			ThumbnailBLOB,
			TakenBy,
			TakenTime,
			ModifiedBy,
			ModifiedOn
		FROM 
			Registration.Photo P
		WHERE 
			P.PhotoID = @PhotoID	
			AND P.IsActive = 1
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


