-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPermissionLevelDetails]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get Permission Level details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetPermissionLevelDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT 
		P.PermissionLevelID,
		P.Name,
		OA.DataKey
	FROM 
		Core.[PermissionLevel] P
		INNER JOIN Core.OrganizationAttributes OA
			ON P.AttributeID = OA.AttributeID
	WHERE 
		P.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO