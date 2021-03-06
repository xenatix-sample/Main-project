-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_DeleteScreening
-- Author:		Sumana Sangapu
-- Date:		10/08/2015
--
-- Purpose:		Deactivates the Screening
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	Sumana Sangapu	TFS:2620	Initial Creation
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_DeleteScreening]
	@ScreeningID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

DECLARE @AD_ID TABLE (ID BIGINT);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'Screening', @ScreeningID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [ECI].[Screening]
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ScreeningID = @ScreeningID

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'Screening', @AuditDetailID, @ScreeningID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

----------------------------------Screening Contact------------------------------------------------------------------------
	
	DECLARE @ScreeningContactID BIGINT;

	SELECT @ScreeningContactID = ScreeningContactID FROM ECI.ScreeningContact WHERE ScreeningID = @ScreeningID AND IsActive = 1;

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'ScreeningContact', @ScreeningContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [ECI].[ScreeningContact]
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ScreeningID = @ScreeningID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'ScreeningContact', @AuditDetailID, @ScreeningContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END