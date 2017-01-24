CREATE PROCEDURE [Reference].[usp_GetCauseOfDeath]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		CauseOfDeathID, CauseOfDeathName 
		FROM		[Reference].[CauseOfDeath] 
		WHERE		IsActive = 1
		ORDER BY	CauseOfDeathName  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END