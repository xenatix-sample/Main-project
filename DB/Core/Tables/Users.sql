-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Users]
-- Author:		Rajiv Ranjan
-- Date:		
--
-- Purpose:		Store the user details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Rajiv Ranjan	Initial creation.
-- 07/30/2015	Suresh Pandey	TFS# - changed modified by and modified on not null
-- 07/30/2015   John Crossen    Change schema from dbo to Core, rename table to users
-- 10/02/2015   Justin Spalti   Added the IsTemporaryPassword column and default value
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/24/2016   Justin Spalti   Added the GenderID column with a default value of unknown along with a FK to the Gender table
-- 02/26/2016	Scott Martin	Added DOB column
-- 03/01/2016	Sumana Sangapu	Added UserGUID and ADFlag columns
-- 03/22/2016	Sumana Sangapu	Added PrintSignature and DigitalPassword columns
-- 04/06/2016	Sumana Sangapu	Added IsInternal column
-- 07/21/2016	RAV - Reviewed The Query http://sqlmag.com/sql-server-2000/designing-performance-null-or-not-null
-- 09/13/2016	Rahul Vats		Reviewed the Table and made the design change to ADFlag
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[Users] (
    [UserID]              INT              IDENTITY (1, 1) NOT NULL,
    [UserName]            NVARCHAR (100)   NULL,
    [FirstName]           NVARCHAR (50)    NULL,
    [MiddleName]          NVARCHAR (50)    NULL,
    [LastName]            NVARCHAR (50)    NULL,
    [GenderID]            INT              NULL,
    [DOB]                 DATE             NULL,
    [Password]            NVARCHAR (255)   NULL,
    [IsTemporaryPassword] BIT              CONSTRAINT [DF_Users_IsTemporaryPassword] DEFAULT ((1)) NOT NULL,
    [EffectiveToDate]     DATETIME         NULL,
    [LoginAttempts]       INT              NULL,
    [LoginCount]          INT              NULL,
    [LastLogin]           DATETIME         NULL,
    [UserGUID]            NVARCHAR (500)   NULL,
    [ADFlag]              BIT              NOT NULL DEFAULT ((0)),
    [PrintSignature]      NVARCHAR (100)   NULL,
    [DigitalPassword]     VARBINARY (2000) NULL,
    [IsInternal]          BIT              NULL,
    [IsActive]            BIT              CONSTRAINT [DF_Users_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]          INT              NOT NULL,
    [ModifiedOn]          DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]           INT              DEFAULT ((1)) NOT NULL,
    [CreatedOn]           DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]     DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn]    DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [EffectiveFromDate]   DATETIME         NULL,
    CONSTRAINT [PK_User_UserID] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_Gender_GenderID] FOREIGN KEY ([GenderID]) REFERENCES [Reference].[Gender] ([GenderID]),
    CONSTRAINT [IX_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
)
GO

CREATE NONCLUSTERED INDEX [IX_Users_IsActive_UserName_UserID] ON [Core].[Users]
(
	[IsActive] ASC,
	[UserName] ASC,
	[UserID] ASC
)
INCLUDE ( 	[FirstName],
	[LastName],
	[Password],
	[EffectiveToDate],
	[LoginAttempts],
	[LoginCount],
	[LastLogin],
	[UserGUID],
	[ModifiedBy],
	[ModifiedOn]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO