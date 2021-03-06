 -----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_MergeADUserSyncUserPhones]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Merge the User Phone details from Users for AD Services. Used in ADUserSync SSIS package.
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
CREATE PROCEDURE [Synch].[usp_MergeADUserSyncUserPhones]
	@ResultCode INT OUTPUT, -- Missing from before
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	BEGIN TRY 
			DECLARE @i int  
			--Get the latest Identity value for Core.Phone' and update the PhoneID in Synch.ADUserStage
			SELECT @i = ISNULL(IDENT_CURRENT('Core.Phone'),0) + 100 FROM Synch.ADUserStage

			CREATE TABLE #PhoneStage ( PhoneID bigint NULL,
									   Phone nvarchar(200) NULL,
									   UserGUID nvarchar(500) ) 
			INSERT INTO #PhoneStage
			( Phone ) 
			SELECT DISTINCT Phone  FROM Synch.ADUserStage ad LEFT JOIN Core.Phone p 
			ON (	ISNULL(p.Number,'') = ISNULL(ad.Phone,''))
			WHERE	Phone IS NOT NULL AND Phone <> '' 
			UPDATE #PhoneStage
			SET PhoneID = @i, 
				@i = @i+1
			WHERE Phone IS NOT NULL AND Phone <> '' 
			--select * from #PhoneStage
			
			--MERGE Core.Phone with the latest PhoneNumbers
			SET IDENTITY_INSERT Core.Phone ON
			MERGE Core.Phone AS target
			USING (	SELECT PhoneID, Phone, '2' as PhoneTypeID, NULL as Extension ,'1' as IsActive, '1' AS ModifiedBy, GETDATE() AS ModifiedOn, '1' AS CreatedBy, GETDATE() AS CreatedOn, GETDATE() AS SystemCreatedOn, GETDATE() AS SystemModifiedOn 
					FROM #PhoneStage
					WHERE Phone is not NULL AND Phone <> '' ) AS SOURCE 
			ON (	ISNULL(target.Number,'') = ISNULL(Source.Phone,''))
			WHEN MATCHED THEN 
				UPDATE SET	
					Number = Source.Phone  
			WHEN NOT MATCHED THEN
				INSERT (PhoneID, PhoneTypeID, Number, Extension, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn)
				VALUES (Source.PhoneID,'2',Source.Phone, NULL , Source.IsActive, Source.ModifiedBy, Source.ModifiedOn, Source.CreatedBy, Source.CreatedOn, Source.SystemCreatedOn, Source.SystemModifiedOn);
			SET IDENTITY_INSERT Core.Phone OFF
			
			UPDATE ad SET PhoneID = d.PhoneID
			FROM Synch.ADUserStage ad 
			INNER JOIN Core.Phone d
			ON ad.Phone = d.Number
			
			--MERGE Core.UserPhone with the latest PhoneNumbers from Staging and Core.Phone
			MERGE Core.UserPhone AS target
			USING (	SELECT u.UserID as UserID, us.PhoneID as PhoneID, '5' as PhonePermissionID, '1' as IsPrimary, 
						   u.IsActive, 1 as ModifiedBy, GETDATE() AS ModifiedOn, 1 as CreatedBy, GETDATE() AS CreatedOn, GETDATE() AS SystemCreatedOn, GETDATE() AS SystemModifiedOn  
					FROM Core.Users u 
					INNER JOIN Synch.ADUserStage  us
					ON u.UserGUID = us.UserGUID
 					WHERE us.PhoneID is not NULL AND us.PhoneID <> '' ) AS SOURCE 
			ON (	ISNULL(target.UserID,0) = ISNULL(Source.UserID,0))
			WHEN MATCHED THEN 
				UPDATE SET	
						PhoneID = source.PhoneID,
						PhonePermissionID = source.PhonePermissionID,
						IsPrimary = source.IsPrimary
			WHEN NOT MATCHED THEN
				INSERT (UserID, PhoneID, PhonePermissionID, IsPrimary, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn)
				VALUES ( Source.UserID, Source.PhoneID, Source.PhonePermissionID, Source.IsPrimary, Source.IsActive, Source.ModifiedBy, Source.ModifiedOn, Source.CreatedBy, Source.CreatedOn, Source.SystemCreatedOn, Source.SystemModifiedOn);	
			
			IF  OBJECT_ID(N'#PhoneStage') IS NOT NULL DROP TABLE #PhoneStage
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO