-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[SurgicalHistory]
-- Author:		John Crossen
-- Date:		11/30/2015
--
-- Purpose:		Surgical History Header Table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/30/2015	John Crossen	TFS# 3899 - Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[SurgicalHistory](
	[SurgicalHistoryID] [BIGINT] NOT NULL IDENTITY(1,1),
	[EncounterID] [BIGINT] NULL,
	[ContactID] [BIGINT] NOT NULL,
	[TakenBy] [INT] NOT NULL,
	[TakenTime] [DATETIME] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SurgicalHistory] PRIMARY KEY CLUSTERED 
(
	[SurgicalHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Clinical.SurgicalHistory WITH CHECK ADD CONSTRAINT [FK_SurgicalHistory_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SurgicalHistory CHECK CONSTRAINT [FK_SurgicalHistory_UserModifedBy]
GO
ALTER TABLE Clinical.SurgicalHistory WITH CHECK ADD CONSTRAINT [FK_SurgicalHistory_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SurgicalHistory CHECK CONSTRAINT [FK_SurgicalHistory_UserCreatedBy]
GO
