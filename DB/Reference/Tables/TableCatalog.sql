-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[TableCatalog]
-- Author:		Scott Martin
-- Date:		09/14/2016
--
-- Purpose:		Lists and assigns an ID to all tables within the db
--
-- Notes:		Table will be populated and updated by stored procedure so any additional tables will be pulled in when added
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/14/2016	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[TableCatalog](
	[TableCatalogID] [int] NOT NULL IDENTITY(1,1),
	[SchemaName] [NVARCHAR] (200) NOT NULL,
	[TableName] [NVARCHAR] (200) NOT NULL,
	[Caption] [NVARCHAR] (200) NULL,
	[Description] [NVARCHAR] (500) NULL,
	[IsOptionSet] BIT NOT NULL DEFAULT(0),
	[IsUserOptionSet] BIT NOT NULL DEFAULT(0),
	[IsActive] BIT DEFAULT(1) NOT NULL,
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_TableCatalog_TableCatalogID] PRIMARY KEY CLUSTERED 
(
	[TableCatalogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.TableCatalog WITH CHECK ADD CONSTRAINT [FK_TableCatalog_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.TableCatalog CHECK CONSTRAINT [FK_TableCatalog_UserModifedBy]
GO
ALTER TABLE Reference.TableCatalog WITH CHECK ADD CONSTRAINT [FK_TableCatalog_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.TableCatalog CHECK CONSTRAINT [FK_TableCatalog_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'User Option Sets', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = TableCatalog;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Catalog of tables that users may update the values for', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = TableCatalog;
GO;

