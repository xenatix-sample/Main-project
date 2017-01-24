-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[usp_SaveOrganizationDetailsMappings]
-- Author:		Scott Martin
-- Date:		12/22/2016
--
-- Purpose:		Insert/Update for Organization Details Mapping
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/22/2016	Scott Martin	Initial Creation
-- 12/27/2016	Scott Martin	Fixed a bug where duplicate program unit mappings would be created
-- 01/09/2017	Scott Martin	Made a change to the code so that a new mapping will be created if a similar one is already expired
-- 01/10/2017	Scott Martin	Changed how the xml is parsed
-- 01/17/2017	Scott Martin	Fixed a bug where MappingIDs were not updating in the temp table for existing records
-- 01/19/2017	Scott Martin	Adjusted the code to return the mapping ID for an Unexpired mapping
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveOrganizationDetailsMappings]
	@OrganizationDetailsMappingXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT,
		@docHandle int,
		@NodeExists BIT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @OrganizationDetailsMappingXML;

	CREATE TABLE #OrganizationHierarchy
	(
		CompanyMappingID BIGINT,
		CompanyDetailID BIGINT,
		CompanyEffectiveDate DATE,
		CompanyExpirationDate DATE,
		UpdateCompanyDates BIT DEFAULT(0),
		DivisionMappingID BIGINT,
		DivisionDetailID BIGINT,
		DivisionEffectiveDate DATE,
		DivisionExpirationDate DATE,
		UpdateDivisionDates BIT DEFAULT(0),
		ProgramMappingID BIGINT,
		ProgramDetailID BIGINT,
		ProgramEffectiveDate DATE,
		ProgramExpirationDate DATE,
		UpdateProgramDates BIT DEFAULT(0),
		ProgramUnitMappingID BIGINT,
		ProgramUnitDetailID BIGINT,
		ProgramUnitEffectiveDate DATE,
		ProgramUnitExpirationDate DATE,
		UpdateProgramUnitDates BIT DEFAULT(0)
	)

	SET @NodeExists = @OrganizationDetailsMappingXML.exist('/OrganizationDetails/Hierarchy/Companies/Company/Divisions')

	IF @NodeExists = 0
		BEGIN
		INSERT INTO #OrganizationHierarchy
		(
			CompanyMappingID,
			CompanyDetailID,
			CompanyEffectiveDate,
			CompanyExpirationDate
		)
		SELECT
			CompanyMappingID,
			CompanyDetailID,
			CompanyEffectiveDate,
			CompanyExpirationDate	
		FROM OPENXML(@docHandle, N'/OrganizationDetails/Hierarchy/Companies/Company')
		WITH
 		(
			CompanyMappingID			BIGINT			'CompanyMappingID',
			CompanyDetailID				BIGINT			'CompanyDetailID',
			CompanyEffectiveDate		DATE			'CompanyEffectiveDate',
			CompanyExpirationDate		DATE			'CompanyExpirationDate'
		);
		END

	SET @NodeExists = @OrganizationDetailsMappingXML.exist('/OrganizationDetails/Hierarchy/Companies/Company/Divisions/Division/Programs')

	IF @NodeExists = 0
		BEGIN
		INSERT INTO #OrganizationHierarchy
		(
			CompanyMappingID,
			CompanyDetailID,
			CompanyEffectiveDate,
			CompanyExpirationDate,
			DivisionMappingID,
			DivisionDetailID,
			DivisionEffectiveDate,
			DivisionExpirationDate
		)
		SELECT
			CompanyMappingID,
			CompanyDetailID,
			CompanyEffectiveDate,
			CompanyExpirationDate,
			DivisionMappingID,
			DivisionDetailID,
			DivisionEffectiveDate,
			DivisionExpirationDate
		FROM OPENXML(@docHandle, N'/OrganizationDetails/Hierarchy/Companies/Company/Divisions/Division')
		WITH
 		(
			CompanyMappingID			BIGINT			'..//../CompanyMappingID',
			CompanyDetailID				BIGINT			'..//../CompanyDetailID',
			CompanyEffectiveDate		DATE			'..//../CompanyEffectiveDate',
			CompanyExpirationDate		DATE			'..//../CompanyExpirationDate',
			DivisionMappingID			BIGINT			'DivisionMappingID',
			DivisionDetailID			BIGINT			'DivisionDetailID',
			DivisionEffectiveDate		DATE			'DivisionEffectiveDate',
			DivisionExpirationDate		DATE			'DivisionExpirationDate'
		);
		END

	SET @NodeExists = @OrganizationDetailsMappingXML.exist('/OrganizationDetails/Hierarchy/Companies/Company/Divisions/Division/Programs/Program/ProgramUnits')

	IF @NodeExists = 0
		BEGIN
		INSERT INTO #OrganizationHierarchy
		(
			CompanyMappingID,
			CompanyDetailID,
			CompanyEffectiveDate,
			CompanyExpirationDate,
			DivisionMappingID,
			DivisionDetailID,
			DivisionEffectiveDate,
			DivisionExpirationDate,
			ProgramMappingID,
			ProgramDetailID,
			ProgramEffectiveDate,
			ProgramExpirationDate
		)
		SELECT
			CompanyMappingID,
			CompanyDetailID,
			CompanyEffectiveDate,
			CompanyExpirationDate,
			DivisionMappingID,
			DivisionDetailID,
			DivisionEffectiveDate,
			DivisionExpirationDate,
			ProgramMappingID,
			ProgramDetailID,
			ProgramEffectiveDate,
			ProgramExpirationDate
		FROM OPENXML(@docHandle, N'/OrganizationDetails/Hierarchy/Companies/Company/Divisions/Division/Programs/Program')
		WITH
 		(
			CompanyMappingID			BIGINT			'..//..//..//../CompanyMappingID',
			CompanyDetailID				BIGINT			'..//..//..//../CompanyDetailID',
			CompanyEffectiveDate		DATE			'..//..//..//../CompanyEffectiveDate',
			CompanyExpirationDate		DATE			'..//..//..//../CompanyExpirationDate',
			DivisionMappingID			BIGINT			'..//../DivisionMappingID',
			DivisionDetailID			BIGINT			'..//../DivisionDetailID',
			DivisionEffectiveDate		DATE			'..//../DivisionEffectiveDate',
			DivisionExpirationDate		DATE			'..//../DivisionExpirationDate',
			ProgramMappingID			BIGINT			'ProgramMappingID',
			ProgramDetailID				BIGINT			'ProgramDetailID',
			ProgramEffectiveDate		DATE			'ProgramEffectiveDate',
			ProgramExpirationDate		DATE			'ProgramExpirationDate'
		);
		END

	INSERT INTO #OrganizationHierarchy
	(
		CompanyMappingID,
		CompanyDetailID,
		CompanyEffectiveDate,
		CompanyExpirationDate,
		DivisionMappingID,
		DivisionDetailID,
		DivisionEffectiveDate,
		DivisionExpirationDate,
		ProgramMappingID,
		ProgramDetailID,
		ProgramEffectiveDate,
		ProgramExpirationDate,
		ProgramUnitMappingID,
		ProgramUnitDetailID,
		ProgramUnitEffectiveDate,
		ProgramUnitExpirationDate
	)
	SELECT
		CompanyMappingID,
		CompanyDetailID,
		CompanyEffectiveDate,
		CompanyExpirationDate,
		DivisionMappingID,
		DivisionDetailID,
		DivisionEffectiveDate,
		DivisionExpirationDate,
		ProgramMappingID,
		ProgramDetailID,
		ProgramEffectiveDate,
		ProgramExpirationDate,
		ProgramUnitMappingID,
		ProgramUnitDetailID,
		ProgramUnitEffectiveDate,
		ProgramUnitExpirationDate	
	FROM OPENXML(@docHandle, N'/OrganizationDetails/Hierarchy/Companies/Company/Divisions/Division/Programs/Program/ProgramUnits/ProgramUnit')
	WITH
 	(
		CompanyMappingID			BIGINT			'..//..//..//..//..//../CompanyMappingID',
		CompanyDetailID				BIGINT			'..//..//..//..//..//../CompanyDetailID',
		CompanyEffectiveDate		DATE			'..//..//..//..//..//../CompanyEffectiveDate',
		CompanyExpirationDate		DATE			'..//..//..//..//..//../CompanyExpirationDate',
		DivisionMappingID			BIGINT			'..//..//..//../DivisionMappingID',
		DivisionDetailID			BIGINT			'..//..//..//../DivisionDetailID',
		DivisionEffectiveDate		DATE			'..//..//..//../DivisionEffectiveDate',
		DivisionExpirationDate		DATE			'..//..//..//../DivisionExpirationDate',
		ProgramMappingID			BIGINT			'..//../ProgramMappingID',
		ProgramDetailID				BIGINT			'..//../ProgramDetailID',
		ProgramEffectiveDate		DATE			'..//../ProgramEffectiveDate',
		ProgramExpirationDate		DATE			'..//../ProgramExpirationDate',
		ProgramUnitMappingID		BIGINT			'ProgramUnitMappingID',
		ProgramUnitDetailID			BIGINT			'ProgramUnitDetailID',
		ProgramUnitEffectiveDate	DATE			'ProgramUnitEffectiveDate',
		ProgramUnitExpirationDate	DATE			'ProgramUnitExpirationDate'
	);

	--Company Mapping
	UPDATE #OrganizationHierarchy
	SET CompanyMappingID = 1,
		CompanyDetailID = 1,
		DivisionMappingID = CASE WHEN DivisionDetailID IS NOT NULL AND DivisionMappingID IS NULL THEN 0 ELSE DivisionMappingID END,
		ProgramMappingID = CASE WHEN ProgramDetailID IS NOT NULL AND ProgramMappingID IS NULL THEN 0 ELSE ProgramMappingID END,
		ProgramUnitMappingID = CASE WHEN ProgramUnitDetailID IS NOT NULL AND ProgramUnitMappingID IS NULL THEN 0 ELSE ProgramUnitMappingID END

	UPDATE OH
	SET CompanyMappingID = CASE
			WHEN DATEDIFF(DAY, ISNULL(ODM.ExpirationDate, '12/31/2099'), @ModifiedOn) < 0 THEN ODM.MappingID
			ELSE 0 END,
		UpdateCompanyDates = CASE
			WHEN CompanyEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), CompanyEffectiveDate) <> 0 THEN 1
			WHEN CompanyExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), CompanyExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.CompanyDetailID = ODM.DetailID
	WHERE
		OH.CompanyDetailID = ODM.DetailID
		AND DATEDIFF(DAY, @ModifiedOn, ODM.ExpirationDate) > 0
		AND ISNULL(OH.CompanyMappingID, 0) = 0;

	UPDATE OH
	SET UpdateCompanyDates = CASE
			WHEN CompanyEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), CompanyEffectiveDate) <> 0 THEN 1
			WHEN CompanyExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), CompanyExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.CompanyMappingID = ODM.MappingID
	WHERE
		OH.CompanyMappingID = ODM.MappingID;

	--Division Mapping
	UPDATE OH
	SET DivisionMappingID = CASE
			WHEN DATEDIFF(DAY, ISNULL(ODM.ExpirationDate, '12/31/2099'), @ModifiedOn) < 0 THEN ODM.MappingID
			ELSE 0 END,
		UpdateDivisionDates = CASE
			WHEN DivisionEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), DivisionEffectiveDate) <> 0 THEN 1
			WHEN DivisionExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), DivisionExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.DivisionDetailID = ODM.DetailID
			AND OH.CompanyMappingID = ODM.ParentID
	WHERE
		OH.DivisionDetailID = ODM.DetailID
		AND OH.CompanyMappingID = ODM.ParentID
		AND DATEDIFF(DAY, @ModifiedOn, ODM.ExpirationDate) > 0
		AND ISNULL(OH.DivisionMappingID, 0) = 0;

	UPDATE OH
	SET UpdateDivisionDates = CASE
			WHEN DivisionEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), DivisionEffectiveDate) <> 0 THEN 1
			WHEN DivisionExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), DivisionExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.DivisionMappingID = ODM.MappingID
	WHERE
		OH.DivisionMappingID = ODM.MappingID;

	--Program Mapping
	UPDATE OH
	SET ProgramMappingID = CASE
			WHEN DATEDIFF(DAY, ISNULL(ODM.ExpirationDate, '12/31/2099'), @ModifiedOn) < 0 THEN ODM.MappingID
			ELSE 0 END,
		UpdateProgramDates = CASE
			WHEN ProgramEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), ProgramEffectiveDate) <> 0 THEN 1
			WHEN ProgramExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), ProgramExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.ProgramDetailID = ODM.DetailID
			AND OH.DivisionMappingID = ODM.ParentID
	WHERE
		OH.ProgramDetailID = ODM.DetailID
		AND OH.DivisionMappingID = ODM.ParentID
		AND DATEDIFF(DAY, @ModifiedOn, ODM.ExpirationDate) > 0
		AND ISNULL(OH.ProgramMappingID, 0) = 0;

	UPDATE OH
	SET UpdateProgramDates = CASE
			WHEN ProgramEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), ProgramEffectiveDate) <> 0 THEN 1
			WHEN ProgramExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), ProgramExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.ProgramMappingID = ODM.MappingID
	WHERE
		OH.ProgramMappingID = ODM.MappingID;

	--Program Mapping
	UPDATE OH
	SET ProgramUnitMappingID = CASE
			WHEN DATEDIFF(DAY, ISNULL(ODM.ExpirationDate, '12/31/2099'), @ModifiedOn) < 0 THEN ODM.MappingID
			ELSE 0 END,
		UpdateProgramUnitDates = CASE
			WHEN ProgramUnitEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), ProgramUnitEffectiveDate) <> 0 THEN 1
			WHEN ProgramUnitExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), ProgramUnitExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.ProgramUnitDetailID = ODM.DetailID
			AND OH.ProgramMappingID = ODM.ParentID
	WHERE
		OH.ProgramUnitDetailID = ODM.DetailID
		AND OH.ProgramMappingID = ODM.ParentID
		AND DATEDIFF(DAY, @ModifiedOn, ODM.ExpirationDate) > 0
		AND ISNULL(OH.ProgramUnitMappingID, 0) = 0;

	UPDATE OH
	SET UpdateProgramUnitDates = CASE
			WHEN ProgramUnitEffectiveDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(EffectiveDate, '1900-01-01'), ProgramUnitEffectiveDate) <> 0 THEN 1
			WHEN ProgramUnitExpirationDate IS NOT NULL AND DATEDIFF(DAY, ISNULL(ExpirationDate, '1900-01-01'), ProgramUnitExpirationDate) <> 0 THEN 1
			ELSE 0 END
	FROM
		#OrganizationHierarchy OH
		INNER JOIN  Core.OrganizationDetailsMapping ODM
			ON OH.ProgramUnitMappingID = ODM.MappingID
	WHERE
		OH.ProgramUnitMappingID = ODM.MappingID;

	--Add new mappings
	DECLARE @PKID BIGINT, @LOOP INT;

	DECLARE @Mappings TABLE
	(
		RecordID INT IDENTITY(1,1),
		DetailID BIGINT,
		ParentID BIGINT,
		EffectiveDate DATE,
		ExpirationDate DATE,
		Completed BIT DEFAULT(0)
	);

	--Company Mappings
	INSERT INTO @Mappings
	(
		DetailID,
		ParentID,
		EffectiveDate,
		ExpirationDate
	)
	SELECT DISTINCT
		CompanyDetailID,
		NULL,
		CompanyEffectiveDate,
		CompanyExpirationDate
	FROM
		#OrganizationHierarchy
	WHERE
		CompanyMappingID = 0;

	SELECT @Loop = COUNT(*) FROM @Mappings WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = RecordID FROM @Mappings WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Core.OrganizationDetailsMapping
	(
		DetailID,
		ParentID,
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
		ParentID,
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
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationDetailsMapping', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationDetailsMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @Mappings
	SET Completed = 1
	WHERE
		RecordID = @PKID;

	UPDATE #OrganizationHierarchy
	SET CompanyMappingID = @ID
	FROM
		#OrganizationHierarchy OH
		INNER JOIN @Mappings M
			ON OH.CompanyDetailID = M.DetailID
	WHERE
		OH.CompanyDetailID = M.DetailID
		AND M.RecordID = @PKID
	END

	--Division Mappings
	DELETE FROM @Mappings

	INSERT INTO @Mappings
	(
		DetailID,
		ParentID,
		EffectiveDate,
		ExpirationDate
	)
	SELECT DISTINCT
		DivisionDetailID,
		CompanyMappingID,
		DivisionEffectiveDate,
		DivisionExpirationDate
	FROM
		#OrganizationHierarchy
	WHERE
		DivisionMappingID = 0;

	SELECT @Loop = COUNT(*) FROM @Mappings WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = RecordID FROM @Mappings WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Core.OrganizationDetailsMapping
	(
		DetailID,
		ParentID,
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
		ParentID,
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
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationDetailsMapping', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationDetailsMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @Mappings
	SET Completed = 1
	WHERE
		RecordID = @PKID;

	UPDATE #OrganizationHierarchy
	SET DivisionMappingID = @ID
	FROM
		#OrganizationHierarchy OH
		INNER JOIN @Mappings M
			ON OH.DivisionDetailID = M.DetailID
			AND OH.CompanyMappingID = M.ParentID
	WHERE
		OH.DivisionDetailID = M.DetailID
		AND OH.CompanyMappingID = M.ParentID
		AND M.RecordID = @PKID
	END

	--Program Mappings
	DELETE FROM @Mappings

	INSERT INTO @Mappings
	(
		DetailID,
		ParentID,
		EffectiveDate,
		ExpirationDate
	)
	SELECT DISTINCT
		ProgramDetailID,
		DivisionMappingID,
		ProgramEffectiveDate,
		ProgramExpirationDate
	FROM
		#OrganizationHierarchy
	WHERE
		ProgramMappingID = 0;

	SELECT @Loop = COUNT(*) FROM @Mappings WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = RecordID FROM @Mappings WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Core.OrganizationDetailsMapping
	(
		DetailID,
		ParentID,
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
		ParentID,
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

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationDetailsMapping', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationDetailsMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @Mappings
	SET Completed = 1
	WHERE
		RecordID = @PKID;

	UPDATE #OrganizationHierarchy
	SET ProgramMappingID = @ID
	FROM
		#OrganizationHierarchy OH
		INNER JOIN @Mappings M
			ON OH.ProgramDetailID = M.DetailID
			AND OH.DivisionMappingID = M.ParentID
	WHERE
		OH.ProgramDetailID = M.DetailID
		AND OH.DivisionMappingID = M.ParentID
		AND M.RecordID = @PKID
	END

	--Program Unit Mappings
	DELETE FROM @Mappings

	INSERT INTO @Mappings
	(
		DetailID,
		ParentID,
		EffectiveDate,
		ExpirationDate
	)
	SELECT DISTINCT
		ProgramUnitDetailID,
		ProgramMappingID,
		ProgramUnitEffectiveDate,
		ProgramUnitExpirationDate
	FROM
		#OrganizationHierarchy
	WHERE
		ProgramUnitMappingID = 0;

	SELECT @Loop = COUNT(*) FROM @Mappings WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = RecordID FROM @Mappings WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Core.OrganizationDetailsMapping
	(
		DetailID,
		ParentID,
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
		ParentID,
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
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationDetailsMapping', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationDetailsMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @Mappings
	SET Completed = 1
	WHERE
		RecordID = @PKID;

	UPDATE #OrganizationHierarchy
	SET ProgramUnitMappingID = @ID
	FROM
		#OrganizationHierarchy OH
		INNER JOIN @Mappings M
			ON OH.ProgramUnitDetailID = M.DetailID
			AND OH.ProgramMappingID = M.ParentID
	WHERE
		OH.ProgramUnitDetailID = M.DetailID
		AND OH.ProgramMappingID = M.ParentID
		AND M.RecordID = @PKID
	END

	--Update existing Mappings
	DECLARE @IDS TABLE (MappingID BIGINT, EffectiveDate DATE, ExpirationDate DATE, Completed BIT);

	INSERT INTO @IDS
	(
		MappingID,
		EffectiveDate,
		ExpirationDate,
		Completed
	)
	SELECT
		CompanyMappingID,
		CompanyEffectiveDate,
		CompanyExpirationDate,
		0
	FROM
		#OrganizationHierarchy OH
	WHERE
		OH.UpdateCompanyDates = 1
	UNION ALL
	SELECT
		DivisionMappingID,
		DivisionEffectiveDate,
		DivisionExpirationDate,
		0
	FROM
		#OrganizationHierarchy OH
	WHERE
		OH.UpdateDivisionDates = 1
	UNION ALL
	SELECT
		ProgramMappingID,
		ProgramEffectiveDate,
		ProgramExpirationDate,
		0
	FROM
		#OrganizationHierarchy OH
	WHERE
		OH.UpdateProgramDates = 1
	UNION ALL
	SELECT
		ProgramUnitMappingID,
		ProgramUnitEffectiveDate,
		ProgramUnitExpirationDate,
		0
	FROM
		#OrganizationHierarchy OH
	WHERE
		OH.UpdateProgramUnitDates = 1;

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = MappingID FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationDetailsMapping', @PKID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE ODM
	SET EffectiveDate = IDS.EffectiveDate,
		ExpirationDate = IDS.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Core.OrganizationDetailsMapping ODM
		INNER JOIN @IDS IDS
			ON ODM.MappingID = IDS.MappingID
	WHERE
		ODM.MappingID = IDS.MappingID
		AND IDS.MappingID = @PKID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationDetailsMapping', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @IDS
	SET Completed = 1
	WHERE
		MappingID = @PKID;
	END

	EXEC sp_xml_removedocument @docHandle;

	DROP TABLE #OrganizationHierarchy
  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END