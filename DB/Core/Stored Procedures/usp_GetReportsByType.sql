-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReportsByType]
-- Author:		Demetrios C. Christopher
-- Date:		11/04/2015
--
-- Purpose:		Gets the Report details by report type (input parameter @@ReportTypeName) (or all report types if blank)
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/04/2015	Demetrios C. Christopher - Initial creation.
-- 05/03/2016	Scott Martin				Refactored query to added ssrs related fields
-- 05/11/2016	Scott Martin				Added UserID Parameter to be able to pass in logged in user to Report
-- 06/19/2016	Scott Martin				Fixed an issue where all reports were being returned when only a few are selected in role management
----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_GetReportsByType]
	@ReportTypeName VARCHAR(50),
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	DECLARE @ReportServerURL NVARCHAR(50)

	IF @ReportTypeName = 'ReportingServices'
		BEGIN
		SELECT
			@ReportServerURL = Value
		FROM
			Core.Settings S
			INNER JOIN Core.SettingValues SV
				ON S.SettingsID = SV.SettingsID
		WHERE
			S.Settings = 'ReportServerUrl'
			AND SV.IsActive = 1;

		;WITH AssignedReports (ModuleComponentID)
		AS
		(
			SELECT DISTINCT
				MC.ModuleComponentID
			FROM
				Core.UserRole UR
				INNER JOIN Core.RoleModule RM
					ON UR.RoleID = RM.RoleID
					AND RM.IsActive = 1
				LEFT OUTER JOIN Core.RoleModuleComponent RMC
					ON RM.RoleModuleID = RMC.RoleModuleID
					AND RMC.IsActive = 1
				LEFT OUTER JOIN Core.ModuleComponent MC
					ON RMC.ModuleComponentID = MC.ModuleComponentID
				LEFT OUTER JOIN Core.RoleModuleComponentPermission RMCP
					ON RMC.RoleModuleComponentID = RMCP.RoleModuleComponentID
					AND RMCP.IsActive = 1
			WHERE
				MC.ModuleID = 10
				AND UR.IsActive = 1
				AND UR.UserID = @UserID
				AND RMCP.PermissionLevelID IS NOT NULL
		)
		SELECT
			R.ReportID,
			R.ReportTypeID,
			R.ReportName,
			R.ReportModel,
			CASE
				WHEN RT.ReportTypeName = 'ReportingServices' THEN CONCAT(@ReportServerURL, '/Pages/ReportViewer.aspx?', R.ReportPath, '&ReportUserID=', @UserID) ELSE NULL END AS ReportURL,
			R.ReportGroupID,
			RG.ReportGroup,
			RT.ReportTypeName,
			R.ReportDisplayName
		FROM
			Core.Reports R
			INNER JOIN Reference.ReportType RT
				ON r.ReportTypeID = rt.ReportTypeID
				AND r.IsActive = 1
				AND rt.IsActive = 1
				AND rt.ReportTypeName = CASE WHEN @ReportTypeName = '' THEN ReportTypeName ELSE @ReportTypeName END
			LEFT OUTER JOIN Reference.ReportGroup RG
				ON R.ReportGroupID = RG.ReportGroupID
			INNER JOIN AssignedReports AR
				ON R.ModuleComponentID = AR.ModuleComponentID
		END
	ELSE
		BEGIN
		SELECT
			R.ReportID,
			R.ReportTypeID,
			R.ReportName,
			R.ReportModel,
			NULL AS ReportURL,
			R.ReportGroupID,
			RG.ReportGroup,
			RT.ReportTypeName
		FROM
			Core.Reports R
			INNER JOIN Reference.ReportType RT
				ON r.ReportTypeID = rt.ReportTypeID
				AND r.IsActive = 1
				AND rt.IsActive = 1
				AND rt.ReportTypeName = CASE WHEN @ReportTypeName = '' THEN ReportTypeName ELSE @ReportTypeName END
			LEFT OUTER JOIN Reference.ReportGroup RG
				ON R.ReportGroupID = RG.ReportGroupID
		END
		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO