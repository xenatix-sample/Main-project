-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_GetScreeningDetails]
-- Author:		Chad Roberts
-- Date:		11/01/2015
--
-- Purpose:		Get a specific ISP
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2015	Chad Roberts	TFS:3058	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetIFSP]
	@IFSPID						bigint,
	@ResultCode					int OUTPUT,
	@ResultMessage				nvarchar(500) OUTPUT
AS
BEGIN
		  SELECT
			@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

		  BEGIN TRY

					SELECT
						ifsp.ContactID, ifsp.IFSPID, ifsp.AssessmentID, ifsp.AssistiveTechnologyNeeded, ifsp.Comments,
						ifsp.IFSPFamilySignedDate, ifsp.IFSPMeetingDate, ifsp.IFSPTypeID, ifsp.MeetingDelayed, ifsp.ReasonForDelayID, ifsp.ResponseID,
						ifsp.IsActive, ifsp.ModifiedBy, ifsp.ModifiedOn
					FROM	ECI.IFSP ifsp
					WHERE ifsp.IFSPID = @IFSPID
		  END TRY
		  BEGIN CATCH
			SELECT
			  @ResultCode		= ERROR_SEVERITY(),
			  @ResultMessage	= ERROR_MESSAGE()
		  END CATCH
END