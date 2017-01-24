-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactClientIdentifierChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Get ContactClientIdentifierChangeLog
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14753	Initial Creation
-- 22/11/2016   Gurpreet Singh	Added Order by on the basis of ClientIdentifierType
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Auditing].[usp_GetContactClientIdentifierChangeLog]
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
		FROM Auditing.ContactClientIdentifierChangeLog
		WHERE ContactID = @ContactID
		ORDER BY ContactClientIdentifierID,ChangedDate DESC;
	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END