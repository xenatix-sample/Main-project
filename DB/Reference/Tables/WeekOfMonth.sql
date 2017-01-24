-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management

CREATE TABLE [Reference].[WeekOfMonth] (
    [WeekOfMonthID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_WeekOfMonth_WeekOfMonthID] PRIMARY KEY CLUSTERED ([WeekOfMonthID] ASC)
);

GO;

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Week Of the Month', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = WeekOfMonth;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating which week of the month it is', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = WeekOfMonth;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = WeekOfMonth;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = WeekOfMonth;
GO;