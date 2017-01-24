

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAppointmentStatusDetail]
-- Author:		John Crossen
-- Date:		03/11/2016
--
-- Purpose:		Insert for Appointment Status Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	John Crossen   7687	- Initial creation.
-- 03/17/2016	Sumana SAngapu Added Cancel fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].usp_UpdateAppointmentStatusDetail
	@AppointmentStatusDetailID BIGINT ,
	@AppointmentResourceID BIGINT,
	@AppointmentStatusID INT,
	@IsCancelled bit,
	@CancelReasonID int,
	@Comments nvarchar(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'AppointmentStatusDetails', @AppointmentStatusDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
			UPDATE Scheduling.AppointmentStatusDetails
			SET 
			AppointmentResourceID= @AppointmentResourceID,
			AppointmentStatusID=@AppointmentStatusID,
			IsCancelled = @IsCancelled,
			CancelReasonID = @CancelReasonID,
			Comments = @Comments,
			StartDateTime=@ModifiedOn,
			ModifiedBy =@ModifiedBy ,
		    ModifiedOn=@ModifiedOn
			WHERE AppointmentStatusDetailID=@AppointmentStatusDetailID
			
	
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'AppointmentStatusDetails', @AuditDetailID, @AppointmentStatusDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  	


	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END