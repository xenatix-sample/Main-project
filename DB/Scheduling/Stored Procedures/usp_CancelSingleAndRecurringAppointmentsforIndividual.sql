-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_CancelSingleAndRecurringAppointmentsforIndividual]
-- Author:		Sumana Sangapu
-- Date:		04/20/2016
--
-- Purpose:		Cancels single and recurring appointments for an individual from Single Appointment Cancel screen. Cancels the appointment related to all the related resources as well.
--				
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/20/2016	Sumana Sangapu    Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_CancelSingleAndRecurringAppointmentsforIndividual]
	@AppointmentID bigint,
	@RecurrenceID INT NULL,
	@CancelReasonID INT,
	@CancelComment NVARCHAR(500),
	@IsSelectedAppointment BIT = NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT, 
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
	
	SELECT
			@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
		DECLARE @AppointmentStatusDetails TABLE (ID BIGINT, ModifiedOn datetime)

		DECLARE @ID int

		DECLARE @AppDetails TABLE ( [AppointmentID] int NOT NULL,
									[AppointmentResourceID] int NOT NULL)

		DECLARE @AppIDs TABLE ( [AppointmentID] int NOT NULL )

		DECLARE @AppointmentIDString varchar(100)
								
		-- Fetch all the appointments for that Resource and If @IsSelectedAppointment is 0 then cancell All recurring appointments or else cancel just that appointment 
		
		-- Selected Appointmeent for an individual 
		IF @IsSelectedAppointment = 1 
		BEGIN

			INSERT INTO @AppDetails
			SELECT 		A.AppointmentID, R.AppointmentResourceID
			FROM		Scheduling.AppointmentResource R
			INNER JOIN	Scheduling.Appointment A
			ON			R.AppointmentID = A.AppointmentID
			WHERE 		A.AppointmentID = @AppointmentID
			AND			R.IsActive = 1
			AND			A.IsActive = 1
			AND			A.IsCancelled = 0 

			--add date filter after modifying AppointmentStartTime to time
		END 
		ELSE  -- Cancel all the recurring appointments for the individual related to that appointment
		BEGIN
		
			INSERT INTO @AppDetails
			SELECT 		A.AppointmentID, R.AppointmentResourceID
			FROM		Scheduling.AppointmentResource R
			INNER JOIN	Scheduling.Appointment A
			ON			R.AppointmentID = A.AppointmentID
			WHERE 		A.RecurrenceID = @RecurrenceID
			AND			R.IsActive = 1
			AND			A.IsActive = 1
			AND			A.IsCancelled = 0 
			--add date filter after modifying AppointmentStartTime to time
		END 
	 
		-- Insert into Scheduling.AppointmentStatusDetails
		INSERT INTO Scheduling.AppointmentStatusDetails
		(
		AppointmentResourceID,
		AppointmentStatusId,
		StartDateTime,
		IsCancelled,
		CancelReasonID,
		Comments,
		IsActive,
		ModifiedBy ,
		ModifiedOn ,
		CreatedBy ,
		CreatedOn
		)
		OUTPUT INSERTED.AppointmentStatusDetailID, Inserted.ModifiedOn INTO @AppointmentStatusDetails
		SELECT	AppointmentResourceID, 
				2, -- AppointmentStatusID = 2 ( Cancel )(Reference.AppointmentStatus)
				@ModifiedOn,
				1, --@IsCancelled
				@CancelReasonID,
				@CancelComment,
				1,
				@ModifiedBy,
				@ModifiedOn,
				@ModifiedBy,
				@ModifiedOn
		FROM @AppDetails
		
		-- Run the audit procs
		DECLARE AuditCursor CURSOR FOR
		SELECT ID, ModifiedOn FROM @AppointmentStatusDetails;    

		OPEN AuditCursor 
		FETCH NEXT FROM AuditCursor INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentStatusDetails', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentStatusDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM AuditCursor INTO @ID, @ModifiedOn
		END; 

		CLOSE AuditCursor;
		DEALLOCATE AuditCursor;

		INSERT INTO @AppIDs
		SELECT  DISTINCT AppointmentID FROM @AppDetails

		-- Update the IsCancelled fields in Scheduling.Appointment since the entire appointment is cancelled

		IF @IsSelectedAppointment = 1 
		BEGIN 
			SELECT @AppointmentIDString = @AppointmentID
		END
		ELSE
		BEGIN 
			SELECT  @AppointmentIDString = COALESCE(@AppointmentIDString + ', ', '') + convert (varchar(10),AppointmentID) FROM @AppIDs 
		END 
			
		EXEC [Scheduling].[usp_UpdateAppointmenttoCancelStatus]	@AppointmentIDString, @CancelReasonID, @CancelComment, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END