-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_DeleteUser
-- Author:		Justin Spalti
-- Date:		07/21/2015
--
-- Purpose:		Deactivates a user based on UserID
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015 - Renamed the original procedure named usp_RemoveUser to usp_DeleteUser
-- 07/23/2015 - Added dbo to the table
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 9/23/2015    John Crossen                Add Audit
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteUser]
	@UserID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	DECLARE @AuditDetailID BIGINT;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'Users', @UserID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.[Users] 
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		UserID = @UserID

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'Users', @AuditDetailID, @UserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END



