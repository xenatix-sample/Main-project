
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCrisisLine]
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
-- 01/27/2016	John Crossen   5714	- Initial creation.
-- 02/01/2016	Gurpreet Singh	Refactoring sproc
-- 02/04/20116	Gurpreet Singh	Added FollowUpRequired
-- 02/16/2016	Rajiv Ranjan	Added providerID & dateOfIncident
-- 03/03/2016	Gaurav Gupta	Added referral agency id
-- 04/27/2016                   Added OtherReferralAgency
-- 05/10/2016   Lokesh Singhal  Added IsLinkedToContact
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetCrisisLine]
	@CallCenterHeaderID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
	@ResultMessage = 'Executed Successfully'

	BEGIN TRY
		SELECT  CH.CallCenterHeaderID,
				CH.ParentCallCenterHeaderID,
			    CH.CallCenterTypeID, 
				CH.CallerID, 
				CH.ContactID, 
				CH.ContactTypeID,
				CH.ProviderID,
				CH.CallStartTime, 
				CH.CallEndTime, 
				CH.CallStatusID, 
				CH.ProgramUnitID, 
			    CC.ReferralAgencyID,
				CC.OtherReferralAgency,
				CH.CountyID,
				CC.CallCenterPriorityID, 
				CC.SuicideHomicideID, 
				CC.DateOfIncident,
				CC.ReasonCalled, 
				CC.Disposition, 
				CC.OtherInformation, 
				CC.FollowUpRequired,
				CC.Comments,
				CH.ModifiedBy, 
				CH.ModifiedOn,
				CH.IsLinkedToContact
		FROM 
				CallCenter.CallCenterHeader CH 
				LEFT JOIN CallCenter.CrisisCall CC ON CC.CallCenterHeaderID = CH.CallCenterHeaderID
		WHERE 
				CH.CallCenterHeaderID=@CallCenterHeaderID  
	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

