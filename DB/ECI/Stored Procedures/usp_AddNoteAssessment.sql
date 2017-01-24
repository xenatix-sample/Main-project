-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_AddNoteAssessment]
-- Author:		Gurpreet Singh
-- Date:		01/12/2016
--
-- Purpose:		Add Note Assessment
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2016	Gurpreet Singh	Initial Creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-- 01/29/2016   Satish Singh		Added Location
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_AddNoteAssessment]
	@ProgressNoteID BIGINT,
	@NoteAssessmentDate DATETIME,
	@NoteAssessmentTime TIME(7),
	@LocationID INT,
	@Location NVARCHAR(500),
	@ProviderID INT,
	@MembersInvited NVARCHAR(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode	INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT  @ID = 0,
			@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	INSERT [ECI].[ScheduleNoteAssessment]
	(
		ProgressNoteID,
		NoteAssessmentDate,
		NoteAssessmentTime,
		LocationID,
		Location,
		ProviderID,
		MembersInvited,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ProgressNoteID,
		@NoteAssessmentDate,
		@NoteAssessmentTime,
		@LocationID,
		@Location,
		@ProviderID,
		@MembersInvited,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'ScheduleNoteAssessment', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'ScheduleNoteAssessment', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END