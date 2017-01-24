-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRecipientCodeModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		06/22/2016
--
-- Purpose:		Gets the list of Recipient Code Module Component lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/22/2016	Scott Martin	- Initial creation.
-- 01/24/2017	Kyle Campbell	TFS #14007	Added IsActive in WHERE clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetRecipientCodeModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		RCMC.RecipientCodeModuleComponentID,
		RC.CodeID,
		RC.Code,
		RC.CodeDescription,
		RCMC.ServicesID AS ServiceID,
		MC.ModuleComponentID,
		MC.DataKey,
		RCMC.IsActive
	FROM
		Reference.RecipientCodeModuleComponent RCMC
		INNER JOIN Reference.RecipientCode RC
			ON RCMC.RecipientCodeID = RC.CodeID
		INNER JOIN Core.ModuleComponent MC
			ON RCMC.ModuleComponentID = MC.ModuleComponentID
	WHERE RC.IsActive = 1
		AND RCMC.IsActive = 1
	ORDER BY DataKey, ServiceID, CodeDescription
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END