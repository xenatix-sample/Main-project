
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAppointmentResourceDetails]
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
-- 10/30/2015   Rajiv Ranjan	-		Refactored proc to use merge statement for handling add/update & delete
-- 11/06/2015   Rajiv Ranjan	-		Added IsActive=true while updating a record 
-- 02/03/2016   Satish Singh            Added IsActive in update
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateAppointmentResourceDetails]
	@AppointmentResourceXML xml,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditPost XML,
		@AuditPre XML,
		@AuditID BIGINT;

	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully'--

		declare @AppointmentID BIGINT,
				@ModifiedOn DATETIME

		SELECT @AppointmentID = @AppointmentResourceXML.value('(Appointment/AppointmentResourceDetails/AppointmentID)[1]', 'BIGINT');
		SELECT @ModifiedOn = @AppointmentResourceXML.value('(Appointment/AppointmentResourceDetails/ModifiedOn)[1]', 'DATETIME');

	BEGIN TRY
		DECLARE @AR TABLE
		(
			MergeAction NVARCHAR(25),
			AppointmentResourceID BIGINT,
			AppointmentID BIGINT,
			PrevAppointmentID BIGINT,
			ResourceID INT,
			PrevResourceID INT,
			ResourceTypeID SMALLINT,
			PrevResourceTypeID SMALLINT,
			ParentID BIGINT,
			PrevParentID BIGINT,
			IsActive BIT,
			PrevIsActive BIT,
			GroupHeaderID BIGINT,
			PrevGroupHeaderID BIGINT,
			ModifiedBy INT,
			PrevModifiedBy INT,
			ModifiedOn DATETIME,
			PrevModifiedOn DATETIME
		);

		MERGE [Scheduling].[AppointmentResource] AS TARGET
		USING 
		(
			SELECT
				T.C.value('AppointmentResourceID[1]','BIGINT') as AppointmentResourceID,
				T.C.value('ResourceTypeID[1]','SMALLINT') as ResourceTypeID,
				T.C.value('AppointmentID[1]','BIGINT') as AppointmentID,
				T.C.value('ResourceID[1]','INT') as ResourceID,
				T.C.value('ParentID[1]','BIGINT') as ParentID,
				case T.C.value('GroupHeaderID[1]','BIGINT') when '' then null else T.C.value('GroupHeaderID[1]','BIGINT') end as GroupHeaderID,
				T.C.value('ModifiedOn[1]','DATETIME') as ModifiedOn,
				T.C.value('IsActive[1]','BIT') as IsActive
			FROM 
				@AppointmentResourceXML.nodes('Appointment/AppointmentResourceDetails') AS T(C)
		) 
		AS 
			source (AppointmentResourceID,ResourceTypeID, AppointmentID, ResourceID, ParentID, GroupHeaderID, ModifiedOn, IsActive)
		ON 
		TARGET.AppointmentResourceID = source.AppointmentResourceID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT 
			(
				ResourceTypeID, 
				AppointmentID, 
				ResourceID, 
				ParentID,
				GroupHeaderID,
				IsActive, 
				ModifiedBy, 
				ModifiedOn
			)
			VALUES 
			(
				source.ResourceTypeID, 
				source.AppointmentID,
				source.ResourceID, 
				source.ParentID,
				source.GroupHeaderID,
				1,
				@ModifiedBy,
				source.ModifiedOn
			)
		WHEN MATCHED THEN
			UPDATE SET
				TARGET.ResourceTypeID=source.ResourceTypeID,
				TARGET.AppointmentID = source.AppointmentID,
				TARGET.ResourceID = source.ResourceID,
				TARGET.ParentID=source.ParentID,
				TARGET.GroupHeaderID=source.GroupHeaderID,
				TARGET.IsActive = ISNULL(source.IsActive, 1)
		
		OUTPUT
			$Action,
			inserted.AppointmentResourceID,
			inserted.AppointmentID,
			deleted.AppointmentID AS PrevAppointmentID,
			inserted.ResourceID,
			deleted.ResourceID AS PrevResourceID,
			inserted.ResourceTypeID,
			deleted.ResourceTypeID AS PrevResourceTypeID,
			inserted.ParentID,
			deleted.ParentID AS PrevParentID,
			inserted.GroupHeaderID,
			deleted.GroupHeaderID AS PrevGroupHeaderID,
			inserted.IsActive,
			deleted.IsActive AS PrevIsActive,
			inserted.ModifiedBy,
			deleted.ModifiedBy AS PrevModifiedBy,
			inserted.ModifiedOn,
			deleted.ModifiedOn AS PrevModifiedOn
		INTO @AR;

		DECLARE @TableCatalogID INT;
		SELECT @TableCatalogID = TableCatalogID FROM Reference.TableCatalog TC WHERE TC.SchemaName = 'Scheduling' AND TC.TableName = 'AppointmentResource';

		IF EXISTS (SELECT TOP 1 MergeAction FROM @AR WHERE MergeAction = 'INSERT')
			BEGIN
			INSERT INTO Auditing.Audits
			(
				AuditTypeID ,
				UserID,
				CreatedOn,
				AuditTimeStamp ,
				IsArchivable
			)
			VALUES
			(
				[Core].[fn_GetAuditType]('Insert'),
				@ModifiedBy ,
				@ModifiedOn,
				GETUTCDATE() , 
				0  
			);

			SELECT @AuditID = SCOPE_IDENTITY();

			INSERT INTO Auditing.AuditDetail
			(
				AuditID,
				AuditPost,
				AuditPrimaryKeyValue,
				TableCatalogID
			)
			SELECT
				@AuditID,
				(SELECT * FROM Scheduling.AppointmentResource WHERE AppointmentResourceID = AR.AppointmentResourceID FOR XML AUTO),
				AR.AppointmentResourceID,
				@TableCatalogID
			FROM
				@AR AR
			WHERE
				MergeAction = 'INSERT';
			END

		IF EXISTS (SELECT TOP 1 MergeAction FROM @AR WHERE MergeAction = 'UPDATE')
			BEGIN
			INSERT INTO Auditing.Audits
			(
				AuditTypeID ,
				UserID,
				CreatedOn,
				AuditTimeStamp ,
				IsArchivable
			)
			VALUES
			(
				[Core].[fn_GetAuditType]('Update'),
				@ModifiedBy ,
				@ModifiedOn,
				GETUTCDATE() , 
				0  
			);

			SELECT @AuditID = SCOPE_IDENTITY();

			INSERT INTO Auditing.AuditDetail
			(
				AuditID,
				AuditPre,
				AuditPost,
				AuditPrimaryKeyValue,
				TableCatalogID
			)
			SELECT
				@AuditID,
				(SELECT AppointmentResourceID, PrevAppointmentID AS AppointmentID, PrevResourceID AS ResourceID, PrevResourceTypeID AS ResourceTypeID, PrevParentID AS ParentID, PrevGroupHeaderID as GroupHeaderID, PrevModifiedBy AS ModifiedBy, PrevModifiedOn AS ModifiedOn FROM @AR WHERE AppointmentResourceID = AR.AppointmentResourceID FOR XML AUTO),
				(SELECT AppointmentResourceID, AppointmentID, ResourceID, ResourceTypeID, ParentID, ModifiedBy, ModifiedOn FROM @AR WHERE AppointmentResourceID = AR.AppointmentResourceID FOR XML AUTO),
				AR.AppointmentResourceID,
				@TableCatalogID
			FROM
				@AR AR
			WHERE
				MergeAction = 'UPDATE';
			END

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END