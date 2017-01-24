-----------------------------------------------------------------------------------------------------------------------
-- Table:		Clinical.[ChiefComplaint]
-- Author:		John Crossen
-- Date:		10/30/2015
--
-- Purpose:		Store Chief Complaint Data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/30/2015	John Crossen    TFS#2885		Modification .
-- 11/11/2015   John Crossen    TFS#3537        Move to clinical schema
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- ---------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[ChiefComplaint](
	[ChiefComplaintID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChiefComplaint] [nvarchar](1000) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	EncounterID BIGINT,
	TakenBy INT NOT NULL,
	TakenTime DATETIME NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ChiefComplaint] PRIMARY KEY CLUSTERED 
(
	[ChiefComplaintID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[ChiefComplaint]  WITH CHECK ADD  CONSTRAINT [FK_ChiefComplaint_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Clinical].[ChiefComplaint] CHECK CONSTRAINT [FK_ChiefComplaint_Contact]
GO

ALTER TABLE [Clinical].[ChiefComplaint]  WITH CHECK ADD  CONSTRAINT [FK_ChiefComplaint_Users] FOREIGN KEY([TakenBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Clinical].[ChiefComplaint] CHECK CONSTRAINT [FK_ChiefComplaint_Users]
GO

ALTER TABLE [Clinical].[ChiefComplaint]  WITH CHECK ADD  CONSTRAINT [FK_ChiefComplaint_EncounterID] FOREIGN KEY([EncounterID])
REFERENCES [Clinical].[Encounter] ([EncounterID])
GO

ALTER TABLE [Clinical].[ChiefComplaint] CHECK CONSTRAINT [FK_ChiefComplaint_EncounterID]
GO

ALTER TABLE Clinical.ChiefComplaint WITH CHECK ADD CONSTRAINT [FK_ChiefComplaint_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ChiefComplaint CHECK CONSTRAINT [FK_ChiefComplaint_UserModifedBy]
GO
ALTER TABLE Clinical.ChiefComplaint WITH CHECK ADD CONSTRAINT [FK_ChiefComplaint_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ChiefComplaint CHECK CONSTRAINT [FK_ChiefComplaint_UserCreatedBy]
GO
