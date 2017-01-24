----------------------------------------------------------------------------------------------------------------------
-- Procedure: [usp_DeleteMergedContacts]
-- Author:           Scott Martin
-- Date:             12/05/2016
--
-- Purpose:          Inactivates the new contact and activates the Parent/Child contacts
--
-- Notes:            n/a (or any additional notes)
--
-- Depends:          n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/09/2016	Scott Martin         Initial creation.
-- 01/11/2017	Scott Martin	Changed how UnMerge is handled in the db
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteMergedContact]
	@MergedContactsMappingID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
				@ResultMessage = 'executed successfully'

	BEGIN TRY
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ParentID BIGINT,
			@ChildID BIGINT,
			@MergedContactID BIGINT,
			@MergedTransactionLogID BIGINT,
			@TransactionLogID BIGINT,
			@ContactID BIGINT,
			@ModuleComponentID BIGINT,
			@AllowUnMerge BIT;

	EXEC Core.usp_ValidateUnMergeRequest @MergedContactsMappingID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AllowUnMerge OUTPUT;

	IF @AllowUnMerge = 1
		BEGIN
		CREATE TABLE #TLog (TransactionLogID NVARCHAR(16));

		INSERT INTO #TLog EXEC Core.usp_GenerateTransactionLogID @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

		SELECT @TransactionLogID = TransactionLogID FROM #TLog;

		DROP TABLE #TLog;
	
		SELECT
			@ParentID = MCM.ParentID,
			@ChildID = MCM.ChildID,
			@MergedContactID = MCM.ContactID,
			@MergedTransactionLogID = MCM.TransactionLogID
		FROM
			Core.MergedContactsMapping MCM
		WHERE
			MCM.MergedContactsMappingID = @MergedContactsMappingID;

		--Inactivate merged contact mapping record
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'MergedContactsMapping', @MergedContactsMappingID, NULL, @TransactionLogID, 103, @MergedContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.MergedContactsMapping
		SET IsActive = 0,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			MergedContactsMappingID = @MergedContactsMappingID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'MergedContactsMapping', @AuditDetailID, @MergedContactsMappingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		--Activate Parent record
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Contact', @ParentID, NULL, @TransactionLogID, 23, @ParentID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.Contact
		SET IsActive = 1,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactID = @ParentID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Contact', @AuditDetailID, @ParentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactMRN', @ParentID, NULL, @TransactionLogID, 23, @ParentID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactMRN
		SET IsActive = 1,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactID = @ParentID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactMRN', @AuditDetailID, @ParentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		--Activate Child record
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Contact', @ChildID, NULL, @TransactionLogID, 23, @ChildID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.Contact
		SET IsActive = 1,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactID = @ChildID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Contact', @AuditDetailID, @ChildID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactMRN', @ChildID, NULL, @TransactionLogID, 23, @ChildID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactMRN
		SET IsActive = 1,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactID = @ChildID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactMRN', @AuditDetailID, @ChildID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		--Inactivate merged contact record
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'Contact', @MergedContactID, NULL, @TransactionLogID, 23, @MergedContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.Contact
		SET IsActive = 0,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactID = @MergedContactID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'Contact', @AuditDetailID, @ChildID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactMRN', @MergedContactID, NULL, @TransactionLogID, 23, @MergedContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactMRN
		SET IsActive = 0,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactID = @MergedContactID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactMRN', @AuditDetailID, @ChildID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		--Re-map services to original records
		SELECT
			ServiceRecordingID,
			SourceHeaderID,
			ContactID,
			0 AS Completed
		INTO
			#Services
		FROM
			Auditing.ServiceRecordingHistory SRH
		WHERE
			SRH.TransactionLogID = @MergedTransactionLogID;

		DECLARE @PKID BIGINT, @Loop INT

		SELECT @Loop = COUNT(*) FROM #Services WHERE Completed = 0;

		WHILE @LOOP > 0

		BEGIN

		SET ROWCOUNT 1
		SELECT @PKID = ServiceRecordingID, @ContactID = ContactID FROM #Services WHERE Completed = 0;
		SET ROWCOUNT 0
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ServiceRecording', @PKID, NULL, @TransactionLogID, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		UPDATE Core.ServiceRecording
		SET SourceHeaderID = S.SourceHeaderID,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Core.ServiceRecording SR
			INNER JOIN #Services S
				ON SR.ServiceRecordingID = S.ServiceRecordingID
		WHERE
			S.ServiceRecordingID = @PKID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ServiceRecording', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SELECT @Loop = @Loop - 1
		UPDATE #Services
		SET Completed = 1
		WHERE
			ServiceRecordingID = @PKID;
		END

		--Re-map child relationship on other contact
		SELECT
			AD.AuditPrimaryKeyValue AS ContactRelationshipID,
			AuditPre.value('(Registration.ContactRelationship/@ParentContactID)[1]','bigint') AS ParentContactID,
			AuditPre.value('(Registration.ContactRelationship/@ChildContactID)[1]','bigint') AS ChildContactID,
			0 AS Completed
		INTO
			#Relationships
		FROM
			Auditing.Audits A
			INNER JOIN Auditing.AuditDetail AD
				ON A.AuditID = AD.AuditID
			LEFT OUTER JOIN Reference.TableCatalog TC
				ON AD.TableCatalogID = TC.TableCatalogID
		WHERE
			TransactionLogID = @MergedTransactionLogID
			AND TC.SchemaName = 'Registration'
			AND TC.TableName = 'ContactRelationship'
			AND AuditTypeID = 2
			AND EntityID <> @MergedContactID;

		SELECT @Loop = COUNT(*) FROM #Relationships WHERE Completed = 0;

		WHILE @LOOP > 0

		BEGIN

		SET ROWCOUNT 1
		SELECT @PKID = ContactRelationshipID, @ContactID = ParentContactID FROM #Relationships WHERE Completed = 0;
		SET ROWCOUNT 0
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactRelationship', @PKID, NULL, @TransactionLogID, 26, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		UPDATE Registration.ContactRelationship
		SET ChildContactID = R.ChildContactID,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Registration.ContactRelationship CR
			INNER JOIN #Relationships R
				ON CR.ContactRelationshipID = R.ContactRelationshipID
		WHERE
			R.ContactRelationshipID = @PKID;
		
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactRelationship', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SELECT @Loop = @Loop - 1;
		UPDATE #Relationships
		SET Completed = 1
		WHERE
			ContactRelationshipID = @PKID;
		END

		--Re-map policy holder on other contact
		SELECT
			AD.AuditPrimaryKeyValue AS ContactPayorID,
			AuditPre.value('(Registration.ContactPayor/@ContactID)[1]','bigint') AS ContactID,
			AuditPre.value('(Registration.ContactPayor/@PolicyHolderID)[1]','bigint') AS PolicyHolderID,
			AuditPre.value('(Registration.ContactPayor/@PolicyHolderFirstName)[1]','nvarchar(200)') AS PolicyHolderFirstName,
			AuditPre.value('(Registration.ContactPayor/@PolicyHolderMiddleName)[1]','nvarchar(200)') AS PolicyHolderMiddleName,
			AuditPre.value('(Registration.ContactPayor/@PolicyHolderLastName)[1]','nvarchar(200)') AS PolicyHolderLastName,
			AuditPre.value('(Registration.ContactPayor/@PolicyHolderSuffixID)[1]','int') AS PolicyHolderSuffixID,
			0 AS Completed
		INTO
			#Payors
		FROM
			Auditing.Audits A
			INNER JOIN Auditing.AuditDetail AD
				ON A.AuditID = AD.AuditID
			LEFT OUTER JOIN Reference.TableCatalog TC
				ON AD.TableCatalogID = TC.TableCatalogID
		WHERE
			TransactionLogID = @MergedTransactionLogID
			AND TC.SchemaName = 'Registration'
			AND TC.TableName = 'ContactPayor'
			AND AuditTypeID = 2
			AND EntityID <> @MergedContactID;		

		SELECT @Loop = COUNT(*) FROM #Payors WHERE Completed = 0;

		WHILE @LOOP > 0

		BEGIN

		SET ROWCOUNT 1
		SELECT @PKID = ContactPayorID, @ContactID = ContactID FROM #Payors WHERE Completed = 0;
		SET ROWCOUNT 0
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPayor', @PKID, NULL, @TransactionLogID, 33, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		UPDATE Registration.ContactPayor
		SET PolicyHolderID = R.PolicyHolderID,
			PolicyHolderFirstName = R.PolicyHolderFirstName,
			PolicyHolderMiddleName = R.PolicyHolderMiddleName,
			PolicyHolderLastName = R.PolicyHolderLastName,
			PolicyHolderSuffixID = R.PolicyHolderSuffixID,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Registration.ContactPayor CP
			INNER JOIN #Payors R
				ON CP.ContactPayorID= R.ContactPayorID
		WHERE
			R.ContactPayorID = @PKID;
		
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPayor', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SELECT @Loop = @Loop - 1;
		UPDATE #Payors
		SET Completed = 1
		WHERE
			ContactPayorID = @PKID;
		END

		--Activate original CallCenterHeader records
		SELECT
			AD.AuditPrimaryKeyValue AS CallCenterHeaderID,
			AD.EntityID AS ContactID,
			A.ModuleComponentID,
			0 AS Completed
		INTO
			#CallCenterA
		FROM
			Auditing.Audits A
			INNER JOIN Auditing.AuditDetail AD
				ON A.AuditID = AD.AuditID
			LEFT OUTER JOIN Reference.TableCatalog TC
				ON AD.TableCatalogID = TC.TableCatalogID
		WHERE
			TransactionLogID = @MergedTransactionLogID
			AND TC.SchemaName = 'CallCenter'
			AND TC.TableName = 'CallCenterHeader'
			AND AuditTypeID = 3
			AND EntityID <> @MergedContactID;

		SELECT @Loop = COUNT(*) FROM #CallCenterA WHERE Completed = 0;

		WHILE @LOOP > 0

		BEGIN

		SET ROWCOUNT 1
		SELECT @PKID = CallCenterHeaderID, @ContactID = ContactID, @ModuleComponentID = ModuleComponentID FROM #CallCenterA WHERE Completed = 0;
		SET ROWCOUNT 0
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'CallCenter', 'CallCenterHeader', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		UPDATE CallCenter.CallCenterHeader
		SET IsActive = 1,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			CallCenter.CallCenterHeader CCH
			INNER JOIN #CallCenterA CCA
				ON CCH.CallCenterHeaderID = CCA.CallCenterHeaderID
		WHERE
			CCA.CallCenterHeaderID = @PKID;
		
		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SELECT @Loop = @Loop - 1;
		UPDATE #CallCenterA
		SET Completed = 1
		WHERE
			CallCenterHeaderID = @PKID;
		END

		--Inactivate merged CallCenterHeader records
		SELECT
			AD.AuditPrimaryKeyValue AS CallCenterHeaderID,
			AD.EntityID AS ContactID,
			A.ModuleComponentID,
			0 AS Completed
		INTO
			#CallCenterI
		FROM
			Auditing.Audits A
			INNER JOIN Auditing.AuditDetail AD
				ON A.AuditID = AD.AuditID
			LEFT OUTER JOIN Reference.TableCatalog TC
				ON AD.TableCatalogID = TC.TableCatalogID
		WHERE
			TransactionLogID = @MergedTransactionLogID
			AND TC.SchemaName = 'CallCenter'
			AND TC.TableName = 'CallCenterHeader'
			AND AuditTypeID = 4
			AND EntityID = @MergedContactID;

		SELECT @Loop = COUNT(*) FROM #CallCenterI WHERE Completed = 0;

		WHILE @LOOP > 0

		BEGIN

		SET ROWCOUNT 1
		SELECT @PKID = CallCenterHeaderID, @ContactID = ContactID, @ModuleComponentID = ModuleComponentID FROM #CallCenterI WHERE Completed = 0;
		SET ROWCOUNT 0
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'CallCenter', 'CallCenterHeader', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
		UPDATE CallCenter.CallCenterHeader
		SET IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			CallCenter.CallCenterHeader CCH
			INNER JOIN #CallCenterI CCA
				ON CCH.CallCenterHeaderID = CCA.CallCenterHeaderID
		WHERE
			CCA.CallCenterHeaderID = @PKID;
		
		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SELECT @Loop = @Loop - 1;
		UPDATE #CallCenterI
		SET Completed = 1
		WHERE
			CallCenterHeaderID = @PKID;
		END

		DROP TABLE #Services
		DROP TABLE #Relationships
		DROP TABLE #Payors
		DROP TABLE #CallCenterA
		DROP TABLE #CallCenterI
		END
	ELSE
		BEGIN
		--Inactivate merged contact mapping record
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'MergedContactsMapping', @MergedContactsMappingID, NULL, @TransactionLogID, 103, @MergedContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.MergedContactsMapping
		SET IsUnMergeAllowed = 0,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			MergedContactsMappingID = @MergedContactsMappingID;

		EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'MergedContactsMapping', @AuditDetailID, @MergedContactsMappingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
		
	END TRY
	BEGIN CATCH
			SELECT @ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
