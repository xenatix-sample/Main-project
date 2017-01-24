-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactAlias]
-- Author:		Kyle Campbell
-- Date:		03/11/2016
--
-- Purpose:		Inactivate Contact Alias Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	Kyle Campbell	Initial Creation
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/18/2016	Kyle Campbell	TFS #14753	Added proc call for change log
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_DeleteContactAlias]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactAliasID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ContactID BIGINT;			

	BEGIN TRY
		SELECT @ContactID = ContactID FROM Registration.ContactAlias WHERE ContactAliasID = @ContactAliasID;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactAlias', @ContactAliasID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactAlias SET 
			IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE ContactAliasID = @ContactAliasID

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactAlias', @AuditDetailID, @ContactAliasID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddContactAliasChangeLog @TransactionLogID, @ContactAliasID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END