-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactRelationForContactType]
-- Author:		Gurpreet Singh
-- Date:		08/24/2015
--
-- Purpose:		Get Contact List for given ContactType for Patient
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	-	Gurpreet Singh	-	Initial Creation
-- 08/26/2015	-	Gurpreet Singh	-	Added ContactRelationshipID in output list
-- 09/01/2015	-	Gurpreet Singh	-	Added SSN, DriverLicense, AlternateID fields to output
-- 09/03/2015	-	Gurpreet Singh	-	Updated IsActive check from Contact to ContactRelationship table, Removed Age
-- 09/08/2015	-	Rajiv Ranjan	-	Added DriverLicenseStateID & ClientIdentifierTypeID
-- 09/17/2015	-	D. Christopher	-	Added ParentContactID for completeness and to support entity hierarchy in Offline Mode
-- 09/17/2015	-	Avikal Gupta	-	TFS#1983 -- Added LivingWithClientStatus, ReceiveCorrespondenceID and removed LivingWithClientStatusID
-- 10/28/2016   John Crossen    tfs#3070  Change EmergencyContact Field 
-- 10/31/2015 - Arun Choudhary - Added modifiedon in select. Needed in tiles flyout.
-- 01/08/2015	Vishal Joshi 	Removed AlternateID and ClientIndentifierTypeID
-- 01/12/2016 - Gaurav Gupta   - Added EducationStatusID,EmploymentStatusID column
-- 03/14/2016	Kyle Campbell	TFS #5809 Modified proc to return CollateralTypeID
-- 03/16/2016	Rajiv Ranjan	Added VetranStatusID
-- 03/17/2016   Arun Choudhary  Added SchoolAttended,SchoolBeginDate,SchoolEndDate
-- 04/12/2016   Arun Choudhary	Added IsPolicyHolder
-- 06/07/2016   Lokesh Singhal	Removed IsPolicyHolder,OtherRelationship,RelationshipTypeID,CollateralTypeID
-- 09/14/2016   Arun Choudhary	Added CollateralEffectiveDate and CollateralExpirationDate
-- 11/02/2016   Rajiv Ranjan	Added Relationship(s)
-- 11/04/2016   Vishal Yadav	IsLAR introduced
-- 12/09/2016   Arun Choudhary	CollateralTypes introduced
-- 12/09/2016   Arun Choudhary	Filter inactive Relationship(s)
-- 01/05/2017	Sumana Sangapu	Correction for Duplicate Collaterals Issue 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactRelationForContactType]
	@ContactID BIGINT,
	@ContactTypeID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	;WITH CTERelationShip
		AS
		(
		SELECT 
			CR.[ChildContactID],
			 STUFF((SELECT
					   ', ' + 
					  CASE WHEN RelationShipType IS NULL THEN '' ELSE RelationShipType END + 
					  CASE WHEN OtherRelationship IS NULL THEN '' ELSE ( Case when (RelationshipTypeID =7 OR RelationshipTypeID =49 OR 
					  RelationshipTypeID =125) AND OtherRelationship IS NOT NULL Then ' (' +OtherRelationship +')' Else '' End ) END
					   FROM
					  ( SELECT
					  ROW_NUMBER() OVER(PARTITION BY RT1.RelationShipType ORDER BY (SELECT 1)) RowNo,
					   RT1.RelationShipType,
					  CRT1.OtherRelationship,
					  rt1.RelationshipTypeID
					  FROM [Registration].[ContactRelationship] CR1
						LEFT JOIN Registration.ContactRelationshipType CRT1 ON CRT1.ContactRelationshipID = CR1.ContactRelationshipID
						LEFT JOIN Reference.RelationshipType rt1 ON rt1.RelationshipTypeID=CRT1.RelationshipTypeID
					  WHERE CR.[ChildContactID]= CR1.[ChildContactID]
					  AND CRT1.IsActive=1
						AND RT1.IsActive=1 AND CR1.IsActive=1
						AND CR1.ContactRelationshipID=CR.ContactRelationshipID
						AND  (RT1.RelationshipTypeID IS NOT NULL OR CRT1.OtherRelationship IS NOT NULL)
						AND ( CAST(ExpirationDate as Date) > CAST(GETDATE() as Date) or ExpirationDate IS NULL)
						)RelationshipTable WHERE RowNo=1
					  FOR XML PATH (''))
					  , 1, 1, '') AS RelationShipType 

			FROM [Registration].[ContactRelationship] CR
			WHERE 
						CR.[ParentContactID] = @ContactID 
						AND CR.[IsActive] = 1
						AND CR.[ContactTypeID] = @ContactTypeID
		)
		,IsLARCTE(ChildContactID,Value)
		AS
		(
			SELECT ChildContactId,Value From(
			SELECT ROW_NUMBER() OVER(PARTITION BY CR.ChildContactId ORDER BY (SELECT 1)) RowNo, CR.ChildContactID, 1 Value
			FROM [Registration].[ContactRelationship] CR
						JOIN Registration.ContactRelationshipType CRT ON CRT.ContactRelationshipID = CR.ContactRelationshipID
						JOIN Reference.RelationshipType rt ON rt.RelationshipTypeID=CRT.RelationshipTypeID
			WHERE 
						CR.[ParentContactID] = @ContactID 
						AND CR.[IsActive] = 1
						AND CRT.IsActive=1
						AND RT.IsActive=1
						AND CR.[ContactTypeID] = @ContactTypeID
						AND RT.RelationshipTypeID=14
						)IsLARCTETable WHERE RowNo=1
		)
		,CollateralType
		AS
		(
			SELECT  ChildContactID,
					STUFF(
						 (
						 SELECT  ', ' + convert(varchar(MAX),CollateralType)
						 FROM (
							SELECT ROW_NUMBER() OVER(PARTITION BY CT.CollateralType ORDER BY (SELECT 1)) RowNo, CT.CollateralType
						  FROM Registration.ContactRelationshipType CRT
							INNER JOIN Registration.ContactRelationship CR
								ON CRT.ContactRelationshipID = CR.ContactRelationshipID
							INNER JOIN Reference.RelationshipGroupDetails RGD
								ON CRT.RelationshipTypeID = RGD.RelationshipTypeID
								INNER JOIN Reference.RelationshipGroup RG
								ON RG.RelationshipGroupID = RGD.RelationshipGroupID
								INNER JOIN Reference.CollateralType CT
								ON CT.RelationshipGroupID = RGD.RelationshipGroupID
						WHERE 
							CRT.IsActive = 1 		
						  AND  CRR.ChildContactID = CR.ChildContactID
						  AND  CRR.ParentContactID = CR.ParentContactID
						  AND (CRT.ExpirationDate IS NULL 
								OR CRT.ExpirationDate > Cast(GETDATE() as Date)) 
								)CollateralTypeTable WHERE RowNo=1
						  FOR XML PATH (''))
						  , 1, 1, '')  AS CollateralTypes

				FROM Registration.ContactRelationship CRR
				WHERE CRR.[ParentContactID] = @ContactID   
				AND   CRR.IsActive=1	 
		)
		SELECT
			CR.[ParentContactID],
			C.[ContactID], 
			C.[FirstName],
			C.[LastName],
			C.[Middle],
			C.[DOB],
			C.[SuffixID],
			C.[GenderID],
			CR.[ContactTypeID],
			CR.[LivingWithClientStatus],
			CR.ReceiveCorrespondenceID,
			CR.[ContactRelationshipID],
			CR.IsEmergency AS EmergencyContact,
			CR.EducationStatusID ,
			CR.SchoolAttended,
			CR.SchoolBeginDate,
			CR.SchoolEndDate,
			CR.EmploymentStatusID,
			CR.VeteranStatusID,
			C.[SSN],
			C.[DriverLicense],
			c.[DriverLicenseStateID],
			CR.[CollateralEffectiveDate],
			CR.[CollateralExpirationDate],
			c.ModifiedOn,
			CTE.RelationShipType AS Relationships,
			Case When LAR.Value=1 Then CAST (1 AS bit) Else CAST (0 AS bit) End 'IsLAR',
			CT.CollateralTypes
		FROM 
			[Registration].[ContactRelationship] AS CR
			INNER JOIN [Registration].[Contact] AS C ON CR.[ChildContactID] = C.[ContactID]
			INNER JOIN CTERelationShip CTE ON CTE.ChildContactID=CR.ChildContactID
			LEFT JOIN IsLARCTE LAR ON LAR.ChildContactID=CR.ChildContactID
			LEFT JOIN CollateralType CT ON CT.ChildContactID=C.ContactID
			
		WHERE 
			CR.[ParentContactID] = @ContactID 
			AND CR.[IsActive] = 1
			AND C.IsActive=1
			AND CR.[ContactTypeID] = @ContactTypeID

	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END