
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ReferralOutcomeDetails]
-- Author:		Sumana Sangapu
-- Date:		12/11/2015
--
-- Purpose:		Lookup for Referral Outcome Details  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/11/2015	Sumana Sangapu	Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralOutcomeDetails](
	[ReferralOutcomeDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ReferralHeaderID] [bigint]  NOT NULL,
	[FollowupExpected] [bit] NOT NULL,
	[FollowupProviderID] [int]    NULL,
	[FollowupDate] [date]   NULL,
	[FollowupOutcome] [nvarchar](500)   NULL,
	[IsAppointmentNotified] [bit] NULL,
	[AppointmentNotificationMethod] [nvarchar] (100) NULL,
	[Comments] [nvarchar] (500) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralOutcomeDetails] PRIMARY KEY CLUSTERED 
(
	[ReferralOutcomeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ReferralOutcomeDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralOutcomeDetails_Referral] FOREIGN KEY([ReferralHeaderID])
REFERENCES [Registration].[ReferralHeader] ([ReferralHeaderID])
GO

ALTER TABLE [Registration].[ReferralOutcomeDetails] CHECK CONSTRAINT [FK_ReferralOutcomeDetails_Referral]
GO

ALTER TABLE Registration.ReferralOutcomeDetails WITH CHECK ADD CONSTRAINT [FK_ReferralOutcomeDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralOutcomeDetails CHECK CONSTRAINT [FK_ReferralOutcomeDetails_UserModifedBy]
GO
ALTER TABLE Registration.ReferralOutcomeDetails WITH CHECK ADD CONSTRAINT [FK_ReferralOutcomeDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralOutcomeDetails CHECK CONSTRAINT [FK_ReferralOutcomeDetails_UserCreatedBy]
GO
