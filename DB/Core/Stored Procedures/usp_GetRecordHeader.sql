
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetRecordHeader]
-- Author:		Sumana Sangapu
-- Date:		01/12/2017
--
-- Purpose:		Get Record Header for print view snapshot details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2017	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRecordHeader]
	@RecordPrimaryKeyValue  BIGINT,
	@WorkflowDataKey nvarchar(250),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
			DECLARE @WorkflowID BIGINT

			-- Fetch WorkflowID from WorkflowDataKey
			SELECT		@WorkflowID = w.WorkflowID 
			FROM		Core.Workflow w 
			INNER JOIN	Core.WorkflowComponentMapping wcm
			ON			w.WorkflowID	= wcm.WorkflowID
			INNER JOIN	Core.ModuleComponent mc
			ON			wcm.ModuleComponentID = mc.ModuleComponentID
			WHERE		mc.DataKey = @WorkflowDataKey 


			SELECT	RecordHeaderID, WorkflowID, RecordPrimaryKeyValue, IsActive
			FROM	Core.RecordHeader 
			WHERE   WorkflowID  = @WorkflowID
			AND		RecordPrimaryKeyValue = @RecordPrimaryKeyValue 
			
	END TRY

	BEGIN CATCH
		SELECT  @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END