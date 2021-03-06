-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupType]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Get Group Type lookup details for scheduling
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetGroupType]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[GroupTypeID], [GroupType] 
		FROM		[Reference].[GroupType]
		WHERE		IsActive = 1
		ORDER BY	[GroupType]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END