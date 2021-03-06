-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddPayorDetails]
-- Author:		John Crossen
-- Date:		08/20/2015
--
-- Purpose:		Add Payor lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

-- 09/04/2015	John Crossen		TFS# 1882 - Initial creation.
-- 9/23/2015    John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-- 12/13/2016	Sumana Sangapu		Altered the procedure to accept PayorTypeID and remove PayorRank
-- 12/16/2016	Atul Chauhan		Inserted PayorRank as 0.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_AddPayorDetails]
	@PayorName NVARCHAR(250),
	@PayorCode INT,
	@PayorTypeID INT,
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@PayorID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
	IF NOT EXISTS (SELECT 'x' FROM Reference.Payor WHERE PayorName = @PayorName and PayorCode = @PayorCode and PayorTypeID = @PayorTypeID)
		BEGIN
		INSERT INTO Reference.Payor
		(
			PayorCode ,
			PayorName ,
			PayorTypeID ,
			EffectiveDate ,
			ExpirationDate ,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		) 
		VALUES
		(
			ISNULL(@PayorCode, ''),
			@PayorName,
			@PayorTypeID,
			ISNULL(@EffectiveDate, CAST(GETUTCDATE() AS DATE)),
			@ExpirationDate,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @PayorID = SCOPE_IDENTITY();
		
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'Payor', @PayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'Payor', @AuditDetailID, @PayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END
	ELSE
		BEGIN
		SELECT
			@PayorID = PayorID
		FROM
			Reference.Payor
		WHERE
			PayorName = @PayorName;
		END
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


