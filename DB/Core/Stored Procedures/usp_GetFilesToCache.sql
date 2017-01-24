-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetFilesToCache]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get the list of files to Cache
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Modification.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/05/2015 - Justin Spalti - Changed the inner join to a left join so that the default(null-roles) files can be retrieved for caching
-- 08/05/2015 - Justin Spalti - Updated the logic to get the version and the LastUpdated from the new ManifestVersion table
-- 07/07/2016 - Deepak Kumar  - Change xml concept suggested by Atul chauhan
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_GetFilesToCache]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		DECLARE @Version INT
		DECLARE @LastUpdatedOn DATETIME
		SELECT TOP 1 @Version=mv.[Version], @LastUpdatedOn=mv.ModifiedOn
						FROM [Core].ManifestVersion mv
						WHERE mv.IsActive = 1
						ORDER BY mv.ManifestVersionID

		SELECT ManifestID, FilePath, @Version [Version], SecurityRoleID, ModifiedBy, m.ModifiedOn, @LastUpdatedOn 'LastUpdatedOn', IsActive
		FROM Core.Manifest m
		WHERE
			m.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
