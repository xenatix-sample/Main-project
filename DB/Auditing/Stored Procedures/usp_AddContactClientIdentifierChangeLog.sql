-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactClientIdentifierChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Add ContactClientIdentifierChangeLog
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/18/2016	Kyle Campbell	TFS #14753	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Auditing].[usp_AddContactClientIdentifierChangeLog]
	@TransactionLogID BIGINT,
	@ContactClientIdentifierID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		IF OBJECT_ID('tempdb..#tmpContactClientIdentifier') IS NOT NULL
			DROP TABLE #tmpContactClientIdentifier
		IF OBJECT_ID('tempdb..#tmpContactClientIdentifierChangeLog') IS NOT NULL
			DROP TABLE #tmpContactClientIdentifierChangeLog
		IF @TransactionLogID > 0
			BEGIN
				SELECT 
					ContactClientIdentifierID, 
					ContactID,
					ClientIdentifierTypeID,
					AlternateID,
					ExpirationReasonID,
					EffectiveDate,
					ExpirationDate,
					IsActive,
					ModifiedBy,
					ModifiedOn
				INTO #tmpContactClientIdentifier
				FROM Registration.ContactClientIdentifier
				WHERE ContactClientIdentifierID = @ContactClientIdentifierID;

				SELECT DISTINCT
					@TransactionLogID AS [TransactionLogID],
					@ModifiedBy AS UserID,
					CCI.ModifiedOn AS [ChangedDate],
					U.FirstName AS [UserFirstName],
					U.LastName AS [UserLastName],
					CCI.ContactClientIdentifierID,
					CCI.ContactID,
					CIT.ClientIdentifierType,
					AlternateID,
					OER.ExpirationReason,
					EffectiveDate,
					ExpirationDate,
					CCI.IsActive
				INTO #tmpContactClientIdentifierChangeLog	
				FROM
					#tmpContactClientIdentifier CCI
					LEFT JOIN Core.Users U ON U.UserID = @ModifiedBy
					LEFT JOIN Reference.ClientIdentifierType CIT ON CCI.ClientIdentifierTypeID = CIT.ClientIdentifierTypeID
					LEFT JOIN Reference.OtherIDExpirationReasons OER ON CCI.ExpirationReasonID = OER.ExpirationReasonID

				MERGE INTO Auditing.ContactClientIdentifierChangeLog AS TARGET
				USING (SELECT * FROM #tmpContactClientIdentifierChangeLog) AS SOURCE
				ON (SOURCE.TransactionLogID = TARGET.TransactionLogID AND SOURCE.ContactClientIdentifierID = TARGET.ContactClientIdentifierID)
				WHEN NOT MATCHED THEN INSERT
				(
					TransactionLogID,
					UserID,
					ChangedDate,
					UserFirstName,
					UserLastName, 
					ContactClientIdentifierID,
					ContactID,
					ClientIdentifierType,
					AlternateID,
					ExpirationReason,
					EffectiveDate,
					ExpirationDate,
					IsActive
				) 
				VALUES
				(
					SOURCE.TransactionLogID,
					SOURCE.UserID,
					SOURCE.ChangedDate,
					SOURCE.UserFirstName,
					SOURCE.UserLastName, 
					SOURCE.ContactClientIdentifierID,
					SOURCE.ContactID,
					SOURCE.ClientIdentifierType,
					SOURCE.AlternateID,
					SOURCE.ExpirationReason,
					SOURCE.EffectiveDate,
					SOURCE.ExpirationDate,
					SOURCE.IsActive
				)
				WHEN MATCHED THEN UPDATE
				SET 
					TransactionLogID = SOURCE.TransactionLogID,
					UserID = SOURCE.UserID,
					ChangedDate = SOURCE.ChangedDate,
					UserFirstName = SOURCE.UserFirstName,
					UserLastName = SOURCE.UserLastName, 
					ContactClientIdentifierID = SOURCE.ContactClientIdentifierID,
					ContactID = SOURCE.ContactID,
					ClientIdentifierType = SOURCE.ClientIdentifierType,
					AlternateID = SOURCE.AlternateID,
					ExpirationReason = SOURCE.ExpirationReason,
					EffectiveDate = SOURCE.EffectiveDate,
					ExpirationDate = SOURCE.ExpirationDate,
					IsActive = SOURCE.IsActive;

				DROP TABLE #tmpContactClientIdentifier
				DROP TABLE #tmpContactClientIdentifierChangeLog
			END
	END TRY
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END