-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ReferralDispositionDetails]
-- Author:		Sumana Sangapu
-- Date:		12/11/2015
--
-- Purpose:		Lookup for Referral Disposition Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/11/2015	Sumana Sangapu	Initial creation.
-- 12/21/2015   Satish Singh	DispositionID is not mandatory
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralDispositionDetails](
	[ReferralDispositionDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ReferralHeaderID] [bigint]  NOT NULL,
	[ReferralDispositionID] [int] NULL,
	[ReasonforDenial] [nvarchar](500) NULL,
	[ReferralDispositionOutcomeID] [INT] NULL,
	[AdditionalNotes] [nvarchar](500) NULL,
	[UserID] [int] NOT NULL,
	[DispositionDate]  [date] NOT NULL DEFAULT(GETUTCDATE()),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralDispositionDetail] PRIMARY KEY CLUSTERED 
(
	[ReferralDispositionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ReferralDispositionDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDispositionDetails_ReferralHeader] FOREIGN KEY([ReferralHeaderID])
REFERENCES [Registration].[ReferralHeader] ([ReferralHeaderID])
GO

ALTER TABLE [Registration].[ReferralDispositionDetails] CHECK CONSTRAINT [FK_ReferralDispositionDetails_ReferralHeader]
GO

ALTER TABLE [Registration].[ReferralDispositionDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDispositionDetails_ReferralDisposition] FOREIGN KEY([ReferralDispositionID])
REFERENCES [Reference].[ReferralDisposition] ([ReferralDispositionID])
GO

ALTER TABLE [Registration].[ReferralDispositionDetails] CHECK CONSTRAINT [FK_ReferralDispositionDetails_ReferralDisposition]
GO

ALTER TABLE [Registration].[ReferralDispositionDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDispositionDetails_DispositionOutcome] FOREIGN KEY([ReferralDispositionOutcomeID])
REFERENCES [Reference].[ReferralDispositionOutcome] ([ReferralDispositionOutcomeID])
GO

ALTER TABLE [Registration].[ReferralDispositionDetails] CHECK CONSTRAINT [FK_ReferralDispositionDetails_DispositionOutcome]
GO

ALTER TABLE [Registration].[ReferralDispositionDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralDispositionDetails_Users] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Registration].[ReferralDispositionDetails] CHECK CONSTRAINT [FK_ReferralDispositionDetails_Users]
GO

ALTER TABLE Registration.ReferralDispositionDetails WITH CHECK ADD CONSTRAINT [FK_ReferralDispositionDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralDispositionDetails CHECK CONSTRAINT [FK_ReferralDispositionDetails_UserModifedBy]
GO
ALTER TABLE Registration.ReferralDispositionDetails WITH CHECK ADD CONSTRAINT [FK_ReferralDispositionDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralDispositionDetails CHECK CONSTRAINT [FK_ReferralDispositionDetails_UserCreatedBy]
GO
