-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentTypeServicesMapping]
-- Author:		Sumana SAngapu
-- Date:		04/14/2016
--
-- Purpose:		Gets the list of Services  by AppointmentTypes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/14/2016	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetAppointmentTypeServicesMapping]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

		SELECT a.ServicesID as ServiceID, s.ServiceName, a.AppointmentTypeID
		FROM Reference.AppointmentTypeServicesMapping a 
		INNER JOIN Reference.ServicesModuleMapping sm
		ON a.ServicesID = sm.ServicesID
		INNER JOIN Reference.[Services]  s 
		ON a.ServicesID = s.ServicesID
		WHERE	sm.ModuleID = 5 -- Scheduling
		AND		a.IsActive = 1
		ORDER BY ServiceName ASC

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END