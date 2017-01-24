-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactAliases]
-- Author:		Kyle Campbell
-- Date:		03/11/2016
--
-- Purpose:		Gets a list of Contact Aliases
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	Kyle Campbell	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetContactAliases]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
		SELECT	[ContactAliasID],
				[ContactID],
				[AliasFirstName],
				[AliasMiddle],
				[AliasLastName],
				[SuffixID],
				[IsActive],
				[ModifiedBy],
				[ModifiedOn]
		FROM Registration.ContactAlias
		WHERE	ContactID = @ContactID 
				AND IsActive = 1;
	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END