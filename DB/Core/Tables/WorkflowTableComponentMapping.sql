-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[WorkflowTableCatalogMappingTableCatalogMapping]
-- Author:		Sumana Sangapu
-- Date:		01/11/2017
--
-- Purpose:		This table will hold the Module and Feature association for the WorkflowTableCatalogMapping.
--
-- Notes:		n/a 
--
-- Depends:		n/a 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/11/2017   Sumana Sangapu	- Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[WorkflowTableCatalogMapping](
	[WorkflowTableCatalogMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[WorkflowID] BIGINT NOT NULL,
	[TableCatalogID] INT NOT NULL,
	[IsActive] [bit] NOT NULL  DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_WorkflowTableCatalogMapping_WorkflowTableCatalogMappingID] PRIMARY KEY CLUSTERED  ([WorkflowTableCatalogMappingID] ASC)
)
ON [PRIMARY]
GO

ALTER TABLE [Core].[WorkflowTableCatalogMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowTableCatalogMapping_WorkflowID] FOREIGN KEY ([WorkflowID]) REFERENCES Core.Workflow (WorkflowID)
GO
ALTER TABLE [Core].[WorkflowTableCatalogMapping] CHECK CONSTRAINT [FK_WorkflowTableCatalogMapping_WorkflowID]
GO
ALTER TABLE [Core].[WorkflowTableCatalogMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowTableCatalogMapping_TableCatalogID] FOREIGN KEY ([TableCatalogID]) REFERENCES Reference.TableCatalog ([TableCatalogID])
GO
ALTER TABLE [Core].[WorkflowTableCatalogMapping] CHECK CONSTRAINT [FK_WorkflowTableCatalogMapping_TableCatalogID]
GO
ALTER TABLE [Core].[WorkflowTableCatalogMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowTableCatalogMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[WorkflowTableCatalogMapping] CHECK CONSTRAINT [FK_WorkflowTableCatalogMapping_UserModifedBy]
GO
ALTER TABLE [Core].[WorkflowTableCatalogMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowTableCatalogMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[WorkflowTableCatalogMapping] CHECK CONSTRAINT [FK_WorkflowTableCatalogMapping_UserCreatedBy]
GO