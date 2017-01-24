-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetOrgStructureDetailsByUserID
-- Author:		Scott Martin
-- Date:		05/18/2016
--
-- Purpose:		Get the Org Structure for a User in an unflattened format
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/18/2016	Scott Martin	Initial Creation 
-- 05/23/2016	Scott Martin	Added DataKey to select query
-- 01/17/2017	Scott Martin	Added a filter to exclude mappings that have expired
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetOrgStructureDetailsByUserID] 
	@UserID nvarchar(50),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'
	BEGIN TRY
	DECLARE @Timezone NVARCHAR(50),
			@CurrentDate DATETIME;

	SELECT @Timezone = SV.Value FROM Core.Settings S INNER JOIN Core.SettingValues SV ON S.SettingsID = SV.SettingsID WHERE S.Settings = 'DefaultTimeZone';

	SELECT @CurrentDate = Tzdb.UtcToLocal(GETUTCDATE(), @Timezone);

	;WITH UORG (MappingID, DetailID, ParentID, DataKey)
	AS
	(
		SELECT
			OSD.MappingID,
			OSD.DetailID,
			OSD.ParentID,
			OSD.DataKey
		FROM
			Core.UserOrganizationDetailsMapping UODM
			INNER JOIN Core.vw_GetOrganizationStructureDetails OSD
				ON UODM.MappingID = OSD.MappingID
		WHERE
			UODM.IsActive = 1
			AND OSD.DataKey = 'ProgramUnit'
			AND UODM.UserID = @UserID
			AND DATEDIFF(DAY, ISNULL(MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0
			AND DATEDIFF(DAY, ISNULL(MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0
		UNION ALL
		SELECT
			OSD.MappingID,
			OSD.DetailID,
			OSD.ParentID,
			OSD.DataKey
		FROM
			UORG
			INNER JOIN Core.vw_GetOrganizationStructureDetails OSD
				ON UORG.ParentID = OSD.MappingID
		WHERE
			DATEDIFF(DAY, ISNULL(MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0
			AND DATEDIFF(DAY, ISNULL(MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0
	)
	SELECT DISTINCT
		UORG.MappingID,
		UORG.DetailID,
		UORG.ParentID,
		UORG.DataKey
	FROM
		UORG
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END