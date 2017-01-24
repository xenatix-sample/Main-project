
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactBenefits]
-- Author:		Avikal Gupta
-- Date:		08/24/2015
--
-- Purpose:		Get all benefit information related to the contact / Benefit information for contact and Payor 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	Avikal Task	Initial creation
-- 09/02/2015	Avikal	- Added ContactPayorRank and PolicyHolderName
-- 09/14/2015	Rajiv Ranjan	--  Added CoPay in ResultSet
-- 09/15/2015  John Crossen     -- Migrate data elements from GroupPayor
--09/16/2015    John Crossen      TFS 2366 -- Move Address Information
-- 09/20/2015   Arun Choudhary    Changes to accomodate new benefit structure.
--09/23/2015    Arun Choudhary  TFS 2377 - Renamed ID1 to PolicyID.
-- 03/14/2016	Arun Choudhary	Added CoInsurance.
-- 03/16/2016	Kyle Campbell	TFS #7308 Added PayorExpirationReasonID
-- 06/14/2016	Atul Chauhan	Added PolicyHolderEmployer (PBI -11154 Policy Holder Employer Field)
-- 06/15/2016	Atul Chauhan	Added Payor Billing info
-- 06/17/2016	Atul Chauhan	Added GroupID,AdditionalInformation
-- 06/17/2016	Atul Chauhan	Added PayorCode
-- 09/13/2016	Gaurav Gupta	Added HasPolicyHolderSameCardName
---------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetContactBenefits]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT ContactPayor.[ContactPayorID]
			  ,ContactPayor.[ContactID]
			  ,ContactPayor.[PayorID]
			  ,ContactPayor.[PayorPlanID]
			  ,ContactPayor.[PayorGroupID]
			  ,ContactPayor.[PolicyHolderID]
			  ,ContactPayor.[PolicyHolderName] 
			  ,ContactPayor.[GroupID]
			  ,ContactPayor.[PolicyHolderFirstName] 
			  ,ContactPayor.[PolicyHolderMiddleName] 
			  ,ContactPayor.[PolicyHolderLastName]
			  ,ContactPayor.[PolicyHolderEmployer]
			  ,ContactPayor.[PolicyHolderSuffixID]  
			  ,ContactPayor.[PayorAddressID]
			  ,ContactPayor.[BillingFirstName] 
			  ,ContactPayor.[BillingMiddleName] 
			  ,ContactPayor.[BillingLastName]
			  ,ContactPayor.[BillingSuffixID]
			  ,ContactPayor.[AdditionalInformation]
			  ,ContactPayor.[PolicyID] 
			  ,ContactPayor.[Deductible]
			  ,ContactPayor.[Copay]
			  ,ContactPayor.[CoInsurance]
			  ,ContactPayor.[EffectiveDate]
			  ,ContactPayor.[ExpirationDate]
			  ,ContactPayor.[PayorExpirationReasonID]
			  ,ContactPayor.[ExpirationReason]
			  ,ContactPayor.[AddRetroDate]
			  ,ContactPayor.[ContactPayorRank]
			  ,ContactPayor.[IsActive]
			  ,ContactPayor.[ModifiedBy]
			  ,ContactPayor.[ModifiedOn]
			  ,ContactPayor.[HasPolicyHolderSameCardName]

			  ,CoreAddress.AddressID
			  ,CoreAddress.Line1 
			  ,CoreAddress.Line2 
			  ,CoreAddress.City
			  ,CoreAddress.StateProvince
			  ,CoreAddress.County
			  ,CoreAddress.Zip

			  ,PayorPlan.[PlanName]
			  ,PayorPlan.[PlanID]
			  
			  ,PayorGroup.[GroupName]
			  ,PayorAddress.[ElectronicPayorID]
			  
			  ,Payor.[PayorName]
			  ,Payor.[PayorCode]
			  
		  FROM [Registration].[ContactPayor] AS ContactPayor
		  INNER JOIN [Reference].[Payor] AS Payor ON ContactPayor.[PayorID] = Payor.[PayorID]
		  LEFT OUTER JOIN [Reference].PayorPlan AS PayorPlan ON PayorPlan.PayorID = Payor.PayorID  And ContactPayor.PayorPlanID = PayorPlan.PayorPlanID
		  LEFT OUTER JOIN [Reference].PayorGroup AS PayorGroup ON PayorGroup.PayorPlanID = PayorPlan.PayorPlanID and  ContactPayor.PayorGroupID=PayorGroup.PayorGroupID
		  LEFT OUTER JOIN [Registration].[PayorAddress] AS PayorAddress ON  PayorAddress.[PayorAddressID] = ContactPayor.[PayorAddressID] 
		  LEFT OUTER JOIN [Core].[Addresses] AS CoreAddress ON CoreAddress.[AddressID] = PayorAddress.[AddressID]
		  WHERE 
			ContactPayor.[ContactID] = @ContactID
			AND ContactPayor.[IsActive] = 1
		  
	END TRY
	BEGIN CATCH
		SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

