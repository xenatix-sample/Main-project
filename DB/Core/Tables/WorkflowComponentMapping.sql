-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[WorkflowComponentMapping]
-- Author:		Sumana Sangapu
-- Date:		01/11/2017
--
-- Purpose:		This table will hold the workflow and modulecomponent mapping for navigation.
--
-- Notes:		n/a 
--
-- Depends:		n/a 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/11/2017   Sumana Sangapu	- Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[WorkflowComponentMapping](
	[WorkflowComponentMappingID] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowID] [bigint] NOT NULL,
	[ModuleComponentID] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Workflow_WorkflowComponentMappingID] PRIMARY KEY CLUSTERED  ([WorkflowComponentMappingID] ASC)
)
ON [PRIMARY]

GO
ALTER TABLE [Core].[WorkflowComponentMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowComponentMapping_WorkflowID] FOREIGN KEY ([WorkflowID]) REFERENCES Core.Workflow ([WorkflowID])
GO
ALTER TABLE [Core].[WorkflowComponentMapping] CHECK CONSTRAINT [FK_WorkflowComponentMapping_WorkflowID]
GO
ALTER TABLE [Core].[WorkflowComponentMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowComponentMapping_ModuleComponentID] FOREIGN KEY ([ModuleComponentID]) REFERENCES Core.ModuleComponent ([ModuleComponentID])
GO
ALTER TABLE [Core].[WorkflowComponentMapping] CHECK CONSTRAINT [FK_WorkflowComponentMapping_ModuleComponentID]
GO
ALTER TABLE [Core].[WorkflowComponentMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowComponentMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[WorkflowComponentMapping] CHECK CONSTRAINT [FK_WorkflowComponentMapping_UserModifedBy]
GO
ALTER TABLE [Core].[WorkflowComponentMapping] WITH CHECK ADD CONSTRAINT [FK_WorkflowComponentMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[WorkflowComponentMapping] CHECK CONSTRAINT [FK_WorkflowComponentMapping_UserCreatedBy]
GO 