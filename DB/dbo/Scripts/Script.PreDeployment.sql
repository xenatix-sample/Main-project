/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to Reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Full Text Catalog needs to be created as a first step prior to creating tables. Please donot remove.
/****** Object:  FullTextCatalog [XenatixCatalog]  ******/

IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'XenatixCatalog')
	CREATE FULLTEXT CATALOG [XenatixCatalog] AS DEFAULT  


/*** Delete the Payor data to handle the dropping of PayorRank and ExpirationReason from Reference.Payor. This is just for Internal purpose and one time deal. ****/

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Registration' AND TABLE_NAME = N'ContactPayor'   )
	DELETE FROM Registration.ContactPayor 

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Registration' AND TABLE_NAME = N'PayorAddress'   )
	DELETE FROM Registration.PayorAddress 

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reference' AND TABLE_NAME = N'PayorPlan'    )
	DELETE FROM Reference.PayorPlan 

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reference' AND TABLE_NAME = N'Payor'    )
	DELETE FROM Reference.Payor 
GO