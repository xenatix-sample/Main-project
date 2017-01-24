-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateContactAllergyDetail]
-- Author:		John Crossen
-- Date:		11/13/2015
--
-- Purpose:		Update Contact Allergy
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	John Crossen	TFS# 3566 - Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 11/20/2015   Justin Spalti - Removed References to allergy symptoms
-- 11/23/2015   Justin Spalti - Updated the parameter list, added a merge statement to handle the new contactallergysymptomdetail records(temporarily commented the audit logic)
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-- 02/25/2016   Lokesh Singhal  Fixed issue 'Cannot insert the value NULL into column 'AllergySymptomID' while removing any saved symptom.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateContactAllergyDetail]
	@AllergySymptomXML XML,
	@ContactAllergyDetailID BIGINT,
	@AllergyID INT,
	@SeverityID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@AllergySymptomID BIGINT,
		@AuditDetailID BIGINT,
		@AuditCursor CURSOR,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed successfully'

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'ContactAllergyDetail', @ContactAllergyDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE cca
	SET AllergyID = @AllergyID,
		SeverityID = @SeverityID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Clinical.ContactAllergyDetail cca
	WHERE
		cca.ContactAllergyDetailID = @ContactAllergyDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'ContactAllergyDetail', @AuditDetailID, @ContactAllergyDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	DECLARE @CADS TABLE
	(
		ContactAllergyDetailSymptomID BIGINT,
		AllergySymptomID BIGINT,
		ContactAllergyDetailID BIGINT,
		AuditDetailID BIGINT
	);

	INSERT INTO @CADS
	SELECT
		CADS.ContactAllergyDetailSymptomID,
		T.C.value('(.)[1]','int') AS AllergySymptomID,
		CADS.ContactAllergyDetailID,
		NULL
	FROM
		@AllergySymptomXML.nodes('SymptomDetails/AllergySymptomID') AS T(C)
		FULL OUTER JOIN (SELECT * FROM Clinical.ContactAllergyDetailSymptoms WHERE ContactAllergyDetailID = @ContactAllergyDetailID) AS CADS
			ON T.C.value('(.)[1]','int') = CADS.AllergySymptomID;

	IF EXISTS (SELECT TOP 1 * FROM @CADS WHERE ContactAllergyDetailSymptomID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ContactAllergyDetailSymptomID, AllergySymptomID FROM @CADS WHERE ContactAllergyDetailSymptomID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AllergySymptomID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @AllergySymptomID IS NOT NULL
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'ContactAllergyDetailSymptoms', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Clinical', 'ContactAllergyDetailSymptoms', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		UPDATE @CADS
		SET AuditDetailID = @AuditDetailID
		WHERE
			ContactAllergyDetailSymptomID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AllergySymptomID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	DECLARE @CADS_ID TABLE
	(
		Operation NVARCHAR(12),
		ID BIGINT,
		ModifiedOn DATETIME
	);

	MERGE [Clinical].[ContactAllergyDetailSymptoms] AS TARGET
	USING (SELECT * FROM @CADS WHERE AllergySymptomID IS NOT NULL) AS SOURCE
		ON  SOURCE.AllergySymptomID = TARGET.AllergySymptomID
		AND TARGET.ContactAllergyDetailID = @ContactAllergyDetailID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			ContactAllergyDetailID,
			AllergySymptomID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ContactAllergyDetailID,
			SOURCE.AllergySymptomID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)
	WHEN NOT MATCHED BY SOURCE AND TARGET.ContactAllergyDetailID = @ContactAllergyDetailID THEN
		UPDATE
		SET TARGET.IsActive = 0,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = @ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	WHEN MATCHED THEN
		UPDATE
		SET TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = @ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.ContactAllergyDetailSymptomID,
		inserted.ModifiedOn
	INTO
		@CADS_ID;	

	IF EXISTS (SELECT TOP 1 * FROM @CADS_ID WHERE Operation = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @CADS_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'ContactAllergyDetailSymptoms', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'ContactAllergyDetailSymptoms', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @CADS WHERE ContactAllergyDetailSymptomID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ContactAllergyDetailSymptomID, AllergySymptomID, AuditDetailID FROM @CADS WHERE ContactAllergyDetailSymptomID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AllergySymptomID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @AllergySymptomID IS NOT NULL
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'ContactAllergyDetailSymptoms', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Clinical', 'ContactAllergyDetailSymptoms', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END		

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AllergySymptomID, @AuditDetailID
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