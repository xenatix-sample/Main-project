-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAttendanceStatussByServicesModuleComponentID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of AttendanceStatus based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetAttendanceStatusByServicesModuleComponentID]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT ASMC.AttendanceStatusID, 
			AttendanceStatus,
			ASMC.IsActive,
			ASMC.ModifiedBy,
			ASMC.ModifiedOn  
		FROM Reference.AttendanceStatusModuleComponent ASMC
			INNER JOIN Reference.[AttendanceStatus] [AS] ON ASMC.AttendanceStatusID = [AS].AttendanceStatusID
		WHERE ASMC.ServicesID = @ServicesID AND ASMC.ModuleComponentID = @ModuleComponentID			
			AND ASMC.IsActive = 1
			AND [AS].IsActive = 1
		ORDER BY AttendanceStatus ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
