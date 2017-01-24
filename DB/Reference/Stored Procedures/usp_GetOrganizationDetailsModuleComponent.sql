
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetOrganizationDetailsModuleComponent]
-- Author:		Kyle Campbell	
-- Date:		01/13/2017
--
-- Purpose:		Gets the list of program units for service screens
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/13/2017	Kyle Campbell	TFS #14007	Initial creation
-- 01/16/2017	Vishal Yadav	Implement Service configuration on all service screens
-- 01/16/2017	Kyle Campbell	TFS #14007	Changed source of OrganizationID column from DetailID to MappingID, added DetailID column
-- 01/18/2017	Atul Chauhan	TFS #14007	Reference services join plus some it wasn't bringing all the needed Organisations
-- 01/19/2017	Vishal Yadav	TFS #14007	Proc will return all the values even if it is inactive and added IsActive as return parameter
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetOrganizationDetailsModuleComponent]
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
			MC.DataKey,
			ODMC.IsActive
		FROM
			Core.vw_GetOrganizationStructureDetails v 
			INNER JOIN Core.OrganizationDetailsModuleComponent ODMC ON V.DetailID = ODMC.DetailID
			INNER JOIN Core.ModuleComponent MC ON ODMC.ModuleComponentID = MC.ModuleComponentID
	ORDER BY v.Name
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END