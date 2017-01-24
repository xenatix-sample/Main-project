-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ConfirmationStatementGroup]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Allows for grouping of Confirmation Statements
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ConfirmationStatementGroup]
(
	[ConfirmationStatementGroupID] INT IDENTITY (1,1) NOT NULL,
	[ConfirmationStatementGroup] NVARCHAR(100) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_ConfirmationStatementGroup_ConfirmationStatementGroupID] PRIMARY KEY CLUSTERED ([ConfirmationStatementGroupID] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [FK_ConfirmationStatementGroup_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_ConfirmationStatementGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
) ON [PRIMARY]
GO
