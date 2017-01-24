-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[Encounter]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Encounter Table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/27/2015	John Crossen	TFS# 2891 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE Clinical.[Encounter](
	[EncounterID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[EncounterStart] [datetime] NOT NULL,
	[EncounterEnd] [datetime] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Encounter] PRIMARY KEY CLUSTERED 
(
	[EncounterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Clinical.[Encounter] ADD  CONSTRAINT [DF_Encounter_EncounterStart]  DEFAULT (getutcdate()) FOR [EncounterStart]
GO

ALTER TABLE Clinical.[Encounter]  WITH CHECK ADD  CONSTRAINT [FK_Encounter_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE Clinical.[Encounter] CHECK CONSTRAINT [FK_Encounter_Contact]
GO

ALTER TABLE Clinical.Encounter WITH CHECK ADD CONSTRAINT [FK_Encounter_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Encounter CHECK CONSTRAINT [FK_Encounter_UserModifedBy]
GO
ALTER TABLE Clinical.Encounter WITH CHECK ADD CONSTRAINT [FK_Encounter_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.Encounter CHECK CONSTRAINT [FK_Encounter_UserCreatedBy]
GO
