-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Assessments]
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
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 04/08/2016	Scott Martin	Added column ExpirationAssessmentID for assessments that require it
-- 04/26/2016	Kyle Campbell	Add "UseSnapshot" field to indicate if assessment needs to be saved and loaded as a snapshot (Consents)
-- 04/30/2016	Kyle Campbell	TFS #10111	Add Attributes field to Assessments table
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Assessments](
	[AssessmentID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Abbreviation] [nvarchar] (20) NULL,
	[VersionID] [nvarchar](20) NULL,
	[ProgramID] [INT] NULL,
	[FrequencyID] [int] NULL,
	[ImageID] [bigint] NULL,
	[IsSignatureRequired] [bit] NULL,
	[SignatureTypeID] [int] NULL,
	[ExpirationAssessmentID] BIGINT NULL,
	[UseSnapshot] [bit] NULL,
	[Attributes] [nvarchar](max) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_Assessments] PRIMARY KEY CLUSTERED 
(
	[AssessmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[Assessments]  WITH CHECK ADD  CONSTRAINT [FK_Assessments_AssessmentFrequency] FOREIGN KEY([FrequencyID])
REFERENCES [Reference].[AssessmentFrequency] ([FrequencyID])
GO
ALTER TABLE [Core].[Assessments] CHECK CONSTRAINT [FK_Assessments_AssessmentFrequency]
GO

ALTER TABLE [Core].[Assessments]  WITH CHECK ADD  CONSTRAINT [FK_Assessments_Program] FOREIGN KEY([ProgramID])
REFERENCES [Reference].[Program] ([ProgramID])
GO
ALTER TABLE [Core].[Assessments] CHECK CONSTRAINT [FK_Assessments_Program]
GO

ALTER TABLE [Core].[Assessments]  WITH CHECK ADD  CONSTRAINT [FK_Assessments_Images] FOREIGN KEY([ImageID])
REFERENCES [Core].[Images] ([ImageID])
GO
ALTER TABLE [Core].[Assessments] CHECK CONSTRAINT [FK_Assessments_Images]
GO

ALTER TABLE [Core].[Assessments]  WITH CHECK ADD  CONSTRAINT [FK_Assessments_SignatureTypeID] FOREIGN KEY([SignatureTypeID])
REFERENCES [ESignature].[SignatureTypes] ([SignatureTypeID])
GO
ALTER TABLE [Core].[Assessments] CHECK CONSTRAINT [FK_Assessments_SignatureTypeID]
GO

ALTER TABLE Core.Assessments WITH CHECK ADD CONSTRAINT [FK_Assessments_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Assessments CHECK CONSTRAINT [FK_Assessments_UserModifedBy]
GO
ALTER TABLE Core.Assessments WITH CHECK ADD CONSTRAINT [FK_Assessments_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Assessments CHECK CONSTRAINT [FK_Assessments_UserCreatedBy]
GO
