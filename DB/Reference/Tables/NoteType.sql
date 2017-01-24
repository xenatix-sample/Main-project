-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.NoteType
-- Author:		Scott Martin
-- Date:		1/7/2016
--
-- Purpose:		Associate Module Note Types with their specific types
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/7/2016	Scott Martin		Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Reference.NoteType (
	NoteTypeID						INT IDENTITY(1,1) NOT NULL,
	NoteType						NVARCHAR(50) NOT NULL,
	ModuleNoteTypeID				INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_NoteType_NoteTypeID] PRIMARY KEY CLUSTERED 
(
	[NoteTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[NoteType]  WITH CHECK ADD  CONSTRAINT [FK_NoteType_ModuleNoteTypeID] FOREIGN KEY([ModuleNoteTypeID])
REFERENCES [Reference].[ModuleNoteType] ([ModuleNoteTypeID])
GO

ALTER TABLE [Reference].[NoteType] CHECK CONSTRAINT [FK_NoteType_ModuleNoteTypeID]
GO

ALTER TABLE Reference.NoteType WITH CHECK ADD CONSTRAINT [FK_NoteType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.NoteType CHECK CONSTRAINT [FK_NoteType_UserModifedBy]
GO
ALTER TABLE Reference.NoteType WITH CHECK ADD CONSTRAINT [FK_NoteType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.NoteType CHECK CONSTRAINT [FK_NoteType_UserCreatedBy]
GO
