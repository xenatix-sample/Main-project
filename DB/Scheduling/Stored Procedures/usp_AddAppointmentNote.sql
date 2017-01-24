-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddAppointmentNote]
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

CREATE PROCEDURE [Scheduling].[usp_AddAppointmentNote]
	@ContactID BIGINT NULL,	
	@GroupID BIGINT NULL,
	@UserID BIGINT NULL,
	@NoteTypeID INT,
	@AppointmentID BIGINT,
	@NoteText NVARCHAR(MAX),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	
		--Contact note: get note header id
		IF @ContactID IS NOT NULL

		BEGIN

		DECLARE @NoteHeaderID BIGINT

		INSERT INTO Registration.NoteHeader
			        ( ContactID ,
			          NoteTypeID ,
			          TakenBy ,
			          TakenTime ,
			          IsActive ,
			          ModifiedBy ,
			          ModifiedOn ,
			          CreatedBy ,
			          CreatedOn
			        )
			VALUES  (@ContactID,
					@NoteTypeID,
					@ModifiedBy,
					@ModifiedOn,
					1,
					@ModifiedBy,
					@ModifiedOn,
					@ModifiedBy,
					@ModifiedOn)


			SELECT @NoteHeaderID=SCOPE_IDENTITY()
			
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeader', @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeader', @AuditDetailID, @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					
			INSERT INTO Scheduling.AppointmentNote
			(
			NoteHeaderID,
			GroupHeaderID,
			UserID,
			AppointmentId,
			NoteText,
			IsActive,
			ModifiedBy ,
		    ModifiedOn ,
			CreatedBy ,
			CreatedOn
			        )
			VALUES(
			@NoteHeaderID,
			NULL,
			NULL,
			@AppointmentID,
			@NoteText,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn)


			SELECT @ID=SCOPE_IDENTITY()

			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentNote', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			END

			--Group note: insert into Scheduling.AppointmentNote
			ELSE IF @GroupID IS NOT NULL

			BEGIN

			INSERT INTO Scheduling.AppointmentNote
			(
			NoteHeaderID,
			GroupHeaderID,
		    UserID,
			AppointmentId,
			NoteText,
			IsActive,
			ModifiedBy ,
		    ModifiedOn ,
			CreatedBy ,
			CreatedOn
			        )
			VALUES(
			NULL,
			@GroupID,
			NULL,
			@AppointmentID,
			@NoteText,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn)


			SELECT @ID=SCOPE_IDENTITY()

			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentNote', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			END

			--User/Provider note: insert into Scheduling.AppointmentNote
			ELSE IF @UserID IS NOT NULL

			BEGIN

			INSERT INTO Scheduling.AppointmentNote
			(
			NoteHeaderID,
			GroupHeaderID,
		    UserID,
			AppointmentId,
			NoteText,
			IsActive,
			ModifiedBy ,
		    ModifiedOn ,
			CreatedBy ,
			CreatedOn
			        )
			VALUES(
			NULL,
			NULL,
			@UserID,
			@AppointmentID,
			@NoteText,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn)


			SELECT @ID=SCOPE_IDENTITY()

			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentNote', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			END

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END