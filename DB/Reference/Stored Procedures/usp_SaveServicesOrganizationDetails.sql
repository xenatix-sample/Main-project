-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_SaveServicesOrganizationDetails]
-- Author:		Scott Martin
-- Date:		12/27/2016
--
-- Purpose:		Save Services to Organization Details mappings
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/27/2016	Scott Martin	Initial Creation
-- 12/28/2016	Scott Martin	Fixed a few minor bugs
-- 12/30/2016	Scott Martin	Fixed an issue where a date was being saved when none was passed in XML
-- 01/09/2017	Scott Martin	Modified the proc so DetailID was optional to allow for saving a service to multiple details
-- 01/23/2017	Gurpreet Singh	#22102	Not updating Dates when passed as blank
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Reference.usp_SaveServicesOrganizationDetails	
	@ServicesXML XML,
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

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @ServicesXML;

	CREATE TABLE #ServicesOrganizationDetails
	(
		ServicesOrganizationDetailsID BIGINT,
		ServicesID INT,
		DetailID BIGINT,
		EffectiveDate DATE,
		ExpirationDate DATE,
		UpdateDates BIT DEFAULT(0)
	);

	INSERT INTO #ServicesOrganizationDetails
	(
		ServicesOrganizationDetailsID,
		ServicesID,
		DetailID,
		EffectiveDate,
		ExpirationDate
	)
	SELECT
		ISNULL(ServicesOrganizationDetailsID, 0),
		ServicesID,
		DetailID,
		CASE WHEN EffectiveDate = '1900-01-01' THEN NULL ELSE EffectiveDate END,
		CASE WHEN ExpirationDate = '1900-01-01' THEN NULL ELSE ExpirationDate END
	FROM OPENXML(@docHandle, N'/OrganizationDetails/Services/Service')
	WITH
 	(
		ServicesOrganizationDetailsID		BIGINT			'ServicesOrganizationDetailsID',
		ServicesID							INT				'ServicesID',
		DetailID							BIGINT			'DetailID',
		EffectiveDate						DATE			'EffectiveDate',
		ExpirationDate						DATE			'ExpirationDate'
	);

	--Existing Mappings
	UPDATE SOD
	SET ServicesOrganizationDetailsID = RSOD.ServicesOrganizationDetailsID,
		UpdateDates = CASE
			WHEN DATEDIFF(DAY, ISNULL(RSOD.EffectiveDate, '1900-01-01'), ISNULL(SOD.EffectiveDate, '1900-01-01')) <> 0 THEN 1
			WHEN DATEDIFF(DAY, ISNULL(RSOD.ExpirationDate, '1900-01-01'), ISNULL(SOD.ExpirationDate, '1900-01-01')) <> 0 THEN 1
			ELSE 0 END
	FROM
		#ServicesOrganizationDetails SOD
		INNER JOIN Reference.ServicesOrganizationDetails RSOD
			ON SOD.ServicesID = RSOD.ServicesID
			AND SOD.DetailID = RSOD.DetailID
	WHERE
		SOD.ServicesID = RSOD.ServicesID
		AND SOD.DetailID = RSOD.DetailID
		AND ISNULL(SOD.ServicesOrganizationDetailsID, 0) = 0;

	UPDATE SOD
	SET UpdateDates = CASE
			WHEN DATEDIFF(DAY, ISNULL(RSOD.EffectiveDate, '1900-01-01'), ISNULL(SOD.EffectiveDate, '1900-01-01')) <> 0 THEN 1
			WHEN DATEDIFF(DAY, ISNULL(RSOD.ExpirationDate, '1900-01-01'), ISNULL(SOD.ExpirationDate, '1900-01-01')) <> 0 THEN 1
			ELSE 0 END
	FROM
		#ServicesOrganizationDetails SOD
		INNER JOIN Reference.ServicesOrganizationDetails RSOD
			ON SOD.ServicesOrganizationDetailsID = RSOD.ServicesOrganizationDetailsID
	WHERE
		SOD.ServicesOrganizationDetailsID = RSOD.ServicesOrganizationDetailsID;


	--Add new mappings
	DECLARE @PKID BIGINT, @LOOP INT;

	DECLARE @Mappings TABLE
	(
		RecordID INT IDENTITY(1,1),
		ServicesID INT,
		DetailID BIGINT,
		EffectiveDate DATE,
		ExpirationDate DATE,
		Completed BIT DEFAULT(0)
	);

	--Service Mappings
	INSERT INTO @Mappings
	(
		ServicesID,
		DetailID,
		EffectiveDate,
		ExpirationDate
	)
	SELECT DISTINCT
		ServicesID,
		DetailID,
		EffectiveDate,
		ExpirationDate
	FROM
		#ServicesOrganizationDetails
	WHERE
		ServicesOrganizationDetailsID = 0;

	SELECT @Loop = COUNT(*) FROM @Mappings WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = RecordID FROM @Mappings WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Reference.ServicesOrganizationDetails
	(
		ServicesID,
		DetailID,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		ServicesID,
		DetailID,
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
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'ServicesOrganizationDetails', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'ServicesOrganizationDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @Mappings
	SET Completed = 1
	WHERE
		RecordID = @PKID;

	UPDATE #ServicesOrganizationDetails
	SET ServicesOrganizationDetailsID = @ID
	FROM
		#ServicesOrganizationDetails SOD
		INNER JOIN @Mappings M
			ON SOD.ServicesID = M.ServicesID
			AND SOD.DetailID = M.DetailID
	WHERE
		SOD.ServicesID = M.ServicesID
		AND SOD.DetailID = M.DetailID
		AND M.RecordID = @PKID
	END

	--Update existing Mappings
	DECLARE @IDS TABLE (ServicesOrganizationDetailsID BIGINT, EffectiveDate DATE, ExpirationDate DATE, Completed BIT);

	INSERT INTO @IDS
	(
		ServicesOrganizationDetailsID,
		EffectiveDate,
		ExpirationDate,
		Completed
	)
	SELECT
		ServicesOrganizationDetailsID,
		EffectiveDate,
		ExpirationDate,
		0
	FROM
		#ServicesOrganizationDetails
	WHERE
		UpdateDates = 1;

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ServicesOrganizationDetailsID FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'ServicesOrganizationDetails', @PKID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE SOD
	SET EffectiveDate = IDS.EffectiveDate,
		ExpirationDate = IDS.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Reference.ServicesOrganizationDetails SOD
		INNER JOIN @IDS IDS
			ON SOD.ServicesOrganizationDetailsID = IDS.ServicesOrganizationDetailsID
	WHERE
		SOD.ServicesOrganizationDetailsID = IDS.ServicesOrganizationDetailsID
		AND IDS.ServicesOrganizationDetailsID = @PKID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'ServicesOrganizationDetails', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @IDS
	SET Completed = 1
	WHERE
		ServicesOrganizationDetailsID = @PKID;
	END
	
	EXEC sp_xml_removedocument @docHandle;
	
	DROP TABLE #ServicesOrganizationDetails	
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
