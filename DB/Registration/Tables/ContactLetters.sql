-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Registration].[ContactLetters]
-- Author:		Deepak Kumar
-- Date:		06/08/2016
--
-- Purpose:		Holds Contact letter logging info
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Deepak Kumar	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactLetters](
	[ContactLettersID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[AssessmentID] [bigint] NULL,
	[ResponseID] [bigint] NULL,
	[SentDate] [datetime] NULL,
	[UserID] [int] NULL,
	[LetterOutcomeID] [int] NULL,
	[Comments] [nvarchar](4000) NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ContactLetters] PRIMARY KEY CLUSTERED 
(
	[ContactLettersID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
	
ALTER TABLE [Registration].[ContactLetters] ADD  CONSTRAINT [DF_ContactLetters_SentDate]  DEFAULT (getutcdate()) FOR [SentDate]
GO	

ALTER TABLE [Registration].[ContactLetters] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [Registration].[ContactLetters] ADD  DEFAULT (getutcdate()) FOR [ModifiedOn]
GO

ALTER TABLE [Registration].[ContactLetters] ADD  DEFAULT ((1)) FOR [CreatedBy]
GO

ALTER TABLE [Registration].[ContactLetters] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [Registration].[ContactLetters] ADD  DEFAULT (getutcdate()) FOR [SystemCreatedOn]
GO

ALTER TABLE [Registration].[ContactLetters] ADD  DEFAULT (getutcdate()) FOR [SystemModifiedOn]
GO
	
ALTER TABLE [Registration].[ContactLetters]  WITH CHECK ADD  CONSTRAINT [FK_ContactLetters_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [Registration].[ContactLetters] CHECK CONSTRAINT [FK_ContactLetters_AssessmentID]
GO

ALTER TABLE [Registration].[ContactLetters]  WITH CHECK ADD  CONSTRAINT [FK_ContactLetters_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ContactLetters] CHECK CONSTRAINT [FK_ContactLetters_ContactID]
GO

ALTER TABLE [Registration].[ContactLetters]  WITH CHECK ADD  CONSTRAINT [FK_ContactLetters_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO

ALTER TABLE [Registration].[ContactLetters] CHECK CONSTRAINT [FK_ContactLetters_ResponseID]
GO

ALTER TABLE [Registration].[ContactLetters]  WITH CHECK ADD  CONSTRAINT [FK_ContactLetters_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ContactLetters] CHECK CONSTRAINT [FK_ContactLetters_UserCreatedBy]
GO

ALTER TABLE [Registration].[ContactLetters]  WITH CHECK ADD  CONSTRAINT [FK_ContactLetters_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ContactLetters] CHECK CONSTRAINT [FK_ContactLetters_UserID]
GO

ALTER TABLE [Registration].[ContactLetters]  WITH CHECK ADD  CONSTRAINT [FK_ContactLetters_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ContactLetters] CHECK CONSTRAINT [FK_ContactLetters_UserModifedBy]
GO

ALTER TABLE [Registration].[ContactLetters]  WITH CHECK ADD  CONSTRAINT [FK_ContactLetters_LetterOutcomeID] FOREIGN KEY([LetterOutcomeID])
REFERENCES [Reference].[LetterOutcome] ([LetterOutcomeID])
GO

ALTER TABLE [Registration].[ContactLetters] CHECK CONSTRAINT [FK_ContactLetters_LetterOutcomeID]
GO	
	
	
	
	
	
	
	
	
	
	
	
	







