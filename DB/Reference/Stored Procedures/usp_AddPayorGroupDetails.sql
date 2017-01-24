
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddPayorGroupDetails]
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
-- 09/15/2015   John Crossen    -- Move data elements to ContactBenefit from Group Payor
-- 09/21/2015   Arun Choudhary     Added check for groupID and payor planID
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_AddPayorGroupDetails]
	@PayorPlanID INT ,
	@GroupID NVARCHAR(50) NULL,
	@GroupName NVARCHAR(250) NULL,
	@IsActive BIT NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@PayorGroupID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		@PayorGroupID = @PayorGroupID 
	FROM
		Reference.PayorGroup
	WHERE
		GroupName = @GroupName
		AND GroupID = @GroupID
		AND PayorPlanID = @PayorPlanID;

	IF (ISNULL(@PayorGroupID,0)=0)
		BEGIN
		INSERT INTO [Reference].[PayorGroup]
		(
			PayorPlanID,
			GroupID,
			GroupName,
			[IsActive],
			[ModifiedBy],
			[ModifiedOn],
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@PayorPlanID,
			@GroupID,
			@GroupName,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @PayorGroupID = SCOPE_IDENTITY();

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'PayorGroup', @PayorGroupID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'PayorGroup', @AuditDetailID, @PayorGroupID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END