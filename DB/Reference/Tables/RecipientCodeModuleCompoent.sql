-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[RecipientCodeModuleComponent]
-- Author:		Scott Martin
-- Date:		06/22/2016
--
-- Purpose:		Mapping table for Service Recording and Recipient Code Module Components
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/22/2016	Scott Martin	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[RecipientCodeModuleComponent](
	[RecipientCodeModuleComponentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleComponentID] BIGINT NOT NULL,
	[RecipientCodeID] [smallint] NOT NULL,
	[ServicesID] [int] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate())
 CONSTRAINT [PK_RecipientCodeModuleComponentID] PRIMARY KEY CLUSTERED 
(
	[RecipientCodeModuleComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO
ALTER TABLE  [Reference].[RecipientCodeModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_RecipientCodeModuleComponent_ModuleComponentID] FOREIGN KEY([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO

ALTER TABLE  [Reference].[RecipientCodeModuleComponent] CHECK CONSTRAINT [FK_RecipientCodeModuleComponent_ModuleComponentID]
GO

ALTER TABLE  [Reference].[RecipientCodeModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_RecipientCodeModuleComponent_RecipientCodeID] FOREIGN KEY([RecipientCodeID]) REFERENCES [Reference].[RecipientCode] ([CodeID])
GO

ALTER TABLE  [Reference].[RecipientCodeModuleComponent] CHECK CONSTRAINT [FK_RecipientCodeModuleComponent_RecipientCodeID]
GO

ALTER TABLE  [Reference].[RecipientCodeModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_RecipientCodeModuleComponent_ServicesID] FOREIGN KEY([ServicesID]) REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE  [Reference].[RecipientCodeModuleComponent] CHECK CONSTRAINT [FK_RecipientCodeModuleComponent_ServicesID]
GO

ALTER TABLE [Reference].[RecipientCodeModuleComponent] WITH CHECK ADD CONSTRAINT [FK_RecipientCodeModuleComponent_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[RecipientCodeModuleComponent] CHECK CONSTRAINT [FK_RecipientCodeModuleComponent_UserModifedBy]
GO
ALTER TABLE [Reference].[RecipientCodeModuleComponent] WITH CHECK ADD CONSTRAINT [FK_RecipientCodeModuleComponent_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[RecipientCodeModuleComponent] CHECK CONSTRAINT [FK_RecipientCodeModuleComponent_UserCreatedBy]
GO
