-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [[Registration]].[[NoteHeader]]
-- Author:		John Crossen
-- Date:		1/4/2016
--
-- Purpose:		Header Table for Notes
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/4/2016	John Crossen		TFS:4963	Initial Creation
-- 1/7/2016		Scott Martin		Renamed NoteDate to TakenTime, Added TakenBy, Added FK to new Reference.NoteType Table
-- 01/09/2016	Gurpreet Singh	Updated fields NoteTypeID, TakenBy, TakenTime to accept NULL
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 06/06/2016	Scott Martin	Changed NoteTypeID to NOT NULL, TakenBy to NOT NULL and added a foreign key to TakenBy
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[NoteHeader](
	[NoteHeaderID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ContactID] BIGINT NOT NULL,
	[NoteTypeID] [INT] NOT NULL,
	[TakenBy] [INT] NOT NULL,
	[TakenTime] [DATETIME] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_NoteHeader] PRIMARY KEY CLUSTERED 
(
	[NoteHeaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[NoteHeader]  WITH CHECK ADD  CONSTRAINT [FK_NoteHeader_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[NoteHeader] CHECK CONSTRAINT [FK_NoteHeader_ContactID]
GO

ALTER TABLE [Registration].[NoteHeader]  WITH CHECK ADD  CONSTRAINT [FK_NoteHeader_NoteTypeID] FOREIGN KEY([NoteTypeID])
REFERENCES [Reference].[NoteType] ([NoteTypeID])
GO

ALTER TABLE [Registration].[NoteHeader] CHECK CONSTRAINT [FK_NoteHeader_NoteTypeID]
GO

ALTER TABLE Registration.NoteHeader WITH CHECK ADD CONSTRAINT [FK_NoteHeader_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.NoteHeader CHECK CONSTRAINT [FK_NoteHeader_UserModifedBy]
GO
ALTER TABLE Registration.NoteHeader WITH CHECK ADD CONSTRAINT [FK_NoteHeader_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.NoteHeader CHECK CONSTRAINT [FK_NoteHeader_UserCreatedBy]
GO
ALTER TABLE Registration.NoteHeader WITH CHECK ADD CONSTRAINT [FK_NoteHeader_TakenBy] FOREIGN KEY ([TakenBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.NoteHeader CHECK CONSTRAINT [FK_NoteHeader_TakenBy]
GO

