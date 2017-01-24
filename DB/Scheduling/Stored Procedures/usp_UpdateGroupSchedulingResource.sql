-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateGroupSchedulingResource]
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
-- 03/28/2016   Justin Spalti - Converted parameter list to xml and added a merge statement to cover all scenarios
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateGroupSchedulingResource]
	@GroupResourceXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@AuditPost XML,
			@AuditPre XML,
			@AuditID BIGINT,
			@ModifiedOn DATETIME;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully'

	BEGIN TRY
		SELECT @ModifiedOn = @GroupResourceXML.value('(GroupResource/GroupSchedulingResource/ModifiedOn)[1]', 'DATETIME');

		DECLARE @GR TABLE
		(
			MergeAction NVARCHAR(25),
			GroupResourceID BIGINT,
			ResourceID INT,
			PrevResourceID INT,
			ResourceTypeID SMALLINT,
			PrevResourceTypeID SMALLINT,
			IsPrimary BIT,
			PrevIsPrimary BIT,
			IsActive BIT,
			PrevIsActive BIT,
			GroupHeaderID BIGINT,
			PrevGroupHeaderID BIGINT,
			ModifiedBy INT,
			PrevModifiedBy INT,
			ModifiedOn DATETIME,
			PrevModifiedOn DATETIME
		);

		MERGE [Scheduling].[GroupSchedulingResource] AS TARGET
		USING 
		(
			SELECT
				T.C.value('GroupResourceID[1]','BIGINT') as AppointmentResourceID,
				T.C.value('ResourceTypeID[1]','SMALLINT') as ResourceTypeID,
				T.C.value('ResourceID[1]','INT') as ResourceID,
				T.C.value('GroupHeaderID[1]','BIGINT') as GroupHeaderID,
				T.C.value('ModifiedOn[1]','DATETIME') as ModifiedOn,
				T.C.value('IsPrimary[1]','BIT') as IsPrimary,
				T.C.value('IsActive[1]','BIT') as IsActive
			FROM 
				@GroupResourceXML.nodes('GroupResource/GroupSchedulingResource') AS T(C)
		)
		AS 
			SOURCE (GroupResourceID, ResourceTypeID, ResourceID, GroupHeaderID, ModifiedOn, IsPrimary, IsActive)
		ON 
		TARGET.GroupResourceID = SOURCE.GroupResourceID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT 
			(
				ResourceTypeID,  
				ResourceID, 
				GroupHeaderID,
				IsPrimary,
				IsActive, 
				ModifiedBy, 
				ModifiedOn
			)
			VALUES 
			(
				SOURCE.ResourceTypeID, 
				SOURCE.ResourceID, 
				SOURCE.GroupHeaderID,
				SOURCE.IsPrimary,
				SOURCE.IsActive,
				@ModifiedBy,
				SOURCE.ModifiedOn
			)
		WHEN NOT MATCHED BY SOURCE THEN
			UPDATE
			SET TARGET.IsActive = 0,
				TARGET.ModifiedOn = GETUTCDATE(),
				TARGET.ModifiedBy = @ModifiedBy
		WHEN MATCHED THEN
			UPDATE SET
				TARGET.ResourceTypeID = SOURCE.ResourceTypeID,
				TARGET.ResourceID = SOURCE.ResourceID,
				TARGET.GroupHeaderID = SOURCE.GroupHeaderID,
				TARGET.IsPrimary = ISNULL(SOURCE.IsPrimary, 0),
				TARGET.IsActive = ISNULL(SOURCE.IsActive, 1),
				TARGET.ModifiedOn = SOURCE.ModifiedOn	

		OUTPUT
			$Action,
			inserted.GroupResourceID,
			inserted.ResourceID,
			deleted.ResourceID AS PrevResourceID,
			inserted.ResourceTypeID,
			deleted.ResourceTypeID AS PrevResourceTypeID,
			inserted.GroupHeaderID,
			deleted.GroupHeaderID AS PrevGroupHeaderID,
			inserted.IsPrimary,
			deleted.IsPrimary AS PrevIsPrimary,
			inserted.IsActive,
			deleted.IsActive AS PrevIsActive,
			inserted.ModifiedBy,
			deleted.ModifiedBy AS PrevModifiedBy,
			inserted.ModifiedOn,
			deleted.ModifiedOn AS PrevModifiedOn
		INTO @GR;

		DECLARE @TableCatalogID INT;
		SELECT @TableCatalogID = TableCatalogID FROM Reference.TableCatalog TC WHERE TC.SchemaName = 'Scheduling' AND TC.TableName = 'GroupResource';

		IF EXISTS (SELECT TOP 1 MergeAction FROM @GR WHERE MergeAction = 'INSERT')
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
				(SELECT * FROM Scheduling.GroupSchedulingResource WHERE GroupResourceID = GR.GroupResourceID FOR XML AUTO),
				GR.GroupResourceID,
				@TableCatalogID
			FROM
				@GR GR
			WHERE
				MergeAction = 'INSERT';
			END

		IF EXISTS (SELECT TOP 1 MergeAction FROM @GR WHERE MergeAction = 'UPDATE')
			BEGIN
			INSERT INTO Auditing.Audits
			(
				AuditTypeID ,
				UserId ,
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
				(SELECT GroupResourceID, PrevResourceID AS ResourceID, PrevResourceTypeID AS ResourceTypeID, PrevGroupHeaderID as GroupHeaderID, PrevModifiedBy AS ModifiedBy, PrevModifiedOn AS ModifiedOn FROM @GR WHERE GroupResourceID = GR.GroupResourceID FOR XML AUTO),
				(SELECT GroupResourceID, ResourceID, ResourceTypeID, ModifiedBy, ModifiedOn FROM @GR WHERE GroupResourceID = GR.GroupResourceID FOR XML AUTO),
				GR.GroupResourceID,
				@TableCatalogID
			FROM
				@GR GR
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