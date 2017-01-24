----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetMergedContacts]
-- Author:		Scott Martin
-- Date:		12/05/2016
--
-- Purpose:		Gets the summary of contacts that have been merged together
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/05/2016	Scott Martin		Initial creation.
-- 01/06/2017	Scott Martin		Only pulling in Primary phone number and returning a list of emails to fix an issue with duplicates
-- 01/09/2017	Scott Martin		Changed proc to return active records
-- 01/11/2017	Scott Martin		Changed proc to return only records that are allowed to be un-merged
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetMergedContacts]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT
		MCM.MergedContactsMappingID,
		MC.ContactID,
		MC.MRN,
		MC.FirstName,
		MC.LastName,
		MC.DOB,
		MC.SSN,
		MC.DriverLicense,
		SP.StateProvinceCode,
		LTRIM(STUFF((SELECT ',' + E.Email FROM Registration.ContactEmail CE INNER JOIN Core.Email E ON CE.EmailID = E.EmailID WHERE CE.IsActive = 1 AND CE.ContactID = MC.ContactID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS Email,
		P.Number AS PhoneNumber,
		MCM.CreatedOn AS MergedDate,
		CAST(1 AS BIT) AS IsMaster
	FROM
		Core.MergedContactsMapping MCM
		INNER JOIN Registration.Contact MC
			ON MCM.ParentID = MC.ContactID
		LEFT OUTER JOIN Reference.StateProvince SP
			ON MC.DriverLicenseStateID = SP.StateProvinceID
		LEFT OUTER JOIN Registration.ContactPhone CP
			ON MC.ContactID = CP.ContactID
			AND CP.IsPrimary = 1
			AND CP.IsActive = 1
		LEFT OUTER JOIN Core.Phone P
			ON CP.PhoneID = P.PhoneID
	WHERE
		MCM.IsActive = 1
		AND MCM.IsUnMergeAllowed = 1
	UNION
	SELECT
		MCM.MergedContactsMappingID,
		CC.ContactID,
		CC.MRN,
		CC.FirstName,
		CC.LastName,
		CC.DOB,
		CC.SSN,
		CC.DriverLicense,
		SP.StateProvinceCode,
		LTRIM(STUFF((SELECT ',' + E.Email FROM Registration.ContactEmail CE INNER JOIN Core.Email E ON CE.EmailID = E.EmailID WHERE CE.IsActive = 1 AND CE.ContactID = CC.ContactID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS Email,
		P.Number AS PhoneNumber,
		MCM.CreatedOn AS MergedDate,
		CAST(0 AS BIT) AS IsMaster
	FROM
		Core.MergedContactsMapping MCM
		INNER JOIN Registration.Contact CC
			ON MCM.ChildID = CC.ContactID
		LEFT OUTER JOIN Reference.StateProvince SP
			ON CC.DriverLicenseStateID = SP.StateProvinceID
		LEFT OUTER JOIN Registration.ContactPhone CP
			ON CC.ContactID = CP.ContactID
			AND CP.IsPrimary = 1
			AND CP.IsActive = 1
		LEFT OUTER JOIN Core.Phone P
			ON CP.PhoneID = P.PhoneID
	WHERE
		MCM.IsActive = 1
		AND MCM.IsUnMergeAllowed = 1
	ORDER BY
		MergedDate DESC,
		MergedContactsMappingID,
		IsMaster DESC;
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO