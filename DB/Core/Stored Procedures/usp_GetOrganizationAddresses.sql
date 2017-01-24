-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_GetOrganizationAddresses]
-- Author:		Kyle Campbell
-- Date:		12/15/2016
--
-- Purpose:		Get Organization Addresses
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2016	Kyle Campbell	TFS #17999	Initial Creation
-- 12/30/2016	Scott Martin	Added sorting
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetOrganizationAddresses]
	@DetailID bigint,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'
	BEGIN TRY
		SELECT
			OD.DetailID,
			OA.[OrganizationAddressID],
			A.[AddressID],
			[AT].[AddressTypeID],
			A.[Line1],
			A.[Line2], 
			A.[City],
			A.[StateProvince],
			A.[County],
			A.[Zip],
			OA.[IsPrimary],
			OA.[EffectiveDate],
			OA.[ExpirationDate],			
			OA.[ModifiedOn]
		FROM 
			Core.OrganizationDetails OD
			INNER JOIN Core.OrganizationAddress OA
				ON OD.DetailID = OA.DetailID
				AND OA.[IsActive]=1
			LEFT JOIN [Core].[Addresses] A
				ON A.AddressID = OA.AddressID
			LEFT JOIN [Reference].[AddressType] [AT]
				ON A.AddressTypeID = [AT].AddressTypeID
		WHERE
			OD.DetailID = @DetailID	
		ORDER BY
			OA.ExpirationDate,
			OA.EffectiveDate DESC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END