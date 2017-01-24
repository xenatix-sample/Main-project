-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ServerResourceType]
-- Author:		Suersh Pandey
-- Date:		07/29/2015
--
-- Purpose:		This table will store the type of server (e.g. Presentation Engine, Rule Engine and Data Engine)
--
-- Notes:		n/a 
--
-- Depends:		n/a 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Suresh Pandey	TFS#  - Initial creation.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[ServerResourceType]
(
	[ServerResourceTypeID]  INT IDENTITY(1,1) NOT NULL,
    [ServerResourceType] NVARCHAR(200) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_ServerResourceType_ServerResourceTypeID] PRIMARY KEY CLUSTERED  ([ServerResourceTypeID] ASC)
)
GO
CREATE UNIQUE INDEX [IX_ServerResourceType_ServerResourceType] ON [Reference].[ServerResourceType] ([ServerResourceType])
GO

ALTER TABLE Reference.ServerResourceType WITH CHECK ADD CONSTRAINT [FK_ServerResourceType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ServerResourceType CHECK CONSTRAINT [FK_ServerResourceType_UserModifedBy]
GO
ALTER TABLE Reference.ServerResourceType WITH CHECK ADD CONSTRAINT [FK_ServerResourceType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ServerResourceType CHECK CONSTRAINT [FK_ServerResourceType_UserCreatedBy]
GO
