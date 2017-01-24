-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_AddPotentialMergeContacts
-- Author:		Scott Martin
-- Date:		12/19/2016
--
-- Purpose:		Populates potential contact match table
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2016	Scott Martin	Initial creation
-- 01/05/2017	Scott Martin	Modified the proc to insert a blank record if no duplicates found; Fixed a bug where Matching for Driver License wasn't working
-- 01/06/2017	Scott Martin	Changed the Phone match to only look at Primary phone numbers
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddPotentialMergeContacts]
(
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,	
			@ResultMessage = 'Data saved successfully'

	BEGIN TRY	
	CREATE TABLE #Matches
	(
		ContactID BIGINT,
		SSNMatch TINYINT,
		DOBMatch TINYINT,
		DLMatch TINYINT,
		PhoneMatch TINYINT,
		EmailMatch TINYINT,
		MedicaidIDMatch TINYINT,
		JailIDMatch TINYINT
	)

	;WITH NamePhone (ContactID, PhCount, CCount)
	AS
	(
		SELECT
			C.ContactID,
			COUNT(*) OVER(PARTITION BY FirstName, LastName, P.Number),
			COUNT(*) OVER(PARTITION BY C.ContactID, FirstName, LastName, P.Number)
		FROM
			Registration.Contact C
			INNER JOIN Registration.ContactPhone CP
				ON C.ContactID = CP.ContactID
				AND CP.IsActive = 1
				AND CP.IsPrimary = 1
			INNER JOIN Core.Phone P
				ON CP.PhoneID = P.PhoneID
		WHERE
			ISNULL(FirstName, '') <> ''
			AND ISNULL(LastName, '') <> ''
			AND C.IsActive = 1
			AND C.MRN IS NOT NULL
	)
	INSERT INTO #Matches
	(
		ContactID,
		PhoneMatch
	)
	SELECT
		NamePhone.ContactID,
		1
	FROM
		NamePhone
	WHERE
		NamePhone.PHCount > 1
		AND NamePhone.PhCount <> NamePhone.CCount


	;WITH NameEmail (ContactID, EmailCount)
	AS
	(
		SELECT
			C.ContactID,
			COUNT(*) OVER(PARTITION BY FirstName, LastName, CE.EmailID)
		FROM
			Registration.Contact C
			INNER JOIN Registration.ContactEmail CE
				ON C.ContactID = CE.ContactID
				AND CE.IsActive = 1
		WHERE
			ISNULL(FirstName, '') <> ''
			AND ISNULL(LastName, '') <> ''
			AND C.IsActive = 1
			AND C.MRN IS NOT NULL
	)
	INSERT INTO #Matches
	(
		ContactID,
		EmailMatch
	)
	SELECT
		ContactID,
		1
	FROM
		NameEmail
	WHERE
		NameEmail.EmailCount > 1


	;WITH NameDL (ContactID, DLCount)
	AS
	(
		SELECT
			ContactID,
			COUNT(*) OVER(PARTITION BY FirstName, LastName, DriverLicense, DriverLicenseStateID)
		FROM
			Registration.Contact
		WHERE
			ISNULL(FirstName, '') <> ''
			AND ISNULL(LastName, '') <> ''
			AND ISNULL(DriverLicense, '') <> ''
			AND ISNULL(DriverLicenseStateID, '') <> ''
			AND IsActive = 1
			AND MRN IS NOT NULL
	)
	INSERT INTO #Matches
	(
		ContactID,
		DLMatch
	)
	SELECT
		ContactID,
		1
	FROM
		NameDL
	WHERE
		NameDL.DLCount > 1


	;WITH NameDOB (ContactID, DOBCount)
	AS
	(
		SELECT
			ContactID,
			COUNT(*) OVER(PARTITION BY FirstName, LastName, DOB)
		FROM
			Registration.Contact
		WHERE
			ISNULL(FirstName, '') <> ''
			AND ISNULL(LastName, '') <> ''
			AND ISNULL(DOB, '') <> ''
			AND IsActive = 1
			AND MRN IS NOT NULL
	)
	INSERT INTO #Matches
	(
		ContactID,
		DOBMatch
	)
	SELECT
		NameDOB.ContactID,
		1
	FROM
		NameDOB
	WHERE
		NameDOB.DOBCount > 1


	EXEC Core.usp_OpenEncryptionkeys NULL, NULL

	;WITH SSNDecrypt (ContactID, SSN, SSNCount)
	AS
	(
		SELECT
			ContactID,
			Core.fn_Decrypt(SSNEncrypted),
			COUNT(*) OVER(PARTITION BY Core.fn_Decrypt(SSNEncrypted)) SSNCount
		FROM
			Registration.Contact
		WHERE
			SSNEncrypted IS NOT NULL
			AND IsActive = 1
			AND MRN IS NOT NULL
	)
	INSERT INTO #Matches
	(
		ContactID,
		SSNMatch
	)
	SELECT
		ContactID,
		1
	FROM
		SSNDecrypt
	WHERE
		SSN <> ''
		AND SSNDecrypt.SSNCount > 1

	;WITH JailMatch (ContactID, JailCount)
	AS
	(
		SELECT
			C.ContactID,
			COUNT(*) OVER(PARTITION BY C.FirstName, C.LastName, CCI.AlternateID)
		FROM
			Registration.Contact C
			INNER JOIN Registration.ContactClientIdentifier CCI
				ON C.ContactID = CCI.ContactID
				AND CCI.IsActive = 1
		WHERE
			CCI.ClientIdentifierTypeID = 6
			AND C.IsActive = 1
			AND C.MRN IS NOT NULL
	)
	INSERT INTO #Matches
	(
		ContactID,
		JailIDMatch
	)
	SELECT
		ContactID,
		1
	FROM
		JailMatch
	WHERE
		JailMatch.JailCount > 1


	;WITH MedicaidMatch (ContactID, MedicaidIDCount, CCount)
	AS
	(
		SELECT
			C.ContactID,
			COUNT(*) OVER(PARTITION BY C.FirstName, C.LastName, CP.PolicyID),
			COUNT(*) OVER(PARTITION BY C.ContactID, CP.PolicyID)
		FROM
			Registration.Contact C
			INNER JOIN Registration.ContactPayor CP
				ON C.ContactID = CP.ContactID
				AND CP.IsActive = 1
			INNER JOIN Reference.Payor P
				ON CP.PayorID = P.PayorID
		WHERE
			P.PayorName LIKE '%Medicaid%'
			AND C.IsActive = 1
			AND C.MRN IS NOT NULL
	)
	INSERT INTO #Matches
	(
		ContactID,
		MedicaidIDMatch
	)
	SELECT
		ContactID,
		1
	FROM
		MedicaidMatch
	WHERE
		MedicaidMatch.MedicaidIDCount > 1
		AND MedicaidMatch.CCount <> MedicaidMatch.MedicaidIDCount

	TRUNCATE TABLE Core.PotentialContactMatches;

	IF EXISTS (SELECT TOP 1 * FROM #Matches)
		BEGIN
		INSERT INTO Core.PotentialContactMatches
		(
			ContactID,
			SSNMatch,
			DOBMatch,
			DLMatch,
			PhoneMatch,
			EmailMatch,
			MedicaidIDMatch,
			JailIDMatch,
			IsActive,
			CreatedBy,
			CreatedOn
		)
		SELECT
			ContactID,
			CAST(MAX(ISNULL(SSNMatch, 0)) AS BIT),
			CAST(MAX(ISNULL(DOBMatch, 0)) AS BIT),
			CAST(MAX(ISNULL(DLMatch, 0)) AS BIT),
			CAST(MAX(ISNULL(PhoneMatch, 0)) AS BIT),
			CAST(MAX(ISNULL(EmailMatch, 0)) AS BIT),
			CAST(MAX(ISNULL(MedicaidIDMatch, 0)) AS BIT),
			CAST(MAX(ISNULL(JailIDMatch, 0)) AS BIT),
			1,
			@ModifiedBy,
			@ModifiedOn
		FROM
			#Matches
		GROUP BY
			ContactID
		END
	ELSE
		BEGIN
		INSERT INTO Core.PotentialContactMatches
		(
			ContactID,
			IsActive,
			CreatedBy,
			CreatedOn
		)
		VALUES (NULL, 0, @ModifiedBy, @ModifiedOn)
		END

	DROP TABLE #Matches
		

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END