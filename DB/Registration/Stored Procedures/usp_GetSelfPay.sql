-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetSelfPay]
-- Author:		Kyle Campbell
-- Date:		03/21/2016
--
-- Purpose:		Get Contact SelfPay Records
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/21/2016	Kyle Campbell	TFS #7798	Initial Creation
-- 03/30/2016	Kyle Campbell	Renamed OrganizationID to OrganizationDetailID for clarity
--								Added foreign key on OrganizationDetailID to OrganizationDetails.DetailID
-- 04/06/2016	Scott Martin	Changed ContactID to SelfPayHeaderID
-- 17/10/2016   Gaurav Gupta    Added flag IsViewSelfPay for read only view
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetSelfPay]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	
		;WITH CTE_FinancialSummary AS
		(
				SELECT Distinct  selfPay.crossApply.value('(SelfPayID)[1]','NVARCHAR(MAX)') AS SelfPayID  
				FROM    Registration.FinancialSummary  FS
				Cross   Apply FinancialAssessmentXML.nodes('/FinancialSummary/SelfPay/MonthlyAbilities/MonthlyAbility') selfPay(crossApply)  
		)

		SELECT
			SP.[SelfPayID],
			[ContactID],
			[OrganizationDetailID],
			[SelfPayAmount],
			[IsPercent], 	
			[EffectiveDate],
			[ExpirationDate],
			[IsChildInConservatorship],
			[IsNotAttested],
			[IsApplyingForPublicBenefits],
			[IsEnrolledInPublicBenefits],
			[IsRequestingReconsideration],
			[IsNotGivingConsent],
			[IsOtherChildEnrolled],
			[IsReconsiderationOfAdjustment],
			Case when FAS.SelfPayID is null then Cast(0 as BIT) else Cast(1 as BIT) end IsViewSelfPay
		FROM 
			Registration.SelfPay SP
			Left Join CTE_FinancialSummary FAS on SP.SelfPayID=FAS.SelfPayID 
		WHERE 
			ContactID = @ContactID
			AND IsActive = 1
		ORDER BY ModifiedOn DESC
	END TRY
	
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultCode = ERROR_MESSAGE()
	END CATCH
END
