----------------------------------------------------------------------------------------------------------------------
-- Table:		Synch.CMHCContactPayorDetails
-- Author:		Sumana Sangapu
-- Date:		07/19/2016
--
-- Purpose:		Table to hold the Client Payor details for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/19/2016	Sumana Sangapu		Initial creation
-- 08/22/2016	Sumana Sangapu		Add the Primary key and foregin key constraints
-- 09/13/2016	Sumana Sangapu		Add ErrorMessage and remove constraints
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE Synch.CMHCContactPayorDetails(
	[PayorDetailsID] INT IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint]  NULL,
	[MRN] nvarchar(50)  NULL,
	[ContractID] nvarchar(100)  NULL,
	[BillingClientLastName] [nvarchar](200) NULL,
	[BillingClientFirstName] [nvarchar](200) NULL,
	[BillingClientMiddleName] [nvarchar](200) NULL,
	[BillingClientSuffix] [nvarchar](50) NULL,

	[PolicyHolderLastName] [nvarchar](200) NULL,
	[PolicyHolderFirstName] [nvarchar](200) NULL,
	[PolicyHolderMiddleName] [nvarchar](200) NULL,
	[PolicyHolderSuffix] [nvarchar](50) NULL,

	[PayorID] [int] NULL,
	[PayorPlanID] [int] NULL,
	[GroupID] nvarchar(50) NULL,
	[PolicyHolderID] [bigint] NULL,

	[PolicyHolderStreetAddress] [nvarchar](200) NULL,
	[PolicyHolderCityName] [nvarchar](200) NULL,
	[PolicyHolderStateName] nchar(2) NULL,
	[PolicyHolderZip] [nvarchar](10) NULL,
	[PolicyHolderPhone] [nvarchar](50) NULL,
	[PolicyHolderRelationship] nvarchar(50) NULL,
	[PolicyHolderGender] nvarchar(10) NULL,
	[PolicyHolderDOB] nvarchar(10) NULL,
	[PolicyHolderSSN] nvarchar(15) NULL,

	[PolicyID] [nvarchar](50) NULL,
	[Deductible] [decimal](18, 2) NULL,
	[Copay] [decimal](18, 2) NULL,
	[CoInsurance] [decimal](18, 2) NULL,
	[EffectiveDate] nvarchar(20) NULL,
	[ExpirationDate] nvarchar(20) NULL,
	[AddRetroDate] nvarchar(20) NULL,
	[PayorExpirationReasonID] nvarchar(255) NULL,
	[UniqueIdentifierID] bigint NULL,
	BatchID BIGINT NOT NULL,
	PolicyHolderSuffixID INT NULL,
	BillingClientSuffixID INT NULL,
	ErrorMessage varchar(max) NULL,
	CONSTRAINT [PK_PayorDetailsID] PRIMARY KEY CLUSTERED ([PayorDetailsID] ASC)
	)
	GO 

	ALTER TABLE  Synch.CMHCContactPayorDetails WITH CHECK ADD CONSTRAINT [FK_CMHCContactPayorDetails_BatchID] FOREIGN KEY (BatchID) REFERENCES Synch.Batch (BatchID)
	GO
