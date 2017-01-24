-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactAlias]
-- Author:		Kyle Campbell
-- Date:		03/11/2016
--
-- Purpose:		Update Contact Alias Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	Kyle Campbell	Initial Creation
-- 03/16/2016	Rajiv Ranjan	Corrected order of parameter
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/18/2016	Kyle Campbell	TFS #14753	Added proc call for change log
-- 09/22/2016	Gurpreet Singh	Check whether record exist as it is 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactAlias]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactAliasID BIGINT,
	@ContactID BIGINT,
	@AliasFirstName NVARCHAR(200),
	@AliasMiddle NVARCHAR(200),
	@AliasLastName NVARCHAR(200),
	@SuffixID INT,
	@IsActive BIT,	
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);			

	BEGIN TRY

	IF not Exists(Select  1 from Registration.ContactAlias Where ContactAliasID = @ContactAliasID AND  
			isnull(AliasFirstName,0) = isnull(@AliasFirstName,0) AND 
			isnull(AliasMiddle,0) = isnull(@AliasMiddle,0) AND 
			isnull(AliasLastName,0) = isnull(@AliasLastName,0) AND 
			isnull(SuffixID,0) = isnull(@SuffixID,0) AND 
			@IsActive=@IsActive)
		BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactAlias', @ContactAliasID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactAlias SET 
			AliasFirstName = @AliasFirstName,
			AliasMiddle = @AliasMiddle,
			AliasLastName = @AliasLastName,
			SuffixID = @SuffixID,
			@IsActive=@IsActive,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE ContactAliasID = @ContactAliasID

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactAlias', @AuditDetailID, @ContactAliasID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddContactAliasChangeLog @TransactionLogID, @ContactAliasID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
		END
	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
