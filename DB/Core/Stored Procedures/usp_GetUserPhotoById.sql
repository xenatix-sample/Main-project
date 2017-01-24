----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserPhotoById]
-- Author:		Scott Martin
-- Date:		02/25/2016
--
-- Purpose:		Gets primary User photo data By Id
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/25/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserPhotoById]
	@UserPhotoID BIGINT,
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
			CP.PhotoID,
			CP.IsPrimary
		FROM 
			Core.UserPhoto CP
			INNER JOIN Core.Photo P
				ON CP.PhotoID = P.PhotoID
		WHERE 
			CP.UserPhotoID = @UserPhotoID			
			AND P.IsActive = 1
			AND CP.IsActive = 1
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


