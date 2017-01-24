-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_UpdateUser
-- Author:		Justin Spalti
-- Date:		07/21/2015
--
-- Purpose:		This will update the user's recod as well as merging in their assigned roles
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015 - Added the procedure header
-- 07/23/2015 - Added dbo to the table name
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/11/2015 - Justin Spalti - Updated the merge statement to correct a bug that began after upgrading to sql 2014
-- 09/02/2015 - Justin Spalti - Added additional parameters to handle the updating of the email field on the user management screen
-- 09/11/2015 - Justin Spalti - Updated the manner in which the expirationdate is being saved to the DB
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field, Added CreatedBy and CreatedOn fields
-- 02/09/2016 - Justin Spalti - Removed the roles and credentials parameters since they will be added on a different screen
-- 02/24/2016 - Justin Spalti - Added the new audit logging logic
-- 02/24/2016 - Justin Spalti - Added the GenderID parameter
-- 04/11/2016 - Sumana Sangapu	- Added IsInternal parameter
-- 06/08/2016 - Sumana Sangapu - Added GUID parameter as NULL
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUser]
	@UserID INT,
	@UserName NVARCHAR(100),
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@MiddleName NVARCHAR(50),
	@GenderID INT,
	@IsActive BIT,
	@EffectiveFromDate DATETIME,
	@EffectiveToDate DATETIME,
	@ResetLoginAttempts BIT,
	@PrimaryEmail NVARCHAR(50),
	@EmailID BIGINT,
	@IsInternal BIT = NULL,
	@UserGUID nvarchar(500) = NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT


AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'User updated successfully'
		   --<RolesXMLValue><RoleID>1</RoleID><RoleID>2</RoleID></RolesXMLValue>

	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		    @AuditDetailID BIGINT,
			@EmailXML XML

	BEGIN TRY			
		--Update user data before handling the roles
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Users', @UserID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.[Users] 
		SET UserName = @UserName,
			FirstName = @FirstName,
			LastName = @LastName,
			MiddleName = @MiddleName,
			GenderID = @GenderID,
			IsActive = @IsActive,
			EffectiveFromDate = @EffectiveFromDate,
			EffectiveToDate = @EffectiveToDate,
			LoginAttempts = CASE WHEN @ResetLoginAttempts = 1 THEN 0 ELSE LoginAttempts END,
			IsInternal = @IsInternal,
			UserGUID = @UserGUID,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE UserID = @UserID

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Users', @AuditDetailID, @UserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SET @EmailXML =
	(
		SELECT
			UE.UserEmailID,
			E.EmailID,
			@PrimaryEmail AS Email,
			UE.EmailPermissionID,
			UE.IsPrimary,
			1 AS IsActive,
			@ModifiedOn AS ModifiedOn
		FROM
			Core.Email E
			INNER JOIN Core.UserEmail UE
				ON E.EmailID = UE.EmailID
		WHERE
			UE.UserID = @UserID
			AND UE.EmailID = @EmailID
		FOR XML PATH ('Email'), ROOT ('RequestXMLValue')
	);

	EXEC Core.usp_UpdateUserEmails @EmailXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END