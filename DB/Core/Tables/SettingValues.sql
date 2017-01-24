-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[SettingValues]
-- Author:		Sumana Sangapu
-- Date:		07/29/2015
--
-- Purpose:		Holds data for Configuration Setting Values
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Registration.SettingValues
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Sumana Sangapu	TFS#/WorkItem  875 - Initial creation.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/07/2015   Justin Spalti TFS: 875 - Added the SID column so that a FK could be associated with the Settings table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

 CREATE TABLE [Core].[SettingValues](
	[SettingValuesID] [int] IDENTITY(1,1) NOT NULL,
	[SettingsID] [int] NOT NULL,
	[SettingsTypeID] [int] NOT NULL,
	[Value] [nvarchar](50) NULL,
	[EntityID] [int] NULL,
	[SID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SettingValues_SettingValuesID] PRIMARY KEY CLUSTERED 
(
	[SettingValuesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[SettingValues]  WITH CHECK ADD  CONSTRAINT [FK_SettingValues_SettingsTypeID] FOREIGN KEY([SettingsTypeID])
REFERENCES [Core].[SettingsType] ([SettingsTypeID])
GO

ALTER TABLE [Core].[SettingValues] CHECK CONSTRAINT [FK_SettingValues_SettingsTypeID]
GO

ALTER TABLE [Core].[SettingValues]  WITH CHECK ADD  CONSTRAINT [FK_SettingValues_SID] FOREIGN KEY([SID])
REFERENCES [Core].[Settings] ([SID])
GO

ALTER TABLE [Core].[SettingValues] CHECK CONSTRAINT [FK_SettingValues_SID]
GO

ALTER TABLE Core.SettingValues WITH CHECK ADD CONSTRAINT [FK_SettingValues_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SettingValues CHECK CONSTRAINT [FK_SettingValues_UserModifedBy]
GO
ALTER TABLE Core.SettingValues WITH CHECK ADD CONSTRAINT [FK_SettingValues_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SettingValues CHECK CONSTRAINT [FK_SettingValues_UserCreatedBy]
GO
