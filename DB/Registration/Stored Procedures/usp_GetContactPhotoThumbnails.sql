----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactPhotoThumbnails]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Gets a list of contact photo thumbnail data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/29/2015	Scott Martin		Initial creation.
-- 01/07/2016	Rajiv Ranjan		Added IsActive check for ContactPhoto
-- 02/24/2016	Scott Martin		Moved Photo from Registration to Core Schema
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactPhotoThumbnails]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			CP.ContactPhotoID,
			CP.ContactID,
			P.PhotoID,
			P.ThumbnailBLOB,
			CP.IsPrimary,
			P.TakenBy,
			P.TakenTime,
			CP.ModifiedBy,
			CP.ModifiedOn
		FROM 
			Registration.ContactPhoto CP
			INNER JOIN Core.Photo P
				ON CP.PhotoID = P.PhotoID
		WHERE 
			CP.ContactID = @ContactID	
			AND P.IsActive = 1
			AND CP.IsActive = 1
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


