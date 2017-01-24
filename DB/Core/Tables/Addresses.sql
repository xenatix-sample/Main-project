-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Address]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Store Address data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015   John Crossen        Add isVerified column
-- 08/25/2015   Rajiv Ranjan		Changed address type to nullable
-- 09/22/2015   Gurpreet Singh      Changed AptComplexName to ComplexName
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[Addresses] (
    [AddressID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [AddressTypeID]  INT            NULL,
    [Name]           NVARCHAR (200) NULL,
    [Line1]          NVARCHAR (200) NULL,
    [Line2]          NVARCHAR (200) NULL,
    [City]           NVARCHAR (200) NULL,
    [County]         INT			NULL,
    [StateProvince]  INT			NULL,
    [Zip]            NVARCHAR (10)  NULL,
    [Country]        NVARCHAR (100) NULL,
    [Lattitude]      DECIMAL (18)   NULL,
    [Longitude]      DECIMAL (18)   NULL,
    [ComplexName] NVARCHAR (255) NULL,
    [GateCode]       NVARCHAR (50)  NULL,
    [IsVerified] BIT NULL DEFAULT 0,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Addresses_AddressID]		PRIMARY KEY CLUSTERED ([AddressID] ASC),
    CONSTRAINT [FK_Addresses_AddressTypeID]		FOREIGN KEY ([AddressTypeID]) REFERENCES [Reference].[AddressType] ([AddressTypeID])
);

GO

ALTER TABLE Core.Addresses WITH CHECK ADD CONSTRAINT [FK_Addresses_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Addresses CHECK CONSTRAINT [FK_Addresses_UserModifedBy]
GO
ALTER TABLE Core.Addresses WITH CHECK ADD CONSTRAINT [FK_Addresses_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Addresses CHECK CONSTRAINT [FK_Addresses_UserCreatedBy]
GO

CREATE NONCLUSTERED INDEX [IX_Addresses_AddressID_County_StateProvince_AddressTypeID] ON [Core].[Addresses]
(
	[AddressID] ASC,
	[County] ASC,
	[StateProvince] ASC,
	[AddressTypeID] ASC
)
INCLUDE ( 	[Line1],
	[Line2],
	[City],
	[Zip],
	[ComplexName],
	[GateCode]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO