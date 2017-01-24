-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateFamilyRelationship]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Update Family Relationship Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateFamilyRelationship]
	@FamilyRelationshipXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed successfully'

	DECLARE @FamilyRelationship TABLE
	(
		FamilyRelationshipID BIGINT,
		RelationshipTypeID INT,
		FirstName NVARCHAR(200),
		LastName NVARCHAR(200),
		IsDeceased BIT,
		IsInvolved BIT,
		AuditDetailID BIGINT
	);

	INSERT INTO @FamilyRelationship
	SELECT
		T.C.value('FamilyRelationshipID[1]','BIGINT'),
		T.C.value('RelationshipTypeID[1]','BIGINT'),
		T.C.value('FirstName[1]','NVARCHAR(200)'),
		T.C.value('LastName[1]','NVARCHAR(200)'),
		T.C.value('IsDeceased[1]','BIT'),
		T.C.value('IsInvolved[1]','BIT'),
		NULL
	FROM
		@FamilyRelationshipXML.nodes('Contact/FamilyRelationships') as T(C);

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT FamilyRelationshipID FROM @FamilyRelationship;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'FamilyRelationship', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE @FamilyRelationship
		SET AuditDetailID = @AuditDetailID
		WHERE
			FamilyRelationshipID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE FR
	SET RelationshipTypeID = tFR.RelationshipTypeID,
		FirstName = tFR.FirstName,
		LastName = tFR.LastName,
		IsDeceased = tFR.IsDeceased,
		IsInvolved = tFR.IsInvolved,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Clinical.FamilyRelationship FR
		JOIN @FamilyRelationship tFR
			ON FR.FamilyRelationshipID = tFR.FamilyRelationshipID
	WHERE
		FR.FamilyRelationshipID = tFR.FamilyRelationshipID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AuditDetailID, FamilyRelationshipID FROM @FamilyRelationship;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'FamilyRelationship', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID
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