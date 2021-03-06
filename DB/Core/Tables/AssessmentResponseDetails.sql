-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AssessmentResponseDetails]
-- Author:		Sumana Sangapu
-- Date:		08/15/2015
--
-- Purpose:		Holds the ResponseDetails of the users to the assessment questionnaire
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2015   Sumana Sangapu  Task# 1601 - Assessment DB Design
-- 10/08/2015   Rajiv Ranjan - Updated foreign key Reference 'FK_ResponseDetails_AssessmentSectionID'
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AssessmentResponseDetails](
	[ResponseDetailsID] [bigint] NOT NULL IDENTITY(1,1),
	[ResponseID] [bigint] NULL,
	[QuestionID] [bigint] NULL,
	[AssessmentSectionID] [bigint] NULL,
	[OptionsID] [bigint] NULL,
	[ResponseText] nvarchar(max) NULL,
	[Rating] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentResponseDetails_ResponseDetailsID] PRIMARY KEY CLUSTERED 
(
	[ResponseDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Core].[AssessmentResponseDetails]  WITH CHECK ADD  CONSTRAINT [FK_ResponseDetails_OptionID] FOREIGN KEY([OptionsID])
REFERENCES [Core].[AssessmentOptions] ([OptionsID])
GO
ALTER TABLE [Core].[AssessmentResponseDetails] CHECK CONSTRAINT [FK_ResponseDetails_OptionID]
GO
ALTER TABLE [Core].[AssessmentResponseDetails]  WITH CHECK ADD  CONSTRAINT [FK_ResponseDetails_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO
ALTER TABLE [Core].[AssessmentResponseDetails] CHECK CONSTRAINT [FK_ResponseDetails_ResponseID]
GO
ALTER TABLE [Core].[AssessmentResponseDetails]  WITH CHECK ADD  CONSTRAINT [FK_ResponseDetails_AssessmentSectionID] FOREIGN KEY([AssessmentSectionID])
REFERENCES [Core].[AssessmentSections]([AssessmentSectionID])
GO
ALTER TABLE [Core].[AssessmentResponseDetails] CHECK CONSTRAINT [FK_ResponseDetails_AssessmentSectionID]
GO

CREATE INDEX [IX_AssessmentResponseDetails_ReponseID_AssessmentSection_IsActive] ON [Core].[AssessmentResponseDetails] ([ResponseID], [AssessmentSectionID], [IsActive]) INCLUDE ([ResponseDetailsID], [QuestionID], [OptionsID], [ResponseText], [Rating])
GO

ALTER TABLE Core.AssessmentResponseDetails WITH CHECK ADD CONSTRAINT [FK_AssessmentResponseDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentResponseDetails CHECK CONSTRAINT [FK_AssessmentResponseDetails_UserModifedBy]
GO
ALTER TABLE Core.AssessmentResponseDetails WITH CHECK ADD CONSTRAINT [FK_AssessmentResponseDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentResponseDetails CHECK CONSTRAINT [FK_AssessmentResponseDetails_UserCreatedBy]
GO
