----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserPhotoThumbnails]
-- Author:		Scott Martin
-- Date:		02/25/2016
--
-- Purpose:		Gets a list of User photo thumbnail data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/25/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserPhotoThumbnails]
	@UserID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			CP.UserPhotoID,
			CP.UserID,
			P.PhotoID,
			P.ThumbnailBLOB,
			CP.IsPrimary,
			P.TakenBy,
			P.TakenTime,
			CP.ModifiedBy,
			CP.ModifiedOn
		FROM 
			Core.UserPhoto CP
			INNER JOIN Core.Photo P
				ON CP.PhotoID = P.PhotoID
		WHERE 
			CP.UserID = @UserID	
			AND P.IsActive = 1
			AND CP.IsActive = 1
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


