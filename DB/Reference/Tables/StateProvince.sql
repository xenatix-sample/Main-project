-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.StateProvince
-- Author:		Sumana Sangapu
-- Date:		07/24/2015
--
-- Purpose:		Holds StateProvine(States) data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Reference.Country
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/24/2015	Sumana Sangapu	TFS#	 674 - Initial creation.
-- 07/30/2015	Sumana Sangapu			 1016	Change schema from dbo to Registration/Reference/Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-- 09/07/2016	Rahul Vats		Review the table
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Reference].[StateProvince](
	[StateProvinceID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NOT NULL,
	[StateProvinceCode] [nvarchar](2) NOT NULL,
	[StateProvinceName] [nvarchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_StateProvince_StateProvinceID] PRIMARY KEY CLUSTERED 
(
	[StateProvinceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_StateProvince_StateProvinceName_CountryID] UNIQUE NONCLUSTERED 
(
	[CountryID] ASC,
	[StateProvinceName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[StateProvince]  WITH CHECK ADD  CONSTRAINT [FK_StateProvince_CountryID] FOREIGN KEY([CountryID]) REFERENCES [Reference].[Country] ([CountryID])
GO
ALTER TABLE [Reference].[StateProvince] CHECK CONSTRAINT [FK_StateProvince_CountryID]
GO
ALTER TABLE Reference.StateProvince WITH CHECK ADD CONSTRAINT [FK_StateProvince_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.StateProvince CHECK CONSTRAINT [FK_StateProvince_UserModifedBy]
GO
ALTER TABLE Reference.StateProvince WITH CHECK ADD CONSTRAINT [FK_StateProvince_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.StateProvince CHECK CONSTRAINT [FK_StateProvince_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'State or Province', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = StateProvince;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'States and Provinces', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = StateProvince;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = StateProvince;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = StateProvince;
GO;