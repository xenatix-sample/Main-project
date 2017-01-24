

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateCrisisLine]
-- Author:		John Crossen
-- Date:		01/27/2016
--
-- Purpose:		Update for Crisis Line
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/27/2016	John Crossen   5714	- Initial creation.
-- 02/01/2016	Gurpreet Singh	Refactoring sproc
-- 02/16/2016	Rajiv Ranjan	Added providerID & dateOfIncident
-- 02/17/2016	Kyle Campbell	Added audit proc calls
-- 03/03/2016   Gaurav Gupta    Added referral agency id
-- 04/27/2016                   Added OtherReferralAgency
-- 05/10/2016   Lokesh Singhal  Added IsLinkedToContact
-- 07/07/2016	Rajiv Ranjan	Increased length of comments and disposition
-- 08/17/2016	Deepak Kumar	Modified the contactid for child in crisis line
-- 12/15/2016	Scott Martin	Updated auditing
-- 12/28/2016	Arun choudhary	Increased length of OtherInformation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_UpdateCrisisLine]
	@CallCenterHeaderID BIGINT,
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
	@OtherReferralAgency NVARCHAR(500),
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
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY

		DECLARE @ClientContactID BIGINT
				,@CallerContactID BIGINT
		Declare @OldContactID BIGINT
		SELECT @OldContactID=ContactID FROM CallCenter.CallCenterHeader WHERE CallCenterHeaderID=@CallCenterHeaderID

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CallCenterHeader', @CallCenterHeaderID, NULL, NULL, NULL, @ClientID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			UPDATE CallCenter.CallCenterHeader 
			SET CallerID = @CallerID, 
				ContactID = @ClientID, 
				ContactTypeID = @ContactTypeID,
				ProviderID=@ProviderID,
				CallStartTime = @CallStartTime,
				CallEndTime = @CallEndTime, 
				CallStatusID = @CallStatusID,
				ProgramUnitID = @ProgramUnitID, 
				CountyID = @CountyID, 
				ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn = GETUTCDATE(),
				IsLinkedToContact = @IsLinkedToContact
			WHERE CallCenterHeaderID = @CallCenterHeaderID

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CrisisCall', @CallCenterHeaderID, NULL, NULL, NULL, @ClientID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			UPDATE CallCenter.CrisisCall 
			SET CallCenterPriorityID = @CallCenterPriorityID, 
				ReferralAgencyID =@ReferralAgencyID,
				OtherReferralAgency = @OtherReferralAgency,
				SuicideHomicideID = @SuicideHomicideID,
				DateOfIncident = @DateOfIncident,
				ReasonCalled = @ReasonCalled, 
				Disposition = @Disposition, 
				OtherInformation = @OtherInformation, 
				Comments = @Comments,
				FollowUpRequired = @FollowUpRequired,
				ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn=GETUTCDATE()
			WHERE CallCenterHeaderID=@CallCenterHeaderID

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CrisisCall', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;


			IF(ISNULL(@ClientID,'')<>ISNULL(@OldContactID,''))
			BEGIN
			Declare @ParentHeaderID Bigint
			;WITH CTEParent
				AS
				(
					SELECT 
						CallCenterHeaderID,
						ParentCallCenterHeaderID,
						ContactID, 
						0 AS HIERARCHY
					FROM 
						CallCenter.CallCenterHeader 
					WHERE 
						CallCenterHeaderID=@CallCenterHeaderId
					UNION ALL
					SELECT 
						CH.CallCenterHeaderID,
						CH.ParentCallCenterHeaderID,
						CH.ContactID,
						CTE.HIERARCHY-1
					FROM 
						CallCenter.CallCenterHeader CH 
						INNER JOIN CTEParent CTE ON CTE.ParentCallCenterHeaderID=CH.CallCenterHeaderID 
							AND CH.CallCenterHeaderID<CTE.CallCenterHeaderID
							AND CTE.HIERARCHY!>0.
					)
					Select @ParentHeaderID= CallCenterHeaderID From CTEParent Where ParentCallCenterHeaderID is null
					--print @ParentHeaderID
		;WITH CTE
		AS
		(
			SELECT 
				CallCenterHeaderID,
				ParentCallCenterHeaderID,
				ContactID, 
				0 AS HIERARCHY
			FROM 
				CallCenter.CallCenterHeader 
			WHERE 
				CallCenterHeaderID=@ParentHeaderID
			UNION ALL
			SELECT 
				CH.CallCenterHeaderID,
				CH.ParentCallCenterHeaderID,
				CH.ContactID,
				CTE.HIERARCHY+1
			FROM 
				CallCenter.CallCenterHeader CH 
				INNER JOIN CTE ON CTE.CallCenterHeaderID=CH.parentcallCenterHeaderID 
					AND CH.CallCenterHeaderID>CTE.CallCenterHeaderID
					AND CTE.HIERARCHY!<0
		)
		UPDATE CallCenter.CallCenterHeader
		SET ContactID=@ClientID,
			ModifiedOn=@ModifiedOn,
			ModifiedBy=@ModifiedBy,
			SystemModifiedOn=GETUTCDATE()
		WHERE CallCenterHeaderID IN (Select CallCenterHeaderID from CTE)
			END
	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

