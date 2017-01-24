-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSettingsToCache]
-- Author:		Justin Spalti
-- Date:		08/04/2015
--
-- Purpose:		Gets all of the settings that can be cached. Various settings could be overwritten by a user setting
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/04/2015   Justin Spalti       Task ID : 875 - Initial creation.
-- 08/04/2015   Justin Spalti       Task ID : 875 - Updated the where clause to match correctly when EntityID is null.
----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_GetSettingsToCache]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT s.SettingsID, s.Settings, sv.Value, sv.EntityID, st.SettingsTypeID, st.SettingsType
		FROM [Core].Settings s
		JOIN [Core].SettingValues sv
			ON sv.[SID] = s.[SID]
		JOIN [Core].SettingsType st
			ON st.SettingsTypeID = sv.SettingsTypeID
		WHERE s.IsCachable = 1
			AND s.IsActive = 1
			AND sv.IsActive = 1
			AND st.IsActive = 1
			AND ISNULL(sv.EntityID, 0) = CASE WHEN ST.SettingsTypeID = 3 THEN @UserID ELSE ISNULL(sv.EntityID, 0) END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH						
END

