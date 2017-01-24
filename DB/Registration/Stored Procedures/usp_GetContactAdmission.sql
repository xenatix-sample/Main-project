----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactAdmission]
-- Author:		Scott Martin
-- Date:		03/21/2016
--
-- Purpose:		Gets specific organization level for contact
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
-- 11/02/2016   Gaurav Gupta		Added AdmissionReasonID fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactAdmission]
	@ContactAdmissionID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		ContactAdmissionID,
		ContactID,
		CompanyID,
		DivisionID,
		ProgramID,
		ProgramUnitID,
		DataKey,
		EffectiveDate,
		AdmissionReasonID,
		UserID,
		ContactDischargeNoteID,
		DischargeDate,
		IsDischarged,
		IsDocumentationComplete,
		Comments,
		ISNULL((SELECT MAX(CASE WHEN IsDischarged = 1 THEN 0 ELSE 1 END) FROM Registration.vw_ContactAdmissionDischarge WHERE ContactID = CAD.ContactID AND CompanyID = CAD.CompanyID AND DataKey = 'Company'), 0) AS IsCompanyActiveForProgramUnit,
		ISNULL((SELECT MAX(CASE WHEN IsDischarged = 1 THEN 0 ELSE 1 END) FROM Registration.vw_ContactAdmissionDischarge WHERE ContactID = CAD.ContactID AND CompanyID = CAD.CompanyID AND DataKey = 'ProgramUnit'), 0) AS IsProgramUnitActiveForCompany
	FROM
		Registration.vw_ContactAdmissionDischarge CAD
	WHERE
		ContactAdmissionID = @ContactAdmissionID;		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
