-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[ContactAllergyDetails]
-- Author:		Justin Spalti
-- Date:		11/20/2015
--
-- Purpose:		Link Contact to Allergies and Symptoms
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015   Justin Spalti - Initial creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[ContactAllergyDetailSymptoms](
	[ContactAllergyDetailSymptomID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactAllergyDetailID] [bigint] NOT NULL,
	[AllergySymptomID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactAllergyDetailSymptoms] PRIMARY KEY CLUSTERED 
(
	[ContactAllergyDetailSymptomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Index [IX_ContactAllergyDetailID_AllergySymptomID]    Script Date: 11/20/2015 11:05:11 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ContactAllergyDetailID_AllergySymptomID] ON [Clinical].[ContactAllergyDetailSymptoms]
(
	[ContactAllergyDetailSymptomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [Clinical].[ContactAllergyDetailSymptoms]  WITH CHECK ADD  CONSTRAINT [FK_AllergySymptom_ContactAllergyDetailSymptoms] FOREIGN KEY([AllergySymptomID])
REFERENCES [Clinical].[AllergySymptom] ([AllergySymptomID])
GO

ALTER TABLE [Clinical].[ContactAllergyDetailSymptoms] CHECK CONSTRAINT [FK_AllergySymptom_ContactAllergyDetailSymptoms]
GO

ALTER TABLE [Clinical].[ContactAllergyDetailSymptoms]  WITH CHECK ADD  CONSTRAINT [FK_ContactAllergyDetailSymptom_ContactAllergyDetail] FOREIGN KEY([ContactAllergyDetailID])
REFERENCES [Clinical].[ContactAllergyDetail] ([ContactAllergyDetailID])
GO

ALTER TABLE [Clinical].[ContactAllergyDetailSymptoms] CHECK CONSTRAINT [FK_ContactAllergyDetailSymptom_ContactAllergyDetail]
GO

ALTER TABLE [Clinical].[ContactAllergyDetailSymptoms]  WITH CHECK ADD  CONSTRAINT [FK_Users_ContactAllergyDetailSymptoms] FOREIGN KEY([ContactAllergyDetailSymptomID])
REFERENCES [Clinical].[ContactAllergyDetailSymptoms] ([ContactAllergyDetailSymptomID])
GO

ALTER TABLE [Clinical].[ContactAllergyDetailSymptoms] CHECK CONSTRAINT [FK_Users_ContactAllergyDetailSymptoms]
GO

ALTER TABLE Clinical.ContactAllergyDetailSymptoms WITH CHECK ADD CONSTRAINT [FK_ContactAllergyDetailSymptoms_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ContactAllergyDetailSymptoms CHECK CONSTRAINT [FK_ContactAllergyDetailSymptoms_UserModifedBy]
GO
ALTER TABLE Clinical.ContactAllergyDetailSymptoms WITH CHECK ADD CONSTRAINT [FK_ContactAllergyDetailSymptoms_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ContactAllergyDetailSymptoms CHECK CONSTRAINT [FK_ContactAllergyDetailSymptoms_UserCreatedBy]
GO


