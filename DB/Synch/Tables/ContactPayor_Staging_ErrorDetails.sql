 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[contactpayor_Staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Table to hold the error details from the validation of lookup data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[ContactPayor_Staging_ErrorDetails](
	[ContactPayorID] [bigint] NULL,
	[ContactID] [bigint] NULL,
	[PayorID] [varchar](275) NULL,
	[PayorPlanID] [varchar](308) NULL,
	[PayorGroupID] [varchar](255) NULL,
	[PolicyHolderID] [bigint] NULL,
	[PolicyHolderName] [varchar](330) NULL,
	[PolicyHolderFirstName] [varchar](121) NULL,
	[PolicyHolderMiddleName] [varchar](154) NULL,
	[PolicyHolderLastName] [varchar](165) NULL,
	[PolicyHolderSuffixID] [varchar](255) NULL,
	[PayorAddressID] [varchar](88) NULL,
	[PolicyID] [varchar](209) NULL,
	[Deductable] [varchar](255) NULL,
	[Copay] [varchar](255) NULL,
	[CoInsurance] [varchar](255) NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpirationDate] [varchar](255) NULL,
	[PayorExpirationReasonID] [varchar](255) NULL,
	[PayorExpirationReason] [varchar](255) NULL,
	[AddRetroDate] [datetime] NULL,
	[ContactPayorRank] [varchar](255) NULL,
	[IsActive] [varchar](44) NULL,
	[ModifiedBy] [varchar](33) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	GroupID varchar(50) NULL,
    PolicyHolderEmployer varchar(50) NULL,
    BillingFirstName varchar(50) NULL,
    BillingMiddleName varchar(50) NULL,
    BillingLastName varchar(50) NULL,
    BillingSuffixID varchar(50) NULL,
    AdditionalInfo varchar(50) NULL,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY]

GO


