

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddCrisisLine]
-- Author:		John Crossen
-- Date:		01/27/2016
--
-- Purpose:		Insert for Crisis Line
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/27/2016	John Crossen   5714	- Initial creation.
-- 02/01/2016	Gurpreet Singh	Refactoring sproc
-- 02/16/2016	Rajiv Ranjan	Added providerID & dateOfIncident
-- 02/16/2016	Kyle Campbell	Added proc calls for Audit
-- 03/03/2016	Gaurav Gupta	Added referral agency id
-- 04/27/2016                   Added OtherReferralAgency
-- 05/10/2016   Lokesh Singhal  Added IsLinkedToContact
-- 07/07/2016	Rajiv Ranjan	Increased length of comments and disposition
-- 12/15/2016	Scott Martin	Updated auditing
-- 12/28/2016	Arun choudhary	Increased length of OtherInformation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_AddCrisisLine]
	@ParentCallCenterHeaderID BIGINT = NULL,
	@CallCenterTypeID SMALLINT,
	@CallerID BIGINT NULL,
	@ClientID BIGINT NULL,
	@ContactTypeID INT NULL,
	@ProviderID BIGINT NULL,
	@CallStartTime DATETIME,
	@CallEndTime DATETIME,
	@CallStatusID SMALLINT NULL,
	@ProgramUnitID BIGINT,
	@ReferralAgencyID INT,
	@OtherReferralAgency  NVARCHAR(500),
	@CountyID INT NULL,
	@SuicideHomicideID SMALLINT,
	@CallCenterPriorityID SMALLINT NULL,
	@DateOfIncident DATETIME NULL,
	@ReasonCalled NVARCHAR(4000),
	@Disposition  NVARCHAR(MAX),
	@OtherInformation  NVARCHAR(4000) NULL,
	@Comments  NVARCHAR(MAX) NULL,
	@FollowUpRequired BIT NULL,
	@ModifiedOn DATETIME,
	@IsLinkedToContact BIT,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditPost XML,
		@AuditID BIGINT;

DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
		-- Add CallCenterHeader record
		INSERT INTO CallCenter.CallCenterHeader
				(ParentCallCenterHeaderID,
				 CallCenterTypeID,
				 CallerID,
				 ContactID,
				 ContactTypeID,
				 ProviderID,
				 CallStartTime,
				 CallEndTime,
				 CallStatusID,
				 ProgramUnitID,
				 CountyID,
				 IsActive,
				 ModifiedBy,
				 ModifiedOn,
				 CreatedBy,
				 CreatedOn,
				 IsLinkedToContact)
		VALUES  (@ParentCallCenterHeaderID,
				@CallCenterTypeID,
				@CallerID,
				@ClientID,
				@ContactTypeID,
				@ProviderID,
				@CallStartTime,
				@CallEndTime,
				@CallStatusID,
				@ProgramUnitID,
				@CountyID,
				1,
				@ModifiedBy,
				@ModifiedOn,
				@ModifiedBy,
				@ModifiedOn,
				@IsLinkedToContact)

		SELECT @ID = SCOPE_IDENTITY()

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CallCenterHeader', @ID, NULL, NULL, NULL, @ClientID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		-- Add CrisisLine record		
		INSERT INTO CallCenter.CrisisCall
				( CallCenterHeaderID,
				  CallCenterPriorityID,
				  SuicideHomicideID,
				  ReferralAgencyID,
				  OtherReferralAgency,
				  DateOfIncident,
				  ReasonCalled,
				  Disposition,
				  OtherInformation,
				  Comments,
				  FollowUpRequired,
				  IsActive,
				  ModifiedBy,
				  ModifiedOn,
				  CreatedBy,
				  CreatedOn)
		VALUES (@ID, 
				@CallCenterPriorityID, 
				@SuicideHomicideID,
				@ReferralAgencyID, 
				@OtherReferralAgency,
				@DateOfIncident,
				@ReasonCalled, 
				@Disposition, 
				@OtherInformation, 
				@Comments,
				@FollowUpRequired,
				1,
				@ModifiedBy,
				@ModifiedOn, 
				@ModifiedBy, 
				@ModifiedOn)		

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CrisisCall', @ID, NULL, NULL, NULL, @ClientID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CrisisCall', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;


END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END


