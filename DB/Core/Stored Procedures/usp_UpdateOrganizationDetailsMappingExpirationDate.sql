-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationDetailsMappingByDetailID]
-- Author:		Scott Martin
-- Date:		01/05/2017
--
-- Purpose:		Update the expiration date on all mappings linked to an Organization Detail. This includes all child mappings.
--
-- Notes:		n/a
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/18/2017	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_UpdateOrganizationDetailsMappingExpirationDate
	@DetailID BIGINT,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully';

	DECLARE	@ExistingExpirationDate DATE

	SELECT @ExistingExpirationDate = ExpirationDate FROM Core.OrganizationDetails WHERE DetailID = @DetailID;

	;WITH Mappings (MappingID, ExpirationDate)
	AS
	(
		SELECT
			MappingID,
			CASE
				WHEN MappingExpirationDate IS NULL THEN @ExpirationDate
				WHEN DATEDIFF(DAY, MappingExpirationDate, @ExistingExpirationDate) = 0 AND DATEDIFF(DAY, @ModifiedOn, MappingExpirationDate) >= 0 THEN @ExpirationDate
				WHEN DATEDIFF(DAY, MappingExpirationDate, @ExpirationDate) < 0 AND DATEDIFF(DAY, @ModifiedOn, MappingExpirationDate) > 0 THEN @ExpirationDate
				ELSE NULL END
		FROM
			Core.vw_GetOrganizationStructureDetails
		WHERE
			DetailID = @DetailID
		UNION ALL
		SELECT
			OSD.MappingID,
			CASE
				WHEN MappingExpirationDate IS NULL THEN @ExpirationDate
				WHEN DATEDIFF(DAY, MappingExpirationDate, @ExistingExpirationDate) = 0 AND DATEDIFF(DAY, @ModifiedOn, MappingExpirationDate) >= 0 THEN @ExpirationDate
				WHEN DATEDIFF(DAY, MappingExpirationDate, @ExpirationDate) < 0 AND DATEDIFF(DAY, @ModifiedOn, MappingExpirationDate) > 0 THEN @ExpirationDate
				ELSE NULL END
		FROM
			Mappings M
			INNER JOIN Core.vw_GetOrganizationStructureDetails OSD
				ON M.MappingID = OSD.ParentID
	)
	SELECT
		MappingID,
		ExpirationDate,
		0 AS PreUpdate,
		0 AS PostUpdate
	INTO #Mappings
	FROM
		Mappings
	WHERE
		ExpirationDate IS NOT NULL
	ORDER BY
		MappingID;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) from #Mappings WHERE PreUpdate = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = MappingID FROM #Mappings WHERE PreUpdate = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationDetailsMapping', @PKID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;

	UPDATE #Mappings
	SET PreUpdate = 1
	WHERE MappingID = @PKID

	END

	UPDATE ODM
	SET ExpirationDate = M.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		Core.OrganizationDetailsMapping ODM
		INNER JOIN #Mappings M
			ON ODM.MappingID = M.MappingID
	WHERE
		ODM.MappingID = M.MappingID;

	SELECT @Loop = COUNT(*) from #Mappings WHERE PostUpdate = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = MappingID FROM #Mappings WHERE PostUpdate = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationDetailsMapping', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;

	UPDATE #Mappings
	SET PostUpdate = 1
	WHERE MappingID = @PKID

	END

	DROP TABLE #Mappings
		
  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO