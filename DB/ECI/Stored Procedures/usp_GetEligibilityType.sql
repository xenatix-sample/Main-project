
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_GetEligibilityType]
-- Author:		Sumana Sangapu
-- Date:		10/13/2015
--
-- Purpose:		Lookup for EligibilityType
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Sumana Sangapu TFS# 2700 - Initial creation.
 -----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [ECI].[usp_GetEligibilityType]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[EligibilityTypeID],[EligibilityType]
		FROM		[ECI].[EligibilityType] 
		WHERE		IsActive = 1
		ORDER BY	EligibilityType  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END