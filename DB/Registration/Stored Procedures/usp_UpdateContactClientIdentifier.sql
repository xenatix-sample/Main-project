-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateContactClientIdentifier]
-- Author:		Scott Martin
-- Date:		12/22/2015
--
-- Purpose:		Update Contact Alternate ID Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/22/2015	Scott Martin	Initial creation.
-- 03/09/2016	Kyle Campbell	TFS #6339 Added EffectiveDate and ExpirationDate fields
-- 07/27/2016	Deepak Kumar	Added ExpirationReasonID column wrt [Reference].[OtherIDExpirationReasons]
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/18/2016	Kyle Campbell	TFS #14753	Added proc call for change log
-- 10/26/2016	Arun Choudhary	Removed @ID parameter
-- 09/22/2016	Gurpreet Singh	Check whether record exist as it is 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactClientIdentifier]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactClientIdentifierID BIGINT,
	@ContactID BIGINT,
	@ClientIdentifierTypeID INT NULL,
	@AlternateID NVARCHAR(50),
	@ExpirationReasonID INT NULL,
	@EffectiveDate DATE NULL,
	@ExpirationDate DATE NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

		IF not Exists(Select  1 from [Registration].[ContactClientIdentifier] Where ContactClientIdentifierID = @ContactClientIdentifierID AND
			[ContactID] = @ContactID AND 
			isnull([ClientIdentifierTypeID],0) = isnull(@ClientIdentifierTypeID,0) AND 
			isnull([ExpirationReasonID],0) = isnull(@ExpirationReasonID,0) AND 
			isnull(EffectiveDate,'') = isnull(@EffectiveDate,'') AND 
			isnull(ExpirationDate,'') = isnull(@ExpirationDate,'') AND 
			isnull([AlternateID],0) = isnull(@AlternateID,0))
		BEGIN

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactClientIdentifier', @ContactClientIdentifierID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactClientIdentifier]
	SET	[ContactID] = @ContactID,
		[ClientIdentifierTypeID] = @ClientIdentifierTypeID,
		[ExpirationReasonID] = @ExpirationReasonID,
		EffectiveDate = @EffectiveDate,
		ExpirationDate = @ExpirationDate,
		[AlternateID] = @AlternateID,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactClientIdentifierID = @ContactClientIdentifierID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactClientIdentifier', @AuditDetailID, @ContactClientIdentifierID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddContactClientIdentifierChangeLog @TransactionLogID, @ContactClientIdentifierID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
		END
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


