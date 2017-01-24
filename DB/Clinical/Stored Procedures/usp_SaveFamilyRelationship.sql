-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_SaveFamilyRelationship]
-- Author:		Scott Martin
-- Date:		12/10/2015
--
-- Purpose:		Add Family Relationship Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/10/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement, Added CreatedBy and CreatedOn to Insert Statement
-- 02/22/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_SaveFamilyRelationship]
	@FamilyRelationshipXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

DECLARE @FR_ID TABLE (ID BIGINT);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	DECLARE @FamilyRelationship TABLE
	(
		[ContactID] BIGINT,
		[RelationshipTypeID] INT,
		[FirstName] NVARCHAR(200),
		[LastName] NVARCHAR(200),
		[IsDeceased] BIT,
		[IsInvolved] BIT
	);

	INSERT INTO @FamilyRelationship
    (
		[ContactID],
		[RelationshipTypeID],
		[FirstName],
		[LastName],
		[IsDeceased],
		[IsInvolved]
	)
	SELECT
		T.C.value('ContactID[1]','BIGINT'),
		T.C.value('RelationshipTypeID[1]','INT'),
		T.C.value('FirstName[1]','NVARCHAR(200)'),
		T.C.value('LastName[1]','NVARCHAR(200)'),
		T.C.value('IsDeceased[1]','BIT'),
		T.C.value('IsInvolved[1]','BIT')
	FROM 
		@FamilyRelationshipXML.nodes('Contact/FamilyRelationships') AS T(C);

	MERGE [Clinical].[FamilyRelationship] AS TARGET
	USING @FamilyRelationship AS SOURCE
		ON SOURCE.ContactID = TARGET.ContactID
		AND SOURCE.RelationshipTypeID = TARGET.RelationshipTypeID
		AND SOURCE.FirstName = TARGET.FirstName
		AND SOURCE.LastName = TARGET.LastName
		AND SOURCE.IsDeceased = TARGET.IsDeceased
		AND SOURCE.IsInvolved = TARGET.IsInvolved
		AND TARGET.IsActive = 1
	WHEN NOT MATCHED THEN
		INSERT
		(
			[ContactID],
			[RelationshipTypeID],
			[FirstName],
			[LastName],
			[IsDeceased],
			[IsInvolved],
			[IsActive],
			[ModifiedBy],
			[ModifiedOn],
			[CreatedBy],
			[CreatedOn]
		)
		VALUES
		(
			SOURCE.ContactID,
			SOURCE.RelationshipTypeID,
			SOURCE.FirstName,
			SOURCE.LastName,
			SOURCE.IsDeceased,
			SOURCE.IsInvolved,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)
	OUTPUT inserted.FamilyRelationshipID
	INTO @FR_ID;

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @FR_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'FamilyRelationship', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'FamilyRelationship', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO