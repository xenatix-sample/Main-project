-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_AddEligibility]
-- Author:		Sumana Sangapu
-- Date:		10/13/2015
--
-- Purpose:		Add Contact's ECI Eligiblity Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/13/2015	Sumana Sangapu	TFS:2700	Initial Creation
-- 10/30/2015   Justin Spalti - Corrected a bug with the XML parsing
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_AddEligibility]
	@ContactID BIGINT,
	@EligibilityDate DATE,
	@EligibilityTypeID int,
	@EligibilityDurationID int,
	@EligibilityCategoryID int,
	@EligibilityXML xml,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@NewID BIGINT;

 SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	INSERT INTO  [ECI].[Eligibility]
	(
		ContactID,
		EligibilityDate,
		EligibilityTypeID,
		EligibilityDurationID,
		EligibilityCategoryID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES 
	(
		@ContactID,
		@EligibilityDate,
		@EligibilityTypeID,
		@EligibilityDurationID,
		@EligibilityCategoryID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
			
	SELECT @ID =  SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'Eligibility', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'Eligibility', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

----------------------------------Eligibility Team Discipline--------------------------------------------------------------------------

	INSERT INTO [ECI].[EligibilityTeamDiscipline]
	(
		EligibilityID,
		UserID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		@ID as EligibilityID,
		T.C.value('(.)[1]','int') as UserID,
		1, 
		@ModifiedBy, 
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		@EligibilityXML.nodes('EligibilityDetails/UserID') AS T(C);

	SELECT @NewID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ECI', 'EligibilityTeamDiscipline', @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ECI', 'EligibilityTeamDiscipline', @AuditDetailID, @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 END TRY
  BEGIN CATCH
    SELECT
	  @ID = 0,
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH
END