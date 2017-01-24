-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.Ethnicity
-- Author:		Sumana Sangapu
-- Date:		07/24/2015
--
-- Purpose:		Holds Ethnicity data 
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
-- 06/27/2016	Sumana Sangapu	LegacyCode and LegacyCodeDescription
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[Ethnicity](
	[EthnicityID] [int] IDENTITY (1,1) NOT NULL,
	[Ethnicity] [nvarchar](50) NULL,
	[LegacyCode] nvarchar(10) NULL,
	[LegacyCodeDescription] nvarchar(100) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Ethnicity_EthnicityID] PRIMARY KEY CLUSTERED 
(
	[EthnicityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[Ethnicity] ADD CONSTRAINT IX_Ethnicity UNIQUE(Ethnicity)
GO
ALTER TABLE Reference.Ethnicity WITH CHECK ADD CONSTRAINT [FK_Ethnicity_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.Ethnicity CHECK CONSTRAINT [FK_Ethnicity_UserModifedBy]
GO
ALTER TABLE Reference.Ethnicity WITH CHECK ADD CONSTRAINT [FK_Ethnicity_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.Ethnicity CHECK CONSTRAINT [FK_Ethnicity_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Ethnicity', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Ethnicity;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating ethnic background', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Ethnicity;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Ethnicity;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = Ethnicity;
GO;