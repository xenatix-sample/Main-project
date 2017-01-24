-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ReferralReferredToDetails]
-- Author:		Sumana Sangapu
-- Date:		12/11/2015
--
-- Purpose:		Lookup for Referred To Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/11/2015	Sumana Sangapu	Initial creation.
-- 12/16/2015   Satish Singh    Added ProgramID and ReferredDateTime
-- 12/21/2015   Satish Singh	ProgramID,ReferredDateTime, ActionTaken and Comments  are not mandatory
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralReferredToDetails](
	[ReferredToDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ReferralHeaderID] [bigint]  NOT NULL,
	[ProgramID] int NULL,
	[OrganizationID] [bigint]  NULL,
	[ReferredDateTime] [datetime]  NULL,
	[ActionTaken] [nvarchar](500)  NULL,
	[Comments] [nvarchar](500)  NULL,
	[UserID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralReferredToDetails] PRIMARY KEY CLUSTERED 
(
	[ReferredToDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ReferralReferredToDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralReferredToDetails_Referral] FOREIGN KEY([ReferralHeaderID])
REFERENCES [Registration].[ReferralHeader] ([ReferralHeaderID])
GO

ALTER TABLE [Registration].[ReferralReferredToDetails] CHECK CONSTRAINT [FK_ReferralReferredToDetails_Referral]
GO

ALTER TABLE [Registration].[ReferralReferredToDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralReferredToDetails_Users] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ReferralReferredToDetails] CHECK CONSTRAINT [FK_ReferralReferredToDetails_Users]
GO

ALTER TABLE [Registration].[ReferralReferredToDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralReferredToDetails_OrganizationID] FOREIGN KEY([OrganizationID])
REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID])
GO

ALTER TABLE [Registration].[ReferralReferredToDetails] CHECK CONSTRAINT [FK_ReferralReferredToDetails_OrganizationID]
GO

ALTER TABLE Registration.ReferralReferredToDetails WITH CHECK ADD CONSTRAINT [FK_ReferralReferredToDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralReferredToDetails CHECK CONSTRAINT [FK_ReferralReferredToDetails_UserModifedBy]
GO
ALTER TABLE Registration.ReferralReferredToDetails WITH CHECK ADD CONSTRAINT [FK_ReferralReferredToDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralReferredToDetails CHECK CONSTRAINT [FK_ReferralReferredToDetails_UserCreatedBy]
GO
