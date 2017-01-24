----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[CMHCClientAddress]
-- Author:		Sumana Sangapu
-- Date:		07/18/2016
--
-- Purpose:		Table to hold the Client Address data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/18/2016	Sumana Sangapu		Initial creation
-- 08/22/2016	Sumana Sangapu		Add the Primary key and foregin key constraints
-- 09/13/2016	Sumana Sangapu		Add ErrorMessage
-- 10/18/2016	Sumana Sangapu		Add EffectiveDate for ContactAddress
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[CMHCClientAddress](
	[CMHCClientAddressID] INT IDENTITY(1,1) NOT NULL,
	[ContactID] bigint  NULL,
	[MRN] nvarchar(50)  NULL,
	[AddressType] nvarchar(50) NULL,
	[Name] [nvarchar](200) NULL,
	[Line1] [nvarchar](200) NULL,
	[City] [nvarchar](200) NULL,
	[County] nvarchar(10) NULL,
	[StateProvinceCode] nvarchar(2) NULL,
	[Zip] [nvarchar](10) NULL,
	[ComplexName] [nvarchar](255) NULL,
	[GateCode] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[ContactAddressID] bigint  NULL,
	[BatchID] bigint NOT NULL,
	ErrorMessage varchar(max) NULL, -- Datatype is chosen as Varchar as, DT_NTEXT , the equivalent datatype is not supported in SSIS.
	EffectiveDate nvarchar(20) NULL
	CONSTRAINT [PK_CMHCClientAddressID] PRIMARY KEY CLUSTERED ([CMHCClientAddressID] ASC)
	)
	GO 

	ALTER TABLE [Synch].[CMHCClientAddress] WITH CHECK ADD CONSTRAINT [FK_CMHCClientAddress_BatchID] FOREIGN KEY (BatchID) REFERENCES Synch.Batch (BatchID)
	GO
