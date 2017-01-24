-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Reference].[ReferralCategorySource]
-- Author:		John Crossen
-- Date:		1/2/2016
--
-- Purpose:		ReferralCategorySource
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Reference.[[ReferralCategorySource]]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/2/2016	John Crossen	 TFS:4909		Create Table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ReferralCategorySource](
	[ReferralCategorySourceID] [int] IDENTITY(1,1) NOT NULL,
	[ReferralCategoryID] [int] NOT NULL,
	[ReferralSource] [nvarchar](1000) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralCategorySource] PRIMARY KEY CLUSTERED 
(
	[ReferralCategorySourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ReferralCategorySource WITH CHECK ADD CONSTRAINT [FK_ReferralCategorySource_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralCategorySource CHECK CONSTRAINT [FK_ReferralCategorySource_UserModifedBy]
GO
ALTER TABLE Reference.ReferralCategorySource WITH CHECK ADD CONSTRAINT [FK_ReferralCategorySource_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralCategorySource CHECK CONSTRAINT [FK_ReferralCategorySource_UserCreatedBy]
GO
