-----------------------------------------------------------------------------------------------------------------------
-- Table:		Clinical.[Notes]
-- Author:		Scott Martin
-- Date:		11/13/2015
--
-- Purpose:		Clinical Note Data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Scott Martin    TFS# 3468		Initial Creation.
-- 11/19/2015	Scott Martin	Added DocumentStatus column
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[Notes](
	[NoteID] [bigint] IDENTITY(1,1) NOT NULL,
	[Notes] [nvarchar](max) NOT NULL,
	[NoteTypeID] [smallint] NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[EncounterID] BIGINT,
	[TakenBy] INT NOT NULL,
	[TakenTime] DATETIME NOT NULL,
	[DocumentStatusID] SMALLINT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED 
(
	[NoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Clinical].[Notes] CHECK CONSTRAINT [FK_Notes_Contact]
GO

ALTER TABLE [Clinical].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_EncounterID] FOREIGN KEY([EncounterID])
REFERENCES [Clinical].[Encounter] ([EncounterID])
GO

ALTER TABLE [Clinical].[Notes] CHECK CONSTRAINT [FK_Notes_EncounterID]
GO

ALTER TABLE [Clinical].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_NoteType] FOREIGN KEY([NoteTypeID])
REFERENCES [Clinical].[NoteType] ([NoteTypeID])
GO

ALTER TABLE [Clinical].[Notes] CHECK CONSTRAINT [FK_Notes_NoteType]
GO

ALTER TABLE [Clinical].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_DocumentStatus] FOREIGN KEY([DocumentStatusID])
REFERENCES [Reference].[DocumentStatus] ([DocumentStatusID])
GO

ALTER TABLE [Clinical].[Notes] CHECK CONSTRAINT [FK_Notes_DocumentStatus]
GO

ALTER TABLE Clinical.Notes WITH CHECK ADD CONSTRAINT [FK_Notes_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Notes CHECK CONSTRAINT [FK_Notes_UserModifedBy]
GO
ALTER TABLE Clinical.Notes WITH CHECK ADD CONSTRAINT [FK_Notes_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Notes CHECK CONSTRAINT [FK_Notes_UserCreatedBy]
GO
