
-- Procedure:	[usp_GetRecurrence]
-- Author:		Chad Roberts
-- Date:		3/15/2016
--
-- Purpose:		Get a recurrence
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/15/2015	John Crossen    TFS#2731		Initial creation.
-- 05/01/2016 - Karl Jablonski	- Added missing dayofmonth field, changed monthid to yearlymonthid
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_GetRecurrence]
	@RecurrenceID BIGINT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
SELECT
	@ResultCode = 0,
	@ResultMessage = 'Executed Successfully'

	BEGIN TRY 
	SELECT 
		RecurrenceID,
		Frequency,	
		DailyFrequency,
		DailyDays,
		DayOfMonth,
		WeeklyRecurrence,
		WeekDayMon,
		WeekDayTue,
		WeekDayWed,
		WeekDayThur,
		WeekDayFri,
		WeekDaySat,
		WeekDaySun,
		MonthlyFrequency,
		MonthlyDay,
		DayMonths,
		WeekOfMonthID,
		DayOfWeekID,
		TheMonths,
		YearlyRecurrence,
		YearlyFrequency,
		YearlyMonthID,
		YearlyMonthOnTheID,
		StartDate,
		Ending,
		EndAfter,
		EndDate
	FROM
		Scheduling.Recurrence
	WHERE
		RecurrenceID = @RecurrenceID
		AND IsActive=1

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END