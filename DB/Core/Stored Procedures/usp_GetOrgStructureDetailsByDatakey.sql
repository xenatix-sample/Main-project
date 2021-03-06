-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetOrgStructureDetailsByDatakey
-- Author:		Sumana Sangapu
-- Date:		03/27/2016
--
-- Purpose:		Get the Org details by Datakey. 
--				To get list of any attribute pass PArentMappingID as NULL.
--				To get the list of cascading details pass datakey and correcponding ParentMappingID
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/27/2016 - Sumana Sangapu	Initial Creation 
-- 01/17/2017	Scott Martin	Added a filter to exclude mappings that have expired
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetOrgStructureDetailsByDatakey] 
	@DataKey nvarchar(50),
	@ParentMappingID INT = NULL,
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
		
		IF @ParentMappingID IS NULL
		BEGIN
				SELECT
					MappingID,
					DetailID,
					Name,
					ParentID,
					AttributeID,
					AttributeName,
					DataKey,
					IsExternal
				FROM
					[Core].[vw_GetOrganizationStructureDetails]
				WHERE
					DataKey = @DataKey
					AND DATEDIFF(DAY, ISNULL(MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0
					AND DATEDIFF(DAY, ISNULL(MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0

		END
		ELSE -- Get the cascading org details
		BEGIN
				SELECT
					MappingID,
					DetailID,
					Name,
					ParentID,
					AttributeID,
					AttributeName,
					DataKey 
				FROM
					[Core].[vw_GetOrganizationStructureDetails]
				WHERE
					DataKey = @DataKey 
					AND ParentID = @ParentMappingID
					AND DATEDIFF(DAY, ISNULL(MappingEffectiveDate, '1/1/1900'), @CurrentDate) >= 0
					AND DATEDIFF(DAY, ISNULL(MappingExpirationDate, '12/31/2099'), @CurrentDate) <= 0
		END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

