-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrgStructurePrograms]
-- Author:		Kyle Campbell
-- Date:		12/20/2016
--
-- Purpose:		Get Program Unit Records for Program Units screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/20/2016	Kyle Campbell	TFS #17998	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetOrgStructureProgramUnits
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		SELECT 
			OD.DetailID AS ProgramUnitID, 
			OD.Name AS ProgramUnitName, 
			OD.Acronym, 
			OD.Code,
			LTRIM(STUFF((SELECT DISTINCT ',' + P.Name FROM Core.vw_GetOrganizationStructureDetails PU INNER JOIN Core.vw_GetOrganizationStructureDetails P ON PU.ParentID = P.MappingID WHERE PU.DetailID = OD.DetailID AND PU.DataKey = 'ProgramUnit' FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS Programs,
			LTRIM(STUFF((SELECT DISTINCT ',' + D.Name FROM Core.vw_GetOrganizationStructureDetails PU INNER JOIN Core.vw_GetOrganizationStructureDetails P ON PU.ParentID = P.MappingID INNER JOIN Core.vw_GetOrganizationStructureDetails D ON P.ParentID = D.MappingID WHERE PU.DetailID = OD.DetailID AND PU.DataKey = 'ProgramUnit' FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS Divisions,
			OD.EffectiveDate,
			OD.ExpirationDate,
			OD.CreatedBy, 
			OD.ModifiedBy, 
			OD.ModifiedOn
		FROM
			Core.OrganizationDetails OD
			INNER JOIN Core.OrganizationAttributesMapping OAM
			ON OD.DetailID = OAM.DetailID
			INNER JOIN Core.OrganizationAttributes OA
			ON OAM.AttributeID = OA.AttributeID
		WHERE
			OD.IsActive = 1
			AND OA.DataKey = 'ProgramUnit'
		ORDER BY
			OD.ModifiedON DESC

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO

