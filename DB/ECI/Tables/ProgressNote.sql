-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
CREATE TABLE ECI.ProgressNote (
	ProgressNoteID 					BIGINT IDENTITY(1,1) NOT NULL,
	NoteHeaderID 					BIGINT NULL,
	StartTime 						TIME NULL,
	EndTime 						TIME NULL,
	ContactMethodID 				INT NULL,
	ContactMethodOther 				NVARCHAR(50) NULL,
	FirstName 						NVARCHAR(200) NULL,
	LastName 						NVARCHAR(200) NULL,
	RelationshipTypeID 				INT NULL,
	Summary 						NVARCHAR(1000) NULL,
	ReviewedSourceConcerns 			BIT NULL ,
	ReviewedECIProcess				BIT NULL ,
	ReviewedECIServices				BIT NULL ,
	ReviewedECIRequirements			BIT NULL ,
	IsSurrogateParentNeeded			BIT NULL ,
	Comments						NVARCHAR(500) NULL,
	IsDischarged					BIT NULL ,
	ProgressNoteDate				DATETIME NULL,
	TakenBy							INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralProgressNote_ReferralProgressNoteID] PRIMARY KEY CLUSTERED 
(
	[ProgressNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[ProgressNote]  WITH CHECK ADD  CONSTRAINT [FK_ProgressNote_NoteHeaderID] FOREIGN KEY([NoteHeaderID])
REFERENCES [Registration].[NoteHeader] ([NoteHeaderID])
GO

ALTER TABLE [ECI].[ProgressNote] CHECK CONSTRAINT [FK_ProgressNote_NoteHeaderID]
GO

ALTER TABLE ECI.[ProgressNote]  WITH CHECK ADD  CONSTRAINT [FK_ProgressNote_ContactMethodID] FOREIGN KEY([ContactMethodID])
REFERENCES [Reference].[ContactMethod] ([ContactMethodID])
GO

ALTER TABLE [ECI].[ProgressNote] CHECK CONSTRAINT [FK_ProgressNote_ContactMethodID]
GO

ALTER TABLE [ECI].[ProgressNote]  WITH CHECK ADD  CONSTRAINT [FK_ProgressNote_RelationshipTypeID] FOREIGN KEY([RelationshipTypeID])
REFERENCES [Reference].[RelationshipType] ([RelationshipTypeID])
GO

ALTER TABLE [ECI].[ProgressNote] CHECK CONSTRAINT [FK_ProgressNote_RelationshipTypeID]
GO

ALTER TABLE ECI.ProgressNote WITH CHECK ADD CONSTRAINT [FK_ProgressNote_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ProgressNote CHECK CONSTRAINT [FK_ProgressNote_UserModifedBy]
GO
ALTER TABLE ECI.ProgressNote WITH CHECK ADD CONSTRAINT [FK_ProgressNote_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ProgressNote CHECK CONSTRAINT [FK_ProgressNote_UserCreatedBy]
GO
