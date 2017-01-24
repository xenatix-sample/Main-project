-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_UpdateContactRelationshipType]
-- Author:		Lokesh Singhal
-- Date:		06/08/2016
--
-- Purpose:		Update Contact RelationshipType Data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Lokesh Singhal	Initial creation.
-- 09/14/2016    Arun Choudhary	Added EffectiveDate and ExpirationDate
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactRelationshipType]
	@ContactRelationshipTypeID BIGINT,
	@ParentContactID BIGINT,
	@ContactID BIGINT,
	@RelationshipTypeID INT ,
	@IsPolicyHolder BIT ,
	@OtherRelationship NVARCHAR (200),
	@EffectiveDate DATE,
	@ExpirationDate DATE,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactRelationshipID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactRelationshipType', @ContactRelationshipTypeID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactRelationshipType]
	SET	[RelationshipTypeID] = @RelationshipTypeID,
		[IsPolicyHolder] = @IsPolicyHolder,
		[OtherRelationship] = @OtherRelationship,
		[EffectiveDate] = @EffectiveDate,
		ExpirationDate = @ExpirationDate,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactRelationshipTypeID = @ContactRelationshipTypeID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactRelationshipType', @AuditDetailID, @ContactRelationshipTypeID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


