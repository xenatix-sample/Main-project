-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateSetting]
-- Author:		Justin Spalti
-- Date:		08/03/2015
--
-- Purpose:		Updates a single setting via the Configuration Settings form
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2015	Justin Spalti		Task ID : 875 - Initial creation.
-- 08/04/2015 - John Crossen -- Data type mismatches resolved
-- 08/05/2015	Justin Spalti		Task ID : 875 - Updated the code to update the current manifest version for use with caching.
-- 09/24/2015 - Justin Spalti - Updated the procedure to make the update using the SettingValues' primary key.
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 02/17/2016	Scott Martin		Refactored for audit loggin
----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_UpdateSetting]
	@SettingValuesID INT,
	@SettingTypeID INT,
	@SettingValue NVARCHAR(50),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN TRY
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);	

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'SettingValues', @SettingValuesID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	UPDATE [Core].SettingValues
	SET Value = @SettingValue,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		SettingValuesID = @SettingValuesID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'SettingValues', @AuditDetailID, @SettingValuesID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	--update the manifestversion table if this is a non-user settingtype
	IF @SettingTypeID <> 3
		BEGIN
		UPDATE mv
		SET ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE() 
		FROM 
			[Core].ManifestVersion mv INNER JOIN (
				SELECT TOP 1 
					mv.ManifestVersionID, 
					mv.[Version], 
					mv.ModifiedOn
				FROM 
					[Core].ManifestVersion mv
				WHERE 
					mv.IsActive = 1
				ORDER BY 
					mv.ManifestVersionID
			) m
		ON mv.ManifestVersionID = m.ManifestVersionID
		END
END TRY
BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
		   @ResultMessage = ERROR_MESSAGE()
END CATCH