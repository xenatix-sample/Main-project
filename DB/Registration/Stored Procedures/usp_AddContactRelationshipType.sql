-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddContactRelationshipType]
-- Author:		Lokesh Singhal
-- Date:		06/08/2016
--
-- Purpose:		Add Contact RelationshipType Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Lokesh Singhal	Initial creation.
-- 09/14/2016    Arun Choudhary	Added EffectiveDate and ExpirationDate
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactRelationshipType]
	@ParentContactID BIGINT,
	@ContactID BIGINT,
	@RelationshipTypeID INT,
	@IsPolicyHolder BIT,
	@OtherRelationship NVARCHAR (200),
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactRelationshipID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactRelationshipID = ContactRelationshipID FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID;

	INSERT INTO [Registration].[ContactRelationshipType]
	(
		[ContactRelationshipID],
		[RelationshipTypeID], 
		[IsPolicyHolder],
		[OtherRelationship],
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
		@ContactRelationshipID,
		@RelationshipTypeID,
		@IsPolicyHolder,
		@OtherRelationship,
		@EffectiveDate,
		@ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactRelationshipType', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactRelationshipType', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


