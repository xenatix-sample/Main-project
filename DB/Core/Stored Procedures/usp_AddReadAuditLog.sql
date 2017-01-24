----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddReadAuditLog]
-- Author:		Scott Martin
-- Date:		06/23/2016
--
-- Purpose:		Logs the values for tracking client viewed info
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/23/2016	Scott Martin		Initial creation.
-- 12/19/2016	Scott Martin		Added CreatedOn to the insert statement for the audit table
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddReadAuditLog]
	@AuditSource NVARCHAR(255),
	@ReadDetails XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	DECLARE @ID BIGINT

	INSERT INTO Auditing.Audits
	(
		AuditTypeID ,
		UserId,
		CreatedOn,
		AuditTimeStamp ,
		IsArchivable
	)
	VALUES
	(
		[Core].[fn_GetAuditType]('Select'),
		@ModifiedBy ,
		GETDATE(),
		GETUTCDATE() , 
		0  
	);

	SELECT @ID = SCOPE_IDENTITY();

	INSERT INTO Auditing.AuditDetail
	(
		AuditID,
		AuditSource,
		AuditPost
	)
	VALUES
	(
		@ID,
		@AuditSource,
		@ReadDetails
	)

	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
				   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END