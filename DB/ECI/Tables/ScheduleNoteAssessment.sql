-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ScheduleNoteAssessment]
-- Author:		Gurpreet Singh
-- Date:		1/12/2016
--
-- Purpose:		ECI Progress Note Appointment
-- Notes:		N/A (or any additional notes)
--
-- Depends:		ECI.[[ProgressNote]]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/14/2016	Scott Martin		Added new columns
-- 01/29/2016   Satish Singh        Added Location
-- 02/25/2016	Kyle Campbell		Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ScheduleNoteAssessment](
	[ScheduleNoteAssessmentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProgressNoteID] [bigint] NULL,
	[NoteAssessmentDate] [datetime] NULL,
	[NoteAssessmentTime] [time](7) NULL,
	[LocationID] [int] NULL,
	[Location] [nvarchar](500) NULL,
	[ProviderID] [int] NULL,
	[MembersInvited] [nvarchar](1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK__Schedule__96B12A6E6A8334A5] PRIMARY KEY CLUSTERED 
(
	[ScheduleNoteAssessmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[ScheduleNoteAssessment]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleNoteAssessment_ProgressNote] FOREIGN KEY([ProgressNoteID])
REFERENCES [ECI].[ProgressNote] ([ProgressNoteID])
GO

ALTER TABLE [ECI].[ScheduleNoteAssessment] CHECK CONSTRAINT [FK_ScheduleNoteAssessment_ProgressNote]
GO

ALTER TABLE ECI.ScheduleNoteAssessment WITH CHECK ADD CONSTRAINT [FK_ScheduleNoteAssessment_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScheduleNoteAssessment CHECK CONSTRAINT [FK_ScheduleNoteAssessment_UserModifedBy]
GO
ALTER TABLE ECI.ScheduleNoteAssessment WITH CHECK ADD CONSTRAINT [FK_ScheduleNoteAssessment_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScheduleNoteAssessment CHECK CONSTRAINT [FK_ScheduleNoteAssessment_UserCreatedBy]
GO
