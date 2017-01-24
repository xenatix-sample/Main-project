
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_DeleteUserAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Delete User CoSignatures
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016 - Sumana Sangapu Initial Creation 
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteUserAdditionalDetails]
	@UserAdditionalDetailsID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@UserID INT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @UserID = UserID FROM Core.UserAdditionalDetails WHERE UserAdditionalDetailID = @UserAdditionalDetailsID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'UserAdditionalDetails', @UserAdditionalDetailsID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE  Core.UserAdditionalDetails
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		UserAdditionalDetailID = @UserAdditionalDetailsID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'UserAdditionalDetails', @AuditDetailID, @UserAdditionalDetailsID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END