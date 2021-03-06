
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddPayorPlanDetails]
-- Author:		John Crossen
-- Date:		08/20/2015
--
-- Purpose:		Add Payor Group Plan lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

-- 09/04/2015	John Crossen		TFS# 1882 - Initial creation.
--09/08/2015	Gurpreet Singh		Saved @GroupID, @PlanID, @PlanName, @GrouPlanAddressID, @EffectiveDate, @ExpirationDate
-- 09/15/2015   John Crossen		-- Move data elements to ContactBenefit from Group Payor
-- 09/21/2015   Arun Choudhary      Added check for planID and payorID
-- 9/23/2015    John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-- 12/13/2016	Sumana Sangapu		Add EffectiveDate and ExpirationDate
-- 12/16/2016	Atul Chauhan		Changed the order of PlanID and PlanName to make it in sync with update Stored Prodecure
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_AddPayorPlanDetails]
	@PayorID INT ,
	@PlanName NVARCHAR(250),
	@PlanID NVARCHAR(50),
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@PayorPlanID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT
		@PayorPlanID = PayorPlanID
	FROM
		Reference.[PayorPlan]
	WHERE
		[PlanName] = @PlanName
		AND [PlanID] = @PlanID
		AND [PayorID] = @PayorID;

	IF (ISNULL(@PayorPlanID, 0) = 0)
		BEGIN
		INSERT INTO [Reference].[PayorPlan]
		(
			[PayorID],
			[PlanName],
			[PlanID],
			[EffectiveDate],
			[ExpirationDate],
			[IsActive],
			[ModifiedBy],
			[ModifiedOn],
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@PayorID,
			@PlanName,
			@PlanID,
			@EffectiveDate,
			@ExpirationDate,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @PayorPlanID = SCOPE_IDENTITY();

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'PayorPlan', @PayorPlanID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'PayorPlan', @AuditDetailID, @PayorPlanID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END