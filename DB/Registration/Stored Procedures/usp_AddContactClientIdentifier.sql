-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddContactClientIdentifier]
-- Author:		Scott Martin
-- Date:		12/22/2015
--
-- Purpose:		Add Contact Alternate ID Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/22/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 03/09/2016	Kyle Campbell	TFS #6339 Added EffectiveDate and ExpirationDate fields
-- 03/16/2016	Rajiv Ranjan	Added @EffectiveDate & @ExpirationDate
-- 07/27/2016	Deepak Kumar	Added ExpirationReasonID column wrt [Reference].[OtherIDExpirationReasons]
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/18/2016	Kyle Campbell	TFS #14753	Added proc call for change log
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactClientIdentifier]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactID BIGINT,
	@ClientIdentifierTypeID INT NULL,
	@AlternateID NVARCHAR(50),
	@ExpirationReasonID INT NULL,
	@EffectiveDate DATE NULL,
	@ExpirationDate DATE NULL,
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
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO [Registration].[ContactClientIdentifier]
	(
		[ContactID],
		[ClientIdentifierTypeID],
		[AlternateID],
		[ExpirationReasonID],
		[EffectiveDate],
		[ExpirationDate],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@ClientIdentifierTypeID,
		@AlternateID,
		@ExpirationReasonID,
		@EffectiveDate,
		@ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactClientIdentifier', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactClientIdentifier', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
	EXEC Auditing.usp_AddContactClientIdentifierChangeLog @TransactionLogID, @ID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


