-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactAlias]
-- Author:		Kyle Campbell
-- Date:		03/11/2016
--
-- Purpose:		Add Contact Alias
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	Kyle Campbell	Initial Creation
-- 03/16/2016	Rajiv Ranjan	Corrected Order of Parameter - @ModifiedOn & @ModifiedBy
-- 09/16/2016	Kyle Campbell	TFS #14753	Added parameters and proc call for change log
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_AddContactAlias]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactID BIGINT,
	@AliasFirstName NVARCHAR(200),
	@AliasMiddle NVARCHAR(200),
	@AliasLastName NVARCHAR(200),
	@SuffixID INT,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

		BEGIN TRY
			INSERT INTO Registration.ContactAlias
			(
				ContactID,
				AliasFirstName,
				AliasMiddle,
				AliasLastName,
				SuffixID,
				IsActive,
				ModifiedBy,
				ModifiedOn,
				CreatedBy,
				CreatedOn
			)
			VALUES
			(
				@ContactID,
				@AliasFirstName,
				@AliasMiddle,
				@AliasLastName,
				@SuffixID,
				1,
				@ModifiedBy,
				@ModifiedOn,
				@ModifiedBy,
				@ModifiedOn
			);

			SELECT @ID = SCOPE_IDENTITY();

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactAlias', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactAlias', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddContactAliasChangeLog @TransactionLogID, @ID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
		END TRY

		BEGIN CATCH
			SELECT	@ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE();					
		END CATCH
END
