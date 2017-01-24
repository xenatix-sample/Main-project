-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactDemographicChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Add ContactDemographicChangeLog
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14753	Initial Creation
-- 11/03/2016	Kyle Campbell	TFS #16309	Modified to store SSNEncrypted field
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Auditing].[usp_AddContactDemographicChangeLog]
	@TransactionLogID BIGINT,
	@ContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		IF OBJECT_ID('tempdb..#tmpContact') IS NOT NULL
			DROP TABLE #tmpContact
		IF OBJECT_ID('tempdb..#tmpContactPresentingProblem') IS NOT NULL
			DROP TABLE #tmpContactPresentingProblem
		IF OBJECT_ID('tempdb..#tmpContactDemographicChangeLog') IS NOT NULL
			DROP TABLE #tmpContactDemographicChangeLog
		IF @TransactionLogID > 0
			BEGIN
				SELECT 
					ContactID,
					MRN,
					MPI,
					ContactTypeID,
					ClientTypeID,
					FirstName,
					Middle,
					LastName,
					SuffixID,
					GenderID,
					PreferredGenderID,
					TitleID,
					SequesteredByID,
					DOB,
					DOBStatusID,
					SSN,
					SSNStatusID,
					DriverLicense,
					DriverLicenseStateID,
					PreferredName,
					DeceasedDate,
					PreferredContactMethodID,
					ReferralSourceID,
					IsPregnant,
					GestationalAge,
					IsActive,
					SSNEncrypted,
					ModifiedBy,
					ModifiedOn
				INTO #tmpContact
				FROM Registration.Contact
				WHERE ContactID = @ContactID;

				SELECT 
					ContactPresentingProblemID,
					ContactID,
					PresentingProblemTypeID,
					EffectiveDate,
					ExpirationDate,
					IsActive,
					ModifiedBy,
					ModifiedOn
				INTO #tmpContactPresentingProblem
				FROM Registration.ContactPresentingProblem
				WHERE ContactID = @ContactID AND IsActive = 1;

				SELECT DISTINCT
					@TransactionLogID AS [TransactionLogID],
					@ModifiedBy AS UserID,
					C.ModifiedOn AS [ChangedDate],
					U.FirstName AS [UserFirstName],
					U.LastName AS [UserLastName],
					C.ContactID,
					PPT.PresentingProblemType,
					CPP.EffectiveDate,
					CPP.ExpirationDate,
					C.MRN,
					C.MPI,
					CT.ClientType,
					C.FirstName,
					C.LastName,
					C.Middle,
					C.PreferredName,
					T.Title,
					S.Suffix,
					G.Gender,
					PG.Gender AS [PreferredGender],
					C.DOB,
					DS.DOBStatus,
					C.SSN,
					SS.SSNStatus,
					C.DriverLicense,
					SP.StateProvinceName AS [DriverLicenseState],
					CM.ContactMethod AS [PreferredContactMethod],
					C.IsPregnant,
					C.GestationalAge,
					C.IsActive,
					C.SSNEncrypted
				INTO #tmpContactDemographicChangeLog	
				FROM
					#tmpContact C
					LEFT JOIN #tmpContactPresentingProblem CPP ON C.ContactID = CPP.ContactID
					LEFT JOIN Core.Users U ON U.UserID = @ModifiedBy
					LEFT JOIN Reference.ClientType CT ON C.ClientTypeID = CT.ClientTypeID
					LEFT JOIN Reference.Title T ON C.TitleID = T.TitleID
					LEFT JOIN Reference.Suffix S ON C.SuffixID = S.SuffixID
					LEFT JOIN Reference.Gender G ON C.GenderID = G.GenderID
					LEFT JOIN Reference.Gender PG ON C.PreferredGenderID = PG.GenderID
					LEFT JOIN Reference.DOBStatus DS ON C.DOBStatusID = DS.DOBStatusID
					LEFT JOIN Reference.SSNStatus SS ON C.SSNStatusID = SS.SSNStatusID
					LEFT JOIN Reference.StateProvince SP ON C.DriverLicenseStateID = SP.StateProvinceID
					LEFT JOIN Reference.ContactMethod CM ON C.PreferredContactMethodID = CM.ContactMethodID
					LEFT JOIN Reference.PresentingProblemType PPT ON PPT.PresentingProblemTypeID = CPP.PresentingProblemTypeID

				MERGE INTO Auditing.ContactDemographicChangeLog AS TARGET
				USING (SELECT * FROM #tmpContactDemographicChangeLog) AS SOURCE
				ON (SOURCE.TransactionLogID = TARGET.TransactionLogID)
				WHEN NOT MATCHED THEN INSERT
				(
					TransactionLogID,
					UserID,
					ChangedDate,
					UserFirstName,
					UserLastName, 
					ContactID,			
					PresentingProblemType,			
					EffectiveDate,
					ExpirationDate,
					MRN,
					MPI,
					ClientType,
					FirstName,
					LastName,
					Middle,
					PreferredName,
					Title,
					Suffix,
					Gender,
					PreferredGender,
					DOB,
					DOBStatus,
					SSN,
					SSNStatus,
					DriverLicense,
					DriverLicenseState,
					PreferredContactMethod,
					IsPregnant,
					GestationalAge,
					IsActive,
					SSNEncrypted
				) 
				VALUES
				(
					SOURCE.TransactionLogID,
					SOURCE.UserID,
					SOURCE.ChangedDate,
					SOURCE.UserFirstName,
					SOURCE.UserLastName, 
					SOURCE.ContactID,
					SOURCE.PresentingProblemType,
					SOURCE.EffectiveDate,
					SOURCE.ExpirationDate,
					SOURCE.MRN,
					SOURCE.MPI,
					SOURCE.ClientType,
					SOURCE.FirstName,
					SOURCE.LastName,
					SOURCE.Middle,
					SOURCE.PreferredName,
					SOURCE.Title,
					SOURCE.Suffix,
					SOURCE.Gender,
					SOURCE.PreferredGender,
					SOURCE.DOB,
					SOURCE.DOBStatus,
					SOURCE.SSN,
					SOURCE.SSNStatus,
					SOURCE.DriverLicense,
					SOURCE.DriverLicenseState,
					SOURCE.PreferredContactMethod,
					SOURCE.IsPregnant,
					SOURCE.GestationalAge,
					SOURCE.IsActive,
					SOURCE.SSNEncrypted
				)
				WHEN MATCHED THEN UPDATE
				SET 
					TransactionLogID = SOURCE.TransactionLogID,
					UserID = SOURCE.UserID,
					ChangedDate = SOURCE.ChangedDate,
					UserFirstName = SOURCE.UserFirstName,
					UserLastName = SOURCE.UserLastName, 
					ContactID = SOURCE.ContactID,
					PresentingProblemType = SOURCE.PresentingProblemType,
					EffectiveDate = SOURCE.EffectiveDate,
					ExpirationDate = SOURCE.ExpirationDate,
					MRN = SOURCE.MRN,
					MPI = SOURCE.MPI,
					ClientType = SOURCE.ClientType,
					FirstName = SOURCE.FirstName,
					LastName = SOURCE.LastName,
					Middle = SOURCE.Middle,
					PreferredName = SOURCE.PreferredName,
					Title = SOURCE.Title,
					Suffix = SOURCE.Suffix,
					Gender = SOURCE.Gender,
					PreferredGender = SOURCE.PreferredGender,
					DOB = SOURCE.DOB,
					DOBStatus = SOURCE.DOBStatus,
					SSN = SOURCE.SSN,
					SSNStatus = SOURCE.SSNStatus,
					DriverLicense = SOURCE.DriverLicense,
					DriverLicenseState = SOURCE.DriverLicenseState,
					PreferredContactMethod = SOURCE.PreferredContactMethod,
					IsPregnant = SOURCE.IsPregnant,
					GestationalAge = SOURCE.GestationalAge,
					IsActive = SOURCE.IsActive,
					SSNEncrypted = SOURCE.SSNEncrypted;

				DROP TABLE #tmpContact
				DROP TABLE #tmpContactPresentingProblem
				DROP TABLE #tmpContactDemographicChangeLog
		END
	END TRY
	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
