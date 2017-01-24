-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactConsent]
-- Author:		Scott Martin
-- Date:		04/08/2016
--
-- Purpose:		Update Contact Consent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Name			Description
-- 04/08/2016	Scott Martin	Initial Creation
-- 06/01/2016	Scott Martin	Added ExpirationReasonID and ExpiredBy
-- 06/02/2016	Gurpreet Singh	Added ExpiredResponseID
-- 06/06/2016	Gurpreet Singh	Removed isActive and updated query to remove update parameters which should not be updated 
-- 06/09/2016	Gurpreet Singh	Restoring earlier removed params as it is required for offline 
-- 07/22/2016	RAV				Effective Date and Expiration Date are datetime in the Registration.ContactConsent 
-- 11/28/2016	Deepak Kumar	In case of "Consent Expiration" some details will not get updated
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactConsent]
	@ContactConsentID BIGINT,
	@ContactID BIGINT,
	@AssessmentID BIGINT,
	@AssessmentSectionID BIGINT,
	@ResponseID BIGINT,
	@DateSigned DATE,
	@EffectiveDate datetime, --If Effectivedate should not be updated, we should consider not passing it even and removing it from here
	@ExpirationDate datetime,
	@ExpirationReasonID INT,
	@ExpiredResponseID INT,
	@ExpiredBy INT,
	@SignatureStatusID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactConsent', @ContactConsentID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		IF(@AssessmentID=(select AssessmentID from Core.AssessmentSections Where SectionName='Consent Expiration'))
			Begin
				UPDATE Registration.ContactConsent
				SET	--ContactID = @ContactID,							-- will never be updated
					--AssessmentID = @AssessmentID,						-- will never be updated
					--AssessmentSectionID = @AssessmentSectionID,		-- will never be updated
					--ResponseID = @ResponseID,							-- will never be updated
					DateSigned = CASE ISNULL(DateSigned, cast(-53690 as datetime)) WHEN cast(-53690 as datetime) THEN @DateSigned ELSE DateSigned END,
					--EffectiveDate = @EffectiveDate,					-- will never be updated
					ExpirationDate = @ExpirationDate,
					ExpirationReasonID = @ExpirationReasonID,
					ExpiredResponseID =  @ExpiredResponseID,
					ExpiredBy =  @ExpiredBy,
					SignatureStatusID =  SignatureStatusID,
					ModifiedBy = @ModifiedBy,
					ModifiedOn = @ModifiedOn,
					SystemModifiedOn = GETUTCDATE()
				WHERE
					ContactConsentID = @ContactConsentID;
			End
		Else
			Begin
				UPDATE Registration.ContactConsent
				SET	--ContactID = @ContactID,							-- will never be updated
					--AssessmentID = @AssessmentID,						-- will never be updated
					--AssessmentSectionID = @AssessmentSectionID,		-- will never be updated
					--ResponseID = @ResponseID,							-- will never be updated
					DateSigned = CASE ISNULL(DateSigned, cast(-53690 as datetime)) WHEN cast(-53690 as datetime) THEN @DateSigned ELSE DateSigned END,
					--EffectiveDate = @EffectiveDate,					-- will never be updated
					ExpirationDate = ExpirationDate,
					ExpirationReasonID = ExpirationReasonID,
					ExpiredResponseID = ExpiredResponseID,
					ExpiredBy = ExpiredBy,
					SignatureStatusID = @SignatureStatusID,
					ModifiedBy = @ModifiedBy,
					ModifiedOn = @ModifiedOn,
					SystemModifiedOn = GETUTCDATE()
				WHERE
					ContactConsentID = @ContactConsentID;
			End
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactConsent', @AuditDetailID, @ContactConsentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
