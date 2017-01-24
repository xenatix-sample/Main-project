-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[[[MedicalHistory]]]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		MedicalHistory Header table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3664 - Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[MedicalHistory](
	[MedicalHistoryID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	EncounterID bigint NULL,
	[TakenTime] [datetime] NOT NULL,
	[TakenBy] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_MedicalHistory] PRIMARY KEY CLUSTERED 
(
	[MedicalHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[MedicalHistory] ADD  CONSTRAINT [DF_MedicalHistory_TakenTime]  DEFAULT (getutcdate()) FOR [TakenTime]
GO

ALTER TABLE [Clinical].[MedicalHistory]  WITH CHECK ADD  CONSTRAINT [FK_MedicalHistory_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Clinical].[MedicalHistory] CHECK CONSTRAINT [FK_MedicalHistory_Contact]
GO

ALTER TABLE [Clinical].[MedicalHistory]  WITH CHECK ADD  CONSTRAINT [FK_MedicalHistory_Users] FOREIGN KEY([TakenBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Clinical].[MedicalHistory] CHECK CONSTRAINT [FK_MedicalHistory_Users]
GO

ALTER TABLE Clinical.MedicalHistory WITH CHECK ADD CONSTRAINT [FK_MedicalHistory_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalHistory CHECK CONSTRAINT [FK_MedicalHistory_UserModifedBy]
GO
ALTER TABLE Clinical.MedicalHistory WITH CHECK ADD CONSTRAINT [FK_MedicalHistory_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.MedicalHistory CHECK CONSTRAINT [FK_MedicalHistory_UserCreatedBy]
GO
