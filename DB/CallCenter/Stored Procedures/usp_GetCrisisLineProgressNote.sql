
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCrisisLineProgressNote]
-- Author:		John Crossen
-- Date:		01/27/2016
--
-- Purpose:		Get Proc for CallCenter
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/27/2016	John Crossen   6115	- Initial creation.
-- 04/03/2016	Scott Martin	Joined to new ProgressNote table
-- 07/07/2016	Lokesh Singhal	Added IsCallerSame & NewCallerID fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetCrisisLineProgressNote]
	@CallCenterHeaderID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
	@ResultMessage = 'Executed Successfully'

	BEGIN TRY
		SELECT  CH.CallCenterHeaderID,
			    CH.CallCenterTypeID, 
				CH.CallerID, 
				CH.ContactID, 
				CH.CallStartTime, 
				CH.CallEndTime, 
				CH.CallStatusID, 
				CH.ProgramUnitID, 
				CH.CountyID,
				CC.CallCenterPriorityID, 
				CC.SuicideHomicideID, 
				CC.ReasonCalled, 
				PN.Disposition, 
				CC.OtherInformation, 
				CC.FollowUpRequired,
				PN.Comments,
				PN.CallTypeID,
				PN.CallTypeOther,
				PN.FollowupPlan,
				PN.NatureofCall,
				PN.ProgressNoteID,
				CC.ClientStatusID,
				CC.ClientProviderID,
				CC.NoteHeaderID,
				CC.BehavioralCategoryID,
				CH.ModifiedBy, 
				CH.ModifiedOn,
				PN.IsCallerSame,
				PN.NewCallerID
		FROM 
				CallCenter.CallCenterHeader CH 
				LEFT JOIN CallCenter.CrisisCall CC
					ON CC.CallCenterHeaderID = CH.CallCenterHeaderID
				LEFT OUTER JOIN CallCenter.ProgressNote PN
					ON CH.CallCenterHeaderID = PN.CallCenterHeaderID
					AND PN.NoteHeaderID NOT IN (SELECT NoteHeaderID FROM Registration.NoteHeaderVoid NHV)
		WHERE 
				CH.CallCenterHeaderID = @CallCenterHeaderID  
	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

