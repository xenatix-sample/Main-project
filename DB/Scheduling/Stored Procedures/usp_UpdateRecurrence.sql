-- Procedure:	[usp_UpdateRecurrence]
-- Author:		Chad Roberts
-- Date:		3/15/2016
--
-- Purpose:		Update recurrences
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 3/15/2016	 Chad Roberts    		Initial creation.
-- 05/01/2016 - Karl Jablonski	- Added missing dayofmonth field, changed monthid to yearlymonthid
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateRecurrence]
	@RecurrenceID bigint,
	@Frequency INT,	
	@DailyFrequency INT,
	@DailyDays INT,
	@DayOfMonth INT,
	@WeeklyRecurrence INT,
	@WeekDayMon BIT,
	@WeekDayTue BIT,
	@WeekDayWed BIT,
	@WeekDayThur BIT,
	@WeekDayFri BIT,
	@WeekDaySat BIT,
	@WeekDaySun BIT,
	@MonthlyFrequency INT,
	@MonthlyDay INT,
	@DayMonths INT,
	@WeekOfMonthID INT,
	@DayOfWeekID INT,
	@TheMonths INT,
	@YearlyRecurrence INT,
	@YearlyFrequency INT,
	@YearlyMonthID INT,
	@YearlyMonthOnTheID INT,
	@StartDate DATETIME,
	@Ending INT,
	@EndAfter INT,
	@EndDate DATETIME,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
	
	update [Scheduling].[Recurrence]
    set Frequency = @Frequency,	
		DailyFrequency = @DailyFrequency,
		DailyDays = @DailyDays,
		DayOfMonth = @DayOfMonth,
		WeeklyRecurrence = @WeeklyRecurrence,
		WeekDayMon = @WeekDayMon,
		WeekDayTue = @WeekDayTue,
		WeekDayWed = @WeekDayWed,
		WeekDayThur = @WeekDayThur,
		WeekDayFri = @WeekDayFri,
		WeekDaySat = @WeekDaySat,
		WeekDaySun = @WeekDaySun,
		MonthlyFrequency = @MonthlyFrequency,
		MonthlyDay = @MonthlyDay,
		DayMonths = @DayMonths,
		WeekOfMonthID = @WeekOfMonthID,
		DayOfWeekID = @DayOfWeekID,
		TheMonths = @TheMonths,
		YearlyRecurrence = @YearlyRecurrence,
		YearlyFrequency = @YearlyFrequency,
		YearlyMonthID = @YearlyMonthID,
		YearlyMonthOnTheID = @YearlyMonthOnTheID,
		StartDate = @StartDate,
		Ending = @Ending,
		EndAfter = @EndAfter,
		EndDate = @EndDate,
		[IsActive] = @IsActive,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		CreatedBy = @ModifiedBy,
		CreatedOn = @ModifiedOn
	where RecurrenceID = @RecurrenceID
	
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'Recurrence', @RecurrenceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'Recurrence', @AuditDetailID, @RecurrenceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END