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
-- 02/24/2016	Scott Martin		Moved from Registration to Core Schema
-- 11/17/2016	Rajiv Ranjan		Removed 'PhotoBLOB' to reduce http overhead(photoBLOB and ThumbnailBLOB both contains same data )
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetPhoto]
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
			ThumbnailBLOB,
			TakenBy,
			TakenTime,
			ModifiedBy,
			ModifiedOn
		FROM 
			Core.Photo P
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


