-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ReferralHeader]
-- Author:		Sumana Sangapu
-- Date:		12/14/2015
--
-- Purpose:		Lookup for ReferralHeader 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/14/2015	Sumana Sangapu	Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 01/04/2017	Sumana Sangapu	Added IsLinkedToContact and IsReferrerConvertedtoCollateral
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ReferralHeader](
	[ReferralHeaderID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint]  NOT NULL,
	[ReferralDate] DATE NOT NULL,
	[ReferralStatusID] [int]  NULL,
	[ReferralTypeID] [int]  NULL,
	[ResourceTypeID] [int] NULL,
	[ReferralCategorySourceID] [int] NULL,
	[ReferralOriginID] [int] NULL,
	[ProgramID] int NULL,
	[OrganizationID] BIGINT NULL,
	[ReferralOrganizationID] INT NULL,
	[OtherOrganization] NVARCHAR(100) NULL,
	[ReferralSourceID]	INT NULL,
	[OtherSource] NVARCHAR(100) NULL,
	[IsLinkedToContact] BIT NULL,
	[IsReferrerConvertedToCollateral] BIT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ReferralHeader] PRIMARY KEY CLUSTERED 
(
	[ReferralHeaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Registration].[ReferralHeader] ADD  CONSTRAINT [DF_ReferralHeader_ReferralDate]  DEFAULT (GETUTCDATE()) FOR [ReferralDate]
GO

ALTER TABLE [Registration].[ReferralHeader]  WITH CHECK ADD  CONSTRAINT [FK_ReferralHeader_ContactID] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ReferralHeader] CHECK CONSTRAINT [FK_ReferralHeader_ContactID]
GO

ALTER TABLE [Registration].[ReferralHeader]  WITH CHECK ADD  CONSTRAINT [FK_ReferralHeader_ReferralStatus] FOREIGN KEY([ReferralStatusID])
REFERENCES [Reference].[ReferralStatus] ([ReferralStatusID])
GO

ALTER TABLE [Registration].[ReferralHeader] CHECK CONSTRAINT [FK_ReferralHeader_ReferralStatus]
GO

ALTER TABLE [Registration].[ReferralHeader]  WITH CHECK ADD  CONSTRAINT [FK_ReferralHeader_ReferralCategory] FOREIGN KEY([ReferralCategorySourceID])
REFERENCES [Reference].[ReferralCategorySource] ([ReferralCategorySourceID])
GO

ALTER TABLE [Registration].[ReferralHeader] CHECK CONSTRAINT [FK_ReferralHeader_ReferralCategory]
GO

ALTER TABLE [Registration].[ReferralHeader]  WITH CHECK ADD  CONSTRAINT [FK_ReferralHeader_ReferralOrigin] FOREIGN KEY([ReferralOriginID])
REFERENCES [Reference].[ReferralOrigin] ([ReferralOriginID])
GO

ALTER TABLE [Registration].[ReferralHeader] CHECK CONSTRAINT [FK_ReferralHeader_ReferralOrigin]
GO


ALTER TABLE [Registration].[ReferralHeader]  WITH CHECK ADD  CONSTRAINT [FK_ReferralHeader_ReferralType] FOREIGN KEY([ReferralTypeID])
REFERENCES [Reference].[ReferralType] ([ReferralTypeID])
GO

ALTER TABLE [Registration].[ReferralHeader] CHECK CONSTRAINT [FK_ReferralHeader_ReferralType]
GO


ALTER TABLE [Registration].[ReferralHeader]  WITH CHECK ADD  CONSTRAINT [FK_ReferralHeader_OrganizationID] FOREIGN KEY([OrganizationID])
REFERENCES [Core].OrganizationDetailsMapping ([MappingID])
GO

ALTER TABLE [Registration].[ReferralHeader] CHECK CONSTRAINT [FK_ReferralHeader_OrganizationID]
GO

ALTER TABLE [Registration].[ReferralHeader]  WITH CHECK ADD  CONSTRAINT [FK_ReferralHeader_Organization] FOREIGN KEY([ReferralOrganizationID])
REFERENCES [Reference].[ReferralOrganization] ([ReferralOrganizationID])
GO

ALTER TABLE [Registration].[ReferralHeader] CHECK CONSTRAINT [FK_ReferralHeader_Organization]
GO

ALTER TABLE Registration.ReferralHeader WITH CHECK ADD CONSTRAINT [FK_ReferralHeader_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralHeader CHECK CONSTRAINT [FK_ReferralHeader_UserModifedBy]
GO
ALTER TABLE Registration.ReferralHeader WITH CHECK ADD CONSTRAINT [FK_ReferralHeader_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ReferralHeader CHECK CONSTRAINT [FK_ReferralHeader_UserCreatedBy]
GO
