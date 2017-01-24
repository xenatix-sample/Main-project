-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ReferralClosureReason]
-- Author:		John Crossen
-- Date:		10/08/2015
--
-- Purpose:		Lookup for [ReferralClosureReason] details
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

CREATE TABLE [Reference].[ReferralClosureReason](
	[ReferralClosureReasonID] [int] IDENTITY(1,1) NOT NULL,
	[ReferralClosureReason] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_ECIClosureReason_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_ECIClosureReason_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ECIClosureReason] PRIMARY KEY CLUSTERED 
(
	[ReferralClosureReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ReferralClosureReason WITH CHECK ADD CONSTRAINT [FK_ReferralClosureReason_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralClosureReason CHECK CONSTRAINT [FK_ReferralClosureReason_UserModifedBy]
GO
ALTER TABLE Reference.ReferralClosureReason WITH CHECK ADD CONSTRAINT [FK_ReferralClosureReason_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralClosureReason CHECK CONSTRAINT [FK_ReferralClosureReason_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Referral Closure Reason', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralClosureReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating reason for closing referral', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralClosureReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralClosureReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralClosureReason;
GO;
