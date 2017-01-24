-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_ValidateUnMergeRequest
-- Author:		Scott Martin
-- Date:		01/09/2017
--
-- Purpose:		Check to see if any records have been added/modified to Merged Contact, if so, inactive MergedContact
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/09/2017	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_ValidateUnMergeRequest]
(
	@MergedContactsMappingID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@AllowUnMerge BIT OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,	
			@ResultMessage = 'Data saved successfully'

	BEGIN TRY	
	DECLARE @ContactID BIGINT,
			@TransactionLogID BIGINT,
			@MergeDate DATETIME,
			@LastActionDate DATETIME;

	SELECT
		@ContactID = ContactID,
		@TransactionLogID = TransactionLogID,
		@MergeDate = MergeDate
	FROM
		Core.MergedContactsMapping
	WHERE
		ContactID = @MergedContactsMappingID;

	SELECT
		@LastActionDate = MAX(A.CreatedOn)
	FROM
		Auditing.Audits A WITH(NOLOCK)
		INNER JOIN Auditing.AuditDetail AD WITH(NOLOCK)
			ON A.AuditID = AD.AuditID
	WHERE
		AD.EntityTypeID = 2
		AND AD.EntityID = @ContactID
		AND A.TransactionLogID <> @TransactionLogID
		AND A.AuditTypeID <> 1;

	IF @LastActionDate > @MergeDate
		BEGIN
		SELECT @ResultCode = -16,
				@ResultMessage = 'The contact may not be un-merged because data has either been changed or modified.'

		SET @AllowUnMerge = 0;
		END
	ELSE
		BEGIN
		SET @AllowUnMerge = 1;
		END

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END