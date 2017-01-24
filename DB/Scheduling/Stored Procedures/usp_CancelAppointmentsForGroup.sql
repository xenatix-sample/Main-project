-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_CancelAppointmentsForGroup]
-- Author:		Sumana Sangapu
-- Date:		03/24/2016
--
-- Purpose:		Cancels all appointments for that Group - Selected appointment and All appointments.
--				- Identify all the appointments for that group.
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Sumana Sangapu 	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_CancelAppointmentsForGroup]
	@GroupDetailID	INT,
	@CancelReasonID INT,
	@CancelComment NVARCHAR(1000),
	@AppointmentID bigint = NULL,
	@ModifiedOn datetime,
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

	DECLARE @AppDetails TABLE ( 
								[AppointmentResourceID] int NOT NULL,
								[AppointmentID] int NOT NULL)

	DECLARE @AppointmentIDString varchar(100)
								
	-- Fetch all the appointments for that Resource present in that Group and If AppointmentID is null then cancell All or else cancel just that appointment 
	-- Selected Appointmeent for that Group
	IF @AppointmentID IS NOT NULL 
	BEGIN
		INSERT INTO @AppDetails
		SELECT	ar.AppointmentResourceID, ar.AppointmentID
		FROM	[Scheduling].[GroupDetails] g
		INNER JOIN [Scheduling].[GroupSchedulingHeader] gh
		ON		g.GroupDetailID = gh.GroupDetailID
		LEFT JOIN [Scheduling].[AppointmentResource] ar
		ON		ar.GroupHeaderID = gh.GroupHeaderID
		LEFT JOIN [Scheduling].[Appointment] a
		ON		a.AppointmentID = ar.AppointmentID
		WHERE	g.GroupDetailID = @GroupDetailID
		AND		a.AppointmentID = @appointmentID
		--add date filter after modifying AppointmentStartTime to time
	END 
	ELSE  -- Cancel all the appointments for that group
	BEGIN
		INSERT INTO @AppDetails
		SELECT	ar.AppointmentResourceID, ar.AppointmentID
		FROM	[Scheduling].[GroupDetails] g
		INNER JOIN [Scheduling].[GroupSchedulingHeader] gh
		ON		g.GroupDetailID = gh.GroupDetailID
		LEFT JOIN [Scheduling].[AppointmentResource] ar
		ON		ar.GroupHeaderID = gh.GroupHeaderID
		LEFT JOIN [Scheduling].[Appointment] a
		ON		a.AppointmentID = ar.AppointmentID
		WHERE	g.GroupDetailID = @GroupDetailID
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
			2, -- 2 = Cancel (Reference.AppointmentStatus)
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
	
	-- Update the IsCancelled fields in Scheuling.Appointment since the entire appointment is cancelled

	SELECT @AppointmentIDString = COALESCE(@AppointmentIDString + ', ', '') + convert (varchar(10),AppointmentID) FROM @AppDetails 
	
	EXEC [Scheduling].[usp_UpdateAppointmenttoCancelStatus]	@AppointmentIDString, @CancelReasonID, @CancelComment, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	END TRY
				
	BEGIN CATCH
		SELECT
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH

END