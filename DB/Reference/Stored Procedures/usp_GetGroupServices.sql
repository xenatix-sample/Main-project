-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupServicesByType]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Get Group services by Type lookup details for scheduling
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-- 03/16/2016    Justin Spalti - Removed the GroupTypeIFD parameter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetGroupServices]
--@GroupTypeID int,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		gts.[GroupTypeID], s.[ServicesID], s.[ServiceName]
		FROM		[Reference].[GroupTypeServices] gts
		INNER JOIN  [Reference].[Services] s
			ON gts.ServicesID = s.ServicesID
		--WHERE		gts.GroupTypeID = @GroupTypeID
		WHERE gts.IsActive = 1
		ORDER BY	s.[ServiceName]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END