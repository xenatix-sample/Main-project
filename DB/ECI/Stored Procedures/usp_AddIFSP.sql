-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_AddIFSP]
-- Author:		Gurpreet Singh
-- Date:		10/21/2015
--
-- Purpose:		Inserts IFSP
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
-- 10/30/2015   John Crossen        Add ResponseID
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging/Removed IFSPID parameter
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_AddIFSP]
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
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID							INT OUTPUT

AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@NewID BIGINT;

DECLARE @TD_ID TABLE (ID BIGINT);
DECLARE @PG_ID TABLE (ID BIGINT);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully';

	BEGIN TRY
	INSERT INTO [ECI].[IFSP]
	(
		[ContactID],
		[IFSPTypeID],
		[IFSPMeetingDate],
		[IFSPFamilySignedDate],
		[MeetingDelayed],
		[ReasonForDelayID],
		[Comments],
		[AssessmentID],
		[ResponseID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@ContactID,
		@IFSPTypeID,
		@IFSPMeetingDate,
		@IFSPFamilySignedDate,
		@MeetingDelayed,
		@ReasonForDelayID,
		@Comments,
		@AssessmentID,
		@ResponseID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'IFSP', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'IFSP', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

---------------------------------IFSP Team Discipline----------------------------------------------------------------------------
		
	INSERT INTO [ECI].[IFSPTeamDiscipline]
	(
		IFSPID, 
		UserID, 
		IsActive, 
		ModifiedBy, 
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT inserted.TeamDisciplineID
	INTO @TD_ID
	SELECT 
		@ID,
		T.C.value('text()[1]','int'),
		1, 
		@ModifiedBy, 
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		@MembersXML.nodes('MemberDetails/UserID') AS T(C);

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @TD_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @NewID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'IFSPTeamDiscipline', @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'IFSPTeamDiscipline', @AuditDetailID, @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @NewID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

---------------------------------IFSP Parent Guardian----------------------------------------------------------------------------

	INSERT INTO [ECI].[IFSPParentGuardian]
	(
		IFSPID, 
		ContactID, 
		IsActive, 
		ModifiedBy, 
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT inserted.ParentGuardianID
	INTO @PG_ID
	SELECT 
		@ID,
		T.C.value('text()[1]','int'),
		1, 
		@ModifiedBy, 
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		@ParentGuardiansXML.nodes('ParentGuardianDetails/ContactID') AS T(C);

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @PG_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @NewID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'IFSPParentGuardian', @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'IFSPParentGuardian', @AuditDetailID, @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @NewID
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



