-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_MergeADUserSyncUserEmail]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Merge the User Email details from Users for AD Services. Used in ADUserSync SSIS package.
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
CREATE PROCEDURE [Synch].[usp_MergeADUserSyncUserEmail]
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
		-------------------------------------------------------------------------------------------------------------------------
		DECLARE @i int  
		--Get the latest Identity value for Core.Email' and update the EmailID in Synch.ADUserStage
		SELECT @i = ISNULL(IDENT_CURRENT('Core.Email'),0) + 100 FROM Synch.ADUserStage

		CREATE TABLE #EmailStage (
									EmailID bigint NULL,
									Email nvarchar(255) NULL,
									UserGUID nvarchar(500) )  
		INSERT INTO #EmailStage
		( Email ) 
		SELECT DISTINCT ad.Email FROM Synch.ADUserStage ad LEFT JOIN Core.Email e ON ad.Email = e.Email  WHERE ad.Email IS NOT NULL AND ad.Email <> '' 
		UPDATE #EmailStage
		SET EmailID = @i, 
			@i = @i+1
		WHERE EmailID IS NULL
		-------------------------------------------------------------------------------------------------------------------------
		--MERGE Core.Email with the latest EmailNumbers
		SET IDENTITY_INSERT Core.Email ON

		MERGE Core.Email AS target
		USING (	SELECT EmailID, Email, '2' as EmailTypeID, '1' as IsActive, '1' AS ModifiedBy, GETDATE() AS ModifiedOn, '1' AS CreatedBy, GETDATE() AS CreatedOn, GETDATE() AS SystemCreatedOn, GETDATE() AS SystemModifiedOn 
				FROM #EmailStage
				WHERE Email is not NULL AND Email <> '' ) AS SOURCE 
		ON (	ISNULL(target.Email,'') = ISNULL(Source.Email,'') )
		WHEN MATCHED THEN 
			UPDATE SET Email = SOURCE.Email
 		WHEN NOT MATCHED THEN
			INSERT (EmailID, Email,IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn)
			VALUES (Source.EmailID, Source.Email, Source.IsActive, Source.ModifiedBy, Source.ModifiedOn, Source.CreatedBy, Source.CreatedOn, Source.SystemCreatedOn, Source.SystemModifiedOn);
		SET IDENTITY_INSERT Core.Email OFF
		UPDATE ad SET EmailID = d.EmailID
		FROM Synch.ADUserStage ad 
		INNER JOIN Core.Email d
		ON ad.Email = d.Email
		-------------------------------------------------------------------------------------------------------------------------
		--MERGE Core.UserEmail with the latest EmailNumbers from Staging and Core.Email
		MERGE Core.UserEmail AS target
		USING (	SELECT u.UserID as UserID, us.EmailID as EmailID, '1' as EmailPermissionID, '1' as IsPrimary, 
					   u.IsActive, 1 as ModifiedBy, GETDATE() AS ModifiedOn, 1 as CreatedBy, GETDATE() AS CreatedOn, GETDATE() AS SystemCreatedOn, GETDATE() AS SystemModifiedOn 
				FROM Core.Users u 
				INNER JOIN Synch.ADUserStage  us
				ON u.UserGUID = us.UserGUID
 				WHERE us.EmailID is not NULL AND us.EmailID <> '' ) AS SOURCE 
		ON (	ISNULL(target.UserID,0) = ISNULL(Source.UserID,0))
		WHEN MATCHED THEN 
			UPDATE SET	
					EmailID = source.EmailID,
					EmailPermissionID = source.EmailPermissionID,
					IsPrimary = source.IsPrimary
		WHEN NOT MATCHED THEN
			INSERT (UserID, EmailID, EmailPermissionID, IsPrimary, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn)
			VALUES ( Source.UserID, Source.EmailID, Source.EmailPermissionID, Source.IsPrimary, Source.IsActive, Source.ModifiedBy, Source.ModifiedOn, Source.CreatedBy, Source.CreatedOn, Source.SystemCreatedOn, Source.SystemModifiedOn);	
		IF  OBJECT_ID(N'#EmailStage') IS NOT NULL DROP TABLE #EmailStage
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO