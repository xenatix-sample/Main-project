-----------------------------------------------------------------------------------------------------------------------
-- Script:		Script to create full text indexes
-- Author:		Suresh Pandey
-- Date:		08/12/2015
--
-- Purpose:		Require full text indexes on Address, Gender, County, StateProvince and Contact tables for client search screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Address, Gender, County, StateProvince and Contact tables
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	Suresh Pandey	Initial creation.
-- 08/20/2015	Suresh Pandey	Removed prefix and email from fulltext column
-- 08/30/2015	Suresh Pandey	Added DriverLicense in Registration.Contact table for fulltext search
-- 09/22/2015   Gurpreet Singh      Changed AptComplexName to ComplexName
-- 10/09/2015   Arun Choudhary	Removed MRN from full text search as datatype is changed to bigint.
-----------------------------------------------------------------------------------------------------------------------


/****** Object:  FullTextCatalog [XenatixCatalog]  ******/

--IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'XenatixCatalog')
--	CREATE FULLTEXT CATALOG [XenatixCatalog]WITH ACCENT_SENSITIVITY = ON


/****** full text index on [Core].[Addresses]  ******/
 
IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[Core].[Addresses]'))
	CREATE FULLTEXT INDEX on Core.Addresses
	(ComplexName,City,Country,GateCode,Line1,Line2,Name,Zip) KEY index PK_Addresses_AddressID ON XenatixCatalog

/****** full text index on [Reference].[County]  ******/

IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[Reference].[County]'))
	CREATE FULLTEXT INDEX on Reference.County
	(CountyName) KEY index PK_County_CountyID ON XenatixCatalog


/****** full text index on [Reference].[Gender]  ******/

IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[Reference].[Gender]'))
	CREATE FULLTEXT INDEX on Reference.Gender
	(Gender) KEY index PK_Gender_GenderID ON XenatixCatalog 

/****** full text index on [Reference].[StateProvince]  ******/

IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[Reference].[StateProvince]'))
	CREATE FULLTEXT INDEX on Reference.StateProvince
	(StateProvinceCode,StateProvinceName) KEY index PK_StateProvince_StateProvinceID ON XenatixCatalog 
