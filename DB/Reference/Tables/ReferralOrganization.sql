-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Reference].[ReferralOrganization]
-- Author:		John Crossen
-- Date:		1/2/2016
--
-- Purpose:		ReferralOrganization
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Reference.[ReferralOrganization]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/2/2016	John Crossen	 TFS:4909		Create Table
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ReferralOrganization](
	[ReferralOrganizationID] [int] IDENTITY(1,1) NOT NULL,
	[ReferralOrganization] [nvarchar](400) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralOrganization] PRIMARY KEY CLUSTERED 
(
	[ReferralOrganizationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ReferralOrganization WITH CHECK ADD CONSTRAINT [FK_ReferralOrganization_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralOrganization CHECK CONSTRAINT [FK_ReferralOrganization_UserModifedBy]
GO
ALTER TABLE Reference.ReferralOrganization WITH CHECK ADD CONSTRAINT [FK_ReferralOrganization_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ReferralOrganization CHECK CONSTRAINT [FK_ReferralOrganization_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Referral Organization', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralOrganization;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating referral organizationt', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralOrganization;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralOrganization;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ReferralOrganization;
GO;
