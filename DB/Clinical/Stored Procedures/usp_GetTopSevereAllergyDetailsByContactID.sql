-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetTopSevereAllergyDetailsByContactID]
-- Author:		Scott Martin
-- Date:		12/7/2015
--
-- Purpose:		Get top 5 most severe allergies by Contact ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/7/2015	Scott Martin	Initial creation.
-- 12/7/2015	Scott Martin	Modified the query to get the most recent contact allergy bundle
-- 12/7/2015	Scott Martin	Modified the query to get most recent allergy bundle for each AllergyType and return
--								the top 5 based on all AllergyTypes
-- 12/8/2015    Jason Smith     Added support to show No known allergies or Allergies not provided 
-- 01/14/2016   Lokesh Singhal  Show allergy name from drug table in case allergytype is drug allergies or drug intolerances
-- 01/14/2016   Lokesh Singhal  Remove allergy from header when saved allergy is removed
-- 03/14/2016	Scott Martin	Removed Top 5/Order by for allerty cte
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Clinical].[usp_GetTopSevereAllergyDetailsByContactID]
	@ContactID BIGINT
	,@ResultCode INT OUTPUT
	,@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';
		
		WITH Bundles (ContactID, AllergyTypeID, ContactAllergyID)
		AS
		(
			SELECT
				ContactID,
				AllergyTypeID,
				MAX(ContactAllergyID)
			FROM
				Clinical.ContactAllergy
			WHERE
				IsActive = 1
				AND ContactID = @ContactID			
			GROUP BY
				ContactID,
				AllergyTypeID
		), top5 as
		(
			SELECT
				CA.ContactAllergyID,
				AD.ContactAllergyDetailID,
				AD.AllergyID,
				case CA.AllergyTypeID when 1 then A.AllergyName else D.IngredientName end AS AllergyName,
				CA.AllergyTypeID,
				ASV.AllergySeverityID,
				ASV.AllergySeverity,
				ASV.SortOrder
			FROM
				Bundles CA
				INNER JOIN [Clinical].[ContactAllergyDetail] AD
					ON CA.ContactAllergyID = AD.ContactAllergyID
				LEFT OUTER JOIN Clinical.Allergy A
					ON AD.AllergyID = A.AllergyID
				LEFT OUTER JOIN Clinical.DrugIngredientsList D
					ON AD.AllergyID = D.IngredientID
				INNER JOIN Clinical.AllergySeverity ASV
					ON AD.SeverityID = ASV.AllergySeverityID
					WHERE AD.IsActive = 1
		), kna as
		(
			select distinct
				0 as ContactAllergyID,
				0 as ContactAllergyDetailID,
				0 as AllergyID,
				'No known allergies' AS AllergyName,
				0 as AllergyTypeID,
				0 as AllergySeverityID,
				'' as AllergySeverity,
				0 as SortOrder
			from bundles b
			--left join Clinical.ContactAllergy ca on ca.ContactAllergyID = b.ContactAllergyID and ca.NoKnownAllergy = 1
			where (select count(*) from top5) = 0 and not exists(select * from Clinical.ContactAllergy where ContactAllergyID = b.ContactAllergyID and NoKnownAllergy = 0)
		)
		-- show the top 5 allergies if at least one active bundle is not marked as no known allergies
		select * from top5
		-- show the no known allergies if all active bundles are marked
		union select * from kna
		-- otherwise, show allergies have not been provided (no data collected)
		union select
			0 as ContactAllergyID,
			0 as ContactAllergyDetailID,
			0 as AllergyID,
			'Allergies have not been provided' AS AllergyName,
			0 as AllergyTypeID,
			0 as AllergySeverityID,
			'' as AllergySeverity,
			0 as SortOrder
			where not exists(select * from top5 union select * from kna)
		order by
			SortOrder desc
			,AllergyName
		;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END