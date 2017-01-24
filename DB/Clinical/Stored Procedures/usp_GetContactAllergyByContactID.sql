-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetContactAllergyByContactID]
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
-- 11/20/2015   Justin Spalti - Updated the logic to grab the most recent bundle
-- 02/11/2016   Justin Spalti - Added SystemModifiedOn as a secondary value in the order by clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetContactAllergyByContactID]
@ContactID BIGINT,
@AllergyTypeID SMALLINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		--We are only getting the most recent and active bundle of allergies
		SELECT TOP 1 [ContactAllergyID]
					,[EncounterID]
					,[ContactID]
					,[AllergyTypeID]
					,[NoKnownAllergy]
					,[TakenBy]
					,[TakenTime]
					,[IsActive]
					,[ModifiedBy]
					,[ModifiedOn]
		FROM [Clinical].[ContactAllergy]
		WHERE ContactID = @ContactID 
			AND AllergyTypeID = @AllergyTypeID
			AND IsActive = 1
		ORDER BY TakenTime DESC, SystemModifiedOn DESC
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END