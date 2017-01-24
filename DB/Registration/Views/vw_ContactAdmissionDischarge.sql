-----------------------------------------------------------------------------------------------------------------------
-- View:	    Registration.vw_ContactAdmissionDischarge
-- Author:		Scott Martin
-- Date:		03/29/2016
--
-- Purpose:		View to get the Contact Admission and Discharge details
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/29/2016	Scott Martin	Initial Creation 
-- 04/17/2016   Satish Singh    IsActive added
-- 04/18/2016   Gurmant Singh   Added EffDateModifiedOn and DischargeDateModifiedOn column
-- 06/18/2016   Gurmant Singh   Added a Reference to DocumentEntitySignatures to get the DateSigned and Entity fields
-- 11/08/2016	Scott Martin	Added With(Nolock)
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW Registration.vw_ContactAdmissionDischarge
AS
SELECT
		CA.ContactAdmissionID,
		CA.ContactID,
		OSD3.MappingID AS CompanyID,
		OSD2.MappingID AS DivisionID,
		OSD1.MappingID AS ProgramID,
		OSD.MappingID AS ProgramUnitID,
		CA.IsActive AS IsCompanyActive,
		OSD.DataKey,
		CA.EffectiveDate,
		CA.ModifiedOn EffDateModifiedOn,
		CDN.ModifiedOn DischargeDateModifiedOn,
		CA.UserID,
		CDN.ContactDischargeNoteID,
		CDN.DischargeDate,
		CA.IsDocumentationComplete,
		CAST(CASE
			WHEN CDN.ContactDischargeNoteID IS NOT NULL AND CDN.SignatureStatusID = 2 THEN 1
			ELSE 0 END AS BIT) AS IsDischarged,
		CA.Comments,
		CA.AdmissionReasonID
	FROM
		Registration.ContactAdmission CA WITH(NOLOCK)
		LEFT OUTER JOIN Registration.ContactDischargeNote CDN WITH(NOLOCK)
			ON CA.ContactAdmissionID = CDN.ContactAdmissionID
			AND CDN.IsActive = 1
		INNER JOIN Core.vw_GetOrganizationStructureDetails OSD
			ON CA.OrganizationID = OSD.MappingID
		INNER JOIN Core.vw_GetOrganizationStructureDetails OSD1
			ON OSD.ParentID = OSD1.MappingID
		INNER JOIN Core.vw_GetOrganizationStructureDetails OSD2
			ON OSD1.ParentID = OSD2.MappingID
		INNER JOIN Core.vw_GetOrganizationStructureDetails OSD3
			ON OSD2.ParentID = OSD3.MappingID
	WHERE
		CA.IsActive = 1
	UNION ALL
	SELECT
			CA.ContactAdmissionID,
			CA.ContactID,
			OSD.MappingID AS CompanyID,
			NULL AS DivisionID,
			NULL AS ProgramID,
			NULL AS ProgramUnitID,
			CA.IsActive AS IsCompanyActive,
			OSD.DataKey,
			CA.EffectiveDate,
			CA.ModifiedOn EffDateModifiedOn,
			CDN.ModifiedOn DischargeDateModifiedOn,
			CA.UserID,
			CDN.ContactDischargeNoteID,
			CDN.DischargeDate,
			CA.IsDocumentationComplete,
			CAST(CASE
				WHEN CDN.ContactDischargeNoteID IS NOT NULL THEN 1
				ELSE 0 END AS BIT) AS IsDischarged,
			CA.Comments,
			CA.AdmissionReasonID
		FROM
			Registration.ContactAdmission CA
			LEFT OUTER JOIN Registration.ContactDischargeNote CDN
				ON CA.ContactAdmissionID = CDN.ContactAdmissionID
				AND CDN.IsActive = 1
			INNER JOIN Core.vw_GetOrganizationStructureDetails OSD
				ON CA.OrganizationID = OSD.MappingID
		WHERE
			CA.IsActive = 1
			AND OSD.DataKey = 'Company'
