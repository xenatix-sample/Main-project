-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateUserOrgDetailsMapping]
-- Author:		Sumana Sangapu
-- Date:		03/29/2016
--
-- Purpose:		Updates the user organization details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/29/2016	Sumana Sangapu    Initial creation. 
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserOrgDetailsMapping]
	@UserOrgDetailsXML xml,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditPost XML,
		@AuditPre XML,
		@AuditID BIGINT;

	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully'

		declare @UserID INT,
				@ModifiedOn DATETIME

		SELECT @UserID = @UserOrgDetailsXML.value('(UserOrgDetails/UserOrgDetails/UserID)[1]', 'BIGINT');
		SELECT @ModifiedOn = @UserOrgDetailsXML.value('(UserOrgDetails/UserOrgDetails/ModifiedOn)[1]', 'DATETIME');

	BEGIN TRY
		DECLARE @UO TABLE
		(
			MergeAction NVARCHAR(25),
			UserID INT,
			MappingID BIGINT,
			PrevMappingID BIGINT,
			IsActive BIT,
			PrevIsActive BIT,
			ModifiedBy INT,
			PrevModifiedBy INT,
			ModifiedOn DATETIME,
			PrevModifiedOn DATETIME
		);

		MERGE [Core].[UserOrganizationDetailsMapping]  AS TARGET
		USING 
		(
			SELECT
				T.C.value('UserID[1]','INT') as UserID,
				T.C.value('MappingID[1]','BIGINT') as MappingID,
				T.C.value('IsActive[1]','BIT') as IsActive,
				T.C.value('ModifiedBy[1]','INT') as ModifiedBy,
				T.C.value('ModifiedOn[1]','DATETIME') as ModifiedOn
			FROM 
				@UserOrgDetailsXML.nodes('UserOrgDetails/UserOrgDetails') AS T(C)
		) 
		AS 
			SOURCE (UserID, MappingID, IsActive,ModifiedBy, ModifiedOn  )
		ON 
		TARGET.UserID = source.UserID
		AND TARGET.MappingID = Source.MappingID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT 
			(
				UserID, 
				MappingID,
				IsActive, 
				ModifiedBy, 
				ModifiedOn
			)
			VALUES 
			(
				source.UserID, 
				source.MappingID,
				1,
				@ModifiedBy,
				source.ModifiedOn
			)
		WHEN MATCHED THEN
			UPDATE SET
				TARGET.UserID=source.UserID,
				TARGET.MappingID = source.MappingID,
				TARGET.IsActive = ISNULL(source.IsActive, 1)
		
		OUTPUT
			$Action,
			inserted.UserID,
			inserted.MappingID,
			deleted.MappingID AS PrevMappingID,
			inserted.IsActive,
			deleted.IsActive AS PrevIsActive,
			inserted.ModifiedBy,
			deleted.ModifiedBy AS PrevModifiedBy,
			inserted.ModifiedOn,
			deleted.ModifiedOn AS PrevModifiedOn
		INTO @UO;

		DECLARE @TableCatalogID INT;
		SELECT @TableCatalogID = TableCatalogID FROM Reference.TableCatalog TC WHERE TC.SchemaName = 'Core' AND TC.TableName = 'UserOrganizationDetailsMapping';

		IF EXISTS (SELECT TOP 1 MergeAction FROM @UO WHERE MergeAction = 'INSERT')
			BEGIN
			INSERT INTO Auditing.Audits
			(
				AuditTypeID ,
				UserID ,
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
				TableCatalogID,
				EntityID,
				EntityTypeID
			)
			SELECT
				@AuditID,
				(SELECT * FROM Core.UserOrganizationDetailsMapping WHERE UserID = UO.UserID FOR XML AUTO),
				UO.UserID,
				@TableCatalogID,
				UserID,
				1
			FROM
				@UO UO
			WHERE
				MergeAction = 'INSERT';
			END

		IF EXISTS (SELECT TOP 1 MergeAction FROM @UO WHERE MergeAction = 'UPDATE')
			BEGIN
			INSERT INTO Auditing.Audits
			(
				AuditTypeID ,
				UserID ,
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
				TableCatalogID,
				EntityID,
				EntityTypeID
			)
			SELECT
				@AuditID,
				(SELECT UserID, PrevMappingID AS MappingID, PrevModifiedBy AS ModifiedBy, PrevModifiedOn AS ModifiedOn FROM @UO WHERE UserID = AR.UserID FOR XML AUTO),
				(SELECT UserID, MappingID, ModifiedBy, ModifiedOn FROM @UO WHERE UserID = AR.UserID FOR XML AUTO),
				AR.UserID,
				@TableCatalogID,
				UserID,
				1
			FROM
				@UO AR
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