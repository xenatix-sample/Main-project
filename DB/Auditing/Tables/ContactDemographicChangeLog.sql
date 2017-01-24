-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Auditing].[ContactDemographicChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Holds the change log details for contact demographics
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14793	Initial creation
-- 11/03/2016	Kyle Campbell	TFS #16309	Add SSNEncrypted column
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Auditing].[ContactDemographicChangeLog]
(
	[ContactDemographicChangeLogID] bigint identity(1,1) NOT NULL,
	[TransactionLogID] bigint NOT NULL,
	[UserID] int NOT NULL,
	[ChangedDate] datetime NOT NULL,
	[UserFirstName] nvarchar(50),
	[UserLastName] nvarchar(50),	
	[ContactID] bigint NOT NULL,
	[PresentingProblemType] nvarchar(255),
	[EffectiveDate] date,
	[ExpirationDate] date,
	[MRN] bigint,
	[MPI] nvarchar(200),
	[ClientType] nvarchar(200),
	[FirstName] nvarchar(200),
	[LastName] nvarchar(200),
	[Middle] nvarchar(200),
	[Title] nvarchar(10),
	[Suffix] nvarchar(50),
	[Gender] nvarchar(50),
	[PreferredGender] nvarchar(50),
	[DOB] date,
	[DOBStatus] nvarchar(150),
	[SSN] nvarchar(9),
	[SSNStatus] nvarchar(50),
	[DriverLicense] nvarchar(50),
	[DriverLicenseState] nvarchar(50),
	[PreferredName] nvarchar(200),
	[DeceasedDate] datetime,
	[PreferredContactMethod] nvarchar(150),
	[ReferralSource] nvarchar(150),
	[IsPregnant] bit,
	[GestationalAge] decimal(3,1),
	[IsActive] bit,
	[SSNEncrypted] varbinary(2000) NULL
	CONSTRAINT [PK_ContactDemographicChangeLog_ContactDemographicChangeLogID] PRIMARY KEY CLUSTERED ([ContactDemographicChangeLogID] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_ContactDemographicChangeLog_TransactionLogID] ON [Auditing].[ContactDemographicChangeLog]
(
	[TransactionLogID] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactDemographicChangeLog_ContactID] ON [Auditing].[ContactDemographicChangeLog]
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Auditing].[ContactDemographicChangeLog]  WITH CHECK ADD  CONSTRAINT [FK_ContactDemographicChangeLog_TransasctionLogId] FOREIGN KEY([TransactionLogId]) REFERENCES [Core].[TransactionLog] ([TransactionLogId])
GO
ALTER TABLE [Auditing].[ContactDemographicChangeLog] CHECK CONSTRAINT [FK_ContactDemographicChangeLog_TransasctionLogId]
GO
