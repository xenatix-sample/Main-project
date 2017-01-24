-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_SaveOrganizationAddresses]
-- Author:		Scott Martin
-- Date:		12/27/2016
--
-- Purpose:		Save Organization Address Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/27/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_SaveOrganizationAddresses
	@DetailID BIGINT,
	@AddressesXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	DECLARE @AddAddressXML XML = @AddressesXML,
			@UpdateAddressXML XML = @AddressesXML

	SET @AddAddressXML.modify('delete /RequestXMLValue/Address[OrganizationAddressID>0]');
	SET @UpdateAddressXML.modify('delete /RequestXMLValue/Address[OrganizationAddressID=0]');

	EXEC Core.usp_UpdateOrganizationAddresses @UpdateAddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	IF @ResultCode <> 0
		BEGIN
		RETURN
		END

	EXEC Core.usp_AddOrganizationAddresses @DetailID, @AddAddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
