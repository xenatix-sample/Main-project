-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ReferralAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		12/11/2015
--
-- Purpose:		Lookup for ReferralAdditionalDetails 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY -------------------------------------------------------------------------------------------------------------
-- 12/11/2015	Sumana Sangapu	Initial creation.
-- 12/22/2015	Sumana Sangapu	Mapping to Contact table 
-- 01/04/2016	Sumana Sangapu	Removed fields not related to ECI since they are captured as Additional Demogratphics in ECI Registration
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralAdditionalDetails](
	[ReferralAdditionalDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ReferralHeaderID] [bigint]  NOT NULL,
	[ContactID] [bigint] NOT NULL,
--	[IsCurentlyHospitalized] bit NULL,
--	[ExpectedDischargeDate] date NULL,
--	[SSI]	bit NULL DEFAULT('N'),
--	[ManagedCare] bit NULL DEFAULT ('N'),
	[ReasonforCare] nvarchar(100) NULL,
	[IsTransferred] bit NULL,
	[IsHousingProgram] bit NULL,
	[HousingDescription] nvarchar(100) NULL,
	[IsEligibleforFurlough] bit NULL,
--	[ProgramTransferredFromID] int NULL,
	[IsReferralDischargeOrTransfer] bit NULL,
	[IsConsentRequired] bit NULL,
	[Comments] nvarchar(500) NULL,
	[AdditionalConcerns]	[nvarchar] (500) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralAdditionalDetails] PRIMARY KEY CLUSTERED 
(
	[ReferralAdditionalDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Registration].[ReferralAdditionalDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralAdditionalDetails_ReferralHeader] FOREIGN KEY([ReferralHeaderID])
REFERENCES [Registration].[ReferralHeader] ([ReferralHeaderID])
GO

ALTER TABLE [Registration].[ReferralAdditionalDetails]  CHECK CONSTRAINT [FK_ReferralAdditionalDetails_ReferralHeader]
GO

ALTER TABLE [Registration].[ReferralAdditionalDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralAdditionalDetails_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ReferralAdditionalDetails]  CHECK CONSTRAINT [FK_ReferralAdditionalDetails_Contact]
GO

--ALTER TABLE [Registration].[ReferralAdditionalDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReferralAdditionalDetails_ProgramID] FOREIGN KEY([ProgramTransferredFromID])
--REFERENCES [Reference].[Program] ([ProgramID])
--GO

--ALTER TABLE [Registration].[ReferralAdditionalDetails] CHECK CONSTRAINT [FK_ReferralAdditionalDetails_ProgramID]
--GO

ALTER TABLE Registration.ReferralAdditionalDetails WITH CHECK ADD CONSTRAINT [FK_ReferralAdditionalDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralAdditionalDetails CHECK CONSTRAINT [FK_ReferralAdditionalDetails_UserModifedBy]
GO
ALTER TABLE Registration.ReferralAdditionalDetails WITH CHECK ADD CONSTRAINT [FK_ReferralAdditionalDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralAdditionalDetails CHECK CONSTRAINT [FK_ReferralAdditionalDetails_UserCreatedBy]
GO

