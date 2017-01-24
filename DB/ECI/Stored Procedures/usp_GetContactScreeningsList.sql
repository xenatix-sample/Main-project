/****** Object:  StoredProcedure [ECI].[usp_GetContactScreeningsList]    Script Date: 10/29/2015 1:59:44 AM ******/
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetContactScreeningsList]
-- Author:		Sumana Sangapu
-- Date:		10/08/2015
--
-- Purpose:		Get the list of screenings for that contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	Sumana Sangapu	TFS:2620	Initial Creation
-- 10/29/2015	D Christopher				Made consistent with usp_GetScreeningDetails
-- 12/09/2015   Arun Choudhary  TFS:4244    Added orderby on ScreeningDate.
-- exec  [ECI].[usp_GetContactScreeningsList] 1,'',''
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetContactScreeningsList]
	@ContactID					bigint,
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
						s.IsActive, s.ModifiedBy, s.ModifiedOn, s.ResponseID as ResponseID,
						(select top 1 AssessmentSectionID from Core.AssessmentSections where AssessmentID = a.AssessmentID and IsActive = 1 order by SortOrder asc) as SectionID
					FROM	ECI.ScreeningContact sc
					INNER JOIN ECI.Screening s
					ON		s.ScreeningID = sc.ScreeningID
					AND		s.IsActive = sc.IsActive 
					LEFT JOIN Reference.Program p
					ON		s.ProgramID = p.ProgramID
					LEFT JOIN [ECI].[ScreeningType] st
					ON		s.ScreeningTypeID = st.ScreeningTypeID	
					LEFT JOIN [Core].[Assessments] a
					ON		s.AssessmentID = a.AssessmentID
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
					WHERE	sc.ContactID = @ContactID
					AND		sc.IsActive = 1
					ORDER BY ScreeningDate DESC
		  END TRY
		  BEGIN CATCH
			SELECT
			  @ResultCode		= ERROR_SEVERITY(),
			  @ResultMessage	= ERROR_MESSAGE()
		  END CATCH
END