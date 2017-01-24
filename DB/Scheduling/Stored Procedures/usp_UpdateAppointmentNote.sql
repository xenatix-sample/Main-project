-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAppointmentNote]
-- Author:		John Crossen
-- Date:		03/09/2016
--
-- Purpose:		Insert for Appointment Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/09/2016	John Crossen   7591	- Initial creation.
-- 03/17/2016	Karl Jablonski	Modify to work for users/providers, contacts and groups

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateAppointmentNote]
	@AppointmentNoteID BIGINT,
	@ContactID BIGINT NULL,
	@GroupID BIGINT NULL,
	@UserID BIGINT NULL,
	@NoteTypeID INT,
	@IsActive BIT,
	@AppointmentID BIGINT,
	@NoteText NVARCHAR(MAX),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	
		--Contact note: get note header id and update
		IF @ContactID IS NOT NULL
		BEGIN

		DECLARE @NoteHeaderID BIGINT
		SELECT @NoteHeaderID=NoteHeaderID FROM Scheduling.AppointmentNote

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Noteheader', @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	

		UPDATE Registration.Noteheader 
		SET ContactID=@ContactID, 
		NoteTypeID=@NoteTypeID, 
		TakenBy=@ModifiedBy, 
		TakenTime=@ModifiedOn, 
		IsActive=@IsActive, 
		ModifiedBy=@ModifiedBy, 
		ModifiedOn=@ModifiedOn
		WHERE NoteHeaderID=@NoteHeaderID

			
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Noteheader', @AuditDetailID, @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  	
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'AppointmentNote', @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		UPDATE Scheduling.AppointmentNote
		SET 
		AppointmentID=@AppointmentID,
		Notetext=@NoteText,
		IsActive=@IsActive,
		ModifiedBy=@ModifiedBy, 
		ModifiedOn=@ModifiedOn
		WHERE AppointmentNoteID=@AppointmentNoteID	

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'AppointmentNote', @AuditDetailID, @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  		END

		--Group note: update Scheduling.AppointmentNote
		ELSE IF @GroupID IS NOT NULL
		BEGIN

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'AppointmentNote', @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		UPDATE Scheduling.AppointmentNote
		SET 
		GroupHeaderID=@GroupID,
		AppointmentID=@AppointmentID,
		Notetext=@NoteText,
		IsActive=@IsActive,
		ModifiedBy=@ModifiedBy, 
		ModifiedOn=@ModifiedOn
		WHERE AppointmentNoteID=@AppointmentNoteID	

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'AppointmentNote', @AuditDetailID, @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  		END


		--User/Provider note: update Scheduling.AppointmentNote
		ELSE IF @UserID IS NOT NULL
		BEGIN

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'AppointmentNote', @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		UPDATE Scheduling.AppointmentNote
		SET 
		UserID=@UserID,
		AppointmentID=@AppointmentID,
		Notetext=@NoteText,
		IsActive=@IsActive,
		ModifiedBy=@ModifiedBy, 
		ModifiedOn=@ModifiedOn
		WHERE AppointmentNoteID=@AppointmentNoteID	

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'AppointmentNote', @AuditDetailID, @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  		END

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END