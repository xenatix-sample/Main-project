-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetMedicalConditionsByMedicalHistoryID]
-- Author:		Scott Martin
-- Date:		12/4/2015
--
-- Purpose:		Get Medical Condition Data including 'Other' data entered
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/4/2015	Scott Martin	Initial creation.
-- 12/8/2015	Scott Martin	Refactored query to include MedicalHistoryCondition ID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetMedicalConditionsByMedicalHistoryID]
	@MedicalHistoryID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		MC.[MedicalConditionID],
		[MedicalCondition],
		MHC.MedicalHistoryConditionID,
		isnull(MHC.HasCondition,0) as HasCondition,
		MC.[IsSystem],
		MC.[IsActive],
		MC.[ModifiedBy],
		MC.[ModifiedOn]
	FROM
		[Clinical].[MedicalCondition] MC
		LEFT OUTER JOIN [Clinical].[MedicalHistoryCondition] MHC
			ON MC.MedicalConditionID = MHC.MedicalConditionID
			AND MHC.IsActive = 1
			AND MHC.MedicalHistoryID = @MedicalHistoryID
	WHERE
		MC.IsActive = 1
		AND MC.IsSystem = 1
	UNION
	SELECT
		MC.[MedicalConditionID],
		[MedicalCondition],
		MHC.MedicalHistoryConditionID,
		isnull(MHC.HasCondition,0) as HasCondition,
		MC.[IsSystem],
		MC.[IsActive],
		MC.[ModifiedBy],
		MC.[ModifiedOn]
	FROM
		[Clinical].[MedicalCondition] MC
		INNER JOIN [Clinical].[MedicalHistoryCondition] MHC
			ON MC.MedicalConditionID = MHC.MedicalConditionID
	WHERE
		MHC.IsActive = 1
		AND MHC.MedicalHistoryID = @MedicalHistoryID
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO