-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteContactRelationship]
-- Author:		Gurpreet Singh
-- Date:		09/03/2015
--
-- Purpose:		Delete Contact relationship
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/03/2015	-	Gurpreet Singh	-	Initial Creation
-- 09/21/2015	-	Demetrios Christopher - Changed the lookup column from the ChildContactID to the more specific ContactRelationshipID
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 02/01/2016	Arun Choudhary  Because of offline and screen changes. We are sending Parent ContactID and child ContactID to delete contact relationship.
-- 11/24/2016	Vishal Yadav    Update the IsActive status in ContactRelationshipType table as well
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactRelationship]
	@ParentContactID BIGINT,
	@ContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@PrimaryKeyValue BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT TOP 1 @PrimaryKeyValue = ContactRelationshipID FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID;
	
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactRelationship', @PrimaryKeyValue, NULL, NULL, NULL, @ParentContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactRelationship]
	SET	[IsActive] = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactRelationshipID = @PrimaryKeyValue;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactRelationship', @AuditDetailID, @PrimaryKeyValue, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactRelationshipType', @PrimaryKeyValue, NULL, NULL, NULL, @ParentContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE Registration.ContactRelationshipType
	SET	[IsActive] = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactRelationshipID = @PrimaryKeyValue;
	
	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactRelationshipType', @AuditDetailID, @PrimaryKeyValue, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
