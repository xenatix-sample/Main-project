-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_MergeADUserSyncUserAddress]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Merge the User Address details from Users for AD Services. Used in ADUserSync SSIS package.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--03/01/2016	Sumana Sangapu	Initial creation.
--06/12/2016	Sumana Sangapu	Changed to Synch schema to be consistent and renamed dbo.UserStage to  Synch.ADUserStage
--06/29/2016	Sumana Sangapu	Changed the logic to handle emailid foreignkey conflict
--09/06/2016	Rahul Vats		Moved the Proc from Core to Synch
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_MergeADUserSyncUserAddress]
	@ResultCode INT OUTPUT, -- Missing from before
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully';
	BEGIN TRY
		DECLARE @i int
		SELECT @i = ISNULL(IDENT_CURRENT('Core.Addresses'),0) + 100 FROM Synch.ADUserStage
		-- Create A Temp Table to Act as a placeholder to determine the values for merge.
		CREATE TABLE #AddressStage ( AddressID bigint NULL,
							AddressLine1 nvarchar(255) NULL,
							AddressLine2 nvarchar(255) NULL,
							City nvarchar(255) NULL,
							StateProvince nvarchar(255) NULL,
							ZipPostalCode nvarchar(200) NULL,
							CountryRegion nvarchar(255) NULL,
							StateProvinceID int NULL,
							CountryID int NULL,
							UserGUID nvarchar(500) ) 
		
		-- UPDATE Stateids
		update u
		SET StateProvinceID = s.StateProvinceID
		--Select u.UserStageID, u.BatchID, u.UserName, u.StateProvince,s.StateProvinceCode
		FROM Synch.ADUserStage u
		LEFT JOIN [Reference].StateProvince s
		ON u.StateProvince = s.StateProvinceCode
		
		-- UPDATE CountryIDs
		update u
		SET CountryID = c.CountryID
		--Select u.UserStageID, u.BatchID, u.UserName, u.CountryRegion,c.CountryName
		FROM Synch.ADUserStage u
		LEFT JOIN [Reference].Country c
		ON u.CountryRegion = c.CountryName
		
		INSERT INTO #AddressStage (AddressID,AddressLine1, AddressLine2, City, StateProvince, ZipPostalCode, CountryRegion, StateProvinceID, CountryID )
		--Create Address with Null IDs
		SELECT DISTINCT a.AddressID, AddressLine1, AddressLine2, ad.City, ad.StateProvince, ZipPostalCode, CountryRegion, StateProvinceID, CountryID
		FROM	Synch.ADUserStage ad LEFT JOIN Core.Addresses a
		ON		ISNULL(ad.AddressLine1, '') = ISNULL(a.Line1, '')
				AND ISNULL(ad.AddressLine2, '') = ISNULL(a.Line2, '')
				AND ISNULL(ad.City, '') = ISNULL(a.City, '')
				AND ISNULL(ad.StateProvinceID, 0) = ISNULL(a.StateProvince, 0)
				AND ISNULL(ad.ZipPostalCode, '') = ISNULL(a.Zip, '')
				AND ISNULL(ad.CountryID, '') = ISNULL(a.Country, '')
		WHERE	AddressLine1 is not NULL AND AddressLine1 <> ''
		OR		AddressLine2 is not NULL AND AddressLine2 <> ''
		OR		ad.City is not NULL AND ad.City <> '' 
		OR		ad.StateProvince is not NULL AND ad.StateProvince <> '' 
		OR		ad.ZipPostalCode is not NULL AND ad.ZipPostalCode <> '' 
		OR		ad.CountryID is not NULL AND a.Country <> '' 
		
		--Update the Address IDs to maintain the necessary gap for AD Users
		UPDATE #AddressStage
		SET AddressID = @i, 
			@i = @i+1
		WHERE AddressID IS NULL
		
		--MERGE Core.Addresses with the latest AddressesNumbers
		--Set Identity_Insert ON
		SET IDENTITY_INSERT Core.Addresses ON
		--Define Target as Core.Addresses and 
		MERGE Core.Addresses AS TARGET
		USING (	SELECT	AddressID, '3' as AddressTypeID, AddressLine1, AddressLine2, City, StateProvince, ZipPostalCode, CountryRegion, 
						'1' as IsActive, '1' AS ModifiedBy, GETDATE() AS ModifiedOn, '1' AS CreatedBy, GETDATE() AS CreatedOn, GETDATE() AS SystemCreatedOn, GETDATE() AS SystemModifiedOn ,
						 StateProvinceID, CountryID
				FROM	#AddressStage
				WHERE	AddressLine1 is not NULL AND AddressLine1 <> ''
				OR		AddressLine2 is not NULL AND AddressLine2 <> ''
				OR		City is not NULL AND City <> '' 
				OR		StateProvince is not NULL AND StateProvince <> '' 
				OR		ZipPostalCode is not NULL AND ZipPostalCode <> ''
				OR		CountryID is not NULL AND Countryid <> ''  ) AS SOURCE 
		ON	ISNULL(SOURCE.AddressTypeID, 0) = ISNULL(TARGET.AddressTypeID, 0)
			AND	ISNULL(SOURCE.AddressLine1, '') = ISNULL(TARGET.Line1, '')
			AND ISNULL(SOURCE.AddressLine2, '') = ISNULL(TARGET.Line2, '')
			AND ISNULL(SOURCE.City, '') = ISNULL(TARGET.City, '')
			AND ISNULL(SOURCE.StateProvinceID, 0) = ISNULL(TARGET.StateProvince, 0)
			AND ISNULL(SOURCE.ZipPostalCode, '') = ISNULL(TARGET.Zip, '')
			AND ISNULL(SOURCE.AddressID,'') = ISNULL(TARGET.AddressID,'')	
			AND ISNULL(SOURCE.CountryID, '') = ISNULL(TARGET.Country, '')
		WHEN MATCHED THEN 
			UPDATE SET	
			AddressTypeID = SOURCE.AddressTypeID,
			Line1 = SOURCE.AddressLine1,
			Line2 = SOURCE.AddressLine2,
			City = SOURCE.City,
			StateProvince = SOURCE.StateProvinceID,
			Country = SOURCE.CountryID,
			Zip = SOURCE.ZipPostalCode				
		WHEN NOT MATCHED THEN
			INSERT (AddressID, AddressTypeID, Line1, Line2, City, Country, StateProvince, 
			Zip,  IsVerified, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn)
			VALUES (Source.AddressID, Source.AddressTypeID, Source.AddressLine1, Source.AddressLine2, Source.City,Source.CountryID, Source.StateProvinceID, 
			Source.ZipPostalCode,'1',Source.IsActive, Source.ModifiedBy, Source.ModifiedOn, Source.CreatedBy, Source.CreatedOn, Source.SystemCreatedOn, Source.SystemModifiedOn);
		--Turn the Identity Insert On
		SET IDENTITY_INSERT Core.Addresses OFF
		--Update the Address ID in the Synch.ADUserStage
		UPDATE ad SET AddressID = d.AddressID
		FROM Synch.ADUserStage ad  
		INNER JOIN Core.Addresses d
		ON ISNULL(ad.AddressLine1, '') = ISNULL(d.Line1, '')
				AND ISNULL(ad.AddressLine2, '') = ISNULL(d.Line2, '')
				AND ISNULL(ad.City, '') = ISNULL(d.City, '')
				AND ISNULL(ad.StateProvinceID, 0) = ISNULL(d.StateProvince, 0)
				AND ISNULL(ad.ZipPostalCode, '') = ISNULL(d.Zip, '')
				AND ISNULL(ad.CountryID, '') = ISNULL(d.Country, '')
		--MERGE Core.UserAddresses with the latest AddressesNumbers from Staging and Core.Addresses
		MERGE Core.UserAddress AS target
		USING (	SELECT u.UserID as UserID, us.AddressID as AddressID, '1' as MailPermissionID, '1' as IsPrimary, 
					   u.IsActive, 1 as ModifiedBy, GETDATE() AS ModifiedOn, 1 as CreatedBy, GETDATE() AS CreatedOn, GETDATE() AS SystemCreatedOn, GETDATE() AS SystemModifiedOn 
				FROM Core.Users u 
				INNER JOIN [Synch].[ADUserStage]  us
				ON u.UserGUID = us.UserGUID WHERE us.AddressID is not NULL AND us.AddressID <> '' 
 			) AS SOURCE 
		ON (	ISNULL(target.UserID,0) = ISNULL(Source.UserID,0))
		WHEN MATCHED THEN 
			UPDATE SET	
					AddressID = source.AddressID,
					MailPermissionID = source.MailPermissionID,
					IsPrimary = source.IsPrimary
		WHEN NOT MATCHED THEN
			INSERT (UserID, AddressID, MailPermissionID, IsPrimary, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn)
			VALUES ( Source.UserID, Source.AddressID, Source.MailPermissionID, Source.IsPrimary, Source.IsActive, Source.ModifiedBy, Source.ModifiedOn, Source.CreatedBy, Source.CreatedOn, Source.SystemCreatedOn, Source.SystemModifiedOn);
		IF  OBJECT_ID(N'#AddressStage') IS NOT NULL DROP TABLE #AddressStage
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO