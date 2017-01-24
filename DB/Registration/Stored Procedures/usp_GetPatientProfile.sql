-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetPatientProfile]
-- Author:		suresh Pandey
-- Date:		09/10/2015
--
-- Purpose:		Get Patient Profile
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Suresh Pandey	Initial
-- 10/23/2015   Justin Spalti - Added logic to get the contact's emergency contact information
-- 10/28/2016   John Crossen    tfs#3070  Change EmergencyContact Field 
-- 03/08/2016	Kyle Campbell	TFS #7099	Renamed "Alias" to "PreferredName"
-- 04/13/2016	Scott Martin	Added MedicaidID field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetPatientProfile]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY
		IF OBJECT_ID('tempdb..#tmpPatientProfile') IS NOT NULL
			DROP TABLE #tmpPatientProfile

		CREATE TABLE #tmpPatientProfile
		(
			PatientProfileID INT PRIMARY KEY IDENTITY(1, 1), 
			ContactID BIGINT NOT NULL,
			ContactTypeID INT NOT NULL,
			MRN BIGINT,
			MedicaidID NVARCHAR(50),
			ClientTypeID INT,
			FirstName NVARCHAR(200), 
			Middle NVARCHAR(200),
			LastName NVARCHAR(200),
			GenderID INT,
			DOB DATE,
			PreferredName NVARCHAR(200),
			LegalStatusID INT,
			EmergencyContactFirstName NVARCHAR(200),
			EmergencyContactLastName NVARCHAR(200),
			EmergencyContactPhoneNumber NVARCHAR(50),
			EmergencyContactExtension NVARCHAR(5)
		)
		
		INSERT INTO #tmpPatientProfile
		(
			ContactID,
			ContactTypeID,
			MRN,
			ClientTypeID,
			FirstName, 
			Middle,
			LastName,
			GenderID,
			DOB,
			PreferredName,
			LegalStatusID
		)
		SELECT
			c.ContactID,
			c.ContactTypeID,
			c.MRN,
			c.ClientTypeID, 
			c.FirstName,
			c.Middle,
			c.LastName,
			c.GenderID,
			c.DOB,
			c.PreferredName,
			ad.LegalStatusID
		FROM Registration.Contact c
		JOIN Registration.AdditionalDemographics ad
			ON ad.ContactID = c.ContactID
		WHERE c.ContactID = @ContactID 
			AND c.IsActive = 1;

		UPDATE #tmpPatientProfile
		SET MedicaidID = 
		ISNULL(
		(
			SELECT TOP 1
				CP.PolicyID
			FROM
				Registration.ContactPayor CP
				INNER JOIN Reference.Payor P
					ON CP.PayorID = P.PayorID
			WHERE
				PayorName LIKE '%medicaid%'
				AND CP.IsActive = 1
				AND ContactID = @ContactID
			ORDER BY
				CP.ContactPayorRank
		), 'N/A');


		IF OBJECT_ID('tempdb..#tmpEmergencyContact') IS NOT NULL
			DROP TABLE #tmpEmergencyContact

		CREATE TABLE #tmpEmergencyContact(EmergencyContactID INT PRIMARY KEY IDENTITY(1, 1),
										  ParentContactID BIGINT NOT NULL, FirstName NVARCHAR(200), LastName NVARCHAR(200),
										  PhoneNumber NVARCHAR(50), Extension NVARCHAR(5))
		
		INSERT INTO #tmpEmergencyContact(ParentContactID, FirstName, LastName, PhoneNumber, Extension)
		SELECT TOP 1 cr.ParentContactID, c.FirstName, c.LastName, p.Number, p.Extension
		FROM Registration.ContactRelationship cr
		JOIN Registration.Contact c
			ON c.ContactID = cr.ChildContactID
		LEFT JOIN Registration.ContactPhone cp
			ON cp.ContactID = c.ContactID
				AND cp.IsActive = 1
				AND cp.IsPrimary = 1
		LEFT JOIN Core.Phone p
			ON p.PhoneID = cp.PhoneID
		WHERE cr.ParentContactID = @ContactID			
			AND cr.IsActive = 1
			AND c.IsActive = 1
			AND cr.ContactTypeID = CASE WHEN cr.ContactTypeID = 3 THEN cr.ContactTypeID 
										WHEN cr.ContactTypeID = 4 AND cr.IsEmergency = 1 THEN cr.ContactTypeID
										ELSE -1
								   END
		ORDER BY CASE WHEN cr.ContactTypeID = 3 THEN 1 ELSE 0 END DESC

		UPDATE p
		SET p.EmergencyContactFirstName = e.FirstName,
			p.EmergencyContactLastName = e.LastName,
			p.EmergencyContactPhoneNumber = e.PhoneNumber,
			p.EmergencyContactExtension = e.Extension
		FROM #tmpPatientProfile p
		JOIN #tmpEmergencyContact e
			ON e.ParentContactID = p.ContactID

		SELECT ContactID, ContactTypeID, MRN, MedicaidID, ClientTypeID, FirstName, 
			   Middle, LastName, GenderID, DOB, PreferredName, LegalStatusID,
			   EmergencyContactFirstName, EmergencyContactLastName,
			   EmergencyContactPhoneNumber, EmergencyContactExtension
		FROM #tmpPatientProfile

		DROP TABLE #tmpEmergencyContact
		DROP TABLE #tmpPatientProfile
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END