-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.ModuleNoteType
-- Author:		Scott Martin
-- Date:		1/7/2016
--
-- Purpose:		Associate different kinds of notes with their respective modules
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/7/2016	Scott Martin		Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Reference.ModuleNoteType (
	ModuleNoteTypeID				INT IDENTITY(1,1) NOT NULL,
	ModuleNoteNoteType				NVARCHAR(50) NOT NULL,
	ModuleID						BIGINT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ModuleNoteType_ModuleNoteTypeID] PRIMARY KEY CLUSTERED 
(
	[ModuleNoteTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ModuleNoteType]  WITH CHECK ADD  CONSTRAINT [FK_ModuleNoteType_ModuleID] FOREIGN KEY([ModuleID])
REFERENCES [Core].[Module] ([ModuleID])
GO

ALTER TABLE [Reference].[ModuleNoteType] CHECK CONSTRAINT [FK_ModuleNoteType_ModuleID]
GO

ALTER TABLE Reference.ModuleNoteType WITH CHECK ADD CONSTRAINT [FK_ModuleNoteType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ModuleNoteType CHECK CONSTRAINT [FK_ModuleNoteType_UserModifedBy]
GO
ALTER TABLE Reference.ModuleNoteType WITH CHECK ADD CONSTRAINT [FK_ModuleNoteType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ModuleNoteType CHECK CONSTRAINT [FK_ModuleNoteType_UserCreatedBy]
GO
