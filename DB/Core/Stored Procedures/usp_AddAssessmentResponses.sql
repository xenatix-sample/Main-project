
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddResponsesAndDetails]
-- Author:		Sumana Sangapu
-- Date:		10/01/2015
--
-- Purpose:		Add Financial Assessment Details in Finance Screen
--
-- Notes:		
--				
-- Depends:		Registration.Contact
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/07/2015   Sumana Sangapu	1601 - Initial Creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 02/16/2016	Kyle Campbell		Changed EnterDate from DATE to DATETIME to be consistent with UpdateAssessmentResponses proc
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddAssessmentResponses]
	@ResponseID BIGINT,
	@ContactID BIGINT,
	@AssessmentID BIGINT,
	@EnterDate DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
		
		SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully';

		BEGIN TRY
	INSERT INTO [Core].[AssessmentResponses]
	(
		ContactID,
		AssessmentID,
		EnterDate,
		EnterDateINT,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@AssessmentID,
		@EnterDate,
		CAST(CONVERT(VARCHAR(20), @EnterDate, 112) AS INT),
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'AssessmentResponses', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'AssessmentResponses', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END
