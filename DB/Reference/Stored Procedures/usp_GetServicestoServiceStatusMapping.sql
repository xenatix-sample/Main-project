

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServicestoServiceStatusMapping]
-- Author:		Sumana SAngapu
-- Date:		04/01/2016
--
-- Purpose:		Gets the list of ServicesStatus by Services
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/01/2016	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServicestoServiceStatusMapping]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

		SELECT so.ServicesID as ServiceID, s.ServiceStatusID, ServiceStatus
		FROM Reference.ServicestoServiceStatusMapping so 
		INNER JOIN Reference.[ServiceStatus]  s 
		ON so.ServiceStatusID = s.ServiceStatusID
		WHERE  so.IsActive = 1
		ORDER BY ServiceStatus ASC

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

