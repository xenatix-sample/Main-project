-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[RecordHeaderDetails]
-- Author:		Sumana Sangapu
-- Date:		01/11/2017
--
-- Purpose:		This table will hold the snapshot of the Print Header data elements
--
-- Notes:		n/a 
--
-- Depends:		n/a 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/11/2017   Sumana Sangapu	- Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[RecordHeaderDetails](
	[RecordHeaderDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[RecordHeaderID] [bigint] NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[MRN] [bigint] NULL,
	[FirstName] [nvarchar](200) NULL,
	[Middle] [nvarchar](200) NULL,
	[LastName] [nvarchar](200) NULL,
	[SuffixID] [int] NULL,
	[DOB] [date] NULL,
	[MedicaidID] [nvarchar](50) NULL,
	[SSNEncrypted] [varbinary](2000) NULL,
	[OrganizationMappingID] [bigint] NULL,
	[SourceHeaderID] [bigint] NULL,
	[Age] [int] NULL,
	[RaceID] [nvarchar](25) NULL,
	[EthnicityID] [int] NULL,
	[PhoneTypeID] [int] NULL,
	[Number] [nvarchar](50) NULL,
	[Extension] [nvarchar](10) NULL,
	[AddressTypeID] [int] NULL,
	[Line1] [nvarchar](200) NULL,
	[Line2] [nvarchar](200) NULL,
	[City] [nvarchar](200) NULL,
	[County] [int] NULL,
	[StateProvince] [int] NULL,
	[Zip] [nvarchar](50) NULL,
	[Country] [nvarchar](100) NULL,
	[ComplexName] [nvarchar](255) NULL,
	[GateCode] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL  DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_RecordHeaderDetails_RecordHeaderDetailID] PRIMARY KEY CLUSTERED  ([RecordHeaderDetailID] ASC)
)
ON [PRIMARY]
GO
ALTER TABLE [Core].[RecordHeaderDetails] WITH CHECK ADD CONSTRAINT [FK_RecordHeaderDetails_RecordHeaderID] FOREIGN KEY ([RecordHeaderID]) REFERENCES Core.RecordHeader ([RecordHeaderID])
GO
ALTER TABLE [Core].[RecordHeaderDetails] CHECK CONSTRAINT [FK_RecordHeaderDetails_RecordHeaderID]
GO
ALTER TABLE [Core].[RecordHeaderDetails] WITH CHECK ADD CONSTRAINT [FK_RecordHeaderDetails_ContactID] FOREIGN KEY ([ContactID]) REFERENCES Registration.Contact ([ContactID])
GO
ALTER TABLE [Core].[RecordHeaderDetails] CHECK CONSTRAINT [FK_RecordHeaderDetails_ContactID]
GO
ALTER TABLE [Core].[RecordHeaderDetails] WITH CHECK ADD CONSTRAINT [FK_RecordHeaderDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[RecordHeaderDetails] CHECK CONSTRAINT [FK_RecordHeaderDetails_UserModifedBy]
GO
ALTER TABLE [Core].[RecordHeaderDetails] WITH CHECK ADD CONSTRAINT [FK_RecordHeaderDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[RecordHeaderDetails] CHECK CONSTRAINT [FK_RecordHeaderDetails_UserCreatedBy]
GO 


