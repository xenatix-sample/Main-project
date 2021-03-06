-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSettingsbyType]
-- Author:		Sumana Sangapu
-- Date:		08/03/2015
--
-- Purpose:		Gets the Setting Values by SettingsType. Inputs parameter @SettingsTypeID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2015	Sumana Sangapu		Task ID : 875 - Initial creation.
-- 08/03/2015   Justin Spalti       Task ID : 875 - Added a value to the where clause to only retrieve settings that are configurable
-- 08/03/2015   Justin Spalti       Task ID : 875 - Updated the schema of the SettingsType and Settings tables to Core.
-- 08/04/2015 - John Crossen -- Data type mismatches resolved
-- 09/04/2015   Justin Spalti - Added logic to decide whether the setting's value should be displayed on the UI

-- exec [Core].[usp_GetSettingsByType] '1,3,34,5','',''
----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_GetSettingsByType] 
@SettingsTypeID NVARCHAR(500),
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
				WITH CTE AS ( 
						SELECT row_number() over (partition by  sv.SettingsID,sv.SettingsTypeID order by sv.ModifiedOn desc) RN,
							   sv.[SettingsID] AS SettingsID
							  ,st.[SettingsTypeID] AS SettingsTypeID
							  ,sv.[SettingValuesID] AS SettingValuesID
							  ,s.[Settings] AS Settings
							  ,st.[SettingsType] AS SettingsType
							  ,CASE WHEN s.IsDisplayed = 0 THEN '*****' ELSE sv.[Value] END AS Value
							  ,sv.[EntityID] AS EntityID
							  ,sv.[ModifiedBy] AS ModifiedBy
							  ,sv.[ModifiedOn] AS ModifiedOn
						  FROM [Core].[SettingValues] sv
						  LEFT JOIN [Core].[Settings] s 
						  ON sv.[SID] = s.[SID]
						  LEFT JOIN [Core].[SettingsType] st
						  ON  sv.SettingsTypeID = st.SettingsTypeID
						  WHERE sv.SettingsTypeID IN (SELECT Items FROM [Core].[fn_Split](@SettingsTypeID,',') )
						  AND	sv.IsActive = 1
						  AND s.IsConfigurable = 1 
						) 
				SELECT  SettingsID, SettingsTypeID, SettingValuesID, Settings, SettingsType, Value, EntityID,
						ModifiedBy, ModifiedOn
				FROM    CTE
				WHERE   RN = 1 
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END