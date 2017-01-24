-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.Credentials
-- Author:		John Crossen
-- Date:		08/12/2014
--
-- Purpose:		Provide a brief description of what your function does.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2014	John Crossen		TFS# 885 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 06/07/2016	Kyle Campbell	TFS #11310	Added IsSystem column for Consents Staff Credentials extended dropdown 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[Credentials](
	[CredentialID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[CredentialAbbreviation] [NVARCHAR](20) NOT NULL,
	[CredentialName] [NVARCHAR](255) NOT NULL,
	[LicenseRequired] BIT NOT NULL DEFAULT(0),
	[EffectiveDate] [DATE] NOT NULL  DEFAULT GETUTCDATE(),
	[ExpirationDate] [DATE] NULL,
	[ExpirationReason] [NVARCHAR](500) NULL,
	[IsInternal] [BIT] NOT NULL  DEFAULT 1,
	[IsSystem] BIT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Credentials] PRIMARY KEY CLUSTERED 
(
	[CredentialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Credentials_CredentialName] ON [Reference].[Credentials]
(
	[CredentialName] ASC,
	[CredentialAbbreviation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [Reference].[Credentials] WITH CHECK ADD CONSTRAINT [FK_Credentials_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[Credentials] CHECK CONSTRAINT [FK_Credentials_UserModifedBy]
GO
ALTER TABLE [Reference].[Credentials] WITH CHECK ADD CONSTRAINT [FK_Credentials_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[Credentials] CHECK CONSTRAINT [FK_Credentials_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Credentials', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Credentials;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating user certifications', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Credentials;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Credentials;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Credentials;
GO;