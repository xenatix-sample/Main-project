
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_AddSchedulingConfiguration]
-- Author:		John Crossen
-- Date:		02/11/2016
--
-- Purpose:		Add Scheduling Config
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/06/2015 - John Crossen TFS @ 6198
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].usp_AddSchedulingConfiguration
	@SchedulingXML xml,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@AdditionalResult XML OUTPUT
AS
BEGIN
DECLARE @AuditPost XML,
		@AuditID BIGINT;

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
		IF OBJECT_ID('tempdb..#tmpSchedulingConfig') IS NOT NULL
			DROP TABLE #tmpSchedulingConfig

		
		CREATE TABLE #tmpSchedulingConfig 
		(
			ResourceID int,
			ResourceTypeID smallint,
			FacilityID INT,
			DefaultFacilityID INT,
			[DayOfWeekID] NVARCHAR(10),
			[AvailabilityStartTime] NVARCHAR(10),
			[AvailabilityEndTime] NVARCHAR(10),
			IsActive BIT,
			ModifiedBy INT,
			ModifiedOn DATETIME
		);

		INSERT INTO #tmpSchedulingConfig 
		(
			ResourceID, 
			ResourceTypeID,
			FacilityID, 
			DefaultFacilityID, 
			[DayOfWeekID], 
			[AvailabilityStartTime], 
			[AvailabilityEndTime],
			IsActive, 
			ModifiedBy,
			ModifiedOn
		)
		SELECT
			T.C.value('(./ResourceID/text())[1]', 'INT'),
			T.C.value('ResourceTypeID[1]', 'INT'),
			T.C.value('FacilityID[1]', 'INT'),
			T.C.value('DefaultFacilityID[1]', 'INT'),
			T.C.value('DayOfWeekID[1]', 'NVARCHAR(10)'),
			T.C.value('AvailabilityStartTime[1]', 'NVARCHAR(10)'),
			T.C.value('AvailabilityEndTime[1]', 'NVARCHAR(10)'),
			T.C.value('IsActive[1]', 'BIT'),
			T.C.value('ModifiedBy[1]', 'INT'),
			T.C.value('ModifiedOn[1]', 'DATETIME')
		FROM 
			@SchedulingXML.nodes('RequestXMLValue/Scheduling') AS T (C);

		
		MERGE INTO Scheduling.ResourceAvailability AS TARGET
		USING ( SELECT DISTINCT t.ResourceID, t.ResourceTypeID, t.FacilityID, t.DefaultFacilityID, t.Days, t.AvailabilityStartTime, t.AvailabilityEndTime, t.IsActive, 
		t.ModifiedBy, ModifiedOn FROM #tmpSchedulingConfig t ) AS SOURCE
			ON ISNULL(SOURCE.ResourceID, 0) = ISNULL(TARGET.ResourceID, 0)
			AND	ISNULL(SOURCE.ResourceTypeID, 0) = ISNULL(TARGET.ResourceTypeID, 0)
			AND ISNULL(SOURCE.FacilityID, 0) = ISNULL(TARGET.FacilityID, 0)
			AND ISNULL(SOURCE.DefaultFacilityID, 0) = ISNULL(TARGET.DefaultFacilityID, 0)
			AND ISNULL(SOURCE.DayOfWeekID, 1) = ISNULL(TARGET.DayOfWeekID, 1)
			AND ISNULL(SOURCE.AvailabilityStartTime, '') = ISNULL(TARGET.AvailabilityStartTime, '')
			AND ISNULL(SOURCE.AvailabilityEndTime, '') = ISNULL(TARGET.AvailabilityEndTime, '')
			AND ISNULL(SOURCE.IsActive, 0) = ISNULL(TARGET.IsActive, 0)
			AND ISNULL(SOURCE.ModifiedBy, 0) = ISNULL(TARGET.ModifiedBy, 0)
			AND ISNULL(SOURCE.ModifiedOn, '') = ISNULL(TARGET.ModifiedOn, '')
		WHEN NOT MATCHED THEN
			INSERT
			(
				ResourceID,
				ResourceTypeID,
				FacilityID,
				DefaultFacilityID,
				DayOfWeekID,
				AvailabilityStartTime,
				AvailabilityEndTime,
				ModifiedBy,
				ModifiedOn,
				IsActive,
				ModifiedBy,
				ModifiedOn,
				CreatedBy,
				CreatedOn
			)
			VALUES
			(
				SOURCE.ResourceID,
				SOURCE.ResourceTypeID,
				SOURCE.FacilityID,
				SOURCE.DefaultFacilityID,
				SOURCE.DayOfWeekID,
				SOURCE.AvailabilityStartTime,
				SOURCE.AvailabilityEndTime,
				SOURCE.ModifiedBy,
				SOURCE.ModifiedOn,
				SOURCE.IsActive,
				SOURCE.ModifiedBy,
				SOURCE.ModifiedOn,
				SOURCE.ModifiedBy,
				SOURCE.ModifiedOn
			);
		

	
		
		
	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END