-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Registration].[ContactProgram]
-- Author:		John Crossen
-- Date:		08/03/2015
--
-- Purpose:		Link Between Contact and Program
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	John Crossen		TFS# 1487 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns

CREATE TABLE [Registration].[ContactProgram](
	[ProgramContactID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ContactID] [BIGINT] NOT NULL,
	[ProgramID] [INT] NOT NULL,
	[ProgramStartDate] [DATE] NULL,
	[ProgramEndDate] [DATE] NULL,
	[ProgramExitDate] [DATE] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactProgram] PRIMARY KEY CLUSTERED 
(
	[ProgramContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ContactProgram]  WITH CHECK ADD  CONSTRAINT [FK_ContactProgram_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ContactProgram] CHECK CONSTRAINT [FK_ContactProgram_Contact]
GO

ALTER TABLE [Registration].[ContactProgram]  WITH CHECK ADD  CONSTRAINT [FK_ContactProgram_Program] FOREIGN KEY([ProgramID])
REFERENCES [Reference].[Program] ([ProgramID])
GO

ALTER TABLE [Registration].[ContactProgram] CHECK CONSTRAINT [FK_ContactProgram_Program]
GO

ALTER TABLE Registration.ContactProgram WITH CHECK ADD CONSTRAINT [FK_ContactProgram_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactProgram CHECK CONSTRAINT [FK_ContactProgram_UserModifedBy]
GO
ALTER TABLE Registration.ContactProgram WITH CHECK ADD CONSTRAINT [FK_ContactProgram_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactProgram CHECK CONSTRAINT [FK_ContactProgram_UserCreatedBy]
GO


