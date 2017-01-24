-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetAllergySeverity]
-- Author:		Scott Martin
-- Date:		11/13/2015
--
-- Purpose:		Get list of Allergy Severities
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Scott Martin	Initial creation.
-- 11/30/2015 - Justin Spalti - Added the IsActive flag to the where clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetAllergySeverity]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

SELECT [AllergySeverityID]
      ,[AllergySeverity]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
 FROM Clinical.[AllergySeverity]
 WHERE [IsActive] = 1
 ORDER BY [AllergySeverity]

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


