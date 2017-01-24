-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactClientIdentifiers]
-- Author:		Rajiv Ranjan
-- Date:		12/23/2015
--
-- Purpose:		Gets a list of alternate ID's
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/23/2015	Scott Martin		Initial creation.
-- 03/09/2016	Kyle Campbell	TFS #6339 Added EffectiveDate and ExpirationDate fields
-- 03/16/2016	Rajiv Ranjan	Added [EffectiveDate] & [ExpirationDate]
-- 07/27/2016	Deepak Kumar	Added ExpirationReasonID wrt [Reference].[OtherIDExpirationReasons]
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactClientIdentifiers]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT 
			[ContactClientIdentifierID],
			[ContactID],
			[ClientIdentifierTypeID],
			[AlternateID],
			[ExpirationReasonID],
			[EffectiveDate],
			[ExpirationDate],
			[IsActive],
			[ModifiedBy],
			[ModifiedOn]
		FROM 
			Registration.ContactClientIdentifier CCI
		WHERE 
			CCI.ContactID = @ContactID	
			AND CCI.IsActive = 1 		
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END