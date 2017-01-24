-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_DeleteContactAllergyDetail]
-- Author:		John Crossen
-- Date:		11/13/2015
--
-- Purpose:		Delete Contact Allergy
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	John Crossen	TFS# 3566 - Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 12/06/2015   Justin Spalti - Added the additional ReasonForDeletion parameter
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_DeleteContactAllergyDetail]
	@ContactAllergyDetailID BIGINT,
	@ReasonForDeletion VARCHAR(250),
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Clinical', 'ContactAllergyDetail', @ContactAllergyDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.ContactAllergyDetail
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactAllergyDetailID = @ContactAllergyDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Clinical', 'ContactAllergyDetail', @AuditDetailID, @ContactAllergyDetailID, @ReasonForDeletion, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO