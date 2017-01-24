----------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAssessmentsByContactID]
-- Author:		Gaurav Gupta
-- Date:		07/28/2016
--
-- Purpose:		Get Proc for Assessments
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/27/2016	Gaurav Gupta   12667	- Initial creation.
-- 08/04/2016	Gaurav Gupta   12667	- Remove user id from user signed and add is active record 
-- 17/08/2016   Gaurav Gupta   1322     - Added service recording ID column 
-- 09/14/2016	Gaurav Gupta            - Update Service Recording Document Type ID and Sign Joined with Service Recording.
-- 10/05/2016	Gaurav Gupta   15519	- Remove law laison screen for assessment report.
-- 11/15/2016	Scott Martin	Refactored query to be more efficient and to remove duplicates
-- 11/16/2016	Scott Martin	Removed BAPN and Contact Forms and filtered out Progress Notes
-- 11/17/2016	Scott Martin	Fixed a bug where Assessment was not being returned if no service was entered
-- 12/23/2016	Scott Martin	Added IDD form assessment
-- 12/29/2016	Scott Martin	Added AssessmentName for IDD forms assessment; CallCenterHeader cannot be NULL
-- 01/02/2017	Arun Choudhary	Added ServiceRecordingSourceID column
-- 01/19/2017	Scott Martin	Added a new Column called AssessmentName and changed the Source for IDD Intake
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentsByContactID] 
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY
	  ;WITH CCARD (ResponseID, ModifiedOn)
		AS
		(
		SELECT
			AR.ResponseID,
			MAX(ARD.ModifiedOn) AS ModifiedOn
		FROM
			Core.AssessmentResponseDetails ARD
			INNER JOIN Core.AssessmentResponses AR
				ON ARD.ResponseID = AR.ResponseID
		WHERE
			ARD.IsActive = 1
			AND AR.ContactID = @ContactID
		GROUP BY
			AR.ResponseID
		),
		CFARD (ResponseID, AssessmentName, ModifiedOn)
		AS
		(
		SELECT
			AR.ResponseID,
			CAS.SectionName,
			MAX(ARD.ModifiedOn) AS ModifiedOn
		FROM
			Core.AssessmentResponseDetails ARD
			INNER JOIN Core.AssessmentResponses AR
				ON ARD.ResponseID = AR.ResponseID
			INNER JOIN Core.AssessmentSections CAS
				ON ARD.AssessmentSectionID = CAS.AssessmentSectionID
		WHERE
			ARD.IsActive = 1
			AND ARD.AssessmentSectionID = 77
			AND AR.ContactID = @ContactID
		GROUP BY
			AR.ResponseID,
			CAS.SectionName
		)
		SELECT
			AR.AssessmentID,
			A.Name AS AssessmentName,
			AR.ResponseID,
			CCH.CallCenterHeaderID,
			CCT.CallCenterType AS Source,
			CCH.CallStartTime AS ServiceStartDate,
			CCH.ProviderID AS ProviderID,
			CCSRD.ServiceRecordingID,
			ISNULL(CCSRD.ProgramUnitID, CCH.ProgramUnitID) AS OrganizationID,
			CS.CallStatus AS CallStatus,
			ISNULL(CCSRD.IsVoided, 0) AS IsVoided,
			CCSRD.SignatureDate AS SignedOn,
			CCARD.ModifiedOn AS ModifiedOn,
			CCSRD.ServiceRecordingSourceID
		FROM
			Core.AssessmentResponses AR
			INNER JOIN Core.Assessments A
				ON AR.AssessmentID = A.AssessmentID
			INNER JOIN CallCenter.CallCenterAssessmentResponse CCAR
				ON AR.ResponseID = CCAR.ResponseID
			INNER JOIN CallCenter.CallCenterHeader CCH
				ON CCAR.CallCenterHeaderID = CCH.CallCenterHeaderID
			INNER JOIN CallCenter.CallCenterType CCT
				ON CCH.CallCenterTypeID = CCT.CallCenterTypeID
			LEFT OUTER JOIN CallCenter.CallStatus CS
				ON CCH.CallStatusID = CS.CallStatusID
			LEFT OUTER JOIN CallCenter.vw_GetCallCenterServiceRecordingDetails CCSRD
				ON CCAR.CallCenterHeaderID = CCSRD.SourceHeaderID
			LEFT OUTER JOIN Reference.ServiceRecordingSource SRS
				ON CCSRD.ServiceRecordingSourceID = SRS.ServiceRecordingSourceID
			LEFT OUTER JOIN CCARD
				ON AR.ResponseID = CCARD.ResponseID
		WHERE
			CCARD.ModifiedOn IS NOT NULL
			AND AR.AssessmentID NOT IN (10,11,12)
			AND AR.ContactID = @ContactID
		UNION
		SELECT
			AR.AssessmentID,
			CFARD.AssessmentName,
			AR.ResponseID,
			0 AS CallCenterHeaderID,
			A.Name AS Source,
			CF.CreatedOn AS ServiceStartDate,
			CFSRD.ServiceProviderID AS ProviderID,
			CFSRD.ServiceRecordingID,
			CFSRD.ProgramUnitID AS OrganizationID,
			CFSRD.DocumentStatus AS CallStatus,
			ISNULL(CFSRD.IsVoided, 0) AS IsVoided,
			CFSRD.SignatureDate AS SignedOn,
			CFARD.ModifiedOn AS ModifiedOn,
			CFSRD.ServiceRecordingSourceID
		FROM
			Core.AssessmentResponses AR
			INNER JOIN Registration.ContactForms CF
				ON CF.ResponseID = AR.ResponseID
			INNER JOIN Registration.vw_GetContactFormsServiceRecordingDetails CFSRD
				ON CF.ContactFormsID = CFSRD.SourceHeaderID
			INNER JOIN Core.Assessments A
				ON AR.AssessmentID = A.AssessmentID
			INNER JOIN CFARD
				ON AR.ResponseID = CFARD.ResponseID
		WHERE
			AR.ContactID = @ContactID
		ORDER BY
			ModifiedOn DESC;

  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH
END