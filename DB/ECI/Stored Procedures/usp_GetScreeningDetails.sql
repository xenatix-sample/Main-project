-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_GetScreeningDetails]
-- Author:		Sumana Sangapu
-- Date:		10/07/2015
--
-- Purpose:		Get the Screening Details per ScreeningID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2015	Sumana Sangapu	TFS:2620	Initial Creation
-- 10/29/2015	D Christopher				Made consistent with usp_GetContactScreeningsList
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetScreeningDetails]
	@ScreeningID				bigint,
	@ResultCode					int OUTPUT,
	@ResultMessage				nvarchar(500) OUTPUT
AS
BEGIN
		  SELECT
			@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

		  BEGIN TRY

					SELECT
						sc.ContactID, s.ScreeningID, s.ProgramID as ProgramID, p.ProgramName as ProgramName, InitialContactDate,
						InitialServiceCoordinatorID, uInitial.FirstName + ' ' + uInitial.LastName as InitialServiceCoordinator,
						PrimaryServiceCoordinatorID, uPrimary.FirstName + ' ' + uPrimary.LastName as PrimaryServiceCoordinator,
						ScreeningDate, st.ScreeningTypeID as ScreeningTypeID, ScreeningType, a.AssessmentID, a.Name As AssessmentName,
						s.ScreeningResultsID as ScreeningResultID, ScreeningResult, ScreeningScore,
						s.ScreeningStatusID, s.SubmittedByID, u.FirstName + ' ' + u.LastName As SubmittedBy,
						s.IsActive, s.ModifiedBy, s.ModifiedOn, s.ResponseID as ResponseID
					FROM	ECI.Screening s
					INNER JOIN ECI.ScreeningContact sc
					ON		s.ScreeningID = sc.ScreeningID
					AND		s.IsActive = sc.IsActive AND s.IsActive = 1
					LEFT JOIN Reference.Program p
					ON		s.ProgramID = p.ProgramID
					LEFT JOIN [ECI].[ScreeningType] st
					ON		s.ScreeningTypeID = st.ScreeningTypeID	
					LEFT JOIN [Core].[Assessments] a
					ON		s.AssessmentID = a.AssessmentID
					--LEFT JOIN [Core].[AssessmentResponses] resp
					--ON		resp.AssessmentID	= a.AssessmentID
					LEFT JOIN [ECI].[ScreeningResults] r
					ON		s.ScreeningResultsID = r.ScreeningResultsID
					LEFT JOIN [ECI].[ScreeningStatus] su
					ON		su.ScreeningStatusID = s.ScreeningStatusID
					LEFT JOIN [Core].Users u
					ON		u.UserID = s.SubmittedByID
					LEFT JOIN [Core].Users uInitial
					ON		uInitial.UserID = s.InitialServiceCoordinatorID
					LEFT JOIN [Core].Users uPrimary
					ON		uPrimary.UserID = s.PrimaryServiceCoordinatorID		
					WHERE	s.ScreeningID = @ScreeningID 	
		  END TRY
		  BEGIN CATCH
			SELECT
			  @ResultCode		= ERROR_SEVERITY(),
			  @ResultMessage	= ERROR_MESSAGE()
		  END CATCH
END