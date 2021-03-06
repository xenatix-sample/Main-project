
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetBlockedTimeAppointments]
-- Author:		Sumana Sangapu
-- Date:		04/26/2016
--
-- Purpose:		Get all the blocked time appointments for User 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/26/2016 - Sumana Sangapu	- Initial Creation
-- 05/01/2016 - Karl Jablonski	- Adding changes to occurrence counts
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_GetBlockedTimeAppointments]
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY

				DECLARE @RecurrenceDetails TABLE  (	RecurrenceID INT,
													Occurence INT,
													Frequency varchar(50),
													StartDate date,
													EndDate date,
													Days1 varchar(100) ) 
				 

				SELECT			AppointmentResourceID,
								AppointmentID,
								ResourceID,
								ResourceTypeID,
								ParentID,
								ProgramID,
								AppointmentTypeID,
								ServicesID,
								AppointmentDate,
								AppointmentStartTime,
								AppointmentLength,
								SupervisionVisit,
								ReferredBy,
								ReasonForVisit,
								RecurrenceID,
								IsCancelled,
								CancelReasonID,
								CancelComment,
								Comments,
								IsBlocked,
								IsGroupAppointment,
								GroupDetailID,
								GroupHeaderID as GroupID,
								AppointmentStatusID,
								GroupType
					INTO		#AptDetails
					FROM	Scheduling.vw_GetAppointmentDetails 
					WHERE	ResourceID = @ResourceID
					AND		Resourcetypeid = @ResourceTypeID -- User - ResourceType
					AND		IsActive = 1

					-- Insert Daily Recurrence Details
					INSERT INTO @RecurrenceDetails
					SELECT	r.RecurrenceID,(SELECT COUNT(*) FROM #AptDetails a where a.RecurrenceID = r.RecurrenceID) as Occurence, SchedulingFrequency, Startdate,
							CASE WHEN Ending = 3  THEN EndDate 
								 WHEN Ending = 2  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID )  
								 WHEN Ending = 1  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID ) END as EndDate,  NULL as Days1
					FROM #AptDetails a
					INNER JOIN Scheduling.Recurrence r
					ON a.RecurrenceID = r.RecurrenceID
					INNER JOIN  [Scheduling].[SchedulingFrequency] sf
					ON r.Frequency = sf.[SchedulingFrequencyID] 
					WHERE r.Frequency = 1 

					-- Insert Weekly Recurrence Details
					INSERT INTO @RecurrenceDetails
					Select RecurrenceID,(SELECT COUNT(*) FROM #AptDetails a where a.RecurrenceID = RecurrenceID) as Occurence , SchedulingFrequency,Startdate,EndDate, 
					ISNULL(WeekDayMon ,'') +' '+ ISNULL(WeekDayTue ,'')+' '+ ISNULL(WeekDayWed ,'')+' '+ ISNULL(WeekDayThur ,'')+' '+ ISNULL(WeekDayFri ,'')+' '+ ISNULL(WeekDaySat ,'')+' '+ ISNULL(WeekDaySun ,'') as Days1
					FROM (
					SELECT	r.RecurrenceID,(SELECT COUNT(*) FROM #AptDetails a where a.RecurrenceID = r.RecurrenceID) as Occurence, SchedulingFrequency, Startdate, 
							CASE WHEN WeekDayMon IS NOT NULL THEN 'Mon' END as WeekdayMon,
							CASE WHEN WeekDayTue IS NOT NULL THEN 'Tue' END as WeekDayTue,
							CASE WHEN WeekDayWed IS NOT NULL THEN 'Wed' END as WeekDayWed,
							CASE WHEN WeekDayThur IS NOT NULL THEN 'Thur' END as WeekDayThur,
							CASE WHEN WeekDayFri IS NOT NULL THEN 'Fri' END as WeekDayFri,
							CASE WHEN WeekDaySat IS NOT NULL THEN 'Sat' END as WeekDaySat,
							CASE WHEN WeekDaySun IS NOT NULL THEN 'Sun' END as WeekDaySun,
							CASE WHEN Ending = 3  THEN EndDate 
								 WHEN Ending = 2  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID ) 
								 WHEN Ending = 1  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID ) END as EndDate, NULL as Days1
					FROM #AptDetails a
					INNER JOIN Scheduling.Recurrence r
					ON a.RecurrenceID = r.RecurrenceID
					INNER JOIN  [Scheduling].[SchedulingFrequency] sf
					ON r.Frequency = sf.[SchedulingFrequencyID] 
					WHERE r.Frequency = 2 
					)a
				
					-- Insert Monthly Recurrence Details
					INSERT INTO @RecurrenceDetails
					SELECT	r.RecurrenceID,(SELECT COUNT(*) FROM #AptDetails a where a.RecurrenceID = r.RecurrenceID) as Occurence, SchedulingFrequency, Startdate, 
							CASE WHEN Ending = 3  THEN EndDate 
								 WHEN Ending = 2  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID )  
								 WHEN Ending = 1  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID ) END as EndDate, NULL as Days1
					FROM #AptDetails a
					INNER JOIN Scheduling.Recurrence r
					ON a.RecurrenceID = r.RecurrenceID
					INNER JOIN  [Scheduling].[SchedulingFrequency] sf
					ON r.Frequency = sf.[SchedulingFrequencyID] 
					WHERE r.Frequency = 3 

					-- Insert Yearly Recurrence Details
					INSERT INTO @RecurrenceDetails
					SELECT	r.RecurrenceID,(SELECT COUNT(*) FROM #AptDetails a where a.RecurrenceID = r.RecurrenceID) as Occurence, SchedulingFrequency, Startdate, 
							CASE WHEN Ending = 3  THEN EndDate 
								 WHEN Ending = 2  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID )  
								 WHEN Ending = 1  THEN (select max(AppointmentDate) from #AptDetails a where a.RecurrenceID = r.RecurrenceID ) END as EndDate, NULL as Days1
					FROM #AptDetails a
					INNER JOIN Scheduling.Recurrence r
					ON a.RecurrenceID = r.RecurrenceID
					INNER JOIN  [Scheduling].[SchedulingFrequency] sf
					ON r.Frequency = sf.[SchedulingFrequencyID]  
					WHERE r.Frequency = 4 

					--select * from scheduling.appointment 

					SELECT * 
					INTO #DistinctAppointment
					FROM 
					(
							select * from  
							(
							SELECT  ROW_NUMBER() OVER( PARTITION BY  ReasonForVisit ORDER BY recurrenceid DESC) AS RowNum,  AppointmentID, Comments, RecurrenceID,AppointmentDate, AppointmentStartTime, AppointmentLength, ReasonForVisit -- , r.RecurrenceID, r.Occurence, Days1, r.Frequency, EndDate   
							FROM #AptDetails a ) a
							where rownum = 1 
					) A
					 
					-- Return the result set
					SELECT DISTINCT  AppointmentID, AppointmentDate, AppointmentStartTime, AppointmentLength, ReasonForVisit , Comments, r.RecurrenceID as RecurrID, r.Occurence as NumberOfOccurences, Days1 as RecurrenceDay, r.Frequency as RecurrenceFrequency, EndDate   as RecurrenceEndDate
					FROM #DistinctAppointment a
					LEFT JOIN @RecurrenceDetails r
					ON a.RecurrenceID = r.RecurrenceID


					DROP TABLE #AptDetails
	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END