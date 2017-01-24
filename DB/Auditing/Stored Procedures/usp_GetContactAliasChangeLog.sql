-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactAliasChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Get ContactAliasChangeLog
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14753	Initial Creation
-- 22/11/2016   Gurpreet Singh	Added Order by on the basis of ContactAliasID
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Auditing].[usp_GetContactAliasChangeLog]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
		SELECT
			[TransactionLogID],
			[UserID],
			[ChangedDate],
			[UserFirstName],
			[UserLastName],
			[ContactAliasID],	
			[ContactID],
			[AliasFirstName],
			[AliasMiddle],
			[AliasLastName],
			[Suffix],
			[IsActive]
		FROM Auditing.ContactAliasChangeLog
		WHERE ContactID = @ContactID
		ORDER BY ContactAliasID,ChangedDate DESC;
	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
