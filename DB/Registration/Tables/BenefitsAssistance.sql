-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Registration].[BenefitsAssistance]
-- Author:		Scott Martin
-- Date:		05/19/2016
--
-- Purpose:		Holds Benefits Assistance progress note logging info
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Scott Martin	Initial Creation
-- 06/23/2016	Scott Martin	Added DocumentStatus foreign key and changed datatype to smallint
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[BenefitsAssistance]
(	
	BenefitsAssistanceID bigint IDENTITY (1,1) NOT NULL,
	ContactID bigint NOT NULL,
	DateEntered DATETIME NULL,
	UserID INT,
	AssessmentID bigint NULL,
	ResponseID bigint NULL,
	DocumentStatusID SMALLINT,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_BenefitsAssistance] PRIMARY KEY CLUSTERED 
(
	[BenefitsAssistanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [Registration].[BenefitsAssistance] ADD  CONSTRAINT [DF_BenefitsAssistance_DateEntered]  DEFAULT (getutcdate()) FOR [DateEntered]
GO
ALTER TABLE [Registration].[BenefitsAssistance]  WITH CHECK ADD  CONSTRAINT [FK_BenefitsAssistance_ContactID] FOREIGN KEY([ContactID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE [Registration].[BenefitsAssistance] CHECK CONSTRAINT [FK_BenefitsAssistance_ContactID]
GO
ALTER TABLE [Registration].[BenefitsAssistance]  WITH CHECK ADD  CONSTRAINT [FK_BenefitsAssistance_AssessmentID] FOREIGN KEY([AssessmentID]) REFERENCES [Core].[Assessments] ([AssessmentID])
GO
ALTER TABLE [Registration].[BenefitsAssistance] CHECK CONSTRAINT [FK_BenefitsAssistance_AssessmentID]
GO
ALTER TABLE [Registration].[BenefitsAssistance]  WITH CHECK ADD  CONSTRAINT [FK_BenefitsAssistance_ResponseID] FOREIGN KEY([ResponseID]) REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO
ALTER TABLE [Registration].[BenefitsAssistance] CHECK CONSTRAINT [FK_BenefitsAssistance_ResponseID]
GO
ALTER TABLE Registration.BenefitsAssistance WITH CHECK ADD CONSTRAINT [FK_BenefitsAssistance_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.BenefitsAssistance CHECK CONSTRAINT [FK_BenefitsAssistance_UserModifedBy]
GO
ALTER TABLE Registration.BenefitsAssistance WITH CHECK ADD CONSTRAINT [FK_BenefitsAssistance_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.BenefitsAssistance CHECK CONSTRAINT [FK_BenefitsAssistance_UserCreatedBy]
GO
ALTER TABLE Registration.BenefitsAssistance WITH CHECK ADD CONSTRAINT [FK_BenefitsAssistance_UserID] FOREIGN KEY([UserID]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.BenefitsAssistance CHECK CONSTRAINT [FK_BenefitsAssistance_UserID]
GO
ALTER TABLE [Registration].[BenefitsAssistance]  WITH CHECK ADD  CONSTRAINT [FK_BenefitsAssistance_DocumentStatusID] FOREIGN KEY([DocumentStatusID]) REFERENCES [Reference].[DocumentStatus] ([DocumentStatusID])
GO
ALTER TABLE [Registration].[BenefitsAssistance] CHECK CONSTRAINT [FK_BenefitsAssistance_DocumentStatusID]
GO	