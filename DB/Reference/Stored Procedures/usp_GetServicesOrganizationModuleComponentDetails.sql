-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServicesOrganizationModuleComponentDetails]
-- Author:		Kyle Campbell	
-- Date:		01/19/2017
--
-- Purpose:		Gets the list of Services for service screens
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/19/2017	Kyle Campbell	TFS #14007	Initial creation
-- 01/23/2017	Kyle Campbell	TFS #22107	Modified to not return expired Services on service screens
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetServicesOrganizationModuleComponentDetails]
	@DetailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT DISTINCT
		v.MappingID AS OrganizationID,
		v.DetailID,
		v.Name As Name,
		SMC.ServicesID AS ServiceID,
		S.ServiceName,
		MC.DataKey
	FROM
		Reference.ServicesOrganizationDetails SOD
		INNER JOIN Reference.Services S
			ON SOD.ServicesID = S.ServicesID
		INNER JOIN Core.vw_GetOrganizationStructureDetails v
			ON SOD.DetailID = v.DetailID
		INNER JOIN Reference.ServicesModuleComponent SMC ON S.ServicesID = SMC.ServicesID
		INNER JOIN Core.ModuleComponent MC ON SMC.ModuleComponentID = MC.ModuleComponentID
		INNER JOIN Core.OrganizationDetailsModuleComponent ODMC ON SMC.ModuleComponentID = ODMC.ModuleComponentID AND SOD.DetailID = ODMC.DetailID
	WHERE 
		CASE WHEN IsNull(S.ExpirationDate, '12-31-2999') < CONVERT(DATE, GETDATE(), 101) THEN 1 ELSE 0 END = 0 
		AND CASE WHEN IsNull(SOD.ExpirationDate, '12-31-2999') < CONVERT(DATE, GETDATE(), 101) THEN 1 ELSE 0 END = 0
		AND SMC.IsActive = 1 
		AND SOD.IsActive = 1	
		AND ODMC.IsActive = 1
		AND v.DetailID = @DetailID
	ORDER BY v.Name, S.ServiceName
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

