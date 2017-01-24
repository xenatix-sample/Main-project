-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAssessmentResponses]
-- Author:		Sumana Sangapu
-- Date:		10/01/2015
--
-- Purpose:		Update Assessment Response in the Response table
--
-- Notes:		
--				
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/07/2015   Sumana Sangapu	1601 - Initial Creation

-- exec [Core].[usp_UpdateAssessmentResponses] '1','3','2015-10-01','1',' ','',''
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 12/15/2016	Scott Martin		Update auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateAssessmentResponses]
	@ResponseID BIGINT, 
	@EnterDate DATETIME,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Core.AssessmentResponses WHERE ResponseID = @ResponseID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'AssessmentResponses', @ResponseID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE a
	SET EnterDate = @EnterDate,
		EnterDateINT = CAST(CONVERT(VARCHAR(20),@EnterDate,112) AS INT),
		IsActive = @IsActive,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		[Core].[AssessmentResponses] a
	WHERE
		ResponseID = @ResponseID;
	
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'AssessmentResponses', @AuditDetailID, @ResponseID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
