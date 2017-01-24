-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactConsents]
-- Author:		Scott Martin
-- Date:		04/08/2016
--
-- Purpose:		Gets a list of contact consents
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Name			Description
-- 04/08/2016	Scott Martin		Initial creation.
-- 04/10/2016	Scott Martin		Added ConsentName
-- 05/09/2016	Scott Martin		Added sorting by ModifiedOn
-- 06/01/2016	Scott Martin		Removed Reference to ContactConsentExpiration
-- 06/02/2016	Gurpreet Singh		Added ExpiredResponseID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactConsents]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		CC.ContactConsentID,
		CC.AssessmentID,
		CC.AssessmentSectionID,
		A.Name AS ConsentName,
		ResponseID,
		ExpiredResponseID,
		CC.DateSigned,
		EffectiveDate,
		ExpirationDate,
		ExpirationReasonID,
		ExpiredBy,
		AER.AssessmentExpirationReason AS ExpirationReason,
		CC.SignatureStatusID,
		SS.SignatureStatus,
		CC.ModifiedOn
	FROM
		Registration.ContactConsent CC
		LEFT OUTER JOIN Reference.AssessmentExpirationReason AER
			ON CC.ExpirationReasonID = AER.AssessmentExpirationReasonID
		LEFT OUTER JOIN Reference.SignatureStatus SS
			ON CC.SignatureStatusID = SS.SignatureStatusID
		LEFT OUTER JOIN Core.Assessments A
			ON CC.AssessmentID = A.AssessmentID
	WHERE
		CC.ContactID = @ContactID
		AND CC.IsActive = 1
	ORDER BY
		EffectiveDate DESC,
		CC.ModifiedOn DESC;	
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END