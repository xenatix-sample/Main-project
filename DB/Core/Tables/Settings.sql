-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Settings]
-- Author:		Sumana Sangapu
-- Date:		07/29/2015
--
-- Purpose:		Lookup for Configuration Settings 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TFS#/WorkItem  875 - Initial creation.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015   Justin Spalti  TFS# 875 - Updated the schema of the SettingsType and Settings tables to Core.
-- 08/07/2015   Justin Spalti  TFS 875 - Added the SID column and a unique constraint to handle the need to have duplicate SettingIDs
-- 09/04/2015   Justin Spalti - Updated the table to include the IsDisplayed column which will specify whether a setting's value will be displayed in the UI.	
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Settings](
	[SID] [int] IDENTITY(1,1) NOT NULL,
	[SettingsID] [int] NOT NULL,
	[Settings] [nvarchar](50) NOT NULL,
	[SettingsTypeID] [int] NOT NULL,
	[IsConfigurable] [bit] NOT NULL DEFAULT(1),
	[IsCachable] [bit] NOT NULL DEFAULT(1),
	[IsDisplayed] [bit] NOT NULL DEFAULT (1),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Settings_SID] PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Settings] UNIQUE NONCLUSTERED 
(
	[SettingsID] ASC,
	[Settings] ASC,
	[SettingsTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_SettingsTypeID] FOREIGN KEY([SettingsTypeID])
REFERENCES [Core].[SettingsType] ([SettingsTypeID])
GO

ALTER TABLE [Core].[Settings] CHECK CONSTRAINT [FK_Settings_SettingsTypeID]
GO

ALTER TABLE Core.Settings WITH CHECK ADD CONSTRAINT [FK_Settings_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Settings CHECK CONSTRAINT [FK_Settings_UserModifedBy]
GO
ALTER TABLE Core.Settings WITH CHECK ADD CONSTRAINT [FK_Settings_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Settings CHECK CONSTRAINT [FK_Settings_UserCreatedBy]
GO

