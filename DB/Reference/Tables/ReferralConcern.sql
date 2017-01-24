-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ReferralConcern]
-- Author:		John Crossen
-- Date:		10/08/2015
--
-- Purpose:		Lookup for [ReferralConcern] details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	John Crossen	TFS# 2661 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[ReferralConcern](
	[ReferralConcernID] [int] IDENTITY(1,1) NOT NULL,
	[ReferralConcern] [nvarchar](50) NOT NULL,
	[ProgramID] INT NULL,
	[IsActive] [bit] NOT NULL DEFAULT 1,
	[ModifiedBy] [int] NOT NULL CONSTRAINT [DF_ECIReferralConcern_ModifiedBy]  DEFAULT ((1)),
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_ECIReferralConcern_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ECIReferralConcern] PRIMARY KEY CLUSTERED 
(
	[ReferralConcernID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ReferralConcern]  WITH CHECK ADD  CONSTRAINT [FK_ReferralConcern_ProgramID] FOREIGN KEY([ProgramID])
REFERENCES [Reference].[Program] ([ProgramID])
GO

ALTER TABLE [Reference].[ReferralConcern] CHECK CONSTRAINT [FK_ReferralConcern_ProgramID]
GO
ALTER TABLE Reference.ReferralConcern WITH CHECK ADD CONSTRAINT [FK_ReferralConcern_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralConcern CHECK CONSTRAINT [FK_ReferralConcern_UserModifedBy]
GO
ALTER TABLE Reference.ReferralConcern WITH CHECK ADD CONSTRAINT [FK_ReferralConcern_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralConcern CHECK CONSTRAINT [FK_ReferralConcern_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Referral Concern', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralConcern;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating concern of referral', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralConcern;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralConcern;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralConcern;
GO;
