-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AssessmentGroup]
-- Author:		Scott Martin
-- Date:		06/08/2016
--
-- Purpose:		Header table for separating out groups of Assessments
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AssessmentGroup](
	[AssessmentGroupID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssessmentGroup] [nvarchar](255) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentGroup] PRIMARY KEY CLUSTERED 
(
	[AssessmentGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.AssessmentGroup WITH CHECK ADD CONSTRAINT [FK_AssessmentGroup_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentGroup CHECK CONSTRAINT [FK_AssessmentGroup_UserModifedBy]
GO
ALTER TABLE Reference.AssessmentGroup WITH CHECK ADD CONSTRAINT [FK_AssessmentGroup_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentGroup CHECK CONSTRAINT [FK_AssessmentGroup_UserCreatedBy]
GO

