
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddGroupSchedulingResource]
-- Author:		Sumana Sangapu
-- Date:		03/11/2016
--
-- Purpose:		Insert for Appointment Status Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	Sumana Sangapu    Initial creation.
-- 03/24/2016	Sumana Sangapu	Bulk insert using XML
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_AddGroupSchedulingResource]
	@GroupResourceXML xml,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT--,
	--@GroupResourceID BIGINT OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID BIGINT

	SELECT	@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	DECLARE @GR_ID TABLE (ID INT, ModifiedOn DATETIME);
				
			INSERT INTO [Scheduling].[GroupSchedulingResource]
			(
			GroupHeaderID,
			ResourceID,
			ResourceTypeID,
			IsPrimary,
			IsActive,
			ModifiedBy ,
		    ModifiedOn ,
			CreatedBy ,
			CreatedOn
			)
			OUTPUT inserted.GroupResourceID, inserted.ModifiedOn
			INTO @GR_ID	
			SELECT
				T.C.value('GroupHeaderID[1]','Bigint') as GroupHeaderID,
				T.C.value('ResourceID[1]','Int') as ResourceID,
				T.C.value('ResourceTypeID[1]','smallint') as ResourceTypeID,
				T.C.value('IsPrimary[1]','BIT') as IsPrimary,
				T.C.value('IsActive[1]','BIT') as IsActive,
				@ModifiedBy,
				@ModifiedOn as ModifiedOn,
				@ModifiedBy,
				@ModifiedOn as CreatedOn
			FROM @GroupResourceXML.nodes('GroupResource/GroupSchedulingResource') AS T(C);

			DECLARE @AuditCursor CURSOR;
			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT ID, ModifiedOn FROM @GR_ID;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @ID, @ModifiedOn

				WHILE @@FETCH_STATUS = 0
				BEGIN
					EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'GroupSchedulingResource', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
					EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'GroupSchedulingResource', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
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
