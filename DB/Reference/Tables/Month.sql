CREATE TABLE [Reference].[Month] (
    [MonthID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Month_MonthID] PRIMARY KEY CLUSTERED ([MonthID] ASC)
);

