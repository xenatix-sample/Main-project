-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddFamilyRelationship]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Add Social Relationship Detail Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 12/03/2015	Gurpreet Singh	Added parameters to update SocialRelationship and its Details
-- 12/03/2015	Gurpreet Singh	Removed parameters to update SocialRelationship and its Details
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddFamilyRelationship]
	@FamilyRelationshipXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

DECLARE @HPID_ID TABLE (ID BIGINT);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO [Clinical].[FamilyRelationship]
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
	OUTPUT inserted.FamilyRelationshipID
	INTO @HPID_ID
	SELECT
		T.C.value('ContactID[1]','BIGINT'),
		T.C.value('RelationshipTypeID[1]','BIGINT'),
		T.C.value('FirstName[1]','NVARCHAR(200)'),
		T.C.value('LastName[1]','NVARCHAR(200)'),
		T.C.value('IsDeceased[1]','BIT'),
		T.C.value('IsInvolved[1]','BIT'),
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM 
		@FamilyRelationshipXML.nodes('Contact/FamilyRelationships') AS T(C);

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @HPID_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'SurgicalHistoryDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'SurgicalHistoryDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO