-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateSocialRelationshipDetail]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Update Social Relationship Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/02/2016   Lokesh Singhal  Pass parameter to procedure in correct order as required.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateSocialRelationshipDetail]
	@SocialRelationshipXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AdditionalResult XML,
		@ID BIGINT;

DECLARE @AD_ID TABLE (ID BIGINT);

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'Executed successfully';

		DECLARE @SocialRelationshipDetail TABLE
		(
		SocialRelationshipDetailID BIGINT,
		SocialRelationshipID BIGINT,
		FamilyRelationshipID BIGINT,
		ContactID BIGINT,
		RelationshipTypeID INT,
		FirstName NVARCHAR(200),
		LastName NVARCHAR(200),
		IsDeceased BIT,
		IsInvolved BIT,
		AuditDetailID BIGINT
		);

		INSERT INTO @SocialRelationshipDetail
		SELECT
			T.C.value('SocialRelationshipDetailID[1]','BIGINT'),
			T.C.value('SocialRelationshipID[1]','BIGINT'),
			T.C.value('FamilyRelationshipID[1]','BIGINT'),
			T.C.value('ContactID[1]','BIGINT'),
			T.C.value('RelationshipTypeID[1]','INT'),
			T.C.value('FirstName[1]','NVARCHAR(200)'),
			T.C.value('LastName[1]','NVARCHAR(200)'),
			T.C.value('IsDeceased[1]','BIT'),
		T.C.value('IsInvolved[1]','BIT'),
		NULL
		FROM 
			@SocialRelationshipXML.nodes('SocialRelationship/SocialRelationshipDetails') AS T(C);

		SET @AdditionalResult = (SELECT FamilyRelationshipID, ContactID, RelationshipTypeID, FirstName, LastName, IsDeceased, IsInvolved FROM @SocialRelationshipDetail FOR XML RAW ('FamilyRelationships'), ROOT ('Contact'), ELEMENTS);

		EXEC Clinical.usp_SaveFamilyRelationship @AdditionalResult, @ModifiedOn, @ModifiedBy, @ResultCode, @ResultMessage;

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

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT SocialRelationshipDetailID FROM @SocialRelationshipDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'SocialRelationshipDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE @SocialRelationshipDetail
		SET AuditDetailID = @AuditDetailID
		WHERE
			SocialRelationshipDetailID = @ID;
			
		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

		UPDATE SRD
		SET FamilyRelationshipID = DET.FamilyRelationshipID,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Clinical.SocialRelationshipDetail SRD
			JOIN @SocialRelationshipDetail DET
				ON SRD.SocialRelationshipDetailID = DET.SocialRelationshipDetailID
		WHERE
			SRD.SocialRelationshipDetailID = DET.SocialRelationshipDetailID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AuditDetailID, SocialRelationshipDetailID FROM @SocialRelationshipDetail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AuditDetailID, @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'SocialRelationshipDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

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