-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupSchedulingResource]
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
-- 03/11/2016	Justin Spalti - Initial creation.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetContactResourceByAppointmentID]
	@AppointmentID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully';

	BEGIN TRY	
		SELECT c.ContactID, c.FirstName, c.LastName,
			   c.IsActive, c.ModifiedBy, c.ModifiedOn, c.CreatedBy, c.CreatedOn
		FROM [Scheduling].[AppointmentResource] ar
		JOIN Registration.Contact c
			ON c.ContactID = ar.ResourceID
		WHERE ar.IsActive = 1 
			AND ar.AppointmentID = @AppointmentID
			AND ar.ResourceTypeID = 7; -- Contact
	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE();
	END CATCH;
END;