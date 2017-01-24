-- Procedure:	[usp_AddRecurrence]
-- Author:		Chad Roberts
-- Date:		3/15/2016
--
-- Purpose:		Adding recurrences
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 3/15/2016	 Chad Roberts    		Initial creation.
-- 05/01/2016 - Karl Jablonski	- Added missing dayofmonth field, changed monthid to yearlymonthid
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_AddRecurrence]
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
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY

	INSERT INTO [Scheduling].[Recurrence]
    (
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
		EndDate,
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@Frequency,	
		@DailyFrequency,
		@DailyDays,
		@DayOfMonth,
		@WeeklyRecurrence,
		@WeekDayMon,
		@WeekDayTue,
		@WeekDayWed,
		@WeekDayThur,
		@WeekDayFri,
		@WeekDaySat,
		@WeekDaySun,
		@MonthlyFrequency,
		@MonthlyDay,
		@DayMonths,
		@WeekOfMonthID,
		@DayOfWeekID,
		@TheMonths,
		@YearlyRecurrence,
		@YearlyFrequency,
		@YearlyMonthID,
		@YearlyMonthOnTheID,
		@StartDate,
		@Ending,
		@EndAfter,
		@EndDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'Recurrence', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'Recurrence', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END