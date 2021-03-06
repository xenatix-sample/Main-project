-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AssessmentOptions]
-- Author:		Sumana Sangapu
-- Date:		08/15/2015
--
-- Purpose:		Holds the Options details of assessments  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2015   Sumana Sangapu  Task# 1601 - Assessment DB Design
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 03/23/2016	Kyle Campbell	TFS #8273 Added unique constraint on Datakey field, which now needs to be unique for new assessment logic mapping table
--								Changed Datakey field from varchar to BIGINT since we will be joining on these fields
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AssessmentOptions](
	[OptionsID] [bigint] NOT NULL IDENTITY(1,1),
	[OptionsGroupID] [bigint] NOT NULL,
	[Options] [nvarchar](1000) NULL,
	[SortOrder] [int] NULL,
	[DataKey] BIGINT NOT NULL,
	[Attributes] [NVARCHAR](MAX) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Options] PRIMARY KEY CLUSTERED 
(
	[OptionsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Core].[AssessmentOptions]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentOptions_OptionsGroup] FOREIGN KEY([OptionsGroupID])
REFERENCES [Core].[AssessmentOptionsGroup] ([OptionsGroupID])
GO
ALTER TABLE [Core].[AssessmentOptions] CHECK CONSTRAINT [FK_AssessmentOptions_OptionsGroup]
GO

ALTER TABLE Core.AssessmentOptions WITH CHECK ADD CONSTRAINT [FK_AssessmentOptions_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentOptions CHECK CONSTRAINT [FK_AssessmentOptions_UserModifedBy]
GO
ALTER TABLE Core.AssessmentOptions WITH CHECK ADD CONSTRAINT [FK_AssessmentOptions_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentOptions CHECK CONSTRAINT [FK_AssessmentOptions_UserCreatedBy]
GO

ALTER TABLE [Core].[AssessmentOptions] ADD CONSTRAINT IX_OptionDataKey UNIQUE (DataKey)
GO