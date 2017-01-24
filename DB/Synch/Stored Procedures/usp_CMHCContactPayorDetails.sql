 
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_CMHCContactPayorDetails
-- Author:		Sumana Sangapu
-- Date:		07/09/2016
--
-- Purpose:		Generate Client Payor data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/09/2016	Sumana Sangapu		Initial creation
-- 08/08/2016	Sumana Sangapu		Filter only for contacttypeid=1 and MRN is not NULL
-- 08/09/2016	Sumana Sangapu		Added the exec proc to open up the Encryptionkeys
-- 08/10/2016	Sumana Sangapu		Added PolicyHolderSuffixID and BillingClientSuffixID
-- 08/19/2016	Sumana Sangapu		Fix for GroupID not showingup
-- 11/14/2016	Sumana Sangapu		Isactive = 1 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_CMHCContactPayorDetails]
@BatchID BIGINT,
@LastRunDate DATETIME
AS
BEGIN


	TRUNCATE TABLE Synch.CMHCContactPayorDetails

	EXEC [Core].[usp_OpenEncryptionKeys] '',''

	INSERT INTO Synch.CMHCContactPayorDetails
	  (ContactID, MRN, ContractID, PayorID, PayorPlanID, GroupID, PolicyHolderID,
	   BillingClientFirstName, BillingClientMiddleName, BillingClientLastName, BillingClientSuffixID, BillingClientSuffix,  
	   PolicyID, Deductible, Copay, CoInsurance, EffectiveDate, ExpirationDate, AddRetroDate, [PayorExpirationReasonID],
	   PolicyHolderFirstName, PolicyHolderMiddleName, PolicyHolderLastName, PolicyHolderSuffixID, PolicyHolderSuffix, 
	   PolicyHolderStreetAddress, PolicyHolderCityName,PolicyHolderStateName, PolicyHolderZip, 
	   PolicyHolderPhone, PolicyHolderRelationship, PolicyHolderSSN,
	   UniqueIdentifierID, BatchID )
	 SELECT cp.contactid, STUFF(MRN, 1, 0, REPLICATE('0', 9 - LEN(MRN))) AS MRN,-- Padded to 9, 
	   pa.ContactID, p.PayorID, pa.PayorPlanID, UPPER(cp.GroupID) , cp.PolicyHolderID, 
	   UPPER(cp.BillingFirstName),
	   UPPER(cp.BillingMiddleName),
	   UPPER(cp.BillingLastName),
	   BillingSuffixID,
	   UPPER(cp.BillingSuffixID),
	   PolicyID, Deductible, Copay, Coinsurance, CONVERT(varchar,cp.EffectiveDate,101),CONVERT(varchar,cp.ExpirationDate,101) , 
	   REPLACE(ISNULL(CONVERT(varchar, AddRetroDate,101), ''), '01/01/1900', '') as AddRetroDate, cp.[PayorExpirationReasonID],
	   UPPER(PolicyHolderFirstName), UPPER(PolicyHolderMiddleName), UPPER(PolicyHolderLastName), PolicyHolderSuffixID,UPPER(REPLACE(s.Suffix,'.','')) as  PolicyHolderSuffix, 
	   NULL as PolicyHolderStreetAddress, NULL as PolicyHolderCityName, NULL as  PolicyHolderStateName, NULL as PolicyHolderZip, 
	   NULL as  PolicyHolderPhone,NULL as  PolicyHolderRelationship,NULL as PolicyHolderSSN,
	   cp.ContactPayorId as UniqueIdentifierID , @BatchID
	  FROM  Registration.ContactPayor cp 
	  INNER JOIN Registration.Contact c
	  ON cp.ContactID = C.ContactID AND C.ContactTypeID = 1 AND C.MRN IS NOT NULL AND cp.Isactive = 1 
	  LEFT join Registration.PayorAddress pa
	  on cp.payoraddressid = pa.payoraddressid 
	  LEFT join [Reference].[Payor] p
	  on cp.PayorID = p.PayorID 
	  LEFT join [Reference].[PolicyHolder] ph
	  on cp.PolicyholderID = ph.Policyholderid 
	  LEFT OUTER JOIN Reference.Suffix S 
	  ON cp.PolicyHolderSuffixID = S.SuffixID
	  WHERE	(cp.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR cp.[SystemModifiedOn] >= ISNULL(@LastRunDate,''))  


	  -- PolicyHolderID = ContactID (Self) THEN BILLINGDETIALS TO BE POPULATED WITH POLICY HOLDER's DETAILS
	  UPDATE c
	  SET	C.BillingClientFirstName = UPPER(c.PolicyHolderFirstName),
			C.BillingClientLastName = UPPER(c.PolicyHolderLastName),
			C.BillingClientMiddleName = UPPER(c.PolicyHolderMiddleName),
			C.BillingClientSuffix = NULL
	  FROM Synch.CMHCContactPayorDetails c
	  WHERE c.ContactID = c.PolicyHolderID

	  -- PolicyHolderID = ContactID (Self) THEN NULL ALL POLICYHOLDERDETAILS  
	  UPDATE c
	  SET	C.PolicyHolderFirstName = NULL,
			C.PolicyHolderLastName = NULL,
			C.PolicyHolderMiddleName = NULL,
			C.PolicyHolderSuffix = NULL
	  FROM Synch.CMHCContactPayorDetails c
	  WHERE c.ContactID = c.PolicyHolderID

	  --When PolicyholderID <> Contactid then BillingDetails and PolicyHolderdetails are as is from Registration.ContactPayor and 

	   --PolicyHolderdetails 
	   UPDATE cpd
		SET PolicyHolderSSN = CONVERT(NVARCHAR(9), Core.fn_Decrypt(c.SSNEncrypted)) ,
			PolicyHolderDOB = CONVERT(varchar,DOB,101),
			PolicyHolderGender = CASE WHEN Gender = 'Female' THEN 'F' 
										  WHEN Gender = 'Male' THEN 'M' END 
		FROM Synch.CMHCContactPayorDetails cpd 
		LEFT JOIN  Registration.Contact C  
		ON  cpd.PolicyHolderID = c.ContactID  
		LEFT JOIN Reference.Suffix S ON C.SuffixID=S.SuffixID
		LEFT JOIN Reference.Gender g ON c.GenderID = g.GenderID
	    WHERE cpd.ContactID <> cpd.PolicyHolderID

	   -- BillingClientSuffix
	   UPDATE cpd
		SET BillingClientSuffix  = S.LegacyCode 
		FROM Synch.CMHCContactPayorDetails cpd 
		LEFT JOIN Reference.Suffix S ON BillingClientSuffixID = s.SuffixID


		UPDATE cpd
		SET	PolicyHolderStreetAddress = UPPER(a.Line1), 
			PolicyHolderCityName = UPPER(a.City), 
			PolicyHolderStateName = sp.[StateProvinceCode] ,  
			PolicyHolderZip = a.Zip
		FROM Synch.CMHCContactPayorDetails cpd
		LEFT JOIN Registration.ContactAddress ca
		ON cpd.PolicyHolderID = ca.ContactID
		INNER JOIN Core.Addresses a
		ON ca.AddressID = a.AddressID
		AND A.AddressTypeID = 1 --'Residence/Home'
		LEFT OUTER JOIN Reference.AddressType AT 
		ON AT.AddressTypeID = A.AddressTypeID    
		LEFT JOIN [Reference].[StateProvince] sp
		ON a.StateProvince = sp.[StateProvinceID] 
		LEFT JOIN [Reference].[County] c
		ON a.County = c.CountyID
		AND c.StateProvinceID = sp.[StateProvinceID] 
		WHERE cpd.ContactID <> cpd.PolicyHolderID
		AND ca.IsPrimary = 1 

		UPDATE cpd
		SET PolicyHolderPhone = phone.Number
		FROM Synch.CMHCContactPayorDetails cpd 
		LEFT JOIN Registration.ContactPhone cph
		ON cpd.PolicyHolderID = cph.ContactID 
		LEFT JOIN Core.Phone phone
		ON cph.PhoneID = phone.PhoneID
		WHERE cpd.ContactID <> cpd.PolicyHolderID
		AND	cph.IsPrimary = 1 
	
		UPDATE cpd
		SET [PolicyHolderRelationship] = UPPER(rt.LegacyCode)
		FROM Synch.CMHCContactPayorDetails cpd 
		LEFT JOIN [Registration].[ContactRelationship] cr
		ON  cpd.ContactID = cr.ParentContactID 
		AND cpd.PolicyHolderID = cr.ChildContactID 
		LEFT JOIN [Registration].[ContactRelationshipType] crt
		ON cr.ContactRelationshipID = crt.ContactRelationshipID
		LEFT JOIN [Reference].[RelationshipType] rt
		ON crt.RelationshipTypeID = rt.RelationshipTypeID 
		WHERE cpd.ContactID <> cpd.PolicyHolderID
	 
		 UPDATE cpd 
		 SET AddRetroDate = CONVERT(varchar,AddRetroDate,101)
		 FROM Synch.CMHCContactPayorDetails cpd 


	 END
