
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
-- 12/16/2016	Atul chauhan		Removed Expiration reason.
-- 12/20/2016	Sumana Sangapu		Added Rules for Effective and Expiration Dates
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_UpdatePayorDetails]
	@PayorID INT,
	@PayorName NVARCHAR(500),
	@PayorCode INT,
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
	SET	 
		 PayorName  = @PayorName,
		 PayorCode  = @PayorCode,
		 PayorTypeID= @PayorTypeID,
		 EffectiveDate = @EffectiveDate,
		 ExpirationDate = @ExpirationDate,
		 ModifiedBy = @ModifiedBy,
		 ModifiedOn = @ModifiedOn,
		 SystemModifiedOn = GETUTCDATE()
	WHERE
		PayorID = @PayorID;

	DECLARE @tblPayorPlanEffective TABLE (PayorPlanID BIGINT);
	DECLARE @tblPayorPlanExpiration TABLE (PayorPlanID BIGINT);

	-- Update the ExpirationDate of Reference.PayorPlan 
	UPDATE pp
	SET		ExpirationDate = @ExpirationDate
	OUTPUT  inserted.PayorPlanID
	INTO	@tblPayorPlanExpiration
	FROM	Reference.PayorPlan pp 
	INNER JOIN Reference.Payor p 
	ON		p.PayorID = pp.PayorID
	WHERE	p.PayorID =  @PayorID 
	AND		( ISNULL(pp.ExpirationDate,GETDATE()) >= @ExpirationDate OR pp.ExpirationDate IS NULL )

	-- Update the ExpirationDate of Registration.PayorAddress
	UPDATE  pa
	SET		ExpirationDate = @ExpirationDate
	FROM	Registration.PayorAddress pa
	INNER JOIN @tblPayorPlanExpiration pp
	ON		pa.PayorPlanID = pp.PayorPlanID
	WHERE	( ISNULL(pa.ExpirationDate,GETDATE()) >= @ExpirationDate OR pa.ExpirationDate IS NULL )	
	
	-- Update the EffectiveDate of Reference.PayorPlan 
	UPDATE pp
	SET		EffectiveDate = @EffectiveDate
	OUTPUT  inserted.PayorPlanID
	INTO	@tblPayorPlanEffective
	FROM	Reference.PayorPlan pp 
	INNER JOIN Reference.Payor p 
	ON		p.PayorID = pp.PayorID
	WHERE	p.PayorID =  @PayorID 
	AND		( ISNULL(pp.EffectiveDate,GETDATE()) <= @EffectiveDate OR pp.EffectiveDate IS NULL )

	-- Update the EffectiveDate of Registration.PayorAddress
	UPDATE  pa
	SET		EffectiveDate = @EffectiveDate
	FROM	Registration.PayorAddress pa
	INNER JOIN @tblPayorPlanEffective pp
	ON		pa.PayorPlanID = pp.PayorPlanID
	WHERE	( ISNULL(pa.EffectiveDate,GETDATE()) <= @EffectiveDate OR pa.EffectiveDate IS NULL )

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'Payor', @AuditDetailID, @PayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

