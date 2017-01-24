----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetECIReferrals]
-- Author:		Scott Martin
-- Date:		01/04/2017
--
-- Purpose:		Get the ECI referrals
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2016	Scott Martin		Initial creation.
-- 01/24/2017	Scott Martin		Added AdditionalConcerns column
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetECIReferrals]
	@StartDate DATETIME,
	@EndDate DATETIME,
	@OrganizationID NVARCHAR(MAX),
	@IsCompanyActive NVARCHAR(MAX),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

	;WITH AddrCTE (ContactID, Line1, Line2, City, StateProvince,StateProvinceName,CountyName, CountryName, Zip, AddrRank)
	AS
	(
		SELECT
			CA.ContactID,
			A.Line1,
			A.Line2,
			A.City,
			A.StateProvince,
			CSP.StateProvinceName,
			CSP.CountyName,
			CSP.CountryName,
			A.Zip,
			RANK() OVER(PARTITION BY CA.ContactID ORDER BY CA.IsPrimary DESC, CA.EffectiveDate DESC, CA.ModifiedOn DESC)
		FROM
			Registration.ContactAddress CA WITH(NOLOCK)
			INNER JOIN Core.Addresses A WITH(NOLOCK)
				ON CA.AddressID = A.AddressID
			INNER JOIN [Reference].[vw_CountryStateProvinceCounty] CSP
				ON A.County = CSP.CountyID
			LEFT JOIN [Registration].[ReferralAdditionalDetails] RD WITH(NOLOCK)
				ON RD.ContactID = CA.ContactID
			INNER JOIN [Registration].[ReferralHeader] RH WITH(NOLOCK)
				ON RD.ReferralHeaderID = Rh.ReferralHeaderID
			INNER JOIN Registration.Contact C WITH(NOLOCK)
				ON CA.ContactID = C.ContactID
				AND C.ClientTypeID = 1
		WHERE
			CA.IsActive = 1
			AND (RH.ReferralDate >= @StartDate AND RH.ReferralDate < DATEADD(DAY, 1, @EndDate))
	),
	PhoneCTE (ContactID, Number,PhonePermission, PhoneRank)
	AS
	(
		SELECT
			CP.ContactID,
			P.Number,
			PP.PhonePermission,
			RANK() OVER(PARTITION BY CP.ContactID ORDER BY CP.IsPrimary DESC, CP.EffectiveDate DESC, CP.ModifiedOn DESC)
		FROM
			Registration.ContactPhone CP WITH(NOLOCK)
			INNER JOIN Core.Phone P WITH(NOLOCK)
				ON CP.PhoneID = P.PhoneID
			INNER JOIN Reference.PhonePermission PP 
				ON PP.PhonePermissionID = CP.PhonePermissionID
				AND PP.IsActive = 1
			LEFT JOIN [Registration].[ReferralAdditionalDetails] RD WITH(NOLOCK)
				ON RD.ContactID = CP.ContactID
			INNER JOIN [Registration].[ReferralHeader] RH WITH(NOLOCK)
				ON RD.ReferralHeaderID = Rh.ReferralHeaderID
			INNER JOIN Registration.Contact C WITH(NOLOCK)
				ON CP.ContactID = C.ContactID
				AND C.ClientTypeID = 1
		WHERE
			CP.IsActive = 1
			AND (RH.ReferralDate >= @StartDate AND RH.ReferralDate < DATEADD(DAY, 1, @EndDate))
	),
	EmailCTE (ContactID, Email,EmailPermission, EmailRank)
	AS
	(
		SELECT
			CE.ContactID,
			E.Email,
			EP.EmailPermission,
			RANK() OVER(PARTITION BY CE.ContactID ORDER BY CE.IsPrimary DESC, CE.EffectiveDate DESC, CE.ModifiedOn DESC)
		FROM
			Registration.ContactEmail CE WITH(NOLOCK)
			INNER JOIN Core.Email E WITH(NOLOCK)
				ON CE.EmailID = E.EmailID
			INNER JOIN Reference.EmailPermission EP 
				ON EP.EmailPermissionID = CE.EmailPermissionID
				AND EP.IsActive = 1
			LEFT JOIN [Registration].[ReferralAdditionalDetails] RD WITH(NOLOCK)
				ON RD.ContactID = CE.ContactID
			INNER JOIN [Registration].[ReferralHeader] RH WITH(NOLOCK)
				ON RD.ReferralHeaderID = Rh.ReferralHeaderID
			INNER JOIN Registration.Contact C
				ON CE.ContactID = C.ContactID
				AND C.ClientTypeID = 1
		WHERE
			EP.IsActive = 1
			AND (RH.ReferralDate >= @StartDate AND RH.ReferralDate < DATEADD(DAY, 1, @EndDate))
	),
	ActiveStatus (ContactID, IsCompanyActive)
	AS
	(
		SELECT
			CAD.ContactID,
			MAX(CAST(CAD.IsCompanyActive AS INT))
		FROM
			Registration.vw_ContactAdmissionDischarge CAD
			LEFT JOIN [Registration].[ReferralAdditionalDetails] RD WITH(NOLOCK)
				ON RD.ContactID = CAD.ContactID
			INNER JOIN [Registration].[ReferralHeader] RH WITH(NOLOCK)
				ON RD.ReferralHeaderID = Rh.ReferralHeaderID
			INNER JOIN Registration.Contact C WITH(NOLOCK)
				ON CAD.ContactID = C.ContactID
				AND C.ClientTypeID = 1
		WHERE
			(RH.ReferralDate >= @StartDate AND RH.ReferralDate < DATEADD(DAY, 1, @EndDate))
		GROUP BY
			CAD.ContactID
	)
	SELECT DISTINCT
		C.ContactID
		,RH.ReferralDate
		,DATEADD(DAY, 45, RH.ReferralDate) AS 'A45Days'
		,(select STUFF((SELECT ', ' + CAST(OSD.Name AS NVARCHAR(MAX))
				FROM
					Registration.vw_ContactAdmissionDischarge CAD WITH(NOLOCK)
					LEFT JOIN Core.vw_GetOrganizationStructureDetails OSD WITH(NOLOCK)
						ON OSD.MappingID = CAD.ProgramUnitID
				WHERE
					CAD.ContactID = c.ContactID
					AND CAD.DataKey = 'ProgramUnit'
					AND CAD.IsDischarged = 0
					--AND ISNULL(CAD.ProgramUnitID, 0) IN (@OrganizationID)	
				FOR XML PATH('')),1,1,'') ) AS ProgramUnitName
		,c.MRN
		,C.LastName
		,C.FirstName
		,C.Middle
		,S.Suffix
		,C.DOB
		,C.SSN
		,A.Line1
		,A.Line2
		,A.City
		,A.StateProvinceName 'State'
		,A.CountyName
		,A.CountryName
		,A.Zip
		,LEFT(P.Number,3)+'-'+ SUBSTRING(P.Number,4,3) +'-'+ SUBSTRING(P.Number,7,LEN(P.NUMBER)) AS 'PhoneNumber'
		,CCI1.Alternateid AS 'TKIDChildID'
		,CCI1.EffectiveDate AS 'TKIDChildEffectiveDate'
		,CCI.Alternateid AS 'TKIDCaseID'
		,CCI.EffectiveDate AS 'TKIDCaseIDEffectiveDate'
		,G.Gender
		,SD.SchoolDistrictName
		,(select STUFF((SELECT ', ' + CAST(r.race AS NVARCHAR(MAX))
				FROM [Reference].[Race] R 
			LEFT JOIN [Registration].[ContactRace] CR ON CR.RaceID=R.RaceID
			WHERE CR.CONTACTID=C.ContactID
					FOR XML PATH('')),1,1,'')) Race
		,ECI.Ethnicity
		,L.LanguageName AS 'Language'
		,RAD.OtherPreferredLanguage
		,EAD.IsCPSInvolved
		,EAD.IsChildHospitalized
		,EAD.IsTransfer
		,ROZ.ReferralSource AS 'ContactOrganization'
		,(select STUFF((SELECT ', ' + CAST(RCN.ReferralConcern AS NVARCHAR(MAX))
				FROM [Reference].[ReferralConcern] RCN
				LEFT JOIN [Registration].[ReferralConcernDetails] RCD ON RCN.ReferralConcernID = RCD.ReferralConcernID 
				where  RCD.ReferralAdditionalDetailID = RD.ReferralAdditionalDetailID
						FOR XML PATH('')),1,1,'') )ReferralConcern
		,RD.AdditionalConcerns
		,RC.ReferralCategory
		,RCS.ReferralSource AS 'ReferralCategorySource'
		,RO.ReferralOrigin AS 'HowDidReferrerHearAboutCenter'
		,C1.FirstName AS 'ReferrerFirstName'
		,C1.LastName AS 'ReferrerLastName'
		,RH.OtherOrganization AS 'ReferrerNameOfOrg'
		,LEFT(P1.Number,3)+'-'+ SUBSTRING(P1.Number,4,3) +'-'+ SUBSTRING(P1.Number,7,LEN(P1.NUMBER)) AS 'ReferrerPhoneNumber'
		,P1.PhonePermission AS 'ReferrerPhonePermission'
		,E1.Email AS 'ReferrerEmail'
		,E1.EmailPermission AS 'ReferrerEmailPermission'
		,A1.Line1 AS 'ReferrerLine1'
		,A1.Line2 AS 'ReferrerLine2'
		,A1.City AS 'ReferrerCity'
		,A1.StateProvinceName 'ReferrerState'
		,A1.CountyName 'ReferrerCounty'
		,A1.CountryName AS 'ReferrerCountryName'
		,A1.Zip AS 'ReferrerZip'
	FROM Registration.Contact c WITH(NOLOCK)
		LEFT OUTER JOIN Registration.AdditionalDemographics RAD WITH(NOLOCK)
			ON RAD.ContactID = c.ContactID
		LEFT OUTER JOIN ECI.AdditionalDemographics EAD WITH(NOLOCK)
			ON RAD.AdditionalDemographicID = EAD.RegistrationAdditionalDemographicID
			AND EAD.IsActive = 1
		LEFT JOIN Registration.vw_ContactAdmissionDischarge CAD1 WITH(NOLOCK)
			ON CAD1.ContactID = RAD.ContactID
			AND CAD1.DataKey = 'ProgramUnit'
			AND CAD1.IsDischarged = 0
		LEFT JOIN Reference.Gender G WITH(NOLOCK)
			ON G.GenderID=C.GenderID
		LEFT JOIN REFERENCE.SUFFIX S WITH(NOLOCK)
			ON S.SuffixID=C.SuffixID
		LEFT JOIN [Registration].[ContactClientIdentifier] CCI WITH(NOLOCK)
			ON CCI.ContactID = C.ContactID
			AND CCI.ClientIdentifierTypeID = 11 --Tkid case id
		LEFT JOIN [Registration].[ContactClientIdentifier] CCI1 WITH(NOLOCK)
			ON CCI1.ContactID = C.ContactID
			AND CCI1.ClientIdentifierTypeID = 12 -- tkid child id
		LEFT JOIN [Reference].[SchoolDistrict] SD WITH(NOLOCK)
			ON SD.SchoolDistrictID=RAD.SchoolDistrictID
		LEFT JOIN [Reference].[Ethnicity] ECI WITH(NOLOCK)
			ON ECI.EthnicityID=RAD.EthnicityID
		LEFT JOIN Reference.Languages L WITH(NOLOCK)
			ON L.LanguageID=RAD.PrimaryLanguageID
		LEFT JOIN [Registration].[ReferralAdditionalDetails] RD WITH(NOLOCK)
			ON RD.ContactID = C.ContactID
		INNER JOIN [Registration].[ReferralHeader] RH WITH(NOLOCK)
			ON RD.ReferralHeaderID = Rh.ReferralHeaderID
		LEFT JOIN Reference.ReferralSource ROZ WITH(NOLOCK)
			ON ROZ.ReferralSourceID = RH.ReferralSourceID
		LEFT JOIN Reference.ReferralCategorySource RCS WITH(NOLOCK)
			ON RCS.ReferralCategorySourceID = RH.ReferralCategorySourceID
		LEFT JOIN Reference.ReferralCategory RC WITH(NOLOCK)
			ON RC.ReferralCategoryID = RCS.ReferralCategoryID
		LEFT JOIN Reference.ReferralOrigin RO WITH(NOLOCK)
			ON RO.ReferralOriginID = RH.ReferralOriginID
		LEFT JOIN AddrCTE A WITH(NOLOCK)
			ON A.ContactID=C.ContactID
			AND A.AddrRank = 1
		LEFT JOIN PhoneCTE P WITH(NOLOCK)
			ON P.ContactID=C.ContactID
			AND P.PhoneRank = 1
		LEFT JOIN Registration.Contact c1 WITH(NOLOCK)
			ON C1.ContactID = RH.ContactID
		LEFT JOIN AddrCTE A1 WITH(NOLOCK)
			ON A1.ContactID=C1.ContactID
			AND A1.AddrRank = 1
		LEFT JOIN PhoneCTE P1 WITH(NOLOCK)
			ON P1.ContactID=C1.ContactID
			AND P1.PhoneRank = 1
		LEFT JOIN EmailCTE E1 WITH(NOLOCK)
			ON E1.ContactID=C1.ContactID
			AND E1.EmailRank = 1
		LEFT JOIN ActiveStatus WITH(NOLOCK)
			ON ActiveStatus.ContactID = C.ContactID
	WHERE
		C.ClientTypeID = 1
		AND ISNULL(ActiveStatus.IsCompanyActive, 0) IN (SELECT CAST(Items AS BIGINT) FROM Core.fn_Split(REPLACE(@IsCompanyActive, '"', ''), ','))
		AND ISNULL(CAD1.ProgramUnitID, 0) IN (SELECT CAST(Items AS BIGINT) FROM Core.fn_Split(REPLACE(@OrganizationID, '"', ''), ','))
		AND (RH.ReferralDate >= @StartDate AND RH.ReferralDate < DATEADD(DAY, 1, @EndDate))

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
