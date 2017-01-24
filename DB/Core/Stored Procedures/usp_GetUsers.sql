-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetUsers
-- Author:		Justin Spalti
-- Date:		07/21/2015
--
-- Purpose:		Gets a list of users based on various search criteria
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015 - Updated the logic to accept an additional search parameter named IsActive
-- 07/23/2015 - Added dbo to the table name
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 09/02/2015 - Justin Spalti -- Added code to get a user's primary email address
-- 09/04/2015 - Justin Spalti -- Updated the procedure to accept a single search field that applies to multiple searchable fields
-- 02/02/2016 - Lokesh Singhal -- Allow search with firstname and lastname provided with space
-- 03/01/2016	Scott Martin		Refactored the query to add additional columns and to work correctly
-- 03/02/2016	Scott Martin		Trimmed down the code to make it easier to maintain and added HasSupervisor flag
-- 03/03/2016	Gurpreet Singh		Added IsActive check so HasSupervisor is not set for deleted records
-- 03/10/2016 - Justin Spalti - Swapped the order by columns
-- 03/29/2016	Scott Martin	Temporarily removing Program and ProgramUnit from the query until performance improves
-- 07/02/2016	Kyle Campbell	TFS #12135	Modified to search on username
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_GetUsers]
	@UserSch NVARCHAR(100),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Users retrieved successfully'

	BEGIN TRY	
	DECLARE @ProgramDataKey NVARCHAR(50) = 'Program',
			@ProgramUnitDataKey NVARCHAR(50) = 'ProgramUnit';

	DECLARE @SearchWords TABLE (Item NVARCHAR(100));

	INSERT INTO @SearchWords
	SELECT
		Item
	FROM
		[Core].[fn_IterativeWordChop] (@UserSch);

	IF NOT EXISTS (SELECT TOP 1 * FROM @SearchWords)
		BEGIN
		INSERT INTO @SearchWords VALUES ('');
		END

	SELECT	
		U.UserID,
		U.UserGUID,
		U.ADFlag,
		U.UserName,
		U.FirstName,
		U.MiddleName,
		U.LastName,
		U.GenderID,
		G.Gender,
		U.DOB,
		U.Password,
		NULL AS Program,
		NULL AS ProgramUnit,
		u.IsActive,
		u.EffectiveToDate,
		u.LoginAttempts, 
		u.LoginCount,
		u.LastLogin,
		u.ModifiedOn,
		u.ModifiedBy,
		e.EmailID,
		e.Email AS 'PrimaryEmail',
		COALESCE(U.UserName + ':', '') + COALESCE(U.FirstName + ':', '') + COALESCE(U.MiddleName + ':', '') + COALESCE(U.LastName + ':', '') + COALESCE(CONVERT(NVARCHAR(20),DOB) + ':', '') /* + COALESCE(ProgramNames.OrganizationLevelNames + ':', '') + COALESCE(ProgramUnitNames.OrganizationLevelNames + ':', '') */ AS SearchString
	INTO #UserData
	FROM
		Core.Users U
		LEFT OUTER JOIN Reference.Gender G
			ON U.GenderID = G.GenderID
		--LEFT OUTER JOIN Core.fn_UserOrganizationLevelNames(@ProgramDataKey) ProgramNames
		--	ON U.UserID = ProgramNames.UserID
		--LEFT OUTER JOIN Core.fn_UserOrganizationLevelNames(@ProgramUnitDataKey) ProgramUnitNames
		--	ON U.UserID = ProgramUnitNames.UserID
		JOIN Core.[UserEmail] UE
			ON UE.UserID = U.UserID
		JOIN Core.[Email] E
			ON E.EmailID = UE.EmailID;

	SELECT DISTINCT
		UD.UserID,
		UserGUID,
		ADFlag,
		UserName,
		FirstName,
		MiddleName,
		LastName,
		GenderID,
		Gender,
		DOB,
		Password,
		Program,
		ProgramUnit,
		UD.IsActive,
		EffectiveToDate,
		LoginAttempts, 
		LoginCount,
		LastLogin,
		UD.ModifiedOn,
		UD.ModifiedBy,
		EmailID,
		PrimaryEmail,
		CAST(CASE WHEN UHM.ParentID IS NOT NULL THEN 1 ELSE 0 END AS bit) AS HasSupervisor
	FROM
		#UserData UD
		INNER JOIN @SearchWords W
			ON UD.SearchString LIKE '%' + W.Item + '%'
			OR GenderID IN (SELECT GenderID FROM Reference.Gender G INNER JOIN @SearchWords SW ON G.Gender = SW.Item)
		LEFT OUTER JOIN Core.UsersHierarchyMapping UHM
			ON UD.UserID = UHM.UserID AND UHM.IsActive = 1
	ORDER BY
		UserName,
		UD.IsActive

	DROP TABLE #UserData
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END