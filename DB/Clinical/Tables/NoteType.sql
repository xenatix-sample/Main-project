-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[NoteType]
-- Author:		Scott Martin
-- Date:		11/13/2015
--
-- Purpose:		Link NoteTypes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/11/2015	Scott Martin	TFS# 3468 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[NoteType](
	[NoteTypeID] [smallint] IDENTITY(1,1) NOT NULL,
	[NoteType] [nvarchar](255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_NoteType] PRIMARY KEY CLUSTERED 
(
	[NoteTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_NoteType_NoteType] ON [Clinical].[NoteType]
(
	[NoteType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Clinical.NoteType WITH CHECK ADD CONSTRAINT [FK_NoteType_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.NoteType CHECK CONSTRAINT [FK_NoteType_UserModifedBy]
GO
ALTER TABLE Clinical.NoteType WITH CHECK ADD CONSTRAINT [FK_NoteType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.NoteType CHECK CONSTRAINT [FK_NoteType_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Note Type', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = NoteType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating kind of note', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = NoteType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = NoteType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Clinical, 
@level1type = N'TABLE',  @level1name = NoteType;
GO;