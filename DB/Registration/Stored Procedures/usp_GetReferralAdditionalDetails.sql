-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReferralAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Get Referral Additional/Client details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu  Initial Creation
-- 12/17/2015   Scott Martin  	Changed @ReferralAdditionalDetailID to @ReferralHeaderID
-- 01/04/2016	Sumana Sangapu	Removed fields not related to ECI since they are captured as Additional Demogratphics in ECI Registration
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetReferralAdditionalDetails]
		@ReferralheaderID		BIGINT,
		@ResultCode						INT OUTPUT,
		@ResultMessage					NVARCHAR(500) OUTPUT
AS
BEGIN
		
		SELECT  @ResultCode = 0,
			    @ResultMessage = 'executed successfully'

		BEGIN TRY
			

			SELECT 	ReferralAdditionalDetailID, ReferralHeaderID, ContactID, --IsCurentlyHospitalized, 
					--ExpectedDischargeDate, SSI, ManagedCare, 
					ReasonforCare, IsTransferred, IsHousingProgram, HousingDescription, IsEligibleforFurlough, 
					--ProgramTransferredFromID, 
					IsReferralDischargeOrTransfer, IsConsentRequired, 
					Comments, AdditionalConcerns, IsActive, ModifiedBy, ModifiedOn
			FROM	Registration.ReferralAdditionalDetails
			WHERE	ReferralheaderID = @ReferralheaderID
			AND		IsActive = 1 

		END TRY
		BEGIN CATCH
			SELECT 
				   @ResultCode = ERROR_SEVERITY(),
				   @ResultMessage = ERROR_MESSAGE()
		END CATCH
END