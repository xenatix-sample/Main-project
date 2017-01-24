-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactDemographicChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Get ContactDemographicChangeLog
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14753	Initial Creation
-- 11/03/2016	Kyle Campbell	TFS #16309	Modified to return full unencrypted SSN in result grid
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Auditing].[usp_GetContactDemographicChangeLog]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';
	BEGIN TRY		
		EXEC Core.usp_OpenEncryptionKeys @ResultCode OUTPUT, @ResultMessage OUTPUT

		SELECT
			TransactionLogID,
			UserID,
			UserFirstName,
			UserLastName, 
			ChangedDate,
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
			CONVERT(NVARCHAR(9), Core.fn_Decrypt(SSNEncrypted)) AS SSN,
			SSNStatus,
			DriverLicense,
			DriverLicenseState,
			PreferredContactMethod,
			IsPregnant,
			GestationalAge,
			IsActive			
		FROM Auditing.ContactDemographicChangeLog
		WHERE ContactID = @ContactID
		ORDER BY ChangedDate DESC;
	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
