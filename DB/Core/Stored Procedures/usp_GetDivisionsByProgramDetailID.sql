-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetDivisionsByProgramDetailID]
-- Author:		Kyle Campbell
-- Date:		12/20/2016
--
-- Purpose:		Get Hierarchy Grid for Org Structure Program screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/20/2016	Kyle Campbell	TFS #17998	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetDivisionsByProgramDetailID
	@DetailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT 
			V1.MappingID, 
			V1.DetailID, 
			V1.Name AS ProgramName, 
			V2.Name AS DivisionName, 
			V2.MappingEffectiveDate, 
			V2.MappingExpirationDate
		FROM Core.vw_GetOrganizationStructureDetails V1 
			LEFT JOIN Core.vw_GetOrganizationStructureDetails V2 ON V1.ParentID = V2.MappingID
		WHERE V1.DetailID = @DetailID

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO
