-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ReferralForwardedDetails]
-- Author:		Sumana Sangapu
-- Date:		12/11/2015
--
-- Purpose:		Lookup for Referral Forwarded To Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/11/2015	Sumana Sangapu	Initial creation.
-- 12/16/2015	Scott Martin	Added FacilityID
-- 12/17/2015	Sumana Sangapu	Added UserID Reference from Core.UserFacility 
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralForwardedDetails](
	[ReferralForwardedDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ReferralHeaderID] [bigint]  NOT NULL,
	[OrganizationID] [bigint] NOT NULL,
	[FacilityID] int NULL,
	[SendingReferralToID] [int] NULL,
	[Comments] [nvarchar](500) NULL,
	[UserID] [int] NOT NULL,
	[ReferralSentDate]  [date] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralForwardedDetails] PRIMARY KEY CLUSTERED 
(
	[ReferralForwardedDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ReferralForwardedDetails]   WITH CHECK ADD  CONSTRAINT [FK_ReferralForwardedDetails_Referral] FOREIGN KEY([ReferralHeaderID])
REFERENCES [Registration].[ReferralHeader]  ([ReferralHeaderID])
GO

ALTER TABLE [Registration].[ReferralForwardedDetails] CHECK CONSTRAINT [FK_ReferralForwardedDetails_Referral]
GO

ALTER TABLE [Registration].[ReferralForwardedDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralForwardedDetails_Users] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ReferralForwardedDetails] CHECK CONSTRAINT [FK_ReferralForwardedDetails_Users]
GO

ALTER TABLE [Registration].[ReferralForwardedDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralForwardedDetails_OrganizationID] FOREIGN KEY([OrganizationID])
REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID])
GO

ALTER TABLE [Registration].[ReferralForwardedDetails] CHECK CONSTRAINT [FK_ReferralForwardedDetails_OrganizationID]
GO

ALTER TABLE [Registration].[ReferralForwardedDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralForwardedDetails_SendingReferralToID] FOREIGN KEY([SendingReferralToID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ReferralForwardedDetails] CHECK CONSTRAINT [FK_ReferralForwardedDetails_Users]
GO

ALTER TABLE Registration.ReferralForwardedDetails WITH CHECK ADD CONSTRAINT [FK_ReferralForwardedDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralForwardedDetails CHECK CONSTRAINT [FK_ReferralForwardedDetails_UserModifedBy]
GO
ALTER TABLE Registration.ReferralForwardedDetails WITH CHECK ADD CONSTRAINT [FK_ReferralForwardedDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralForwardedDetails CHECK CONSTRAINT [FK_ReferralForwardedDetails_UserCreatedBy]
GO
