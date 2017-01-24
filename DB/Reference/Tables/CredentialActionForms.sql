

-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[CredentialActionFormsForms]
-- Author:		Sumana Sangapu
-- Date:		04/07/2016
--
-- Purpose:		Lookup to store the forms upon which Credential Business rules will be applied. 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/07/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[CredentialActionForms](
	[CredentialActionFormID] [int] IDENTITY(1,1) NOT NULL,
	[CredentialActionForm] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_CredentialActionForms_ActionID] PRIMARY KEY CLUSTERED 
(
	[CredentialActionFormID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ActionForm] UNIQUE NONCLUSTERED 
(
	[CredentialActionForm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[CredentialActionForms]  WITH CHECK ADD  CONSTRAINT [FK_CredentialActionForms_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[CredentialActionForms] CHECK CONSTRAINT [FK_CredentialActionForms_UserCreatedBy]
GO

ALTER TABLE [Reference].[CredentialActionForms]  WITH CHECK ADD  CONSTRAINT [FK_CredentialActionForms_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[CredentialActionForms] CHECK CONSTRAINT [FK_CredentialActionForms_UserModifedBy]
GO


