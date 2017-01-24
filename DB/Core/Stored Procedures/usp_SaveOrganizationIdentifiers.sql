-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_SaveOrganizationIdentifiers]
-- Author:		Scott Martin
-- Date:		12/28/2016
--
-- Purpose:		Save Identifier details for an organization detail
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_SaveOrganizationIdentifiers
	@DetailID BIGINT,
	@OrganizationIdentifierXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	DECLARE @ID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@AuditDetailID BIGINT,
			@docHandle int;

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @OrganizationIdentifierXML;

	CREATE TABLE #OrganizationIdentifiers
	(
		OrganizationIdentifierID BIGINT,
		DetailID BIGINT,
		OrganizationIdentifierTypeID INT,
		OrganizationIdentifier NVARCHAR(50),
		EffectiveDate DATE,
		ExpirationDate DATE,
		UpdateRecord BIT DEFAULT(0)
	);

	INSERT INTO #OrganizationIdentifiers
	(
		OrganizationIdentifierID,
		DetailID,
		OrganizationIdentifierTypeID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate
	)
	SELECT
		ISNULL(OrganizationIdentifierID, 0),
		@DetailID,
		OrganizationIdentifierTypeID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate
	FROM OPENXML(@docHandle, N'/OrganizationDetails/Identifiers/Identifier')
	WITH
 	(
		OrganizationIdentifierID			BIGINT			'OrganizationIdentifierID',
		DetailID							BIGINT			'DetailID',
		OrganizationIdentifierTypeID		INT				'OrganizationIdentifierTypeID',
		OrganizationIdentifier				NVARCHAR(50)	'OrganizationIdentifier',
		EffectiveDate						DATE			'EffectiveDate',
		ExpirationDate						DATE			'ExpirationDate'
	);

	--Existing Mappings
	UPDATE OI
	SET OrganizationIdentifierID = COI.OrganizationIdentifierID,
		UpdateRecord = CASE
			WHEN OI.EffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(COI.EffectiveDate, '1900-01-01'), OI.EffectiveDate) <> 0 THEN 1
			WHEN OI.ExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(COI.ExpirationDate, '1900-01-01'), OI.ExpirationDate) <> 0 THEN 1
			WHEN ISNULL(OI.OrganizationIdentifier, '') <> ISNULL(COI.OrganizationIdentifier, '') THEN 1
			ELSE 0 END
	FROM
		#OrganizationIdentifiers OI
		INNER JOIN Core.OrganizationIdentifiers COI
			ON OI.DetailID = COI.DetailID
			AND OI.OrganizationIdentifierID = COI.OrganizationIdentifierID
	WHERE
		OI.DetailID = COI.DetailID
		AND OI.OrganizationIdentifierID = COI.OrganizationIdentifierID
		AND ISNULL(OI.OrganizationIdentifierID, 0) = 0;

	UPDATE OI
	SET UpdateRecord = CASE
			WHEN OI.EffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(COI.EffectiveDate, '1900-01-01'), OI.EffectiveDate) <> 0 THEN 1
			WHEN OI.ExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(COI.ExpirationDate, '1900-01-01'), OI.ExpirationDate) <> 0 THEN 1
			WHEN ISNULL(OI.OrganizationIdentifier, '') <> ISNULL(COI.OrganizationIdentifier, '') THEN 1
			ELSE 0 END
	FROM
		#OrganizationIdentifiers OI
		INNER JOIN Core.OrganizationIdentifiers COI
			ON OI.OrganizationIdentifierID = COI.OrganizationIdentifierID
	WHERE
		OI.OrganizationIdentifierID = COI.OrganizationIdentifierID;


	--Add new mappings
	DECLARE @PKID BIGINT, @LOOP INT;

	DECLARE @Mappings TABLE
	(
		RecordID INT IDENTITY(1,1),
		DetailID BIGINT,
		OrganizationIdentifierTypeID INT,
		OrganizationIdentifier NVARCHAR(50),
		EffectiveDate DATE,
		ExpirationDate DATE,
		Completed BIT DEFAULT(0)
	);

	--Service Mappings
	INSERT INTO @Mappings
	(
		DetailID,
		OrganizationIdentifierTypeID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate
	)
	SELECT DISTINCT
		DetailID,
		OrganizationIdentifierTypeID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate
	FROM
		#OrganizationIdentifiers
	WHERE
		OrganizationIdentifierID = 0;

	SELECT @Loop = COUNT(*) FROM @Mappings WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = RecordID FROM @Mappings WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Core.OrganizationIdentifiers
	(
		DetailID,
		OrganizationIdentifierTypeID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		DetailID,
		OrganizationIdentifierTypeID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		@Mappings M
	WHERE
		M.RecordID = @PKID

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationIdentifiers', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationIdentifiers', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @Mappings
	SET Completed = 1
	WHERE
		RecordID = @PKID;

	UPDATE #OrganizationIdentifiers
	SET OrganizationIdentifierID = @ID
	FROM
		#OrganizationIdentifiers OI
		INNER JOIN @Mappings M
			ON OI.DetailID = M.DetailID
			AND OI.OrganizationIdentifierTypeID = M.OrganizationIdentifierTypeID
	WHERE
		OI.DetailID = M.DetailID
		AND OI.OrganizationIdentifierTypeID = M.OrganizationIdentifierTypeID
		AND M.RecordID = @PKID
	END

	--Update existing Mappings
	DECLARE @IDS TABLE (OrganizationIdentifierID BIGINT, OrganizationIdentifier NVARCHAR(50), EffectiveDate DATE, ExpirationDate DATE, Completed BIT);

	INSERT INTO @IDS
	(
		OrganizationIdentifierID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate,
		Completed
	)
	SELECT
		OrganizationIdentifierID,
		OrganizationIdentifier,
		EffectiveDate,
		ExpirationDate,
		0
	FROM
		#OrganizationIdentifiers
	WHERE
		UpdateRecord = 1;

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = OrganizationIdentifierID FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationIdentifiers', @PKID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE OI
	SET OrganizationIdentifier = IDS.OrganizationIdentifier,
		EffectiveDate = IDS.EffectiveDate,
		ExpirationDate = IDS.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Core.OrganizationIdentifiers OI
		INNER JOIN @IDS IDS
			ON OI.OrganizationIdentifierID = IDS.OrganizationIdentifierID
	WHERE
		OI.OrganizationIdentifierID = IDS.OrganizationIdentifierID
		AND IDS.OrganizationIdentifierID = @PKID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationIdentifiers', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @IDS
	SET Completed = 1
	WHERE
		OrganizationIdentifierID = @PKID;
	END
	
	EXEC sp_xml_removedocument @docHandle;
	
	DROP TABLE #OrganizationIdentifiers	
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
