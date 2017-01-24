-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetFeatureByModuleId]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get feature details by Module ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetFeatureByModuleId]
	@ModuleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,  
     @ResultMessage = 'executed successfully'  
  
 BEGIN TRY   
		SELECT   
			mf.ModuleFeatureID,  
			m.ModuleID,  
			m.Name AS ModuleName,  
			f.FeatureID,  
			f.Name As FeatureName,  
			f.Description,
			f.ParentFeatureID   
		FROM   
			Core.Feature f  
			INNER JOIN Core.ModuleFeature mf ON f.FeatureID = mf.FeatureID
			INNER JOIN Core.Module m ON mf.ModuleID=m.ModuleID  
		WHERE   
			m.ModuleID=@ModuleID    
 END TRY  
 BEGIN CATCH  
  SELECT @ResultCode = ERROR_SEVERITY(),  
      @ResultMessage = ERROR_MESSAGE()  
 END CATCH 
END

