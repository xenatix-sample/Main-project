  -----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetChiefComplaintList]
-- Author:		Chad Roberts
-- Date:		11/20/2015
--
-- Purpose:		To get Clinical Chief Complaint list for a contact
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/20/2015   Chad Roberts		Initial Creation
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetChiefComplaintList]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT
			cc.[ChiefComplaintID]
			,cc.[ChiefComplaint]
			,cc.[TakenBy]
			,cc.[TakenTime]
			,cc.[IsActive]
			,cc.[ModifiedBy]
			,cc.[ModifiedOn]
		FROM [Clinical].[ChiefComplaint] cc
		WHERE cc.[ContactID] = @ContactID AND cc.[IsActive] = 1
		ORDER BY cc.[ModifiedOn] DESC

	END TRY
	BEGIN CATCH
		SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END