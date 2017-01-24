----------------------------------------------------------------------------------------------------------------------
-- Table:		Synch.Config
-- Author:		
-- Date:		
--
-- Purpose:		Add Config Values
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/26/2016   RAV 			Added Unique Constraint on ConfigName, ConfigTypeID, Prettifying the code
-- 09/03/2016	Rahul Vats		Review the table
-- 09/20/2016	Sumana Sangapu	Changed the datatype of ConfigXML from XML to 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[Config](
	[ConfigID] [int] IDENTITY(1,1) NOT NULL,
	[ConfigName] [nvarchar](255) NOT NULL,
	[ConfigXML] nvarchar(2000) NOT NULL, -- Changed the datatype from XML to nvarchar(2000). Cannot change the columnname as it is being used in multiple places.
	[ConfigTypeID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_CreatedOn]  DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL CONSTRAINT [DF_SystemCreatedOn]  DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_SystemModifiedOn]  DEFAULT (getutcdate()),
	CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED ( [ConfigID] ASC),
	CONSTRAINT [IX_ConfigName_ConfigType] UNIQUE NONCLUSTERED ( [ConfigName] ASC, [ConfigTypeID] ASC )
)
GO

ALTER TABLE Synch.Config WITH CHECK ADD CONSTRAINT [FK_Config_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.Config CHECK CONSTRAINT [FK_Config_UserModifedBy]
GO
ALTER TABLE Synch.Config WITH CHECK ADD CONSTRAINT [FK_Config_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.Config CHECK CONSTRAINT [FK_Config_UserCreatedBy]
GO
