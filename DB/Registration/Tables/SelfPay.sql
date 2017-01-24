  -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Registration].[SelfPay]
-- Author:		Kyle Campbell
-- Date:		03/21/2015
--
-- Purpose:		Holds SelfPay screen details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Registration.Contact
--				
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/21/2016	Kyle Campbell	Initial Creation
-- 03/30/2016	Kyle Campbell	Renamed OrganizationID to OrganizationDetailID for clarity
--								Added foreign key on OrganizationDetailID to OrganizationDetails.DetailID
-- 04/06/2016	Scott Martin	Added SelfPayHeaderId and removed ContactID
-- 08/26/2016	Scott Martin	Added index
-- 10/25/2016	Arun Choudhary	Allow null for SelfPayAmount, IsPercent
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[SelfPay](
	[SelfPayID] BIGINT NOT NULL IDENTITY (1,1),	
	[ContactID] BIGINT NOT NULL,
	[OrganizationDetailID] BIGINT NOT NULL,
	[SelfPayAmount] DECIMAL(15,2) NULL,
	[IsPercent] BIT NULL, 	
	[EffectiveDate] DATE,
	[ExpirationDate] DATE,
	[IsChildInConservatorship] BIT DEFAULT(0),
	[IsNotAttested] BIT DEFAULT(0),
	[IsApplyingForPublicBenefits] BIT DEFAULT(0),
	[IsEnrolledInPublicBenefits] BIT DEFAULT(0),
	[IsRequestingReconsideration] BIT DEFAULT(0),
	[IsNotGivingConsent] BIT DEFAULT(0),
	[IsOtherChildEnrolled] BIT DEFAULT(0),
	[IsReconsiderationOfAdjustment] BIT DEFAULT(0),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	 CONSTRAINT [PK_SelfPay_SelfPayID] PRIMARY KEY CLUSTERED 
	(
		[SelfPayID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_SelfPay_ContactID] ON [Registration].[SelfPay]
(
	[ContactID] ASC
)
INCLUDE ( 	[EffectiveDate],
	[SelfPayAmount],
	[IsPercent],
	[ExpirationDate],
	[SystemCreatedOn],
	[SystemModifiedOn]
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_SelfPay_SystemCreatedOn] ON [Registration].[SelfPay]
(
	[SystemCreatedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Registration.SelfPay WITH CHECK ADD CONSTRAINT [FK_SelfPay_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.SelfPay CHECK CONSTRAINT [FK_SelfPay_UserModifedBy]
GO
ALTER TABLE Registration.SelfPay WITH CHECK ADD CONSTRAINT [FK_SelfPay_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.SelfPay CHECK CONSTRAINT [FK_SelfPay_UserCreatedBy]
GO
ALTER TABLE [Registration].[SelfPay]  WITH CHECK ADD  CONSTRAINT [FK_SelfPay_ContactID] FOREIGN KEY([ContactID]) REFERENCES [Registration].[Contact]  ([ContactID])
GO
ALTER TABLE  [Registration].[SelfPay] CHECK CONSTRAINT [FK_SelfPay_ContactID]
GO
ALTER TABLE Registration.SelfPay WITH CHECK ADD CONSTRAINT [FK_SelfPay_OrganizationDetailID] FOREIGN KEY ([OrganizationDetailID]) REFERENCES [Core].[OrganizationDetails] ([DetailID])
GO
ALTER TABLE Registration.SelfPay CHECK CONSTRAINT [FK_SelfPay_OrganizationDetailID]
GO


EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'References DetailID of Core.OrganizationDetails', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = SelfPay,
@level2type = N'COLUMN', @level2name = OrganizationDetailID;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the dollar or decimal amount of self pay', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = SelfPay,
@level2type = N'COLUMN', @level2name = SelfPayAmount;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Identifies if the SelfPayAmount field represents a dollar amount or percentage amount', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = SelfPay,
@level2type = N'COLUMN', @level2name = IsPercent;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the effective date of the self pay record', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = SelfPay,
@level2type = N'COLUMN', @level2name = EffectiveDate;
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the expiration date of the self pay record', 
@level0type = N'SCHEMA', @level0name = Registration, 
@level1type = N'TABLE',  @level1name = SelfPay,
@level2type = N'COLUMN', @level2name = ExpirationDate;
GO

