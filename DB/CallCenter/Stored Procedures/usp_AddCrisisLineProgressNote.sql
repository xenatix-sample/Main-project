-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddCrisisLineProgressNote]
-- Author:		John Crossen
-- Date:		01/27/2016
--
-- Purpose:		Insert for Crisis Line Progress Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/27/2016	John Crossen   5714	- Initial creation.
-- 02/11/2016	Gurpreet Singh	Added FollowupPlan, corrected NatureofCall in query, 
--02/15/2016	Gurpreet Singh	Added Disposition, CallStartTime
--02/16/2016	Gurpreet Singh	Made input parameters null
--02/17/2016	Kyle Campbell	Added audit proc calls
-- 02/26/2016	Gurpreet Singh	Added CallEndTime
-- 07/07/2016	Rajiv Ranjan	Increased length of comments and disposition
-- 07/07/2016	Lokesh Singhal	Added IsCallerSame & NewCallerID fields
-- 07/16/2016	Rajiv Ranjan	Increased length of NatureofCall and FollowupPlan
-- 09/20/2016	Scott Martin	Fixed an issue where ProgressNote wasn't being saved in the audit table when inserting
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_AddCrisisLineProgressNote]
	@CallCenterHeaderID BIGINT,
	@NatureofCall NVARCHAR(MAX) NULL,
	@CallTypeID SMALLINT NULL,
	@CallTypeOther NVARCHAR(100) NULL,
	@FollowupPlan  NVARCHAR(MAX) NULL,
	@Comments  NVARCHAR(MAX) NULL,
	@BehavioralCategoryID SMALLINT NULL,
	@ClientStatusID SMALLINT NULL,
	@ClientProviderID  INT NULL,
	@Disposition  NVARCHAR(MAX) NULL,
	@CallStartTime DATETIME NULL,
	@CallEndTime DATETIME NULL,
	@ModifiedOn DATETIME,
	@IsCallerSame BIT NULL,
	@NewCallerID BIGINT NULL,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID BIGINT;

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
			DECLARE @NoteHeaderID BIGINT, @ContactID BIGINT, @NoteTypeID INT;

			SELECT @ContactID = ContactID FROM CallCenter.CallCenterHeader WHERE CallCenterHeaderID = @CallCenterHeaderID;
			
			SELECT @NoteTypeID = NoteTypeID  FROM Reference.NoteType WHERE NoteType = 'Crisis Line';

			SELECT
				@NoteHeaderID = NH.NoteHeaderID
			FROM
				CallCenter.ProgressNote p
				Inner JOIN 
				Registration.NoteHeader NH on p.NoteHeaderID=NH.NoteHeaderID
				LEFT OUTER JOIN Registration.NoteHeaderVoid NHV
					ON NH.NoteHeaderID = NHV.NoteHeaderID
			WHERE
				NH.ContactID = @ContactID
				AND P.CallCenterHeaderID=@CallCenterHeaderID
				AND NH.NoteTypeID = @NoteTypeID
				AND NHV.NoteHeaderVoidID IS NULL

			IF @NoteHeaderID IS NULL
				BEGIN
				INSERT INTO Registration.NoteHeader
						( ContactID ,
						  NoteTypeID ,
						  TakenBy ,
						  TakenTime ,
						  IsActive ,
						  ModifiedBy ,
						  ModifiedOn ,
						  CreatedBy ,
						  CreatedOn
						)
				VALUES  (@ContactID,
						@NoteTypeID,
						@ModifiedBy,
						@ModifiedOn,
						1,
						@ModifiedBy,
						@ModifiedOn,
						@ModifiedBy,
						@ModifiedOn)


				SELECT @NoteHeaderID = SCOPE_IDENTITY()
			
				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeader', @NoteHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeader', @AuditDetailID, @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			

				INSERT INTO CallCenter.ProgressNote
				(
					[CallCenterHeaderID],
					[NoteHeaderID],
					[Disposition],
					[Comments],
					[CallTypeID],
					[CallTypeOther],
					[FollowupPlan],
					[NatureofCall],
					[IsCallerSame],
					[NewCallerID],
					[IsActive],
					[ModifiedBy],
					[ModifiedOn],
					[CreatedBy],
					[CreatedOn]
				)
				VALUES
				(
					@CallCenterHeaderID,
					@NoteHeaderID,
					@Disposition,
					@Comments,
					@CallTypeID,
					@CallTypeOther,
					@FollowupPlan,
					@NatureofCall,
					@IsCallerSame,
					@NewCallerID,
					1,
					@ModifiedBy,
					@ModifiedOn,
					@ModifiedBy,
					@ModifiedOn
				);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'ProgressNote', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'ProgressNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				END
			ELSE
				BEGIN
				SELECT @ID = ProgressNoteID FROM CallCenter.ProgressNote PN WHERE NoteHeaderId = @NoteHeaderID

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'ProgressNote', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				UPDATE CallCenter.ProgressNote
				SET [Disposition] = @Disposition,
					[Comments] = @Comments,
					[CallTypeID] = @CallTypeID,
					[CallTypeOther] = @CallTypeOther,
					[FollowupPlan] = @FollowupPlan,
					[NatureofCall] = @NatureofCall,
					[IsCallerSame] = @IsCallerSame,
					[NewCallerID] = @NewCallerID,
					[ModifiedBy] = @ModifiedBy,
					[ModifiedOn] = @ModifiedOn,
					[SystemModifiedOn] = GETUTCDATE()
				WHERE
					NoteHeaderID = @NoteHeaderID;

				EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'ProgressNote', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				END
	
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CrisisCall', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE CallCenter.CrisisCall 
			SET Comments = @Comments,
				ClientStatusID = @ClientStatusID,
				ClientProviderID = @ClientProviderID,
				BehavioralCategoryID = @BehavioralCategoryID,
				Disposition = @Disposition,
				ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn = GETUTCDATE()
			WHERE
				CallCenterHeaderID = @CallCenterHeaderID;

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CrisisCall', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CallCenterHeader', @CallCenterHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE CallCenter.CallCenterHeader
			SET CallStartTime = @CallStartTime,
				CallEndTime = @CallEndTime
			WHERE
				CallCenterHeaderID=@CallCenterHeaderID

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END