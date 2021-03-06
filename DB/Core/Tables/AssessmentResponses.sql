-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AssessmentResponses]
-- Author:		Sumana Sangapu
-- Date:		08/15/2015
--
-- Purpose:		Holds the Responses of the users to the assessment questionnaire
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2015   Sumana Sangapu  Task# 1601 - Assessment DB Design
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AssessmentResponses](
	[ResponseID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NULL,
	[AssessmentID] [bigint] NULL,
	[EnterDate] [datetime] NULL,
	[EnterDateINT] [int] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentResponse_ResponseID] PRIMARY KEY CLUSTERED 
(
	[ResponseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Core].[AssessmentResponses]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentResponses_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO
ALTER TABLE [Core].[AssessmentResponses] CHECK CONSTRAINT [FK_AssessmentResponses_AssessmentID]
GO

ALTER TABLE [Core].[AssessmentResponses]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentResponses_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE [Core].[AssessmentResponses]  CHECK CONSTRAINT [FK_AssessmentResponses_ContactID]
GO

ALTER TABLE Core.AssessmentResponses WITH CHECK ADD CONSTRAINT [FK_AssessmentResponses_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentResponses CHECK CONSTRAINT [FK_AssessmentResponses_UserModifedBy]
GO
ALTER TABLE Core.AssessmentResponses WITH CHECK ADD CONSTRAINT [FK_AssessmentResponses_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentResponses CHECK CONSTRAINT [FK_AssessmentResponses_UserCreatedBy]
GO
