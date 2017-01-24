 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[EligibilityTeamDiscipline]
-- Author:		Sumana Sangapu
-- Date:		10/13/2015
--
-- Purpose:		ECI EligibilityTeamDiscipline  table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Sumana Sangapu	TFS:2700		Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[EligibilityTeamDiscipline](
	[TeamDisciplineID] [bigint] IDENTITY (1,1) NOT NULL,
	[EligibilityID] [bigint] NOT NULL,
	[UserID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_EligibilityTeamDiscipline_TeamDisciplineID] PRIMARY KEY CLUSTERED 
(
	[TeamDisciplineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] 
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[EligibilityTeamDiscipline]  WITH CHECK ADD  CONSTRAINT [FK_EligibilityTeamDiscipline_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [ECI].[EligibilityTeamDiscipline] CHECK CONSTRAINT [FK_EligibilityTeamDiscipline_UserID]
GO

ALTER TABLE ECI.EligibilityTeamDiscipline WITH CHECK ADD CONSTRAINT [FK_EligibilityTeamDiscipline_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.EligibilityTeamDiscipline CHECK CONSTRAINT [FK_EligibilityTeamDiscipline_UserModifedBy]
GO
ALTER TABLE ECI.EligibilityTeamDiscipline WITH CHECK ADD CONSTRAINT [FK_EligibilityTeamDiscipline_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.EligibilityTeamDiscipline CHECK CONSTRAINT [FK_EligibilityTeamDiscipline_UserCreatedBy]
GO
