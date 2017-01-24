-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetLetterOutcome]
-- Author:		Scott Martin
-- Date:		06/08/2016
--
-- Purpose:		Get Letter Outcome lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	 Scott Martin  Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetLetterOutcome]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[LetterOutcomeID], [LetterOutcome] 
		FROM		[Reference].[LetterOutcome]
		WHERE		IsActive = 1
		ORDER BY	[LetterOutcome]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END