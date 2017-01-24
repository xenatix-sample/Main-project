-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationDetailsMappingByDetailID]
-- Author:		Scott Martin
-- Date:		01/05/2017
--
-- Purpose:		Get Organization Details Mappings by Detail 
--
-- Notes:		The default behavior of this is to return all of the parent records (all records above) for an Organization Detail.
--				To return child records, it is necessary to pass in the DataKey of the level below the Organization Detail
--				Example: If Program Detail ID is passed in, the proc will return the Division and Company the Program is mapped to but
--						 if you want to see Program Units the DataKeyFilter will need to be filled in with ProgramUnit
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Scott Martin	Initial Creation
-- 01/06/2017	Scott Martin	Modified proc to return DetailIDs for Company, Division, Program, and Program Unit
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetOrganizationDetailsMappingByDetailID
	@DetailID BIGINT,
	@DataKeyFilter NVARCHAR(50) = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		IF @DataKeyFilter IS NULL
			BEGIN
			SELECT @DataKeyFilter = DataKey FROM Core.vw_GetOrganizationStructureDetails WHERE DetailID = @DetailID;
			END

		SELECT
			OSD1.MappingID,
			OSD1.MappingEffectiveDate AS EffectiveDate,
			OSD1.MappingExpirationDate AS ExpirationDate,
			CASE
				WHEN OSD1.DataKey = 'Company' THEN OSD1.DetailID
				WHEN OSD2.DataKey = 'Company' THEN OSD2.DetailID
				WHEN OSD3.DataKey = 'Company' THEN OSD3.DetailID
				WHEN OSD4.DataKey = 'Company' THEN OSD4.DetailID
				ELSE NULL END AS CompanyID,
			CASE
				WHEN OSD1.DataKey = 'Company' THEN OSD1.Name
				WHEN OSD2.DataKey = 'Company' THEN OSD2.Name
				WHEN OSD3.DataKey = 'Company' THEN OSD3.Name
				WHEN OSD4.DataKey = 'Company' THEN OSD4.Name
				ELSE NULL END AS CompanyName,
			CASE
				WHEN OSD1.DataKey = 'Division' THEN OSD1.DetailID
				WHEN OSD2.DataKey = 'Division' THEN OSD2.DetailID
				WHEN OSD3.DataKey = 'Division' THEN OSD3.DetailID
				WHEN OSD4.DataKey = 'Division' THEN OSD4.DetailID
				ELSE NULL END AS DivisionID,
			CASE
				WHEN OSD1.DataKey = 'Division' THEN OSD1.Name
				WHEN OSD2.DataKey = 'Division' THEN OSD2.Name
				WHEN OSD3.DataKey = 'Division' THEN OSD3.Name
				WHEN OSD4.DataKey = 'Division' THEN OSD4.Name
				ELSE NULL END AS DivisionName,
			CASE
				WHEN OSD1.DataKey = 'Program' THEN OSD1.DetailID
				WHEN OSD2.DataKey = 'Program' THEN OSD2.DetailID
				WHEN OSD3.DataKey = 'Program' THEN OSD3.DetailID
				WHEN OSD4.DataKey = 'Program' THEN OSD4.DetailID
				ELSE NULL END AS ProgramID,
			CASE
				WHEN OSD1.DataKey = 'Program' THEN OSD1.Name
				WHEN OSD2.DataKey = 'Program' THEN OSD2.Name
				WHEN OSD3.DataKey = 'Program' THEN OSD3.Name
				WHEN OSD4.DataKey = 'Program' THEN OSD4.Name
				ELSE NULL END AS ProgramName,
			CASE
				WHEN OSD1.DataKey = 'ProgramUnit' THEN OSD1.DetailID
				WHEN OSD2.DataKey = 'ProgramUnit' THEN OSD2.DetailID
				WHEN OSD3.DataKey = 'ProgramUnit' THEN OSD3.DetailID
				WHEN OSD4.DataKey = 'ProgramUnit' THEN OSD4.DetailID
				ELSE NULL END AS ProgramUnitID,
			CASE
				WHEN OSD1.DataKey = 'ProgramUnit' THEN OSD1.Name
				WHEN OSD2.DataKey = 'ProgramUnit' THEN OSD2.Name
				WHEN OSD3.DataKey = 'ProgramUnit' THEN OSD3.Name
				WHEN OSD4.DataKey = 'ProgramUnit' THEN OSD4.Name
				ELSE NULL END AS ProgramUnitName
		FROM
			Core.vw_GetOrganizationStructureDetails OSD1
			LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD2
				ON OSD1.ParentID = OSD2.MappingID
			LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD3
				ON OSD2.ParentID = OSD3.MappingID
			LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD4
				ON OSD3.ParentID = OSD4.MappingID
		WHERE
			OSD1.DataKey = @DataKeyFilter
			AND
			(
				OSD1.DetailID = @DetailID
				OR OSD2.DetailID = @DetailID
				OR OSD3.DetailID = @DetailID
				OR OSD4.DetailID = @DetailID
			)
  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO