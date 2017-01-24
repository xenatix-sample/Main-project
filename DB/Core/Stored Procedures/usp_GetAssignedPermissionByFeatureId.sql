-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAssignedPermissionByFeatureId]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get assigned permission details by Feature ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssignedPermissionByFeatureId]
	@FeatureID BIGINT,
	@RoleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			p.PermissionID,			
			p.Name,
			p.Description,
			CAST(CASE WHEN frp.FeatureRolePermissionID IS NULL THEN 0 ELSE 1 END AS BIT) AS Selected
		FROM 
			Core.Permission p 
			INNER JOIN Core.FeaturePermission fp ON 
			p.PermissionID=fp.PermissionID AND fp.FeatureID = @FeatureID
			LEFT JOIN Core.FeatureRolePermission frp ON 
			frp.FeatureID=fp.FeatureID AND frp.RoleID=@RoleID AND frp.IsActive = 1
			AND fp.PermissionID=frp.PermissionID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO


