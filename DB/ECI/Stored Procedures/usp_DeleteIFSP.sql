-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_DeleteIFSP
-- Author:		Sumana Sangapu
-- Date:		10/27/2015
--
-- Purpose:		Deactivates the IFSP
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/27/2015	Sumana Sangapu	TFS:3037	Initial Creation
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_DeleteIFSP]
	@IFSPID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'IFSP', @IFSPID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE	[ECI].[IFSP]
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		IFSPID = @IFSPID

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'IFSP', @AuditDetailID, @IFSPID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END