-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Registration].[ContactForms]
-- Author:		Scott Martin
-- Date:		06/10/2016
--
-- Purpose:		Holds Contact form logging info
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/10/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactForms](
	[ContactFormsID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[AssessmentID] [bigint] NULL,
	[ResponseID] [bigint] NULL,
	[UserID] [int] NULL,
	[DocumentStatusID] [smallint] NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ContactForms] PRIMARY KEY CLUSTERED 
(
	[ContactFormsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ContactForms] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [Registration].[ContactForms] ADD  DEFAULT (getutcdate()) FOR [ModifiedOn]
GO

ALTER TABLE [Registration].[ContactForms] ADD  DEFAULT ((1)) FOR [CreatedBy]
GO

ALTER TABLE [Registration].[ContactForms] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [Registration].[ContactForms] ADD  DEFAULT (getutcdate()) FOR [SystemCreatedOn]
GO

ALTER TABLE [Registration].[ContactForms] ADD  DEFAULT (getutcdate()) FOR [SystemModifiedOn]
GO
	
ALTER TABLE [Registration].[ContactForms]  WITH CHECK ADD  CONSTRAINT [FK_ContactForms_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [Registration].[ContactForms] CHECK CONSTRAINT [FK_ContactForms_AssessmentID]
GO

ALTER TABLE [Registration].[ContactForms]  WITH CHECK ADD  CONSTRAINT [FK_ContactForms_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ContactForms] CHECK CONSTRAINT [FK_ContactForms_ContactID]
GO

ALTER TABLE [Registration].[ContactForms]  WITH CHECK ADD  CONSTRAINT [FK_ContactForms_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO

ALTER TABLE [Registration].[ContactForms] CHECK CONSTRAINT [FK_ContactForms_ResponseID]
GO

ALTER TABLE [Registration].[ContactForms]  WITH CHECK ADD  CONSTRAINT [FK_ContactForms_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ContactForms] CHECK CONSTRAINT [FK_ContactForms_UserCreatedBy]
GO

ALTER TABLE [Registration].[ContactForms]  WITH CHECK ADD  CONSTRAINT [FK_ContactForms_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ContactForms] CHECK CONSTRAINT [FK_ContactForms_UserID]
GO

ALTER TABLE [Registration].[ContactForms]  WITH CHECK ADD  CONSTRAINT [FK_ContactForms_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ContactForms] CHECK CONSTRAINT [FK_ContactForms_UserModifedBy]
GO

ALTER TABLE [Registration].[ContactForms]  WITH CHECK ADD  CONSTRAINT [FK_ContactForms_DocumentStatusID] FOREIGN KEY([DocumentStatusID])
REFERENCES [Reference].[DocumentStatus] ([DocumentStatusID])
GO

ALTER TABLE [Registration].[ContactForms] CHECK CONSTRAINT [FK_ContactForms_DocumentStatusID]
GO	
	
	
	
	
	
	
	
	
	
	
	
	







