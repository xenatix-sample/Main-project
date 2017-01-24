-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_ReceiveCorrespondenceDetails]
-- Author:		Avikal Gupta
-- Date:		09/17/2015
--
-- Purpose:		Gets the list of receive correspondence Status lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/17/2015	Avikal		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_ReceiveCorrespondenceDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ReceiveCorrespondenceID, ReceiveCorrespondenceValue as ReceiveCorrespondence
		FROM		[Reference].[ReceiveCorrespondence] 
		WHERE		IsActive = 1
		ORDER BY	ReceiveCorrespondenceValue ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO


