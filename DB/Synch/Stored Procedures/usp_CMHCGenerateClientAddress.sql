-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_CMHCGenerateClientAddress
-- Author:		Sumana Sangapu
-- Date:		07/09/2016
--
-- Purpose:		Generate Client Address data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/09/2016	Sumana Sangapu		Initial creation
-- 08/08/2016	Sumana Sangapu		Filter only for contacttypeid=1 and MRN is not NULL
-- 08/09/2016	Sumana Sangapu		Address type fix
-- 10/06/2016	Sumana Sangapu		Return 255 as CountyCode when State is TX and no county is selected.	
-- 10/13/2016	Sumana Sangapu		Remove the ISPrimary and send only addresses with "Home/Residence" addresstype ( Defect ID - 16199 )
-- 10/14/2016	Sumana Sangapu		Return 256 when State <> TX and CountyCode IS NULL
-- 10/18/2016	Sumana Sangapu		Return EffectiveDate for Registration.ContactAddress
-- 11/14/2016	Sumana Sangapu		Isactive = 1 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Synch.usp_CMHCGenerateClientAddress
@BatchID BIGINT,
@LastRunDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	TRUNCATE TABLE [Synch].[CMHCClientAddress]

	INSERT INTO [Synch].[CMHCClientAddress]
	(ContactID, MRN, AddressType, Line1, City, County, StateProvinceCode, Zip, ComplexName, GateCode, PhoneNumber, ContactAddressID, BatchID,EffectiveDate)
	SELECT 
		CA.ContactID,
		STUFF(Contact.MRN, 1, 0, REPLICATE('0', 9 - LEN(Contact.MRN))) AS MRN,-- Padded to 9
		UPPER(AT.AddressType),
		RTRIM(LEFT(UPPER(A.Line1) +' ' +ISNULL(UPPER(A.Line2),''),26)) AS Street,
		UPPER(A.City) AS City,
		CASE WHEN A.StateProvince = 43 AND A.County IS NULL THEN '255' 
			 WHEN A.StateProvince = 43 AND A.County IS NOT NULL THEN C.LegacyCode
			 WHEN A.StateProvince <> 43 AND A.County IS NULL THEN '256'
			 WHEN A.StateProvince <> 43 AND A.County IS NOT NULL THEN '256' END AS CountyCode,
		UPPER(SP.StateProvinceCode) AS StateCode,
		-- For county, make sure in TX, else code 256
		REPLACE(A.Zip,'-','') AS PostalCode,
		UPPER(A.ComplexName) AS ApartmentComplex,
		A.GateCode AS GateCode,
		P.Number AS PreferredPhone,
		CA.ContactAddressID AS UniqueID,
		@BatchID as BatchID,
		CONVERT(varchar, CA.EffectiveDate,101) as EffectiveDate 
		FROM 
		Core.Addresses A 
		JOIN Registration.ContactAddress CA ON CA.AddressID = A.AddressID AND A.AddressTypeID = 1 AND  CA.Isactive = 1 
		JOIN Registration.Contact Contact ON CA.ContactID=Contact.ContactID AND Contact.ContactTypeID = 1 AND Contact.MRN IS NOT NULL
		JOIN Reference.AddressType AT ON AT.AddressTypeID = A.AddressTypeID 
		LEFT OUTER JOIN Registration.ContactPhone CP ON CP.ContactID = CA.ContactID AND CP.IsPrimary=1
		LEFT OUTER JOIN Core.Phone P ON P.PhoneID = CP.PhoneID
		LEFT OUTER JOIN Reference.County C ON A.County=C.CountyID
		LEFT OUTER JOIN Reference.StateProvince SP ON A.StateProvince = SP.StateProvinceID
		WHERE (A.[SystemCreatedOn] >= ISNULL(@LastRunDate,'') OR A.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR 
			(CA.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR CA.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR
			(P.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR P.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR
			(CP.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR CP.[SystemModifiedOn] >= ISNULL(@LastRunDate,''))
		Order by ca.ContactID


END
GO
