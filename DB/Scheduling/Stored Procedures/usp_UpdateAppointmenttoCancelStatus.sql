
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAppointmenttoCancelStatus]
-- Author:		John Crossen
-- Date:		10/05/2015
--
-- Purpose:		Update Appointment to Cancel
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Sumana Sangapu 	Initial creation.
-- 04/19/2016	Scott Martin	Added a CAST statement to the split function
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateAppointmenttoCancelStatus]
	@AppointmentID varchar(100),
	@CancelReasonID INT,
	@CancelComment NVARCHAR(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID BIGINT

	DECLARE @AppointmentIDs TABLE (ID BIGINT)

	SELECT
			@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY

	  			INSERT INTO @AppointmentIDs 
				SELECT CAST(Items AS BIGINT) FROM Core.fn_Split (@AppointmentID, ',')

				-- Run the audit procs
				DECLARE @AuditCursor CURSOR;

				SET @AuditCursor = CURSOR FOR
				SELECT ID FROM @AppointmentIDs;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @ID

				WHILE @@FETCH_STATUS = 0
				BEGIN
						EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'Appointment', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
						FETCH NEXT FROM @AuditCursor INTO @ID
				END; 

				CLOSE @AuditCursor;

				Update [Scheduling].[Appointment]
				SET [CancelReasonID] = @CancelReasonID,
					[CancelComment] = @CancelComment,
					[IsCancelled] = 1,
					[ModifiedBy] = @ModifiedBy,
					[ModifiedOn] = @ModifiedOn,
					CreatedBy = @ModifiedBy,
					CreatedOn = @ModifiedOn
				WHERE
					AppointmentID IN (SELECT ID FROM @AppointmentIDs)

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @ID

				WHILE @@FETCH_STATUS = 0
				BEGIN
						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'Appointment', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
						FETCH NEXT FROM @AuditCursor INTO @ID
				END; 

				CLOSE @AuditCursor;
				DEALLOCATE @AuditCursor;	
	END TRY
				
	BEGIN CATCH
		SELECT
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH

END