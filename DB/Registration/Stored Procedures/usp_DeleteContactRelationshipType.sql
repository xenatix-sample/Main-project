-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_DeleteContactRelationshipType]
-- Author:		Lokesh Singhal
-- Date:		06/08/2016
--
-- Purpose:		Delete contact RelationshipType
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Lokesh Singhal	Initial creation.
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactRelationshipType]
	@ContactRelationshipTypeID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = 1 FROM Registration.ContactRelationshipType CRT INNER JOIN Registration.ContactRelationship CR ON CRT.ContactRelationshipID = CR.ContactRelationshipID WHERE CRT.ContactRelationshipTypeID = @ContactRelationshipTypeID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactRelationshipType', @ContactRelationshipTypeID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactRelationshipType
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactRelationshipTypeID = @ContactRelationshipTypeID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactRelationshipType', @AuditDetailID, @ContactRelationshipTypeID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
	
 	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


