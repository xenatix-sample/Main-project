-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ReferralAgency]
-- Author:		Gaurav Gupta
-- Date:		03/03/2016
--
-- Purpose:		Lookup for [ReferralAgency] details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/03/2016	Gaurav Gupta	TFS# 2661 - Initial creation.
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ReferralAgency](
	[ReferralAgencyID] [INT] IDENTITY(1,1) NOT NULL,
	[ReferralAgency] [NVARCHAR](50) NOT NULL,
	[IsActive] [BIT] NOT NULL CONSTRAINT [DF_ECIAgency_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [INT] NOT NULL,
	[ModifiedOn] [DATETIME] NOT NULL CONSTRAINT [DF_ECIAgency_ModifiedOn]  DEFAULT (GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ECIAgency] PRIMARY KEY CLUSTERED 
(
	[ReferralAgencyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ReferralAgency WITH CHECK ADD CONSTRAINT [FK_ReferralAgency_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralAgency CHECK CONSTRAINT [FK_ReferralAgency_UserModifedBy]
GO
ALTER TABLE Reference.ReferralAgency WITH CHECK ADD CONSTRAINT [FK_ReferralAgency_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralAgency CHECK CONSTRAINT [FK_ReferralAgency_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Referral Agency', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralAgency;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating referral agency', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralAgency;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralAgency;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralAgency;
GO;
