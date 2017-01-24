-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddSelfPay]
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
-- 12/04/2016   Gaurav Gupta    Change order of ModifiedOn and ModifiedBy
-- 04/28/2016   Lokesh Singhal  Change scale value of parameter as per database
-- 09/30/2016   Gaurav Gupta    Change isParcent if null pass 0 
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddSelfPay]
	@ContactID BIGINT,
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
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

		BEGIN TRY
			SELECT	@ResultCode = 0,
					@ResultMessage = 'executed successfully';

			INSERT INTO Registration.SelfPay
			(
				ContactID,
				OrganizationDetailID,
				SelfPayAmount,
				IsPercent,
				EffectiveDate,
				ExpirationDate,
				IsChildInConservatorship,
				IsNotAttested,
				IsApplyingForPublicBenefits,
				IsEnrolledInPublicBenefits,
				IsRequestingReconsideration,
				IsNotGivingConsent,
				IsOtherChildEnrolled,
				IsReconsiderationOfAdjustment,
				IsActive,
				ModifiedBy,
				ModifiedOn,
				CreatedBy,
				CreatedOn
			)
			VALUES
			(
				@ContactID,
				@OrganizationDetailID,
				@SelfPayAmount,
				ISNULL(@IsPercent,0),
				@EffectiveDate,
				@ExpirationDate,
				@IsChildInConservatorship,
				@IsNotAttested,
				@IsApplyingForPublicBenefits,
				@IsEnrolledInPublicBenefits,
				@IsRequestingReconsideration,
				@IsNotGivingConsent,
				@IsOtherChildEnrolled,
				@IsReconsiderationOfAdjustment,
				1,
				@ModifiedBy,
				@ModifiedOn,
				@ModifiedBy,
				@ModifiedOn
			);

			SELECT @ID = SCOPE_IDENTITY();

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'SelfPay', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'SelfPay', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END TRY

		BEGIN CATCH
			SELECT	@ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE();
		END CATCH
END
GO

