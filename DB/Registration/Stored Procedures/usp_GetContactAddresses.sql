-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactAddresses]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Get Contact Address Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification.0
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/14/2015	Sumana Sangapu 1227 Refactor schema for ContactMethods
-- 08/15/2015	Rajiv Ranjan	Added ContactID into select
-- 08/19/2015 - Rajiv Ranjan	Changed LEFT join to INNER join for ContactAddress table
-- 08/24/2015 - Gurpreet Singh - Added ContactTypeID parameter to generalize SP for all screens
-- 08/25/2015 - Rajiv Ranjan	Changed LEFT join to INNER join for ContactAddress table, this canges was reverted in last check in
-- 09/03/2015 - Gurpreet Singh - Added IsActive check, Check contactTypeID from ContactRelationType table
-- 09/22/2015   Gurpreet Singh      Changed AptComplexName to ComplexName
-- 10/08/2015	Arun Choudhary	- Get only active addresses
-- 10/31/2015 - Arun Choudhary - Added modifiedon in select. Needed in tiles flyout.
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 03/10/2016	Scott Martin	Removed ContactMethodPreferenceID
-- 09/08/2016	Scott Martin	Added DISTINCT to query because join on ContactRelationship was causing duplicates
-- 10/27/2016	Scott Martin	Refactored query to improve performance
----------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [Registration].[usp_GetContactAddresses]
	@ContactID BIGINT,
	@ContactTypeID INT = 1,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			c.[ContactID],
			ca.[ContactAddressID],
			a.[AddressID],
			at.[AddressTypeID],
			a.[Line1],
			a.[Line2], 
			a.[City],
			a.[StateProvince],
			a.[County],
			a.[Zip],
			a.[ComplexName],
			a.[GateCode],
			ca.[MailPermissionID],
			ca.[EffectiveDate],
			ca.[ExpirationDate],
			ca.[IsPrimary],
			ca.[ModifiedOn]
		FROM 
			[Registration].[Contact] c
			INNER JOIN [Registration].[ContactAddress] ca
				ON ca.ContactID = c.ContactID
				AND ca.[IsActive]=1
			LEFT OUTER JOIN [Core].[Addresses] a
				ON a.AddressID = ca.AddressID
			LEFT OUTER JOIN [Reference].[AddressType] at
				ON a.AddressTypeID = at.AddressTypeID
			LEFT OUTER JOIN [Reference].[ContactMethod] cm
				ON cm.ContactMethodID = c.PreferredContactMethodID
		WHERE
			c.[IsActive] = 1 
			AND c.ContactID = @ContactID
			AND c.[ContactTypeID] = @ContactTypeID
		UNION
		SELECT
			c.[ContactID],
			ca.[ContactAddressID],
			a.[AddressID],
			at.[AddressTypeID],
			a.[Line1],
			a.[Line2], 
			a.[City],
			a.[StateProvince],
			a.[County],
			a.[Zip],
			a.[ComplexName],
			a.[GateCode],
			ca.[MailPermissionID],
			ca.[EffectiveDate],
			ca.[ExpirationDate],
			ca.[IsPrimary],
			ca.[ModifiedOn]
		FROM 
			[Registration].[ContactRelationship] AS cr
			INNER JOIN [Registration].[Contact] c
				ON cr.[ChildContactID] = c.[ContactID]
				AND cr.[IsActive] = 1
			INNER JOIN [Registration].[ContactAddress] ca
				ON ca.ContactID = c.ContactID
				AND ca.[IsActive]=1
			LEFT OUTER JOIN [Core].[Addresses] a
				ON a.AddressID = ca.AddressID
			LEFT OUTER JOIN [Reference].[AddressType] at
				ON a.AddressTypeID = at.AddressTypeID
			LEFT OUTER JOIN [Reference].[ContactMethod] cm
				ON cm.ContactMethodID = c.PreferredContactMethodID
		WHERE
			c.[IsActive] = 1 
			AND cr.ParentContactID = @ContactID
			AND cr.[ContactTypeID] = @ContactTypeID
		GROUP BY
			c.[ContactID],
			ca.[ContactAddressID],
			a.[AddressID],
			at.[AddressTypeID],
			a.[Line1],
			a.[Line2], 
			a.[City],
			a.[StateProvince],
			a.[County],
			a.[Zip],
			a.[ComplexName],
			a.[GateCode],
			ca.[MailPermissionID],
			ca.[EffectiveDate],
			ca.[ExpirationDate],
			ca.[IsPrimary],
			ca.[ModifiedOn]
		ORDER BY
			ca.[EffectiveDate] DESC,
			ca.[ExpirationDate] DESC

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END