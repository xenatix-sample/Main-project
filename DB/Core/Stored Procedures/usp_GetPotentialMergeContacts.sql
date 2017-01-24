----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPotentialMergeContacts]
-- Author:		Scott Martin
-- Date:		12/05/2016
--
-- Purpose:		Gets the list of contacts that have the potential to be merged together
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/05/2016	Scott Martin		Initial creation.
-- 12/21/2016	Scott Martin		Adding sorting to result set
-- 01/06/2017	Scott Martin		Only pulling in Primary phone number and returning a list of emails to fix an issue with duplicates
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetPotentialMergeContacts]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_OpenEncryptionkeys @ResultCode OUTPUT, @ResultMessage OUTPUT;

	SELECT
		C.ContactID,
		C.MRN,
		C.FirstName,
		C.LastName,
		C.DOB,
		C.SSN,
		C.DriverLicense,
		SP.StateProvinceCode,
		LTRIM(STUFF((SELECT ',' + E.Email FROM Registration.ContactEmail CE INNER JOIN Core.Email E ON CE.EmailID = E.EmailID WHERE CE.IsActive = 1 AND CE.ContactID = C.ContactID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS Email,
		P.Number AS PhoneNumber
	FROM
		Registration.Contact C
		LEFT OUTER JOIN Reference.StateProvince SP
			ON C.DriverLicenseStateID = SP.StateProvinceID
		LEFT OUTER JOIN Registration.ContactPhone CP
			ON C.ContactID = CP.ContactID
			AND CP.IsActive = 1
			AND CP.IsPrimary = 1
		LEFT OUTER JOIN Core.Phone P
			ON CP.PhoneID = P.PhoneID
		INNER JOIN Core.PotentialContactMatches PCM
			ON C.ContactID = PCM.ContactID
			AND PCM.IsActive = 1
	ORDER BY
		PCM.SSNMatch DESC,
		Core.fn_Decrypt(C.SSNEncrypted),
		C.FirstName,
		C.LastName
		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO