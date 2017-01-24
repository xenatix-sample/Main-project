-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[fn_GetAddressMappingCount]
-- Author:		Scott Martin
-- Date:		12/21/2016
--
-- Purpose:		Gets a count of Addresses used excluding a specific mapping type
--
-- Notes:		Function is used as a way to consolidate the process of counting the number to times an Address is mapped
--				to a record (User, Contact, Organization Detail...) while excluding the counts of a specific set 
--				1 = Contact, 2 = User, 3 = Organization Detail
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/21/2016	Scott Martin	Initial Creation
----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION [Core].[fn_GetAddressMappingCount]
(
	@AddressID BIGINT = 1,
	@ExcludeAddressMappingType TINYINT
)
	RETURNS INT
AS 
BEGIN
	DECLARE @AddressCount INT

	;WITH AddressCounts (AddressCount, AddressMappingType)
	AS
	(
		SELECT
			COUNT(*) AS AddressCount,
			1 AS AddressMappingType
		FROM
			Registration.ContactAddress
		WHERE
			AddressID = @AddressID
		UNION ALL
		SELECT
			COUNT(*) AS AddressCount,
			2 AS AddressMappingType
		FROM
			Core.UserAddress
		WHERE
			AddressID = @AddressID
		UNION ALL
		SELECT
			COUNT(*) AS AddressCount,
			3 AS AddressMappingType
		FROM
			Core.OrganizationAddress
		WHERE
			AddressID = @AddressID
	)
	SELECT
		@AddressCount = SUM(AddressCount)
	FROM
		AddressCounts
	WHERE
		AddressMappingType <> @ExcludeAddressMappingType;

	RETURN @AddressCount;
END
GO