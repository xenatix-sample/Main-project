
-----------------------------------------------------------------------------------------------------------------------
-- Author:		Atul Chauhan
-- Date:		12/07/2016
--
-- Purpose:		Update Payor Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/07/2016	Atul Chauhan  		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_UpdatePayorDetails]
	@PayorID INT,
	@PayorCode INT,
	@PayorName NVARCHAR(500),
	@PayorTypeID INT,
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'Payor', @PayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Reference].[Payor]
	SET	 PayorCode  = @PayorCode,
		 PayorName  = @PayorName,
		 PayorTypeID= @PayorTypeID,
		 EffectiveDate = @EffectiveDate,
		 ExpirationDate = @ExpirationDate,
		 ModifiedBy = @ModifiedBy,
		 ModifiedOn = @ModifiedOn,
		 SystemModifiedOn = GETUTCDATE()
	WHERE
		PayorID = @PayorID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'Payor', @AuditDetailID, @PayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
