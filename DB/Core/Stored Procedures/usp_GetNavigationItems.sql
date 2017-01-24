-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetNavigationItems]
-- Author:		Justin Spalti
-- Date:		10/01/2015
--
-- Purpose:		Gets all data needed to populate Items on the aXis navigation menus
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.Users, Core.UserSecurityQuestions
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	Justin Spalti - Initial creation
-- 10/02/2015   Justin Spalti - Added the IsTemporaryPassword column to the case statement to determine if the profile has been completed
-- 11/26/2015	Gurpreet Singh - Added UserID, UserName columns
-- 12/17/2015   Satish Singh   - Added Phone Number
-- 04/12/2016	Sumana Sangapu	- Return Decrypted DigitalPassword
-- 04/21/2016   Rajiv Ranjan  - Added UserPhoneID and CredentialID
-- 06/14/2016	Scott Martin	Refactored the return query to order the list based on assigned permissions
-- 08/24/2016	Vishal Yadav  - Added Extension as well for return
-- 11/09/2016   Atul Chauhan  - Made Changes to get Digital Signature
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetNavigationItems]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

		BEGIN TRY

			EXEC Core.usp_OpenEncryptionKeys @ResultCode OUTPUT, @ResultMessage OUTPUT

			;WITH RoleCount (RoleID, PermissionCount)
			AS
			(
				SELECT
					R.RoleID,
					COUNT(*)
				FROM
					Core.Role R
					INNER JOIN Core.RoleModule RM
						ON R.RoleID = RM.RoleID
						AND RM.IsActive = 1
					INNER JOIN Core.RoleModuleComponent RMC
						ON RM.RoleModuleID = RMC.RoleModuleID
						AND RMC.IsActive = 1
					INNER JOIN Core.RoleModuleComponentPermission RMCP
						ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
						AND RMCP.IsActive = 1
						AND RMCP.PermissionLevelID IS NOT NULL
				GROUP BY
					R.RoleID
			)
			SELECT TOP 1
				   u.FirstName + ' ' + u.LastName AS UserFullName, 
				   ISNULL(r.Name, 'Administrator') AS UserRolePrimary,
				   u.UserID,
				   u.UserName,
				   CASE WHEN u.IsTemporaryPassword = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsProfileComplete,
				   CASE WHEN usq.UserSecurityQuestionID IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsSecurityQuestionComplete,
				   pn.Number as ContactNumber,
				   pn.PhoneID as ContactNumberID,
				   pn.Extension,
				   up.UserPhoneID as UserPhoneID,
				   uc.CredentialID as CredentialID,
				   photo.ThumbnailBLOB,
				   CONVERT(NVARCHAR(100), Core.fn_Decrypt(u.[DigitalPassword])) as DigitalPassword,
				   u.PrintSignature
			FROM Core.Users u
			INNER JOIN Core.UserRole ur
				ON ur.UserID = u.UserID
			LEFT OUTER JOIN Core.[Role] r
				ON r.RoleID = ur.RoleID
			LEFT OUTER JOIN Core.UserSecurityQuestion usq
				ON usq.UserID = u.UserID
			LEFT OUTER JOIN Core.UserPhone up
				on up.UserID=@UserID
				AND up.IsPrimary=1
				AND up.IsActive=1
			LEFT OUTER JOIN Core.Phone pn
				on pn.PhoneID=up.PhoneID 
			LEFT OUTER JOIN Core.UserCredential uc
				on uc.UserID=@UserID
				AND uc.IsActive=1
			LEFT OUTER JOIN Core.UserPhoto userphoto
				on userphoto.UserID = u.UserID
				AND userphoto.IsPrimary = 1
				AND userphoto.IsActive=1
			LEFT OUTER JOIN Core.Photo photo
				on photo.PhotoID = userphoto.PhotoID
			LEFT OUTER JOIN RoleCount RC
				ON ur.RoleID = RC.RoleID
			WHERE u.UserID = @UserID
				AND r.IsActive = 1
				AND ur.IsActive = 1
			ORDER BY
				PermissionCount DESC
		END TRY
		BEGIN CATCH
			SELECT @ResultCode = ERROR_SEVERITY(),
				   @ResultMessage = ERROR_MESSAGE()
		END CATCH
END
