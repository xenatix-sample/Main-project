-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[Category]
-- Author:		Suresh Pandey
-- Date:		08/03/2015
--
-- Purpose:		Lookup for Category details for Financial Assessment Screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2015	Suresh Pandey	TFS# - Initial creation.
-- 08/06/2015   Sumana Sangapu  Task# 634 - Added CategoryType for the FinancialAssessment Screen. Hold 'Income','Expenses' values.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[Category]
(
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[CategoryTypeID] int NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_Category_CategoryID] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],

	CONSTRAINT [FK_Category_CategoryTypeID]	FOREIGN KEY ([CategoryTypeID]) REFERENCES [Reference].[CategoryType] ([CategoryTypeID])
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[Category] ADD CONSTRAINT IX_Category UNIQUE(Category,CategoryTypeID)
GO

ALTER TABLE Reference.Category WITH CHECK ADD CONSTRAINT [FK_Category_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.Category CHECK CONSTRAINT [FK_Category_UserModifedBy]
GO
ALTER TABLE Reference.Category WITH CHECK ADD CONSTRAINT [FK_Category_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.Category CHECK CONSTRAINT [FK_Category_UserCreatedBy]
GO

