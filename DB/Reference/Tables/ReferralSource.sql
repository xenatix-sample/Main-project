-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ReferralSource]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Lookup for ReferralSource details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TFS# 675 - Initial creation.
-- 07/24/2015   John Crossen    Change IsActive to NOT NULL and add default value
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ReferralSource](
	[ReferralSourceID] [int]  IDENTITY (1,1) NOT NULL,
	[ReferralSource] [nvarchar](150) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralSource_ReferralSourceID] PRIMARY KEY CLUSTERED 
(
	[ReferralSourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ReferralSource] ADD CONSTRAINT IX_ReferralSource UNIQUE(ReferralSource)
GO

ALTER TABLE Reference.ReferralSource WITH CHECK ADD CONSTRAINT [FK_ReferralSource_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralSource CHECK CONSTRAINT [FK_ReferralSource_UserModifedBy]
GO
ALTER TABLE Reference.ReferralSource WITH CHECK ADD CONSTRAINT [FK_ReferralSource_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralSource CHECK CONSTRAINT [FK_ReferralSource_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Referral Source', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralSource;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating source of referral', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralSource;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralSource;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralSource;
GO;
