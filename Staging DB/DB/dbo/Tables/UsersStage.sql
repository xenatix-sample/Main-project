-----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[UserStage]
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
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[UserStage] (
    [UserStageID]         INT              IDENTITY (1, 1) NOT NULL,
	[BatchID]		   BIGINT	NULL,
    [UserGUID]            VARBINARY (5000) NULL,
    [ManagerGUID]         VARBINARY (5000) NULL,
    [UserName]            NVARCHAR (100)   NULL,
    [FirstName]           NVARCHAR (50)    NULL,
    [LastName]            NVARCHAR (50)    NULL,
    [Phone]               NVARCHAR (15)    NULL,
    [Email]               NVARCHAR (255)   NULL,
    [AddressLine1]        NVARCHAR (255)   NULL,
    [AddressLine2]        NVARCHAR (255)   NULL,
    [City]                NVARCHAR (255)   NULL,
    [StateProvince]       NVARCHAR (255)   NULL,
    [ZipPostalCode]       NVARCHAR (15)    NULL,
    [CountryRegion]       NVARCHAR (255)   NULL,
    [IsTemporaryPassword] BIT              CONSTRAINT [DF_UserStage_IsTemporaryPassword] DEFAULT ((1)) NULL,
    [EffectiveToDate]     DATETIME         NULL,
    [LoginAttempts]       INT              NULL,
    [LoginCount]          INT              NULL,
    [LastLogin]           DATETIME         NULL,
    [IsActive]            BIT              CONSTRAINT [DF_UserStage_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]          INT              NOT NULL,
    [ModifiedOn]          DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]           INT              DEFAULT ((1)) NOT NULL,
    [CreatedOn]           DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]     DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn]    DATETIME         DEFAULT (getutcdate()) NOT NULL,
	[UserGUIDConverted] [nvarchar](500) NULL,
	[ManagerGUIDConverted] [nvarchar](500) NULL,
	[PhoneID] [int] NULL,
	[EmailID] [int] NULL,
	[AddressID] [int] NULL,
	[StateProvinceID] [int] NULL,
	[CountryID]	[int] NULL,
    CONSTRAINT [PK_UserStage_UserStageID] PRIMARY KEY CLUSTERED ([UserStageID] ASC)
);




GO
