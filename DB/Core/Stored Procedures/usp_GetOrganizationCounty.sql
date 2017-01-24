-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationCounty]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of Call Status 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/19/2016	John Crossen -   TFS # 5505     Initial creation
-- 04/13/2016	Scott Martin	Changed ProgramUnitID to OrganizationDetailID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.[usp_GetOrganizationCounty]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		OSD.MappingID AS OrganizationID,
		C.CountyID,
		C.CountyName,
		SP.StateProvinceName 
	FROM
		Core.OrganizationCounty OC
		INNER JOIN Core.vw_GetOrganizationStructureDetails OSD
			ON OC.DetailID = OSD.DetailID
		LEFT OUTER JOIN Reference.County C
			ON C.CountyID = OC.CountyID
		LEFT OUTER JOIN Reference.StateProvince SP
			ON C.StateProvinceID=SP.StateProvinceID
	WHERE
		OC.IsActive = 1
		AND C.IsActive = 1
		AND SP.IsActive = 1

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO