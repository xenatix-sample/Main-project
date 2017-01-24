-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_CopyContactAddress]
-- Author:		Sumana Sangapu
-- Date:		09/23/2015
--
-- Purpose:		Wrapper for Copy Contact Address functionality
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/23/2015	Sumana Sangapu	Initial Creation
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field, added CreatedBy and CreatedOn to Insert
-- 01/29/2016   Justin Spalti   Removed the ModifiedOn Parameter and added logic to get ModifiedOn from the AddressesXML parameter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_CopyContactAddress]
	@ContactID bigint,
	@AddressesXML xml,
	@Action	nvarchar(10),
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @ModifiedOn DATETIME;

	SELECT
		@ResultCode = 0,
		@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	SELECT @ModifiedOn = T.C.value('ModifiedOn[1]', 'DATETIME') FROM  @AddressesXML.nodes('RequestXMLValue/Address') AS T(C)

	IF @Action = 'Add' 
		BEGIN
		---- Set the IsPrimary for existing ContactAddress to 0 and set the new Address to IsPrimay through the AddContactAddress procedure
		--UPDATE	[Registration].[ContactAddress]
		--SET		IsPrimary = 0,
		--		ModifiedBy = @ModifiedBy,
		--		ModifiedOn = @ModifiedOn,
		--		SystemModifiedOn = GETUTCDATE()
		--WHERE	ContactID = @ContactID
		--AND		IsActive = 1 

		EXEC [Registration].[usp_AddContactAddresses] @ContactID,@AddressesXML,@ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
	END
	ELSE IF @Action = 'Update'
		BEGIN
		EXEC [Registration].[usp_UpdateContactAddresses] @AddressesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT 
		END

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

 