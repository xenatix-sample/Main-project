-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[ContactAllergyDetails]
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
-- 11/20/2015   Justin Spalti - Removed References to allergy symptoms
-- 11/30/2015   Justin Spalti - Removed the foreign key relationship to the allergy table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[ContactAllergyDetail](
	[ContactAllergyDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactAllergyID] [bigint] NOT NULL,
	[AllergyID] [int] NOT NULL,
	[SeverityID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ContactAllergyDetail] PRIMARY KEY CLUSTERED 
(
	[ContactAllergyDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[ContactAllergyDetail]  WITH CHECK ADD  CONSTRAINT [FK_ContactAllergyDetail_AllergySeverity] FOREIGN KEY([SeverityID])
REFERENCES [Clinical].[AllergySeverity] ([AllergySeverityID])
GO

ALTER TABLE [Clinical].[ContactAllergyDetail] CHECK CONSTRAINT [FK_ContactAllergyDetail_AllergySeverity]
GO

ALTER TABLE [Clinical].[ContactAllergyDetail]  WITH CHECK ADD  CONSTRAINT [FK_ContactAllergyDetail_ContactAllergy] FOREIGN KEY([ContactAllergyID])
REFERENCES [Clinical].[ContactAllergy] ([ContactAllergyID])
GO

ALTER TABLE [Clinical].[ContactAllergyDetail] CHECK CONSTRAINT [FK_ContactAllergyDetail_ContactAllergy]
GO

ALTER TABLE Clinical.ContactAllergyDetail WITH CHECK ADD CONSTRAINT [FK_ContactAllergyDetail_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ContactAllergyDetail CHECK CONSTRAINT [FK_ContactAllergyDetail_UserModifedBy]
GO
ALTER TABLE Clinical.ContactAllergyDetail WITH CHECK ADD CONSTRAINT [FK_ContactAllergyDetail_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ContactAllergyDetail CHECK CONSTRAINT [FK_ContactAllergyDetail_UserCreatedBy]
GO
