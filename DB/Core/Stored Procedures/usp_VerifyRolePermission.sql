-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_VerifyRolePermission]
-- Author:		Rajiv Ranjan
-- Date:		07/30/2015
--
-- Purpose:		Verify wether use have module/feature's permissiosn or not
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/30/2015	Rajiv Ranjan	Initial creation.
-- 08/03/2015 - John Crossen	Change from dbo to Core schema
-- 08/28/2015 - Rajiv Ranjan	Added IsActive check
-- 09/01/2015 - Rajiv Ranjan	Added IsActive check for module permissions
-- 05/19/2016 - Rajiv Ranjan	Refactored proc
-- 06/22/2016 - Lokesh Singhal	Replace , with | as it is split with | instead of ,
-- 06/30/2016 - Rajiv Ranjan	Verify permission for company and PU level
-- 10/12/2016 - Arun Choudhary	Refactored proc
-- 10/27/2016	Scott Martin	Refactored query to improve performance
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_VerifyRolePermission]
	@UserID INT,
	@Modules NVARCHAR(MAX),
	@DataKey NVARCHAR(MAX),
	@ActionName NVARCHAR(250),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		DECLARE @MTable TABLE
		(
			Items VARCHAR(MAX)
		);

		INSERT INTO @MTable
		SELECT Items FROM [Core].[fn_Split](@Modules,'|');

		SELECT 
			@DataKey = COALESCE(@DataKey + '|', '') + CAST(DataKey AS VARCHAR(MAX)) 
		FROM 
			Core.Module M 
			INNER JOIN Core.ModuleComponent MC
				ON M.ModuleID = MC.ModuleID
			INNER JOIN @MTable MT
				ON M.Name = MT.Items 
	
		DECLARE @ATable TABLE
		(
			Items VARCHAR(MAX)
		);

		INSERT INTO @ATable
		SELECT Items FROM [Core].[fn_Split](@ActionName,'|');

		DECLARE @DTable TABLE
		(
			Items VARCHAR(MAX)
		);

		INSERT INTO @DTable
		SELECT Items FROM [Core].[fn_Split](@DataKey,'|');

			SELECT 
				DISTINCT
				CAST(1 AS BIT) Result
			FROM
				Core.UserRole UR
				INNER JOIN Core.RoleModule RM
					ON UR.RoleID = RM.RoleID
				INNER JOIN Core.Module M
					ON M.ModuleID = RM.ModuleID
				INNER JOIN Core.RoleModulePermission RMP
					ON RMP.RoleModuleID = RM.RoleModuleID
				INNER JOIN Core.Permission P 
					ON P.PermissionID = RMP.PermissionID
				INNER JOIN @MTable MT
					ON M.Name = MT.Items
				INNER JOIN @ATable ATB
					ON P.Name = ATB.Items
			WHERE
				RM.IsActive = 1
				AND UR.UserID = @UserID
				AND UR.IsActive=1
				AND P.IsActive=1
				AND RMP.PermissionLevelID IS NOT NULL

			UNION ALL

			SELECT 
				DISTINCT
				CAST(1 AS BIT) Result
			FROM
				Core.UserRole UR
				INNER JOIN Core.RoleModule RM
					ON UR.RoleID = RM.RoleID
				INNER JOIN Core.RoleModuleComponent RMC
					ON RM.RoleModuleID = RMC.RoleModuleID
				INNER JOIN Core.ModuleComponent MC
					ON RMC.ModuleComponentID = MC.ModuleComponentID
				INNER JOIN Core.RoleModuleComponentPermission RMCP
					ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
				INNER JOIN Core.Permission P 
					ON P.PermissionID = RMCP.PermissionID
				INNER JOIN @DTable DT
					ON MC.DataKey = DT.Items
				INNER JOIN @ATable ATB
					ON P.Name = ATB.Items
			WHERE
				RM.IsActive = 1
				AND RMC.IsActive = 1
				AND RMCP.IsActive = 1
				AND UR.UserID = @UserID
				AND UR.IsActive=1
				AND P.IsActive=1
				AND MC.IsActive=1
				AND RMCP.PermissionLevelID IS NOT NULL
		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO