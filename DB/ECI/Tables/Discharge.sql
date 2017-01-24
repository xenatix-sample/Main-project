-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
CREATE TABLE ECI.Discharge (
	DischargeID			BIGINT IDENTITY(1,1) NOT NULL,
	ProgressNoteID		BIGINT NOT NULL,
	DischargeTypeID		INT NULL,
	TakenBy				INT NULL, 
	DischargeDate		DATE NULL,
	DischargeReasonID	INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 
    CONSTRAINT [PK_Discharge_DischargeID] PRIMARY KEY CLUSTERED 
(
	[DischargeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ECI.[Discharge]  WITH CHECK ADD  CONSTRAINT [FK_Discharge_ProgressNoteID] FOREIGN KEY([ProgressNoteID])
REFERENCES [ECI].[ProgressNote] ([ProgressNoteID])
GO

ALTER TABLE ECI.[Discharge] CHECK CONSTRAINT [FK_Discharge_ProgressNoteID]
GO

ALTER TABLE ECI.[Discharge]  WITH CHECK ADD  CONSTRAINT [FK_Discharge_DischargeTypeID] FOREIGN KEY([DischargeTypeID])
REFERENCES [Reference].[DischargeType] ([DischargeTypeID])
GO

ALTER TABLE ECI.[Discharge] CHECK CONSTRAINT [FK_Discharge_DischargeTypeID]
GO

ALTER TABLE ECI.Discharge WITH CHECK ADD CONSTRAINT [FK_Discharge_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Discharge CHECK CONSTRAINT [FK_Discharge_UserModifedBy]
GO
ALTER TABLE ECI.Discharge WITH CHECK ADD CONSTRAINT [FK_Discharge_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Discharge CHECK CONSTRAINT [FK_Discharge_UserCreatedBy]
GO
