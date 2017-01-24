-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddAppointmentResourceDetails]
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
-- 10/30/2015   Rajiv Ranjan		-	Refactored procedure to accept xml as parameter
-- 12/17/2015	Scott Martin	Added audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-- 02/17/2016	Scott Martin		Added audit logging
-- 02/26/2016   Satish Singh		Added IsActive
-- 09/06/2016	Rahul Vats		Renamed Number to number
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_AddAppointmentResourceDetails]
	@AppointmentResourceXML xml,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ID BIGINT,
		@ModifiedOn DATETIME;
			
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
	DECLARE @AR_ID TABLE (ID INT, ModifiedOn DATETIME);
			
	INSERT INTO Scheduling.AppointmentResource
	( 
		AppointmentID,
		ResourceID,
		ResourceTypeID,
		ParentID,
		GroupHeaderID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT inserted.AppointmentResourceID, inserted.ModifiedOn
	INTO @AR_ID	
	SELECT
		T.C.value('AppointmentID[1]','Bigint') as AppointmentID,
		T.C.value('ResourceID[1]','Int') as ResourceID,
		T.C.value('ResourceTypeID[1]','smallint') as ResourceTypeID,
		T.C.value('ParentID[1]','Bigint') as ParentID,
		T.C.value('number(GroupHeaderID[1])','Bigint') as GroupHeaderID,
		T.C.value('IsActive[1]','BIT') as IsActive,
		@ModifiedBy,
		T.C.value('ModifiedOn[1]','DATETIME') as ModifiedOn,
		@ModifiedBy,
		T.C.value('ModifiedOn[1]','DATETIME') as CreatedOn
	FROM @AppointmentResourceXML.nodes('Appointment/AppointmentResourceDetails') AS T(C);

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @AR_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentResource', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentResource', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	END TRY

	BEGIN CATCH
	SELECT 
			@ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH

END