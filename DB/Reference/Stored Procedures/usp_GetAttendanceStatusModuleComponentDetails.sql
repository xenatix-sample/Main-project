-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAttendanceStatusModuleComponentDetails]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of AttendanceStatus and  Services ModuleComponentID mappings for Service Recording screens
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2017	Kyle Campbell	TFS #14007	Initial Creation
-- 01/24/2017	Kyle Campbell	TFS #14007	Added IsActive in WHERE clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetAttendanceStatusModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT ASMC.AttendanceStatusID, 
			AttendanceStatus,
			ASMC.ServicesID,
			ASMC.ModuleComponentID,
			ASMC.IsActive,
			ASMC.ModifiedBy,
			ASMC.ModifiedOn,
			ASMC.IsActive  
		FROM Reference.AttendanceStatusModuleComponent ASMC
			INNER JOIN Reference.[AttendanceStatus] [AS] ON ASMC.AttendanceStatusID = [AS].AttendanceStatusID
		WHERE [AS].IsActive = 1 
			AND ASMC.IsActive = 1
		ORDER BY AttendanceStatus ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

