-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationLookup]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of Orgs for lookup 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/19/2016	John Crossen -   TFS # 6024     Initial creation
-- 03/28/2016	Scott Martin	Refactored query to use a base view
-- 04/20/2016   Justin Spalti - Ordered the results set by DataKey and Name
-- 01/17/2017	Scott Martin	Added a filter to exclude mappings that have expired
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetOrganizationLookup
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
		OSD.MappingID,
		OSD.Name,
		OSD.ParentID,
		OSD.DataKey
	FROM
		Core.vw_GetOrganizationStructureDetails OSD
	WHERE
		DATEDIFF(DAY, ISNULL(OSD.MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0
		AND DATEDIFF(DAY, ISNULL(OSD.MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0
	ORDER BY OSD.DataKey, OSD.Name
  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO