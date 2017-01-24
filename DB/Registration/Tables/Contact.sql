-----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[Contact]
-- Author:		Saurabh Sahu
-- Date:		07/29/2015
--
-- Purpose:		Store Contact Data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/04/2015	Sumana Sangapu	844 - Added IsPregnant column
-- 08/05/2015	Saurabh Sahu		1) Added SchoolDistrict 2) Modify DriversLicense to AlternateID  .
-- 08/13/2015	Sumana Sangapu		Task #: 1227 - Refactor schema
-- 08/19/2015	Sumana Sangapu		1514 - Added SuffixID and TitleID
-- 08/27/2015	Arun Choudhary		Added a new column for Driver License and removed [SmokingStatusID], [FullCodeDNR] and [SchoolDistrictID] 
-- 09/03/2015	Rajiv Ranjan		Removed marital Status & age field
-- 09/08/2015	Rajiv Ranjan		Adedd DriverLicenseStateID & ClientIdentifierTypeID
-- 10/28/2015	Sumana Sangapu		Added GestationalAge
-- 12/22/2015	Scott Martin		Added PreferredGender
-- 12/23/2015	Scott Martin		Removed AlternateID and ClientIndentifierTypeID
-- 01/13/2016	Scott Martin		Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 01/18/2016   John Crossen        Add indexes
-- 02/26/2016	Kyle Campbell		Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/08/2016	Kyle Campbell		Renamed "Alias" column to "PreferredName"
-- 03/15/2016	Sumana Sangapu		Added SSNEncrypted and SearchableFields PERSISTED columns
-- 08/26/2016	Scott Martin	Added index
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[Contact] (
    [ContactID]                  BIGINT            IDENTITY (1, 1) NOT NULL,
	[MRN]						 BIGINT	NULL,
	[MPI]						 NVARCHAR(200)	NULL,
    [ContactTypeID]              INT            NULL,
    [ClientTypeID]               INT            NULL,
    [FirstName]                  NVARCHAR (200) NULL,
    [Middle]                     NVARCHAR (200) NULL,
    [LastName]                   NVARCHAR (200) NULL,
    [SuffixID]                   INT  NULL,
    [GenderID]                   INT            NULL,
	[PreferredGenderID]          INT            NULL,
    [TitleID]                    INT   NULL,
    [SequesteredByID]            INT            NULL,
    [DOB]                        DATE       NULL,
    [DOBStatusID]                INT            NULL,
    [SSN]                        NVARCHAR (9)   NULL,
    [SSNStatusID]                INT            NULL,
    [DriverLicense]				 NVARCHAR (50)  NULL,
	[DriverLicenseStateID]       INT            NULL,
    [PreferredName]              NVARCHAR (200) NULL,
	[IsDeceased] [bit] NULL,
    [DeceasedDate]               DATETIME       NULL,
	[CauseOfDeath] [int] NULL,
    [PreferredContactMethodID]   INT            NULL,
    [ReferralSourceID]           INT            NULL,    
	[IsPregnant]				 BIT			NULL DEFAULT(0),
	[GestationalAge]			 DECIMAL(3, 1)	NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SSNEncrypted] varbinary(2000) NULL,
	[SearchableFields]  AS (((((((coalesce(isnull([FirstName],'')+':','')+coalesce(isnull([Middle],'')+':',''))+coalesce(isnull([LastName],'')+':',''))+coalesce(isnull(CONVERT([nvarchar](10),[MRN]),'')+': ',''))+coalesce(CONVERT([varchar](10),[DOB],(101))+':',''))+coalesce(isnull([SSN],'')+':',''))+coalesce(isnull([DriverLicense],'')+':',''))+coalesce(isnull([PreferredName],'')+':','')) PERSISTED
    CONSTRAINT [PK_Contact_ContactID]				PRIMARY KEY CLUSTERED ([ContactID] ASC),
    CONSTRAINT [FK_Contact_ClientTypeID]					FOREIGN KEY ([ClientTypeID]) REFERENCES [Reference].[ClientType] ([ClientTypeID]),
    CONSTRAINT [FK_Contact_ContactMethodID]				FOREIGN KEY ([PreferredContactMethodID]) REFERENCES [Reference].[ContactMethod] ([ContactMethodID]),
    CONSTRAINT [FK_Contact_ContactTypeID]					FOREIGN KEY ([ContactTypeID]) REFERENCES [Reference].[ContactType] ([ContactTypeID]),
    CONSTRAINT [FK_Contact_DOBStatusID]					FOREIGN KEY ([DOBStatusID]) REFERENCES [Reference].[DOBStatus] ([DOBStatusID]),
    CONSTRAINT [FK_Contact_ReferralSourceID]				FOREIGN KEY ([ReferralSourceID]) REFERENCES [Reference].[ReferralSource] ([ReferralSourceID]),
    CONSTRAINT [FK_Contact_SSNStatusID]					FOREIGN KEY ([SSNStatusID]) REFERENCES [Reference].[SSNStatus] ([SSNStatusID]),
	CONSTRAINT [FK_Contact_SuffixID]					FOREIGN KEY ([SuffixID]) REFERENCES [Reference].[Suffix] ([SuffixID]),
	CONSTRAINT [FK_Contact_TitleID]					FOREIGN KEY ([TitleID]) REFERENCES [Reference].[Title] ([TitleID])
);

GO

CREATE INDEX [IX_Contact_IsActive] ON [Registration].[Contact] ([IsActive]) INCLUDE ([ContactID], [ContactTypeID])
GO

CREATE NONCLUSTERED INDEX [IX_Contact_ContactID_SuffixID] ON [Registration].[Contact]
(
	[SuffixID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_Contact_ContactTypeID_MRN] ON [Registration].[Contact]
(
	[ContactTypeID] ASC,
	[MRN] ASC
)
INCLUDE ( 	[FirstName],
	[Middle],
	[LastName]
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_Contact_GenderID] ON [Registration].[Contact]
(
	[GenderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_Contact_SystemCreatedOn] ON [Registration].[Contact]
(
	[SystemCreatedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_Contact_SystemModifiedOn] ON [Registration].[Contact]
(
	[SystemModifiedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Registration.Contact WITH CHECK ADD CONSTRAINT [FK_Contact_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.Contact CHECK CONSTRAINT [FK_Contact_UserModifedBy]
GO
ALTER TABLE Registration.Contact WITH CHECK ADD CONSTRAINT [FK_Contact_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.Contact CHECK CONSTRAINT [FK_Contact_UserCreatedBy]
GO

/****** full text index on [Registration].[Contact]  ******/
CREATE FULLTEXT STOPLIST [EmptyStopList] ;
GO
--IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[Registration].[Contact]'))
	CREATE FULLTEXT INDEX on Registration.Contact
	(SearchableFields) KEY index PK_Contact_ContactID WITH STOPLIST = [EmptyStopList], CHANGE_TRACKING = AUTO;
	GO








