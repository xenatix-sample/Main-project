-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetFeatureByRoleId]
-- Author:		Rajiv Ranjan
-- Date:		08/16/2015
--
-- Purpose:		Get feature details by Role ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/19/2015	Rajiv Ranjan		Initial creation.
-- 03/28/2016 - Justin Spalti -- Added IsActive to the where clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetFeatureByRoleId]
	@RoleID   BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,  
     @ResultMessage = 'executed successfully'  
  
 BEGIN TRY   
		SELECT   
			mf.ModuleFeatureID,  
			rm.RoleModuleID,  
			rm.RoleID,  
			rm.ModuleID,
			f.FeatureID,  
			f.Name As FeatureName,  
			f.Description,
			f.ParentFeatureID   
		FROM   
			Core.Feature f  
			INNER JOIN Core.ModuleFeature mf ON f.FeatureID = mf.FeatureID
			INNER JOIN Core.RoleModule rm ON mf.ModuleID=rm.ModuleID  
		WHERE   
			rm.RoleID = @RoleID    
			AND f.IsActive = 1
			AND mf.IsActive = 1
 END TRY  
 BEGIN CATCH  
  SELECT @ResultCode = ERROR_SEVERITY(),  
      @ResultMessage = ERROR_MESSAGE()  
 END CATCH 
END