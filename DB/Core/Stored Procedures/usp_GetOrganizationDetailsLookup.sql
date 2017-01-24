-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationDetailsLookup]
-- Author:		Scott Martin
-- Date:		01/06/2017
--
-- Purpose:		Get lookup information for Organization Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/06/2017	Scott Martin	Initial creation
-- 01/17/2017	Scott Martin	Added a filter to exclude mappings that have expired
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetOrganizationDetailsLookup
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
			OA.DataKey
		FROM
			Core.OrganizationDetails OD
			INNER JOIN Core.OrganizationAttributesMapping OAM
				ON OD.DetailID = OAM.DetailID
			INNER JOIN Core.OrganizationAttributes OA
				ON OAM.AttributeID = OA.AttributeID
		WHERE
			OD.IsActive = 1
			AND DATEDIFF(DAY, ISNULL(OD.EffectiveDate, '1/1/1900'), @CurrentDate) >= 0
			AND DATEDIFF(DAY, ISNULL(OD.ExpirationDate, '12/31/2099'), @CurrentDate) <= 0
		ORDER BY
			OA.DataKey,
			OD.Name
		
  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO