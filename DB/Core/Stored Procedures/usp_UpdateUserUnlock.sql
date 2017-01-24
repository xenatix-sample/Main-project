-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_UpdateUserUnlock
-- Author:		Justin Spalti
-- Date:		07/22/2015
--
-- Purpose:		Resets a user's login attempts to zero
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015 - Initial creation
-- 07/22/2015 - Renamed the procedure to follow our DB standards
-- 07/23/2015 - Added dbo to the table name
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn fields
-- 01/27/2016   Lokesh Singhal Removed encrypted password to be stored in auditdetail as was creating issue during setting in @auditpre as XML 
-- 02/17/2016	Scott Martin		Refactored for audit loggin
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserUnlock]
	@UserID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);	

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Users', @UserID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.[Users]
	SET LoginAttempts = 0,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		UserID = @UserID

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Users', @AuditDetailID, @UserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END