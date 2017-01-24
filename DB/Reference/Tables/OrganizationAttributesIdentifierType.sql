-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[OrganizationAttributesIdentifierType]
-- Author:		Kyle Campbell
-- Date:		12/14/2016
--
-- Purpose:		Lookup for OrganizationAttributesIdentifierType details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[OrganizationAttributesIdentifierType](
	[OrganizationAttributesIdentifierTypeID] [int] IDENTITY (1,1) NOT NULL,
	[AttributeID] INT NOT NULL,
	[OrganizationIdentifierTypeID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_OrganizationAttributesIdentifierType_OrganizationAttributesIdentifierTypeID] PRIMARY KEY CLUSTERED 
(
	[OrganizationAttributesIdentifierTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.OrganizationAttributesIdentifierType WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributesIdentifierType_AttributeID] FOREIGN KEY([AttributeID]) REFERENCES [Core].[OrganizationAttributes] ([AttributeID])
GO
ALTER TABLE Reference.OrganizationAttributesIdentifierType CHECK CONSTRAINT [FK_OrganizationAttributesIdentifierType_AttributeID]
GO
ALTER TABLE Reference.OrganizationAttributesIdentifierType WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributesIdentifierType_OrganizationIdentifierTypeID] FOREIGN KEY([OrganizationIdentifierTypeID]) REFERENCES [Reference].[OrganizationIdentifierType] ([OrganizationIdentifierTypeID])
GO
ALTER TABLE Reference.OrganizationAttributesIdentifierType CHECK CONSTRAINT [FK_OrganizationAttributesIdentifierType_OrganizationIdentifierTypeID]
GO
ALTER TABLE Reference.OrganizationAttributesIdentifierType WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributesIdentifierType_UserModifiedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.OrganizationAttributesIdentifierType CHECK CONSTRAINT [FK_OrganizationAttributesIdentifierType_UserModifiedBy]
GO
ALTER TABLE Reference.OrganizationAttributesIdentifierType WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributesIdentifierType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.OrganizationAttributesIdentifierType CHECK CONSTRAINT [FK_OrganizationAttributesIdentifierType_UserCreatedBy]
GO


--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Organization Attributes Identifier Type', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationAttributesIdentifierType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating what attributes are associated with what organization identifier type', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationAttributesIdentifierType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationAttributesIdentifierType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OrganizationAttributesIdentifierType;
GO;
