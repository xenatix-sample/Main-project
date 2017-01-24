-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_UpdateIFSP]
-- Author:		Gurpreet Singh
-- Date:		10/21/2015
--
-- Purpose:		Updates IFSP
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/21/2015   Gurpreet Singh		Initial Creation
-- 10/26/2015	Gurpreet Singh		Added Members data
-- 10/27/2015	Gurpreet Singh		Added Comments field, Multiple members saving
-- 10/27/2015	Sumana Sangapu		Add AssessmentID
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field, Added CreatedBy and CreatedOn fields
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateIFSP]
	@IFSPID BIGINT,
	@ContactID BIGINT,
	@IFSPTypeID INT,
	@IFSPMeetingDate DATE,
	@IFSPFamilySignedDate DATE,
	@MeetingDelayed	BIT,
	@ReasonForDelayID INT,
	@Comments NVARCHAR(2000),
	@AssessmentID bigint,
	@ResponseID bigint NULL,
	@MembersXML	XML,
	@ParentGuardiansXML	XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@UserID BIGINT,
		@AuditDetailID BIGINT,
		@AuditCursor CURSOR,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'IFSP', @IFSPID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

		UPDATE [ECI].[IFSP]
		SET [ContactID] = @ContactID,
			[IFSPTypeID] = @IFSPTypeID,
			[IFSPMeetingDate] = @IFSPMeetingDate,
			[IFSPFamilySignedDate] = @IFSPFamilySignedDate,
			[MeetingDelayed] = @MeetingDelayed,
			[ReasonForDelayID] = @ReasonForDelayID,
			[Comments] = @Comments,
			[AssessmentID] = @AssessmentID,
			ResponseID=@ResponseID,
			[IsActive] = 1,
			[ModifiedBy] = @ModifiedBy,
			[ModifiedOn] = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			IFSPID = @IFSPID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'IFSP', @AuditDetailID, @IFSPID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;


--------------------------------------IFSP Team Discipline----------------------------------------------------------------------
		
	DECLARE @TD TABLE
		(
			TeamDisciplineID BIGINT,
		UserID BIGINT,
			IFSPID BIGINT,
		AuditDetailID BIGINT
		);

	INSERT INTO @TD
		SELECT 
		TD.TeamDisciplineID,
		T.C.value('text()[1]','int') as UserID,
		TD.IFSPID,
		NULL
		FROM
			@MembersXML.nodes('MemberDetails/UserID') AS T(C)
		FULL OUTER JOIN (SELECT * FROM ECI.IFSPTeamDiscipline WHERE IFSPID = @IFSPID) AS TD
			ON T.C.value('text()[1]','int') = TD.UserID;

	IF EXISTS (SELECT TOP 1 * FROM @TD WHERE TeamDisciplineID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT TeamDisciplineID, UserID FROM @TD WHERE TeamDisciplineID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @UserID IS NOT NULL
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'IFSPTeamDiscipline', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'IFSPTeamDiscipline', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		UPDATE @TD
		SET AuditDetailID = @AuditDetailID
		WHERE
			TeamDisciplineID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	DECLARE @TD_ID TABLE
		(
		Operation NVARCHAR(12),
		ID BIGINT,
		ModifiedOn DATETIME
	);
		
			MERGE [ECI].[IFSPTeamDiscipline] AS TARGET
	USING (SELECT * FROM @TD) AS SOURCE
				ON SOURCE.UserID = TARGET.UserID
				AND TARGET.IFSPID = @IFSPID
			WHEN NOT MATCHED BY TARGET THEN
				INSERT
				(
					IFSPID,
					UserID,
					IsActive,
					ModifiedBy,
					ModifiedOn,
					CreatedBy,
					CreatedOn
				)
				VALUES
				(
					@IFSPID,
					SOURCE.UserID,
					1,
					@ModifiedBy,
					@ModifiedOn,
					@ModifiedBy,
					@ModifiedOn
				)
			WHEN NOT MATCHED BY SOURCE AND TARGET.IFSPID = @IFSPID THEN
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
		inserted.ModifiedOn;

	IF EXISTS (SELECT TOP 1 * FROM @TD_ID WHERE Operation = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @TD_ID WHERE Operation = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'IFSPTeamDiscipline', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'IFSPTeamDiscipline', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @TD WHERE TeamDisciplineID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT TeamDisciplineID, UserID, AuditDetailID FROM @TD WHERE TeamDisciplineID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @UserID IS NOT NULL
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'IFSPTeamDiscipline', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'IFSPTeamDiscipline', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @UserID, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

--------------------------------------Parent Guardian Details----------------------------------------------------------------------
		
	DECLARE @PG TABLE
		(
			ParentGuardianID BIGINT,
		ContactID BIGINT,
			IFSPID BIGINT,
		AuditDetailID BIGINT
		);

	INSERT INTO @PG
		SELECT 
		PG.ParentGuardianID,
		T.C.value('text()[1]','int') as ContactID,
		PG.IFSPID,
		NULL
		FROM
			@ParentGuardiansXML.nodes('ParentGuardianDetails/ContactID') AS T(C)
		FULL OUTER JOIN (SELECT * FROM ECI.IFSPParentGuardian WHERE IFSPID = @IFSPID) AS PG
			ON T.C.value('text()[1]','int') = PG.ContactID;

	IF EXISTS (SELECT TOP 1 * FROM @PG WHERE ParentGuardianID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ParentGuardianID, ContactID FROM @PG WHERE ParentGuardianID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ContactID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @ContactID IS NOT NULL
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'IFSPParentGuardian', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'IFSPParentGuardian', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		UPDATE @PG
		SET AuditDetailID = @AuditDetailID
		WHERE
			ParentGuardianID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ContactID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	DECLARE @PG_ID TABLE
		(
		Operation NVARCHAR(12),
		ID BIGINT,
		ModifiedOn DATETIME
	);

			MERGE [ECI].[IFSPParentGuardian] AS TARGET
	USING (SELECT * FROM @PG) AS SOURCE
				ON SOURCE.ContactID = TARGET.ContactID
				AND TARGET.IFSPID = @IFSPID
			WHEN NOT MATCHED BY TARGET THEN
				INSERT
				(
					IFSPID,
					ContactID,
					IsActive,
					ModifiedBy,
					ModifiedOn,
			CreatedBy,
			CreatedOn
				)
				VALUES
				(
					@IFSPID,
					SOURCE.ContactID,
					1,
					@ModifiedBy,
					@ModifiedOn,
					@ModifiedBy,
					@ModifiedOn
				)
			WHEN NOT MATCHED BY SOURCE AND TARGET.IFSPID = @IFSPID THEN
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
				inserted.ParentGuardianID,
		inserted.ModifiedOn;	

	IF EXISTS (SELECT TOP 1 * FROM @PG_ID WHERE Operation = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @PG_ID WHERE Operation = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'IFSPParentGuardian', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'IFSPParentGuardian', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @PG WHERE ParentGuardianID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ParentGuardianID, ContactID, AuditDetailID FROM @PG WHERE ParentGuardianID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ContactID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @UserID IS NOT NULL
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'IFSPParentGuardian', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'IFSPParentGuardian', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ContactID, @AuditDetailID
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