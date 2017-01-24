-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_SetLoginData
-- Author:		Justin Spalti
-- Date:		07/23/2015
--
-- Purpose:		Updates a user's login related data after a successful or attempted login
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015 - Initial procedure creation
-- 07/23/2015 - Justin Spalti - Added dbo to the table name
-- 07/23/2015 - Justin Spalti - Added the ModifiedBy parameter and included the value in crud operations
-- 07/23/2015 - demetrios.christopher@xenatix.com - Added password hashing
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/05/2015 - Justin Spalti - Updated the where clause to properly compare the passwords since the parameter and the column will be encrypted.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, Added SystemModifiedOn field
-- 09/04/2016	Rahul Vats		This stored proc already takes care of setting the login data for Task# 14254
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SetLoginData]
	@UserID INT,
	@UserName NVARCHAR(100),
	@Password NVARCHAR(100),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Data set successfully';

	BEGIN TRY
	Declare @ADFlag bit
	Set @ADFlag = (SELECT ADFlag FROM Core.Users WHERE UserName = @UserName)
		If(@ADFlag = 1)
		BEGIN
			SET @Password = ''
		END
	IF @UserID < 0
		BEGIN
		SET @UserID = (SELECT UserID FROM Core.Users WHERE UserName = @UserName)
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Users', @UserID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.[Users]
		SET LoginAttempts = CASE WHEN LoginAttempts IS NULL THEN 1 ELSE LoginAttempts + 1 END,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			UserName = @UserName;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Users', @AuditDetailID, @UserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
	ELSE
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Users', @UserID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		UPDATE Core.[Users] SET 
			LoginAttempts = 0,
			LoginCount = CASE WHEN LoginCount IS NULL THEN 1 ELSE LoginCount + 1 END,
			LastLogin = GETUTCDATE(),
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE(),
			ModifiedBy = @ModifiedBy
		WHERE
			UserID = @UserID
			AND	ISNULL([Password],'') = @Password
			AND UserName = @UserName

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Users', @AuditDetailID, @UserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END