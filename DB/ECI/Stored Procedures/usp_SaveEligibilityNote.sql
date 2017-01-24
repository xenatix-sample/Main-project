-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_SaveEligibilityNote]
-- Author:		Justin Spalti
-- Date:		11/03/2015
--
-- Purpose:		Get Contact's ECI Eligibility Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/03/2015	Justin Spalti	TFS:2700	Initial Creation
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_SaveEligibilityNote]
    @EligibilityID BIGINT,
	@Notes NVARCHAR(500),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
    @ResultCode int OUTPUT,
    @ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT @ResultCode = 0,
		@ResultMessage = 'Executed Successfully';

    BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'Eligibility', @EligibilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ECI.Eligibility
	SET Notes = @Notes,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		EligibilityID = @EligibilityID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'Eligibility', @AuditDetailID, @EligibilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

    END TRY
    BEGIN CATCH
        SELECT @ResultCode = ERROR_SEVERITY(),
                @ResultMessage = ERROR_MESSAGE()
    END CATCH
END