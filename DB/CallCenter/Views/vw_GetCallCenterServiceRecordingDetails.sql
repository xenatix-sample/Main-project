-----------------------------------------------------------------------------------------------------------------------
-- View:	    [Core].[vw_GetCallCenterServiceRecordingDetails]
-- Author:		Scott Martin
-- Date:		06/27/2016
--
-- Purpose:		View to get the Service Recording details
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/27/2016	Scott Martin	Initial Creation
-- 08/02/2016   Gurpreet Singh	Updated where caluse to include ServiceRecordingSourceID
-- 09/14/2016	Gaurav Gupta	Update Service Recording Document Type ID and Sign Joined with Service Recording.
-- 11/28/2016	Sumana Sangapu	Return SystemCreatedOn and SystemModifiedOn columns from CallCenter.CallCenterHeader and SentTOCMHCDate 
-- 12/02/2016	Sumana Sangapu	Return ServiceStartDate and ServiceEndDate from Core.vw_ServiceRecordingDetails instead of CallCenter
-- 12/14/2016	Scott Martin	Added IsActive check for call center header
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW CallCenter.vw_GetCallCenterServiceRecordingDetails
AS
WITH ActiveStatus (ContactID, IsCompanyActive)
AS
(
	SELECT
		CAD.ContactID,
		MAX(CAST(CAD.IsCompanyActive AS INT))
	FROM
		Registration.vw_ContactAdmissionDischarge CAD
	GROUP BY
		CAD.ContactID
)
SELECT
	SR.ServiceRecordingID,
	SR.ServiceRecordingVoidID,
	SR.SourceHeaderID,
	SR.ServiceRecordingSourceID,
	SR.IsVoided,
	SR.VoidDate,
	SR.VoidReason,
	SR.VoidUser,
	C.ContactID,
	C.MRN,
	C.FirstName,
	UPPER(LEFT(C.Middle, 1)) AS MiddleInitial,
	C.LastName,
	G.Gender,
	C.DOB,
	SR.CompanyID,
	SR.CompanyName,
	SR.DivisionID,
	SR.DivisionName,
	SR.ProgramID,
	SR.ProgramName,
	SR.ProgramUnitID,
	SR.ProgramUnitName,
	SR.ServiceTypeID,
	SR.ServiceType,
	SR.ServicesID,
	SR.ServiceName,
	SR.ServiceStatusID,
	SR.ServiceStatus,
	SR.ServiceStartDate AS ServiceStartDate,
	SR.ServiceEndDate AS ServiceEndDate,
	SR.ServiceStartTime AS ServiceStartTime,
	SR.ServiceEndTime AS ServiceEndTime,
	CONVERT(VARCHAR(12), DATEADD(second, DATEDIFF(second, SR.ServiceStartTime, SR.ServiceEndTime), 0), 108) AS Duration,
	SR.DeliveryMethodID,
	SR.DeliveryMethod,
	SR.AttendanceStatusID,
	SR.AttendanceStatus,
	SR.ServiceLocationID,
	SR.PlaceOfService,
	SR.TrackingFieldID,
	SR.TrackingField,
	SR.Recipient,
	SR.ServiceProviderID,
	SR.ServiceProvider,
	--SR.CredentialID,
	--SR.CredentialAbbreviation,
	CR.CredentialID,
	CR.CredentialAbbreviation,
	SR.Diagnosis,
	SR.SupervisingProvider,
	SR.CoProvider,
	--SR.SignatureDate,
	DESS.CreatedOn AS SignatureDate,
	SR.RecordedServiceSentDate,
	SR.TransmittalStatus,
	CAST(ISNULL(ActiveStatus.IsCompanyActive, 0) AS BIT) AS IsCompanyActive,
	PN.ProgressNoteID,
	CASE
		WHEN SR.ServiceRecordingSourceID = 6  AND IsVoided = 1 THEN 'Void'
		WHEN SR.ServiceRecordingSourceID = 6  AND DESS.CreatedOn IS NOT NULL THEN 'Completed'
		WHEN SR.ServiceRecordingSourceID = 6  AND DESS.CreatedOn IS NULL THEN 'Draft'
		ELSE CS.CallStatus END AS DocumentStatus,
	CCH.SystemCreatedOn as CallCenterHeaderSystemCreatedOn,
	CCH.SystemModifiedOn as CallCenterHeaderSystemModifiedOn,
	SR.SentToCMHCDate
FROM
	Core.vw_ServiceRecordingDetails SR
	INNER JOIN CallCenter.CallCenterHeader CCH
		ON SR.SourceHeaderID = CCH.CallCenterHeaderID
		AND CCH.IsActive = 1
	LEFT OUTER JOIN Registration.Contact C
		ON CCH.ContactID = C.ContactID
	LEFT OUTER JOIN Reference.Gender G
		ON C.GenderID = G.GenderID
	LEFT OUTER JOIN ActiveStatus
		ON CCH.ContactID = ActiveStatus.ContactID
	LEFT OUTER JOIN CallCenter.ProgressNote PN
		ON CCH.CallCenterHeaderID = PN.CallCenterHeaderID
	LEFT OUTER JOIN ESignature.DocumentEntitySignatures DESS
		ON SR.ServiceRecordingID = DESS.DocumentID
		AND DESS.DocumentTypeID = 9
	LEFT OUTER JOIN ESignature.EntitySignatures ES
		ON DESS.EntitySignatureID = ES.EntitySignatureID
	LEFT OUTER JOIN Reference.Credentials CR
		ON ES.CredentialID = CR.CredentialID
	LEFT OUTER JOIN CallCenter.CallStatus CS
		ON CCH.CallStatusID = CS.CallStatusID
WHERE
	SR.ServiceRecordingSourceID IN (1, 6) --Call Center
GO
