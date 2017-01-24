-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[CareTeam]
-- Author:		Sumana Sangapu
-- Date:		02/09/2016
--
-- Purpose:		To hold the Care team details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/09/2016	Sumana Sangapu	  Initital Creation . 
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[CareTeam](
	[CareTeamID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[UserID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_CareTeamID] PRIMARY KEY CLUSTERED 
(
	[CareTeamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE [Registration].[CareTeam]  WITH CHECK ADD  CONSTRAINT [FK_CareTeam_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact]  ([ContactID])
GO

ALTER TABLE [Registration].[CareTeam] CHECK CONSTRAINT [FK_CareTeam_ContactID] 
GO

ALTER TABLE [Registration].[CareTeam]  WITH CHECK ADD  CONSTRAINT [FK_CareTeam_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users]  ([UserID])
GO

ALTER TABLE [Registration].[CareTeam] CHECK CONSTRAINT [FK_CareTeam_UserID] 
GO

ALTER TABLE Registration.CareTeam WITH CHECK ADD CONSTRAINT [FK_CareTeam_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.CareTeam CHECK CONSTRAINT [FK_CareTeam_UserModifedBy]
GO
ALTER TABLE Registration.CareTeam WITH CHECK ADD CONSTRAINT [FK_CareTeam_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.CareTeam CHECK CONSTRAINT [FK_CareTeam_UserCreatedBy]
GO
