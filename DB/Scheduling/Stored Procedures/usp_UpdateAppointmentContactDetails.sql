-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAppointmentContactDetails]
-- Author:		John Crossen
-- Date:		10/15/2015
--
-- Purpose:		Add Appointment Contact details  
--
-- Notes:		
--				
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/15/2015   John Crossen	2731 - Initial Creation
-- 10/16/2015   John Crossen    2765 - Remove XML
-- 11/02/2015   Rajiv Ranjan	-	   Remove @ModifiedOn parameter - not required
-- 12/15/2015	Scott Martin	Added audit logging
-- 02/17/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateAppointmentContactDetails]
	@AppointmentContactID BIGINT,
	@AppointmentID BIGINT,
	@ContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy int,	
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT

AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'AppointmentContact', @AppointmentContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	-- inactivate any appointment contact records for appointments that are inactive
	update Scheduling.AppointmentContact
	Set IsActive = 0
	from Scheduling.Appointment a
		inner join Scheduling.AppointmentContact ac on ac.AppointmentID = a.AppointmentID
	where ac.ContactID = @ContactID
		and a.IsActive = 0
	
	MERGE [Scheduling].[AppointmentContact] AS TARGET
	USING (select @AppointmentID as AppointmentID, @ContactID as ContactID) AS SOURCE
		ON SOURCE.AppointmentID = TARGET.AppointmentID
		AND SOURCE.ContactID = TARGET.ContactID
		AND TARGET.IsActive = 1
	WHEN NOT MATCHED THEN
		INSERT
		(
			AppointmentID,
			ContactID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@AppointmentID,
			@ContactID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)
	WHEN MATCHED THEN
		UPDATE
		SET AppointmentID = @AppointmentID,
			ContactID = @ContactID,
			IsActive = 1,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn
	;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'AppointmentContact', @AuditDetailID, @AppointmentContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH

END