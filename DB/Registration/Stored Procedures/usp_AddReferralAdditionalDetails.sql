-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddReferralAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Add Referral Additional/Client details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu - Initial Creation
-- 12/16/2015	Scott Martin	Added audit logging
-- 01/04/2016	Sumana Sangapu	Removed the ECI fields from the screen
-- 01/06/2016	Lokesh Singhal	Renamed @referralid parametr to @ContactID
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddReferralAdditionalDetails]
	@ReferralHeaderID				BIGINT,
	@ContactID						BIGINT,
	@ReasonforCare					nvarchar(100),
	@IsTransferred					BIT,
	@IsHousingProgram				BIT,
	@HousingDescription				nvarchar(100),
	@IsEligibleforFurlough			BIT, 
	@IsReferralDischargeOrTransfer	BIT, 
	@IsConsentRequired				BIT, 
	@Comments						nvarchar(500),
	@AdditionalConcerns				nvarchar  (500),
	@ModifiedOn						DATETIME,
	@ModifiedBy						INT,
	@ResultCode						INT OUTPUT,
	@ResultMessage					NVARCHAR(500) OUTPUT,
	@ID								INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
					
	SELECT  @ID = 0,
			@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	INSERT INTO [Registration].[ReferralAdditionalDetails]
	(
		ReferralHeaderID,
		ContactID,
		ReasonforCare,
		IsTransferred,
		IsHousingProgram,
		HousingDescription,
		IsEligibleforFurlough, 
		IsReferralDischargeOrTransfer,
		IsConsentRequired,
		Comments,
		AdditionalConcerns,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ReferralHeaderID,
		@ContactID,
		@ReasonforCare,
		@IsTransferred,
		@IsHousingProgram,
		@HousingDescription,
		@IsEligibleforFurlough,
		@IsReferralDischargeOrTransfer,
		@IsConsentRequired,
		@Comments,
		@AdditionalConcerns,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralAdditionalDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralAdditionalDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	END TRY

	BEGIN CATCH
	SELECT 
			@ID = 0,
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END