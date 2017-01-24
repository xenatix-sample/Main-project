-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AssessmentTemplate]
-- Author:		Kyle Campbell
-- Date:		03/28/2016
--
-- Purpose:		Holds the template names for assessments  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Kyle Campbell	TFS #8273	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AssessmentTemplate](
	[AssessmentTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[AssessmentTemplateName] [nvarchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentTemplate] PRIMARY KEY CLUSTERED 
(
	[AssessmentTemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.AssessmentTemplate WITH CHECK ADD CONSTRAINT [FK_AssessmentTemplate_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentTemplate CHECK CONSTRAINT [FK_AssessmentTemplate_UserModifedBy]
GO
ALTER TABLE Reference.AssessmentTemplate WITH CHECK ADD CONSTRAINT [FK_AssessmentTemplate_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentTemplate CHECK CONSTRAINT [FK_AssessmentTemplate_UserCreatedBy]
GO


EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the template names for the assessments', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentTemplate,
@level2type = N'COLUMN', @level2name = AssessmentTemplateName;
GO