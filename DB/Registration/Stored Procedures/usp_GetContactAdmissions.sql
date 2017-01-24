----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactAdmissions]
-- Author:		Scott Martin
-- Date:		03/21/2016
--
-- Purpose:		Gets organization levels the contact has been admitted to
--
-- Notes:		IsCompanyActiveForProgramUnit is a flag to indicate a contact's Status in the company; 1 = Active, 0 = Inactive
--				IsProgramUnitActive is a flag used for the Company Discharge screen to indicate if the contact is admitted to any Program Unit for the company; 1 = Active, 0 = Inactive
--				IsDischarged just indicates if that admission has been discharge whether it is Program Unit or Company
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/21/2016	Scott Martin		Initial creation.
-- 03/26/2016	Scott Martin		Added a Reference to ContactDischargeNote to replace removed fields in ContactAdmission table
-- 03/29/2016	Scott Martin		Merged EffectiveDate and EffectiveStartTime
-- 04/05/2016	Scott Martin		Renamed 2 derived fields
-- 04/17/2016   Satish Singh	    Added IsCompanyActive
-- 04/18/2016   Gurmant Singh	    Added ModifiedOn Column
-- 06/18/2016   Gurmant Singh	    Get the EntityID, EntityName and DateSigned fields
-- 06/30/2016	Scott Martin		Removed signature data from the view
-- 07/06/2016	Arun Choudhary		TFS #12211
-- 10/05/2016   Vishal Joshi		Included DOB into return select list
-- 11/02/2016   Gaurav Gupta		Added AdmissionReasonID field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactAdmissions]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		ContactAdmissionID,
		CAD.ContactID,
		CompanyID,
		DivisionID,
		ProgramID,
		ProgramUnitID,
		DataKey,
		EffectiveDate,
		(CASE 
			WHEN DischargeDateModifiedOn IS NOT NULL AND (DischargeDateModifiedOn > EffDateModifiedOn)
				THEN DischargeDateModifiedOn
			ELSE EffDateModifiedOn
		END) AS ModifiedOn,
		UserID,
		ContactDischargeNoteID,
		DischargeDate,
		IsDischarged,
		IsDocumentationComplete,
		IsCompanyActive,
		Comments,
		AdmissionReasonID,
		ISNULL((SELECT MAX(CASE WHEN IsDischarged = 1 THEN 0 ELSE 1 END) FROM Registration.vw_ContactAdmissionDischarge WHERE ContactID = CAD.ContactID AND CompanyID = CAD.CompanyID AND DataKey = 'Company'), 0) AS IsCompanyActiveForProgramUnit,
		ISNULL((SELECT MAX(CASE WHEN IsDischarged = 1 THEN 0 ELSE 1 END) FROM Registration.vw_ContactAdmissionDischarge WHERE ContactID = CAD.ContactID AND CompanyID = CAD.CompanyID AND DataKey = 'ProgramUnit'), 0) AS IsProgramUnitActiveForCompany,
		ES.EntityID SignedByEntityID,
		ES.EntityName SignedByEntityName,
		ES.ModifiedOn AS DateSigned,
		C.DOB
	FROM
		Registration.vw_ContactAdmissionDischarge CAD
		LEFT OUTER JOIN [ESignature].[DocumentEntitySignatures] DES
			ON CAD.ContactAdmissionID = DES.DocumentID
			AND DES.DocumentTypeID = 8
		LEFT OUTER JOIN [ESignature].[EntitySignatures] ES
			ON ES.EntitySignatureID = DES.EntitySignatureID
		LEFT OUTER JOIN [ESignature].[Signatures] S
			ON S.SignatureID = ES.SignatureID 
		LEFT JOIN [Registration].[Contact] C 
		    ON CAD.ContactID = C.ContactID 

	WHERE
		CAD.ContactID = @ContactID
	ORDER BY
		EffectiveDate DESC,
		DataKey DESC;
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
