
-- Procedure:	[usp_AddAppointment]
-- Author:		John Crossen
-- Date:		10/05/2015
--
-- Purpose:		Add Appointment Header Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/05/2015	 John Crossen    TFS#2590		Initial creation.
-- 10/15/2015    John Crossen    TFS#2731        Modify proc to accomodate refactored schema
-- 10/30/2015    Rajiv Ranjan    -				 Renamed @AppointmentID to @ID
-- 10/31/2015    Satish Singh	                 Added LocationID, LocationType
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-- 02/12/2016   Satish Singh        Added CancelReasonID and CancelComment
-- 02/17/2016	Scott Martin		Added audit logging
-- 03/02/2016	Scott Martin		Added Comments
-- 04/01/2016   Justin Spalti       Added two parameters for IsInterpreterRequired and ServiceStatusID
-- 04/01/2016	Sumana Sangapu		Added NonMHMRAppointments
-- 04/11/2016	Sumana Sangapu		Added IsGroup and modified AppointmentStartTime to Time datatype
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_AddAppointment]
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
	@ResultMessage nvarchar(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
	

	INSERT INTO [Scheduling].[Appointment]
    (
		[ProgramID],
		FacilityID,
		[AppointmentTypeID],
		[ServicesID],
		[ServiceStatusID],
		[AppointmentDate],
		[AppointmentStartTime],
		[AppointmentLength],
		[SupervisionVisit],
		[ReferredBy],
		[ReasonForVisit],
		RecurrenceID,
		[CancelReasonID],
		[CancelComment],
		[IsCancelled],
		[Comments],
		[IsInterpreterRequired],
		[NonMHMRAppointment],
		[IsGroupAppointment],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ProgramID,
		@FacilityID,
		@AppointmentTypeID,
		@ServicesID,
		@ServiceStatusID,
		@AppointmentDate,
		@AppointmentStartTime,
		@AppointmentLength,
		@SupervisionVisit,
		@ReferredBy,
		@ReasonForVisit,
		@RecurrenceID,
		@CancelReasonID,
		@CancelComment,
		@IsCancelled,
		@Comments,
		@IsInterpreterRequired,
		@NonMHMRAppointment,
		@IsGroupAppointment,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'Appointment', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'Appointment', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END