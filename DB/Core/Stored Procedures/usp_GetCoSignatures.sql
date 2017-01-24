
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetCoSignatures]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Gets User CoSignatures
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016 - Sumana Sangapu Initial Creation 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetCoSignatures]
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully';

	BEGIN TRY

			SELECT  CoSignatureID,CoSigneeID, DocumentTypeGroupID, EffectiveDate, ExpirationDate 
			FROM	[Core].[CoSignatures]  c
			WHERE	UserID = @UserID
			AND		IsActive = 1

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END