-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetProgramsByProgramUnitDetailID]
-- Author:		Kyle Campbell
-- Date:		12/20/2016
--
-- Purpose:		Get Hierarchy Grid for Org Structure Program Unit screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/20/2016	Kyle Campbell	TFS #17998	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetProgramsByProgramUnitDetailID
	@DetailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT 
			V1.MappingID AS ProgramUnitMappingID,
			V1.DetailID AS ProgramUnitID, 
			V1.Name AS ProgramUnitName, 
			V2.DetailID AS ProgramID,
			V2.Name AS ProgramName, 
			V3.DetailID As DivisionID,
			V3.Name As DivisionName,
			V1.MappingEffectiveDate AS EffectiveDate, 
			V1.MappingExpirationDate AS ExpirationDate
		FROM Core.vw_GetOrganizationStructureDetails V1 
			LEFT JOIN Core.vw_GetOrganizationStructureDetails V2 ON V2.MappingID = V1.ParentID
			LEFT JOIN Core.vw_GetOrganizationStructureDetails V3 ON V3.MappingID = V2.ParentID
		WHERE V1.DetailID = @DetailID

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO
