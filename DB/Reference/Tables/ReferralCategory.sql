-----------------------------------------------------------------------------------------------------------------------
-- Table:		[[ReferralCategory]]
-- Author:		John Crossen
-- Date:		10/01/2015
--
-- Purpose:		[ReferralCategory] lookup table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	John Crossen	TFS# 2661 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ReferralCategory](
	[ReferralCategoryID] [INT] IDENTITY(1,1) NOT NULL,
	[ProgramID] [INT] NOT NULL,
	[ReferralCategory] [NVARCHAR](50) NOT NULL,
	[IsActive] [BIT] NOT NULL CONSTRAINT [DF_ECICategory_IsActive]  DEFAULT ((0)),
	[ModifiedBy] [INT] NOT NULL,
	[ModifiedOn] [DATETIME] NOT NULL CONSTRAINT [DF_ECICategory_ModifiedOn]  DEFAULT (GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralCategory] PRIMARY KEY CLUSTERED 
(
	[ReferralCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [UI_ReferralCategory_ReferralCategory_ProgramID] ON [Reference].[ReferralCategory]
(
	[ProgramID] ASC,
	[ReferralCategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Reference].[ReferralCategory]  WITH CHECK ADD  CONSTRAINT [FK_ReferralCategory_Program] FOREIGN KEY([ProgramID]) REFERENCES [Reference].[Program] ([ProgramID])
GO
ALTER TABLE [Reference].[ReferralCategory] CHECK CONSTRAINT [FK_ReferralCategory_Program]
GO
ALTER TABLE Reference.ReferralCategory WITH CHECK ADD CONSTRAINT [FK_ReferralCategory_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralCategory CHECK CONSTRAINT [FK_ReferralCategory_UserModifedBy]
GO
ALTER TABLE Reference.ReferralCategory WITH CHECK ADD CONSTRAINT [FK_ReferralCategory_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralCategory CHECK CONSTRAINT [FK_ReferralCategory_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Referral Category', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralCategory;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating category of referral', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralCategory;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralCategory;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralCategory;
GO;
