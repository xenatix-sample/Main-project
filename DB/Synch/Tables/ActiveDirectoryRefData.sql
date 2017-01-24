-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[ActiveDirectoryRefData]
-- Author:		Rahul Vats
-- Date:		
--
-- Purpose:		Store the data imported from the AD
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- MM/DD/YYYY	Author			Description
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-- 09/09/2016	Sumana Sangapu	Chagned the ErrorMessage length to max
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[ActiveDirectoryRefData](
	objectguid varchar(max) null,
	manager varchar(max) null,
	samaccountname varchar(max) null,
	employeeID varchar(max) null,
	employeeNumber varchar(max) null,
	givenName varchar(max) null,
	middleName varchar(max) null,
	sn varchar(max) null,
	initials varchar(max) null,
	telephoneNumber varchar(max) null,
	mail varchar(max) null,
	streetAddress varchar(max) null,
	l varchar(max) null,
	st varchar(max) null,
	postalCode varchar(max) null,
	co varchar(max) null,
	userAccountControl varchar(max) null,
	distinguishedname varchar(max) null,
	userprincipalname varchar(max) null,
	pager varchar(max) null,
	homePhone varchar(max) null,
	division varchar(max) null,
	company varchar(max) null,
	displayName varchar(max) null,
	facsimileTelephoneNumber varchar(max) null,
	mobile varchar(15) null,
	physicalDeliveryOfficeName varchar(100) null,
	department varchar(max) null,
	title varchar(max) null,
	ErrorMessage varchar(max)
) ON [PRIMARY]
GO