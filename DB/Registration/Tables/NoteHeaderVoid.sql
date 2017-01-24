-----------------------------------------------------------------------------------------------------------------------
-- Table:		[NoteHeaderVoid]
-- Author:		Scott Martin
-- Date:		04/05/2016
--
-- Purpose:		Allows for the voiding of notes without inactivating them
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/05/2016	Scott Martin		Initial Creation
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Registration.NoteHeaderVoid
(
	NoteHeaderVoidID BIGINT IDENTITY(1,1) NOT NULL,
	NoteHeaderID BIGINT NOT NULL,
	NoteHeaderVoidReasonID SMALLINT NULL,
	Comments NVARCHAR(1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_NoteHeaderVoid_NoteHeaderVoidID] PRIMARY KEY CLUSTERED 
(
	[NoteHeaderVoidID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[NoteHeaderVoid]  WITH CHECK ADD  CONSTRAINT [FK_NoteHeaderVoid_NoteHeaderID] FOREIGN KEY([NoteHeaderID])
REFERENCES [Registration].[NoteHeader] ([NoteHeaderID])
GO

ALTER TABLE [Registration].NoteHeaderVoid CHECK CONSTRAINT [FK_NoteHeaderVoid_NoteHeaderID]
GO

ALTER TABLE [Registration].[NoteHeaderVoid]  WITH CHECK ADD  CONSTRAINT [FK_NoteHeaderVoid_NoteHeaderVoidReasonID] FOREIGN KEY([NoteHeaderVoidReasonID])
REFERENCES [Reference].[NoteHeaderVoidReason] ([NoteHeaderVoidReasonID])
GO

ALTER TABLE [Registration].NoteHeaderVoid CHECK CONSTRAINT [FK_NoteHeaderVoid_NoteHeaderID]
GO

ALTER TABLE [Registration].NoteHeaderVoid WITH CHECK ADD CONSTRAINT [FK_NoteHeaderVoid_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Registration].NoteHeaderVoid CHECK CONSTRAINT [FK_NoteHeaderVoid_UserModifedBy]
GO
ALTER TABLE [Registration].NoteHeaderVoid WITH CHECK ADD CONSTRAINT [FK_NoteHeaderVoid_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Registration].NoteHeaderVoid CHECK CONSTRAINT [FK_NoteHeaderVoid_UserCreatedBy]
GO
