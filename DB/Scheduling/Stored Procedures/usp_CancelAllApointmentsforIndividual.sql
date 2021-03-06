-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_CancelAllApointmentsforIndividual]
-- Author:		Sumana Sangapu
-- Date:		04/03/2016
--
-- Purpose:		Cancels all the appointments single or group for an individual
--				For ex: This proc is invoked when the individual is discharged, all his appointments are cancelled.
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/03/2016	Sumana Sangapu    Initial creation.
-- 04/16/2016	Scott Martin	Cursor declaration was missing a value
-- 04/19/2016	Scott Martin	Added filter for Program Unit
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_CancelAllApointmentsforIndividual]
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@OrganizationID BIGINT,
	@CancelComment NVARCHAR(500),
	@ModifiedOn DATETIME,
	@ModifiedBy INT, 
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
		DECLARE @tblApptID TABLE (ApptID bigint, AppointmentResourceID bigint )
		DECLARE @AppID bigint,
				@AppointmentResourceID bigint,
				@GroupHeaderID bigint,
				@ApptIDString VARCHAR(100)

		/*********************************************************** SINGLE APPOINTMENTS ******************************************************************************/
		-- Get all the single appointments that are not cancelled for the ResourceID. Will add the filter on future appointments once references of AppointmentContact are removed. 
		INSERT INTO @tblApptID
		SELECT 		A.AppointmentID, R.AppointmentResourceID
		FROM		Scheduling.AppointmentResource R
		INNER JOIN	Scheduling.Appointment A
		ON			R.AppointmentID = A.AppointmentID
		INNER JOIN	Scheduling.AppointmentType AT
		ON			A.AppointmentTypeID = AT.AppointmentTypeID
		WHERE 		R.ResourceID = @ResourceID 
		AND			R.ResourceTypeID = @ResourceTypeID 
		AND			R.IsActive = 1
		AND			A.IsActive = 1
		AND			A.IsCancelled = 0
		AND			(A.ProgramID = @OrganizationID OR ISNULL(@OrganizationID, '') = '')


		DECLARE @ApptCursor CURSOR;

		SET @ApptCursor = CURSOR FOR
		SELECT ApptID, AppointmentResourceID FROM @tblApptID;    

		OPEN @ApptCursor 
		FETCH NEXT FROM @ApptCursor 
		INTO  @AppID, @AppointmentResourceID


		WHILE @@FETCH_STATUS = 0
		BEGIN
				-- Insert into AppointmentStatusDetail
				EXEC [Scheduling].[usp_AddAppointmentStatusDetail]	@AppointmentResourceID = @AppointmentResourceID, @AppointmentStatusId =2, @IsCancelled =1 ,@CancelReasonID = 2,--Provider Cancelled
																	@Comments = @CancelComment, @ModifiedOn =@ModifiedOn, @ModifiedBy =@ModifiedBy,	@ResultCode =@ResultCode OUTPUT, @ResultMessage = @ResultMessage OUTPUT,
																	@ID = NULL	
																	
				SET @ApptIDString = CAST(@AppID AS VARCHAR(100));													
				-- Update Scheduling.Appointment 's Cancel details
				EXEC [Scheduling].[usp_UpdateAppointmenttoCancelStatus] @AppointmentID = @ApptIDString, @CancelReasonID=2, @CancelComment =@CancelComment, @ModifiedOn =@ModifiedOn, @ModifiedBy =@ModifiedBy,
																		@ResultCode = @ResultCode OUTPUT,	@ResultMessage = @ResultMessage OUTPUT

				FETCH NEXT FROM @ApptCursor INTO  @AppID, @AppointmentResourceID
		END; 

		CLOSE @ApptCursor;

		/****************************************************** GROUP APPOINTMENTS ***************************************************************************/
		DECLARE @tblGroupApptID TABLE (ApptID bigint, AppointmentResourceID bigint, GroupHeaderID bigint )

		INSERT INTO @tblGroupApptID
		SELECT		A.AppointmentID, Ar.ResourceID, gh.GroupHeaderID 
		FROM	[Scheduling].[GroupSchedulingHeader]  gh
		LEFT JOIN [Scheduling].[GroupDetails] gd
		ON		gh.GroupDetailID = gd.GroupDetailID 
		LEFT JOIN Scheduling.AppointmentResource ar
		ON		ar.groupheaderid = gh.GroupHeaderID 
		LEFT JOIN Scheduling.Appointment a
		ON		ar.appointmentid = a.appointmentID 
		LEFT JOIN Reference.Program p
		ON		a.ProgramID = P.ProgramID 
		LEFT JOIN Reference.Facility f
		ON		f.FacilityID = a.FacilityID
		LEFT JOIN Scheduling.AppointmentType at
		on		gd.GroupTypeID = at.AppointmentTypeID
		WHERE	ar.ResourceID = @ResourceID
		AND		ar.resourcetypeid = @ResourceTypeID -- Contact - ResourceType
		AND		gd.IsActive = 1
		AND		a.IsCancelled = 0 -- NotCancelled
		AND		(A.ProgramID = @OrganizationID OR ISNULL(@OrganizationID, 0) = 0)

		DECLARE @GroupApptCursor CURSOR;

		SET @GroupApptCursor = CURSOR FOR
		SELECT ApptID , AppointmentResourceID, GroupHeaderID FROM @tblGroupApptID;    

		OPEN @GroupApptCursor 
		FETCH NEXT FROM @GroupApptCursor 
		INTO  @AppID, @AppointmentResourceID, @GroupHeaderID

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC	[Scheduling].[usp_CancelAllAppointmentsForIndividualFromGroup] @ResourceID= @ResourceID,	@ResourceTypeID = @ResourceTypeID,@GroupHeaderID =@GroupHeaderID, @AppointmentStatusId =2,
																				   @CancelReasonID =2, @CancelComment = @CancelComment, @IsCancelled=1, @ModifiedBy=@ModifiedBy,
																				   @ResultCode =@ResultCode  OUTPUT,@ResultMessage =@ResultMessage OUTPUT

			FETCH NEXT FROM @GroupApptCursor INTO @AppID, @AppointmentResourceID, @GroupHeaderID
		END; 

		CLOSE @GroupApptCursor;



	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END
