-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
CREATE TABLE [Core].[TeamFacilityProgram] (
    [TeamFacilityProgramID] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_TeamFacilityProgramID] PRIMARY KEY CLUSTERED 
	(
	[TeamFacilityProgramID] ASC
	)
WITH 
   (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.TeamFacilityProgram WITH CHECK ADD CONSTRAINT [FK_TeamFacilityProgram_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.TeamFacilityProgram CHECK CONSTRAINT [FK_TeamFacilityProgram_UserModifedBy]
GO
ALTER TABLE Core.TeamFacilityProgram WITH CHECK ADD CONSTRAINT [FK_TeamFacilityProgram_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.TeamFacilityProgram CHECK CONSTRAINT [FK_TeamFacilityProgram_UserCreatedBy]
GO

