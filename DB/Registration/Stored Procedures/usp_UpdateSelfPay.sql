-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateSelfPay]
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
-- 04/28/2016   Lokesh Singhal  Change scale value of parameter as per database
-- 04/28/2016   Gaurav Gupta    Change isParcent if null pass 0 
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateSelfPay]
	@SelfPayID BIGINT,
	@OrganizationDetailID BIGINT,
	@SelfPayAmount DECIMAL(15,2),
	@IsPercent BIT=0, 
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@IsChildInConservatorship BIT,
	@IsNotAttested BIT,
	@IsApplyingForPublicBenefits BIT,
	@IsEnrolledInPublicBenefits BIT,
	@IsRequestingReconsideration BIT,
	@IsNotGivingConsent BIT,
	@IsOtherChildEnrolled BIT,
	@IsReconsiderationOfAdjustment BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ContactID BIGINT;

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully';

		SELECT @ContactID = ContactID FROM Registration.SelfPay WHERE SelfPayID = @SelfPayID;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'SelfPay', @SelfPayID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.SelfPay
		SET	OrganizationDetailID = @OrganizationDetailID,
			SelfPayAmount = @SelfPayAmount,
			IsPercent =ISNULL(@IsPercent,0),
			EffectiveDate = @EffectiveDate,
			ExpirationDate = @ExpirationDate,
			IsChildInConservatorship = @IsChildInConservatorship,
			IsNotAttested = @IsNotAttested,
			IsApplyingForPublicBenefits = @IsApplyingForPublicBenefits,
			IsEnrolledInPublicBenefits = @IsEnrolledInPublicBenefits,
			IsRequestingReconsideration = @IsRequestingReconsideration,
			IsNotGivingConsent = @IsNotGivingConsent,
			IsOtherChildEnrolled = @IsOtherChildEnrolled,
			IsReconsiderationOfAdjustment = @IsReconsiderationOfAdjustment,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE 
			SelfPayID = @SelfPayID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'SelfPay', @AuditDetailID, @SelfPayID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
