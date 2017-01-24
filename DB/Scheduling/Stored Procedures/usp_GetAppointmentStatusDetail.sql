

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentStatusDetail]
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

CREATE PROCEDURE [Scheduling].usp_GetAppointmentStatusDetail
	
	@AppointmentResourceID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	

	SELECT 
	AppointmentStatusDetailID,
	AppointmentResourceID,
	AppointmentStatusID,
	StartDateTime,
	IsCancelled, CancelReasonID,Comments
	FROM [Scheduling].[AppointmentStatusDetails]
	WHERE IsActive=1  AND AppointmentResourceID=@AppointmentResourceID
  	


	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END