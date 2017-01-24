-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[MergedContactsMapping]
-- Author:		John Crossen
-- Date:		08/02/2016
--
-- Purpose:		History Table for Client Merges
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/02/2016	John Crossen	TFS# - Initial creation.
-- 12/02/2016	Scott Martin	Added TransactionLogID and ContactID
-- 01/11/2017	Scott Martin	Removed IsUnMerged and added IsUnMergeAllowed
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[MergedContactsMapping](
	[MergedContactsMappingID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[TransactionLogID] [BIGINT] NOT NULL,
	[ContactID] [BIGINT] NOT NULL,
	[ParentID] [BIGINT] NOT NULL,
	[ChildID] [BIGINT] NOT NULL,
	[IsParentPrimary] [BIT] NOT NULL DEFAULT(1),
	[MergeDate] datetime NOT NULL,
	[IsUnMergeAllowed] BIT NOT NULL DEFAULT(1),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),	
 CONSTRAINT [PK_MergedContactsMapping_MergedContactsMappingID] PRIMARY KEY CLUSTERED 
(
	[MergedContactsMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.MergedContactsMapping WITH CHECK ADD CONSTRAINT [FK_MergedContactsMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.MergedContactsMapping CHECK CONSTRAINT [FK_MergedContactsMapping_UserModifedBy]
GO
ALTER TABLE Core.MergedContactsMapping WITH CHECK ADD CONSTRAINT [FK_MergedContactsMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.MergedContactsMapping CHECK CONSTRAINT [FK_MergedContactsMapping_UserCreatedBy]
GO
ALTER TABLE Core.MergedContactsMapping WITH CHECK ADD CONSTRAINT [FK_MergedContactsMapping_TransactionLogID] FOREIGN KEY ([TransactionLogID]) REFERENCES [Core].[TransactionLog] ([TransactionLogID])
GO
ALTER TABLE Core.MergedContactsMapping CHECK CONSTRAINT [FK_MergedContactsMapping_TransactionLogID]
GO
ALTER TABLE Core.MergedContactsMapping WITH CHECK ADD CONSTRAINT [FK_MergedContactsMapping_ContactID] FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE Core.MergedContactsMapping CHECK CONSTRAINT [FK_MergedContactsMapping_ContactID]
GO
ALTER TABLE Core.MergedContactsMapping WITH CHECK ADD CONSTRAINT [FK_MergedContactsMapping_ParentID] FOREIGN KEY ([ParentID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE Core.MergedContactsMapping CHECK CONSTRAINT [FK_MergedContactsMapping_ParentID]
GO
ALTER TABLE Core.MergedContactsMapping WITH CHECK ADD CONSTRAINT [FK_MergedContactsMapping_ChildID] FOREIGN KEY ([ChildID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE Core.MergedContactsMapping CHECK CONSTRAINT [FK_MergedContactsMapping_ChildID]
GO

