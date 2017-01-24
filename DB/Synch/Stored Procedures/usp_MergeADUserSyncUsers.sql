-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_MergeADUserSyncUsers]
-- Author:		Sumana Sangapu
-- Date:		08/20/2016
--
-- Purpose:		Merge the User details for AD Services. Used in ADUserSync SSIS package.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/20/2016	Sumana Sangapu	Initial creation.
-- 08/29/2016	Scott Martin	Added code to flag AD User records as duplicates
-- 09/06/2016	Rahul Vats		Review the proc and tested the proc.
-- 09/09/2016	Sumana Sangapu	Added the logic to update the UserIdentifierTypeID_2 (MHMRTCID)
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_MergeADUserSyncUsers]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY 
		-- MERGE Core.Users with the latest Users
		-- GenderID not available
		-- DOB  not available

		--Error code: 2627; Error Message: Violation of UNIQUE KEY constraint 'IX_UserName'. Cannot insert duplicate key in object 'Core.Users'. The duplicate key value is (<>).
		UPDATE ADUS
		SET IsDuplicateUser = 1,
			ErrorMessage = ISNULL(ErrorMessage,'')+ 'UserName is associated with a different GUID. '
		FROM
			Synch.ADUserStage ADUS
			INNER JOIN Core.Users U
				ON ADUS.UserName = U.UserName
				AND ISNULL(U.UserGUID,'') <> ISNULL(ADUS.UserGUID ,'')
		WHERE
			ADUS.UserName = U.UserName
			AND ISNULL(U.UserGUID,'') <> ISNULL(ADUS.UserGUID ,'');

		UPDATE ADUS
		SET IsDuplicateUser = 1,
			ErrorMessage = ISNULL(ErrorMessage,'') + 'EmployeeID-"Staff ID"  is associated with a different GUID. '
		FROM
			Synch.ADUserStage ADUS
			INNER JOIN Core.UserIdentifierDetails UID
				ON ADUS.UserIdentifier_1 = UID.IDNumber
				AND UID.UserIdentifierTypeID = 1
			INNER JOIN Core.Users U
				ON UID.UserID = U.UserID
		WHERE
			ADUS.UserIdentifier_1 = UID.IDNumber
			AND ISNULL(U.UserGUID,'') <> ISNULL(ADUS.UserGUID ,'');

		UPDATE ADUS
		SET IsDuplicateUser = 1,
			ErrorMessage = ISNULL(ErrorMessage,'') + 'EmployeeNumber-"MHMRTC ID" is associated with a different GUID. '
		FROM
			Synch.ADUserStage ADUS
			INNER JOIN Core.UserIdentifierDetails UID
				ON ADUS.UserIdentifier_2 = UID.IDNumber
				AND UID.UserIdentifierTypeID = 8
			INNER JOIN Core.Users U
				ON UID.UserID = U.UserID
		WHERE
			ADUS.UserIdentifier_2 = UID.IDNumber
			AND ISNULL(U.UserGUID,'') <> ISNULL(ADUS.UserGUID ,'');

		DECLARE @UserID TABLE (MergeAction NVARCHAR(50), UserGUID NVARCHAR(500), UserName NVARCHAR(100), UserID INT);

		MERGE Core.Users AS TARGET
		USING
		(
			SELECT
				UserName,
				FirstName,
				LastName,
				Middlename,
				IsTemporaryPassword,
				EffectiveFromDate,
				EffectiveToDate,
				UserGUID,   
				IsActive,
				'1' AS ModifiedBy,
				GETDATE() AS ModifiedOn,
				'1' AS CreatedBy,
				GETDATE() AS CreatedOn,
				GETDATE() AS SystemCreatedOn,
				GETDATE() AS SystemModifiedOn,
				StateProvinceID,
				CountryID
			FROM
				Synch.ADUserStage
			WHERE
				IsDuplicateUser = 0
				and ErrorMessage is null
		) AS SOURCE 
			ON ISNULL(TARGET.UserGUID,'') = ISNULL(SOURCE.UserGUID ,'')
			AND TARGET.UserName = SOURCE.Username
		WHEN MATCHED THEN 
			UPDATE SET	
			UserName = SOURCE.Username,
			FirstName = SOURCE.FirstName,
			LastName = SOURCE.LastName,
			MiddleName = SOURCE.MiddleName,
			IsActive = Source.IsActive,
			EffectiveToDate = SOURCE.EffectiveToDate,
			ADFlag = 1
		WHEN NOT MATCHED THEN
			INSERT
			(
				UserName,
				FirstName,
				LastName,
				MiddleName,
				IsTemporaryPassword,
				EffectiveFromDate,
				EffectiveToDate,
				UserGUID,
				ADFlag, 
				IsActive,
				ModifiedBy,
				ModifiedOn,
				CreatedBy,
				CreatedOn,
				SystemCreatedOn,
				SystemModifiedOn
			)
			VALUES
			(
				SOURCE.UserName,
				SOURCE.FirstName,
				SOURCE.LastName,
				SOURCE.MiddleName,
				SOURCE.IsTemporaryPassword,
				SOURCE.EffectiveFromDate,
				SOURCE.EffectiveToDate,
				SOURCE.UserGUID,
				1,
				SOURCE.IsActive,
				SOURCE.ModifiedBy,
				SOURCE.ModifiedOn,
				SOURCE.CreatedBy,
				SOURCE.CreatedOn,
				SOURCE.SystemCreatedOn,
				SOURCE.SystemModifiedOn
			)
		OUTPUT $action, inserted.UserGUID, inserted.UserName, inserted.UserID
		INTO @UserID;

		UPDATE Synch.ADUserStage
		SET UserID = UID.UserID
		FROM
			Synch.ADUserStage ADUS
			INNER JOIN @UserID UID
				ON ADUS.UserGUID = UID.UserGUID
				AND ADUS.UserName = UID.UserName
		WHERE
			ADUS.UserGUID = UID.UserGUID
			AND ADUS.UserName = UID.UserName;

		MERGE Core.UserIdentifierDetails AS TARGET
		USING
		(
			SELECT
				UserID,
				UserIdentifierTypeID_1,
				UserIdentifier_1,
				EffectiveFromDate,
				EffectiveToDate,
				IsActive,
				'1' AS ModifiedBy,
				GETDATE() AS ModifiedOn,
				'1' AS CreatedBy,
				GETDATE() AS CreatedOn,
				GETDATE() AS SystemCreatedOn,
				GETDATE() AS SystemModifiedOn
			FROM
				Synch.ADUserStage
				WHERE UserIdentifier_1 IS NOT NULL  and UserID IS NOT NULL
		) AS SOURCE
			ON TARGET.UserID = SOURCE.UserID
			AND TARGET.UserIdentifierTypeID = SOURCE.UserIdentifierTypeID_1
			--AND TARGET.IDNumber = SOURCE.UserIdentifier_1
		WHEN NOT MATCHED THEN
			INSERT
			(
				UserID,
				UserIdentifierTypeID,
				IDNumber,
				EffectiveDate,
				IsActive,
				ModifiedBy,
				ModifiedOn,
				CreatedBy,
				CreatedOn,
				SystemCreatedOn,
				SystemModifiedOn
			)
			VALUES
			(
				SOURCE.UserID,
				SOURCE.UserIdentifierTypeID_1,
				SOURCE.UserIdentifier_1,
				SOURCE.EffectiveFromDate,
				SOURCE.IsActive,
				SOURCE.ModifiedBy,
				SOURCE.ModifiedOn,
				SOURCE.CreatedBy,
				SOURCE.CreatedOn,
				SOURCE.SystemCreatedOn,
				SOURCE.SystemModifiedOn
			)
			WHEN MATCHED THEN
			UPDATE SET TARGET.IDNumber = SOURCE.UserIdentifier_1;

		MERGE Core.UserIdentifierDetails AS TARGET
		USING
		(
			SELECT
				UserID,
				UserIdentifierTypeID_2,
				UserIdentifier_2,
				EffectiveFromDate,
				EffectiveToDate,
				IsActive,
				'1' AS ModifiedBy,
				GETDATE() AS ModifiedOn,
				'1' AS CreatedBy,
				GETDATE() AS CreatedOn,
				GETDATE() AS SystemCreatedOn,
				GETDATE() AS SystemModifiedOn
			FROM
				Synch.ADUserStage
			WHERE UserIdentifier_2 IS NOT NULL and UserID IS NOT NULL
		) AS SOURCE
			ON TARGET.UserID = SOURCE.UserID
			AND TARGET.UserIdentifierTypeID = SOURCE.UserIdentifierTypeID_2
			--AND TARGET.IDNumber = SOURCE.UserIdentifier_2
		WHEN NOT MATCHED THEN
			INSERT
			(
				UserID,
				UserIdentifierTypeID,
				IDNumber,
				EffectiveDate,
				IsActive,
				ModifiedBy,
				ModifiedOn,
				CreatedBy,
				CreatedOn,
				SystemCreatedOn,
				SystemModifiedOn
			)
			VALUES
			(
				SOURCE.UserID,
				SOURCE.UserIdentifierTypeID_2,
				SOURCE.UserIdentifier_2,
				SOURCE.EffectiveFromDate,
				SOURCE.IsActive,
				SOURCE.ModifiedBy,
				SOURCE.ModifiedOn,
				SOURCE.CreatedBy,
				SOURCE.CreatedOn,
				SOURCE.SystemCreatedOn,
				SOURCE.SystemModifiedOn
			)
			WHEN MATCHED THEN
			UPDATE SET TARGET.IDNumber = SOURCE.UserIdentifier_2;
	END TRY
	BEGIN CATCH
			SELECT  @ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO


