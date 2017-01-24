-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_UpdatePayorPlanDetails]
-- Author:		Sumana Sangapu
-- Date:		12/07/2016
--
-- Purpose:		Update PayorPlan Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/07/2016	Sumana Sangapu	Initial creation.
-- 01/16/2017	Fixed join 'ON' statement to return correct number of records.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_UpdatePayorPlanDetails]
	@PayorPlanID INT,
	@PayorID INT,
	@PlanName NVARCHAR(250),
	@PlanID	NVARCHAR(50),
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@IsActive BIT,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'PayorPlan', @PayorPlanID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Reference].[PayorPlan]
	SET	 PayorID = @PayorID,
		 PlanName = @PlanName,
		 PlanID = @PlanID,
		 EffectiveDate = @EffectiveDate,
		 ExpirationDate = @ExpirationDate,
		 IsActive = @IsActive,
		 ModifiedBy = @ModifiedBy,
		 ModifiedOn = @ModifiedOn,
		 SystemModifiedOn = GETUTCDATE()
	WHERE
		 PayorPlanID = @PayorPlanID;

	-- Update the ExpirationDate of Registration.PayorAddress
	UPDATE  pa
	SET		ExpirationDate = @ExpirationDate
	FROM	Registration.PayorAddress pa 
	INNER JOIN Reference.PayorPlan pp
	ON		pp.PayorPlanID = pa.PayorPlanID
	WHERE	pp.PayorPlanID =  @PayorPlanID 
	AND		( ISNULL(pp.ExpirationDate,GETDATE()) >= @ExpirationDate OR pp.ExpirationDate IS NULL )

	-- Update the EffectiveDate of Registration.PayorAddress
	UPDATE  pa
	SET		EffectiveDate = @EffectiveDate
	FROM	Registration.PayorAddress pa 
	INNER JOIN Reference.PayorPlan pp
	ON		pp.PayorPlanID = pa.PayorPlanID
	WHERE	pp.PayorPlanID =  @PayorPlanID 
	AND		( ISNULL(pp.EffectiveDate,GETDATE()) <= @EffectiveDate OR pp.EffectiveDate IS NULL )

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'PayorPlan', @AuditDetailID, @PayorPlanID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END