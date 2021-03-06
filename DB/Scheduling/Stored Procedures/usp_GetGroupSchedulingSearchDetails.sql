-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetGroupSchedulingSearchDetails]
-- Author:		Sumana Sangapu
-- Date:		03/11/2016
--
-- Purpose:		Get Group Scheduling Search Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	Sumana Sangapu	Initial creation.
-- 03/11/2016	Sumana Sangapu	Modified the proc as per Acceptance Criteria
-- 04/14/2016	Sumana Sangapu	Mapped OrgDetailsMapping view

-- exec [Scheduling].[usp_GetGroupSchedulingSearchDetails] 'REhabGroup','',''
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetGroupSchedulingSearchDetails]
	@GroupName	nvarchar(100),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
				SELECT	@ResultCode = 0,
						@ResultMessage = 'Executed Successfully';

				DECLARE @CurrentDate date,
						@CurrentTime int

				--SET @CurrentDate = CONVERT(DATE,GETUTCDATE())
				--SET @CurrentTime = CAST(DATEPART(HOUR, GETUTCDATE()) AS VARCHAR)  + CAST(DATEPART(MINUTE, GETUTCDATE()) AS VARCHAR) 

		   
				SELECT	gh.GroupHeaderID as GroupID, gd.GroupDetailID, gd.GroupName as GroupName, gd.GroupTypeID as GroupTypeID,  at.GroupType as AppointmentType, 
						a.ProgramID,p.Name as ProgramName, a.FacilityID, f.FacilityName, 
						a.AppointmentID, a.AppointmentDate, a.AppointmentStartTime, 
						CASE WHEN RecurrenceID IS NULL THEN 0 ELSE 1 END as Recurring 
				FROM	[Scheduling].[GroupSchedulingHeader]  gh
				INNER JOIN [Scheduling].[GroupDetails] gd
				ON		gh.GroupDetailID = gd.GroupDetailID 
				INNER JOIN Scheduling.AppointmentResource ar
				ON		ar.GroupHeaderID = gh.GroupHeaderID 
				AND		ar.ResourceTypeID = 5 -- Group - ResourceType
				INNER JOIN Scheduling.Appointment a
				ON		ar.AppointmentID = a.AppointmentID 
				LEFT JOIN [Core].[vw_GetOrganizationStructureDetails] P  
				ON		a.ProgramID = P.MappingID 
				LEFT JOIN Reference.Facility f
				ON		f.FacilityID = a.FacilityID
				LEFT JOIN Reference.GroupType at
				on		gd.GroupTypeID = at.GroupTypeID
				WHERE	gd.GroupName like '%'+@GroupName +'%'
			--	AND		a.AppointmentDate >= @CurrentDate -- CurrentDate
			--	AND		a.AppointmentStartTime >= @CurrentTime -- CurrentTime

				AND		gd.IsActive = 1
				AND		a.IsCancelled = 0 -- NotCancelled 
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END