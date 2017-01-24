-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Workflow]
-- Author:		Sumana Sangapu
-- Date:		01/11/2017
--
-- Purpose:		This table will hold the Module and Feature association for the workflow.
--
-- Notes:		n/a 
--
-- Depends:		n/a 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/11/2017   Sumana Sangapu	- Initial Creation
-----------------------------------------------------------------------------------------------------------------------
-- DROP TABLE [Core].[Workflow]

CREATE TABLE [Core].[Workflow](
	[WorkflowID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleID] [bigint] NOT NULL,
	[FeatureID] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL  DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Workflow_WorkflowID] PRIMARY KEY CLUSTERED  ([WorkflowID] ASC)
)
ON [PRIMARY]

GO

ALTER TABLE [Core].[Workflow] WITH CHECK ADD CONSTRAINT [FK_Workflow_ModuleID] FOREIGN KEY ([ModuleID]) REFERENCES Core.Module (ModuleID)
GO
ALTER TABLE [Core].[Workflow] CHECK CONSTRAINT [FK_Workflow_ModuleID]
GO
ALTER TABLE [Core].[Workflow] WITH CHECK ADD CONSTRAINT [FK_Workflow_FeatureID] FOREIGN KEY ([FeatureID]) REFERENCES Core.Feature ([FeatureID])
GO
ALTER TABLE [Core].[Workflow] CHECK CONSTRAINT [FK_Workflow_FeatureID]
GO
ALTER TABLE [Core].[Workflow] WITH CHECK ADD CONSTRAINT [FK_Workflow_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[Workflow] CHECK CONSTRAINT [FK_Workflow_UserModifedBy]
GO
ALTER TABLE [Core].[Workflow] WITH CHECK ADD CONSTRAINT [FK_Workflow_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[Workflow] CHECK CONSTRAINT [FK_Workflow_UserCreatedBy]
GO

