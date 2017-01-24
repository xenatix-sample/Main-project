----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentNote]
-- Author:		John Crossen
-- Date:		03/09/2016
--
-- Purpose:		Get for Appointment Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/09/2016	John Crossen   7591	- Initial creation.
-- 03/11/2016	Karl Jablonski 7591	- Modified where clause to check against the group id.
-- 03/17/2016	Karl Jablonski 7591	- Modified to get correct note for contacts/users/groups.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetAppointmentNote]
	@AppointmentID BIGINT,
	@ContactID BIGINT NULL,
	@GroupID BIGINT NULL,
	@UserID BIGINT NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	

	IF @GroupID IS NOT NULL

	BEGIN

	SELECT TOP 1 AppointmentNoteID, AppointmentID, NoteText, GroupHeaderID 
	FROM Scheduling.AppointmentNote
	WHERE AppointmentID=@AppointmentID AND GroupHeaderID = @GroupID AND IsActive=1
	
	END


	ELSE IF @UserID IS NOT NULL

	BEGIN

	SELECT AppointmentNoteID, AppointmentID, NoteText, GroupHeaderID  
	FROM Scheduling.AppointmentNote AN JOIN Core.Users CU ON AN.UserID=CU.UserID
	WHERE CU.UserID=@UserID AND AN.AppointmentID=@AppointmentID  AND CU.IsActive=1 AND AN.IsActive=1

	END

	ELSE IF @ContactID IS NOT NULL

	BEGIN

	SELECT AppointmentNoteID, AppointmentID, NoteText, GroupHeaderID  
	FROM Scheduling.AppointmentNote AN JOIN Registration.NoteHeader NH ON AN.NoteHeaderID=NH.NoteHeaderID
	WHERE NH.ContactID=@ContactID AND AN.AppointmentID=@AppointmentID  AND NH.IsActive=1 AND AN.IsActive=1

	END

	
	

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END