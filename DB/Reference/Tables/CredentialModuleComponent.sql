-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[CredentialModuleComponent]
-- Author:		Scott Martin
-- Date:		08/10/2016
--
-- Purpose:		Mapping table for Service Recording and Credential Module Components
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2016	Scott Martin	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[CredentialModuleComponent](
	[CredentialModuleComponentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleComponentID] BIGINT NOT NULL,
	[CredentialID] BIGINT NOT NULL,
	[ServicesID] [int] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate())
 CONSTRAINT [PK_CredentialModuleComponent_CredentialModuleComponentID] PRIMARY KEY CLUSTERED 
(
	[CredentialModuleComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO
ALTER TABLE  [Reference].[CredentialModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_CredentialModuleComponent_ModuleComponentID] FOREIGN KEY([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO

ALTER TABLE  [Reference].[CredentialModuleComponent] CHECK CONSTRAINT [FK_CredentialModuleComponent_ModuleComponentID]
GO

ALTER TABLE  [Reference].[CredentialModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_CredentialModuleComponent_CredentialID] FOREIGN KEY([CredentialID]) REFERENCES [Reference].[Credentials] ([CredentialID])
GO

ALTER TABLE  [Reference].[CredentialModuleComponent] CHECK CONSTRAINT [FK_CredentialModuleComponent_CredentialID]
GO

ALTER TABLE  [Reference].[CredentialModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_CredentialModuleComponent_ServicesID] FOREIGN KEY([ServicesID]) REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE  [Reference].[CredentialModuleComponent] CHECK CONSTRAINT [FK_CredentialModuleComponent_ServicesID]
GO

ALTER TABLE [Reference].[CredentialModuleComponent] WITH CHECK ADD CONSTRAINT [FK_CredentialModuleComponent_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[CredentialModuleComponent] CHECK CONSTRAINT [FK_CredentialModuleComponent_UserModifedBy]
GO
ALTER TABLE [Reference].[CredentialModuleComponent] WITH CHECK ADD CONSTRAINT [FK_CredentialModuleComponent_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[CredentialModuleComponent] CHECK CONSTRAINT [FK_CredentialModuleComponent_UserCreatedBy]
GO
