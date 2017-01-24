-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
CREATE TABLE [Core].[UserSecurityRoleFacilityProgram] (
UserSecurityRoleFacilityProgramID BIGINT NOT NULL IDENTITY(1,1),
    [UserID]            INT      NOT NULL,
    [SecurityRoleID]    INT      NOT NULL,
    [FacilityProgramID] INT      NOT NULL,
    [EffectiveFrom]     DATETIME NULL,
    [EffectiveTo]       DATETIME NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_UserSecurityRoleFacilityProgram] PRIMARY KEY ([UserSecurityRoleFacilityProgramID])
);
GO

ALTER TABLE Core.UserSecurityRoleFacilityProgram WITH CHECK ADD CONSTRAINT [FK_UserSecurityRoleFacilityProgram_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserSecurityRoleFacilityProgram CHECK CONSTRAINT [FK_UserSecurityRoleFacilityProgram_UserModifedBy]
GO
ALTER TABLE Core.UserSecurityRoleFacilityProgram WITH CHECK ADD CONSTRAINT [FK_UserSecurityRoleFacilityProgram_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserSecurityRoleFacilityProgram CHECK CONSTRAINT [FK_UserSecurityRoleFacilityProgram_UserCreatedBy]
GO

