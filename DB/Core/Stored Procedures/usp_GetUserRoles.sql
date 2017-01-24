-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserRoles
-- Author:		Justin Spalti
-- Date:		07/21/2015
--
-- Purpose:		Gets a list of users and their assigned roles
--
-- Notes:		The procedure will return all of the active roles, but only the ones assigned to a user will have a non-zero UserID
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015 - Add the procedure header
-- 07/23/2015 - Added dbo to the table name
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/04/2015 - John Crossen -- Data type mismatches resolved
-- 02/15/2016 - Justin Spalti - Added HasRole to the results set for use on the user roles screen
-- 06/13/2016	Scott Martin	Refactored the return query to order the list based on assigned permissions
-- 09/09/2016 - Gaurav Gupta 14440 Remove RMP.PermissionLevelID is not null check in joins.
-- 01/18/2017 - Karl Jablonski	Bug#21590: Return only those roles with valid, current effective/expiration dates
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserRoles]
@UserID INT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'User roles loaded successfully'

	DECLARE @Timezone NVARCHAR(50), @CurrentDate DATETIME; 
    SELECT @Timezone = SV.Value FROM Core.Settings S INNER JOIN Core.SettingValues SV ON S.SettingsID = SV.SettingsID WHERE S.Settings = 'DefaultTimeZone'; 
    SELECT @CurrentDate = Tzdb.UtcToLocal(GETUTCDATE(), @Timezone);

	BEGIN TRY	
		IF OBJECT_ID('tempdb..#tmpUserRoles') IS NOT NULL
			DROP TABLE #tmpUserRoles

		CREATE TABLE #tmpUserRoles(tmpID INT PRIMARY KEY IDENTITY (1,1), UserRoleID INT, UserID INT, RoleID BIGINT, Name NVARCHAR(250), [Description] NVARCHAR(1000), IsActive bit, HasRole BIT, RoleGUID NVARCHAR(500))
	
		INSERT INTO #tmpUserRoles(UserRoleID, UserID, RoleID, Name, [Description], IsActive, HasRole, RoleGUID)
		SELECT ur.UserRoleID, u.UserID, r.RoleID, r.Name, r.[Description], ur.IsActive, 1, r.RoleGUID
		FROM Core.[Users] u
	    JOIN Core.UserRole ur
			ON ur.UserID = u.UserID
		JOIN Core.[Role] r
			ON r.RoleID = ur.RoleID
 		WHERE u.UserID = @UserID
			AND ur.IsActive = 1
			AND r.IsActive = 1
			AND DATEDIFF(DAY, ISNULL(r.EffectiveDate, '1/1/1900'), @CurrentDate) >= 0
            AND DATEDIFF(DAY, ISNULL(r.ExpirationDate, '12/31/2099'), @CurrentDate) <= 0

		INSERT INTO #tmpUserRoles(UserRoleID, UserID, RoleID, Name, [Description], IsActive, HasRole, RoleGUID) 
		SELECT 0, 0, r.RoleID, r.Name, r.[Description], 1, 0, r.RoleGUID
		FROM Core.[Role] r
		LEFT JOIN #tmpUserRoles t
			ON t.RoleID = r.RoleID
		WHERE t.RoleID IS NULL
			AND r.IsActive = 1
			AND DATEDIFF(DAY, ISNULL(r.EffectiveDate, '1/1/1900'), @CurrentDate) >= 0
            AND DATEDIFF(DAY, ISNULL(r.ExpirationDate, '12/31/2099'), @CurrentDate) <= 0

		SELECT
			UserRoleID,
			UserID,
			tUR.RoleID,
			Name,
			[Description],
			tUR. IsActive,
			HasRole,
			RoleGUID
		FROM
			#tmpUserRoles tUR
			INNER JOIN Core.RoleModule RM
				ON tUR.RoleID = RM.RoleID
				AND RM.IsActive = 1
			INNER JOIN Core.RoleModulePermission RMP
				ON RM.RoleModuleID = RMP.RoleModuleID
				AND RMP.IsActive = 1
			INNER JOIN Core.RoleModuleComponent RMC
				ON RM.RoleModuleID = RMC.RoleModuleID
				AND RMC.IsActive = 1
			INNER JOIN Core.RoleModuleComponentPermission RMCP
				ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
				AND RMCP.IsActive = 1
				AND RMCP.PermissionLevelID IS NOT NULL
		GROUP BY
			UserRoleID,
			UserID,
			tUR.RoleID,
			Name,
			[Description],
			tUR. IsActive,
			HasRole,
			RoleGUID
		ORDER BY
		    tUR.HasRole DESC,
			Name ASC

		DROP TABLE #tmpUserRoles
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END