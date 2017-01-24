-----------------------------------------------------------------------------------------------------------------------
-- View:	    [Core].[vw_ServiceRecordingDetails]
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
-- 11/28/2016	Sumana Sangapu	Return SenttoCMHCDate 
-- 12/09/2016	Sumana Sangapu	Return only the latest Signature dates to avoid duplicate records in the output
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW Core.vw_ServiceRecordingDetails
AS
SELECT
	SR.ServiceRecordingID,
	SRV.ServiceRecordingVoidID,
	SR.SourceHeaderID,
	SR.ServiceRecordingSourceID,
	CASE
		WHEN SRV.ServiceRecordingVoidID IS NOT NULL THEN 1
		ELSE 0 END AS IsVoided,
	SRV.CreatedOn AS VoidDate,
	SRVR.ServiceRecordingVoidReason AS VoidReason,
	CASE
		WHEN SRV.ServiceRecordingVoidID IS NOT NULL THEN CONCAT(VoidUser.LastName, ', ', VoidUser.FirstName)
		ELSE NULL END AS VoidUser,
	OSD3.MappingID AS CompanyID,
	OSD3.Name AS CompanyName,
	OSD2.MappingID AS DivisionID,
	OSD2.Name AS DivisionName,
	OSD1.MappingID AS ProgramID,
	OSD1.Name AS ProgramName,
	OSD.MappingID AS ProgramUnitID,
	OSD.Name AS ProgramUnitName,
	ST.ServiceTypeID,
	ST.ServiceType,
	S.ServicesID,
	S.ServiceName,
	SS.ServiceStatusID,
	SS.ServiceStatus,
	SR.ServiceStartDate,
	SR.ServiceEndDate,
	SR.ServiceStartDate AS ServiceStartTime,
	SR.ServiceEndDate AS ServiceEndTime,
	CONVERT(VARCHAR(12), DATEADD(second, DATEDIFF(second, SR.ServiceStartDate, SR.ServiceEndDate), 0), 108) AS Duration,
	DM.DeliveryMethodID,
	DM.DeliveryMethod,
	ATS.AttendanceStatusID,
	ATS.AttendanceStatus,
	SL.ServiceLocationID,
	SL.ServiceLocation AS PlaceOfService,
	SR.TrackingFieldID,
	TF.TrackingField,
	RC.CodeDescription AS Recipient,
	U.UserID AS ServiceProviderID,
	CONCAT(U.LastName, ', ', U.FirstName) AS ServiceProvider,
	CR.CredentialID,
	CR.CredentialAbbreviation,
	NULL AS Diagnosis,
	CASE
		WHEN SU.UserID IS NOT NULL THEN CONCAT(SU.LastName, ', ', SU.FirstName)
		ELSE NULL END AS SupervisingProvider,
	STUFF((SELECT '; ' + CONCAT(U.LastName, ', ', U.FirstName)
		FROM
			Core.ServiceRecordingAdditionalUser SRAU
			INNER JOIN Core.Users U
				ON SRAU.UserID = U.UserID
		WHERE
			SRAU.ServiceRecordingID = SR.ServiceRecordingID  FOR XML PATH('')),1,1,'') AS CoProvider,
	DESS.SignatureDate AS SignatureDate,
	SentToCMHCDate AS RecordedServiceSentDate,
	SR.TransmittalStatus,
	SR.SentToCMHCDate
FROM
	Core.ServiceRecording SR
	LEFT OUTER JOIN Core.ServiceRecordingVoid SRV
		ON SR.ServiceRecordingID = SRV.ServiceRecordingID
	LEFT OUTER JOIN Reference.ServiceType ST
		ON SR.ServiceTypeID = ST.ServiceTypeID
	LEFT OUTER JOIN Reference.Services S
		ON SR.ServiceItemID = S.ServicesID
	LEFT OUTER JOIN Reference.ServiceStatus SS
		ON SR.ServiceStatusID = SS.ServiceStatusID
	LEFT OUTER JOIN Reference.DeliveryMethod DM
		ON SR.DeliveryMethodID = DM.DeliveryMethodID
	LEFT OUTER JOIN Reference.AttendanceStatus ATS
		ON SR.AttendanceStatusID = ATS.AttendanceStatusID
	LEFT OUTER JOIN Reference.ServiceLocation SL
		ON SR.ServiceLocationID = SL.ServiceLocationID
	LEFT OUTER JOIN Reference.RecipientCode RC
		ON SR.RecipientCodeID = RC.CodeID
	LEFT OUTER JOIN Reference.ServiceRecordingVoidReason SRVR
		ON SRV.ServiceRecordingVoidReasonID = SRVR.ServiceRecordingVoidReasonID
	LEFT OUTER JOIN Core.Users VoidUser
		ON SRV.CreatedBy = VoidUser.UserID
	LEFT OUTER JOIN Core.Users U
		ON SR.UserID = U.UserID
	LEFT OUTER JOIN Core.Users SU
		ON SR.SupervisorUserID = SU.UserID
	LEFT OUTER JOIN ( SELECT * FROM ( SELECT DocumentID, EntitySignatureID, DESS.CreatedOn AS SignatureDate, ROW_NUMBER() OVER(PARTITION BY DESS.DocumentID ORDER BY DESS.SystemCreatedOn DESC) as RN
									  FROM ESignature.DocumentEntitySignatures DESS 
									  WHERE DESS.DocumentTypeID = 9 
									) DESS1
					  WHERE RN = 1 ) DESS
		ON SR.ServiceRecordingID = DESS.DocumentID
	LEFT OUTER JOIN ESignature.EntitySignatures ES
		ON DESS.EntitySignatureID = ES.EntitySignatureID
	LEFT OUTER JOIN Reference.Credentials CR
		ON ES.CredentialID = CR.CredentialID
	LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD
			ON SR.OrganizationID = OSD.MappingID
	LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD1
		ON OSD.ParentID = OSD1.MappingID
	LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD2
		ON OSD1.ParentID = OSD2.MappingID
	LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD3
		ON OSD2.ParentID = OSD3.MappingID
	LEFT OUTER JOIN Reference.TrackingField TF
		ON SR.TrackingFieldID = TF.TrackingFieldID
GO
