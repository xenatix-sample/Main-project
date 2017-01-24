-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddContactAllergyDetail]
-- Author:		John Crossen
-- Date:		11/13/2015
--
-- Purpose:		Update Contact Allergy
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	John Crossen	TFS# 3566 - Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 11/20/2015   Justin Spalti - Removed References to allergy symptoms
-- 11/23/2015   Justin Spalti - Update the parameter list to match the new UI models(temporarily commented the audit logic)
-- 11/24/2015    Justin Spalti - Updated the proc to return the new Identity ID
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddContactAllergyDetail]
	@AllergySymptomXML XML,
	@ContactAllergyID BIGINT,
	@AllergyID INT,
	@SeverityID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @NewID BIGINT,
		@AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

DECLARE @CADS_ID TABLE (ID BIGINT);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO [Clinical].[ContactAllergyDetail]
    (
		[ContactAllergyID],
		[AllergyID],
		[SeverityID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn]
	)
	OUTPUT inserted.ContactAllergyDetailID
	VALUES(@ContactAllergyID,
		   @AllergyID,
		   @SeverityID,
		   1,
		   @ModifiedBy,
		   @ModifiedOn,
		   @ModifiedBy,
		   @ModifiedOn
		   )

	SELECT @ID = SCOPE_IDENTITY()

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'ContactAllergyDetail', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'ContactAllergyDetail', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;


	INSERT INTO [Clinical].ContactAllergyDetailSymptoms
	(
		ContactAllergyDetailID,
		AllergySymptomID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		[CreatedBy],
		[CreatedOn]
	)
	OUTPUT inserted.ContactAllergyDetailSymptomID
	INTO @CADS_ID
	SELECT
		@ID,
		T.C.value('(.)[1]','int'),
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		@AllergySymptomXML.nodes('SymptomDetails/AllergySymptomID') AS T(C);

	DECLARE @AuditCursor CURSOR;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID FROM @CADS_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @NewID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'ContactAllergyDetailSymptoms', @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'ContactAllergyDetailSymptoms', @AuditDetailID, @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @NewID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;	
  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END

GO


