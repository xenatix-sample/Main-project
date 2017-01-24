-- Procedure:	[usp_UpdateAppointment]
-- Author:		John Crossen
-- Date:		10/05/2015
--
-- Purpose:		Update Appointment Header Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/05/2015	John Crossen    TFS#2590		Initial creation.
-- 10/31/2015   Satish Singh	                 Added LocationID, LocationType
-- 02/02/2016   Satish Singh                     Added ModifiedOn 
-- 02/12/2016   Satish Singh        Added CancelReasonID and CancelComment
-- 02/17/2016	Scott Martin		Added audit logging
-- 04/01/2016   Justin Spalti       Added two parameters for IsInterpreterRequired and ServiceStatusID
-- 04/01/2016	Sumana Sangapu		Added NonMHMRAppointment
-- 04/11/2016	Sumana Sangapu		Added IsGroup and modified AppointmentStartTime to Time datatype
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.usp_UpdateAppointment
	@AppointmentID BIGINT,
	@ProgramID INT,
	@FacilityID INT,
	@AppointmentTypeID INT,
	@ServicesID INT,
	@ServiceStatusID SMALLINT,
	@AppointmentDate DATE,
	@AppointmentStartTime SMALLINT,
	@AppointmentLength SMALLINT,
	@SupervisionVisit BIT,
	@ReferredBy NVARCHAR(255),
	@ReasonForVisit NVARCHAR(4000),
	@RecurrenceID BIGINT,
	@CancelReasonID INT,
	@CancelComment NVARCHAR(1000),
	@IsCancelled BIT,
	@Comments NVARCHAR(4000),
	@IsInterpreterRequired BIT,
	@NonMHMRAppointment nvarchar(100),
	@IsGroupAppointment BIT,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'Appointment', @AppointmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	Update [Scheduling].[Appointment]
	SET [ProgramID] = @ProgramID,
		FacilityID = @FacilityID,
		[AppointmentTypeID] = @AppointmentTypeID,
		[ServicesID] = @ServicesID,
		[ServiceStatusID] = @ServiceStatusID,
		[AppointmentDate] = @AppointmentDate,
		[AppointmentStartTime] = @AppointmentStartTime,
		[AppointmentLength] = @AppointmentLength,
		[SupervisionVisit] = @SupervisionVisit,
		[ReferredBy] = @ReferredBy,
		[ReasonForVisit] = @ReasonForVisit,
		[RecurrenceID] = @RecurrenceID,
		[CancelReasonID] = @CancelReasonID,
		[CancelComment] = @CancelComment,
		[IsCancelled] = @IsCancelled,
		[Comments] = @Comments,
		[IsInterpreterRequired] = @IsInterpreterRequired,
		[NonMHMRAppointment] = @NonMHMRAppointment,
		[IsGroupAppointment] = @IsGroupAppointment,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		CreatedBy = @ModifiedBy,
		CreatedOn = @ModifiedOn
	WHERE
		AppointmentID = @AppointmentID;
	
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'Appointment', @AuditDetailID, @AppointmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
				
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END