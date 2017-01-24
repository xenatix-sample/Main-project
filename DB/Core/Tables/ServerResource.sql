-----------------------------------------------------------------------------------------------------------------------
-- Table:	[Core].[ServerResource]
-- Author:		Suersh Pandey
-- Date:		07/29/2015
--
-- Purpose:		This table will store the server IP addresses for validating request come from valid server.
--
-- Notes:		n/a 
--
-- Depends:		[Core].[ServerResourceType]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Name (or email)		TFS# 100 - Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn		
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ServerResource](
	[ServerResourceID] INT IDENTITY(1,1) NOT NULL,
	[ServerResourceTypeID] INT NOT NULL,
	[ServerName] [nvarchar](200) NULL,
	[ServerIP] [nvarchar](200) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ServerResource_ServerResourceID] PRIMARY KEY ([ServerResourceID]), 
    CONSTRAINT [FK_ServerResource_ServerResourceType] FOREIGN KEY ([ServerResourceTypeID]) REFERENCES [Reference].[ServerResourceType]([ServerResourceTypeID])
)
GO

ALTER TABLE Core.ServerResource WITH CHECK ADD CONSTRAINT [FK_ServerResource_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServerResource CHECK CONSTRAINT [FK_ServerResource_UserModifedBy]
GO
ALTER TABLE Core.ServerResource WITH CHECK ADD CONSTRAINT [FK_ServerResource_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServerResource CHECK CONSTRAINT [FK_ServerResource_UserCreatedBy]
GO
