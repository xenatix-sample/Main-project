-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[AppointmentNote]
-- Author:		John Crossen
-- Date:		03/09/2016
--
-- Purpose:		Store Appointment Notes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/08/2016	John Crossen	TFS#6105 - Initial creation.

CREATE TABLE [Scheduling].AppointmentNote(
[AppointmentNoteID] BIGINT NOT NULL IDENTITY(1,1),
[NoteHeaderID] BIGINT NULL,
[GroupHeaderID] BIGINT NULL,
[UserID] INT NULL,
[AppointmentID] BIGINT NOT NULL,
[NoteText] NVARCHAR(MAX) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
  
    CONSTRAINT [PK_AppointmentNote] PRIMARY KEY CLUSTERED 
(
	[AppointmentNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
) ON [PRIMARY]

GO

ALTER TABLE Scheduling.AppointmentNote WITH CHECK ADD CONSTRAINT [FK_AppointmentNote_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentNote CHECK CONSTRAINT [FK_AppointmentNote_UserModifedBy]
GO
ALTER TABLE Scheduling.AppointmentNote WITH CHECK ADD CONSTRAINT [FK_AppointmentNote_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentNote CHECK CONSTRAINT [FK_AppointmentNote_UserCreatedBy]
GO
ALTER TABLE Scheduling.AppointmentNote WITH CHECK ADD CONSTRAINT [FK_AppointmentNote_NoteHeaderID] FOREIGN KEY(NoteHeaderID) REFERENCES Registration.NoteHeader (NoteHeaderID)
GO
ALTER TABLE Scheduling.AppointmentNote CHECK CONSTRAINT [FK_AppointmentNote_NoteHeaderID]
GO
ALTER TABLE Scheduling.AppointmentNote WITH CHECK ADD CONSTRAINT [FK_AppointmentNote_GroupHeaderID] FOREIGN KEY(GroupHeaderID) REFERENCES Scheduling.GroupSchedulingHeader (GroupHeaderID)
GO
ALTER TABLE Scheduling.AppointmentNote CHECK CONSTRAINT [FK_AppointmentNote_GroupHeaderID]
GO
ALTER TABLE Scheduling.AppointmentNote WITH CHECK ADD CONSTRAINT [FK_AppointmentNote_AppointmentID] FOREIGN KEY(AppointmentID) REFERENCES Scheduling.Appointment (AppointmentID)
GO
ALTER TABLE Scheduling.AppointmentNote CHECK CONSTRAINT [FK_AppointmentNote_AppointmentID]
GO
ALTER TABLE Scheduling.AppointmentNote WITH CHECK ADD CONSTRAINT [FK_AppointmentNote_UserID] FOREIGN KEY(UserID) REFERENCES Core.Users (UserID)
GO
ALTER TABLE Scheduling.AppointmentNote CHECK CONSTRAINT [FK_AppointmentNote_UserID]
GO





