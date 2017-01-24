-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[CredentialActionTemplate]
-- Author:		Sumana Sangapu
-- Date:		04/07/2016
--
-- Purpose:		Holds the mapping between actions for the forms based on user credentials.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/07/2016	Sumana Sangapu	Initial creation.
-- 06/24/2016	Scott Martin	Added ServiceID to associate credentials with specific services

 -- drop TABLE [Core].[CredentialActionTemplate]
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[CredentialActionTemplate](
	[CredentialActionTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[CredentialActionFormID] [int] NOT NULL,
	[CredentialActionID] [int] NOT NULL,
	[CredentialID] [bigint] NOT NULL,
	[ServicesID] [int] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_CredentialActionTemplate_CredentialActionTemplateID] PRIMARY KEY CLUSTERED 
(
	[CredentialActionTemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO
ALTER TABLE [Core].[CredentialActionTemplate]  WITH CHECK ADD  CONSTRAINT [FK_CredentialActionTemplate_CredentialActionFormID] FOREIGN KEY([CredentialActionFormID])
REFERENCES [Reference].[CredentialActionForms] ([CredentialActionFormID])
GO

ALTER TABLE [Core].[CredentialActionTemplate] CHECK CONSTRAINT [FK_CredentialActionTemplate_CredentialActionFormID]
GO

ALTER TABLE [Core].[CredentialActionTemplate]  WITH CHECK ADD  CONSTRAINT [FK_CredentialActionTemplate_CredentialActionID] FOREIGN KEY([CredentialActionID])
REFERENCES [Reference].[CredentialAction] ([CredentialActionID])
GO

ALTER TABLE [Core].[CredentialActionTemplate] CHECK CONSTRAINT [FK_CredentialActionTemplate_CredentialActionID]
GO

ALTER TABLE [Core].[CredentialActionTemplate]  WITH CHECK ADD  CONSTRAINT [FK_CredentialActionTemplate_CredentialID] FOREIGN KEY([CredentialID])
REFERENCES [Reference].[Credentials] ([CredentialID])
GO

ALTER TABLE [Core].[CredentialActionTemplate] CHECK CONSTRAINT [FK_CredentialActionTemplate_CredentialID]
GO

ALTER TABLE  [Core].[CredentialActionTemplate] WITH CHECK ADD  CONSTRAINT [FK_CredentialActionTemplate_ServicesID] FOREIGN KEY([ServicesID]) REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE  [Core].[CredentialActionTemplate] CHECK CONSTRAINT [FK_CredentialActionTemplate_ServicesID]
GO

ALTER TABLE [Core].[CredentialActionTemplate]  WITH CHECK ADD  CONSTRAINT [FK_CredentialActionTemplate_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[CredentialActionTemplate] CHECK CONSTRAINT [FK_CredentialActionTemplate_UserCreatedBy]
GO

ALTER TABLE [Core].[CredentialActionTemplate]  WITH CHECK ADD  CONSTRAINT [FK_CredentialActionTemplate_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[CredentialActionTemplate] CHECK CONSTRAINT [FK_CredentialActionTemplate_UserModifedBy]
GO