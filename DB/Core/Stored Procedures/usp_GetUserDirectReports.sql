-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUserDirectReports
-- Author:		Scott Martin
-- Date:		02/26/2016
--
-- Purpose:		Gets a list of users that report to the user or the user reports to
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/26/2016	Scott Martin		Initial Creation
-- 03/02/2016	Gurpreet Singh		Removed IsDirectReport, Updated Join on ProgramDataKey and ProgramUnitDataKey as if they will not return, no output will showup
-- 04/05/2016	Semalai Muthusamy 	Defect 6372 -Temporarily removing Program and ProgramUnit from the query until performance improves. Based on input from Scott Martin  
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserDirectReports]
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
--DECLARE @ProgramDataKey NVARCHAR(50) = 'Program',
--		@ProgramUnitDataKey NVARCHAR(50) = 'ProgramUnit';

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Users retrieved successfully'

	BEGIN TRY	
	SELECT
		UHM.MappingID,
		U.FirstName,
		U.MiddleName,
		U.LastName,
		U.UserName,
		E.Email,
		CAST(0 AS BIT) AS IsSupervisor
		--CAST(0 AS BIT) AS IsSupervisor,
		--ProgramNames.OrganizationLevelNames AS Program,
		--ProgramUnitNames.OrganizationLevelNames AS ProgramUnit
	FROM
		Core.UsersHierarchyMapping UHM
		INNER JOIN Core.Users U
			ON UHM.UserID = U.UserID
		INNER JOIN Core.UserEmail UE
			ON U.UserID = UE.UserID
		INNER JOIN Core.Email E
			ON UE.EmailID = E.EmailID
		--LEFT OUTER JOIN Core.fn_UserOrganizationLevelNames(@ProgramDataKey) ProgramNames
		--	ON U.UserID = ProgramNames.UserID
		--LEFT OUTER JOIN Core.fn_UserOrganizationLevelNames(@ProgramUnitDataKey) ProgramUnitNames
		--	ON U.UserID = ProgramUnitNames.UserID
	WHERE
		UHM.ParentID = @UserID
		AND UHM.IsActive = 1
	UNION
	SELECT
		UHM.MappingID,
		U.FirstName,
		U.MiddleName,
		U.LastName,
		U.UserName,
		E.Email,
		CAST(1 AS BIT) AS IsSupervisor
		--,
		--ProgramNames.OrganizationLevelNames AS Program,
		--ProgramUnitNames.OrganizationLevelNames AS ProgramUnit
	FROM
		Core.UsersHierarchyMapping UHM
		INNER JOIN Core.Users U
			ON UHM.ParentID = U.UserID
		INNER JOIN Core.UserEmail UE
			ON U.UserID = UE.UserID
		INNER JOIN Core.Email E
			ON UE.EmailID = E.EmailID
		--LEFT OUTER JOIN Core.fn_UserOrganizationLevelNames(@ProgramDataKey) ProgramNames
		--	ON U.UserID = ProgramNames.UserID
		--LEFT OUTER JOIN Core.fn_UserOrganizationLevelNames(@ProgramUnitDataKey) ProgramUnitNames
		--	ON U.UserID = ProgramUnitNames.UserID
	WHERE
		UHM.UserID = @UserID
		AND UHM.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END