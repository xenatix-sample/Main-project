-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[SchoolDistrict]
-- Author:		Sumana Sangapu
-- Date:		08/05/2013
--
-- Purpose:		Lookup for School District details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		[Reference].[County] 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Sumana Sangapu	TFS# 972 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 06/27/2016	Sumana Sangapu	LegacyCode and LegacyCodeDescription
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[SchoolDistrict](
	[SchoolDistrictID] [int] IDENTITY(1,1) NOT NULL,
	[CountyID] [int] NOT NULL,
	[SchoolDistrictName] [nvarchar](100) NOT NULL,
	[LegacyCode] nvarchar(10) NULL,
	[LegacyCodeDescription] nvarchar(100) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SchoolDistrict_SchoolDistrictID] PRIMARY KEY CLUSTERED 
(
	[SchoolDistrictID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SchoolDistrict_SchoolDistrictName] UNIQUE NONCLUSTERED 
(
	[SchoolDistrictName] ASC,
	[CountyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[SchoolDistrict]  WITH CHECK ADD  CONSTRAINT [FK_SchoolDistrict_CountyID] FOREIGN KEY([CountyID]) REFERENCES [Reference].[County] ([CountyID])
GO
ALTER TABLE [Reference].[SchoolDistrict] CHECK CONSTRAINT [FK_SchoolDistrict_CountyID]
GO
ALTER TABLE Reference.SchoolDistrict WITH CHECK ADD CONSTRAINT [FK_SchoolDistrict_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SchoolDistrict CHECK CONSTRAINT [FK_SchoolDistrict_UserModifedBy]
GO
ALTER TABLE Reference.SchoolDistrict WITH CHECK ADD CONSTRAINT [FK_SchoolDistrict_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SchoolDistrict CHECK CONSTRAINT [FK_SchoolDistrict_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'School District', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SchoolDistrict;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating school district', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SchoolDistrict;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SchoolDistrict;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SchoolDistrict;
GO;
