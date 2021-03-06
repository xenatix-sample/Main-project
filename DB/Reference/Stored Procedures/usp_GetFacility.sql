-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [Reference].[usp_GetFacility]
-- Author:		Rajiv Ranjan
-- Date:		10/20/2015
--
-- Purpose:		Gets the list of  Facility lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/20/2015	Rajiv Ranjan		 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetFacility]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[FacilityID], [FacilityName] 
		FROM		[Reference].[Facility]
		WHERE		IsActive = 1
		ORDER BY	[FacilityName]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END