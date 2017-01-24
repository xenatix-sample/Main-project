-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactEmails]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Get Contact Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/17/2015 - Rajiv Ranjan	Added ContactID, EmailID fields
-- 08/19/2015 - Rajiv Ranjan	Changed LEFT join to INNER join for ContactEmail
-- 08/24/2015 - Gurpreet Singh - Added ContactTypeID parameter to generalize SP
-- 09/03/2015 - Gurpreet Singh - Added IsActive check, Check contactTypeID from ContactRelationType table
-- 10/08/2015 - Satish Singh - Included IsPrimary in select statement
-- 10/09/2015 - Satish Singh - Check IsActive=1 in ContactEmail
-- 10/31/2015 - Arun Choudhary - Added modifiedon in select. Needed in tiles flyout.
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 06/02/2016	Gurmant Singh	Added EffectiveDate and ExpirationDate
-- 09/08/2016	Scott Martin	Added DISTINCT to query because join on ContactRelationship was causing duplicates
-- 10/27/2016	Scott Martin	Refactored query to improve performance
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetContactEmails]
	@ContactID BIGINT,
	@ContactTypeID INT = 0,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT
			c.ContactID,
			ce.ContactEmailID,
			e.EmailID,
			e.Email,
			ce.EmailPermissionID,
			ce.IsPrimary,
			ce.EffectiveDate,
			ce.ExpirationDate,
			ce.IsActive,
			ce.ModifiedOn
		FROM
			[Registration].[Contact] c
			INNER JOIN Registration.ContactEmail ce
				ON ce.ContactID = c.ContactID
				and ce.IsActive=1
			LEFT OUTER JOIN Core.Email e
				ON e.EmailID = ce.EmailID
		WHERE 
			c.[IsActive] = 1
			AND c.ContactID = @ContactID
			AND c.[ContactTypeID] = @ContactTypeID
		UNION	
		SELECT
			c.ContactID,
			ce.ContactEmailID,
			e.EmailID,
			e.Email,
			ce.EmailPermissionID,
			ce.IsPrimary,
			ce.EffectiveDate,
			ce.ExpirationDate,
			ce.IsActive,
			ce.ModifiedOn
		FROM
			[Registration].[ContactRelationship] AS cr
			INNER JOIN [Registration].[Contact] c
				ON cr.[ChildContactID] = c.[ContactID]
				AND c.[IsActive] = 1
			INNER JOIN Registration.ContactEmail ce
				ON ce.ContactID = c.ContactID
				and ce.IsActive=1
			LEFT OUTER JOIN Core.Email e
				ON e.EmailID = ce.EmailID
		WHERE 
			cr.[IsActive] = 1
			AND cr.ParentContactID = @ContactID
			AND cr.[ContactTypeID] = @ContactTypeID

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END