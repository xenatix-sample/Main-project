-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserProfile
-- Author:		Justin Spalti
-- Date:		09/28/2015
--
-- Purpose:		Gets all user profile related data
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/28/2015 - Initial proedure creation for use on the User Profile form
-- 10/02/2015 - Justin Spalti - Added the IsTemporaryPassword column to select statement
-- 03/01/2016 - Justin Spalti - Added additional columns needed for the new profile workflow.
-- 03/25/2016 - Karl Jablonski - Added PrintSignature field to result set
-- 03/30/2016 - Karl Jablonski - Added DigitalPassword field to result set
-- 07/21/2016	RAV - Reviewed The Query http://sqlmag.com/sql-server-2000/designing-performance-null-or-not-null
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserProfile]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	-- To-Do: Add auditing calls

	BEGIN TRY	
		declare @adUserPasswordResetMessage varchar(255)
		select @adUserPasswordResetMessage = MessageBody
		from Core.messagetemplate
		where MessageTemplateID = 4

		SELECT u.UserID, u.UserName, u.FirstName, u.LastName, u.MiddleName, u.UserGUID, u.IsActive,
			u.IsTemporaryPassword, u.ADFlag, u.PrintSignature, case when u.DigitalPassword IS NULL then '' else 'Password Set' end as CurrentDigitalPassword,
			--Check This Condition
			case when u.ADFlag = 0 then '' else @adUserPasswordResetMessage end as ADUserPasswordResetMessage,
			photo.ThumbnailBLOB
		FROM Core.Users u
		LEFT OUTER JOIN Core.UserPhoto userphoto on userphoto.UserID = u.UserID AND userphoto.IsPrimary = 1 AND userphoto.IsActive=1
		LEFT OUTER JOIN Core.Photo photo on photo.PhotoID = userphoto.PhotoID
		WHERE u.UserID = @UserID
		--	AND u.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END