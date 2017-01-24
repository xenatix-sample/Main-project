-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
CREATE TABLE [ECI].[ProgressNoteAssessment]
(
	[ProgressNoteAssessmentID] BIGINT NOT NULL PRIMARY KEY, 
    [ProgressNoteID] BIGINT NULL, 
    [NoteAssessmentDate] DATETIME NULL, 
    [NoteAssessmentTime] TIME NULL, 
    [LocationID] INT NULL, 
    [ProviderID] INT NULL, 
    [MembersInvited] NVARCHAR(1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
)

GO

ALTER TABLE ECI.ProgressNoteAssessment WITH CHECK ADD CONSTRAINT [FK_ProgressNoteAssessment_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ProgressNoteAssessment CHECK CONSTRAINT [FK_ProgressNoteAssessment_UserModifedBy]
GO
ALTER TABLE ECI.ProgressNoteAssessment WITH CHECK ADD CONSTRAINT [FK_ProgressNoteAssessment_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ProgressNoteAssessment CHECK CONSTRAINT [FK_ProgressNoteAssessment_UserCreatedBy]
GO
