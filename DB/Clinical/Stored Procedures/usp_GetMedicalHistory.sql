-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetMedicalHistory]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Get Medical History Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015	Scott Martin	Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetMedicalHistory]
	@MedicalHistoryID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[MedicalHistoryID],
		[EncounterID],
		[ContactID],
		[TakenBy],
		[TakenTime],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Clinical].[MedicalHistory]
	WHERE
		MedicalHistoryID = @MedicalHistoryID
		AND IsActive=1
	ORDER BY TakenTime desc
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


