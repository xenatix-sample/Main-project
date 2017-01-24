-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReportUserOrgStructure]
-- Author:		Scott Martin
-- Date:		07/14/2016
--
-- Purpose:		Gets the Organization Structure Details the user may access based on a specific report
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/14/2016	Scott Martin	Initial Creation
----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetReportUserOrgStructure
	@ReportUserID INT,
	@ReportName NVARCHAR(100)
AS
BEGIN
DECLARE @OrgLevel INT

IF @ReportUserID <> 0
	BEGIN
	SELECT @OrgLevel =
		MIN(CASE
			WHEN OA.DataKey = 'Company' THEN 1
			WHEN OA.DataKey = 'Division' THEN 2
			WHEN OA.DataKey = 'Program' THEN 3
			WHEN OA.DataKey = 'ProgramUnit' THEN 4
			ELSE 99 END)
	FROM 
		Core.RoleModuleComponentPermission RMP
		INNER JOIN Core.RoleModuleComponent RMC
			ON RMP.RoleModuleComponentID = RMC.RoleModuleComponentID
		LEFT OUTER JOIN Core.Permission P
			ON RMP.PermissionID = P.PermissionID
		LEFT OUTER JOIN Core.PermissionLevel PL
			ON RMP.PermissionLevelID = PL.PermissionLevelID
		INNER JOIN Core.ModuleComponent MC
			ON RMC.ModuleComponentID = MC.ModuleComponentID
		INNER JOIN Core.Component CM
			ON MC.ComponentID = CM.ComponentID
		INNER JOIN Core.RoleModule RM
			ON RMC.RoleModuleID = RM.RoleModuleID
			AND RM.IsActive = 1
		INNER JOIN Core.UserRole UR
			ON RM.RoleID = UR.RoleID
			AND UR.IsActive = 1
		INNER JOIN Core.OrganizationAttributes OA
			ON PL.AttributeID = OA.AttributeID
	WHERE 
		RMP.IsActive = 1
		AND CM.Name = @ReportName
		AND UR.UserID = @ReportUserID;
	END

IF @OrgLevel = 1 OR @ReportUserID = 0
	BEGIN
  SELECT 0 AS OrganizationID
  UNION
	SELECT ISNULL(MappingID, 0) AS OrganizationID FROM Core.vw_GetOrganizationStructureDetails;
	END

IF @OrgLevel = 4 AND @ReportUserID <> 0
	BEGIN
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
			AND UODM.UserID = @ReportUserID
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
	)
  SELECT 0 AS OrganizationID
  UNION
	SELECT DISTINCT
		ISNULL(UORG.MappingID,0) AS OrganizationID
	FROM
		UORG
	END
END