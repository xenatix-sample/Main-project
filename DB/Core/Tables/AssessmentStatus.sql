-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AssessmentStatus]
-- Author:		Scott Martin
-- Date:		11/16/2015
--
-- Purpose:		Assessment Status Data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/16/2015	Scott Martin    Initial Creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- --------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AssessmentStatus](
	[AssessmentStatusID] [smallint] IDENTITY(1,1) NOT NULL,
	[AssessmentStatus] [nvarchar](255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentStatus] PRIMARY KEY CLUSTERED 
(
	[AssessmentStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Core.AssessmentStatus WITH CHECK ADD CONSTRAINT [FK_AssessmentStatus_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentStatus CHECK CONSTRAINT [FK_AssessmentStatus_UserModifedBy]
GO
ALTER TABLE Core.AssessmentStatus WITH CHECK ADD CONSTRAINT [FK_AssessmentStatus_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentStatus CHECK CONSTRAINT [FK_AssessmentStatus_UserCreatedBy]
GO
