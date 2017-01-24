-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[CallCenterAssessmentResponse]
-- Author:		Rajiv Ranjan
-- Date:		06/15/2016
--
-- Purpose:		Mapping b/w CallCenterHeadreID & ResponseID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/15/2016	Rajiv Ranjan	Initial creation.

CREATE TABLE [CallCenter].[CallCenterAssessmentResponse](
	[CallCenterAssessmentResponseID] [bigint] IDENTITY(1,1) NOT NULL,
	[CallCenterHeaderID] [bigint] NULL,
	[AssessmentID] [bigint] NULL,
	[ResponseID] [bigint] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_CallCenterAssessmentResponse] PRIMARY KEY CLUSTERED 
(
	[CallCenterAssessmentResponseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterAssessmentResponse_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse] CHECK CONSTRAINT [FK_CallCenterAssessmentResponse_UserCreatedBy]
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterAssessmentResponse_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse] CHECK CONSTRAINT [FK_CallCenterAssessmentResponse_UserModifedBy]
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterAssessmentResponse_CallCenterHeaderID] FOREIGN KEY([CallCenterHeaderID])
REFERENCES [CallCenter].[CallCenterHeader] ([CallCenterHeaderID])
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse] CHECK CONSTRAINT [FK_CallCenterAssessmentResponse_CallCenterHeaderID]
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterAssessmentResponse_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse] CHECK CONSTRAINT [FK_CallCenterAssessmentResponse_AssessmentID]
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse]  WITH CHECK ADD  CONSTRAINT [FK_CallCenterAssessmentResponse_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO

ALTER TABLE [CallCenter].[CallCenterAssessmentResponse] CHECK CONSTRAINT [FK_CallCenterAssessmentResponse_ResponseID]
GO