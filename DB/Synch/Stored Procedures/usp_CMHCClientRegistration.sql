-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_CMHCClientRegistration]
-- Author:		Sumana Sangapu
-- Date:		07/09/2016
--
-- Purpose:		Generate Client Registration data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/09/2016	Sumana Sangapu		Initial creation
-- 07/21/2016	Sumana Sangapu		Added PresentingPRoblemID
-- 08/08/2016	Sumana Sangapu		Filter only for contacttypeid=1 and MRN is not NULL
-- 09/06/2016	Sumana Sangapu		Removed DISTINCT and timezone conversion
-- 10/14/2016	Sumana Sangapu		Altered procedure to accomodate for multiple referrals and yet be qualified for the interface
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_CMHCClientRegistration]
@BatchID BIGINT,
@LastRunDate DATETIME
AS
BEGIN

			CREATE TABLE #CMHCClientRegistration(
				[ClientRegistrationID] [int] IDENTITY(1,1) NOT NULL,
				[ContactID] [bigint] NULL,
				[MRN] [nvarchar](20) NULL,
				[FirstName] [nvarchar](200) NULL,
				[LastName] [nvarchar](200) NULL,
				[Middle] [nvarchar](200) NULL,
				[Suffix] [nvarchar](10) NULL,
				[EffectiveDate] [nvarchar](10) NULL,
				[EffectiveDateTime] [nvarchar](10) NULL,
				[Gender] [nvarchar](10) NULL,
				[DateofBirth] [nvarchar](10) NULL,
				[SSN] [nvarchar](25) NULL,
				[RaceCode] [nvarchar](10) NULL,
				[EthnicityCode] [nvarchar](10) NULL,
				[LegalStatusCode] [nvarchar](10) NULL,
				[IsActive] [nvarchar](10) NULL,
				[TKIDSID] [nvarchar](50) NULL,
				[TKIDSCASEID] [nvarchar](50) NULL,
				[LCPrimaryLanguageID] [nvarchar](10) NULL,
				[LCSchoolDistrictID] [nvarchar](10) NULL,
				[IsCPSInvolved] [nvarchar](5) NULL,
				[ReferralDate] [nvarchar](10) NULL,
				[CareID] [nvarchar](50) NULL,
				[CMBHSID] [nvarchar](50) NULL,
				[BatchID] [bigint] NOT NULL,
				[PresentingProblemID] [nvarchar](10) NULL,
				[ErrorMessage] [varchar](max) NULL)

				TRUNCATE TABLE Synch.CMHCClientRegistration

				EXEC [Core].[usp_OpenEncryptionKeys] '',''

				-- In the below query, there can be multiples references per contact and any new/modified values to [Registration].[ReferralAdditionalDetails] and Registration.ReferralHeader should trigger a client to qualify for the interface.
				-- This scenario prompts to pull all the records per contact per referral. The requirement is to update only the Max(ReferralDate) for that contact. Hence DISTINCT is being used in the final insert inorder for the contact to be 
				-- qualified for interface and avoid duplicate referrals.

				INSERT INTO #CMHCClientRegistration
				([ContactID],
				[MRN] ,
				[FirstName] ,
				[LastName] ,
				[Middle] ,
				Suffix ,
				EffectiveDate,
				EffectiveDateTime ,
				Gender ,
				DateofBirth ,
				SSN,
				RaceCode ,
				EthnicityCode,
				LegalStatusCode,
				IsActive ,
				TKIDSID ,
				TKIDSCASEID ,
				LCPrimaryLanguageID ,
				LCSchoolDistrictID ,
				IsCPSInvolved ,
				ReferralDate ,
				CareID,
				CMBHSID,
				BatchID,
				PresentingProblemID
				)
				SELECT    
				C.ContactID,
				STUFF(MRN, 1, 0, REPLICATE('0', 9 - LEN(MRN))) AS MRN,-- Padded to 9
				UPPER(FirstName) AS FirstName,
				UPPER(LastName) AS LastName,
				UPPER(Middle) AS MiddleName,
				UPPER(REPLACE(s.LegacyCode,'.','')) AS Suffix,
				CONVERT(varchar, CA.EffectiveDate,101) AS EffectieDate, 
				CONVERT(VARCHAR(5),CA.EffectiveDate, 108) AS EffectiveDateTime, 
				CASE G.Gender WHEN 'Male' THEN 'M' WHEN 'Female' THEN 'F' ELSE NULL END AS Gender, 
				CONVERT(VARCHAR,C.DOB,101) AS DateofBirth ,
				CONVERT(NVARCHAR(9), Core.fn_Decrypt(c.SSNEncrypted)) AS SSN,
				CASE  WHEN RaceCount.RaceCount = 1 THEN Race.LegacyCode  
					  WHEN RaceCount.RaceCount > 1  THEN '9' ELSE NULL END AS Race,
				E.LegacyCode AS Ethnicity,
				L.LegacyCode as LegalStatus,
				CASE C.IsActive WHEN 1 THEN 'TRUE' WHEN 0 THEN 'FALSE' END AS IsActive,
				NULL AS tkidsID,
				NULL AS tKidsCaseID,
				LG.LegacyCode as LCPrimaryLanguageID,
				SD.LegacyCode as LCSchoolDistrictID,
				CASE ECIAD.IsCPSInvolved WHEN 1 THEN '03' ELSE NULL END AS IsCPSInvolved,
				NULL as ReferralDate,
				NULL as CareID,
				NULL as CMBHSID, 
				@BatchID as BatchID,
				UPPER(p.LegacyCode ) as PresentingProblemID
				FROM Registration.Contact C 
				INNER JOIN Registration.AdditionalDemographics AD ON AD.ContactID = C.ContactID AND C.IsActive = 1 AND  C.ContactTypeID = 1  AND C.MRN IS NOT NULL  AND AD.IsActive=1 
				LEFT OUTER JOIN ECI.AdditionalDemographics ECIAD ON ECIAD.RegistrationAdditionalDemographicID = AD.AdditionalDemographicID AND ECIAD.IsCPSInvolved = 1 
				LEFT OUTER JOIN Reference.Suffix S ON C.SuffixID=S.SuffixID
				LEFT OUTER JOIN Reference.Gender G ON C.GenderID=G.GenderID
				LEFT OUTER JOIN (SELECT ca1.*,SystemCreatedOn, SystemModifiedOn from 
										(	SELECT MIN(EffectiveDate) as EffectiveDate, ContactID FROM Registration.ContactAdmission   
											WHERE Organizationid = 1 
											GROUP BY ContactID ) ca1
								 INNER JOIN Registration.ContactAdmission rca 
								 ON		rca.contactID = ca1.Contactid 
								 AND	rca.EffectiveDate = ca1.EffectiveDate ) CA
				ON C.ContactID = CA.ContactID 
				LEFT OUTER JOIN (SELECT R.Race, CR.ContactID, R.LegacyCode, CR.SystemCreatedOn, CR.SystemModifiedOn FROM  Registration.ContactRace CR JOIN Reference.Race R ON R.RaceID = CR.RaceID AND CR.IsActive =1 ) Race ON Race.ContactID = C.ContactID
				LEFT OUTER JOIN (SELECT CASE WHEN COUNT(*) > 1 THEN '9' ELSE COUNT(*) END AS RaceCount, ContactID FROM Registration.ContactRace CR WHERE CR.IsActive =1  GROUP BY CR.ContactID) RaceCount ON RaceCount.ContactID = C.ContactID
				LEFT OUTER JOIN [Registration].[ReferralAdditionalDetails] RD ON  C.ContactID = rd.ContactID 
				LEFT OUTER JOIN Registration.ReferralHeader rh ON RH.ReferralHeaderID = RD.ReferralHeaderID 
				LEFT OUTER JOIN Registration.ContactPresentingProblem cpp on c.ContactID = cpp.ContactID
				LEFT OUTER JOIN Reference.Ethnicity E ON E.EthnicityID = AD.EthnicityID
				LEFT OUTER JOIN Reference.LegalStatus L ON L.LegalStatusID = AD.LegalStatusID
				LEFT OUTER JOIN Reference.Languages LG ON LG.LanguageID = PrimaryLanguageID 
				LEFT OUTER JOIN Reference.SchoolDistrict SD ON SD.SchoolDistrictID = AD.SchoolDistrictID
				LEFT OUTER JOIN reference.presentingproblemtype  p ON cpp.PresentingProblemTypeID = p.PresentingProblemTypeID
				WHERE   (C.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR C.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR 
						(AD.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR AD.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR 
						(CA.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR CA.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR 
						(ECIAD.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR ECIAD.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR
						(Race.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR Race.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR
						(RH.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR RH.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR
						(RD.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR RD.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR
						(CPP.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR CPP.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) 
						


				-- TKIDSID
				UPDATE cd
				SET tkidsID = AlternateID 
				FROM #CMHCClientRegistration cd
				INNER JOIN (select  rn = ROW_NUMBER() OVER (PARTITION BY contactid ORDER BY EffectiveDAte desc ), AlternateID, ContactID  from Registration.ContactClientIdentifier WHERE ClientIdentifierTypeID = 12 ) a
				ON cd.ContactID = a.ContactID
				WHERE rn = 1 
				
				-- TKids Case ID
				UPDATE cd
				SET tKidsCaseID = AlternateID 
				FROM #CMHCClientRegistration cd
				INNER JOIN (select  rn = ROW_NUMBER() OVER (PARTITION BY contactid ORDER BY EffectiveDAte desc ), AlternateID, ContactID  from Registration.ContactClientIdentifier WHERE ClientIdentifierTypeID = 11 ) a
				ON cd.ContactID = a.ContactID
				WHERE rn = 1 

				-- CAREID
				UPDATE cd
				SET CareID = AlternateID 
				FROM #CMHCClientRegistration cd
				INNER JOIN (select  rn = ROW_NUMBER() OVER (PARTITION BY contactid ORDER BY EffectiveDAte desc ), AlternateID, ContactID  from Registration.ContactClientIdentifier WHERE ClientIdentifierTypeID = 1 ) a
				ON cd.ContactID = a.ContactID
				WHERE rn = 1 


				-- CMBHSID
				UPDATE cd
				SET CMBHSID = AlternateID 
				FROM #CMHCClientRegistration cd
				INNER JOIN (select  rn = ROW_NUMBER() OVER (PARTITION BY contactid ORDER BY EffectiveDAte desc ), AlternateID, ContactID  from Registration.ContactClientIdentifier WHERE ClientIdentifierTypeID = 2 ) a
				ON cd.ContactID = a.ContactID
				WHERE rn = 1 

				-- ReferralDate
				UPDATE cd
				SET ReferralDate = rh.ReferralDate
				FROM #CMHCClientRegistration cd 
				LEFT JOIN  ( SELECT MAX(ReferralDate) as ReferralDate, ad.ContactID 
							FROM [Registration].[ReferralAdditionalDetails] AD 
							INNER JOIN  Registration.ReferralHeader rh ON RH.REFERRALHEADERID = AD.REFERRALHEADERID 
							GROUP BY ad.ContactID ) rh
				ON cd.ContactID = rh.ContactID
				WHERE DATEDIFF(yy,DateofBirth,GETDATE()) <= 3

				-- For the use of DISTINCT refer to the above description 
				INSERT INTO Synch.CMHCClientRegistration
				SELECT DISTINCT ContactID, MRN, FirstName, LastName, Middle, Suffix, EffectiveDate, EffectiveDateTime, Gender, DateofBirth, SSN, RaceCode, EthnicityCode, LegalStatusCode, IsActive, 
								TKIDSID, TKIDSCASEID, LCPrimaryLanguageID, LCSchoolDistrictID, IsCPSInvolved, ReferralDate, CareID, CMBHSID, BatchID, PresentingProblemID, ErrorMessage
				FROM #CMHCClientRegistration
 
END