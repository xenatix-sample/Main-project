-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserByID
-- Author:		Justin Spalti
-- Date:		07/21/2015
--
-- Purpose:		Gets a list of users based on various search criteria
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/09/2016 - Justin Spalti - Initial Creation
-- 02/16/2016 - Justin Spalti - Removed IsActive from the where clause since an inactive user may be edited
-- 02/24/2016 - Justin Spalti - Added the GenderID column to the results set
-- 03/23/2016	Sumana Sangapu	Return PrintSignature.
-- 04/13/2016	Karl Jablonski	Added IsInternal field.
-- 06/08/2016	Sumana Sangapu	Return UserGUID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserByID]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Users retrieved successfully'

	BEGIN TRY	
		SELECT u.UserID, u.ADFlag, u.UserGUID, u.UserName, u.FirstName, u.LastName, u.MiddleName, u.GenderID,
			  u.IsActive, u.EffectiveFromDate, u.EffectiveToDate, u.LoginAttempts,
			  u.LoginCount, u.LastLogin, u.ModifiedOn, u.ModifiedBy,
			  e.EmailID, u.IsInternal,e.Email AS 'PrimaryEmail',
			  photo.ThumbnailBLOB, u.PrintSignature, u.UserGUID
		FROM Core.[Users] u
		JOIN Core.[UserEmail] ue
			ON ue.UserID = u.UserID
		JOIN Core.[Email] e
			ON e.EmailID = ue.EmailID
		LEFT OUTER JOIN Core.UserPhoto userphoto on userphoto.UserID = u.UserID AND userphoto.IsPrimary = 1 AND userphoto.IsActive=1
		LEFT OUTER JOIN Core.Photo photo on photo.PhotoID = userphoto.PhotoID
		WHERE u.UserID = @UserID
			AND ue.IsActive = 1
			AND ue.IsPrimary = 1
			AND e.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END