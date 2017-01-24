-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_UpdateUserStatus
-- Author:		Justin Spalti
-- Date:		07/22/2015
--
-- Purpose:		Activates a user based on UserID
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015 - Initial creation
-- 07/22/2015 - Updated the procedure name to match our DB standards
-- 07/23/2015 - Added dbo to the table name
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 9/23/2015    John Crossen                Add Audit
-- 01/22/2016   Lokesh Singhal Removed encrypted password to be stored in auditdetail as was creating issue during setting in @auditpre as XML 
-- 02/17/2016	Scott Martin		Refactored for audit loggin
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserStatus]
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
	SET IsActive = 1,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		UserID = @UserID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Users', @AuditDetailID, @UserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END



