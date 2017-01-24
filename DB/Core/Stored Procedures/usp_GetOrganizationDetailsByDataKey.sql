-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationDetailsByDataKey]
-- Author:		Scott Martin
-- Date:		01/04/2017
--
-- Purpose:		Get Organization Details by DataKey
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Scott Martin	Initial creation
-- 01/11/2017	Scott Martin	Added sorting and only return modified data if different than created date
-- 01/17/2017	Scott Martin	Added a filter to exclude mappings that have expired
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetOrganizationDetailsByDataKey
	@DataKey NVARCHAR(50),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		DECLARE @Timezone NVARCHAR(50),
				@CurrentDate DATETIME;

		SELECT @Timezone = SV.Value FROM Core.Settings S INNER JOIN Core.SettingValues SV ON S.SettingsID = SV.SettingsID WHERE S.Settings = 'DefaultTimeZone';

		SELECT @CurrentDate = Tzdb.UtcToLocal(GETUTCDATE(), @Timezone);

		SELECT
			OD.DetailID,
			OD.Name,
			OD.Acronym,
			OD.Code,
			OD.IsActive,
			OD.IsExternal,
			CASE
				WHEN @DataKey = 'Division' THEN LTRIM(STUFF((SELECT DISTINCT ',' + OSD2.Name FROM Core.vw_GetOrganizationStructureDetails OSD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD2 ON OSD.ParentID = OSD2.MappingID WHERE OSD.DetailID = OD.DetailID AND DATEDIFF(DAY, ISNULL(OSD2.MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0 AND DATEDIFF(DAY, ISNULL(OSD2.MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0 ORDER BY ',' + OSD2.Name FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, ''))
				WHEN @DataKey = 'Program' THEN LTRIM(STUFF((SELECT DISTINCT ',' + OSD3.Name FROM Core.vw_GetOrganizationStructureDetails OSD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD2 ON OSD.ParentID = OSD2.MappingID INNER JOIN Core.vw_GetOrganizationStructureDetails OSD3 ON OSD2.ParentID = OSD3.MappingID WHERE OSD.DetailID = OD.DetailID AND DATEDIFF(DAY, ISNULL(OSD3.MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0 AND DATEDIFF(DAY, ISNULL(OSD3.MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0 ORDER BY ',' + OSD3.Name FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, ''))
				WHEN @DataKey = 'ProgramUnit' THEN LTRIM(STUFF((SELECT DISTINCT ',' + OSD4.Name FROM Core.vw_GetOrganizationStructureDetails OSD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD2 ON OSD.ParentID = OSD2.MappingID INNER JOIN Core.vw_GetOrganizationStructureDetails OSD3 ON OSD2.ParentID = OSD3.MappingID INNER JOIN Core.vw_GetOrganizationStructureDetails OSD4 ON OSD3.ParentID = OSD4.MappingID WHERE OSD.DetailID = OD.DetailID AND DATEDIFF(DAY, ISNULL(OSD4.MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0 AND DATEDIFF(DAY, ISNULL(OSD4.MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0 ORDER BY ',' + OSD4.Name FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, ''))
				ELSE NULL END AS Companies,
			CASE
				WHEN @DataKey = 'Program' THEN LTRIM(STUFF((SELECT DISTINCT ',' + OSD2.Name FROM Core.vw_GetOrganizationStructureDetails OSD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD2 ON OSD.ParentID = OSD2.MappingID WHERE OSD.DetailID = OD.DetailID AND DATEDIFF(DAY, ISNULL(OSD2.MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0 AND DATEDIFF(DAY, ISNULL(OSD2.MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0 ORDER BY ',' + OSD2.Name FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, ''))
				WHEN @DataKey = 'ProgramUnit' THEN LTRIM(STUFF((SELECT DISTINCT ',' + OSD3.Name FROM Core.vw_GetOrganizationStructureDetails OSD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD2 ON OSD.ParentID = OSD2.MappingID INNER JOIN Core.vw_GetOrganizationStructureDetails OSD3 ON OSD2.ParentID = OSD3.MappingID WHERE OSD.DetailID = OD.DetailID AND DATEDIFF(DAY, ISNULL(OSD3.MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0 AND DATEDIFF(DAY, ISNULL(OSD3.MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0 ORDER BY ',' + OSD3.Name FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, ''))
				ELSE NULL END AS Divisions,
			CASE
				WHEN @DataKey = 'ProgramUnit' THEN LTRIM(STUFF((SELECT DISTINCT ',' + OSD2.Name FROM Core.vw_GetOrganizationStructureDetails OSD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD2 ON OSD.ParentID = OSD2.MappingID WHERE OSD.DetailID = OD.DetailID AND DATEDIFF(DAY, ISNULL(OSD2.MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0 AND DATEDIFF(DAY, ISNULL(OSD2.MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0 ORDER BY ',' + OSD2.Name FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, ''))
				ELSE NULL END AS Programs,
			OD.EffectiveDate,
			OD.ExpirationDate,
			OD.CreatedBy,
			CONCAT(CBU.FirstName, ' ', CBU.LastName) AS CreatedByName,
			CASE
				WHEN OD.ModifiedOn = OD.CreatedOn THEN NULL
				ELSE OD.ModifiedBy END AS ModifiedBy,
			CASE
				WHEN OD.ModifiedOn = OD.CreatedOn THEN NULL
				ELSE OD.ModifiedOn END AS ModifiedOn,
			CASE
				WHEN OD.ModifiedOn = OD.CreatedOn THEN NULL
				ELSE CONCAT(MBU.FirstName, ' ', MBU.LastName) END AS ModifiedByName
		FROM
			Core.OrganizationDetails OD
			INNER JOIN Core.OrganizationAttributesMapping OAM
				ON OD.DetailID = OAM.DetailID
			INNER JOIN Core.OrganizationAttributes OA
				ON OAM.AttributeID = OA.AttributeID
			INNER JOIN Core.Users CBU
				ON OD.CreatedBy = CBU.UserID
			INNER JOIN Core.Users MBU
				ON OD.ModifiedBy = MBU.UserID
		WHERE
			OA.DataKey = @DataKey
		ORDER BY
			OD.ExpirationDate,
			OD.Name
		
  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO