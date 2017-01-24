-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ADGroup]
-- Author:		Kyle Campbell
-- Date:		04/28/2016
--
-- Purpose:		Active Directory Groups
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/28/2016	Kyle Campbell	TFS #10341	Initial Creation
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[ADGroup](
	[ADGroupID] BIGINT IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](1000) NULL,
	[ADGroupGUID] nvarchar(500) NULL,
	[ADFlag] bit NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ADGroup_ADGroupID] PRIMARY KEY CLUSTERED 
	(
		[ADGroupID] ASC
	)
	WITH 
   (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.[ADGroup] WITH CHECK ADD CONSTRAINT [FK_ADGroup_UserModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.[ADGroup] CHECK CONSTRAINT [FK_ADGroup_UserModifiedBy]
GO
ALTER TABLE Core.[ADGroup] WITH CHECK ADD CONSTRAINT [FK_ADGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.[ADGroup] CHECK CONSTRAINT [FK_ADGroup_UserCreatedBy]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Name of AD group', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = ADGroup,
@level2type = N'COLUMN', @level2name = Name;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Description of AD Group', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = ADGroup,
@level2type = N'COLUMN', @level2name = [Description];
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'AD Group Globally Unique Identifier', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = ADGroup,
@level2type = N'COLUMN', @level2name = ADGroupGUID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'AD Group Flag', 
@level0type = N'SCHEMA', @level0name = Core, 
@level1type = N'TABLE',  @level1name = ADGroup,
@level2type = N'COLUMN', @level2name = ADFlag;
GO