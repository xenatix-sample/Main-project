-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddSocialRelationshipDetail]
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
-- 12/02/2015	Gurpreet Singh	Removed SocialRelationshipDetailIDin query / Added ID output parameter
-- 12/3/2015	Scott Martin	Removed FamilyRelationshipID
-- 12/28/2015   Justin Spalti	Added logic to conditionally add family relationship records. Commented out the ID output parameter 
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert 
-- 02/02/2016   Lokesh Singhal  Passed ModifiedBy instead of ModifiedOn parametr as required in stored procedure
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddSocialRelationshipDetail]
	@SocialRelationshipXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AdditionalResult XML;

DECLARE @SRD_ID TABLE (ID BIGINT);
DECLARE @ID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	DECLARE @SocialRelationshipDetail TABLE
	(
		[SocialRelationshipID] BIGINT,
		[FamilyRelationshipID] BIGINT,
		[ContactID] BIGINT,
		[RelationshipTypeID] INT,
		[FirstName] NVARCHAR(200),
		[LastName] NVARCHAR(200),
		[IsDeceased] BIT,
		[IsInvolved] BIT
	);

	INSERT INTO @SocialRelationshipDetail
	(
		[SocialRelationshipID],
		[FamilyRelationshipID],
		[ContactID],
		[RelationshipTypeID],
		[FirstName],
		[LastName],
		[IsDeceased],
		[IsInvolved]
	)
	SELECT
		T.C.value('SocialRelationshipID[1]','BIGINT'),
		T.C.value('FamilyRelationshipID[1]','BIGINT'),
		T.C.value('ContactID[1]','BIGINT'),
		T.C.value('RelationshipTypeID[1]','INT'),
		T.C.value('FirstName[1]','NVARCHAR(200)'),
		T.C.value('LastName[1]','NVARCHAR(200)'),
		T.C.value('IsDeceased[1]','BIT'),
		T.C.value('IsInvolved[1]','BIT')
	FROM 
		@SocialRelationshipXML.nodes('SocialRelationship/SocialRelationshipDetails') AS T(C);

	SET @AdditionalResult = (SELECT ContactID, RelationshipTypeID, FirstName, LastName, IsDeceased, IsInvolved FROM @SocialRelationshipDetail WHERE ISNULL(FamilyRelationshipID, 0) = 0 FOR XML RAW ('FamilyRelationships'), ROOT ('Contact'), ELEMENTS);

	EXEC Clinical.usp_AddFamilyRelationship @AdditionalResult, @ModifiedOn, @ModifiedBy, @ResultCode, @ResultMessage;

	UPDATE SRD
	SET FamilyRelationshipID =  FR.FamilyRelationshipID
	FROM
		@SocialRelationshipDetail SRD
		INNER JOIN Clinical.FamilyRelationship FR
			ON SRD.ContactID = FR.ContactID
			AND SRD.RelationshipTypeID = FR.RelationshipTypeID
			AND SRD.FirstName = FR.FirstName
			AND SRD.LastName = FR.LastName
			AND SRD.IsDeceased = FR.IsDeceased
			AND SRD.IsInvolved = FR.IsInvolved
	WHERE
		SRD.ContactID = FR.ContactID
		AND SRD.RelationshipTypeID = FR.RelationshipTypeID
		AND SRD.FirstName = FR.FirstName
		AND SRD.LastName = FR.LastName
		AND SRD.IsDeceased = FR.IsDeceased
		AND SRD.IsInvolved = FR.IsInvolved
		AND FR.IsActive = 1
	
	INSERT INTO [Clinical].[SocialRelationshipDetail]
    (
		[SocialRelationshipID],
		[FamilyRelationshipID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn]
	)
	OUTPUT inserted.SocialRelationshipDetailID
	INTO @SRD_ID
	SELECT
		SocialRelationshipID,
		FamilyRelationshipID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		@SocialRelationshipDetail SRD;	

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @SRD_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'SocialRelationshipDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'SocialRelationshipDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE(),
			   @ID = 0;
	END CATCH
END
GO