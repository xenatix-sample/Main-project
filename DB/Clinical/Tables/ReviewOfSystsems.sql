-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Clinical].[ReviewOfSystems]
-- Author:		Sumana Sangapu
-- Date:		11/13/2015
--
-- Purpose:		Holds Review Of Systems information
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Sumana Sangapu	Initial Creation
-- 11/21/2015	Rajiv Ranjan	Added IsReviewChanged
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[ReviewOfSystems]
(	
	RoSID			bigint IDENTITY (1,1) NOT NULL,
	ContactID		bigint NOT NULL,
	DateEntered		DATETIME NOT NULL,
	ReviewdBy		int NOT NULL,
	AssessmentID	bigint NULL,
	ResponseID		bigint NULL,
	IsReviewChanged bit NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_ReviewOfSystems] PRIMARY KEY CLUSTERED 
(
	[RoSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [Clinical].[ReviewOfSystems] ADD  CONSTRAINT [DF_ReviewOfSystems_DateEntered]  DEFAULT (getutcdate()) FOR [DateEntered]
GO

ALTER TABLE [Clinical].[ReviewOfSystems]  WITH CHECK ADD  CONSTRAINT [FK_ReviewOfSystems_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Clinical].[ReviewOfSystems] CHECK CONSTRAINT [FK_ReviewOfSystems_ContactID]
GO

ALTER TABLE [Clinical].[ReviewOfSystems]  WITH CHECK ADD  CONSTRAINT [FK_ReviewOfSystems_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [Clinical].[ReviewOfSystems] CHECK CONSTRAINT [FK_ReviewOfSystems_AssessmentID]
GO

ALTER TABLE [Clinical].[ReviewOfSystems]  WITH CHECK ADD  CONSTRAINT [FK_ReviewOfSystems_ResponseID] FOREIGN KEY([ResponseID])
REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO

ALTER TABLE [Clinical].[ReviewOfSystems] CHECK CONSTRAINT [FK_ReviewOfSystems_ResponseID]
GO

ALTER TABLE Clinical.ReviewOfSystems WITH CHECK ADD CONSTRAINT [FK_ReviewOfSystems_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ReviewOfSystems CHECK CONSTRAINT [FK_ReviewOfSystems_UserModifedBy]
GO
ALTER TABLE Clinical.ReviewOfSystems WITH CHECK ADD CONSTRAINT [FK_ReviewOfSystems_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.ReviewOfSystems CHECK CONSTRAINT [FK_ReviewOfSystems_UserCreatedBy]
GO
