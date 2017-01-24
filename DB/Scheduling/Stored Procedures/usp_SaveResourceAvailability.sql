-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_SaveResourceAvailability]
-- Author:		Scott Martin
-- Date:		02/16/2016
--
-- Purpose:		Add/Update/Delete proc for Resource Availability
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/16/2016	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].usp_SaveResourceAvailability
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@FacilityID INT,
	@SchedulingXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@AdditionalResult XML OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@AuditDetailID BIGINT,
		@AuditCursor CURSOR,
		@Operation NVARCHAR(10),
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY		
	DECLARE @tmpSource TABLE
	(
		ResourceAvailabilityID BIGINT,
		SourceResourceAvailabilityID BIGINT,
		DefaultFacilityID INT,
		ScheduleTypeID SMALLINT,
		[DayOfWeekID] NVARCHAR(10),
		[AvailabilityStartTime] NVARCHAR(10),
		[AvailabilityEndTime] NVARCHAR(10),
		AuditDetailID BIGINT,
		Operation NVARCHAR(10)
	);

	INSERT INTO @tmpSource 
	(
		ResourceAvailabilityID,
		SourceResourceAvailabilityID,
		DefaultFacilityID,
		ScheduleTypeID,
		[DayOfWeekID], 
		[AvailabilityStartTime], 
		[AvailabilityEndTime],
		AuditDetailID,
		Operation
	)
	SELECT
		RA.ResourceAvailabilityID,
		ISNULL(T.C.value('ResourceAvailabilityID[1]', 'BIGINT'), 0),
		T.C.value('DefaultFacilityID[1]', 'INT'),
		T.C.value('ScheduleTypeID[1]', 'SMALLINT'),
		T.C.value('DayOfWeekID[1]', 'NVARCHAR(10)'),
		T.C.value('AvailabilityStartTime[1]', 'NVARCHAR(10)'),
		T.C.value('AvailabilityEndTime[1]', 'NVARCHAR(10)'),
		NULL,
		CASE
			WHEN ISNULL(T.C.value('ResourceAvailabilityID[1]', 'BIGINT'), 0) = 0 AND RA.ResourceAvailabilityID IS NOT NULL THEN 'Inactivate'
			WHEN ISNULL(T.C.value('ResourceAvailabilityID[1]', 'BIGINT'), 0) = 0 AND RA.ResourceAvailabilityID IS NULL THEN 'Insert'
			WHEN ISNULL(T.C.value('ResourceAvailabilityID[1]', 'BIGINT'), 0) > 0 THEN 'Update' END AS Operation
	FROM 
		@SchedulingXML.nodes('RequestXMLValue/Scheduling') AS T (C)
		FULL OUTER JOIN (SELECT * FROM Scheduling.ResourceAvailability WHERE ResourceID = @ResourceID AND ResourceTypeID = @ResourceTypeID AND FacilityID = @FacilityID AND IsActive = 1) RA
			ON T.C.value('ResourceAvailabilityID[1]', 'BIGINT') = RA.ResourceAvailabilityID;


	IF EXISTS (SELECT TOP 1 * FROM @tmpSource WHERE Operation IN ('Update', 'Inactivate'))
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ResourceAvailabilityID, Operation FROM @tmpSource WHERE Operation IN ('Update', 'Inactivate');    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @Operation

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @Operation = 'Update'
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'ResourceAvailability', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Scheduling', 'ResourceAvailability', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END

		UPDATE @tmpSource
		SET AuditDetailID = @AuditDetailID
		WHERE
			ResourceAvailabilityID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @Operation
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	DECLARE @RA_ID TABLE
	(
		Operation NVARCHAR(12),
		ID BIGINT,
		ModifiedOn DATETIME
	);
	
	MERGE Scheduling.ResourceAvailability AS TARGET
	USING (SELECT * FROM @tmpSource WHERE Operation IN ('Insert', 'Update')) AS SOURCE
		ON SOURCE.SourceResourceAvailabilityID = TARGET.ResourceAvailabilityID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			ResourceID,
			ResourceTypeID,
			FacilityID,
			DefaultFacilityID,
			ScheduleTypeID,
			DayOfWeekID,
			AvailabilityStartTime,
			AvailabilityEndTime,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ResourceID,
			@ResourceTypeID,
			@FacilityID,
			SOURCE.DefaultFacilityID,
			SOURCE.ScheduleTypeID,
			SOURCE.DayOfWeekID,
			SOURCE.AvailabilityStartTime,
			SOURCE.AvailabilityEndTime,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)
	WHEN NOT MATCHED BY SOURCE AND TARGET.ResourceID = @ResourceID AND TARGET.ResourceTypeID = @ResourceTypeID AND TARGET.FacilityID = @FacilityID AND TARGET.IsActive = 1 THEN
		UPDATE
		SET TARGET.IsActive = 0,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = @ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	WHEN MATCHED THEN
		UPDATE
		SET TARGET.DefaultFacilityID = SOURCE.DefaultFacilityID,
			TARGET.ScheduleTypeID = SOURCE.ScheduleTypeID,
			TARGET.DayOfWeekID = SOURCE.DayOfWeekID,
			TARGET.AvailabilityStartTime = SOURCE.AvailabilityStartTime,
			TARGET.AvailabilityEndTime = SOURCE.AvailabilityEndTime,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = @ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.ResourceAvailabilityID,
		inserted.ModifiedOn
	INTO
		@RA_ID;

	IF EXISTS (SELECT TOP 1 * FROM @RA_ID WHERE Operation = 'Insert')
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @RA_ID WHERE Operation = 'Insert';    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'ResourceAvailability', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'ResourceAvailability', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END

	IF EXISTS (SELECT TOP 1 * FROM @tmpSource WHERE Operation IN ('Update', 'Inactivate'))
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ResourceAvailabilityID, Operation, AuditDetailID FROM @tmpSource WHERE Operation IN ('Update', 'Inactivate');    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @Operation, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		IF @Operation = 'Update'
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'ResourceAvailability', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
		ELSE
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Scheduling', 'ResourceAvailability', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @Operation, @AuditDetailID
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