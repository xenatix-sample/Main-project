----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[CMHCSelfPay]
-- Author:		Sumana Sangapu
-- Date:		07/18/2016
--
-- Purpose:		Table to hold the Client Selfpay data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/18/2016	Sumana Sangapu		Initial creation
-- 08/22/2016	Sumana Sangapu		Add the Primary key and foregin key constraints
-- 09/13/2016	Sumana Sangapu		Add ErrorMessage and remove constraints
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[CMHCSelfPay](
	[CMHCSelfPayID] [int] IDENTITY(1,1) NOT NULL,
	[SelfPayID] [bigint]  NULL,
	[ContactID] [bigint]  NULL,
	[MRN] [nvarchar](50)  NULL,
	[EffectiveDate] [varchar](30)  NULL,
	[TotalIncome] [varchar](30) NULL,
	[FamilySize] [varchar](5) NULL,
	[SelfPayAmount] [varchar](30)  NULL,
	[IsPercent] [varchar](5)  NULL,
	[Division] [nvarchar](100)  NULL,
	ExpirationDate varchar(20) NULL,
	BatchID BIGINT NOT NULL,
	ErrorMessage varchar(max) NULL
	CONSTRAINT [PK_CMHCSelfPayID] PRIMARY KEY CLUSTERED ([CMHCSelfPayID] ASC)
	)
	GO 

	ALTER TABLE [Synch].[CMHCSelfPay] WITH CHECK ADD CONSTRAINT [FK_CMHCSelfPay_BatchID] FOREIGN KEY (BatchID) REFERENCES Synch.Batch (BatchID)
	GO




