-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[RecordHeader]
-- Author:		Sumana Sangapu
-- Date:		01/11/2017
--
-- Purpose:		This table will hold the snapshot of PrintHeader record details.
--
-- Notes:		n/a 
--
-- Depends:		n/a 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/11/2017   Sumana Sangapu	- Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE Core.[RecordHeader](
	[RecordHeaderID] [bigint] IDENTITY (1,1) NOT NULL,
	[WorkflowID] [bigint] NOT NULL,
	[RecordPrimaryKeyValue] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_RecordHeader_RecordHeaderID] PRIMARY KEY CLUSTERED  ([RecordHeaderID] ASC)
)
ON [PRIMARY]
GO

ALTER TABLE Core.[RecordHeader] WITH CHECK ADD CONSTRAINT [FK_RecordHeader_WorkflowID] FOREIGN KEY ([WorkflowID]) REFERENCES [Core].[Workflow] ([WorkflowID])
GO
ALTER TABLE Core.[RecordHeader] CHECK CONSTRAINT [FK_RecordHeader_WorkflowID]
GO
ALTER TABLE Core.[RecordHeader] WITH CHECK ADD CONSTRAINT [FK_RecordHeader_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.[RecordHeader] CHECK CONSTRAINT [FK_RecordHeader_UserModifedBy]
GO
ALTER TABLE Core.[RecordHeader] WITH CHECK ADD CONSTRAINT [FK_RecordHeader_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.[RecordHeader] CHECK CONSTRAINT [FK_RecordHeader_UserCreatedBy]
GO 