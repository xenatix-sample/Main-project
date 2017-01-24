-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[MailPermission]
-- Author:		Suresh Pandey
-- Date:		07/30/2015
--
-- Purpose:		Lookup for US Mail Permission Type
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/30/2015	Suresh Pandey			 TFS#  - Initial creation.
-- 07/30/2015	Sumana Sangapu			 1016	Change schema from dbo to Registration/Reference/Core
-- 08/13/2015	Sumana Sangapu			 Task #: 1227 - Refactor schema
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[MailPermission]
(
	[MailPermissionID] INT IDENTITY (1,1) NOT NULL,
    [MailPermission] NVARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_MailPermission_MailPermissionID] PRIMARY KEY CLUSTERED ([MailPermissionID] ASC),
)
GO
ALTER TABLE [Reference].[MailPermission] ADD CONSTRAINT IX_MailPermission UNIQUE([MailPermission])
GO
ALTER TABLE Reference.MailPermission WITH CHECK ADD CONSTRAINT [FK_MailPermission_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.MailPermission CHECK CONSTRAINT [FK_MailPermission_UserModifedBy]
GO
ALTER TABLE Reference.MailPermission WITH CHECK ADD CONSTRAINT [FK_MailPermission_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.MailPermission CHECK CONSTRAINT [FK_MailPermission_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Address Permission', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = MailPermission;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating if/how address may be used', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = MailPermission;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = MailPermission;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = MailPermission;
GO;