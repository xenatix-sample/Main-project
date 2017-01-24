-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateReferralAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Update Referral Additional/Client details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu - Initial Creation
-- 12/16/2015	Scott Martin	Removed ID parameter and added audit logging
-- 01/04/2016	Sumana Sangapu	Removed the ECI fields from the screen
-- 01/06/2016	Lokesh Singhal	Renamed @referralid parametr to @ContactID
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateReferralAdditionalDetails]
	@ReferralAdditionalDetailID BIGINT,
	@ReferralHeaderID BIGINT,
	@ContactID BIGINT,
	@ReasonforCare nvarchar(100),
	@IsTransferred BIT,
	@IsHousingProgram BIT,
	@HousingDescription nvarchar(100),
	@IsEligibleforFurlough BIT, 
	@IsReferralDischargeOrTransfer BIT, 
	@IsConsentRequired BIT, 
	@Comments nvarchar(500),
	@AdditionalConcerns nvarchar(500),
	@ModifiedOn DATETIME,
	@ModifiedBy INT, 
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
					
	SELECT  @ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralAdditionalDetails', @ReferralAdditionalDetailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	
			
	UPDATE [Registration].[ReferralAdditionalDetails]
	SET ReferralHeaderID = @ReferralHeaderID, 
		ContactID = @ContactID, 
		ReasonforCare = @ReasonforCare, 
		IsTransferred = @IsTransferred, 
		IsHousingProgram = @IsHousingProgram, 
		HousingDescription = @HousingDescription, 
		IsEligibleforFurlough = @IsEligibleforFurlough, 
		IsReferralDischargeOrTransfer = @IsReferralDischargeOrTransfer, 
		IsConsentRequired = @IsConsentRequired,
		Comments = @Comments, 
		AdditionalConcerns = @AdditionalConcerns,
		IsActive = 1, 
		ModifiedBy = @ModifiedBy, 
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()			
	WHERE
		ReferralAdditionalDetailID = @ReferralAdditionalDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralAdditionalDetails', @AuditDetailID, @ReferralAdditionalDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
 
	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END