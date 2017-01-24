
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[OrganizationAttributesMapping]
-- Author:		Sumana Sangapu
-- Date:		01/15/2016
--
-- Purpose:		Mapping table for Attributes of an Organization and the data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	Sumana Sangapu		Initial Creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[OrganizationAttributesMapping](
	[AttributeMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[AttributeID] [int] NOT NULL,
	[DetailID] [bigint] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_OrganizationAttributesMapping] PRIMARY KEY CLUSTERED 
(
	[AttributeMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE [Core].[OrganizationAttributesMapping] WITH CHECK ADD  CONSTRAINT [FK_OrganizationAttributesMapping_AttributeID] FOREIGN KEY([AttributeID])
REFERENCES [Core].[OrganizationAttributes] ([AttributeID])
GO

ALTER TABLE [Core].[OrganizationAttributesMapping] CHECK CONSTRAINT [FK_OrganizationAttributesMapping_AttributeID]
GO

ALTER TABLE [Core].[OrganizationAttributesMapping] WITH CHECK ADD  CONSTRAINT [FK_OrganizationAttributesMapping_DetailID] FOREIGN KEY([DetailID])
REFERENCES [Core].[OrganizationDetails] ([DetailID])
GO

ALTER TABLE [Core].[OrganizationAttributesMapping] CHECK CONSTRAINT [FK_OrganizationAttributesMapping_DetailID]
GO

ALTER TABLE Core.OrganizationAttributesMapping WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributesMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationAttributesMapping CHECK CONSTRAINT [FK_OrganizationAttributesMapping_UserModifedBy]
GO
ALTER TABLE Core.OrganizationAttributesMapping WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributesMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationAttributesMapping CHECK CONSTRAINT [FK_OrganizationAttributesMapping_UserCreatedBy]
GO
