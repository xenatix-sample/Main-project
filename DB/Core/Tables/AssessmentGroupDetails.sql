-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AssessmentGroupDetails]
-- Author:		Scott Martin
-- Date:		06/08/2016
--
-- Purpose:		Mapping table for associating a Assessment type to a Assessment group
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AssessmentGroupDetails](
	[AssessmentGroupDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssessmentGroupID] [bigint] NOT NULL,
	[AssessmentID] [bigint] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentGroupDetails] PRIMARY KEY CLUSTERED 
(
	[AssessmentGroupDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[AssessmentGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentGroupDetails_AssessmentGroup] FOREIGN KEY([AssessmentGroupID])
REFERENCES [Reference].[AssessmentGroup] ([AssessmentGroupID])
GO

ALTER TABLE [Reference].[AssessmentGroupDetails] CHECK CONSTRAINT [FK_AssessmentGroupDetails_AssessmentGroup]
GO

ALTER TABLE [Reference].[AssessmentGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentGroupDetails_AssessmentID] FOREIGN KEY([AssessmentID])
REFERENCES [Core].[Assessments] ([AssessmentID])
GO

ALTER TABLE [Reference].[AssessmentGroupDetails] CHECK CONSTRAINT [FK_AssessmentGroupDetails_AssessmentID]
GO

ALTER TABLE Reference.AssessmentGroupDetails WITH CHECK ADD CONSTRAINT [FK_AssessmentGroupDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentGroupDetails CHECK CONSTRAINT [FK_AssessmentGroupDetails_UserModifedBy]
GO
ALTER TABLE Reference.AssessmentGroupDetails WITH CHECK ADD CONSTRAINT [FK_AssessmentGroupDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentGroupDetails CHECK CONSTRAINT [FK_AssessmentGroupDetails_UserCreatedBy]
GO

