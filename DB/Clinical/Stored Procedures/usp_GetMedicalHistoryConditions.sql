-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetMedicalHistoryCondition]
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
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetMedicalHistoryConditions]
	@MedicalHistoryID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[MedicalHistoryConditionID],
		[MedicalHistoryID],
		[MedicalConditionID],
		[HasCondition],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[MedicalHistoryCondition]
	WHERE
		MedicalHistoryID = @MedicalHistoryID
		AND IsActive=1
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END