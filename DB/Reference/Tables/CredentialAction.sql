
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[CredentialAction]
-- Author:		Sumana Sangapu
-- Date:		04/07/2016
--
-- Purpose:		Look to store the Actions. Used for credentials.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/07/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[CredentialAction](
	[CredentialActionID] [int] IDENTITY(1,1) NOT NULL,
	[CredentialAction] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_CredentialAction_ActionID] PRIMARY KEY CLUSTERED 
(
	[CredentialActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_CredentialAction] UNIQUE NONCLUSTERED 
(
	[CredentialAction] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[CredentialAction]  WITH CHECK ADD  CONSTRAINT [FK_CredentialAction_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[CredentialAction] CHECK CONSTRAINT [FK_CredentialAction_UserCreatedBy]
GO

ALTER TABLE [Reference].[CredentialAction]  WITH CHECK ADD  CONSTRAINT [FK_CredentialAction_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[CredentialAction] CHECK CONSTRAINT [FK_CredentialAction_UserModifedBy]
GO


