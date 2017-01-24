-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetAllergyDetails]
-- Author:		John Crossen
-- Date:		11/13/2015
--
-- Purpose:		Get Contact Allergy by Contact ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	John Crossen	TFS# 3566 - Initial creation.
-- 11/20/2015   Justin Spalti - Removed References to allergy symptoms
-- 01/18/2016 -	Sumana Sangapu - List the Ingredients due to change of requirement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetAllergyDetails]
@ContactAllergyID BIGINT,
@AllergyTypeID SMALLINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	IF @AllergyTypeID=1
		BEGIN
			SELECT [ContactAllergyDetailID]
				  ,AD.[ContactAllergyID]
				  ,AD.[AllergyID]
				  ,AD.[SeverityID]		  
			  FROM [Clinical].[ContactAllergyDetail] AD
			  JOIN Clinical.ContactAllergy CA ON CA.ContactAllergyID = AD.ContactAllergyID
			  JOIN Clinical.Allergy A ON AD.AllergyID = A.AllergyID
			  JOIN Clinical.AllergySeverity SE ON SE.AllergySeverityID = AD.SeverityID
			  WHERE AD.ContactAllergyID = @ContactAllergyID
				AND AD.IsActive = 1
		END
	IF @AllergyTypeID IN (2,3)
		BEGIN
			SELECT [ContactAllergyDetailID]
				  ,AD.[ContactAllergyID]
				  ,AD.[AllergyID]
				  ,AD.[SeverityID]
				  ,A.IngredientID as DrugID
			  FROM [Clinical].[ContactAllergyDetail] AD
			  JOIN Clinical.ContactAllergy CA ON CA.ContactAllergyID = AD.ContactAllergyID
			  JOIN Clinical.DrugIngredientsList A ON AD.AllergyID = A.IngredientID
			  JOIN Clinical.AllergySeverity SE ON SE.AllergySeverityID = AD.SeverityID
			  WHERE AD.ContactAllergyID = @ContactAllergyID
				AND AD.IsActive = 1
		END
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END