
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactPhones]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Update Contact Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/11/2015 - Saurabh Sahu -- done changes for altenate phone functionality
-- 08/14/2015	Sumana Sangapu	1227 Refactor Schema for ContactMenthods
-- 08/15/2015	Rajiv Ranjan	Removed case for PreferredPhone, PreferredExtension and also left join for preferred & alternate phone
-- 08/19/2015 - Rajiv Ranjan	Changed LEFT join to INNER join for ContactPhone table
-- 08/24/2015 - Gurpreet Singh - Added ContactTypeID parameter to generalize
-- 09/03/2015 - Gurpreet Singh - Added IsActive check, Check contactTypeID from ContactRelationType table
-- 10/09/2015 - Avikal		   - 2297 : Phone Screen : Added condition to show only active contact phones
-- 10/31/2015 - Arun Choudhary - Added modifiedon in select. Needed in tiles flyout.
-- 12/23/2015 -	Arun Choudhary - Added IsActive in select.
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 06/02/2016	Gurmant Singh	Added EffectiveDate and ExpirationDate
-- 09/08/2016	Scott Martin	Added DISTINCT to query because join on ContactRelationship was causing duplicates
-- 10/27/2016	Scott Martin	Refactored query to improve performance
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetContactPhones]
	@ContactID BIGINT,
	@ContactTypeID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'
	
	BEGIN TRY
		SELECT
			cp.ContactPhoneID,
			c.ContactID,
			p.PhoneID,
			p.PhoneTypeID,
			p.Number,
			p.Extension,			
			cp.PhonePermissionID,
			cp.IsPrimary,
			cp.EffectiveDate,
			cp.ExpirationDate,
			cp.ModifiedOn,
			cp.IsActive
		FROM 
			[Registration].[Contact] c
			INNER JOIN Registration.ContactPhone cp
				ON cp.ContactID = c.ContactID 
			LEFT OUTER JOIN Core.Phone p
				ON p.PhoneID = cp.PhoneID
		WHERE 
			c.[IsActive] = 1 
			AND cp.[IsActive]=1
			AND c.ContactID = @ContactID
			AND c.[ContactTypeID] = @ContactTypeID
		UNION
		SELECT 
			cp.ContactPhoneID,
			c.ContactID,
			p.PhoneID,
			p.PhoneTypeID,
			p.Number,
			p.Extension,			
			cp.PhonePermissionID,
			cp.IsPrimary,
			cp.EffectiveDate,
			cp.ExpirationDate,
			cp.ModifiedOn,
			cp.IsActive
		FROM
			Registration.ContactRelationship cr
			INNER JOIN Registration.Contact c
				ON cr.ChildContactID = c.ContactID
				AND c.IsActive = 1
			INNER JOIN Registration.ContactPhone cp
				ON c.ContactID = cp.ContactID
			LEFT OUTER JOIN Core.Phone p
				ON cp.PhoneID = p.PhoneID
		WHERE
			cr.IsActive = 1
			AND cp.IsActive = 1
			AND cr.ParentContactID = @ContactID
			AND cr.[ContactTypeID] = @ContactTypeID
		GROUP BY
			cp.ContactPhoneID,
			c.ContactID,
			p.PhoneID,
			p.PhoneTypeID,
			p.Number,
			p.Extension,			
			cp.PhonePermissionID,
			cp.IsPrimary,
			cp.EffectiveDate,
			cp.ExpirationDate,
			cp.ModifiedOn,
			cp.IsActive
		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END