-----------------------------------------------------------------------------------------------------------------------
-- Table:	[Registration].[ReferralConcernDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:	 Referral Concern details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu - Initial Creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralConcernDetails](
	[ReferralConcernDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralAdditionalDetailID] [bigint] NULL,
	[ReferralConcernID] [int] NULL,
	--[AdditionalConcerns]	[nvarchar] (500) NULL,
	[Diagnosis] [nvarchar](1000) NULL,
	[ReferralPriorityID] [int] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralConcernDetails] PRIMARY KEY CLUSTERED 
(
	[ReferralConcernDetailID]  ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
 

GO

ALTER TABLE [Registration].[ReferralConcernDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralConcernDetails_ReferralAdditionalDetails] FOREIGN KEY([ReferralAdditionalDetailID])
REFERENCES [Registration].[ReferralAdditionalDetails] ([ReferralAdditionalDetailID])
GO

ALTER TABLE [Registration].[ReferralConcernDetails]  CHECK CONSTRAINT [FK_ReferralConcernDetails_ReferralAdditionalDetails]
GO

ALTER TABLE [Registration].[ReferralConcernDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralConcernDetails_ReferralConcern] FOREIGN KEY([ReferralConcernID])
REFERENCES [Reference].[ReferralConcern] ([ReferralConcernID])
GO

ALTER TABLE [Registration].[ReferralConcernDetails]  CHECK CONSTRAINT [FK_ReferralConcernDetails_ReferralConcern]
GO

ALTER TABLE [Registration].[ReferralConcernDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralConcernDetails_ReferralPriority] FOREIGN KEY([ReferralPriorityID])
REFERENCES [Reference].[ReferralPriority] ([ReferralPriorityID])
GO

ALTER TABLE [Registration].[ReferralConcernDetails]  CHECK CONSTRAINT [FK_ReferralConcernDetails_ReferralPriority]
GO

ALTER TABLE Registration.ReferralConcernDetails WITH CHECK ADD CONSTRAINT [FK_ReferralConcernDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralConcernDetails CHECK CONSTRAINT [FK_ReferralConcernDetails_UserModifedBy]
GO
ALTER TABLE Registration.ReferralConcernDetails WITH CHECK ADD CONSTRAINT [FK_ReferralConcernDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralConcernDetails CHECK CONSTRAINT [FK_ReferralConcernDetails_UserCreatedBy]
GO
