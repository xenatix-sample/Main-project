-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[CollateralType]
-- Author:		Kyle Campbell
-- Date:		03/10/2016
--
-- Purpose:		Store Contact Collateral Types
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/10/2016	Kyle Campbell	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Reference].[CollateralType]
(
	[CollateralTypeID] INT IDENTITY (1,1) NOT NULL,
	[CollateralType] NVARCHAR(100) NOT NULL, 
	[RelationshipGroupID] BIGINT NOT NULL,
	[SortOrder] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT (1), 
    [ModifiedBy] INT NOT NULL, 
    [ModifiedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [CreatedBy] INT NOT NULL DEFAULT (1), 
    [CreatedOn] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_CollateralType_CollateralTypeID] PRIMARY KEY CLUSTERED ([CollateralTypeID] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT IX_CollateralType UNIQUE (CollateralType),
	CONSTRAINT [FK_CollateralType_RelationshipGroupID] FOREIGN KEY ([RelationshipGroupID]) REFERENCES [Reference].[RelationshipGroup] ([RelationshipGroupID]),
	CONSTRAINT [FK_CollateralType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID]),
	CONSTRAINT [FK_CollateralType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the Collateral Types', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = CollateralType,
@level2type = N'COLUMN', @level2name = CollateralType;
GO
