
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddAppointmentStatusDetail]
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
-- 03/17/2016	Karl Jablonski	Update to return generic ID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_AddAppointmentStatusDetail]
	@AppointmentResourceID BIGINT,
	@AppointmentStatusID INT,
	@IsCancelled BIT,	
	@CancelReasonID int,
	@Comments nvarchar(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	
			INSERT INTO Scheduling.AppointmentStatusDetails
			(
			AppointmentResourceID,
			AppointmentStatusID,
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
			VALUES(
			@AppointmentResourceID,
			@AppointmentStatusID,
			@ModifiedOn,
			@IsCancelled,
			@CancelReasonID,
			@Comments,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn)


			SELECT @ID=SCOPE_IDENTITY()

			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentStatusDetails', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentStatusDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			



	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END