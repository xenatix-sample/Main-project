----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[CMHCClientRegistration]
-- Author:		Sumana Sangapu
-- Date:		07/09/2016
--
-- Purpose:		Table to hold the Client Registration data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/09/2016	Sumana Sangapu		Initial creation
-- 07/21/2016	Sumana Sangapu		Added PresentingProblemID
-- 08/22/2016	Sumana Sangapu		Add the Primary key and foregin key constraints
-- 09/07/2016	Sumana Sangapu		Added NOT NULL to MRN
-- 09/14/2016	Sumana Sangapu		Add ErrorMessage and remove constraints
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Synch].[CMHCClientRegistration](
	[ClientRegistrationID] INT IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint]  NULL,
	[MRN] [nvarchar](20)  NULL,
	[FirstName] [nvarchar](200)  NULL,
	[LastName] [nvarchar](200)  NULL,
	[Middle] [nvarchar](200) NULL,
	[Suffix] [nvarchar](10) NULL,
	[EffectiveDate] [nvarchar](10) NULL,
	[EffectiveDateTime] [nvarchar](10) NULL,
	[Gender] [nvarchar](10) NULL,
	[DateofBirth] [nvarchar](10) NULL,
	[SSN] [nvarchar](25) NULL,
	[RaceCode] [nvarchar](10) NULL,
	[EthnicityCode] [nvarchar](10) NULL,
	[LegalStatusCode] [nvarchar](10) NULL,
	[IsActive] [nvarchar](10) NULL,
	[TKIDSID] [nvarchar](50) NULL,
	[TKIDSCASEID] [nvarchar](50) NULL,
	[LCPrimaryLanguageID] [nvarchar](10) NULL,
	[LCSchoolDistrictID] [nvarchar](10) NULL,
	[IsCPSInvolved] [nvarchar](5) NULL,
	[ReferralDate] [nvarchar](10) NULL,
	[CareID] [nvarchar](50) NULL,
	[CMBHSID] [nvarchar](50) NULL,
	[BatchID] [bigint] NOT NULL,
	PresentingProblemID nvarchar(10) NULL,
	ErrorMessage varchar(max) NULL,
	CONSTRAINT [PK_ClientRegistrationID] PRIMARY KEY CLUSTERED ([ClientRegistrationID] ASC)
	)
	GO 

	ALTER TABLE [Synch].[CMHCClientRegistration] WITH CHECK ADD CONSTRAINT [FK_CMHCClientRegistration_BatchID] FOREIGN KEY (BatchID) REFERENCES Synch.Batch (BatchID)
	GO

