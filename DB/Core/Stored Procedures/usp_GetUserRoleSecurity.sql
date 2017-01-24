-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserRoleSecurity]
-- Author:		Rajiv Ranjan
-- Date:		07/28/2015
--
-- Purpose:		Get user's role permission
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/28/2015	Rajiv Ranjan		Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/28/2015 - Rajiv Ranjan	Added IsActive check
-- 1/5/2016		Scott Martin	Added distinct to select statement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserRoleSecurity]
	@UserID INT,	
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		
		SELECT DISTINCT
			m.ModuleID,
			m.Name AS ModuleName,
			f.FeatureID,
			f.Name As FeatureName
		FROM  
			Core.UserRole ur 
			INNER JOIN Core.[Role] r ON r.RoleID = ur.RoleID AND ur.UserID = @UserID
			INNER JOIN Core.RoleModule rm on rm.RoleID = r.RoleID
			INNER JOIN Core.Module m on m.ModuleID = rm.ModuleID
			INNER JOIN Core.ModuleFeature mf on mf.ModuleID = m.ModuleID
			INNER JOIN Core.Feature f on f.FeatureID = mf.FeatureID
		WHERE
			ur.IsActive = 1 AND
			r.IsActive = 1 AND
			rm.IsActive = 1 AND
			m.IsActive = 1 AND
			mf.IsActive = 1 AND
			f.IsActive = 1 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO