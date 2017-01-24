----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetProviders]
-- Author:		Scott Martin
-- Date:		10/04/2016
--
-- Purpose:		Gets the users associated with provider drop downs 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/04/2016		Scott Martin		Initial creation.
-- 10/05/2016		Rajiv Ranjan		Refactored proc to return result based on filter criteria
-- 12/27/2016		Scott Martin		Refactored some queries to use a new service/organization details mapping table
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetProviders]
	@FilterCriteria INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	/* PROVIDER_KEY:Admission Provider Start */
	IF(@FilterCriteria=1)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
		FROM
			Core.Users U
			INNER JOIN Core.UserRole UR
				ON U.UserID = UR.UserID
				AND UR.IsActive = 1
			INNER JOIN Core.RoleModule RM
				ON UR.RoleID = RM.RoleID
				AND RM.IsActive = 1
			INNER JOIN Core.RoleModuleComponent RMC
				ON RM.RoleModuleID = RMC.RoleModuleID
				AND RMC.IsActive = 1
			INNER JOIN Core.RoleModuleComponentPermission RMCP
				ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
				AND RMCP.IsActive = 1
			INNER JOIN Core.ModuleComponent MC
				ON RMC.ModuleComponentID = MC.ModuleComponentID
		WHERE
			RMCP.PermissionID IS NOT NULL
			AND RMCP.PermissionLevelID IS NOT NULL
			AND DataKey = 'General-General-Admission'
	End
	/* PROVIDER_KEY:Admission Provider End */

	/* PROVIDER_KEY:IDD_Intake Provider Start */
	Else If(@FilterCriteria=2)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
		FROM
			Core.vw_GetOrganizationStructureDetails Div
			INNER JOIN Core.vw_GetOrganizationStructureDetails Prg
				ON Div.MappingID = Prg.ParentID
			INNER JOIN Core.vw_GetOrganizationStructureDetails PrgU
				ON Prg.MappingID = PrgU.ParentID
			INNER JOIN Core.UserOrganizationDetailsMapping UODM
				ON PrgU.MappingID = UODM.MappingID
			INNER JOIN Core.Users U
				ON UODM.UserID = U.UserID
		WHERE
			Div.Name = 'IDD'
			AND UODM.IsActive = 1
			AND U.IsActive = 1
	End
	/* PROVIDER_KEY:IDD_Intake Provider End */

	/* PROVIDER_KEY:BAPN_Service Provider Start */
	Else If(@FilterCriteria=3)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
		FROM
			Core.vw_GetOrganizationStructureDetails Div
			INNER JOIN Core.vw_GetOrganizationStructureDetails Prg
				ON Div.MappingID = Prg.ParentID
			INNER JOIN Core.vw_GetOrganizationStructureDetails PrgU
				ON Prg.MappingID = PrgU.ParentID
			INNER JOIN Core.UserOrganizationDetailsMapping UODM
				ON PrgU.MappingID = UODM.MappingID
			INNER JOIN Core.Users U
				ON UODM.UserID = U.UserID
		WHERE
			Div.Name IN ('IDD', 'BH-SUD', 'BH-MH')
			AND UODM.IsActive = 1
			AND U.IsActive = 1
	End
	/* PROVIDER_KEY:BAPN_Service Provider End */

	/* PROVIDER_KEY:CrisisLine_Service Provider Start */
	Else If(@FilterCriteria=4)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
		FROM
			Reference.ServicesOrganizationDetails so 
			INNER JOIN Reference.[Services] s
				ON so.ServicesID = s.ServicesID
			INNER JOIN [Core].[vw_GetOrganizationStructureDetails] v
				ON so.DetailID = v.DetailID
			INNER JOIN Core.UserOrganizationDetailsMapping UODM
				ON v.MappingID = UODM.MappingID
			INNER JOIN Core.Users U
				ON UODM.UserID = U.UserID
		WHERE
			so.IsActive = 1
			AND UODM.IsActive = 1
			AND U.IsActive = 1
			AND	s.IsInternal = 1
			AND s.ServicesID = 189
	End
	/* PROVIDER_KEY:CrisisLine_Service Provider End */

	/* PROVIDER_KEY:CrisisLine_Service_Supervising Provider Start */
	Else If(@FilterCriteria=5)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
 		FROM
			Core.Users U
			INNER JOIN Core.UserRole UR
				ON U.UserID = UR.UserID
				AND UR.IsActive = 1
			INNER JOIN Core.RoleModule RM
				ON UR.RoleID = RM.RoleID
				AND RM.IsActive = 1
			INNER JOIN Core.RoleModuleComponent RMC
				ON RM.RoleModuleID = RMC.RoleModuleID
				AND RMC.IsActive = 1
			INNER JOIN Core.RoleModuleComponentPermission RMCP
				ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
				AND RMCP.IsActive = 1
			INNER JOIN Core.ModuleComponent MC
				ON RMC.ModuleComponentID = MC.ModuleComponentID
		WHERE
			RMCP.PermissionID IS NOT NULL
			AND RMCP.PermissionLevelID IS NOT NULL
			AND DataKey = 'CrisisLine-CrisisLine-Approver'
	End
	/* PROVIDER_KEY:CrisisLine_Service_Supervising Provider End */

	/* PROVIDER_KEY:LawLiaison_Service Provider Start */
	Else If(@FilterCriteria=6)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
		FROM
			Reference.ServicesOrganizationDetails so 
			INNER JOIN Reference.[Services] s
				ON so.ServicesID = s.ServicesID
			INNER JOIN [Core].[vw_GetOrganizationStructureDetails] v
				ON so.DetailID = v.DetailID
			INNER JOIN Core.UserOrganizationDetailsMapping UODM
				ON v.MappingID = UODM.MappingID
			INNER JOIN Core.Users U
				ON UODM.UserID = U.UserID
		WHERE
			so.IsActive = 1
			AND UODM.IsActive = 1
			AND U.IsActive = 1
			AND	s.IsInternal = 1
			AND s.ServicesID = 190
	End
	/* PROVIDER_KEY:LawLiaison_Service Provider End */

	/* PROVIDER_KEY:LawLiaison_Service_Supervising Provider Start */
	Else If(@FilterCriteria=7)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
		FROM
			Core.Users U
			INNER JOIN Core.UserRole UR
				ON U.UserID = UR.UserID
				AND UR.IsActive = 1
			INNER JOIN Core.RoleModule RM
				ON UR.RoleID = RM.RoleID
				AND RM.IsActive = 1
			INNER JOIN Core.RoleModuleComponent RMC
				ON RM.RoleModuleID = RMC.RoleModuleID
				AND RMC.IsActive = 1
			INNER JOIN Core.RoleModuleComponentPermission RMCP
				ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
				AND RMCP.IsActive = 1
			INNER JOIN Core.ModuleComponent MC
				ON RMC.ModuleComponentID = MC.ModuleComponentID
		WHERE
			RMCP.PermissionID IS NOT NULL
			AND RMCP.PermissionLevelID IS NOT NULL
			AND DataKey = 'LawLiaison-LawLiaison-Approver'
	End
	/* PROVIDER_KEY:LawLiaison_Service_Supervising Provider End */

	/* PROVIDER_KEY:ECI_Registration_AdditionalDemography Provider Start */
	Else If(@FilterCriteria=8)
	BEGIN
		SELECT DISTINCT
			U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name
		FROM
			Core.vw_GetOrganizationStructureDetails Div
			INNER JOIN Core.vw_GetOrganizationStructureDetails Prg
				ON Div.MappingID = Prg.ParentID
			INNER JOIN Core.vw_GetOrganizationStructureDetails PrgU
				ON Prg.MappingID = PrgU.ParentID
			INNER JOIN Core.UserOrganizationDetailsMapping UODM
				ON PrgU.MappingID = UODM.MappingID
			INNER JOIN Core.Users U
				ON UODM.UserID = U.UserID
			INNER JOIN Core.UserCredential UC
				ON U.UserID = UC.UserID
		WHERE
			Div.Name = 'ECS'
			AND UODM.IsActive = 1
			AND U.IsActive = 1
			AND UC.IsActive = 1
			AND UC.CredentialID IN (24, 74)
	End
	/* PROVIDER_KEY:ECI_Registration_AdditionalDemography Provider End */

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO