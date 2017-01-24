-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[ContactPayor]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Holds the ContactPayor data for the Benefits Screens.
--
-- Notes:		
--
-- Depends:		Lookup].Payor,
--				registration.PayorGroupPlan,
--				registration.PolicyHolder,
--				registration.Contact,
--				registration.ContactPayorTypeID
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TaskID# 583 -   Initial creation.
-- 07/30/2015	Sumana Sangapu		 1016	-   Change schema from dbo to Registration, Lookup, Core
-- 08/18/2015	Sumana Sangapu	634	Added 3 columns to the table
-- 09/02/2015	Avikal	- Added ContactPayorRank and PolicyHolderName
-- 09/14/2015	Rajiv Ranjan	- Added CoPay
-- 09/15/2015   John Crossen TFS 2313 -- Refactor tables to add details from GroupPayor information
--09/23/2015    Arun Choudhary  TFS 2377 - Removed ID2, Renamed ID1 to PolicyID and converted PolicyHolderID to bigint.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/14/2016	Arun Choudhary	Added CoInsurance.
-- 03/16/2016	Kyle Campbell	TFS #7308	Added PayorExpirationReasonID column
-- 06/14/2016	Atul Chauhan	Added PolicyHolderEmployer (PBI -11154 Policy Holder Employer Field)
-- 06/15/2016	Atul Chauhan	Added Payor Billing info
-- 06/17/2016	Atul Chauhan	Added GroupID,AdditionalInformation
-- 08/26/2016	Scott Martin	Added index
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactPayor](
	[ContactPayorID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ContactID] [BIGINT] NULL,
	[PayorID] [INT] NULL,
	[PayorPlanID] INT NULL,
    [PayorGroupID] INT NULL,
	[PolicyHolderID] BIGINT NULL,
	[PolicyHolderName] [NVARCHAR](250) NULL,
	[GroupID] [NVARCHAR](50) NULL,
	[PolicyHolderFirstName] NVARCHAR (200) NULL,
    [PolicyHolderMiddleName] NVARCHAR (200) NULL,
    [PolicyHolderLastName] NVARCHAR (200) NULL,
	[PolicyHolderEmployer] NVARCHAR (500),
    [PolicyHolderSuffixID] INT  NULL,
	[BillingFirstName] [nvarchar](200) NULL,
	[BillingMiddleName] [nvarchar](200) NULL,
	[BillingLastName] [nvarchar](200) NULL,
	[BillingSuffixID] [int] NULL,
	[AdditionalInformation] [nvarchar](3000) NULL,
	[PayorAddressID] BIGINT NULL,
	[PolicyID] [NVARCHAR](50) NULL,
	[Deductible] [DECIMAL](18, 2) NULL,
	[Copay] [DECIMAL](18, 2) NULL,
	[CoInsurance] [DECIMAL](18, 2) NULL,
	[EffectiveDate] [DATE] NULL,
	[ExpirationDate] [DATE] NULL,
	[PayorExpirationReasonID] [INT] NULL,
	[ExpirationReason] [NVARCHAR](255) NULL,
	[AddRetroDate] [DATE] NULL,
	[HasPolicyHolderSameCardName] [BIT] NULL,
	[ContactPayorRank] [INT] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_ContactPayor_PolicyHolderSuffixID] FOREIGN KEY ([PolicyHolderSuffixID]) REFERENCES [Reference].[Suffix] ([SuffixID]),
 CONSTRAINT [PK_ContactPayor_ContactPayorID] PRIMARY KEY CLUSTERED 
(
	[ContactPayorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_ContactPayor_ContactID] ON [Registration].[ContactPayor]
(
	[ContactID] ASC
)
INCLUDE ( 	[SystemCreatedOn],
	[SystemModifiedOn]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactPayor_PayorAddressId] ON [Registration].[ContactPayor]
(
	[PayorAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactPayor_PayorID] ON [Registration].[ContactPayor]
(
	[PayorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactPayor_PolicyHolderID] ON [Registration].[ContactPayor]
(
	[PolicyHolderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactPayor_PolicyHolderSuffixID] ON [Registration].[ContactPayor]
(
	[PolicyHolderSuffixID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Registration].[ContactPayor]  WITH CHECK ADD  CONSTRAINT [FK_ContactPayor_ContactID] FOREIGN KEY([ContactID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE [Registration].[ContactPayor] CHECK CONSTRAINT [FK_ContactPayor_ContactID]
GO
ALTER TABLE [Registration].[ContactPayor]  WITH CHECK ADD  CONSTRAINT [FK_ContactPayor_PayorID] FOREIGN KEY([PayorID]) REFERENCES [Reference].[Payor] ([PayorID])
GO
ALTER TABLE [Registration].[ContactPayor] CHECK CONSTRAINT [FK_ContactPayor_PayorID]
GO
ALTER TABLE [Registration].[ContactPayor]  WITH CHECK ADD  CONSTRAINT [FK_ContactPayor_PolicyHolderID] FOREIGN KEY([PolicyHolderID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE [Registration].[ContactPayor] CHECK CONSTRAINT [FK_ContactPayor_PolicyHolderID]
GO
ALTER TABLE [Registration].[ContactPayor]  WITH CHECK ADD CONSTRAINT [FK_ContactPayor_ExpirationReasonID] FOREIGN KEY([PayorExpirationReasonID]) REFERENCES [Reference].[PayorExpirationReason] ([PayorExpirationReasonID])
GO
ALTER TABLE [Registration].[ContactPayor] CHECK CONSTRAINT [FK_ContactPayor_PolicyHolderID]
GO
ALTER TABLE Registration.ContactPayor WITH CHECK ADD CONSTRAINT [FK_ContactPayor_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactPayor CHECK CONSTRAINT [FK_ContactPayor_UserModifedBy]
GO
ALTER TABLE Registration.ContactPayor WITH CHECK ADD CONSTRAINT [FK_ContactPayor_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactPayor CHECK CONSTRAINT [FK_ContactPayor_UserCreatedBy]
GO

