-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[OrganizationAttributes]
-- Author:		Sumana Sangapu
-- Date:		01/15/2016
--
-- Purpose:		Attributes of an Organization
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	Sumana Sangapu		Initial Creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[OrganizationAttributes](
	[AttributeID] [int] IDENTITY (1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DataKey] [nvarchar](50) NOT NULL,
	[IsExternal] [bit] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_OrganizationAttributes] PRIMARY KEY CLUSTERED 
(
	[AttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE Core.OrganizationAttributes WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributes_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationAttributes CHECK CONSTRAINT [FK_OrganizationAttributes_UserModifedBy]
GO
ALTER TABLE Core.OrganizationAttributes WITH CHECK ADD CONSTRAINT [FK_OrganizationAttributes_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationAttributes CHECK CONSTRAINT [FK_OrganizationAttributes_UserCreatedBy]
GO

