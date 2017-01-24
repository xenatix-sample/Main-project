-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetGroupSchedulingResource]
-- Author:		John Crossen
-- Date:		03/11/2016
--
-- Purpose:		Insert for Appointment Status Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	John Crossen   7687	- Initial creation.
-- 03/25/2016   Justin Spalti  Added AppointmentResourceID to the select list
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetGroupSchedulingResource]
	@GroupHeaderID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully';

	BEGIN TRY
		SELECT gsr.[GroupResourceID]
			  ,gsr.[GroupHeaderID]
			  ,gsr.[ResourceID]
			  ,gsr.[ResourceTypeID]
			  ,gsr.[IsPrimary]
			  ,ar.AppointmentResourceID
			  ,gsr.[IsActive]
			  ,gsr.[ModifiedBy]
			  ,gsr.[ModifiedOn]
     
		FROM [Scheduling].[GroupSchedulingResource] gsr
		JOIN [Scheduling].[AppointmentResource] ar
			ON ar.GroupHeaderID = gsr.GroupHeaderID 
			AND ar.ResourceID = gsr.ResourceID 
			AND ar.ResourceTypeID = gsr.ResourceTypeID
		WHERE gsr.IsActive=1 
			AND gsr.GroupHeaderID = @GroupHeaderID;
	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE();
	END CATCH;
END;