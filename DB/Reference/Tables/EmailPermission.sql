-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[EmailPermission]
-- Author:		Sumana Sangapu
-- Date:		08/13/2015
--
-- Purpose:		Lookup for Email Permission
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/13/2015	Sumana Sangapu			 Task #: 1227 - Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[EmailPermission]
(
	[EmailPermissionID] INT IDENTITY (1,1) NOT NULL,
    [EmailPermission] NVARCHAR(20) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_EmailPermission_EmailPermissionID] PRIMARY KEY CLUSTERED ([EmailPermissionID] ASC),
)
GO
ALTER TABLE [Reference].[EmailPermission] ADD CONSTRAINT IX_EmailPermission UNIQUE([EmailPermission])
GO
ALTER TABLE Reference.EmailPermission WITH CHECK ADD CONSTRAINT [FK_EmailPermission_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.EmailPermission CHECK CONSTRAINT [FK_EmailPermission_UserModifedBy]
GO
ALTER TABLE Reference.EmailPermission WITH CHECK ADD CONSTRAINT [FK_EmailPermission_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.EmailPermission CHECK CONSTRAINT [FK_EmailPermission_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Email Permission', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EmailPermission;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating if/how email may be used', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EmailPermission;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EmailPermission;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EmailPermission;
GO;