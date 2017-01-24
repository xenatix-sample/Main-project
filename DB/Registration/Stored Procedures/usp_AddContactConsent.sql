-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactConsent]
-- Author:		Scott Martin
-- Date:		04/08/2016
--
-- Purpose:		Add Contact Consent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Name			Description
-- 04/08/2016	Scott Martin	Initial Creation
-- 06/07/2016	Gurpreet Singh	Removed parameters from query which will never be added
-- 06/09/2016	Gurpreet Singh	Restoring earlier removed params as it is required for offline 
-- 07/22/2016	RAV				Effective Date and Expiration Date are datetime in the Registration.ContactConsent 
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactConsent]
	@ContactID BIGINT,
	@AssessmentID BIGINT,
	@AssessmentSectionID BIGINT,
	@ResponseID BIGINT,
	@DateSigned DATE,
	@EffectiveDate datetime,
	@ExpirationDate datetime,
	@ExpirationReasonID INT,
	@ExpiredResponseID INT,
	@ExpiredBy INT,
	@SignatureStatusID INT,
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
		INSERT INTO Registration.ContactConsent
		(
			ContactID,
			AssessmentID,
			AssessmentSectionID,
			ResponseID,
			DateSigned,
			EffectiveDate,
			--ExpirationDate,			--Will always be null in case of add
			--ExpirationReasonID,
			--ExpiredResponseID,
			--ExpiredBy,
			SignatureStatusID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ContactID,
			@AssessmentID,
			@AssessmentSectionID,
			@ResponseID,
			@DateSigned,
			@EffectiveDate,
			--@ExpirationDate,
			--@ExpirationReasonID,
			--@ExpiredResponseID,
			--@ExpiredBy,
			@SignatureStatusID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @ID = SCOPE_IDENTITY();

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactConsent', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactConsent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
