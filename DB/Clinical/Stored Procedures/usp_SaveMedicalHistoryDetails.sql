-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_SaveMedicalHistoryDetails]
-- Author:		Scott Martin
-- Date:		12/3/2015
--
-- Purpose:		Saves/Updates the various records associated with a Medical History
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/3/2015	Scott Martin	Initial creation.
-- 12/8/2015	Scott Martin	Moved the Family Relationship node elements to the Medical History Condition Detail node
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement, Added CreatedBy and CreatedOn to Insert Statement
-- 02/22/2016	Scott Martin	Added Audit Logging and fixed several bugs (family relationship duplicates, family relationship details not saving correctly, inactivating records when HasCondition = 0)
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_SaveMedicalHistoryDetails]
	@MedicalHistoryXML XML,
	@ContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditCursor CURSOR,
		@AuditDetailID BIGINT,
		@docHandle int,
		@TempID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	DECLARE @MedicalHistory TABLE
	(	
		RowID BIGINT,
		MedicalHistoryID BIGINT,
		MedicalHistoryConditionID BIGINT,
		MedicalConditionID INT,
		HasCondition BIT,
		MedicalHistoryConditionDetailID BIGINT,
		IsSelf BIT,
		Comments NVARCHAR(2000),
		FamilyRelationshipID BIGINT,
		RelationshipTypeID INT,
		FirstName NVARCHAR(200),
		LastName NVARCHAR(200),
		IsDeceased BIT,
		IsActive BIT,
		MHCAuditDetailID BIGINT,
		MHCDAuditDetailID BIGINT
	);

	--Parse incoming XML data and store in temp table
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @MedicalHistoryXML;

	INSERT INTO @MedicalHistory
	SELECT
		ROW_NUMBER() OVER(ORDER BY MedicalHistoryID),
		MedicalHistoryID,
		ISNULL(MedicalHistoryConditionID, 0),
		MedicalConditionID,
		HasCondition,
		ISNULL(MedicalHistoryConditionDetailID, 0),
		ISNULL(IsSelf, 0),
		Comments,
		FamilyRelationshipID,
		RelationshipTypeID,
		ISNULL(FirstName, ''),
		ISNULL(LastName, ''),
		ISNULL(IsDeceased, 0),
		IsActive,
		NULL,
		NULL
	FROM OPENXML(@docHandle, N'/MedicalHistory/MedicalHistoryCondition/MedicalHistoryConditionDetail')
	WITH
	(
		MedicalHistoryID					BIGINT				'..//../MedicalHistoryID',
		MedicalHistoryConditionID			BIGINT				'../MedicalHistoryConditionID',
		MedicalConditionID					INT					'../MedicalConditionID',
		HasCondition						BIT					'../HasCondition',
		MedicalHistoryConditionDetailID		BIGINT				'MedicalHistoryConditionDetailID',
		IsSelf								BIT					'IsSelf',
		Comments							NVARCHAR(2000)		'Comments',
		FamilyRelationshipID				BIGINT				'FamilyRelationshipID',
		RelationshipTypeID					INT					'RelationshipTypeID',
		FirstName							NVARCHAR(200)		'FirstName',
		LastName							NVARCHAR(200)		'LastName',
		IsDeceased							BIT					'IsDeceased',
		IsActive							BIT					'IsActive'
	);

	--Create any necessary FamilyRelationship records
	DECLARE @FR_ID TABLE (ID BIGINT);

	MERGE [Clinical].[FamilyRelationship] AS TARGET
	USING (SELECT DISTINCT RelationshipTypeID, FirstName, LastName, IsDeceased, 0 AS IsInvolved FROM @MedicalHistory WHERE IsSelf = 0 AND RelationshipTypeID IS NOT NULL) AS SOURCE
		ON SOURCE.RelationshipTypeID IS NOT NULL
		AND SOURCE.RelationshipTypeID = TARGET.RelationshipTypeID
		AND SOURCE.FirstName = TARGET.FirstName
		AND SOURCE.LastName = TARGET.LastName
		AND SOURCE.IsDeceased = TARGET.IsDeceased
		AND SOURCE.IsInvolved = TARGET.IsInvolved
		AND TARGET.IsActive = 1
	WHEN NOT MATCHED THEN
		INSERT
		(
			ContactID,
			RelationshipTypeID,
			FirstName,
			LastName,
			IsDeceased,
			IsInvolved,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ContactID,
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

	--Create audit record of new FamilyRelationship records
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

	--Assign FamilyRelationshipID's to records in temp table
	UPDATE MH
	SET FamilyRelationshipID = FR.FamilyRelationshipID
	FROM
		@MedicalHistory MH
		INNER JOIN (SELECT FamilyRelationshipID, RelationshipTypeID, FirstName, LastName, IsDeceased, IsActive FROM Clinical.FamilyRelationship WHERE ContactID = @ContactID AND FamilyRelationshipID is not null) FR
			ON MH.RelationshipTypeID = FR.RelationshipTypeID
			AND MH.FirstName = FR.FirstName
			AND MH.LastName = FR.LastName
			AND MH.IsDeceased = FR.IsDeceased
	WHERE
		MH.RelationshipTypeID = FR.RelationshipTypeID
		AND MH.FirstName = FR.FirstName
		AND MH.LastName = FR.LastName
		AND MH.IsDeceased = FR.IsDeceased
		AND FR.IsActive = 1;

	--Assign existing MedicalHistoryConditionID's to records in temp table
	UPDATE MH
	SET MedicalHistoryConditionID = MHC.MedicalHistoryConditionID
	FROM
		@MedicalHistory MH
		INNER JOIN [Clinical].[MedicalHistoryCondition] MHC
			ON MH.MedicalHistoryID = MHC.MedicalHistoryID
			AND MH.MedicalConditionID = MHC.MedicalConditionID
	WHERE
		MH.MedicalHistoryID = MHC.MedicalHistoryID
		AND MH.MedicalConditionID = MHC.MedicalConditionID
		AND MH.MedicalHistoryConditionID = 0;

	--Inactivate any MedicalHistoryConditions which don't have the HasCondition flagged as true in the temp table. Create audit record.
	SET @AuditCursor = CURSOR FOR
	SELECT MedicalHistoryConditionID FROM Clinical.MedicalHistoryCondition WHERE MedicalHistoryID IN (SELECT DISTINCT MedicalHistoryID FROM @MedicalHistory) AND MedicalConditionID NOT IN (SELECT DISTINCT MedicalConditionID FROM @MedicalHistory WHERE HasCondition = 1) AND IsActive = 1;    

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Clinical', 'MedicalHistoryCondition', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.MedicalHistoryCondition
	SET HasCondition = 0,
		IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		MedicalHistoryConditionID = @ID
		AND IsActive = 1;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Clinical', 'MedicalHistoryCondition', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;
	
	--Create audit record and save the original values of any MedicalHistoryConditions that are being updated
	SET @AuditCursor = CURSOR FOR
	SELECT
		MH.MedicalHistoryConditionID
	FROM
		[Clinical].[MedicalHistoryCondition] MHC
		INNER JOIN @MedicalHistory MH
			ON MHC.MedicalHistoryID = MH.MedicalHistoryID
			AND MHC.MedicalConditionID = MH.MedicalConditionID
	WHERE
		MH.MedicalHistoryConditionID > 0
		AND MHC.HasCondition = 0
		AND MH.HasCondition = 1;    

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'MedicalHistoryCondition', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE @MedicalHistory
	SET MHCAuditDetailID = @AuditDetailID
	WHERE
		MedicalHistoryConditionID = @ID;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	--Insert/Update any MedicalHistoryConditions
	DECLARE @MHC TABLE
	(
		MergeAction NVARCHAR(25),
		MedicalHistoryConditionID BIGINT,
		MedicalHistoryID BIGINT,
		MedicalConditionID INT
	);

	MERGE [Clinical].[MedicalHistoryCondition] AS TARGET
	USING (SELECT DISTINCT MedicalHistoryConditionID, MedicalHistoryID, MedicalConditionID, HasCondition FROM @MedicalHistory) AS SOURCE
		ON SOURCE.MedicalHistoryID = TARGET.MedicalHistoryID
		AND SOURCE.MedicalConditionID = TARGET.MedicalConditionID
	WHEN NOT MATCHED AND SOURCE.MedicalHistoryConditionID = 0 AND SOURCE.HasCondition = 1 THEN
		INSERT
		(
			MedicalHistoryID,
			MedicalConditionID,
			HasCondition,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			SOURCE.MedicalHistoryID,
			SOURCE.MedicalConditionID,
			SOURCE.HasCondition,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)
	WHEN MATCHED AND SOURCE.MedicalHistoryConditionID > 0 AND TARGET.HasCondition = 0 AND SOURCE.HasCondition = 1 THEN
		UPDATE
		SET HasCondition = 1,
			IsActive = 1,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.MedicalHistoryConditionID,
		inserted.MedicalHistoryID,
		inserted.MedicalConditionID
	INTO @MHC;

	--Create an audit record of any inserted MedicalHistoryConditions
	IF EXISTS (SELECT TOP 1 * FROM @MHC WHERE MergeAction = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT MedicalHistoryConditionID FROM @MHC WHERE MergeAction = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'MedicalHistoryCondition', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'MedicalHistoryCondition', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Update the audit record with the changed values to the MedicalHistoryCondition record
	IF EXISTS (SELECT TOP 1 * FROM @MHC WHERE MergeAction = 'Update')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT DISTINCT
			MedicalHistoryConditionID, MH.MHCAuditDetailID
		FROM
			 @MedicalHistory MH
		WHERE
			MH.MHCAuditDetailID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'MedicalHistoryCondition', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	--Update the temp table record with MedicalHistoryConditionID's
	UPDATE MH
	SET MedicalHistoryConditionID = MHC.MedicalHistoryConditionID
	FROM
		@MedicalHistory MH
		INNER JOIN @MHC MHC
			ON MH.MedicalHistoryID = MHC.MedicalHistoryID
			AND MH.MedicalConditionID = MHC.MedicalConditionID
	WHERE
		MH.MedicalHistoryID = MHC.MedicalHistoryID
		AND MH.MedicalConditionID = MHC.MedicalConditionID;

	--Update the temp table records with MedicalHistoryConditionDetailID's
	UPDATE MH
	SET MedicalHistoryConditionDetailID = MHCD.MedicalHistoryConditionDetailID
	FROM
		@MedicalHistory MH
		INNER JOIN [Clinical].[MedicalHistoryConditionDetail] MHCD
			ON MH.MedicalHistoryConditionID = MHCD.MedicalHistoryConditionID
			AND MH.FamilyRelationshipID = MHCD.FamilyRelationshipID
	WHERE
		MH.MedicalHistoryConditionID = MHCD.MedicalHistoryConditionID
		AND MH.FamilyRelationshipID = MHCD.FamilyRelationshipID
		AND MH.HasCondition = 1
		AND MH.IsActive = 1
		AND ISNULL(MH.MedicalHistoryConditionDetailID, 0) = 0;

	UPDATE @MedicalHistory
	SET IsActive = 0
	WHERE
		HasCondition = 0;

	--Inactivate any MedicalHistoryConditionDetails which don't have the MedicalHistoryConditionDetailID in the temp table. Create audit record.
	SET @AuditCursor = CURSOR FOR
	SELECT
		MedicalHistoryConditionDetailID
	FROM
		Clinical.MedicalHistoryConditionDetail MHCD
		INNER JOIN Clinical.MedicalHistoryCondition MHC
			ON MHCD.MedicalHistoryConditionID = MHC.MedicalHistoryConditionID 
	WHERE
		MedicalHistoryConditionDetailID NOT IN (SELECT DISTINCT MedicalHistoryConditionDetailID FROM @MedicalHistory WHERE IsActive = 1)
		AND MHC.MedicalHistoryID IN (SELECT DISTINCT MedicalHistoryID FROM @MedicalHistory)
		AND MHCD.IsActive = 1;    

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Clinical', 'MedicalHistoryConditionDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.MedicalHistoryConditionDetail
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		MedicalHistoryConditionDetailID = @ID
		AND IsActive = 1;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Clinical', 'MedicalHistoryConditionDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	--Create audit record and save the original values of any MedicalHistoryConditionDetails that are being updated
	SET @AuditCursor = CURSOR FOR
	SELECT
		MH.MedicalHistoryConditionDetailID
	FROM
		@MedicalHistory MH
	WHERE
		MH.MedicalHistoryConditionDetailID > 0
		AND MH.IsActive = 1

	OPEN @AuditCursor 
	FETCH NEXT FROM @AuditCursor 
	INTO @ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'MedicalHistoryConditionDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE @MedicalHistory
	SET MHCDAuditDetailID = @AuditDetailID
	WHERE
		MedicalHistoryConditionDetailID = @ID;

	FETCH NEXT FROM @AuditCursor 
	INTO @ID
	END; 

	CLOSE @AuditCursor;
	DEALLOCATE @AuditCursor;

	--Insert/Update MedicalHistoryConditionDetail records
	DECLARE @MHCD_ID TABLE (MergeAction NVARCHAR(10), ID BIGINT);

	MERGE [Clinical].[MedicalHistoryConditionDetail] AS TARGET
	USING @MedicalHistory AS SOURCE
		ON SOURCE.MedicalHistoryConditionDetailID = TARGET.MedicalHistoryConditionDetailID
		AND SOURCE.IsActive = 1
	WHEN NOT MATCHED AND (SOURCE.MedicalHistoryConditionDetailID = 0 AND (SOURCE.FamilyRelationshipID is not null OR SOURCE.Comments <> '' OR SOURCE.IsSelf > 0)) THEN
		INSERT
		(
			MedicalHistoryConditionID,
			FamilyRelationshipID,
			IsSelf,
			Comments,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			SOURCE.MedicalHistoryConditionID,
			SOURCE.FamilyRelationshipID,
			SOURCE.IsSelf,
			SOURCE.Comments,
			SOURCE.IsActive,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)
	WHEN MATCHED THEN
		UPDATE
		SET FamilyRelationshipID = SOURCE.FamilyRelationshipID,
			IsSelf = SOURCE.IsSelf,
			Comments = SOURCE.Comments,
			IsActive = SOURCE.IsActive,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.MedicalHistoryConditionDetailID
	INTO
		@MHCD_ID;

	IF EXISTS (SELECT TOP 1 * FROM @MHCD_ID WHERE MergeAction = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @MHCD_ID WHERE MergeAction = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'MedicalHistoryConditionDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'MedicalHistoryConditionDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @MHCD_ID WHERE MergeAction = 'Update')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT DISTINCT
			MedicalHistoryConditionDetailID, MH.MHCDAuditDetailID
		FROM
			 @MedicalHistory MH
		WHERE
			MH.MHCDAuditDetailID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'MedicalHistoryConditionDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	EXEC sp_xml_removedocument @docHandle; 
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
