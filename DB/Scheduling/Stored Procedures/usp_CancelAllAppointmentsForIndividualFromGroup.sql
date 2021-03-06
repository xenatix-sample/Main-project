-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_CancelAllAppointmentsForIndividualFromGroup]
-- Author:		Sumana Sangapu
-- Date:		03/24/2016
--
-- Purpose:		Cancels all appointments for an individual from Group
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Sumana Sangapu 	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_CancelAllAppointmentsForIndividualFromGroup]
	@ResourceID BIGINT,
	@ResourceTypeID INT,
	@GroupHeaderID	INT,
	@AppointmentStatusId INT,
	@CancelReasonID INT,
	@CancelComment NVARCHAR(1000),
	@IsCancelled BIT,
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

	DECLARE @ID int,
			@ModifiedOn datetime


	-- Fetch all the appointments for that Resource present in that Group
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
	OUTPUT INSERTED.AppointmentStatusDetailID, Inserted.ModifiedOn
	INTO @AppointmentStatusDetails
	SELECT	ar.AppointmentResourceID, 
			@AppointmentStatusId, 
			@ModifiedOn,
			1, -- @IsCancelled
			@CancelReasonID,
			@CancelComment,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
	FROM	[Scheduling].[AppointmentResource] ar
	LEFT JOIN [Scheduling].[Appointment] a
	ON		a.AppointmentID = ar.AppointmentID
	WHERE	ar.GroupHeaderID = @GroupHeaderID
	AND		ar.ResourceID = @ResourceID
	AND		ar.ResourceTypeID = @ResourceTypeID
	--add date filter after modifying AppointmentStartTime to time

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @AppointmentStatusDetails;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentStatusDetails', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentStatusDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
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