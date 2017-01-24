-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetMedicalHistoryConditionDetails]
-- Author:		Scott Martin
-- Date:		12/2/2015
--
-- Purpose:		Get Medical History Condition Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/2/2015	Scott Martin	Initial creation.
-- 12/4/2015	Scott Martin	Added family relationship details
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetMedicalHistoryConditionDetails]
	@MedicalHistoryConditionID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[MedicalHistoryConditionDetailID],
		[MedicalHistoryConditionID],
		FR.[FamilyRelationshipID],
		[IsSelf],
		[Comments],
		[RelationshipTypeID],
		[FirstName],
		[LastName],
		[IsDeceased],
		MHCD.[IsActive],
		MHCD.[ModifiedBy],
		MHCD.[ModifiedOn]
	FROM
		[Clinical].[MedicalHistoryConditionDetail] MHCD
		LEFT OUTER JOIN [Clinical].[FamilyRelationship] FR
			ON MHCD.FamilyRelationshipID = FR.FamilyRelationshipID
	WHERE
		MedicalHistoryConditionID = @MedicalHistoryConditionID
		AND MHCD.IsActive=1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END