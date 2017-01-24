 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[IFSPTeamDiscipline]
-- Author:		Gurpreet Singh
-- Date:		10/26/2015
--
-- Purpose:		ECI IFSPTeamDiscipline  table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Gurpreet Singh	TFS#2983	Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[IFSPTeamDiscipline](
	[TeamDisciplineID] [bigint] IDENTITY(1,1) NOT NULL,
	[IFSPID] [bigint] NULL,
	[UserID] [int] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
PRIMARY KEY CLUSTERED 
(
	[TeamDisciplineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[IFSPTeamDiscipline]  WITH CHECK ADD  CONSTRAINT [FK_IFSPTeamDiscipline_IFSP] FOREIGN KEY([IFSPID])
REFERENCES [ECI].[IFSP] ([IFSPID])
GO

ALTER TABLE [ECI].[IFSPTeamDiscipline] CHECK CONSTRAINT [FK_IFSPTeamDiscipline_IFSP]
GO

ALTER TABLE [ECI].[IFSPTeamDiscipline]  WITH CHECK ADD  CONSTRAINT [FK_IFSPTeamDiscipline_Users] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [ECI].[IFSPTeamDiscipline] CHECK CONSTRAINT [FK_IFSPTeamDiscipline_Users]
GO

ALTER TABLE ECI.IFSPTeamDiscipline WITH CHECK ADD CONSTRAINT [FK_IFSPTeamDiscipline_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.IFSPTeamDiscipline CHECK CONSTRAINT [FK_IFSPTeamDiscipline_UserModifedBy]
GO
ALTER TABLE ECI.IFSPTeamDiscipline WITH CHECK ADD CONSTRAINT [FK_IFSPTeamDiscipline_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.IFSPTeamDiscipline CHECK CONSTRAINT [FK_IFSPTeamDiscipline_UserCreatedBy]
GO
