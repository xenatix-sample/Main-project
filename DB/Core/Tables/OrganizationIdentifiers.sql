-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[OrganizationIdentifiers]
-- Author:		Kyle Campbell
-- Date:		12/14/2016
--
-- Purpose:		Lookup for OrganizationIdentifiers details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2016	Kyle Campbell	TFS# 17998	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[OrganizationIdentifiers](
	[OrganizationIdentifierID] [int] IDENTITY (1,1) NOT NULL,
	[DetailID] BIGINT NOT NULL,
	[OrganizationIdentifierTypeID] INT NOT NULL,
	[OrganizationIdentifier] NVARCHAR(50) NULL,
	[EffectiveDate] DATE,
	[ExpirationDate] DATE,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_OrganizationIdentifiers_OrganizationIdentifiersID] PRIMARY KEY CLUSTERED 
(
	[OrganizationIdentifierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Core.OrganizationIdentifiers WITH CHECK ADD CONSTRAINT [FK_OrganizationIdentifiers_DetailID] FOREIGN KEY([DetailID]) REFERENCES Core.OrganizationDetails ([DetailID])
GO
ALTER TABLE Core.OrganizationIdentifiers CHECK CONSTRAINT [FK_OrganizationIdentifiers_DetailID]
GO

ALTER TABLE Core.OrganizationIdentifiers WITH CHECK ADD CONSTRAINT [FK_OrganizationIdentifiers_OrganizationIdentifierTypeID] FOREIGN KEY([OrganizationIdentifierTypeID]) REFERENCES Reference.OrganizationIdentifierType ([OrganizationIdentifierTypeID])
GO
ALTER TABLE Core.OrganizationIdentifiers CHECK CONSTRAINT [FK_OrganizationIdentifiers_OrganizationIdentifierTypeID]
GO

ALTER TABLE Core.OrganizationIdentifiers WITH CHECK ADD CONSTRAINT [FK_OrganizationIdentifiers_UserModifiedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationIdentifiers CHECK CONSTRAINT [FK_OrganizationIdentifiers_UserModifiedBy]
GO
ALTER TABLE Core.OrganizationIdentifiers WITH CHECK ADD CONSTRAINT [FK_OrganizationIdentifiers_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationIdentifiers CHECK CONSTRAINT [FK_OrganizationIdentifiers_UserCreatedBy]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References detailID in Core.OrganizationDetails', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifiers,
@level2type = N'COLUMN', @level2name = DetailID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References OrganizationIdentifierTypeID in Reference.OrganizationIdentifierType', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifiers,
@level2type = N'COLUMN', @level2name = OrganizationIdentifierTypeID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores actual value of identifier', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifiers,
@level2type = N'COLUMN', @level2name = OrganizationIdentifier;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Effective date of identifier', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifiers,
@level2type = N'COLUMN', @level2name = EffectiveDate;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Expiration date of identifier', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifiers,
@level2type = N'COLUMN', @level2name = ExpirationDate;
GO