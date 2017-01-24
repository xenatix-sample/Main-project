-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[ClinicalAssessments]
-- Author:		Sumana Sangapu
-- Date:		08/15/2015
--
-- Purpose:		Holds the Assessments details  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2015   Sumana Sangapu  Task# 1601 - Assessment DB Design
-- 11/16/2015	Sumana Sangapu	Added IsSignatureRequired and SignatureTypeID columns
-- 11/20/2015	Arun Choudhary  Changed AssessmetDate data type to datetime
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[ClinicalAssessments](
	[ClinicalAssessmentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[AssessmentDate] DATETIME NOT NULL,
	[UserID] [int] NOT NULL,
	[AssessmentID] [bigint] NOT NULL,
	[ResponseID] [bigint] NULL,
	[AssessmentStatusID] [smallint] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ClinicalAssessments] PRIMARY KEY CLUSTERED 
(
	[ClinicalAssessmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[ClinicalAssessments]  WITH CHECK ADD  CONSTRAINT [FK_ClinicalAssessments_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Clinical].[ClinicalAssessments] CHECK CONSTRAINT [FK_ClinicalAssessments_ContactID]
GO

ALTER TABLE [Clinical].[ClinicalAssessments]  WITH CHECK ADD  CONSTRAINT [FK_ClinicalAssessments_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Clinical].[ClinicalAssessments] CHECK CONSTRAINT [FK_ClinicalAssessments_UserID]
GO

ALTER TABLE [Clinical].[ClinicalAssessments]  WITH CHECK ADD  CONSTRAINT [FK_ClinicalAssessments_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [Clinical].[ClinicalAssessments] CHECK CONSTRAINT [FK_ClinicalAssessments_AssessmentID]
GO

ALTER TABLE [Clinical].[ClinicalAssessments]  WITH CHECK ADD  CONSTRAINT [FK_ClinicalAssessments_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO

ALTER TABLE [Clinical].[ClinicalAssessments] CHECK CONSTRAINT [FK_ClinicalAssessments_ResponseID]
GO

ALTER TABLE [Clinical].[ClinicalAssessments]  WITH CHECK ADD  CONSTRAINT [FK_ClinicalAssessments_AssessmentStatusID] FOREIGN KEY([AssessmentStatusID])
REFERENCES [Core].[AssessmentStatus] ([AssessmentStatusID])
GO

ALTER TABLE [Clinical].[ClinicalAssessments] CHECK CONSTRAINT [FK_ClinicalAssessments_AssessmentStatusID]
GO

ALTER TABLE Clinical.ClinicalAssessments WITH CHECK ADD CONSTRAINT [FK_ClinicalAssessments_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ClinicalAssessments CHECK CONSTRAINT [FK_ClinicalAssessments_UserModifedBy]
GO
ALTER TABLE Clinical.ClinicalAssessments WITH CHECK ADD CONSTRAINT [FK_ClinicalAssessments_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ClinicalAssessments CHECK CONSTRAINT [FK_ClinicalAssessments_UserCreatedBy]
GO
