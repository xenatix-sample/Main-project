-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetModuleByRoleId]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get Module details by Role ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_GetModuleByRoleId]
	@RoleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			rm.RoleModuleID,
			r.RoleID,
			r.Name AS RoleName,
			rm.RoleModuleID,
			m.ModuleID,
			m.Name AS ModuleName,
			CAST(CASE WHEN rm.RoleModuleID IS NULL THEN 0 ELSE 1 END AS BIT) AS Selected
		FROM 
			Core.Module m 
			LEFT JOIN Core.RoleModule rm ON m.ModuleID=rm.ModuleID AND rm.RoleID=@RoleID	AND rm.IsActive = 1
			LEFT JOIN Core.Role r ON rm.RoleID=r.RoleID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO