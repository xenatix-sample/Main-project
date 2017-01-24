-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[OrganizationIdentifierType]
-- Author:		Kyle Campbell
-- Date:		12/14/2016
--
-- Purpose:		Lookup for OrganizationIdentifierType details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2016	Kyle Campbell	TFS# 17998	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[OrganizationIdentifierType](
	[OrganizationIdentifierTypeID] [int] IDENTITY (1,1) NOT NULL,
	[OrganizationIdentifierType] [nvarchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_OrganizationIdentifierType_OrganizationIdentifierTypeID] PRIMARY KEY CLUSTERED 
(
	[OrganizationIdentifierTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[OrganizationIdentifierType] ADD CONSTRAINT IX_OrganizationIdentifierType UNIQUE(OrganizationIdentifierType)
GO
ALTER TABLE Reference.OrganizationIdentifierType WITH CHECK ADD CONSTRAINT [FK_OrganizationIdentifierType_UserModifiedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.OrganizationIdentifierType CHECK CONSTRAINT [FK_OrganizationIdentifierType_UserModifiedBy]
GO
ALTER TABLE Reference.OrganizationIdentifierType WITH CHECK ADD CONSTRAINT [FK_OrganizationIdentifierType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.OrganizationIdentifierType CHECK CONSTRAINT [FK_OrganizationIdentifierType_UserCreatedBy]
GO


--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Organization Identifier Type', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifierType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the types of identifiers for organizations', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifierType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifierType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationIdentifierType;
GO;
