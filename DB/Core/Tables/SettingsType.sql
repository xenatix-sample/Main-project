-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[SettingsType]
-- Author:		Sumana Sangapu
-- Date:		07/29/2015
--
-- Purpose:		Lookup for Configuration SettingsType 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Sumana Sangapu	TFS#/WorkItem  875 - Initial creation.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015   Justin Spalti  TFS# 875 - Updated the schema of the SettingsType and Settings tables to Core.
-- 08/07/2015   Justin Spalti  TFS 875 - Updated the table to reflect the new FKs needed after refactoring the Settings and SettingsValues tables
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[SettingsType](
	[SettingsTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SettingsType] [nvarchar](50) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SettingsType_SettingsTypeID] PRIMARY KEY CLUSTERED 
(
	[SettingsTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SettingsType] UNIQUE NONCLUSTERED 
(
	[SettingsType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Core.SettingsType WITH CHECK ADD CONSTRAINT [FK_SettingsType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SettingsType CHECK CONSTRAINT [FK_SettingsType_UserModifedBy]
GO
ALTER TABLE Core.SettingsType WITH CHECK ADD CONSTRAINT [FK_SettingsType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.SettingsType CHECK CONSTRAINT [FK_SettingsType_UserCreatedBy]
GO
