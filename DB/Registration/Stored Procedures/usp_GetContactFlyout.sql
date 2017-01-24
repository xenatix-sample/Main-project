----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactFlyout]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Gets primary contact photo data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/18/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactFlyout]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT TOP 1
		C.ContactID,
		C.MRN,
		C.FirstName,
		C.Middle AS MiddleName,
		C.LastName,
		C.DOB,
		G.Gender,
		A.Line1 AS Address1,
		A.Line2 AS Address2,
		A.City,
		SP.StateProvinceCode AS State,
		A.Zip,
		E.Email,
		P.Number AS PhoneNumber,
		P.Extension,
		CM.ContactMethod,
		CASE
			WHEN CAD.IsCompanyActive = 1 THEN
				CONCAT('Active in comany: ', LTRIM(STUFF((SELECT DISTINCT ', ' + OSD.Name FROM Registration.vw_ContactAdmissionDischarge vCAD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD ON vCAD.CompanyID = OSD.MappingID WHERE ContactID = C.ContactID AND vCAD.DataKey = 'Company' FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')))
			ELSE 'Not Active' END AS CompanyNames	
	FROM
		Registration.Contact C
		LEFT OUTER JOIN Reference.Gender G
			ON C.GenderID = G.GenderID
		LEFT OUTER JOIN Registration.ContactAddress CA
			ON C.ContactID = CA.ContactID
			AND CA.IsActive = 1
		LEFT OUTER JOIN Core.Addresses A
			ON CA.AddressID = A.AddressID
		LEFT OUTER JOIN Reference.StateProvince SP
			ON A.StateProvince = SP.StateProvinceID
		LEFT OUTER JOIN Registration.ContactEmail CE
			ON C.ContactID = CE.ContactID
			AND CE.IsActive = 1
		LEFT OUTER JOIN Core.Email E
			ON CE.EmailID = E.EmailID
		LEFT OUTER JOIN Registration.ContactPhone CP
			ON C.ContactID = CP.ContactID
			AND CP.IsActive = 1
		LEFT OUTER JOIN Core.Phone P
			ON CP.PhoneID = P.PhoneID
		LEFT OUTER JOIN Reference.ContactMethod CM
			ON C.PreferredContactMethodID = CM.ContactMethodID
		LEFT OUTER JOIN Registration.vw_ContactAdmissionDischarge CAD
			ON C.ContactID = CAD.ContactID
			AND CAD.DataKey = 'Company'
			AND CAD.IsDischarged = 0
	WHERE
		C.ContactID = @ContactID
	ORDER BY
		C.ContactID,
		CA.IsPrimary DESC,
		CE.IsPrimary DESC,
		CP.IsPrimary DESC,
		CAD.IsCompanyActive DESC
			
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


