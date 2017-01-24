-----------------------------------------------------------------------------------------------------------------------
-- Table:		[AdvancedDirectiveType]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Advance Directive data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Modification .
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 03/08/02016	Scott Martin	Changed table name to AdvancedDirectiveType from FullCodeDNRType and modified columns accordingly
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AdvancedDirectiveType]
(
	[AdvancedDirectiveTypeID] INT NOT NULL IDENTITY(1,1) , 
    [AdvancedDirectiveType] NVARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [PK_AdvancedDirectiveType_AdvancedDirectiveTypeID] PRIMARY KEY CLUSTERED ([AdvancedDirectiveTypeID] ASC),
)
GO

ALTER TABLE [Reference].[AdvancedDirectiveType] ADD CONSTRAINT IX_AdvancedDirectiveType UNIQUE(AdvancedDirectiveType)
GO
ALTER TABLE Reference.AdvancedDirectiveType WITH CHECK ADD CONSTRAINT [FK_AdvancedDirectiveType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AdvancedDirectiveType CHECK CONSTRAINT [FK_AdvancedDirectiveType_UserModifedBy]
GO
ALTER TABLE Reference.AdvancedDirectiveType WITH CHECK ADD CONSTRAINT [FK_AdvancedDirectiveType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AdvancedDirectiveType CHECK CONSTRAINT [FK_AdvancedDirectiveType_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Advanced Directive Type', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AdvancedDirectiveType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating instructions for treatment', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AdvancedDirectiveType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AdvancedDirectiveType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AdvancedDirectiveType;
GO;