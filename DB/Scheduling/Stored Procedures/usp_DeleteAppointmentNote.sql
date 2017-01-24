

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteAppointmentNote]
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

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].usp_DeleteAppointmentNote
	@AppointmentNoteID BIGINT,
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
	

		DECLARE @NoteHeaderID BIGINT
		SELECT @NoteHeaderID=NoteHeaderID FROM Scheduling.AppointmentNote

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'NoteHeader', @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	

		UPDATE Registration.NoteHeader 
		SET IsActive=0,
		ModifiedBy=@ModifiedBy, 
		ModifiedOn=@ModifiedOn
		WHERE NoteHeaderID=@NoteHeaderID

			
		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'NoteHeader', @AuditDetailID, @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  	
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Scheduling', 'AppointmentNote', @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	


		UPDATE Scheduling.AppointmentNote
		SET IsActive=0,
		ModifiedBy=@ModifiedBy, 
		ModifiedOn=@ModifiedOn
		WHERE AppointmentNoteID=@AppointmentNoteID	

			EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Scheduling', 'AppointmentNote', @AuditDetailID, @AppointmentNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  	

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END