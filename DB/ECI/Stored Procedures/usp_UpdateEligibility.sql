-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_UpdateEligibility]
-- Author:		Sumana Sangapu
-- Date:		10/13/2015
--
-- Purpose:		Update Contact's ECI Eligibility Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Sumana Sangapu	TFS:2700	Initial Creation
-- 10/19/2015   Justin Spalti - Removed a few parameters that were not needed
-- 10/20/2015   Justin Spalti - Changed the update statement to a merge
-- 10/30/2015   Justin Spalti - Corrected a bug in the XML parsing
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field, Added CreatedBy and CreatedOn fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateEligibility]
	@EligibilityID BIGINT,
	@EligibilityDate DATE,
	@EligibilityTypeID int,
	@EligibilityDurationID int,
	@EligibilityCategoryID INT,
	@EligibilityXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT,
		@UserID BIGINT,
		@AuditCursor CURSOR;

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'Eligibility', @EligibilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE a
	SET	EligibilityDate	= @EligibilityDate,
		EligibilityDurationID = @EligibilityDurationID,
		EligibilityCategoryID = @EligibilityCategoryID,
		EligibilityTypeID = @EligibilityTypeID,
		IsActive = 1,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		[ECI].[Eligibility] a
	WHERE
		EligibilityID = @EligibilityID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'Eligibility', @AuditDetailID, @EligibilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

--------------------------------------Eligibility Team Discipline----------------------------------------------------------------------

	DECLARE @ETD TABLE
	(
		TeamDisciplineID BIGINT,
		UserID BIGINT,
		EligibilityID BIGINT,
		AuditDetailID BIGINT
	);

	INSERT INTO @ETD
	SELECT
		ETD.TeamDisciplineID,
		T.C.value('(.)[1]','int') AS UserID,
		ETD.EligibilityID,
		NULL
	FROM
		@EligibilityXML.nodes('EligibilityDetails/UserID') AS T(C)
		FULL OUTER JOIN (SELECT * FROM ECI.EligibilityTeamDiscipline WHERE EligibilityID = @EligibilityID) AS ETD
			ON T.C.value('(.)[1]','int') = ETD.UserID;

	IF EXISTS (SELECT TOP 1 * FROM @ETD WHERE TeamDisciplineID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT TeamDisciplineID, UserID FROM @ETD WHERE TeamDisciplineID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @UserID IS NOT NULL
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'EligibilityTeamDiscipline', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'EligibilityTeamDiscipline', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		UPDATE @ETD
		SET AuditDetailID = @AuditDetailID
		WHERE
			TeamDisciplineID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	DECLARE @ETD_ID TABLE
	(
		Operation NVARCHAR(12),
		ID BIGINT,
		ModifiedOn DATETIME
	);
	
	MERGE [ECI].[EligibilityTeamDiscipline] AS TARGET
	USING (SELECT * FROM @ETD WHERE UserID IS NOT NULL) AS SOURCE
		ON SOURCE.UserID = TARGET.UserID
		AND TARGET.EligibilityID = @EligibilityID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			EligibilityID,
			UserID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@EligibilityID,
			SOURCE.UserID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)
	WHEN NOT MATCHED BY SOURCE AND TARGET.EligibilityID = @EligibilityID THEN
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
		inserted.TeamDisciplineID,
		inserted.ModifiedOn
	INTO
		@ETD_ID;

	IF EXISTS (SELECT TOP 1 * FROM @ETD_ID WHERE Operation = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @ETD_ID WHERE Operation = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'EligibilityTeamDiscipline', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'EligibilityTeamDiscipline', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @ETD WHERE TeamDisciplineID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT TeamDisciplineID, UserID, AuditDetailID FROM @ETD WHERE TeamDisciplineID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @UserID IS NOT NULL
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'EligibilityTeamDiscipline', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'EligibilityTeamDiscipline', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END		

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END