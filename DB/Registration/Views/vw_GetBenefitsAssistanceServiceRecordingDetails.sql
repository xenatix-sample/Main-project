-----------------------------------------------------------------------------------------------------------------------
-- View:	    [Core].[vw_GetBenefitsAssistanceServiceRecordingDetails]
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
-- 06/29/2016	Scott Martin	Initial Creation
-- 11/28/2016	Sumana Sangapu	Return NULL as CallCenterHeaderSystemCreatedOn & CallCenterHeaderSystemModifiedOn & SentTOCMHCDate 
-- 01/19/2017	Scott Martin	Added a clause to return only active Benefits Assistance records
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW Registration.vw_GetBenefitsAssistanceServiceRecordingDetails
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
	SR.ServiceStartDate,
	SR.ServiceEndDate,
	SR.ServiceStartTime,
	SR.ServiceEndTime,
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
	SR.CredentialID,
	SR.CredentialAbbreviation,
	SR.Diagnosis,
	SR.SupervisingProvider,
	SR.CoProvider,
	SR.SignatureDate,
	SR.RecordedServiceSentDate,
	SR.TransmittalStatus,
	CAST(ISNULL(ActiveStatus.IsCompanyActive, 0) AS BIT) AS IsCompanyActive,
	BA.BenefitsAssistanceID AS ProgressNoteID,
	DS.DocumentStatus,
	NULL as CallCenterHeaderSystemCreatedOn,
	NULL as CallCenterHeaderSystemModifiedOn,
	SR.SentToCMHCDate
FROM
	Core.vw_ServiceRecordingDetails SR
	INNER JOIN Registration.BenefitsAssistance BA
		ON SR.SourceHeaderID = BA.BenefitsAssistanceID
	LEFT OUTER JOIN Registration.Contact C
		ON BA.ContactID = C.ContactID
	LEFT OUTER JOIN Reference.Gender G
		ON C.GenderID = G.GenderID
	LEFT OUTER JOIN ActiveStatus
		ON BA.ContactID = ActiveStatus.ContactID
	LEFT OUTER JOIN Reference.DocumentStatus DS
		ON BA.DocumentStatusID = DS.DocumentStatusID
WHERE
	SR.ServiceRecordingSourceID = 4 --Benefits Assistance
	AND BA.IsActive = 1
GO
