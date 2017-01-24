-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[FinanceFrequency]
-- Author:		Sumana Sangapu
-- Date:		08/07/2015
--
-- Purpose:		Lookup for Frequency details for Financial Assessment
--
-- Notes:		n/a (or any additional notes)
--
-- References:	Register.FinancialAssessmentDetails
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/07/2015	Sumana Sangapu	TFS# 634 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[FinanceFrequency](
	[FinanceFrequencyID] [int] IDENTITY(1,1) NOT NULL,
	[FinanceFrequency] [nvarchar](100) NOT NULL,
	[FrequencyFactor] int NOT NULL,
	[MeasureBy] [nvarchar](10) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_FinanceFrequency_FrequencyID] PRIMARY KEY CLUSTERED 
(
	[FinanceFrequencyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_FinanceFrequency] UNIQUE NONCLUSTERED 
(
	[FinanceFrequency] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.FinanceFrequency WITH CHECK ADD CONSTRAINT [FK_FinanceFrequency_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.FinanceFrequency CHECK CONSTRAINT [FK_FinanceFrequency_UserModifedBy]
GO
ALTER TABLE Reference.FinanceFrequency WITH CHECK ADD CONSTRAINT [FK_FinanceFrequency_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.FinanceFrequency CHECK CONSTRAINT [FK_FinanceFrequency_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Finance Frequency', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = FinanceFrequency;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating frequency of pay', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = FinanceFrequency;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = FinanceFrequency;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = FinanceFrequency;
GO;