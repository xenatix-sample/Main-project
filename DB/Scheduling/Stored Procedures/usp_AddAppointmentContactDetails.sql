-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddAppointmentContactDetails]
-- Author:		John Crossen
-- Date:		10/15/2015
--
-- Purpose:		Add Appointment Contact details  
--
-- Notes:		
--				
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/15/2015   John Crossen	2731 - Initial Creation
-- 10/16/2015   John Crossen    2765 - Remove XML
-- 1//30/2015	Rajiv Ranjan		 - Removed @ModifiedOn and @ID, not required
-- 12/17/2015	Scott Martin	Added audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-- 02/17/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_AddAppointmentContactDetails]
	@AppointmentID BIGINT,
	@ContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT;
				
SELECT	@ResultCode = 0,
		@ResultMessage = 'executed successfully'

	BEGIN TRY			
	INSERT INTO Scheduling.AppointmentContact
	(
		AppointmentID ,
		ContactID ,
		IsActive ,
		ModifiedBy ,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)				
	SELECT
		@AppointmentID as AppointmentID,
		@ContactID as ContactID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentContact', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentContact', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 
				
	END TRY

	BEGIN CATCH
	SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END