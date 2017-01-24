-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.County
-- Author:		Sumana Sangapu
-- Date:		07/24/2015
--
-- Purpose:		Holds County data 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Reference.StateProvince
--				
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/24/2015	Sumana Sangapu	TFS# 674  - Initial creation.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from Reference to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 07/06/2016	Sumana Sangapu	Added LEgacyCode for CMHC 
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[County](
	[CountyID] [int] IDENTITY(1,1) NOT NULL,
	[StateProvinceID] [int] NULL,
	[CountyName] [nvarchar](50) NULL,
	[CountyLatitude] [decimal](10, 6) NULL,
	[CountyLongitude] [decimal](10, 6) NULL,
	[LegacyCode] nvarchar(10) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_County_CountyID] PRIMARY KEY CLUSTERED 
(
	[CountyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_County_CountyName_StateProvinceID] UNIQUE NONCLUSTERED 
(
	[CountyName] ASC,
	[StateProvinceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[County]  WITH CHECK ADD  CONSTRAINT [FK_County_StateProvinceID] FOREIGN KEY([StateProvinceID])
REFERENCES [Reference].[StateProvince] ([StateProvinceID])
GO

ALTER TABLE [Reference].[County] CHECK CONSTRAINT [FK_County_StateProvinceID]
GO
CREATE INDEX [IX_County_StateProvinceID] ON [Reference].[County] ([StateProvinceID], [IsActive]) INCLUDE ([CountyID], [CountyName])
GO

ALTER TABLE Reference.County WITH CHECK ADD CONSTRAINT [FK_County_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.County CHECK CONSTRAINT [FK_County_UserModifedBy]
GO
ALTER TABLE Reference.County WITH CHECK ADD CONSTRAINT [FK_County_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.County CHECK CONSTRAINT [FK_County_UserCreatedBy]
GO

CREATE NONCLUSTERED INDEX [IX_County_IsActive_CountyID_StateProvinceID] ON [Reference].[County]
(
	[IsActive] ASC,
	[CountyID] ASC,
	[StateProvinceID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'County', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = County;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Counties', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = County;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = County;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = County;
GO;