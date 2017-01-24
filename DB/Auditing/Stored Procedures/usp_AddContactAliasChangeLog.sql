-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactAliasChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Add ContactAliasChangeLog
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14753	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Auditing].[usp_AddContactAliasChangeLog]
	@TransactionLogID BIGINT,
	@ContactAliasID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		IF OBJECT_ID('tempdb..#tmpContactAlias') IS NOT NULL
			DROP TABLE #tmpContactAlias
		IF OBJECT_ID('tempdb..#tmpContactAliasChangeLog') IS NOT NULL
			DROP TABLE #tmpContactAliasChangeLog
		IF @TransactionLogID > 0
			BEGIN
				SELECT 
					ContactAliasID, 
					ContactID,
					AliasFirstName,
					AliasMiddle,
					AliasLastName,
					SuffixID,
					IsActive,
					ModifiedBy,
					ModifiedOn
				INTO #tmpContactAlias
				FROM Registration.ContactAlias
				WHERE ContactAliasID = @ContactAliasID;

				SELECT DISTINCT
					@TransactionLogID AS [TransactionLogID],
					@ModifiedBy AS UserID,
					CA.ModifiedOn AS [ChangedDate],
					U.FirstName AS [UserFirstName],
					U.LastName AS [UserLastName],
					CA.ContactAliasID,
					CA.ContactID,
					CA.AliasFirstName,
					CA.AliasMiddle,
					CA.AliasLastName,
					S.Suffix,
					CA.IsActive
				INTO #tmpContactAliasChangeLog	
				FROM
					#tmpContactAlias CA
					LEFT JOIN Core.Users U ON U.UserID = @ModifiedBy
					LEFT JOIN Reference.Suffix S ON CA.SuffixID = S.SuffixID

				MERGE INTO Auditing.ContactAliasChangeLog AS TARGET
				USING (SELECT * FROM #tmpContactAliasChangeLog) AS SOURCE
				ON (SOURCE.TransactionLogID = TARGET.TransactionLogID AND SOURCE.ContactAliasID = TARGET.ContactAliasID)
				WHEN NOT MATCHED THEN INSERT
				(
					TransactionLogID,
					UserID,
					ChangedDate,
					UserFirstName,
					UserLastName, 
					ContactAliasID,
					ContactID,			
					AliasFirstName,
					AliasMiddle,
					AliasLastName,
					Suffix,
					IsActive
				) 
				VALUES
				(
					SOURCE.TransactionLogID,
					SOURCE.UserID,
					SOURCE.ChangedDate,
					SOURCE.UserFirstName,
					SOURCE.UserLastName, 
					SOURCE.ContactAliasID,
					SOURCE.ContactID,			
					SOURCE.AliasFirstName,
					SOURCE.AliasMiddle,
					SOURCE.AliasLastName,
					SOURCE.Suffix,
					SOURCE.IsActive
				)
				WHEN MATCHED THEN UPDATE
				SET 
					TransactionLogID = SOURCE.TransactionLogID,
					UserID = SOURCE.UserID,
					ChangedDate = SOURCE.ChangedDate,
					UserFirstName = SOURCE.UserFirstName,
					UserLastName = SOURCE.UserLastName, 
					ContactAliasID = SOURCE.ContactAliasID,
					ContactID = SOURCE.ContactID,			
					AliasFirstname = SOURCE.AliasFirstName,
					AliasMiddle = SOURCE.AliasMiddle,
					AliasLastName = SOURCE.AliasLastName,
					Suffix = SOURCE.Suffix,
					IsActive = SOURCE.IsActive;

				DROP TABLE #tmpContactAlias
				DROP TABLE #tmpContactAliasChangeLog
			END
	END TRY
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END