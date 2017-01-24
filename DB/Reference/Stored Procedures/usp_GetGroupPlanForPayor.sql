-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupPlanForPayor]
-- Author:		Avikal Gupta
-- Date:		08/24/2015
--
-- Purpose:		Get all Group Plan details for particular Payor  
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	Avikal Task	Initial creation
-- 09/08/2015	Gurpreet Singh	-	Returned empty if GroupID, GroupName, PlanID, PlanName are null 
-- 09/16/2015   John Crossen             2313   Refactor Contact benefits
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetGroupPlanForPayor]
	@PayorID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT

AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			payor.PayorID
			,ContactPayor.ContactPayorID
			,ISNULL(PayorGroup.GroupID, '') AS GroupID
			,ISNULL(PayorGroup.GroupName, '') AS GroupName
			,ISNULL(PayorPlan.PlanID, '') AS PlanID
			,ISNULL(PayorPlan.PlanName, '') AS PlanName
			,ContactPayor.Copay
			,CoreAddress.Name GroupPlanAddressName
			,CoreAddress.Line1 
			,CoreAddress.Line2 
			,CoreAddress.City
			,county.CountyName
			,stateProvince.StateProvinceName
			,CoreAddress.Zip
			,country.CountryName
		FROM 
			[Reference].[Payor] AS payor
			INNER JOIN [Registration].[ContactPayor] AS ContactPayor ON payor.[PayorID] = ContactPayor.[PayorID]
			LEFT OUTER JOIN Reference.PayorPlan AS PayorPlan ON payor.PayorID=PayorPlan.PayorID
			LEFT OUTER JOIN Reference.PayorGroup AS PayorGroup ON PayorPlan.PayorPlanID=PayorGroup.PayorPlanID
 			LEFT OUTER JOIN [Core].[Addresses] as CoreAddress ON ContactPayor.PayorAddressID = CoreAddress.AddressID
			LEFT OUTER JOIN [Reference].[County] county ON county.CountyID =CoreAddress.County
			LEFT OUTER JOIN [Reference].[StateProvince] stateProvince ON stateProvince.StateProvinceID = CoreAddress.StateProvince
			LEFT OUTER JOIN [Reference].[Country] country ON country.[CountryID] = CoreAddress.[Country]
		WHERE
			payor.[PayorID] = @PayorID
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

