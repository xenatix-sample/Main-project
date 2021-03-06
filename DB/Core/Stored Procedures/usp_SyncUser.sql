-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_SyncUser
-- Author:		Justin Spalti
-- Date:		07/23/2015
--
-- Purpose:		This will be used to sync an AD user into our users table
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015 - Initial procedure creation
-- 07/23/2015 - Justin Spalti - Added dbo to the table name
-- 07/23/2015 - Justin Spalti - Added the ModifiedBy parameter and included the value in crud operations
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 09/22/2015   John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 01/27/2016   Lokesh Singhal Removed encrypted password to be stored in auditdetail as was creating issue during setting in @auditpre as XML
-- 02/17/2016	Scott Martin		Refactored for audit loggin
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SyncUser]
	@UserName VARCHAR(100),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Data sync executed successfully'

	BEGIN TRY	
	IF NOT EXISTS (SELECT u.UserID FROM Core.[Users] u WHERE u.UserName = @UserName)
		BEGIN
		INSERT INTO Core.[Users]
		(
			UserName,
			ModifiedOn,
			ModifiedBy,
			IsActive,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@UserName,
			@ModifiedOn,
			@ModifiedBy,
			1,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @ID = SCOPE_IDENTITY();

		DECLARE @AuditDetailID BIGINT;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Users', @ID, NULL, NULL, NULL, @ID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Users', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		END
	ELSE
	BEGIN
		RAISERROR('Failed to sync user with that username.', 16, 1)
	END		
		
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

