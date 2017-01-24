-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetPayorSearch
-- Author:		Scott Martin
-- Date:		06/13/2016
--
-- Purpose:		Gets a list of payors based on various search criteria
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/13/2016	Scott Martin	Initial Creation
-- 06/14/2016	Scott Martin	Added GroupName and PayorCode
-- 06/15/2016	Atul Chauhan	Included missed columns in select statement
-- 06/17/2016	Atul Chauhan	Added columns(PayorPlanID,PayorGroupID,PayorAddressID,StateProvinceID) in select statement
-- 09/14/2016	Scott Martin	Added some additional ORDER BY constraints to show exact matches first, and sort by longer words first
-- 09/16/2016	Kyle Campbell	Bug #14721	Removed ItemLength from inner query and sort order as it was causing duplicate records when multiple words were in search string
-- 12/27/2016	Sumana Sangapu	Fetch unexpired Payor records
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetPayorSearch]
	@PayorSch NVARCHAR(100),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Payors retrieved successfully'

	BEGIN TRY	
	DECLARE @SearchWords TABLE (Item NVARCHAR(255));

	INSERT INTO @SearchWords
	SELECT
		Item
	FROM
		[Core].[fn_IterativeWordChop] (@PayorSch);

	IF NOT EXISTS (SELECT TOP 1 * FROM @SearchWords)
		BEGIN
		INSERT INTO @SearchWords VALUES ('');
		END

	SELECT
		P.PayorID,
		P.PayorName,
		PP.PlanID,
		PP.PlanName,
		PP.PayorPlanID,
		PG.GroupName,
		PG.PayorGroupID,
		PA.ElectronicPayorID,
		PA.PayorAddressID,
		P.PayorCode,
		A.Line1,
		A.Line2,
		A.City,
		SP.StateProvinceCode,
		SP.StateProvinceName,
		SP.StateProvinceID,
		A.Zip,
		COALESCE(CAST(P.PayorID AS NVARCHAR(15)) + ':', '') + COALESCE(P.PayorName + ':', '') + COALESCE(PP.PlanName + ':', '') + COALESCE(CAST(PA.ElectronicPayorID AS NVARCHAR(15)) + ':', '') + COALESCE(A.Line1 + ':', '') + + COALESCE(A.Line2 + ':', '') + COALESCE(A.City + ':', '') + COALESCE(SP.StateProvinceCode + ':', '') + COALESCE(SP.StateProvinceName + ':', '') + COALESCE(CAST(A.Zip AS NVARCHAR(10)) + ':', '') AS SearchString
	INTO #PayorData
	FROM
		Reference.Payor P
		LEFT OUTER JOIN Reference.PayorPlan PP
			ON P.PayorID = PP.PayorID
			AND PP.IsActive = 1
		LEFT OUTER JOIN Registration.PayorAddress PA
			ON PP.PayorPlanID = PA.PayorPlanID
			AND PA.IsActive = 1
		LEFT OUTER JOIN Core.Addresses A
			ON PA.AddressID = A.AddressID
		LEFT OUTER JOIN Reference.StateProvince SP
			ON A.StateProvince = SP.StateProvinceID
		LEFT OUTER JOIN Reference.PayorGroup PG
			ON PP.PayorPlanID = PG.PayorPlanID
	WHERE
		P.IsActive = 1
		AND ( ISNULL(P.ExpirationDate,GETDATE()) > GETDATE() OR P.ExpirationDate IS NULL) -- Fetch only unexpired records

	--Encapsulating the code so the additional columns won't affect the UI
	SELECT 
		PayorID,
		PayorName,
		PayorCode,
		PlanID,
		PlanName,
		PayorPlanID,
		GroupName,
		PayorGroupID,
		ElectronicPayorID,
		PayorAddressID,
		Line1,
		Line2,
		City,
		StateProvinceCode,
		StateProvinceName,
		StateProvince,
		Zip
	FROM
	(
		SELECT DISTINCT
			PayorID,
			PayorName,
			PayorCode,
			PlanID,
			PlanName,
			PayorPlanID,
			GroupName,
			PayorGroupID,
			ElectronicPayorID,
			PayorAddressID,
			Line1,
			Line2,
			City,
			StateProvinceCode,
			StateProvinceName,
			StateProvinceID As StateProvince,
			Zip,
			CASE WHEN UD.SearchString LIKE '%' + @PayorSch + '%' THEN 1 ELSE 0 END AS ExactMatch
		FROM
			#PayorData UD
			INNER JOIN @SearchWords W
				ON UD.SearchString LIKE '%' + W.Item + '%'
	) AS SearchResults
	ORDER BY
			ExactMatch DESC,
			PayorName,
			StateProvinceCode

	DROP TABLE #PayorData

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END