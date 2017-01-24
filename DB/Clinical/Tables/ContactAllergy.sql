-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ContactAllergy]
-- Author:		John Crossen
-- Date:		11/11/2015
--
-- Purpose:		Link Contact to Allergies and Symptoms
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/11/2015	John Crossen	TFS# 3546 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[ContactAllergy](
	[ContactAllergyID] [bigint] IDENTITY(1,1) NOT NULL,
	[EncounterID] [bigint] NULL,
	[ContactID] [bigint] NOT NULL,
	[AllergyTypeID] SMALLINT NOT NULL,
	[NoKnownAllergy] [bit] NOT NULL,
	[TakenBy] [int] NOT NULL,
	[TakenTime] [datetime] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactAllergy_1] PRIMARY KEY CLUSTERED 
(
	[ContactAllergyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[ContactAllergy] ADD  CONSTRAINT [DF_ContactAllergy_NoKnownAllergy]  DEFAULT ((0)) FOR [NoKnownAllergy]
GO

ALTER TABLE [Clinical].[ContactAllergy] ADD  CONSTRAINT [DF_ContactAllergy_TakenTime]  DEFAULT (getutcdate()) FOR [TakenTime]
GO

ALTER TABLE [Clinical].[ContactAllergy]  WITH CHECK ADD  CONSTRAINT [FK_ContactAllergy_Encounter] FOREIGN KEY([EncounterID])
REFERENCES [Clinical].[Encounter] ([EncounterID])
GO

ALTER TABLE [Clinical].[ContactAllergy] CHECK CONSTRAINT [FK_ContactAllergy_Encounter]
GO

ALTER TABLE [Clinical].[ContactAllergy]  WITH CHECK ADD  CONSTRAINT [FK_ContactAllergy_Users] FOREIGN KEY([TakenBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Clinical].[ContactAllergy] CHECK CONSTRAINT [FK_ContactAllergy_Users]
GO

ALTER TABLE [Clinical].[ContactAllergy]  WITH CHECK ADD  CONSTRAINT [FK_ContactAllergy_typeID] FOREIGN KEY([AllergyTypeID])
REFERENCES [Clinical].[AllergyType] ([AllergyTypeID])
GO

ALTER TABLE [Clinical].[ContactAllergy] CHECK CONSTRAINT [FK_ContactAllergy_typeID]
GO

ALTER TABLE Clinical.ContactAllergy WITH CHECK ADD CONSTRAINT [FK_ContactAllergy_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ContactAllergy CHECK CONSTRAINT [FK_ContactAllergy_UserModifedBy]
GO
ALTER TABLE Clinical.ContactAllergy WITH CHECK ADD CONSTRAINT [FK_ContactAllergy_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ContactAllergy CHECK CONSTRAINT [FK_ContactAllergy_UserCreatedBy]
GO
