-----------------------------------------------------------------------------------------------------------------------
-- Table:		Synch.[ADUserStage]
-- Author:		Chad Roberts
-- Date:		
--
-- Purpose:		Staging table to hold the users from active directory 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/29/2016	Chad Roberts	Initial creation. 
-- 03/03/2016	Sumana Sangapu	Added additional columns
-- 06/09/2016	Sumana Sangapu  TFS#11411 - Refactor SSIS to move Staging tables to Synch schema
-- 09/03/2016	Rahul Vats		Review the table and adding the missing fields. We will still have to remove not required fields or add any additional that might be needed which we may have overlooked.
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[ADUserStage] (
    [UserStageID] INT IDENTITY (1, 1) NOT NULL,
	[BatchID] BIGINT NULL,
    [UserName] NVARCHAR (100) NULL,
    [FirstName] NVARCHAR (50) NULL,
    [LastName] NVARCHAR (50) NULL,
	[MiddleName] NVARCHAR (50) NULL,
	[Initials] NVARCHAR (50) NULL,
    [Phone] NVARCHAR (15) NULL,
    [Email] NVARCHAR (255) NULL,
    [AddressLine1] NVARCHAR (255) NULL,
    [AddressLine2] NVARCHAR (255) NULL,
    [City] NVARCHAR (255) NULL,
    [StateProvince] NVARCHAR (255) NULL,
    [ZipPostalCode] NVARCHAR (15) NULL,
    [CountryRegion] NVARCHAR (255) NULL,
    [IsTemporaryPassword] BIT CONSTRAINT [DF_ADUserStage_IsTemporaryPassword] DEFAULT ((1)) NULL,
    [EffectiveFromDate] DATETIME NULL,
	[EffectiveToDate] DATETIME NULL,
	[ExpirationDate] DATETIME  NULL,
	[UserIdentifierTypeID_1] INT NULL DEFAULT ((1)),
	[UserIdentifier_1] NVARCHAR(50),
	[UserIdentifierTypeID_2] INT NULL DEFAULT ((8)),
	[UserIdentifier_2] NVARCHAR(50) ,
    [LoginAttempts] INT NULL,
    [LoginCount] INT NULL,
    [LastLogin] DATETIME NULL,
    [IsActive] BIT CONSTRAINT [DF_ADUserStage_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy] INT DEFAULT ((1)) NOT NULL,
    [CreatedOn] DATETIME DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn] DATETIME DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME DEFAULT (getutcdate()) NOT NULL,
	[UserGUID] [nvarchar](500) NULL,
	[Manager] [nvarchar](500) NULL,
	[UserID] [int] NULL,
	[PhoneID] [int] NULL,
	[EmailID] [int] NULL,
	[AddressID] [int] NULL,
	[AddressTypeID] [int] NULL DEFAULT ((1)),
	[PhoneTypeID] [int] NULL DEFAULT ((1)),
	[StateProvinceID] [int] NULL,
	[CountryID]	[int] NULL,
    [IsDuplicateUser] BIT NULL DEFAULT ((0)),
	[ErrorMessage] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_ADUserStage_UserStageID] PRIMARY KEY CLUSTERED ([UserStageID] ASC)
)
GO