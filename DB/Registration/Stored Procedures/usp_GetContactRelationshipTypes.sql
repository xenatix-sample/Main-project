-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactRelationshipTypes]
-- Author:		Lokesh Singhal
-- Date:		06/08/2016
--
-- Purpose:		Gets a list of contact races
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Lokesh Singhal		Initial creation.
-- 09/14/2016   Arun Choudhary		Added EffectiveDate and ExpirationDate
-- 01/18/2017	Sumana Sangapu		Added ReferralHeaderID as parameter
-- 01/19/2017	Gurpreet Singh		Added LivingWithClientStatus in output
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactRelationshipTypes]
	@ContactID BIGINT,
	@ParentContactID BIGINT,
	@ReferralHeaderID BIGINT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'
	DECLARE @ContactRelationshipID BIGINT;

	BEGIN TRY

		IF @ReferralHeaderID IS NULL
		BEGIN
			SELECT @ContactRelationshipID = ContactRelationshipID FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID;
		END 
		BEGIN
			SELECT @ContactRelationshipID = ContactRelationshipID FROM Registration.ContactRelationship WHERE ParentContactID = @ParentContactID AND ChildContactID = @ContactID AND ReferralHeaderID = @ReferralHeaderID;
		END 

		SELECT DISTINCT
			CRT.[ContactRelationshipTypeID],
			CR.[ContactRelationshipID],
			CR.[ChildContactID] AS[ContactID],
			CR.[ParentContactID],
			CRT.[RelationshipTypeID],
			CRT.[IsPolicyHolder],
			CRT.[OtherRelationship],
			CT.[RelationshipGroupID],
			CRT.[EffectiveDate],
			CRT.[ExpirationDate],
			CR.[LivingWithClientStatus],
			CR.[IsActive],
			CR.[ModifiedBy],
			CR.[ModifiedOn]
		FROM 
			Registration.ContactRelationshipType CRT
			INNER JOIN Registration.ContactRelationship CR
				ON CRT.ContactRelationshipID = CR.ContactRelationshipID
			INNER JOIN Reference.RelationshipGroupDetails RGD
				ON CRT.RelationshipTypeID = RGD.RelationshipTypeID
			INNER JOIN Reference.CollateralType CT
				ON CT.RelationshipGroupID = RGD.RelationshipGroupID
		WHERE 
			CRT.ContactRelationshipID = @ContactRelationshipID	
			AND CRT.IsActive = 1 		
	    ORDER BY 
			CR.[ContactRelationshipID] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END